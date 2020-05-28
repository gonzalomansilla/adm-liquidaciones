using Curso.Model.Context;
using Curso2020.Common.DTO;
using Curso2020.ImportingEmployeeFiles;
using Curso2020.ImportingEmployeeFiles.ExceptionClass;
using Curso2020.Model.Model;
using Curso2020.Service.CargaMasiva.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curso2020.Service.CargaMasiva
{
	public class CargaMasivaServiceAsync : ICargaMasivaServiceAsync
	{
		private readonly ILogger<CargaMasivaServiceAsync> _logger;
		private readonly CursoContext _context;

		public CargaMasivaServiceAsync(ILogger<CargaMasivaServiceAsync> logger, CursoContext context)
		{
			_context = context;
			_logger = logger;
			_logger.LogInformation("Contructor service CargaMasiva");
		}

		public async Task<List<EmpleadoDTO>> ImportarArchivoEmpleados(IFormFile file)
		{
			//Existencia de archivo
			ArchivoEmpleados archivoDB = await _context.ArchivosEmpleados
				.Where(a => a.archivo == file.FileName).FirstOrDefaultAsync();
			if (archivoDB != null)
				throw HttpResponseException.BadRequest("El archivo a importar ya fue procesado o tiene el mismo nombre");

			List<Empleado> empleados = new List<Empleado>();

			bool formatoDeArchivoEsCorrecto = await DeserializadorArchivoEmpleados.FormatoDeDatosDeArchivoEsCorrecto(file.OpenReadStream());
			if (formatoDeArchivoEsCorrecto)
			{
				empleados = await DeserializadorArchivoEmpleados.DeserializarArchivo(file.OpenReadStream(), file.FileName);

				empleados = await EliminarEmpleadosExistentesEnDB(empleados);
				if (empleados.Count == 0)
					throw HttpResponseException.BadRequest("Los empleados del archivo ya estan registrados. Pertenecen a otro de nombre diferente");

				ArchivoEmpleados archivoDb = await SubirArchivo(file);
				if (archivoDb != null)
				{
					_context.Empleados.AddRange(empleados);
					int filasAfectadasDeEmpleados = await _context.SaveChangesAsync();

					if (filasAfectadasDeEmpleados == 0)
						throw HttpResponseException.InternalServer("Error al insertar los Empleados del archivo");
				}
				else
				{
					throw HttpResponseException.InternalServer("Error al insertar el archivo");
				}
			}

			List<EmpleadoDTO> resul = CrearListaEmpleadosDTO(empleados);

			return resul;
		}

		private async Task<List<Empleado>> EliminarEmpleadosExistentesEnDB(List<Empleado> empleados)
		{
			List<Empleado> resul = new List<Empleado>();
			foreach (Empleado empleado in empleados)
			{
				bool existeEmpleado = await _context.Empleados.ContainsAsync(empleado);

				if (!existeEmpleado) resul.Add(empleado);
			}

			return resul;
		}

		private List<EmpleadoDTO> CrearListaEmpleadosDTO(List<Empleado> empleados)
		{
			List<EmpleadoDTO> resul = new List<EmpleadoDTO>();
			foreach (Empleado e in empleados)
			{
				resul.Add(new EmpleadoDTO()
				{
					Dni = e.dni,
					Nombre = e.nombre,
					Apellido = e.apellido,
					IdPuesto = e.idPuesto,
					HorasTrabajadasPorMes = e.horasTrabajadasUltimoMes,
					CuitEmpresa = e.cuitEmpresa,
					Archivo = e.archivo
				});
			}
			return resul;
		}

		private async Task<ArchivoEmpleados> SubirArchivo(IFormFile file)
		{
			_context.ArchivosEmpleados.Add(new ArchivoEmpleados()
			{
				fechaDeProcesado = DateTime.Now,
				archivo = file.FileName
			});

			int filasAfectadas = await _context.SaveChangesAsync();

			if (filasAfectadas != 0) return await _context.ArchivosEmpleados
					.Where(a => a.archivo == file.FileName)
					.FirstOrDefaultAsync();
			else
				return null;
		}
	}
}

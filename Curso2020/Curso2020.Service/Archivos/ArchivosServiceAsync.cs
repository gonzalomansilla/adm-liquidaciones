using Curso.Model.Context;
using Curso2020.Common.DTO;
using Curso2020.ImportingEmployeeFiles.ExceptionClass;
using Curso2020.Model.Model;
using Curso2020.Service.Archivos.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curso2020.Service.Archivos
{
	public class ArchivosServiceAsync : IArchivosServiceAsync
	{
		private readonly ILogger<ArchivosServiceAsync> _logger;
		private readonly CursoContext _context;

		public ArchivosServiceAsync(ILogger<ArchivosServiceAsync> looger, CursoContext context)
		{
			_logger = looger;
			_context = context;

			_logger.LogInformation("Constructor ArchivosService");
		}

		public async Task<List<EmpleadoDTO>> EmpleadosDelArchivo(string nombreArchivo)
		{
			var archivoDb = await _context.ArchivosEmpleados
				.Where(a => a.archivo == nombreArchivo).FirstOrDefaultAsync();

			if (archivoDb == null)
				throw HttpResponseException.Conflict("El archivo no existe en la base de datos");

			List<Empleado> empleadosDB = await _context.Empleados
				.Where(e => e.archivo == nombreArchivo).ToListAsync();

			List<EmpleadoDTO> empleados = new List<EmpleadoDTO>();
			foreach (Empleado e in empleadosDB)
			{
				empleados.Add(new EmpleadoDTO()
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

			return empleados;
		}

		public async Task<List<ArchivoEmpleadosDTO>> HistorialDeArchivosProcesados()
		{
			List<ArchivoEmpleados> historialArchivos = await _context.ArchivosEmpleados.ToListAsync();

			List<ArchivoEmpleadosDTO> resul = new List<ArchivoEmpleadosDTO>();
			foreach (ArchivoEmpleados archivo in historialArchivos)
			{
				resul.Add(new ArchivoEmpleadosDTO()
				{
					Nombre = archivo.archivo,
					FechaDeProcesado = archivo.fechaDeProcesado
				});
			}

			return resul;
		}
	}
}

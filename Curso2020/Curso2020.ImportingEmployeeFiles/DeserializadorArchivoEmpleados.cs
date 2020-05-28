using Curso2020.ImportingEmployeeFiles.ExceptionClass;
using Curso2020.Model.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Curso2020.ImportingEmployeeFiles
{
	public class DeserializadorArchivoEmpleados
	{
		// Verificador de formato
		public static async Task<bool> FormatoDeDatosDeArchivoEsCorrecto(Stream stream)
		{
			using (var reader = new StreamReader(stream))
			{
				while (reader.EndOfStream == false)
				{
					string lineaTexto = await reader.ReadLineAsync();
					lineaTexto = lineaTexto.Trim();

					if (lineaTexto == "") throw HttpResponseException
							.BadRequest("El archivo no cumple con el formato requerido. Hay lineas vacias");

					bool formatoCorrectoDeEmpleado = CamposDeEmpleadoCorrecto(lineaTexto);

					if (!formatoCorrectoDeEmpleado) return false;
				}
			}

			return true;
		}

		private static bool CamposDeEmpleadoCorrecto(string texto)
		{
			List<string> camposEmpleadoFormateados = texto
				.Split("|").ToList()
				.Select(campo => campo.Trim()).ToList()
				.Where(campo => !campo.Equals("")).ToList();

			Empleado e;
			string[] arrayCamposEmpleado = camposEmpleadoFormateados.ToArray();
			try
			{
				e = CrearEmpleadoDesdeArray(arrayCamposEmpleado, null);
			}
			catch (IndexOutOfRangeException)
			{
				throw HttpResponseException
					.BadRequest("El archivo no contiene todos los datos requeridos de un Empleado");
			}

			return true;
		}

		// Deserializacion
		public async static Task<List<Empleado>> DeserializarArchivo(Stream stream, string nombreArchivo)
		{
			List<Empleado> empleados = new List<Empleado>();

			using (var reader = new StreamReader(stream))
			{
				while (reader.EndOfStream == false)
				{
					string lineaTexto = await reader.ReadLineAsync();
					lineaTexto = lineaTexto.Trim();

					if (lineaTexto == "") throw HttpResponseException
							.BadRequest("El archivo no cumple con el formato requerido. Hay lineas vacias");

					Empleado e = CrearEmpleadoDeLineaDeTexto(lineaTexto, nombreArchivo);
					empleados.Add(e);
				}
			}

			return empleados;
		}

		private static Empleado CrearEmpleadoDeLineaDeTexto(string texto, string nombreArchivo)
		{
			List<string> camposEmpleadoFormateados = texto
				.Split("|").ToList()
				.Select(campo => campo.Trim()).ToList()
				.Where(campo => !campo.Equals("")).ToList();

			string[] arrayCamposEmpleado = camposEmpleadoFormateados.ToArray();

			Empleado e = CrearEmpleadoDesdeArray(arrayCamposEmpleado, nombreArchivo);

			return e;
		}

		private static Empleado CrearEmpleadoDesdeArray(string[] arrayCamposEmpleado, string nombreArchivo)
		{
			Empleado empleado;
			try
			{
				empleado = new Empleado()
				{
					nombre = arrayCamposEmpleado[0],
					apellido = arrayCamposEmpleado[1],
					dni = arrayCamposEmpleado[2],
					idPuesto = int.Parse(arrayCamposEmpleado[3]),
					horasTrabajadasUltimoMes = int.Parse(arrayCamposEmpleado[4]),
					cuitEmpresa = arrayCamposEmpleado[5],
				};

				if (nombreArchivo != null)
					empleado.archivo = nombreArchivo;
			}
			catch (IndexOutOfRangeException)
			{
				throw HttpResponseException.BadRequest("Los campos del archivo no corresponden a los definidos en la API");
			}
			catch (Exception e)
			{
				throw HttpResponseException.InternalServer(e.Message);
			}

			return empleado;
		}
	}
}

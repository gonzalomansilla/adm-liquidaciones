using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Curso2020.Common.DTO;
using Curso2020.ImportingEmployeeFiles.ExceptionClass;
using Curso2020.Service.Archivos.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Curso2020.Management.Api.Controllers.CargaMasiva
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArchivosController : ControllerBase
	{
		private readonly ILogger<ArchivosController> _logger;
		private readonly IArchivosServiceAsync _serviceArchivo;

		public ArchivosController(ILogger<ArchivosController> logger, IArchivosServiceAsync archivosService)
		{
			_logger = logger;
			_serviceArchivo = archivosService;

			_logger.LogInformation("Constructor ArchivosController");
		}

		[HttpGet("EmpleadosDelArchivo")]
		public async Task<ActionResult> EmpleadosDelArchivo(string nombreArchivo)
		{
			if (nombreArchivo == "" || nombreArchivo == null)
				return BadRequest("No se proporciono un nombre de archivo");

			bool esArchivoTxt = nombreArchivo.EndsWith(".txt");
			if (!esArchivoTxt)
				nombreArchivo += ".txt";

			List<EmpleadoDTO> empleadosDB = new List<EmpleadoDTO>();
			try
			{
				empleadosDB = await _serviceArchivo.EmpleadosDelArchivo(nombreArchivo);
			}
			catch (HttpResponseException http)
			{
				if (http.Status == 409) return Conflict(http.Value);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}

			return Ok(empleadosDB);
		}

		[HttpGet("Historial")]
		public async Task<ActionResult> Historial()
		{
			List<ArchivoEmpleadosDTO> historial = await _serviceArchivo.HistorialDeArchivosProcesados();

			return Ok(historial);
		}
	}
}
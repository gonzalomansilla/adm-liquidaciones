using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Curso2020.Common.DTO;
using Curso2020.ImportingEmployeeFiles.ExceptionClass;
using Curso2020.Model.Model;
using Curso2020.Service.CargaMasiva.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Curso2020.Management.Api.Controllers.CargaMasiva
{
	[Route("api/[controller]")]
	[ApiController]
	[EnableCors("_CORS_")]
	public class CargaMasivaDeDatosController : ControllerBase
	{
		readonly ILogger<CargaMasivaDeDatosController> _logger;
		readonly ICargaMasivaServiceAsync _serviceCargaMasiva;

		public CargaMasivaDeDatosController(ILogger<CargaMasivaDeDatosController> logger, ICargaMasivaServiceAsync serviceCargaMasiva)
		{
			_serviceCargaMasiva = serviceCargaMasiva;
			_logger = logger;
			_logger.LogInformation("Constructor CargaMasivaDeDatos");
		}

		[HttpPost("ImportarArchivoEmpleados")]
		public async Task<ActionResult> ImportarArchivoEmpleados(IFormFile file)
		{
			if (file == null)
				return BadRequest(new ResultJson() { Message = "No se proporciono archivo para importar" });

			List<EmpleadoDTO> empleados;
			try
			{
				empleados = await _serviceCargaMasiva.ImportarArchivoEmpleados(file);
			}
			catch (HttpResponseException e)
			{
				int statusCode = e.Status;

				return statusCode switch
				{
					400 => BadRequest(e.Value),
					_ => StatusCode(500, $"Error en el servicio de Carga masiva\n{e.Value}")
				};
			}
			catch (Exception e2)
			{
				return NotFound(e2.Message);
			}

			return Ok(empleados);
		}
	}
}
using System;
using Curso2020.Common.DTO;
using Curso2020.Model.Model;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Curso2020.Service.ABMEmpleado.Interfaces;

namespace Curso2020.Management.Api.Controllers
{
	[Route("api/EmpleadoController")]
	[ApiController]
	[EnableCors("_CORS_")]
	public class EmpleadoController : ControllerBase
	{
		private readonly ILogger<EmpleadoController> _logger;
		private readonly IEmployeeABMAsync _employeeABM;
		public EmpleadoController(ILogger<EmpleadoController> logger, IEmployeeABMAsync employeeABM)
		{
			this._logger = logger;
			_employeeABM = employeeABM;
			_logger.LogInformation("Constructor EmpleadoController");
		}

		[HttpPost("AddEmployee")]
		public async Task<ActionResult> CreateEmployee([FromBody] EmpleadoDTO empleado)
		{
			if (empleado.Nombre == null || empleado.Nombre.Length < 4 ||
				empleado.Apellido == null || empleado.Apellido.Length < 4 ||
				Convert.ToInt32(empleado.Dni) < 6000000)
			{
				return BadRequest(new ResultJson() { Message = "Verifique los datos ingresados." });
			}

			var dbEmployee = await _employeeABM.AddEmployee(empleado);

			if (dbEmployee.Equals("Creado"))
			{
				return Created("", new ResultJson() { Message = "El empleado se dio de alta exitosamente." });
			}
			else if (dbEmployee.Equals("EmpleadoYaExiste"))
			{
				return BadRequest(new ResultJson() { Message = "Empleado ya existente." });
			}
			else if (dbEmployee.Equals("PuestoNoEncontrado"))
			{
				return NotFound(new ResultJson() { Message = "Puesto no encontrado en la empresa." });
			}
			else
			{
				return BadRequest(new ResultJson() { Message = "Ha ocurrido un error en el alta del empleado." });
			}
		}

		[HttpPost("RemoveEmployee")]
		public async Task<ActionResult> RemoveEmployee([FromBody] EmpleadoDTO empleadoDTO)
		{

			if (Convert.ToInt32(empleadoDTO.Dni) < 10000000)
			{
				return BadRequest(new ResultJson() { Message = "Verifique el dato ingresado." });
			}
			var dbEmployee = await _employeeABM.DeleteEmployee(empleadoDTO);

			if (dbEmployee == true)
			{
				return Ok(new ResultJson() { Message = "El empleado se ha dado de baja exitosamente" });
			}
			else
			{
				return NotFound(new ResultJson() { Message = "El empleado no existe en la empresa" });
			}

		}

		[HttpPost("ModifyEmployee")]
		public async Task<ActionResult> ModifyEmployee([FromBody] EmpleadoDTO empleadoDTO)
		{

			if (Convert.ToInt32(empleadoDTO.Dni) < 10000000)
			{
				return BadRequest(new ResultJson() { Message = "Verifique los datos ingresados." });
			}
			else if (empleadoDTO.HorasTrabajadasPorMes < 80)
			{
				return BadRequest(new ResultJson() { Message = "El minimo de horas trabajadas deben de ser 80hs" });
			}

			var dbEmployee = await _employeeABM.ModifyEmployee(empleadoDTO);

			if (dbEmployee == 1)
			{
				return Created("", new ResultJson() { Message = "El empleado se ha modificado exitosamente" });
			}

			else if (dbEmployee == 2)
			{
				return NotFound(new ResultJson() { Message = "El empleado no existe en la empresa" });
			}

			else if (dbEmployee == 3)
			{
				return NotFound(new ResultJson() { Message = "El puesto no existe en la empresa" });
			}

			else if (dbEmployee == 4)
			{
				return BadRequest(new ResultJson() { Message = "Error al modificar el empleado, no se ha hecho ninguna modificacion" });
			}
			else
			{
				return BadRequest(new ResultJson() { Message = "Ha ocurrido un error en la modificacion" });
			}

		}

		[HttpGet("GetGrilla/{cuitEmpresa}")]
		public async Task<ActionResult> GetGrilla(string cuitEmpresa)
		{
			if (cuitEmpresa.Length < 10 || cuitEmpresa.Length > 11 || (!long.TryParse(cuitEmpresa, out long soloParaValidar)))
			{
				return BadRequest(new ResultJson() { Message = "ingrese cuit valido" });

			}
			var retorno = await this._employeeABM.ObtenerGrilla(cuitEmpresa);
			if (retorno == null)
			{
				return NotFound(new ResultJson() { Message = "Aún no posee empleados" });
			}
			return Ok(retorno);

		}

		[HttpGet("CheckPositions/{cuitEmpresa}")]
		public async Task<List<Puesto>> ConsultarPuestos(string cuitEmpresa)
		{
			return await _employeeABM.CheckJobPositions(cuitEmpresa);
		}

		[HttpGet("GetByDni/{dni}")]
		public async Task<ActionResult> GetByDni([FromRoute]string dni)
		{
			var empleado = await _employeeABM.GetByDni(dni);

			if (dni.Length != 8)
			{
				return BadRequest(new ResultJson() { Message = "Verifique que el dni tenga 8 digitos" });
			}

			if (empleado == null)
			{
				return NotFound(new ResultJson() { Message = "No se encuentra ningun empleado con el Dni: " });
			}
			else
				return Ok(empleado);
		}
	}
}
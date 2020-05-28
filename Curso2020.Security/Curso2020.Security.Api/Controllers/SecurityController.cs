using System.Linq;
using System.Threading.Tasks;
using Curso2020.Security.Common.DTO;
using Curso2020.Security.Model.Model;
using Curso2020.Security.Service.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoginYSeguridad.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[EnableCors("_TP_")]
	public class SecurityController : ControllerBase
    {
		private readonly ILogger<SecurityController> _logger;
		private readonly ISecurity _loginService;

		public SecurityController(ILogger<SecurityController> logger, ISecurity loginService)
		{
			_logger = logger;
			_loginService = loginService;
			_logger.LogInformation("Constructor SecurityController");
		}

		[HttpPost("Login")]
		public async Task<ActionResult> Login([FromBody] UsuarioDTO usuario)
		{
			if (usuario.nombreUsuario == null || usuario.nombreUsuario.Length <= 2 || usuario.contrasenia == null || usuario.contrasenia.Length <= 9)
				return BadRequest(new Json() { Message = "Verifique los datos enviados." });
			var dbUser = await _loginService.Login(usuario);
			if (dbUser == null)
				return Unauthorized(new Json() { Message = "Usuario y/o contraseña invalidos" });
			else return Ok(dbUser);
		}

		[HttpPut("UpdatePass")]
		public async Task<ActionResult> UpdatePass([FromBody] UsuarioDTO usuario)
		{
			if(usuario.nombreUsuario == null || usuario.nombreUsuario.Length <= 2 ||
				usuario.contrasenia == null || usuario.contrasenia.Length <= 9 ||
				usuario.nuevaContrasenia == null || usuario.nuevaContrasenia.Length <= 9)
				return BadRequest(new Json() { Message = "Verifique los datos enviados." });
			var dbUser = await _loginService.UpdatePass(usuario);
			if (dbUser == null)
				return Unauthorized(new Json() { Message = "Usuario y/o contraseña invalidos" });
			else
				return Ok(new Json() { Message = "Contraseña cambiada con exito!" });
		}

		[HttpPost("VerifyToken")]
		public async Task<ActionResult> VerifyToken(string token)
		{
			if (token == "" || token == null)
			{
				token = HttpContext.Request.Headers["Authorization"];
			}
			if (token == "" || token == null)
			{
				return BadRequest("Token invalido");
			}
			var dbUser = await _loginService.VerifyToken(token);

			if (dbUser == null)
			{
				return Unauthorized(new Json() { Message = "Por favor vuelva a ingresar a su cuenta" });
			}
			return Ok(dbUser);
		}

		[HttpPut("CreateUser")]
		public async Task<ActionResult> CreateUser([FromBody] Usuario usuario)
		{
			if (usuario.nombreUsuario == null || usuario.nombreUsuario == "" || usuario.nombreUsuario.Length <= 2 ||
				usuario.contrasenia == null || usuario.contrasenia == "" || usuario.contrasenia.Length <= 9 ||
				usuario.dniCuit == null || usuario.dniCuit == "" || !usuario.dniCuit.All(char.IsDigit) ||
				usuario.rol == null || usuario.rol == "" || usuario.rol != "Empresa" && usuario.rol != "Empleado" && usuario.rol != "Liquidador")
			{
				return BadRequest(new Json() { Message = "Por favor, complete los campos" });
			}

			if(usuario.rol == "Empresa" && usuario.dniCuit.Length < 10 || usuario.dniCuit.Length > 11)
			{
				return BadRequest(new Json() { Message = "El CUIT ingresado no corresponde con los valores requeridos" });
			}
			if (usuario.rol == "Empleado" || usuario.rol == "Liquidador")
			{
				if (usuario.dniCuit.Length < 7 || usuario.dniCuit.Length > 8)
				{
					return BadRequest(new Json() { Message = "El DNI ingresado no corresponde con los valores requeridos" });
				}
			}

			int result = await _loginService.CreateUser(usuario);
			if (result == 0)
			{
				return BadRequest(new Json() { Message = "Ese usuario ya existe en la base de datos" });
			}
			if (result == 2)
			{
				return BadRequest(new Json() { Message = "Ya existe un usuario con ese DNI en la base de datos" });
			}
			if (result == 3)
			{
				return BadRequest(new Json() { Message = "Ya existe un usuario con ese nombre en la base de datos" });
			}
			if (result == 4)
			{
				return BadRequest(new Json() { Message = "La empresa con ese CUIT aún no fue dada de alta" });
			}
			return Ok(new Json() { Message = "Usuario creado con exito!"});
		}
	}
}
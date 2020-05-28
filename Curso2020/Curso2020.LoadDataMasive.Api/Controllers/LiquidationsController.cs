using Curso2020.Common.DTO;
using Curso2020.Seguridad.Service.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using Curso2020.Model.Model;
using Curso2020.Security.Common.DTO;

namespace Curso2020.Liquidations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_CORS_")]
    public class LiquidationsController : ControllerBase
    {
        private readonly ILogger<LiquidationsController> _logger;
        private readonly ILiquidationsServices _liquidationServices;

        public LiquidationsController(ILogger<LiquidationsController> logger, ILiquidationsServices LiquidationServices)
        {
            _logger = logger;
            _liquidationServices = LiquidationServices;
            _logger.LogInformation("Constructor LiquidacionsController");
        }

        [HttpPost("GetLiquidationsByDni")]
        public async Task<ActionResult> GetLiquidationsByDni([FromBody] string dni)
        {
            string token = HttpContext.Request.Headers["Authorization"];

            if (token == null)
            {
                return Unauthorized(new ResultJson() { Message = "Por favor vuelva a ingresar a su cuenta" });
            }

            var result = _liquidationServices.LoginSecurity(token);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return Unauthorized(new ResultJson() { Message = "Por favor vuelva a ingresar a su cuenta" });
            }

            var dbUser = JsonConvert.DeserializeObject<UsuarioDTO>(result.Content);
            if (dbUser.rol == "Empresa")
            {
                if (dni.Length != 7 && dni.Length != 8)
                    return BadRequest(new ResultJson() { Message = "Verifique los datos enviados." });
                var dbLiquidaciones = await _liquidationServices.GetLiquidationsByDni(dni);
                if (dbLiquidaciones == null)
                    return BadRequest(new ResultJson() { Message = "No hay liquidaciones con ese DNI en la base de datos" });
                else
                    return Ok(dbLiquidaciones);
            }
            return Unauthorized(new ResultJson() { Message = "No tiene permitido realizar esa acción" });
        }

        [HttpPost("AuthorizeLiquidation")]
        public async Task<ActionResult> AuthorizeLiquidation([FromBody] string fecha)
        {
            string token = HttpContext.Request.Headers["Authorization"];

            if (token == null)
            {
                return Unauthorized(new ResultJson() { Message = "Por favor vuelva a ingresar a su cuenta" });
            }

            var result = _liquidationServices.LoginSecurity(token);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return Unauthorized(new ResultJson() { Message = "Por favor vuelva a ingresar a su cuenta" });
            }

            var dbUser = JsonConvert.DeserializeObject<UsuarioDTO>(result.Content);
            if (dbUser.rol == "Empresa")
            {

                if (dbUser.dniCuit == null || dbUser.dniCuit.Length != 11 && dbUser.dniCuit.Length != 10)
                    return BadRequest(new ResultJson() { Message = "El usuario no posee CUIT" });
                if (fecha == null)
                    return BadRequest(new ResultJson() { Message = "Verifique los datos enviados" });

                var dbAutorizacion = await _liquidationServices.AuthorizeLiquidations(fecha, dbUser.dniCuit);
                if (dbAutorizacion != null)
                    return Ok(new ResultJson() { Message = "Autorización procesada exitosamente" });
            }
            return Unauthorized(new ResultJson() { Message = "No tiene permitido realizar esa autorización o ya se realizó" });
        }

        [HttpPost("StartLiquidations")]
        public async Task<ActionResult> StartLiquidations([FromBody] Autorizacion autorizacion)
        {
            string token = HttpContext.Request.Headers["Authorization"];

            if (token == null)
            {
                return Unauthorized(new ResultJson() { Message = "Por favor vuelva a ingresar a su cuenta" });
            }

            var resultLogin = _liquidationServices.LoginSecurity(token);
            if (resultLogin.StatusCode != HttpStatusCode.OK)
            {
                return Unauthorized(new ResultJson() { Message = "Por favor vuelva a ingresar a su cuenta" });
            }

            var dbUser = JsonConvert.DeserializeObject<UsuarioDTO>(resultLogin.Content);
            if (dbUser.rol == "Liquidador")
            {
                if (autorizacion.cuitEmpresa.Length != 11 && autorizacion.cuitEmpresa.Length != 10 || autorizacion.fecha == null)
                    return BadRequest(new ResultJson() { Message = "Verifique los datos enviados" });

                var dbAutorizacion = await _liquidationServices.StartLiquidations(autorizacion);
                if (dbAutorizacion != null)
                {
                    var resultLiqudations = await _liquidationServices.Liquidation(autorizacion);
                    if (resultLiqudations == true)
                        return Ok(new ResultJson() { Message = "Liquidación procesada exitosamente" });
                }
            }
            return Unauthorized(new ResultJson() { Message = "No tiene permitido realizar esa autorización o ya se realizó" });
        }

        [HttpGet("GetAuthorizations")]
        public async Task<ActionResult> GetAuthorizations()
        {
            string token = HttpContext.Request.Headers["Authorization"];

            if (token == null)
            {
                return Unauthorized(new ResultJson() { Message = "Por favor vuelva a ingresar a su cuenta" });
            }

            var result = _liquidationServices.LoginSecurity(token);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return Unauthorized(new ResultJson() { Message = "Por favor vuelva a ingresar a su cuenta" });
            }

            var dbUser = JsonConvert.DeserializeObject<UsuarioDTO>(result.Content);

            if (dbUser.rol == "Liquidador")
            {
                var dbAuthrorization = await _liquidationServices.GetAuthorizations();
                if (dbAuthrorization.Rows.Count == 0)
                {
                    return BadRequest(new ResultJson() { Message = "Tabla vacia" });
                }
                else return Ok(dbAuthrorization);
            }
            if (dbUser.rol == "Empresa")
            {
                var dbAuthrorization = await _liquidationServices.GetAuthorizations(dbUser.dniCuit);
                if (dbAuthrorization.Rows.Count == 0)
                {
                    return BadRequest(new ResultJson() { Message = "Tabla vacia" });
                }
                else return Ok(dbAuthrorization);
            }
            return BadRequest(new ResultJson() { Message = "Error al loguearse" }); ;
        }

        [HttpGet("GetLiquidations")]
        public async Task<ActionResult> GetLiquidations()
        {
            string token = HttpContext.Request.Headers["Authorization"];

            if (token == null)
            {
                return Unauthorized(new ResultJson() { Message = "Por favor vuelva a ingresar a su cuenta" });
            }

            var result = _liquidationServices.LoginSecurity(token);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return Unauthorized(new ResultJson() { Message = "Por favor vuelva a ingresar a su cuenta" });
            }

            var dbUser = JsonConvert.DeserializeObject<UsuarioDTO>(result.Content);
            if (dbUser.rol == "Empleado")
            {
                var dbLiquidacion = await _liquidationServices.GetLiquidationsByDni(dbUser.dniCuit);
                if (dbLiquidacion.Rows.Count == 0)
                {
                    return BadRequest(new ResultJson() { Message = "Tabla vacia" });
                }
                else return Ok(dbLiquidacion);
            }
            if (dbUser.rol == "Empresa")
            {
                var dbLiquidacion = await _liquidationServices.GetLiquidationsByCuit(dbUser.dniCuit);
                if (dbLiquidacion.Rows.Count == 0)
                {
                    return BadRequest(new ResultJson() { Message = "Tabla vacia" });
                }
                else return Ok(dbLiquidacion);
            }
            if (dbUser.rol == "Liquidador")
            {
                var dbLiquidacion = await _liquidationServices.GetLiquidations();
                if (dbLiquidacion.Rows.Count == 0)
                {
                    return BadRequest(new ResultJson() { Message = "Tabla vacia" });
                }
                else return Ok(dbLiquidacion);
            }
            return BadRequest(new ResultJson() { Message = "Error al loguearse" }); ;
        }
    }
}

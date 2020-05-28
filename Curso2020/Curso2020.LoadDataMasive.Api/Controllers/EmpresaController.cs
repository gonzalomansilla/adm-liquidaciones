using Curso2020.Common.DTO;
using Curso2020.Service.Empresa.ABM.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Curso2020.Service.Empresa.GridDetail.Interfaces;

namespace Curso2020.Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly ILogger<EmpresaController> _logger;
        private readonly IAbmServiceAsync _abmServiceAsync;
        private readonly IGridServiceAsync _gridDetailServiceAsync;

        public EmpresaController(ILogger<EmpresaController> logger, IAbmServiceAsync abmServiceAsync, IGridServiceAsync gridDetailServiceAsync)
        {
            _logger = logger;
            _logger.LogInformation("Constructor EmpresaController");
            _abmServiceAsync = abmServiceAsync;
            _gridDetailServiceAsync = gridDetailServiceAsync;
        }


        //POST: api/Empresa/Alta
        [HttpPost("Alta")]

        public async Task <ActionResult> Alta([FromBody] EmpresaDTO empresaDTO)
        {
            if (empresaDTO.Cuit == null || empresaDTO.Cuit == "" || empresaDTO.Cuit.Length != 11 || !empresaDTO.Cuit.All(char.IsDigit))
            {
                return BadRequest(new ResultJson() { Message = "Los valores ingresados no corresponden a un CUIT valido" });
               
            }
            else
            {
                if (empresaDTO.Nombre == null || empresaDTO.Nombre == "" || empresaDTO.RazonSocial == null || empresaDTO.RazonSocial == ""
                    || empresaDTO.Direccion == null || empresaDTO.Direccion == "")
                    return BadRequest(new ResultJson() { Message = "complete TODOS los campos" });
                else
                     if (empresaDTO.Nombre.Length <3 || empresaDTO.RazonSocial.Length <3 || empresaDTO.Direccion.Length < 3 )
                    return BadRequest(new ResultJson() { Message = "ingrese datos VALIDOS" });
            }
           

            var businessCreateDB = await _abmServiceAsync.AltaEmpresa(empresaDTO);

            if (businessCreateDB != null)
            {
                return Created("", new ResultJson() { Message = "Empresa dada de ALTA correctamente" });
            }
            else
            {
                return BadRequest(new ResultJson() { Message = "Empresa EXISTENTE" });
            }

        }

        
        //GET: api/Empresa/Grid
        [HttpGet("Grid")]
        public async Task<ActionResult> Grid()
        {

            var user = await _gridDetailServiceAsync.Grid();
            return Ok(user);
        }

        //Post: api/Empresa/Detail
        [HttpPost("Detail")]
        public async Task<ActionResult> Detail(string cuit)
        {

            var user = await _gridDetailServiceAsync.Detail(cuit);
            if (user == null )
            {
                return BadRequest(new ResultJson() { Message = "Los valores ingresados no corresponden a un CUIT valido" });

            }

            return Ok(user);
        }

        //POST: api/Empresa/Modificar
        [HttpPost("Modificar")]
        public async Task<ActionResult> Modificar([FromBody] EmpresaDTO empresa)
        {
            empresa.Cuit.Trim();
            if (string.IsNullOrWhiteSpace(empresa.Cuit) || empresa.Cuit.Length != 11 || !empresa.Cuit.All(char.IsDigit))
            {
                return BadRequest(new ResultJson() { Message = "Formato de cuit incorrecto(11 caracteres numéricos)" });
            }
            if (empresa.Nombre != null && (empresa.Nombre.All(char.IsWhiteSpace) || empresa.Nombre == ""))
            {
                empresa.Nombre = null;
            }
            if (empresa.Direccion != null && (empresa.Direccion.All(char.IsWhiteSpace) || empresa.Direccion == ""))
            {
                empresa.Direccion = null;
            }
            if (empresa.RazonSocial != null && (empresa.RazonSocial.All(char.IsWhiteSpace) || empresa.RazonSocial == ""))
            {
                empresa.RazonSocial = null;
            }
            if (empresa.Direccion != null || empresa.RazonSocial != null || empresa.Nombre != null)
            {
                if (empresa.Direccion != null && empresa.Direccion.Length < 3)
                {
                    return BadRequest(new ResultJson() { Message = "Direccion muy corta" });
                }
                if (empresa.RazonSocial != null && empresa.RazonSocial.Length < 3)
                {
                    return BadRequest(new ResultJson() { Message = "Razon social muy corta" });
                }
                if (empresa.Nombre != null && empresa.Nombre.Length < 3)
                {
                    return BadRequest(new ResultJson() { Message = "Nombre muy corto" });
                }
                var response = await _abmServiceAsync.Modificar(empresa);
                if (response)
                {
                    return Ok(new ResultJson() { Message = "Cambios realizados correctamente" });
                }
                return NotFound(new ResultJson() { Message = "No existe la empresa" });
            }
            return BadRequest(new ResultJson() { Message = "No hay datos a modificar" });
        }


            [HttpPost("Delete")]

        public async Task<ActionResult> Delete([FromBody] string cuit)
        {


            var Respuesta = await _abmServiceAsync.Delete(cuit);
            if (Respuesta)
            {
                return Ok();
            }
            else
            {
                return NotFound();

            }



        }
    }
}
        
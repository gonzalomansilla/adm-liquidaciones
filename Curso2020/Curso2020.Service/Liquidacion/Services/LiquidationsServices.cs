using Curso2020.Liquidations.Common.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Curso.Model.Context;
using Curso2020.Model.Model;
using Curso2020.Seguridad.Service.Interfaces;
using Curso2020.Common.DTO;

namespace Curso2020.Liquidations.Services.Services
{
    public class LiquidationsServices : ILiquidationsServices
    {
        private readonly ILogger<LiquidationsServices> _logger;
        private readonly CursoContext _cursoContext;

        public LiquidationsServices(ILogger<LiquidationsServices> logger, CursoContext cursoContext)
        {
            _logger = logger;
            _cursoContext = cursoContext;
            _logger.LogInformation("Constructor LiquidationsServiceAsync");
        }

        public async Task<LiquidacionesDTO<Liquidacion>> GetLiquidationsByDni(string dni)
        {
            LiquidacionesDTO<Liquidacion> dbLiquidationDTO = new LiquidacionesDTO<Liquidacion>();
            var dbLiquidation = await _cursoContext.Liquidaciones.Where(u => u.dniEmpleado == dni).ToListAsync();
            if (dbLiquidation != null)
            {
                dbLiquidationDTO.Header();
                dbLiquidationDTO.Rows = dbLiquidation;
                return dbLiquidationDTO;
            }
            return null;
        }

        public async Task<LiquidacionesDTO<Liquidacion>> GetLiquidationsByCuit(string cuit)
        {
            LiquidacionesDTO<Liquidacion> dbLiquidationDTO = new LiquidacionesDTO<Liquidacion>();
            var dbLiquidation = await _cursoContext.Liquidaciones.Where(u => u.cuitEmpresa == cuit).ToListAsync();
            if (dbLiquidation != null)
            {
                dbLiquidationDTO.Header();
                dbLiquidationDTO.Rows = dbLiquidation;
                return dbLiquidationDTO;
            }
            return null;
        }

        public async Task<Autorizacion> AuthorizeLiquidations(string fecha, string cuitEmpresa)
        {
            var dbAutorizacion = await _cursoContext.Autorizaciones.Where(u => u.cuitEmpresa == cuitEmpresa
                                                                                    && u.fecha == fecha).FirstOrDefaultAsync();
            if (dbAutorizacion == null)
            {
                Autorizacion autorizacion = new Autorizacion();
                autorizacion.cuitEmpresa = cuitEmpresa;
                autorizacion.fecha = fecha;
                _cursoContext.Autorizaciones.Add(autorizacion);
                _cursoContext.SaveChanges();
                return autorizacion;
            }
            return null;
        }

        public async Task<LiquidacionesDTO<Liquidacion>> GetLiquidations()
        {
            LiquidacionesDTO<Liquidacion> dbLiquidationDTO = new LiquidacionesDTO<Liquidacion>();
            var dbLiquidation = await _cursoContext.Liquidaciones.ToListAsync();
            dbLiquidationDTO.Header();
            dbLiquidationDTO.Rows = dbLiquidation;

            return dbLiquidationDTO;
        }

        public async Task<Autorizacion> StartLiquidations(Autorizacion autorizacion)
        {
            var dbAuthorization = await _cursoContext.Autorizaciones.Where(u => u.cuitEmpresa == autorizacion.cuitEmpresa
                                                                                    && u.fecha == autorizacion.fecha).FirstOrDefaultAsync();
            if (dbAuthorization == null)
            {
                return null;
            }
            var dbLiquidation = await _cursoContext.Liquidaciones.Where(u => u.cuitEmpresa == autorizacion.cuitEmpresa
                                                                                    && u.fecha == autorizacion.fecha).FirstOrDefaultAsync();
            if (dbLiquidation != null)
            {
                return null;
            }
            return dbAuthorization;
        }

        public async Task<AutorizacionesDTO<Autorizacion>> GetAuthorizations()
        {
            AutorizacionesDTO<Autorizacion> dbAutorizacionesDTO = new AutorizacionesDTO<Autorizacion>();
            var dbAutorizacion = await _cursoContext.Autorizaciones.ToListAsync();
            dbAutorizacionesDTO.Header();
            dbAutorizacionesDTO.Rows = dbAutorizacion;

            return dbAutorizacionesDTO;
        }
        public async Task<AutorizacionesDTO<Autorizacion>> GetAuthorizations(string cuit)
        {
            AutorizacionesDTO<Autorizacion> dbAutorizacionesDTO = new AutorizacionesDTO<Autorizacion>();
            var dbAutorizaciones = await _cursoContext.Autorizaciones.Where(u => u.cuitEmpresa == cuit).ToListAsync();
            dbAutorizacionesDTO.Header();
            dbAutorizacionesDTO.Rows = dbAutorizaciones;

            return dbAutorizacionesDTO;
        }

        public async Task<bool> Liquidation(Autorizacion autorizacion)
        {
            var liquidacionesEfectuadas = 0;
            var dbEmpleado = await _cursoContext.Empleados.Where(u => u.cuitEmpresa == autorizacion.cuitEmpresa).ToListAsync();

            if (dbEmpleado != null)
            {
                foreach (Empleado empleado in dbEmpleado)
                {
                    var puesto = await _cursoContext.PuestosEmpresa.Where(u => u.empresaCuit == autorizacion.cuitEmpresa
                                                                            && u.puestoId == empleado.idPuesto).FirstOrDefaultAsync();
                    if (puesto != null)
                    {
                        Liquidacion liquidacionEmpleado = new Liquidacion();
                        liquidacionEmpleado.liquidacion = empleado.horasTrabajadasUltimoMes * puesto.pagoPorHora;
                        liquidacionEmpleado.cuitEmpresa = autorizacion.cuitEmpresa;
                        liquidacionEmpleado.dniEmpleado = empleado.dni;
                        liquidacionEmpleado.fecha = autorizacion.fecha;
                        _cursoContext.Liquidaciones.Add(liquidacionEmpleado);
                        await _cursoContext.SaveChangesAsync();
                        liquidacionesEfectuadas++;
                    }
                    return true;
                }
            }
            return false;
        }

        public IRestResponse LoginSecurity(string token)
        {
            var client = new RestClient("https://localhost:5003/api/Security/VerifyToken");
            var request = new RestRequest(Method.POST);
            client.Timeout = -1;
            request.AddHeader("Content-Type", "application/json");
            request.AddQueryParameter("token", token);
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
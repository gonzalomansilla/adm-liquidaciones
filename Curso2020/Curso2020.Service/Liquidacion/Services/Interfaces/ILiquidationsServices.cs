using Curso2020.Common.DTO;
using Curso2020.Liquidations.Common.DTO;
using Curso2020.Model.Model;
using RestSharp;
using System.Threading.Tasks;

namespace Curso2020.Seguridad.Service.Interfaces
{
    public interface ILiquidationsServices
    {
        Task<LiquidacionesDTO<Liquidacion>> GetLiquidationsByDni(string dni);
        Task<LiquidacionesDTO<Liquidacion>> GetLiquidationsByCuit(string cuit);
        Task<Autorizacion> AuthorizeLiquidations(string fecha, string dniEmpresa);
        Task<Autorizacion> StartLiquidations(Autorizacion autorizacion);
        Task<LiquidacionesDTO<Liquidacion>> GetLiquidations();
        Task<AutorizacionesDTO<Autorizacion>> GetAuthorizations();
        Task<AutorizacionesDTO<Autorizacion>> GetAuthorizations(string dniCuit);
        Task<bool> Liquidation(Autorizacion autorizacion);
        IRestResponse LoginSecurity(string token);
    }
}
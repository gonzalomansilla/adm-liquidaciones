using Curso2020.Common.DTO;
using Curso2020.Model.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Curso2020.Service.ABMEmpleado.Interfaces
{
    public interface IEmployeeABMAsync
    {
        Task<string> AddEmployee(EmpleadoDTO empleado);
        Task<int> ModifyEmployee(EmpleadoDTO empleadoDTO);
        Task<bool> DeleteEmployee(EmpleadoDTO empleadoDTO);
        Task<List<Puesto>> CheckJobPositions(string cuitEmpresa);
        Task<TablaJsonEmpleado<EmpleadoGrillaDTO>> ObtenerGrilla(string cuitEmpresa);
        Task<EmpleadoDTO> GetByDni(string dni);
    }
}

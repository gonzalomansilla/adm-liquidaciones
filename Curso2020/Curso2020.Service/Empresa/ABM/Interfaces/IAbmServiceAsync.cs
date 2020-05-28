using Curso2020.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Curso2020.Service.Empresa.ABM.Interfaces
{
   public interface IAbmServiceAsync
    {
        Task<Model.Model.Empresa>AltaEmpresa(EmpresaDTO empresaDTO);
        Task<bool> Modificar(EmpresaDTO empresa);
        Task<bool> Delete(string cuit);

    }
}

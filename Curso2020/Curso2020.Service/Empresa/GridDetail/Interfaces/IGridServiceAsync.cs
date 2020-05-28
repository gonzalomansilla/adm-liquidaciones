using Curso2020.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Curso2020.Service.Empresa.GridDetail.Interfaces
{
    public interface IGridServiceAsync
    {
        Task<List<EmpresaPresentacionDTO>> Grid();
        Task<Model.Model.Empresa> Detail(string cuit);
    }
}

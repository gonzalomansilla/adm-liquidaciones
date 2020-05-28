using Curso.Model.Context;
using Curso2020.Common.DTO;
using Curso2020.Service.Empresa.GridDetail.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso2020.Service.Empresa.GridDetail
{
    public class GridDetailServiceAsync : IGridServiceAsync
    {

        private readonly ILogger<GridDetailServiceAsync> _logger;
        private readonly CursoContext _context;
        public GridDetailServiceAsync(ILogger<GridDetailServiceAsync> logger, CursoContext context)
        {
            _logger = logger;
            _context = context;
            _logger.LogInformation("Constructor TableController");
        }


        public async Task<List<EmpresaPresentacionDTO>> Grid()
        {

            List<Model.Model.Empresa> empresas = await _context.Empresas.ToListAsync();
            List<EmpresaPresentacionDTO> result = new List<EmpresaPresentacionDTO>();
            foreach (var e in empresas)
            {
                result.Add(new EmpresaPresentacionDTO()
                {
                    Cuit = e.cuit,
                    Nombre = e.nombre
                });
            }

            return result;
        }



        public async Task<Model.Model.Empresa> Detail(string cuit)
        {

            var Empresa = await _context.Empresas.Where(u => u.cuit == cuit).FirstOrDefaultAsync();

            return Empresa;

        }

    }
}

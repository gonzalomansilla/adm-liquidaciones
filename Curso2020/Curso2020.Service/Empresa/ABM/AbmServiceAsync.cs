using Curso.Model.Context;
using Curso2020.Common.DTO;
using Curso2020.Model.Model;
using Curso2020.Service.Empresa.ABM.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso2020.Service.Empresa.ABM
{
    public class AbmServiceAsync : IAbmServiceAsync
    {
        private readonly ILogger<AbmServiceAsync> _logger;
        private readonly CursoContext _empresaContext;

        public AbmServiceAsync(ILogger<AbmServiceAsync> logger, CursoContext empresaContext)
        {
            _logger = logger;
            _empresaContext = empresaContext;
            _logger.LogInformation("Constructor AbmServiceAsync");
        }

        public async Task<Model.Model.Empresa> AltaEmpresa(EmpresaDTO empresaDTO)
        {
            var dbEmpresa = await _empresaContext.Empresas.Where(e => e.cuit == empresaDTO.Cuit).FirstOrDefaultAsync();
            if (dbEmpresa == null)
            {
                Model.Model.Empresa empresaAlta = new Model.Model.Empresa();
                empresaAlta.cuit = empresaDTO.Cuit;
                empresaAlta.nombre = empresaDTO.Nombre;
                empresaAlta.razonSocial = empresaDTO.RazonSocial;
                empresaAlta.direccion = empresaDTO.Direccion;


                _empresaContext.Empresas.Add(empresaAlta);

                List<Puesto> dbPuestos = await _empresaContext.Puestos.ToListAsync();

                foreach (Puesto puesto in dbPuestos)
                {
                    PuestoEmpresa puestoEmpresa = new PuestoEmpresa();
                    puestoEmpresa.puestoId = puesto.id;
                    puestoEmpresa.pagoPorHora = puesto.salarioPorDefecto;
                    puestoEmpresa.empresaCuit = empresaAlta.cuit;

                    _empresaContext.PuestosEmpresa.Add(puestoEmpresa);

                    await _empresaContext.SaveChangesAsync();
                }

                await _empresaContext.SaveChangesAsync();
                return empresaAlta;
            }
            else
                return null;

        }

        public async Task<bool> Modificar(EmpresaDTO empresa)
        {
            var dbEmpresa = await _empresaContext.Empresas.Where(u => u.cuit == empresa.Cuit).FirstOrDefaultAsync();
            if (dbEmpresa != null)
            {
                dbEmpresa.direccion = empresa.Direccion ?? dbEmpresa.direccion;
                dbEmpresa.nombre = empresa.Nombre ?? dbEmpresa.nombre;
                dbEmpresa.razonSocial = empresa.RazonSocial ?? dbEmpresa.razonSocial;
                await _empresaContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> Delete(string cuit)
        {
            var PuestosEmpresaBorrar = _empresaContext.Empresas.Include(c => c.Puestos).First(i => i.cuit == cuit);
            _empresaContext.RemoveRange(PuestosEmpresaBorrar.Puestos);
            var dbEmpresa = await _empresaContext.Empresas.Where(u => u.cuit == cuit).FirstOrDefaultAsync();


            if (dbEmpresa != null)

            {
                _empresaContext.Remove(dbEmpresa);
                _empresaContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}


            
            
        




       


            




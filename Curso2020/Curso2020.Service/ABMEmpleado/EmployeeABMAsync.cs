using System;
using System.Linq;
using Curso.Model.Context;
using Curso2020.Common.DTO;
using Curso2020.Model.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Curso2020.Service.ABMEmpleado.Interfaces;

namespace Curso2020.Service.ABMEmpleado
{
    public class EmployeeABMAsync : IEmployeeABMAsync
    {
        private readonly CursoContext _cursoContext;
        private readonly ILogger<EmployeeABMAsync> _logger;

        public EmployeeABMAsync(ILogger<EmployeeABMAsync> logger, CursoContext cursoContext)
        {
            _logger = logger;
            _cursoContext = cursoContext;
            _logger.LogInformation("Constructor EmployeeABM");
        }

        public async Task<string> AddEmployee(EmpleadoDTO empleado)
        {
            var dbEmployee = await _cursoContext.Empleados.Where(x => x.dni == empleado.Dni).FirstOrDefaultAsync();
            var dbJob = await _cursoContext.PuestosEmpresa.Where(x => x.empresaCuit == empleado.CuitEmpresa && x.puestoId == empleado.IdPuesto).FirstOrDefaultAsync();

            Random random = new Random();
            // Minimo = Trabajo de 4hs * 5 dias * 4 semanas = 80hs, Maximo = Trabajo de 12hs * 5 dias * 4 semanas = 240hs
            int horasTrabajadas = random.Next(80, 240);

            if (dbEmployee == null && dbJob != null)
            {
                _cursoContext.Empleados.Add(new Empleado() { dni = empleado.Dni, cuitEmpresa = empleado.CuitEmpresa, idPuesto = empleado.IdPuesto, archivo = null, nombre = empleado.Nombre, apellido = empleado.Apellido, horasTrabajadasUltimoMes = horasTrabajadas });
                await _cursoContext.SaveChangesAsync();

                return "Creado";
            }
            else if (dbEmployee != null)
            {
                return "EmpleadoYaExiste";
            }
            else if (dbJob == null)
            {
                return "PuestoNoEncontrado";
            }

            return null;
        }

        public async Task<bool> DeleteEmployee(EmpleadoDTO empleadoDTO)
        {
            //Busca el empleado en la base de datos
            var dbEmployee = await _cursoContext.Empleados.Where(x => x.dni == empleadoDTO.Dni && x.cuitEmpresa == empleadoDTO.CuitEmpresa).FirstOrDefaultAsync();

            if (dbEmployee != null)
            {
                _cursoContext.Remove(dbEmployee);
                await _cursoContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<int> ModifyEmployee(EmpleadoDTO empleadoDTO)
        {
            //Busca el empleado en la base de datos
            var dbEmployee = await _cursoContext.Empleados.Where(x => x.dni == empleadoDTO.Dni && x.cuitEmpresa == empleadoDTO.CuitEmpresa).FirstOrDefaultAsync();

            if (dbEmployee == null)
            {
                return 2;
            }

            //Busca el puesto en la base de datos      
            var dbPuesto = await _cursoContext.PuestosEmpresa.Where(x => x.puestoId == empleadoDTO.IdPuesto && x.empresaCuit == dbEmployee.cuitEmpresa).FirstOrDefaultAsync();

            if (dbPuesto == null)
            {
                return 3;
            }

            if (dbEmployee.horasTrabajadasUltimoMes == empleadoDTO.HorasTrabajadasPorMes && dbEmployee.idPuesto == empleadoDTO.IdPuesto)
            {
                return 4;
            }

            if (dbEmployee.idPuesto != empleadoDTO.IdPuesto || dbEmployee.horasTrabajadasUltimoMes != empleadoDTO.HorasTrabajadasPorMes)
            {
                dbEmployee.idPuesto = empleadoDTO.IdPuesto;

                dbEmployee.horasTrabajadasUltimoMes = empleadoDTO.HorasTrabajadasPorMes;

                await _cursoContext.SaveChangesAsync();

                return 1;
            }

            return -1;

        }

        public async Task<TablaJsonEmpleado<EmpleadoGrillaDTO>> ObtenerGrilla(string cuitEmpresa)
        {
            var dbEmpleados = await _cursoContext.Empleados.Where(x => x.cuitEmpresa == cuitEmpresa).ToListAsync();
            if (dbEmpleados.Count > 0)
            {
                TablaJsonEmpleado<EmpleadoGrillaDTO> retorno = new TablaJsonEmpleado<EmpleadoGrillaDTO>();
                foreach (Empleado empleado in dbEmpleados)
                {
                    int idPuesto = empleado.idPuesto;
                    var Puesto = await _cursoContext.Puestos.Where(x => x.id == idPuesto).FirstOrDefaultAsync();
                    
                    EmpleadoGrillaDTO empleadoMostrar = new EmpleadoGrillaDTO()
                    {
                        Nombre = empleado.nombre,
                        Apellido = empleado.apellido,
                        Dni = empleado.dni,
                        Puesto = Puesto.descripcion
                    };
                    if (empleado.archivo == null)
                    {
                        empleadoMostrar.Archivo = "-";
                    }
                    else
                    {
                        empleadoMostrar.Archivo = empleado.archivo;
                    }                    
                    
                    empleadoMostrar.HorasTrabajadas = empleado.horasTrabajadasUltimoMes;
                    retorno.Rows.Add(empleadoMostrar);
                }

                return retorno;
            }
            return null;
        }

        public async Task<List<Puesto>> CheckJobPositions(string cuitEmpresa)
        {
            var dbPuestos = await (from pe in _cursoContext.PuestosEmpresa join p in _cursoContext.Puestos on pe.puestoId equals p.id where pe.empresaCuit == cuitEmpresa select new { Id = p.id, Descripcion = p.descripcion }).ToListAsync();

            List<Puesto> puestos = new List<Puesto>();

            foreach (var p in dbPuestos)
            {
                puestos.Add(new Puesto() { id = p.Id, descripcion = p.Descripcion });
            }

            return puestos;
        }

        public async Task<EmpleadoDTO> GetByDni(string dni)
        {
            var empleado = await _cursoContext.Empleados
                .Where(x => x.dni == dni)
                .FirstOrDefaultAsync();

            if (empleado != null)
            {
                return new EmpleadoDTO
                {
                    Nombre = empleado.nombre,
                    Apellido = empleado.apellido,
                    IdPuesto = empleado.idPuesto,
                    Dni = empleado.dni,
                    CuitEmpresa = empleado.cuitEmpresa,
                    HorasTrabajadasPorMes = empleado.horasTrabajadasUltimoMes,
                };
            }
            else
            {
                return null;
            }
        }

    }
}

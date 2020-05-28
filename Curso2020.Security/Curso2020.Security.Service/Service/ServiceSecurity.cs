using Curso2020.Security.Common.DTO;
using Curso2020.Security.Model.Context;
using Curso2020.Security.Model.Model;
using Curso2020.Security.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Curso2020.Security.Service.Service
{
    public class ServiceSecurity : ISecurity
    {
        private readonly ILogger<ISecurity> _logger;
        private readonly LoginContext _Context;

        public ServiceSecurity(ILogger<ISecurity> logger, LoginContext Context)
        {
            _logger = logger;
            _Context = Context;
            _logger.LogInformation("Constructor SecurityService");
        }
        public async Task<Usuario> Login(UsuarioDTO user)
        {
            var dbUser = await _Context.Usuarios.Where(u => u.nombreUsuario == user.nombreUsuario && u.contrasenia == user.contrasenia).FirstOrDefaultAsync();
            if (dbUser != null)
            {
                dbUser.fechaUltimoIngreso = DateTime.Now;
                dbUser.guid = Guid.NewGuid().ToString();
                await _Context.SaveChangesAsync();

                return dbUser;
            }
            return null;
        }
        public async Task<Usuario> UpdatePass(UsuarioDTO user)
        {
            //Trae el primer User que machea con el nombre de usuario
            var dbUser = await _Context.Usuarios.Where(u => u.nombreUsuario == user.nombreUsuario && u.contrasenia == user.contrasenia).FirstOrDefaultAsync();
            //Comprobar si trajo los datos
            if (dbUser != null)
            {
                //Realizar el cambio de Password
                dbUser.contrasenia = user.nuevaContrasenia;
                await _Context.SaveChangesAsync();
                return dbUser;
            }
            return null;
        }

        public async Task<Usuario> VerifyToken(string token)
        {
            var start = DateTime.Now;
            var dbUser = await _Context.Usuarios.Where(u => u.guid == token).FirstOrDefaultAsync();
            if (dbUser == null || dbUser.guid == null || start.Subtract(dbUser.fechaUltimoIngreso) >= TimeSpan.FromMinutes(10))
            {
                return null;
            }
            return dbUser;
        }

        public async Task<int> CreateUser(Usuario usuario)
        {
            var dbUser = await _Context.Usuarios.Where(u => u.dniCuit == usuario.dniCuit ||
                                                            u.nombreUsuario == usuario.nombreUsuario).FirstOrDefaultAsync();

            if (dbUser == null)
            {
                if (usuario.rol == "Empresa")
                {
                    var result = checkBussiness(usuario.dniCuit);

                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return 4;
                    }
                }
                _Context.Usuarios.Add(usuario);
                _Context.SaveChanges();
                return 1;
            }
            if (dbUser.dniCuit == usuario.dniCuit)
            {
                return 2;
            }
            if (dbUser.nombreUsuario == usuario.nombreUsuario)
            {
                return 3;
            }
            return 0;
        }

        public IRestResponse checkBussiness(string cuit)
        {
            var client = new RestClient("https://localhost:5001/api/Empresa/Detail");
            var request = new RestRequest(Method.POST);
            client.Timeout = -1;
            request.AddHeader("Content-Type", "application/json");
            request.AddQueryParameter("cuit", cuit);
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}

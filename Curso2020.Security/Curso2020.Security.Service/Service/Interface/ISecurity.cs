using Curso2020.Security.Common.DTO;
using Curso2020.Security.Model.Model;
using System.Threading.Tasks;

namespace Curso2020.Security.Service.Interface
{
    public interface ISecurity
    {
        Task<Usuario> Login(UsuarioDTO user);

        Task<Usuario> UpdatePass(UsuarioDTO user);

        Task<Usuario> VerifyToken(string token);

        Task<int> CreateUser(Usuario user);
    }
}

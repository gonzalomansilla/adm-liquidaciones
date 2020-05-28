
namespace Curso2020.Common.DTO
{
    public class EmpresaDTO
    {
        string _cuit;
        string _nombre;
        string _razonSocial;
        string _direccion;
        public string Cuit { get => _cuit; set => _cuit = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public string RazonSocial { get => _razonSocial; set => _razonSocial = value; }
        public string Direccion { get => _direccion; set => _direccion = value; }
    }
}

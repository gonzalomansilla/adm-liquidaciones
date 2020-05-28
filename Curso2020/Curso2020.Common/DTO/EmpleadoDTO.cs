
namespace Curso2020.Common.DTO
{
    public class EmpleadoDTO
    {
        string _dni;
        string _cuitEmpresa;
        int _idPuesto;
        string _archivo;
        string _nombre;
        string _apellido;
        int _horasTrabajadasPorMes;

        public string Dni { get => _dni; set => _dni = value; }
        public string CuitEmpresa { get => _cuitEmpresa; set => _cuitEmpresa = value; }
        public int IdPuesto { get => _idPuesto; set => _idPuesto = value; }
        public string Archivo { get => _archivo; set => _archivo = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public string Apellido { get => _apellido; set => _apellido = value; }
        public int HorasTrabajadasPorMes { get => _horasTrabajadasPorMes; set => _horasTrabajadasPorMes = value; }
    }
}

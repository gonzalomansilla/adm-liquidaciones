namespace Curso2020.Security.Common.DTO
{
    public class UsuarioDTO
    {

		private string _nombreUsuario;
		private string _contrasenia;
		private string _nuevaContrasenia;
		private string _guid;
		private string _dniCuit;
		private string _rol;

		public string nombreUsuario { get => _nombreUsuario; set => _nombreUsuario = value; }
		public string contrasenia { get => _contrasenia; set => _contrasenia = value; }
		public string nuevaContrasenia { get => _nuevaContrasenia; set => _nuevaContrasenia = value; }
		public string guid { get => _guid; set => _guid = value; }
		public string dniCuit { get => _dniCuit; set => _dniCuit = value; }
		public string rol { get => _rol; set => _rol = value; }
	}
}

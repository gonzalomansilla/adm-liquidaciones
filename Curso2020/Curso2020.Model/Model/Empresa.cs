using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Curso2020.Model.Model
{
   public class Empresa
    {
		[Key]
		[MaxLength(11)]
		public string cuit { get; set; }

		[MaxLength(25)]
		[Required]
		public string nombre { get; set; }

		[MaxLength(50)]
		[Required]
		public string razonSocial { get; set; }

		[MaxLength(50)]
		[Required]
		public string direccion { get; set; }

		public virtual ICollection<PuestoEmpresa> Puestos { get; set; }
	}
}

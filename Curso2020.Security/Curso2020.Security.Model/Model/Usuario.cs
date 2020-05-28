using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curso2020.Security.Model.Model
{
   public class Usuario
    {
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
		public long id { get; set; }

		[Required]
		public string dniCuit { get; set; }

		[MaxLength(20)]
		[Required]
		public string rol { get; set; }

		[MaxLength(50)]
		[Required]
		public string nombreUsuario { get; set; }

		[MaxLength(50)]
		[Required]
		public string contrasenia { get; set; }

		[DefaultValue(null)]
		public DateTime fechaUltimoIngreso { get; set; }

		[DefaultValue(null)]
		public string guid { get; set; }
	}
}
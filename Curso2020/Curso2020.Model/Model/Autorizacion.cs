using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curso2020.Model.Model
{
	public class Autorizacion
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
		public long id { get; set; }

		[Required]
		[MaxLength(11)]
		public string cuitEmpresa { get; set; }

		[Required]
		[MaxLength(7)]
		public string fecha { get; set; }
	}
}

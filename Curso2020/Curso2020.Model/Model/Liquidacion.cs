using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curso2020.Model.Model
{
	public class Liquidacion
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
		public long id { get; set; }

		[Required]
		[MaxLength(11)]
		public string cuitEmpresa { get; set; }

		[Required]
		[MaxLength(8)]
		public string dniEmpleado { get; set; }

		[Required]
		public double liquidacion { get; set; }

		[Required]
		[MaxLength(7)]
		public string fecha { get; set; }
	}
}
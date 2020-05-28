using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curso2020.Model.Model
{
	public class ArchivoEmpleados
	{
		[Key]
		[Column("nombre")]
		public string archivo { get; set; }

		[Required]
		public DateTime fechaDeProcesado { get; set; }
	}
}

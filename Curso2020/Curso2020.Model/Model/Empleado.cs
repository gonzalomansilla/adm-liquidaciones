using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curso2020.Model.Model
{
   public class Empleado
    {
        [Key, MaxLength(8)]
        [Required]
        public string dni { get; set; }

        [Required]
        public string cuitEmpresa { get; set; }
        [ForeignKey("cuitEmpresa")]
        public virtual Empresa empresa { get; set; }

        [Required]
        public int idPuesto { get; set; }
        [ForeignKey("idPuesto")]
        public virtual Puesto puestosEmpresas { get; set; }

        public string archivo { get; set; }
        [ForeignKey("archivo")]
        public virtual ArchivoEmpleados archivosEmpleados { get; set; }

        [MaxLength(30)]
        [Required]
        public string nombre { get; set; }

        [MaxLength(30)]
        [Required]
        public string apellido { get; set; }

        [Required]
        public int horasTrabajadasUltimoMes { get; set; }
    }
}

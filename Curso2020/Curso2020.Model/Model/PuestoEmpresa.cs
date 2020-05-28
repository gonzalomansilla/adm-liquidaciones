using System.ComponentModel.DataAnnotations;

namespace Curso2020.Model.Model
{
   public class PuestoEmpresa
    {        
        [Required]
        public int puestoId { get; set; }

        public Puesto puesto { get; set; }

        public string empresaCuit { get; set; }
        public Empresa empresa { get; set; }

        public double pagoPorHora { get; set; }
    }
}

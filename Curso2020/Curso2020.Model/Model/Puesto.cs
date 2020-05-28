using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Curso2020.Model.Model
{
   public class Puesto
    {        
        public int id { get; set; }
        
        [Required]
        public string descripcion { get; set; }
        
        [Required]
        public double salarioPorDefecto { get; set; }
        public virtual ICollection<PuestoEmpresa> Empresas { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Curso2020.Common.DTO
{
    //Este DTO fue creado para poder crear una nueva lista en el GridDetailService y 
    //que solo tome los atributos nombre y cuit de empresa.
    public class EmpresaPresentacionDTO
    {
        string _cuit;
        string _nombre;

        public string Cuit { get => _cuit; set => _cuit = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
    }
}

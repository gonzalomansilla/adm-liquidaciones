using System;
using System.Collections.Generic;
using System.Text;

namespace Curso2020.Common.DTO
{
    public class TablaJsonEmpleado<T>
    {
        private List<string> headers;
        private List<T> rows;

        public TablaJsonEmpleado()
        {
            this.headers = new List<string>();
            this.rows = new List<T>();
            Headers.Add("Nombre");
            Headers.Add("Apellido");
            Headers.Add("DNI");
            Headers.Add("Puesto");
            Headers.Add("Archivo");
            Headers.Add("Horas Trabajadas");
        }

        public List<string> Headers { get => headers; set => headers = value; }
        public List<T> Rows { get => rows; set => rows = value; }
    }
}

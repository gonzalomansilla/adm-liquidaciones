using System;
using System.Collections.Generic;
using System.Text;

namespace Curso2020.Common.DTO
{
    public class EmpleadoGrillaDTO
    {
        private string nombre;
        private string apellido;
        private string dni;
        private string puesto;
        private string archivo;
        private double horasTrabajadas;

        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string Dni { get => dni; set => dni = value; }
        public string Puesto { get => puesto; set => puesto = value; }
        public string Archivo { get => archivo; set => archivo = value; }
        public double HorasTrabajadas { get => horasTrabajadas; set => horasTrabajadas = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Curso2020.Common.DTO
{
	public class ArchivoEmpleadosDTO
	{
		private string _nombre;
		private DateTime _fechaDeProcesado;

		public string Nombre { get => _nombre; set => _nombre = value; }
		public DateTime FechaDeProcesado { get => _fechaDeProcesado; set => _fechaDeProcesado = value; }
	}
}

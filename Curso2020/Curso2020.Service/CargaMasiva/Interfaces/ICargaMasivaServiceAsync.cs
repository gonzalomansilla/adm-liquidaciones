using Curso2020.Common.DTO;
using Curso2020.Model.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Curso2020.Service.CargaMasiva.Interfaces
{
	public interface ICargaMasivaServiceAsync
	{
		public Task<List<EmpleadoDTO>> ImportarArchivoEmpleados(IFormFile file);
	}
}

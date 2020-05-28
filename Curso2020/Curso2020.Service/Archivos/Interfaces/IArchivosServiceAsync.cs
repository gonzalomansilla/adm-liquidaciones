using Curso2020.Common.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Curso2020.Service.Archivos.Interfaces
{
	public interface IArchivosServiceAsync
	{
		Task<List<EmpleadoDTO>> EmpleadosDelArchivo(string nombreArchivo);
		Task<List<ArchivoEmpleadosDTO>> HistorialDeArchivosProcesados();
	}
}

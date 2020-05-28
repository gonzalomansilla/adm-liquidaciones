using System.Collections.Generic;

namespace Curso2020.Liquidations.Common.DTO
{
    public class LiquidacionesDTO<Liquidacion>
    {
        List<string> _headers = new List<string>();
        List<Liquidacion> _rows;

        public LiquidacionesDTO()
        {
            List<Liquidacion> __liquidation = new List<Liquidacion>();
        }

        public void Header()
        {
            _headers.Add("ID");
            _headers.Add("CUIT Empresa");
            _headers.Add("DNI Empleado");
            _headers.Add("Liquidacion Final");
            _headers.Add("Fecha de Emision");
        }

        public List<string> Headers { get => _headers; set => _headers = value; }
        public List<Liquidacion> Rows { get => _rows; set => _rows = value; }
    }
}

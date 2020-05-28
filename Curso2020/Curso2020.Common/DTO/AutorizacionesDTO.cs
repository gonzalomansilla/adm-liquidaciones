using System;
using System.Collections.Generic;
using System.Text;

namespace Curso2020.Common.DTO
{
    public class AutorizacionesDTO<Autorizacion>
    {
        List<string> _headers = new List<string>();
        List<Autorizacion> _rows;

        public AutorizacionesDTO()
        {
            List<Autorizacion> __liquidation = new List<Autorizacion>();
        }

        public void Header()
        {
            _headers.Add("ID");
            _headers.Add("CUIT Empresa");
            _headers.Add("Fecha");
        }

        public List<string> Headers { get => _headers; set => _headers = value; }
        public List<Autorizacion> Rows { get => _rows; set => _rows = value; }
    }
}

using System;

namespace Curso2020.Liquidations.Common.DTO
{
    public class LiquidacionDTO
    {
        private long _id;
        private string _cuitEmpresa;
        private string _dniEmpleado;
        private DateTime _fecha;
        private double _liquidacion;
        public long id { get => _id; set => _id = value; }
        public string cuitEmpresa { get => _cuitEmpresa; set => _cuitEmpresa = value; }
        public string dniEmpleado { get => _dniEmpleado; set => _dniEmpleado = value; }
        public DateTime fecha { get => _fecha; set => _fecha = value; }
        public double liquidacion { get => _liquidacion; set => _liquidacion = value; }
    }
}

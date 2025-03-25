using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NequiGestion.Entidades;

public class Transacciones {

    public required int NumeroTransaccion { get; set; }
    public DateTime? Fecha { get; set; }
    public required int CuentaOrigenID { get; set; }
    public required int CuentaDestinoID { get; set; }
    public required decimal Monto { get; set; }
    public required string Tipo { get; set; }

}

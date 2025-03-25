using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NequiGestion.Entidades;

public class Cuentas {
    public required int CuentaID { get; set; }
    public required string Nombre { get; set; } 
    public required string Apellido { get; set; }
    public required string Email { get; set; }

    [StringLength(10, MinimumLength = 10)]
    public required string Telefono { get; set; }
    public required decimal Saldo { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public  string ContrasenaHash { get; set; }
}

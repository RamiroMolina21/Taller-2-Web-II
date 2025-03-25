using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NequiGestion.Logica.Dtos
{
    public record class EliminarCuentaDto(
    
        int CuentaID,
        string Nombre,
        string Apellido,
        string Email,
        string Telefono,
        decimal Saldo,
        DateTime? FechaCreacion,
        string ContrasenaHash
        );
}

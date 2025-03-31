using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NequiGestion.Logica.Dtos
{
    public record class ActualizarCuentaDto(
    
        int CuentaID,
        string Nombre,
        string Apellido,
        string Email,
        string Telefono,
        string ContrasenaHash
        );

    
}

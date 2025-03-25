using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NequiGestion.Logica.Dtos;

public record class RegistrarTransaccionDto(

    int NumeroTransaccion,
    DateTime? Fecha,
    int CuentaOrigenID,
    int CuentaDestinoID,
    decimal Monto,
    string Tipo
    );



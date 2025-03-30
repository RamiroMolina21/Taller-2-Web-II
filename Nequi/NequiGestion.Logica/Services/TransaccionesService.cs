using NequiGestion.Entidades;
using NequiGestion.Logica.Dtos;
using NequiGestion.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NequiGestion.Logica.Services;

public class TransaccionesService
{
    private readonly TransaccionesRepository _transaccionesRepository = new();

    public bool RegistrarTransaccion(RegistrarTransaccionDto transaccionDto)
    {
        var transaccion = new Transacciones
        {
            CuentaOrigenID = transaccionDto.CuentaOrigenID,
            CuentaDestinoID = transaccionDto.CuentaDestinoID,
            Monto = transaccionDto.Monto,
            Tipo = "salida"
        };
        return _transaccionesRepository.RegistrarTransaccion(transaccion);
    }

    public List<ConsultarTransaccionDto> ConsultarTransacciones(int cuentaID, DateTime? desde, DateTime? hasta)
    {
        return _transaccionesRepository.ConsultarTransacciones(cuentaID, desde, hasta)
               .Select(x => new ConsultarTransaccionDto(x.NumeroTransaccion, x.Fecha ?? DateTime.MinValue, x.CuentaOrigenID, x.CuentaDestinoID, x.Monto, x.Tipo))
               .ToList();
    }

    public List<ConsultarTransaccionDto> ListarTransacciones()
    {
        return _transaccionesRepository.ListarTodasTransacciones()
            .Select(x => new ConsultarTransaccionDto(
                x.NumeroTransaccion,
                x.Fecha ?? DateTime.MinValue, // Aquí se maneja el valor nulo
                x.CuentaOrigenID,
                x.CuentaDestinoID,
                x.Monto,
                x.Tipo
            )).ToList();
    }
}
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
    private readonly CuentasRepository _cuentasRepository = new();

    public bool RegistrarTransaccion(RegistrarTransaccionDto transaccionDto)
    {
        // Obtener la cuenta de origen para verificar el saldo
        var cuentaOrigen = _cuentasRepository.ConsultarCuenta(transaccionDto.CuentaOrigenID);

        if (cuentaOrigen == null)
        {
            throw new Exception("La cuenta de origen no existe.");
        }

        // Validaciones de restricciones
        if (transaccionDto.Monto < 1000)
        {
            throw new Exception("El monto de la transacción no puede ser menor a $1.000 COP.");
        }

        if (transaccionDto.Monto > 5000000)
        {
            throw new Exception("El monto de la transacción no puede superar los $5.000.000 COP.");
        }

        if (transaccionDto.Monto > cuentaOrigen.Saldo)
        {
            throw new Exception("Saldo insuficiente para realizar la transacción.");
        }

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
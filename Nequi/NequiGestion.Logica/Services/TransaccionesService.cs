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

    private readonly TransaccionesRepository _transaccionesRepository = new TransaccionesRepository();

    public bool RegistrarTransaccion(RegistrarTransaccionDto transaccionDto)
    {

        var transaccion = new Transacciones
        {
            NumeroTransaccion = transaccionDto.NumeroTransaccion,
            Fecha = DateTime.Now,
            CuentaOrigenID = transaccionDto.CuentaOrigenID,
            CuentaDestinoID = transaccionDto.CuentaDestinoID,
            Monto = transaccionDto.Monto,
            Tipo = transaccionDto.Tipo
        };
        return _transaccionesRepository.RegistrarTransaccion(transaccion);
    }

    public List<ConsultarTransaccionDto> ListarTransacciones()
    {
        return _transaccionesRepository.ListarTransacciones().Select(x => new ConsultarTransaccionDto(x.NumeroTransaccion, x.Fecha, x.CuentaOrigenID, x.CuentaDestinoID, x.Monto, x.Tipo)).ToList();
    }

    public List<ConsultarTransaccionDto> ListarTransaccionesPorCuenta(int NumeroTransaccion)
    {
        return _transaccionesRepository.ListarTransaccionesPorNumero(NumeroTransaccion).Select(x => new ConsultarTransaccionDto(x.NumeroTransaccion, x.Fecha, x.CuentaOrigenID, x.CuentaDestinoID, x.Monto, x.Tipo)).ToList();
    }

    public ConsultarTransaccionDto ConsultarTransaccion(int numeroTransaccion)
    {
        var transaccion = _transaccionesRepository.ConsultarTransaccion(numeroTransaccion);

        if (transaccion != null)
        {
            return new ConsultarTransaccionDto(
                transaccion.NumeroTransaccion,
                transaccion.Fecha,
                transaccion.CuentaOrigenID,
                transaccion.CuentaDestinoID,
                transaccion.Monto,
                transaccion.Tipo);
        }

        return null;
    }

}
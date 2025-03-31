using NequiGestion.Entidades;
using NequiGestion.Logica.Dtos;
using NequiGestion.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NequiGestion.Logica.Services;

public class CuentasService {

    private readonly CuentasRepository _cuentasRepository = new CuentasRepository();

    public bool RegistrarCuenta(RegistroCuentaDto cuentaDto) {


        var cuenta = new Cuentas {

            CuentaID = cuentaDto.CuentaID,
            Nombre = cuentaDto.Nombre,
            Apellido = cuentaDto.Apellido,
            Email = cuentaDto.Email,
            Telefono = cuentaDto.Telefono,
            Saldo = cuentaDto.Saldo,
            FechaCreacion = DateTime.Now,
            ContrasenaHash = cuentaDto.ContrasenaHash

        };
        return _cuentasRepository.RegistrarCuenta(cuenta);
    }

    public List<ConsultaCuentaDto> ListarCuentas()
    {
        return _cuentasRepository.ListarTodasCuentas().Select(x => new ConsultaCuentaDto(x.CuentaID, x.Nombre, x.Apellido, x.Email, x.Telefono, x.Saldo, x.FechaCreacion)).ToList();
    }


    public ConsultaCuentaDto ConsultarCuentaTelefono(string Telefono)
    {
        var cuenta = _cuentasRepository.ConsultarCuentaTelefono(Telefono);

        if (cuenta != null)
        {
            return new ConsultaCuentaDto(
                cuenta.CuentaID,
                cuenta.Nombre,
                cuenta.Apellido,
                cuenta.Email,
                cuenta.Telefono,
                cuenta.Saldo,
                cuenta.FechaCreacion);
        }

        return null;
    }



    public ConsultaCuentaDto ConsultarCuenta(int cuentaID)
    {
        var cuenta = _cuentasRepository.ConsultarCuenta(cuentaID);

        if (cuenta != null)
        {
            return new ConsultaCuentaDto(
                cuenta.CuentaID,
                cuenta.Nombre,
                cuenta.Apellido,
                cuenta.Email,
                cuenta.Telefono,
                cuenta.Saldo,
                cuenta.FechaCreacion);
        }

        return null; 
    }

    
    public bool ActualizarCuenta(ActualizarCuentaDto cuentaDto)
    {
    
        var cuentaExistente = _cuentasRepository.ConsultarCuenta(cuentaDto.CuentaID);

        if (cuentaExistente != null)
        {
            
            cuentaExistente.Nombre = cuentaDto.Nombre;
            cuentaExistente.Apellido = cuentaDto.Apellido;
            cuentaExistente.Email = cuentaDto.Email;
            cuentaExistente.Telefono = cuentaDto.Telefono;
            cuentaExistente.ContrasenaHash = cuentaDto.ContrasenaHash;

            return _cuentasRepository.ActualizarCuenta(cuentaExistente);
        }

        return false; 
    }


    public bool EliminarCuenta(string Telefono)
    {
        var cuenta = _cuentasRepository.ConsultarCuentaTelefono(Telefono);

        if (cuenta == null)
        {
            return false;
        }

        if (cuenta.Saldo > 50000)
        {
            return false; // No se permite eliminar cuentas con más de $50,000 de saldo
        }

        return _cuentasRepository.EliminarCuenta(Telefono);  
    }

    public decimal ObtenerSaldo(string telefono)
    {
        var cuenta = _cuentasRepository.ConsultarCuentaTelefono(telefono);

        if (cuenta == null)
        {
            throw new KeyNotFoundException($"No se encontró la cuenta con telefono {telefono}");
        }

        return cuenta.Saldo;
    }


}

    





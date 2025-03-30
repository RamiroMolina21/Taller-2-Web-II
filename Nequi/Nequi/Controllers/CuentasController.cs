using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NequiGestion.Entidades;
using NequiGestion.Logica.Dtos;
using NequiGestion.Logica.Services;
using System.Linq;

namespace Nequi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CuentasController : ControllerBase {

    private readonly CuentasService _cuentasService = new();

    [HttpPost]
    public ActionResult<bool> RegistrarCuenta(RegistroCuentaDto cuenta)
    {
        return Ok(_cuentasService.RegistrarCuenta(cuenta));

    }

    // Listar todas las cuentas
    [HttpGet]
    public ActionResult<List<ConsultaCuentaDto>> ListarCuentas()
    {
        return Ok(_cuentasService.ListarCuentas());

    }

    [HttpGet("id/{CuentaID}")]
    public ActionResult<List<ConsultaCuentaDto>> ConsultarCuenta(int CuentaID)
    {
        if(CuentaID == null)
        {

            return NotFound("La cuenta no existe PAILAS");

        }
        else
        {
            return Ok(_cuentasService.ConsultarCuenta(CuentaID));
        }

            
    }

    [HttpGet("telefono/{Telefono}")]
    public ActionResult<List<ConsultaCuentaDto>> ConsultarCuentaPorTelefono(string Telefono)
    {
        if (Telefono == null)
        {
            return NotFound("La cuenta no existe PAILAS");
        }
        else
        {
            return Ok(_cuentasService.ConsultarCuentaTelefono(Telefono));
        }

    }

    [HttpDelete("{Telefono}")]
    public ActionResult<List<EliminarCuentaDto>> EliminarCuenta(string Telefono)
    {
        const decimal LIMITE_SALDO = 5000m; // Declarar la constante

        var saldo = _cuentasService.ObtenerSaldo(Telefono);

        if (saldo > LIMITE_SALDO)
        {
            return BadRequest($"No se puede eliminar la cuenta {Telefono} porque su saldo es mayor a {LIMITE_SALDO}.");
        }

        var resultado = _cuentasService.EliminarCuenta(Telefono);
        return Ok(resultado);
    } 

    [HttpPut("{CuentaID}")]
    public ActionResult<bool> ActualizarCuenta(int CuentaID, [FromBody] ActualizarCuentaDto cuentaDto)
    {
        if (cuentaDto.CuentaID != CuentaID)
        {
            return BadRequest("El ID de la cuenta en la URL no coincide con el cuerpo de la solicitud.");
        }

        bool actualizado = _cuentasService.ActualizarCuenta(cuentaDto);

        if (actualizado)
        {
            return Ok("Cuenta actualizada correctamente.");
        }
        else
        {
            return NotFound("La cuenta no existe."); 
        } 
    }

}


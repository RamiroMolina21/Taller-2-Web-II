using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NequiGestion.Entidades;
using NequiGestion.Logica.Dtos;
using NequiGestion.Logica.Exceptions;
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

    [HttpGet]
    public ActionResult<List<ConsultaCuentaDto>> ListarCuentas()
    {
        return Ok(_cuentasService.ListarCuentas());

    }

    [HttpGet("id/{CuentaID}")]
    public ActionResult<ConsultaCuentaDto> ConsultarCuenta(int CuentaID)
    {
        try
        {
            var cuenta = _cuentasService.ConsultarCuenta(CuentaID);

            if (cuenta == null)
            {
                throw new LogicaNegocioException($"No existe una cuenta con el ID {CuentaID}.");
            }

            return Ok(cuenta);
        }
        catch (LogicaNegocioException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("telefono/{Telefono}")]
    public ActionResult<List<ConsultaCuentaDto>> ConsultarCuentaPorTelefono(string Telefono)
    {

        try
        {
            var telefono = _cuentasService.ConsultarCuentaTelefono(Telefono);
            if (telefono == null)
            {
                return NotFound($"No existe una cuenta con el numero {Telefono}.");
            }
            else
            {
                return Ok(_cuentasService.ConsultarCuentaTelefono(Telefono));
            }

        }catch(LogicaNegocioException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor.", error = ex.Message });
        }


    }

    [HttpDelete("{Telefono}")]
    public ActionResult<List<EliminarCuentaDto>> EliminarCuenta(string Telefono)
    {
        const decimal LIMITE_SALDO = 5000m; // Constante de saldo máximo permitido para eliminar una cuenta

        try
        {
            var saldo = _cuentasService.ObtenerSaldo(Telefono);

            if (saldo > LIMITE_SALDO)
            {
                throw new LogicaNegocioException($"No se puede eliminar la cuenta {Telefono} porque su saldo es mayor a {LIMITE_SALDO}.");
            }

            var resultado = _cuentasService.EliminarCuenta(Telefono);
            return Ok(resultado);
        }
        catch (LogicaNegocioException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor.", error = ex.Message });
        }
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


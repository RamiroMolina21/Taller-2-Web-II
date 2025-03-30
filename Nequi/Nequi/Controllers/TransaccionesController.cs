using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NequiGestion.Entidades;
using NequiGestion.Logica.Dtos;
using NequiGestion.Logica.Exceptions;
using NequiGestion.Logica.Services;
using NequiGestion.Persistencia;

namespace Nequi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {

        private readonly TransaccionesService _transaccionesService = new();


        // 🟢 Registrar una transacción
        [HttpPost("registrar")]
        public IActionResult RegistrarTransaccion([FromBody] RegistrarTransaccionDto transaccionDto)
        {
            try
            {
                bool resultado = _transaccionesService.RegistrarTransaccion(transaccionDto);
                if (resultado)
                {
                    return Ok(new { mensaje = "Transacción registrada exitosamente." });
                }
                else
                {
                    return BadRequest(new { mensaje = "Error al registrar la transacción." });
                }
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

        // 🔍 Consultar transacciones de una cuenta
        [HttpGet("consultar/{cuentaID}")]
        public IActionResult ConsultarTransacciones(int cuentaID, [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        {
            try
            {
                var transacciones = _transaccionesService.ConsultarTransacciones(cuentaID, desde, hasta);
                return Ok(transacciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor.", error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<ConsultarTransaccionDto>> ListarTransacciones()
        {
            return Ok(_transaccionesService.ListarTransacciones());
        }

    }
}

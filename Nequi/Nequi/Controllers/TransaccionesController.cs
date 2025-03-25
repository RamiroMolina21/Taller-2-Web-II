using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NequiGestion.Entidades;
using NequiGestion.Logica.Dtos;
using NequiGestion.Logica.Services;
using NequiGestion.Persistencia;

namespace Nequi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {

        private readonly TransaccionesService _transaccionesService = new();
        [HttpPost]
        public IActionResult RegistrarTransaccion(Transacciones transaccion)
        {
            decimal montoMinimo = 1000;
            decimal montoMaximo = 5000000;

            // Validar el monto antes de continuar
            if (transaccion.Monto < montoMinimo || transaccion.Monto > montoMaximo)
            {
                return BadRequest(new { mensaje = $"El monto debe estar entre {montoMinimo:C} y {montoMaximo:C} COP." });
            }

            // Convertir la entidad Transacciones a DTO
            var transaccionDto = new RegistrarTransaccionDto(
                transaccion.NumeroTransaccion,
                transaccion.Fecha,
                transaccion.CuentaOrigenID,
                transaccion.CuentaDestinoID,
                transaccion.Monto,
                transaccion.Tipo
            );

            // Intentar registrar la transacción con el DTO
            bool resultado = _transaccionesService.RegistrarTransaccion(transaccionDto);

            if (!resultado)
            {
                return StatusCode(500, new { mensaje = "No se pudo registrar la transacción." });
            }

            return Ok(new { mensaje = "Transacción registrada con éxito." });
        }  


        [HttpGet]
        public ActionResult<List<ConsultarTransaccionDto>> ListarTransacciones()
        {
            return Ok(_transaccionesService.ListarTransacciones());

        }

        [HttpGet("{NumeroTransaccion}")]
        public ActionResult<List<ConsultarTransaccionDto>> ConsultarTransaccion(int NumeroTransaccion)
        {
            if (NumeroTransaccion == null)
            {

                return NotFound("La cuenta no existe PAILAS");

            }
            else
            {
                return Ok(_transaccionesService.ConsultarTransaccion(NumeroTransaccion));
            }
        }

    }
}

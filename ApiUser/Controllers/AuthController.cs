using ApiUser.DTOS;
using Dominio.Entidades;
using Dominio.Puertos.Primarios;
using Dominio.Services;
using Microsoft.AspNetCore.Mvc;
using ApiUser.Custom;
using Dominio.Excepciones;  // Agregar el namespace de Utils para generar JWT

namespace ApiUser.Controllers
{
    [Route("Auth")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaServicios _cuentaServicios;
        private readonly Utils _utils; // Instanciar la clase Utils para generar el JWT

        public CuentaController(ICuentaServicios cuentaServicios, Utils utils)
        {
            _cuentaServicios = cuentaServicios;
            _utils = utils;
        }

        // Acción de Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCuentaDto dto)
        {
            try
            {
               

                var cuenta = new Cuenta
                {
                    Correo = dto.Correo,
                    Contraseña = dto.Contraseña
                };

                var cuentaValida = await _cuentaServicios.Login(cuenta);

                if (cuentaValida != null)
                {
                    // Generar el token JWT
                    var token = _utils.generarJwt(cuentaValida);

                    // Establecer el token en la cookie
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,         // La cookie solo será accesible por HTTP, no a través de JavaScript
                        Secure = true,           // Solo se enviará sobre HTTPS
                        SameSite = SameSiteMode.Strict, // Cambiar a SameSiteMode.Lax si tienes problemas con el envío de cookies
                        Expires = DateTime.UtcNow.AddHours(1) // Define el tiempo de expiración del token
                    };

                    // Establecer la cookie "jwt" con el token JWT
                    Response.Cookies.Append("jwt", token, cookieOptions);

                    return Ok();
                }
                else
                {
                    return Unauthorized("Credenciales incorrectas."); // Si no se encuentran las credenciales
                }
            }
            catch (ValidacionException ex)
            {
                return BadRequest(new { errores = ex.Errores });

            }
            catch (Exception ex) 
            {
                return StatusCode(500, "Error interno del servidor.");

            }

        }

        // Acción de Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCuentaDto dto)
        {
            try
            {
                var cuenta = new Cuenta
                {
                    Correo = dto.Correo,
                    Contraseña = dto.Contraseña,  // Asegúrate de que la contraseña esté cifrada antes de llegar aquí
                    Rol = dto.Rol
                };

                await _cuentaServicios.Register(cuenta);

                return CreatedAtAction(nameof(Register), new { correo = cuenta.Correo }, cuenta); // Devuelve un código de estado 201
            }
            catch (ValidacionException ex) 
            {
                return BadRequest(new { errores = ex.Errores });

            }
            catch (CuentaDuplicadaException ex)
            {
                // Loguear la excepción si es necesario (en un log o sistema de monitoreo)
                return Conflict(new { mensaje = ex.Message }); // 409 Conflict: el usuario ya existe
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error interno del servidor."+ex.Message);

            }

        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Eliminar la cookie "jwt"
            Response.Cookies.Delete("jwt");

            return Ok(new { message = "Has cerrado sesión correctamente." });
        }
    }
}

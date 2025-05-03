using ApiUser.DTOS;
using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.Puertos.Primarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiUser.Controllers
{
    [Route("Usuarios")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UsuarioController : ControllerBase
    {
        private readonly ICommonServices<Usuario> _services;

        public UsuarioController(ICommonServices<Usuario> services)
        {
            _services = services;
        }

        // POST: Crear un nuevo usuario
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearUsuarioDTo dto)
        {
            try
            {
                // Validación de datos de entrada
                if (!ModelState.IsValid)
                    return BadRequest(ModelState); // 400 si el modelo es inválido

                // Crear el nuevo usuario
                var usuario = new Usuario
                {
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Cedula = dto.Cedula,
                    Direccion = dto.Direccion
                };

                // Llamar al servicio para agregar el usuario
                await _services.Agregar(usuario);

                // Redirigir al recurso creado
                return CreatedAtAction(nameof(ObtenerUsuario), new { idUsuario = usuario.IdUsuario }, usuario);
            }
            catch(ValidacionException ex)
            {
                return BadRequest(new { errores = ex.Errores });
            }
            catch (UsuarioDuplicadoException ex)
            {
                // Loguear la excepción si es necesario (en un log o sistema de monitoreo)
                return Conflict(new { mensaje = ex.Message }); // 409 Conflict: el usuario ya existe
            }
            catch (Exception ex)
            {
                // Error interno del servidor
                return StatusCode(500, "Error interno del servidor."+ex.Message);
            }
        }

        // GET: Obtener un usuario por ID
        [HttpGet("{idUsuario}")]
        public async Task<IActionResult> ObtenerUsuario(int idUsuario)
        {
            try
            {
                // Obtener el usuario por ID
                var usuario = await _services.Obtener(idUsuario);
                if (usuario == null)
                    return NotFound(); // 404 si no se encuentra el usuario

                return Ok(usuario); // 200 OK con el recurso
            }
            catch (Exception ex)
            {
                // Error interno del servidor
                return StatusCode(500, "Error interno del servidor."+ex.Message);
            }
        }

        // GET: Obtener todos los usuarios
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                // Obtener todos los usuarios
                var usuarios = await _services.ObtenerTodos();

                if (usuarios == null || !usuarios.Any())
                    return NoContent(); // 204 No Content si no hay usuarios

                return Ok(usuarios); // 200 OK con la lista de usuarios
            }
            catch (Exception ex)
            {
                // Error interno del servidor
                return StatusCode(500, "Error interno del servidor."+ex.Message);
            }
        }

        // PUT: Editar un usuario existente
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] EditarUsuarioDto dto)
        {
            try
            {
                // Obtener el usuario por ID
                var usuarioExistente = await _services.Obtener(dto.IdUsuario);
                if (usuarioExistente == null)
                    return NotFound(); // 404 si no se encuentra el usuario

                // Actualizar los datos del usuario
                usuarioExistente.Nombre = dto.Nombre;
                usuarioExistente.Apellido = dto.Apellido;
                usuarioExistente.Cedula = dto.Cedula;
                usuarioExistente.Direccion = dto.Direccion;

                // Llamar al servicio para actualizar el usuario
                await _services.Editar(usuarioExistente);

                // Devolver respuesta de éxito sin contenido
                return NoContent(); // 204 No Content
            }
            catch (ValidacionException ex)
            {
                return BadRequest(new { errores = ex.Errores });
            }
            catch (UsuarioDuplicadoException ex)
            {
                // Loguear la excepción si es necesario (en un log o sistema de monitoreo)
                return Conflict(new { mensaje = ex.Message }); // 409 Conflict: el usuario ya existe
            }
            catch (Exception ex)
            {
                // Error interno del servidor
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // DELETE: Eliminar un usuario por ID
        [HttpDelete("{idUsuario}")]
        public async Task<IActionResult> Eliminar(int idUsuario)
        {
            try
            {
                // Obtener el usuario por ID
                var usuarioExistente = await _services.Obtener(idUsuario);
                if (usuarioExistente == null)
                    return NotFound(); // 404 si no se encuentra el usuario

                // Llamar al servicio para eliminar el usuario
                await _services.Eliminar(idUsuario);

                // Devolver respuesta de éxito sin contenido
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                // Error interno del servidor
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}

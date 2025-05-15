using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.Puertos.Secundarios;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace Infrastructura.Repositorios
{
    public class UserRepository : ICommonRepository<Usuario>
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public async Task Agregar(Usuario usuario)
        {
            try
            {
                bool existeCedula = await _context.Set<Usuario>()
                    .AnyAsync(u => u.Cedula == usuario.Cedula);

                if (existeCedula)
                {
                    throw new UsuarioDuplicadoException();
                }

                _context.Set<Usuario>().Add(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error al agregar usuario en la base de datos", ex);
            }
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodos()
        {
            try
            {
                return await _context.Set<Usuario>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todos los usuarios de la base de datos", ex);
            }
        }



        public async Task<Usuario?> Obtener(int idUsuario)
        {
            try
            {
                return await _context.Set<Usuario>()
                                     .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener usuario en la base de datos", ex);
            }
        }

        public async Task Editar(Usuario usuario)
        {
            try
            {
                var usuarioExistente = await _context.Set<Usuario>()
                                                     .FirstOrDefaultAsync(u => u.IdUsuario == usuario.IdUsuario);

                if (usuarioExistente == null)
                    throw new Exception("Usuario no encontrado");

                // Verificamos duplicado por cédula (excepto él mismo)
                bool cedulaDuplicada = await _context.Set<Usuario>()
                    .AnyAsync(u => u.Cedula == usuario.Cedula && u.IdUsuario != usuario.IdUsuario);

                if (cedulaDuplicada)
                    throw new UsuarioDuplicadoException();

                // Actualizamos propiedades
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.Apellido = usuario.Apellido;
                usuarioExistente.Cedula = usuario.Cedula;
                usuarioExistente.Direccion = usuario.Direccion;

                _context.Update(usuarioExistente);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar usuario en la base de datos", ex);
            }
        }


        public async Task Eliminar(int idUsuario)
        {
            try
            {
                var usuario = await _context.Set<Usuario>()
                                            .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);

                if (usuario == null)
                    throw new Exception("Usuario no encontrado");

                _context.Set<Usuario>().Remove(usuario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar usuario en la base de datos", ex);
            }
        }

    }
}

using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.Puertos.Secundarios;
using Microsoft.Data.SqlClient;
using System.Data;


namespace Infrastructura.Repositorios
{
    public class UserRepository : ICommonRepository<Usuario>
    {
        private readonly Conexion _conexion;

        public UserRepository(Conexion conexion)
        {
            _conexion = conexion;
        }

        public async Task Agregar(Usuario usuario)
        {
            try
            {

                using (var con = _conexion.GetConnection())
                {

                    await con.OpenAsync();

                    using (var cmd = new SqlCommand("sp_CrearUsuario", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
                        cmd.Parameters.AddWithValue("@cedula", usuario.Cedula);
                        cmd.Parameters.AddWithValue("@direccion", usuario.Direccion);

                        await cmd.ExecuteNonQueryAsync();


                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // 2627 es el código de error para violación de clave única en SQL Server
                {
                    throw new UsuarioDuplicadoException(); // Lanzamos una excepción personalizada
                }

                // Si no es un error de duplicado, lanzamos una excepción genérica
                throw new Exception("Error al agregar usuario en la base de datos", ex);
            }
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodos()
        {
            try
            {
                var usuarios = new List<Usuario>();

                using (var con = _conexion.GetConnection())
                {
                    await con.OpenAsync();

                    using (var cmd = new SqlCommand("sp_ObtenerUsuarios", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                usuarios.Add(new Usuario()
                                {
                                    IdUsuario = Convert.ToInt32(reader["idUsuario"]),
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido = reader["apellido"].ToString(),
                                    Cedula = reader["cedula"].ToString(),
                                    Direccion = reader["direccion"].ToString()
                                });
                            }

                            return usuarios;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todos los usuario de la base de datos", ex);
            }
        }

        public async Task<Usuario> Obtener(int idUsuario)
        {
            try
            {
                var usuarios = new List<Usuario>();

                using (var con = _conexion.GetConnection())
                {
                    await con.OpenAsync();

                    using (var cmd = new SqlCommand("sp_ObtenerUsuario", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);


                        using (var reader = await cmd.ExecuteReaderAsync())
                        {

                            if (await reader.ReadAsync())
                            {
                                return new Usuario
                                {
                                    IdUsuario = Convert.ToInt32(reader["idUsuario"]),
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido = reader["apellido"].ToString(),
                                    Cedula = reader["cedula"].ToString(),
                                    Direccion = reader["direccion"].ToString()
                                };
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
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
                using (var con = _conexion.GetConnection())
                {
                    await con.OpenAsync();

                    using (var cmd = new SqlCommand("sp_EditarUsuario", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idUsuario", usuario.IdUsuario);
                        cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
                        cmd.Parameters.AddWithValue("@cedula", usuario.Cedula);
                        cmd.Parameters.AddWithValue("@direccion", usuario.Direccion);

                        await cmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // 2627 es el código de error para violación de clave única en SQL Server
                {
                    throw new UsuarioDuplicadoException(); // Lanzamos una excepción personalizada
                }

                // Si no es un error de duplicado, lanzamos una excepción genérica
                throw new Exception("Error al agregar usuario en la base de datos", ex);
            }
        }

        public async Task Eliminar(int idUsuario)
        {
            try
            {
                using (var con = _conexion.GetConnection())
                {
                    await con.OpenAsync();

                    using (var cmd = new SqlCommand("sp_EliminarUsuario", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idUsuario",idUsuario);
              
                        await cmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar usuario en la base de datos", ex);
            }
        }
    }
}

using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.Puertos.Secundarios;
using Infrastructura.Repositorios.Custom;  // Agregar el namespace para la clase Utils
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infrastructura.Repositorios
{
    public class CuentaRepositorio : ICuentaRepositorio
    {
        private readonly Conexion _conexion;

        public CuentaRepositorio(Conexion conexion)
        {
            _conexion = conexion;
        }

        // Validar credenciales - Consume el procedimiento almacenado sp_ValidarCredenciales
        public async Task<Cuenta?> ValidarCredenciales(Cuenta cuenta)
        {
            using (var con = _conexion.GetConnection())  // Obtén la conexión
            {
                await con.OpenAsync();

                // Configurando el comando para ejecutar el SP
                using (var cmd = new SqlCommand("sp_ValidarCredenciales", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Cifrar la contraseña antes de enviarla
                    string contraseñaCifrada = Utils.EncryptSHA256(cuenta.Contraseña);

                    // Agregar los parámetros al procedimiento almacenado
                    cmd.Parameters.AddWithValue("@Correo", cuenta.Correo);
                    cmd.Parameters.AddWithValue("@Contraseña", contraseñaCifrada);

                    // Ejecutar el procedimiento almacenado y leer el resultado
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())  // Si se encuentran credenciales válidas
                        {
                            // Retornar la cuenta con los datos que devuelve el SP
                            return new Cuenta
                            {
                                IdCuenta = Convert.ToInt32(reader["IdCuenta"]),
                                Correo = reader["Correo"].ToString(),
                                Rol = reader["Rol"].ToString()
                            };
                        }
                        else
                        {
                            // Si las credenciales son incorrectas, retorna null
                            return null;
                        }
                    }
                }
            }
        }

        // Registro de cuenta - Consume el procedimiento almacenado sp_Register
        public async Task Register(Cuenta cuenta)
        {
            try
            {
                using (var con = _conexion.GetConnection())  // Obtén la conexión
                {
                    await con.OpenAsync();

                    // Configurando el comando para ejecutar el SP
                    using (var cmd = new SqlCommand("sp_Register", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Cifrar la contraseña antes de enviarla
                        string contraseñaCifrada = Utils.EncryptSHA256(cuenta.Contraseña);

                        // Agregar los parámetros al procedimiento almacenado
                        cmd.Parameters.AddWithValue("@Correo", cuenta.Correo);
                        cmd.Parameters.AddWithValue("@Contraseña", contraseñaCifrada);  // Usar contraseña cifrada
                        cmd.Parameters.AddWithValue("@Rol", cuenta.Rol);

                        // Ejecutar el procedimiento almacenado
                        await cmd.ExecuteNonQueryAsync();  // Ejecutar el procedimiento sin necesidad de leer resultados

                    }
                }
            }
            catch (SqlException ex)
            {

                if (ex.Number == 2627) // 2627 es el código de error para violación de clave única en SQL Server
                {
                    throw new CuentaDuplicadaException(); // Lanzamos una excepción personalizada
                }

                // Si no es un error de duplicado, lanzamos una excepción genérica
                Console.WriteLine(ex.Message);

                throw new Exception("Error al agregar usuario en la base de datos"+ex.Message, ex);
            }

        }
    }
}

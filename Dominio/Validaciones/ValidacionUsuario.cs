using Dominio.Entidades;
using Dominio.Excepciones;

namespace Dominio.Validaciones
{
    public class ValidacionUsuario
    {
        public static void Validar(Usuario usuario)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                errores.Add("El nombre no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(usuario.Apellido))
                errores.Add("El apellido no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(usuario.Cedula))
                errores.Add("La cédula no puede estar vacía.");
            else if (usuario.Cedula.Length < 10)
                errores.Add("La cédula debe tener al menos 10 dígitos.");

            if (string.IsNullOrWhiteSpace(usuario.Direccion))
                errores.Add("La dirección no puede estar vacía.");

            if (errores.Any())
                throw new ValidacionException(errores);
        }
    }
}

using Dominio.Entidades;
using Dominio.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dominio.Validaciones
{
    public class ValidacionCuenta
    {
        public static void ValidarRegistro(Cuenta cuenta)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(cuenta.Correo) || !EsCorreoValido(cuenta.Correo))
                errores.Add("El correo no tiene un formato válido.");

            if (string.IsNullOrWhiteSpace(cuenta.Contraseña) || !EsContraseñaValida(cuenta.Contraseña))
                errores.Add("La contraseña no es válida. Debe tener al menos 8 caracteres, incluir una mayúscula, una minúscula y un carácter especial.");

            if (string.IsNullOrWhiteSpace(cuenta.Rol) || (cuenta.Rol != "admin" && cuenta.Rol != "user"))
                errores.Add("El rol debe ser 'admin' o 'user'.");

            if (errores.Any())
                throw new ValidacionException(errores);
        }

        public static void ValidarLogin(Cuenta cuenta)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(cuenta.Correo) || !EsCorreoValido(cuenta.Correo))
                errores.Add("El correo no tiene un formato válido.");

            if (string.IsNullOrWhiteSpace(cuenta.Contraseña) || !EsContraseñaValida(cuenta.Contraseña))
                errores.Add("La contraseña no tiene un formato valido");

            if(errores.Any())
                throw new ValidacionException(errores);
        }


        // Validar si el correo es válido usando una expresión regular
        private static bool EsCorreoValido(string correo)
        {
            // Expresión regular para validar correo electrónico
            var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return regex.IsMatch(correo);
        }

        // Validar si la contraseña cumple con los requisitos
        private static bool EsContraseñaValida(string contraseña)
        {
            // La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula y un carácter especial
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return regex.IsMatch(contraseña);
        }
    }
}

using System.Text.RegularExpressions;

namespace Dominio.Entidades
{
    public class Cuenta
    {
        public int IdCuenta {  get; set; }
        public string Correo {  get; set; }
        public string Contraseña { get; set; }
        public string Rol {  get; set; }   
    }
}

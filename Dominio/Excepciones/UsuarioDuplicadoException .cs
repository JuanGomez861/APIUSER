using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Excepciones
{
    public class UsuarioDuplicadoException : Exception
    {
        public UsuarioDuplicadoException()
            : base($"ya existe una usuario registrado con esa cedula")
        {
        }
    }
}

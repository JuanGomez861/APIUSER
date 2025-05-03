using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Excepciones
{
    
        public class CuentaDuplicadaException : Exception
        {
            public CuentaDuplicadaException()
                : base($"ya existe una cuenta registrada con ese email")
            {
            }
        }
    
}

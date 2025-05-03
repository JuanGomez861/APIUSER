using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Puertos.Primarios
{
    public interface ICuentaServicios
    {
        Task<Cuenta?> Login(Cuenta cuenta);
        Task Register(Cuenta cuenta);
    }
}

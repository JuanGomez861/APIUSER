using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Puertos.Secundarios
{
    public interface ICuentaRepositorio
    {
        Task<Cuenta?> ValidarCredenciales(Cuenta cuenta);
        Task Register(Cuenta cuenta);
    }
}



using Dominio.Entidades;
using Dominio.Puertos.Primarios;
using Dominio.Puertos.Secundarios;
using Dominio.Validaciones;

namespace Dominio.Services
{
    public class ServicioCuenta : ICuentaServicios
    {
        private readonly ICuentaRepositorio _repository;

        public ServicioCuenta(ICuentaRepositorio repository)
        {
            _repository = repository;
        }

        public async Task<Cuenta?> Login(Cuenta cuenta)
        {
            ValidacionCuenta.ValidarLogin(cuenta);
            return await _repository.ValidarCredenciales(cuenta);
        }

        public async Task Register(Cuenta cuenta)
        {
            ValidacionCuenta.ValidarRegistro(cuenta);
             await _repository.Register(cuenta);
        }
    }
}

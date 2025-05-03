using Dominio.Entidades;
using Dominio.Puertos.Primarios;
using Dominio.Puertos.Secundarios;
using Dominio.Validaciones;

namespace Dominio.Services
{
    public class ServiciosUsuario : ICommonServices<Usuario>
    {
        private readonly ICommonRepository<Usuario> _repositorio;

        public ServiciosUsuario(ICommonRepository<Usuario> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task Agregar(Usuario usuario) 
        {
            
                ValidacionUsuario.Validar(usuario);
               await _repositorio.Agregar(usuario);
           
            
        }
        public async Task <IEnumerable<Usuario>> ObtenerTodos()
        {
            return await _repositorio.ObtenerTodos();
        }

        public async Task <Usuario> Obtener(int id)
        {
            return await _repositorio.Obtener(id);
        }

        public async Task Editar(Usuario usuario)
        {
            ValidacionUsuario.Validar(usuario);

            await _repositorio.Editar(usuario);
        }

        public async Task Eliminar(int id)
        {
            await _repositorio.Eliminar(id);
        }



    }
}

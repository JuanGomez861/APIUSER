using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Puertos.Secundarios
{
    public interface ICommonRepository<TEntity>
    {
        Task Agregar(TEntity entity);
        Task <IEnumerable<TEntity>> ObtenerTodos();
        Task <TEntity> Obtener(int id);
        Task Editar(TEntity entity);
        Task Eliminar(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Puertos.Primarios
{
    public interface ICommonServices<TEntity>
    {
        Task Agregar(TEntity entity);
        Task <IEnumerable<TEntity>> ObtenerTodos();
        Task <TEntity> Obtener(int id);
        Task Editar(TEntity entity);
        Task Eliminar(int id);

    }
}


namespace Dominio.Excepciones
{
    public class ValidacionException : Exception
    {
        public List<string> Errores { get; }

        public ValidacionException(List<string> errores)
            : base("Uno o más errores de validación ocurrieron.")
        {
            Errores = errores;
        }
    }
}

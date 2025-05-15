using Dominio.Entidades;
using Dominio.Excepciones;
using Dominio.Puertos.Secundarios;
using Infrastructura.Repositorios.Custom;
using Microsoft.EntityFrameworkCore;

namespace Infrastructura.Repositorios
{
    public class CuentaRepositorio : ICuentaRepositorio
    {
        private readonly Context _context;

        public CuentaRepositorio(Context context)
        {
            _context = context;
        }

        // Validar credenciales sin SP
        public async Task<Cuenta?> ValidarCredenciales(Cuenta cuenta)
        {
            string contraseñaCifrada = Utils.EncryptSHA256(cuenta.Contraseña);

            return await _context.Set<Cuenta>()
                .Where(c => c.Correo == cuenta.Correo && c.Contraseña == contraseñaCifrada)
                .Select(c => new Cuenta
                {
                    IdCuenta = c.IdCuenta,
                    Correo = c.Correo,
                    Rol = c.Rol
                })
                .FirstOrDefaultAsync();
        }

        // Registro de cuenta sin SP
        public async Task Register(Cuenta cuenta)
        {
            string contraseñaCifrada = Utils.EncryptSHA256(cuenta.Contraseña);

            bool existeCorreo = await _context.Set<Cuenta>().AnyAsync(c => c.Correo == cuenta.Correo);

            if (existeCorreo)
            {
                throw new CuentaDuplicadaException(); // correo ya está registrado
            }

            cuenta.Contraseña = contraseñaCifrada;

            _context.Set<Cuenta>().Add(cuenta);
            await _context.SaveChangesAsync();
        }
    }
}

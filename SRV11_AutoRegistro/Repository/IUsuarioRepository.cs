using SRV11_AutoRegistro.Entities;

namespace SRV11_AutoRegistro.Repository;

public interface IUsuarioRepository
{
    Task<int> Create(Usuario usuario);

    Task<Usuario?> GetByEmail(string email);

    Task<Usuario?> GetByToken(string token);

    Task<bool> ConfirmarCuenta(string token);

    Task<bool> ExistsByEmail(string email);

}
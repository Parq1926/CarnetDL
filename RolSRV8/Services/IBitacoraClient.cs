namespace RolSRV8.Services;

public interface IBitacoraClient
{
    Task RegistrarAsync(
        string usuario,
        string accion,
        string detalleJson);
}
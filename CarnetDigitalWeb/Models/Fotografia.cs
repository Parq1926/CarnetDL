namespace CarnetDigitalWeb.Models
{
    // Resultado que devuelve SRV13 al actualizar/eliminar
    public class FotografiaUsuario
    {
        public int UsuarioId { get; set; }
        public string? FotografiaBase64 { get; set; }
    }
}

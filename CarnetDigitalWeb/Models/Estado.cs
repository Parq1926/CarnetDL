namespace CarnetDigitalWeb.Models
{
    // Resultado que devuelve SRV12 al cambiar el estado
    public class UsuarioEstado
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public int EstadoId { get; set; }
        public string Estado { get; set; } = null!;
    }
}

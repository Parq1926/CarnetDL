namespace BitacoraSRV9.Entities;

public class BitacoraFiltrosRequest
{
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? Usuario { get; set; }
    public string? Accion { get; set; }

    public int Pagina { get; set; } = 1;

    public int TamanoPagina { get; set; } = 15;

    public bool? SoloErrores { get; set; } = false;
}
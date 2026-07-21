namespace BitacoraSRV9.Entities;

public class BitacoraPaginadaResponse
{
    public IEnumerable<Bitacora> Registros { get; set; } = new List<Bitacora>();
    public int PaginaActual { get; set; }
    public int TamanoPagina { get; set; }
    public int TotalRegistros { get; set; }
    public int TotalPaginas { get; set; }
    public bool TieneAnterior => PaginaActual > 1;
    public bool TieneSiguiente => PaginaActual < TotalPaginas;
}
namespace BitacoraSRV9.Entities;

public class BitacoraRequest
{
    public string Usuario { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public string? DetalleJson { get; set; } 
    public bool EsError { get; set; } = false; 
}
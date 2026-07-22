// Helpers/BitacoraHelper.cs
using System.Text.Json;

namespace BitacoraSRV9.Helpers;

public static class BitacoraHelper
{
    // Para creación de registros
    public static string CrearJsonCreacion(object nuevoRegistro)
    {
        return JsonSerializer.Serialize(new
        {
            tipo = "CREACION",
            nuevo = nuevoRegistro,
            fecha = DateTime.Now
        }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Para actualización de registros
    public static string CrearJsonActualizacion(object registroAnterior, object registroActual)
    {
        return JsonSerializer.Serialize(new
        {
            tipo = "ACTUALIZACION",
            anterior = registroAnterior,
            actual = registroActual,
            fecha = DateTime.Now
        }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Para eliminación de registros
    public static string CrearJsonEliminacion(object registroEliminado)
    {
        return JsonSerializer.Serialize(new
        {
            tipo = "ELIMINACION",
            eliminado = registroEliminado,
            fecha = DateTime.Now
        }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Para errores técnicos
    public static string CrearJsonError(string mensaje, string? stackTrace = null)
    {
        return JsonSerializer.Serialize(new
        {
            tipo = "ERROR",
            mensaje = mensaje,
            stackTrace = stackTrace,
            fecha = DateTime.Now
        }, new JsonSerializerOptions { WriteIndented = true });
    }
}
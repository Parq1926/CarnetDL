using Dapper;
using BitacoraSRV9.Entities;

namespace BitacoraSRV9.Repository;

public class BitacoraRepository
{
    private readonly IDbConnectionFactory _db;

    public BitacoraRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<int> GuardarAsync(Bitacora bitacora)
    {
        using var conn = _db.CreateConnection();

        return await conn.ExecuteAsync(
        @"
        INSERT INTO BITACORA
        (
            USUARIO,
            ACCION,
            DETALLE_JSON,
            ES_ERROR,
            FECHA
        )
        VALUES
        (
            @Usuario,
            @Accion,
            @DetalleJson,
            @EsError,
            @Fecha
        )",
        bitacora);
    }

    public async Task<IEnumerable<Bitacora>> ObtenerTodosAsync()
    {
        using var conn = _db.CreateConnection();

        return await conn.QueryAsync<Bitacora>(
        @"
        SELECT
            ID AS Id,
            USUARIO AS Usuario,
            ACCION AS Accion,
            DETALLE_JSON AS DetalleJson,
            ES_ERROR AS EsError,
            FECHA AS Fecha
        FROM BITACORA
        ORDER BY FECHA DESC");
    }

    public async Task<BitacoraPaginadaResponse> ObtenerConFiltrosAsync(BitacoraFiltrosRequest filtros)
    {
        using var conn = _db.CreateConnection();

        // Construir la consulta WHERE dinámicamente
        var condiciones = new List<string>();
        var parametros = new DynamicParameters();

        if (filtros.FechaInicio.HasValue)
        {
            condiciones.Add("FECHA >= @FechaInicio");
            parametros.Add("FechaInicio", filtros.FechaInicio.Value);
        }

        if (filtros.FechaFin.HasValue)
        {
            condiciones.Add("FECHA <= @FechaFin");
            parametros.Add("FechaFin", filtros.FechaFin.Value);
        }

        if (!string.IsNullOrWhiteSpace(filtros.Usuario))
        {
            condiciones.Add("USUARIO LIKE @Usuario");
            parametros.Add("Usuario", $"%{filtros.Usuario}%");
        }

        if (!string.IsNullOrWhiteSpace(filtros.Accion))
        {
            condiciones.Add("ACCION LIKE @Accion");
            parametros.Add("Accion", $"%{filtros.Accion}%");
        }

        if (filtros.SoloErrores == true)
        {
            condiciones.Add("ES_ERROR = 1");
        }

        var whereClause = condiciones.Count > 0 ? $"WHERE {string.Join(" AND ", condiciones)}" : "";

        // Consulta para contar el total
        var countSql = $@"
            SELECT COUNT(*)
            FROM BITACORA
            {whereClause}";

        var totalRegistros = await conn.ExecuteScalarAsync<int>(countSql, parametros);

        // Consulta para obtener los datos paginados
        var offset = (filtros.Pagina - 1) * filtros.TamanoPagina;
        var sql = $@"
            SELECT
                ID AS Id,
                USUARIO AS Usuario,
                ACCION AS Accion,
                DETALLE_JSON AS DetalleJson,
                ES_ERROR AS EsError,
                FECHA AS Fecha
            FROM BITACORA
            {whereClause}
            ORDER BY FECHA DESC
            OFFSET @Offset ROWS
            FETCH NEXT @TamanoPagina ROWS ONLY";

        parametros.Add("Offset", offset);
        parametros.Add("TamanoPagina", filtros.TamanoPagina);

        var registros = await conn.QueryAsync<Bitacora>(sql, parametros);

        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / filtros.TamanoPagina);

        return new BitacoraPaginadaResponse
        {
            Registros = registros,
            PaginaActual = filtros.Pagina,
            TamanoPagina = filtros.TamanoPagina,
            TotalRegistros = totalRegistros,
            TotalPaginas = totalPaginas
        };
    }
}
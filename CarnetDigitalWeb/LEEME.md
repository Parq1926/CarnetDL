# CarnetDigitalWeb

Proyecto Razor Pages (.NET 10) que consume mis 4 microservicios:

| Pantalla | HU | Microservicio | Ruta base |
|---|---|---|---|
| Cambio de Estado | Web11 | SRV12 | `/EstadoUsuario` |
| Código QR | Web9 | SRV14 | `/CarnetQR` |
| Fotografía | Web8 | SRV13 | `/Fotografia` |
| Parámetros | Web17 | SRV15 | `/Parametro` |

Todas cuelgan de `https://tiusr22pl.cuc-carrera-ti.ac.cr` (configurable en `appsettings.json`, clave `MicroservicioBase`).

## Cómo abrirlo

1. Abrir `CarnetDigitalWeb.csproj` en Visual Studio (o `dotnet run` desde la carpeta).
2. F5 / Iniciar sin depurar.
3. En la pantalla, arriba a la derecha, pegar el token (Bearer) que devuelve el login del SRV1 y darle "Guardar". No hay pantalla de login aquí: el token se escribe a mano y queda en la sesión del servidor.
4. Navegar por el sidebar entre las 4 pantallas.

## Estructura

- `Models/` - DTOs iguales a los de cada microservicio.
- `Services/` - un servicio por microservicio (interfaz + implementación), usa `IHttpClientFactory` con un `HttpClient` nombrado por servicio.
- `Pages/` - una carpeta por pantalla (`EstadoUsuario`, `Qr`, `Fotografia`, `Parametro`), Razor Pages con code-behind.
- `Program.cs` - registra los `HttpClient`, la sesión, y el endpoint mínimo `POST /set-token` que guarda el token en sesión.

No incluye Web1-Web7, Web10, Web12-Web16: esas no son responsabilidad de este equipo/parte.

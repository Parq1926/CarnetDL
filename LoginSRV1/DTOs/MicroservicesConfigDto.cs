namespace LoginSRV1.DTOs
{
    public class MicroservicesConfigDto
    {
        public MicroserviceConfig UsuariosSRV4 { get; set; } = new();
        public MicroserviceConfig TiposUsuarioSRV5 { get; set; } = new();
        public MicroserviceConfig TipoIdentificacionSRV6 { get; set; } = new();
        public MicroserviceConfig CarrerasSRV3 { get; set; } = new();
        public MicroserviceConfig AreasSRV4 { get; set; } = new();
        public MicroserviceConfig InstitucionesSRV { get; set; } = new();
        public MicroserviceConfig FotografiasSRV { get; set; } = new();
        public MicroserviceConfig EstadoUsuarioSRV { get; set; } = new();
        public MicroserviceConfig GenerarQRSRV { get; set; } = new();
        public MicroserviceConfig ModulosSRV { get; set; } = new();
        public MicroserviceConfig RolesSRV { get; set; } = new();
        public MicroserviceConfig BitacorasSRV { get; set; } = new();
        public MicroserviceConfig ParametrosSRV { get; set; } = new();
    }

    public class MicroserviceConfig
    {
        public string Url { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;

        public string FullUrl => Url + Path;
    }
}
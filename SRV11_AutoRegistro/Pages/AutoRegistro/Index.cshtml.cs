using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SRV11_AutoRegistro.Services;
using SRV11_AutoRegistro.Entities;
using SRV11_AutoRegistro.DTOs;

namespace SRV11_AutoRegistro.Pages.AutoRegistro
{
    public class IndexModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ITipoUsuarioService _tipoUsuarioService;
        private readonly ITipoIdentificacionService _tipoIdentificacionService;
        private readonly ICarreraService _carreraService;
        private readonly IInstitucionService _institucionService;
        private readonly IAreaService _areaService;
        private readonly IConfiguration _configuration;
        private readonly IRolService _rolService;

        public IndexModel(
            IUsuarioService usuarioService,
            ITipoUsuarioService tipoUsuarioService,
            ITipoIdentificacionService tipoIdentificacionService,
            ICarreraService carreraService,
            IInstitucionService institucionService,
            IAreaService areaService,
            IConfiguration configuration,
            IRolService rolService)
        {
            _usuarioService = usuarioService;
            _tipoUsuarioService = tipoUsuarioService;
            _tipoIdentificacionService = tipoIdentificacionService;
            _carreraService = carreraService;
            _institucionService = institucionService;
            _areaService = areaService;
            _configuration = configuration;
            _rolService = rolService;
        }

        public string TipoUsuarioUrl { get; private set; } = string.Empty;
        public string TipoIdentificacionUrl { get; private set; } = string.Empty;
        public string InstitucionUrl { get; private set; } = string.Empty;
        public string CarreraUrl { get; private set; } = string.Empty;
        public string AreaUrl { get; private set; } = string.Empty;

        public void OnGet()
        {
            TipoUsuarioUrl = _configuration["Services:TipoUsuario"] ?? "";
            TipoIdentificacionUrl = _configuration["Services:TipoIdentificacion"] ?? "";
            InstitucionUrl = _configuration["Services:Institucion"] ?? "";
            CarreraUrl = _configuration["Services:Carrera"] ?? "";
            AreaUrl = _configuration["Services:Area"] ?? "";
        }

        public async Task<IActionResult> OnGetTiposUsuario()
        {
            try
            {
                var tipos = await _tipoUsuarioService.GetAll();
                return new JsonResult(tipos);
            }
            catch
            {
                return new JsonResult(new List<TipoUsuarioDto>());
            }
        }

        public async Task<IActionResult> OnGetTiposIdentificacion()
        {
            try
            {
                var tipos = await _tipoIdentificacionService.GetAll();
                return new JsonResult(tipos);
            }
            catch
            {
                return new JsonResult(new List<TipoIdentificacionDto>());
            }
        }

        public async Task<IActionResult> OnGetCarreras()
        {
            try
            {
                var carreras = await _carreraService.GetAll();
                return new JsonResult(carreras);
            }
            catch
            {
                return new JsonResult(new List<CarreraDto>());
            }
        }

        public async Task<IActionResult> OnGetInstituciones()
        {
            try
            {
                var instituciones = await _institucionService.GetAll();
                return new JsonResult(instituciones);
            }
            catch
            {
                return new JsonResult(new List<InstitucionDto>());
            }
        }

        public async Task<IActionResult> OnGetAreas()
        {
            try
            {
                var areas = await _areaService.GetAll();
                return new JsonResult(areas);
            }
            catch
            {
                return new JsonResult(new List<AreaDto>());
            }
        }

        public async Task<IActionResult> OnGetRoles()
        {
            try
            {
                var roles = await _rolService.GetAll();
                return new JsonResult(roles);
            }
            catch
            {
                return new JsonResult(new List<Rol>());
            }
        }

        public async Task<IActionResult> OnPostRegistrar([FromBody] RegistroUsuarioDto registro)
        {
            try
            {
                var usuario = new Usuario
                {
                    TipoUsuarioId = registro.TipoUsuarioId,
                    TipoIdentificacionId = registro.TipoIdentificacionId,
                    NumeroIdentificacion = registro.NumeroIdentificacion,
                    Email = registro.Email,
                    NombreCompleto = registro.NombreCompleto,
                    Contrasena = registro.Contrasena,
                    RolId = registro.RolId,
                    Instituciones = registro.Instituciones,
                    CarrerasAsociadas = registro.CarrerasAsociadas,
                    AreasAsociadas = registro.AreasAsociadas,
                    Telefonos = registro.Telefonos,
                    Activo = true,
                    Confirmado = false,
                    FechaCreacion = DateTime.Now
                };

                var (ok, error, usuarioCreado) = await _usuarioService.RegistrarAsync(usuario);

                if (!ok)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        mensaje = error
                    });
                }

                return new JsonResult(new
                {
                    success = true,
                    mensaje = "Usuario registrado exitosamente. Por favor, revise su correo electrónico para confirmar su cuenta."
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    mensaje = ex.Message
                });
            }
        }
    }
}
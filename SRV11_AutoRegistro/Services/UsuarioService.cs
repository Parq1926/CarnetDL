using SRV11_AutoRegistro.Entities;
using SRV11_AutoRegistro.Repository;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;


namespace SRV11_AutoRegistro.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UsuarioRepository _repository;
        private readonly UsuarioCarreraRepository _carreraRepository;
        private readonly UsuarioAreaRepository _areaRepository;
        private readonly UsuarioInstitucionRepository _institucionRepository;
        private readonly IInstitucionService _institucionService;
        private readonly ICarreraService _carreraService;
        private readonly IAreaService _areaService;
        private readonly UsuarioTelefonoRepository _telefonoRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ITipoUsuarioService _tipoUsuarioService;
        private readonly ITipoIdentificacionService _tipoIdentificacionService;
        private readonly IRolService _rolService;


        public UsuarioService(
            UsuarioRepository repository,
            UsuarioCarreraRepository carreraRepository,
            UsuarioAreaRepository areaRepository,
            UsuarioInstitucionRepository institucionRepository,
            IInstitucionService institucionService,
            ICarreraService carreraService,
            IAreaService areaService,
            UsuarioTelefonoRepository telefonoRepository,
            IConfiguration configuration,
            IEmailService emailService,
            ITipoUsuarioService tipoUsuarioService,
            ITipoIdentificacionService tipoIdentificacionService,
            IRolService rolService)
        {
            _repository = repository;
            _carreraRepository = carreraRepository;
            _areaRepository = areaRepository;
            _institucionRepository = institucionRepository;

            _institucionService = institucionService;
            _carreraService = carreraService;
            _areaService = areaService;
            _telefonoRepository = telefonoRepository;
            _configuration = configuration;
            _emailService = emailService;
            _tipoUsuarioService = tipoUsuarioService;
            _tipoIdentificacionService = tipoIdentificacionService;
            _rolService = rolService;
        }

        public async Task<(bool ok, string error, Usuario? usuarioCreado)> RegistrarAsync(Usuario usuario)
        {
            if (usuario.TipoUsuarioId <= 0)
                return (false, "Debe indicar un tipo de usuario", null);

            var tipoUsuario =
                await _tipoUsuarioService.GetById(usuario.TipoUsuarioId);

            if (tipoUsuario is null)
            {
                return (
                    false,
                    $"El tipo de usuario {usuario.TipoUsuarioId} no existe",
                    null
                );
            }

            var nombreTipo = tipoUsuario.Nombre.Trim();

            if (usuario.RolId <= 0)
            {
                return (false, "Debe seleccionar un rol", null);
            }

            var rol = await _rolService.GetById(usuario.RolId);

            if (rol is null)
            {
                return (
                    false,
                    $"El rol {usuario.RolId} no existe",
                    null
                );
            }

            if (usuario.TipoIdentificacionId <= 0)
                return (false, "Debe indicar un tipo de identificación", null);

            var tipoIdentificacion =
                await _tipoIdentificacionService.GetById(usuario.TipoIdentificacionId);

            if (tipoIdentificacion is null)
            {
                return (
                    false,
                    $"El tipo de identificación {usuario.TipoIdentificacionId} no existe",
                    null
                );
            }

            if (string.IsNullOrWhiteSpace(usuario.NumeroIdentificacion))
                return (false, "La identificación es requerida", null);

            if (string.IsNullOrWhiteSpace(usuario.Email))
                return (false, "El email es requerido", null);

            if (!Regex.IsMatch(
                usuario.Email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return (false, "Formato de email inválido", null);
            }

            if (string.IsNullOrWhiteSpace(usuario.NombreCompleto))
                return (false, "El nombre completo es requerido", null);

            if (string.IsNullOrWhiteSpace(usuario.Contrasena))
                return (false, "La contraseña es requerida", null);

            if (nombreTipo.Equals("Estudiante", StringComparison.OrdinalIgnoreCase))
            {
                if (usuario.CarrerasAsociadas == null ||
                    !usuario.CarrerasAsociadas.Any())
                {
                    return (false,
                        "El estudiante debe tener al menos una carrera",
                        null);
                }
            }

            // Validación funcionario
            if (nombreTipo.Equals("Funcionario", StringComparison.OrdinalIgnoreCase))
            {
                if (usuario.AreasAsociadas == null ||
                    !usuario.AreasAsociadas.Any())
                {
                    return (false,
                        "El funcionario debe tener al menos un área",
                        null);
                }
            }

            var existe = await _repository.GetByEmailAsync(usuario.Email);


            if (existe is not null)
            {
                // Cuenta ya confirmada
                if (existe.Confirmado)
                {
                    return (
                        false,
                        "Ya existe una cuenta confirmada con ese correo",
                        null
                    );
                }


                // Cuenta pendiente de confirmación
                await _repository.EliminarUsuarioPendienteAsync(existe.ID);
            }


            if (!usuario.Instituciones.Any())
            {
                return (false,
                    "Debe seleccionar al menos una institución",
                    null);
            }

            var dominioCorreo = usuario.Email
                .Split('@')
                .Last()
                .Trim()
                .ToLower();

            var dominioValido = false;

            foreach (var institucionId in usuario.Instituciones)
            {
                var institucion =
                    await _institucionService.GetById(institucionId);

                if (institucion is null)
                {
                    return (
                        false,
                        $"La institución {institucionId} no existe",
                        null
                    );
                }

                var dominios = institucion.Dominios
                    .Split(',')
                    .Select(x => x.Trim().ToLower());

                if (dominios.Contains(dominioCorreo))
                {
                    dominioValido = true;
                }
            }

            if (!dominioValido)
            {
                return (
                    false,
                    "El dominio del correo no pertenece a ninguna institución seleccionada",
                    null
                );
            }

            if (nombreTipo.Equals("Estudiante", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var carreraId in usuario.CarrerasAsociadas)
                {
                    var carrera = await _carreraService.GetById(carreraId);

                    if (carrera is null)
                    {
                        return (
                            false,
                            $"La carrera {carreraId} no existe",
                            null
                        );
                    }
                }
            }

            if (nombreTipo.Equals("Funcionario", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var areaId in usuario.AreasAsociadas)
                {
                    var area =
                        await _areaService.GetById(areaId);

                    if (area is null)
                    {
                        return (
                            false,
                            $"El área {areaId} no existe",
                            null
                        );
                    }
                }
            }

            var token = Guid.NewGuid().ToString();

            usuario.Contrasena =
                BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);

            usuario.Activo = true;
            usuario.Confirmado = false;
            usuario.FechaCreacion = DateTime.Now;
            usuario.TokenConfirmacion = token;
            var minutosExpiracion =
                _configuration.GetValue<int>("TokenExpirationMinutes");

            usuario.FechaExpiracion =
                DateTime.Now.AddMinutes(minutosExpiracion);

            var usuarioId = await _repository.CreateAsync(usuario);
            usuario.ID = usuarioId;


            var apiUrl = _configuration["Services:AutoRegistro"];

            var enlaceConfirmacion =
                $"{apiUrl}/autoregistro/confirmar/{token}";

            await _emailService.EnviarCorreoConfirmacionAsync(
                usuario.Email,
                enlaceConfirmacion);

            // Instituciones
            if (usuario.Instituciones != null)
            {
                foreach (var institucionId in usuario.Instituciones)
                {
                    await _institucionRepository
                        .AgregarAsync(usuarioId, institucionId);
                }
            }

            // Carreras
            if (usuario.CarrerasAsociadas != null)
            {
                foreach (var carreraId in usuario.CarrerasAsociadas)
                {
                    await _carreraRepository
                        .AgregarAsync(usuarioId, carreraId);
                }
            }

            // Áreas
            if (usuario.AreasAsociadas != null)
            {
                foreach (var areaId in usuario.AreasAsociadas)
                {
                    await _areaRepository
                        .AgregarAsync(usuarioId, areaId);
                }
            }

            // Teléfonos
            if (usuario.Telefonos != null)
            {
                foreach (var telefono in usuario.Telefonos)
                {
                    await _telefonoRepository
                        .AgregarAsync(usuarioId, telefono);
                }
            }

            return (true, string.Empty, usuario);
        }



        public async Task<(bool ok, string error)> ConfirmarCuentaAsync(string token)
        {
            var usuario = await _repository.GetByTokenAsync(token);

            if (usuario is null)
                return (false, "Token inválido");

            if (usuario.FechaExpiracion is null)
                return (false, "El token no tiene fecha de expiración");

            if (usuario.FechaExpiracion < DateTime.Now)
                return (false, "El token ha expirado");

            await _repository.ConfirmarCuentaAsync(usuario.ID);

            return (true, string.Empty);
        }
    }
}
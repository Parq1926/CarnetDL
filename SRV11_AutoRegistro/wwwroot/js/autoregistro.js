// ========================================
// AUTO REGISTRO - JavaScript
// ========================================

// ========================================
// ALERTAS
// ========================================

function mostrarAlerta(mensaje, tipo) {
    const container = document.getElementById('alertContainer');
    container.innerHTML = `
        <div class="alert alert-${tipo} alert-dismissible">
            ${mensaje}
            <button onclick="this.parentElement.remove()" style="float:right; background:none; border:none; font-size:20px; cursor:pointer;">&times;</button>
        </div>
    `;
    setTimeout(() => { container.innerHTML = ''; }, 5000);
}

// ========================================
// LOADING
// ========================================

function mostrarLoading(show) {
    document.getElementById('loading').style.display = show ? 'block' : 'none';
}

// ========================================
// CARGAR COMBOS
// ========================================

async function cargarCombos() {
    try {
        const responseTipoUsuario = await fetch('/AutoRegistro?handler=TiposUsuario');
        const tiposUsuario = await responseTipoUsuario.json();

        const responseTipoIdentificacion = await fetch('/autoregistro?handler=TiposIdentificacion');
        const tiposIdentificacion = await responseTipoIdentificacion.json();

        const responseCarreras = await fetch('/autoregistro?handler=Carreras');
        const carreras = await responseCarreras.json();

        const responseInstituciones = await fetch('/autoregistro?handler=Instituciones');
        const instituciones = await responseInstituciones.json();

        const responseAreas = await fetch('/autoregistro?handler=Areas');
        const areas = await responseAreas.json();

        const responseRoles = await fetch('/autoregistro?handler=Roles');
        const roles = await responseRoles.json();

        // Llenar selects
        const selectTipoUsuario = document.getElementById('tipoUsuarioId');
        selectTipoUsuario.innerHTML = '<option value="">Seleccione...</option>';
        tiposUsuario.forEach(tipo => {
            selectTipoUsuario.innerHTML += `<option value="${tipo.id}">${tipo.nombre}</option>`;
        });

        const selectTipoIdentificacion = document.getElementById('tipoIdentificacionId');
        selectTipoIdentificacion.innerHTML = '<option value="">Seleccione...</option>';
        tiposIdentificacion.forEach(tipo => {
            selectTipoIdentificacion.innerHTML += `<option value="${tipo.id}">${tipo.nombre}</option>`;
        });

        const selectCarreras = document.getElementById('carreras');
        selectCarreras.innerHTML = '';
        carreras.forEach(carrera => {
            selectCarreras.innerHTML += `<option value="${carrera.id}">${carrera.nombre}</option>`;
        });

        const selectInstituciones = document.getElementById('instituciones');
        selectInstituciones.innerHTML = '';
        instituciones.forEach(institucion => {
            selectInstituciones.innerHTML += `<option value="${institucion.id}">${institucion.nombre}</option>`;
        });

        const selectAreas = document.getElementById('areas');
        selectAreas.innerHTML = '';
        areas.forEach(area => {
            selectAreas.innerHTML += `<option value="${area.id}">${area.nombre}</option>`;
        });

        const selectRoles = document.getElementById('rolId');
        selectRoles.innerHTML = '<option value="">Seleccione...</option>';
        roles.forEach(rol => {
            selectRoles.innerHTML += `<option value="${rol.id}">${rol.nombre}</option>`;
        });

    } catch (error) {
        mostrarAlerta('Error al cargar los datos. Verifique el servidor.', 'danger');
    }
}

// ========================================
// VALIDACIONES
// ========================================

function limpiarErrores() {
    document.querySelectorAll('.is-invalid').forEach(el => el.classList.remove('is-invalid'));
    document.querySelectorAll('.invalid-feedback').forEach(el => el.textContent = '');
}

function mostrarError(campoId, mensaje) {
    const campo = document.getElementById(campoId);
    const errDiv = document.getElementById('err' + campoId.charAt(0).toUpperCase() + campoId.slice(1));
    if (campo) campo.classList.add('is-invalid');
    if (errDiv) errDiv.textContent = mensaje;
}

function validarFormulario() {
    let valido = true;
    limpiarErrores();

    const tipoUsuarioId = document.getElementById('tipoUsuarioId').value;
    const tipoIdentificacionId = document.getElementById('tipoIdentificacionId').value;
    const numeroIdentificacion = document.getElementById('numeroIdentificacion').value.trim();
    const nombreCompleto = document.getElementById('nombreCompleto').value.trim();
    const email = document.getElementById('email').value.trim();
    const rolId = document.getElementById('rolId').value;
    const contrasena = document.getElementById('contrasena').value;
    const confirmarContrasena = document.getElementById('confirmarContrasena').value;
    const instituciones = document.getElementById('instituciones').selectedOptions;
    const carreras = document.getElementById('carreras').selectedOptions;
    const areas = document.getElementById('areas').selectedOptions;

    if (!tipoUsuarioId) {
        mostrarError('tipoUsuarioId', 'Seleccione un tipo de usuario');
        valido = false;
    }

    if (!tipoIdentificacionId) {
        mostrarError('tipoIdentificacionId', 'Seleccione un tipo de identificación');
        valido = false;
    }

    if (!numeroIdentificacion) {
        mostrarError('numeroIdentificacion', 'Ingrese el número de identificación');
        valido = false;
    }

    if (!nombreCompleto) {
        mostrarError('nombreCompleto', 'Ingrese el nombre completo');
        valido = false;
    }

    if (!email) {
        mostrarError('email', 'Ingrese el correo electrónico');
        valido = false;
    } else {
        const regex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;
        if (!regex.test(email)) {
            mostrarError('email', 'El correo no es válido');
            valido = false;
        }
    }

    if (!rolId) {
        mostrarError('rolId', 'Seleccione un rol');
        valido = false;
    }

    if (!contrasena) {
        mostrarError('contrasena', 'Ingrese una contraseña');
        valido = false;
    } else if (contrasena.length < 6) {
        mostrarError('contrasena', 'La contraseña debe tener al menos 6 caracteres');
        valido = false;
    }

    if (!confirmarContrasena) {
        mostrarError('confirmarContrasena', 'Confirme la contraseña');
        valido = false;
    } else if (contrasena !== confirmarContrasena) {
        mostrarError('confirmarContrasena', 'Las contraseñas no coinciden');
        valido = false;
    }

    if (instituciones.length === 0) {
        mostrarError('instituciones', 'Seleccione al menos una institución');
        valido = false;
    }

    const tipoSeleccionado = document.getElementById('tipoUsuarioId');
    const nombreTipo = tipoSeleccionado.options[tipoSeleccionado.selectedIndex]?.text || '';

    if (nombreTipo === 'Estudiante' && carreras.length === 0) {
        mostrarError('carreras', 'Seleccione al menos una carrera');
        valido = false;
    }

    if (nombreTipo === 'Funcionario' && areas.length === 0) {
        mostrarError('areas', 'Seleccione al menos un área');
        valido = false;
    }

    return valido;
}

// ========================================
// MOSTRAR/OCULTAR CAMPOS SEGÚN TIPO
// ========================================

function actualizarCamposSegunTipo() {
    const select = document.getElementById('tipoUsuarioId');
    const tipoSeleccionado = select.options[select.selectedIndex]?.text || '';

    const carrerasContainer = document.getElementById('carrerasContainer');
    const areasContainer = document.getElementById('areasContainer');

    carrerasContainer.style.display = 'none';
    areasContainer.style.display = 'none';

    if (tipoSeleccionado === 'Estudiante') {
        carrerasContainer.style.display = 'block';
    } else if (tipoSeleccionado === 'Funcionario') {
        areasContainer.style.display = 'block';
    }
}

// ========================================
// TELÉFONOS
// ========================================

function agregarCampoTelefono() {
    const container = document.getElementById('telefonosContainer');
    const div = document.createElement('div');
    div.className = 'input-group mb-2';
    div.innerHTML = `
        <input type="text" class="form-control telefono-input" placeholder="Número de teléfono" />
        <button type="button" class="btn btn-outline-danger eliminar-telefono">
            <i class="fas fa-times"></i>
        </button>
    `;
    container.appendChild(div);

    div.querySelector('.eliminar-telefono').addEventListener('click', function () {
        div.remove();
    });
}

function obtenerTelefonos() {
    const inputs = document.querySelectorAll('.telefono-input');
    const telefonos = [];
    inputs.forEach(input => {
        const valor = input.value.trim();
        if (valor) {
            telefonos.push(valor);
        }
    });
    return telefonos;
}

function limpiarFormulario() {
    document.getElementById('registroForm').reset();
    document.getElementById('carrerasContainer').style.display = 'none';
    document.getElementById('areasContainer').style.display = 'none';
    limpiarErrores();

    // Reiniciar teléfonos
    const telefonosContainer = document.getElementById('telefonosContainer');
    telefonosContainer.innerHTML = `
        <div class="input-group mb-2">
            <input type="text" class="form-control telefono-input" placeholder="Número de teléfono" />
            <button type="button" class="btn btn-outline-secondary agregar-telefono">
                <i class="fas fa-plus"></i>
            </button>
        </div>
    `;
    inicializarEventosTelefonos();
}

// ========================================
// REGISTRAR USUARIO
// ========================================

async function registrarUsuario() {
    if (!validarFormulario()) {
        return;
    }

    mostrarLoading(true);

    try {
        const tipoUsuarioId = document.getElementById('tipoUsuarioId').value;
        const tipoIdentificacionId = document.getElementById('tipoIdentificacionId').value;
        const numeroIdentificacion = document.getElementById('numeroIdentificacion').value.trim();
        const nombreCompleto = document.getElementById('nombreCompleto').value.trim();
        const email = document.getElementById('email').value.trim();
        const rolId = document.getElementById('rolId').value;
        const contrasena = document.getElementById('contrasena').value;

        const institucionesSelect = document.getElementById('instituciones');
        const instituciones = Array.from(institucionesSelect.selectedOptions).map(opt => parseInt(opt.value));

        const carrerasSelect = document.getElementById('carreras');
        const carreras = Array.from(carrerasSelect.selectedOptions).map(opt => parseInt(opt.value));

        const areasSelect = document.getElementById('areas');
        const areas = Array.from(areasSelect.selectedOptions).map(opt => parseInt(opt.value));

        const telefonos = obtenerTelefonos();

        const usuarioData = {
            tipoUsuarioId: parseInt(tipoUsuarioId),
            tipoIdentificacionId: parseInt(tipoIdentificacionId),
            numeroIdentificacion: numeroIdentificacion,
            nombreCompleto: nombreCompleto,
            email: email,
            rolId: parseInt(rolId),
            contrasena: contrasena,
            instituciones: instituciones,
            carrerasAsociadas: carreras,
            areasAsociadas: areas,
            telefonos: telefonos
        };

        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        if (!token) {
            mostrarLoading(false);
            mostrarAlerta('Token CSRF no encontrado. Recargue la página.', 'danger');
            return;
        }

        const response = await fetch('/autoregistro?handler=registrar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(usuarioData)
        });

        const responseText = await response.text();
        let result;
        try {
            result = JSON.parse(responseText);
        } catch {
            mostrarLoading(false);
            mostrarAlerta('Respuesta inválida del servidor.', 'danger');
            return;
        }

        mostrarLoading(false);

        if (response.ok) {
            mostrarAlerta('✅ ' + (result.mensaje || 'Usuario registrado correctamente.') + ' Por favor, revise su correo electrónico para confirmar su cuenta.', 'success');
            limpiarFormulario();
        } else {
            mostrarAlerta('❌ ' + (result?.mensaje || 'Error al registrar el usuario'), 'danger');
        }

    } catch (error) {
        mostrarLoading(false);
        mostrarAlerta('❌ Error al procesar la solicitud: ' + error.message, 'danger');
    }
}

// ========================================
// EVENTOS
// ========================================

function inicializarEventosTelefonos() {
    const agregarBtn = document.querySelector('.agregar-telefono');
    if (agregarBtn) {
        agregarBtn.addEventListener('click', agregarCampoTelefono);
    }

    const eliminarBtns = document.querySelectorAll('.eliminar-telefono');
    eliminarBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            const div = this.closest('.input-group');
            if (div) div.remove();
        });
    });
}

// ========================================
// INICIALIZAR
// ========================================

document.addEventListener('DOMContentLoaded', async function () {
    await cargarCombos();
    inicializarEventosTelefonos();
    document.getElementById('tipoUsuarioId').addEventListener('change', actualizarCamposSegunTipo);
    document.getElementById('btnRegistrar').addEventListener('click', registrarUsuario);

    document.querySelector('button[type="reset"]').addEventListener('click', function () {
        limpiarErrores();
        document.getElementById('carrerasContainer').style.display = 'none';
        document.getElementById('areasContainer').style.display = 'none';
        document.getElementById('alertContainer').innerHTML = '';
    });
});
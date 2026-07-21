// ========================================
// CONFIGURACIÓN
// ========================================
const API_URL = '/api/Usuarios';
let idEliminar = 0;

// ========================================
// FUNCIONES PARA OBTENER TOKEN
// ========================================

function getToken() {
    return localStorage.getItem('accessToken');
}

function getHeaders() {
    const token = getToken();
    if (!token) {
        mostrarAlerta('No hay sesión activa. Redirigiendo...', 'warning');
        setTimeout(function () {
            window.location.href = 'https://localhost:7019/Login';
        }, 1500);
        return null;
    }
    return {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
    };
}

// ========================================
// FUNCIONES DE MODALES
// ========================================

function abrirModalCrear() {
    document.getElementById('tituloModal').textContent = 'Nuevo Usuario';
    document.getElementById('idUsuario').value = '0';
    limpiarFormulario();
    limpiarErrores();
    document.getElementById('modalFormulario').style.display = 'flex';
}

function cerrarModalFormulario() {
    document.getElementById('modalFormulario').style.display = 'none';
    limpiarErrores();
}

function abrirModalBuscar() {
    document.getElementById('buscarId').value = '';
    document.getElementById('buscarResultado').style.display = 'none';
    document.getElementById('buscarResultado').innerHTML = '';
    document.getElementById('modalBuscar').style.display = 'flex';
}

function cerrarModalBuscar() {
    document.getElementById('modalBuscar').style.display = 'none';
}

function abrirModalFiltros() {
    document.getElementById('filtroIdentificacion').value = '';
    document.getElementById('filtroNombre').value = '';
    document.getElementById('filtroTipoUsuario').value = '';
    document.getElementById('modalFiltros').style.display = 'flex';
}

function cerrarModalFiltros() {
    document.getElementById('modalFiltros').style.display = 'none';
}

function abrirModalEliminar(id, nombre) {
    idEliminar = id;
    document.getElementById('nombreEliminar').textContent = nombre;
    document.getElementById('modalEliminar').style.display = 'flex';
}

function cerrarModalEliminar() {
    document.getElementById('modalEliminar').style.display = 'none';
}

function toggleCamposSegunTipo() {
    const tipo = parseInt(document.getElementById('tipoUsuarioId').value);
    document.getElementById('carrerasContainer').style.display = (tipo === 1) ? 'block' : 'none';
    document.getElementById('areasContainer').style.display = (tipo === 2) ? 'block' : 'none';
}

// ========================================
// TELEFONOS, CARRERAS Y AREAS
// ========================================

function agregarTelefono() {
    const container = document.getElementById('telefonosContainer');
    const div = document.createElement('div');
    div.className = 'input-group';
    div.style.marginBottom = '5px';
    div.innerHTML = '<input type="text" class="form-control telefono-input" placeholder="Ej: 8888-8888"><button class="btn btn-danger btn-sm" onclick="eliminarTelefono(this)">✕</button>';
    container.appendChild(div);
}

function eliminarTelefono(btn) {
    const container = document.getElementById('telefonosContainer');
    if (container.children.length > 1) {
        btn.parentElement.remove();
    }
}

function agregarCarrera() {
    const container = document.getElementById('carrerasList');
    const div = document.createElement('div');
    div.className = 'input-group';
    div.style.marginBottom = '5px';
    div.innerHTML = '<input type="number" class="form-control carrera-input" placeholder="ID de Carrera"><button class="btn btn-danger btn-sm" onclick="eliminarCarrera(this)">✕</button>';
    container.appendChild(div);
}

function eliminarCarrera(btn) {
    const container = document.getElementById('carrerasList');
    if (container.children.length > 1) {
        btn.parentElement.remove();
    }
}

function agregarArea() {
    const container = document.getElementById('areasList');
    const div = document.createElement('div');
    div.className = 'input-group';
    div.style.marginBottom = '5px';
    div.innerHTML = '<input type="number" class="form-control area-input" placeholder="ID de Area"><button class="btn btn-danger btn-sm" onclick="eliminarArea(this)">✕</button>';
    container.appendChild(div);
}

function eliminarArea(btn) {
    const container = document.getElementById('areasList');
    if (container.children.length > 1) {
        btn.parentElement.remove();
    }
}

// ========================================
// LIMPIAR FORMULARIO
// ========================================

function limpiarFormulario() {
    document.getElementById('email').value = '';
    document.getElementById('contrasena').value = '';
    document.getElementById('tipoIdentificacionId').value = '';
    document.getElementById('numeroIdentificacion').value = '';
    document.getElementById('nombreCompleto').value = '';
    document.getElementById('tipoUsuarioId').value = '';
    document.getElementById('activo').value = 'true';
    document.getElementById('carrerasContainer').style.display = 'none';
    document.getElementById('areasContainer').style.display = 'none';

    const telContainer = document.getElementById('telefonosContainer');
    telContainer.innerHTML = '<div class="input-group" style="margin-bottom:5px;"><input type="text" class="form-control telefono-input" placeholder="Ej: 8888-8888"><button class="btn btn-danger btn-sm" onclick="eliminarTelefono(this)">✕</button></div>';

    const carrerasList = document.getElementById('carrerasList');
    carrerasList.innerHTML = '<div class="input-group" style="margin-bottom:5px;"><input type="number" class="form-control carrera-input" placeholder="ID de Carrera"><button class="btn btn-danger btn-sm" onclick="eliminarCarrera(this)">✕</button></div>';

    const areasList = document.getElementById('areasList');
    areasList.innerHTML = '<div class="input-group" style="margin-bottom:5px;"><input type="number" class="form-control area-input" placeholder="ID de Area"><button class="btn btn-danger btn-sm" onclick="eliminarArea(this)">✕</button></div>';
}

function limpiarFiltros() {
    document.getElementById('filtroIdentificacion').value = '';
    document.getElementById('filtroNombre').value = '';
    document.getElementById('filtroTipoUsuario').value = '';
    cargarUsuarios();
    cerrarModalFiltros();
}

// ========================================
// VALIDACIONES
// ========================================

function limpiarErrores() {
    document.querySelectorAll('.is-invalid').forEach(function (el) {
        el.classList.remove('is-invalid');
    });
    document.querySelectorAll('.invalid-feedback').forEach(function (el) {
        el.textContent = '';
    });
}

function mostrarError(campo, mensaje) {
    var el = document.getElementById(campo);
    var err = document.getElementById('err' + campo.charAt(0).toUpperCase() + campo.slice(1));
    el.classList.add('is-invalid');
    err.textContent = mensaje;
}

function validarFormulario() {
    var valido = true;
    limpiarErrores();

    var email = document.getElementById('email').value.trim();
    var contrasena = document.getElementById('contrasena').value.trim();
    var tipoIdentificacionId = document.getElementById('tipoIdentificacionId').value;
    var numeroIdentificacion = document.getElementById('numeroIdentificacion').value.trim();
    var nombreCompleto = document.getElementById('nombreCompleto').value.trim();
    var tipoUsuarioId = document.getElementById('tipoUsuarioId').value;

    if (!email) {
        mostrarError('email', 'El email es requerido');
        valido = false;
    } else {
        var emailRegex = /^[^\s@]+@(cuc\.ac\.cr|cuc\.cr)$/;
        if (!emailRegex.test(email)) {
            mostrarError('email', 'Solo se permiten @cuc.ac.cr o @cuc.cr');
            valido = false;
        }
    }

    if (!contrasena) {
        mostrarError('contrasena', 'La contraseña es requerida');
        valido = false;
    } else if (contrasena.length < 8) {
        mostrarError('contrasena', 'La contraseña debe tener al menos 8 caracteres');
        valido = false;
    }

    if (!tipoIdentificacionId) {
        mostrarError('tipoIdentificacionId', 'Seleccione un tipo de identificación');
        valido = false;
    }

    if (!numeroIdentificacion) {
        mostrarError('numeroIdentificacion', 'El número de identificación es requerido');
        valido = false;
    } else if (numeroIdentificacion.length < 5) {
        mostrarError('numeroIdentificacion', 'El número debe tener al menos 5 caracteres');
        valido = false;
    }

    if (!nombreCompleto) {
        mostrarError('nombreCompleto', 'El nombre completo es requerido');
        valido = false;
    } else if (nombreCompleto.length < 3) {
        mostrarError('nombreCompleto', 'El nombre debe tener al menos 3 caracteres');
        valido = false;
    }

    if (!tipoUsuarioId) {
        mostrarError('tipoUsuarioId', 'Seleccione un tipo de usuario');
        valido = false;
    }

    return valido;
}

function obtenerDatosFormulario() {
    var telefonos = [];
    document.querySelectorAll('.telefono-input').forEach(function (input) {
        if (input.value.trim()) {
            telefonos.push(input.value.trim());
        }
    });

    var carrerasIds = [];
    document.querySelectorAll('.carrera-input').forEach(function (input) {
        if (input.value.trim()) {
            carrerasIds.push(parseInt(input.value.trim()));
        }
    });

    var areasIds = [];
    document.querySelectorAll('.area-input').forEach(function (input) {
        if (input.value.trim()) {
            areasIds.push(parseInt(input.value.trim()));
        }
    });

    var id = document.getElementById('idUsuario').value;
    var isEdit = id !== '0';

    if (isEdit) {
        return {
            id: parseInt(id),
            email: document.getElementById('email').value.trim(),
            tipoIdentificacionId: parseInt(document.getElementById('tipoIdentificacionId').value),
            numeroIdentificacion: document.getElementById('numeroIdentificacion').value.trim(),
            nombreCompleto: document.getElementById('nombreCompleto').value.trim(),
            contrasena: document.getElementById('contrasena').value.trim() || null,
            tipoUsuarioId: parseInt(document.getElementById('tipoUsuarioId').value),
            activo: document.getElementById('activo').value === 'true',
            telefonos: telefonos,
            carrerasIds: carrerasIds,
            areasIds: areasIds
        };
    } else {
        return {
            email: document.getElementById('email').value.trim(),
            tipoIdentificacionId: parseInt(document.getElementById('tipoIdentificacionId').value),
            numeroIdentificacion: document.getElementById('numeroIdentificacion').value.trim(),
            nombreCompleto: document.getElementById('nombreCompleto').value.trim(),
            contrasena: document.getElementById('contrasena').value.trim(),
            tipoUsuarioId: parseInt(document.getElementById('tipoUsuarioId').value),
            telefonos: telefonos,
            carrerasIds: carrerasIds,
            areasIds: areasIds
        };
    }
}

// ========================================
// ALERTAS Y LOADING
// ========================================

function mostrarAlerta(mensaje, tipo) {
    var div = document.getElementById('alertas');
    div.innerHTML = '<div class="alert alert-' + tipo + ' alert-dismissible">' + mensaje + '<button onclick="this.parentElement.remove()" style="float:right; background:none; border:none; font-size:20px; cursor:pointer;">×</button></div>';
    setTimeout(function () {
        div.innerHTML = '';
    }, 4000);
}

function mostrarLoading(show) {
    document.getElementById('loading').style.display = show ? 'block' : 'none';
}

// ========================================
// BUSCAR POR ID
// ========================================

async function buscarUsuarioPorId() {
    var id = document.getElementById('buscarId').value.trim();
    if (!id || parseInt(id) <= 0) {
        mostrarAlerta('Ingrese un ID valido mayor a 0', 'warning');
        return;
    }

    var headers = getHeaders();
    if (!headers) return;

    var resultadoDiv = document.getElementById('buscarResultado');
    resultadoDiv.style.display = 'block';
    resultadoDiv.innerHTML = '<div class="loading"><div class="spinner-border"></div><p>Buscando...</p></div>';

    try {
        var response = await fetch(API_URL + '/' + id, { headers: headers });

        if (response.status === 401) {
            window.location.href = 'https://localhost:7019/Login';
            return;
        }

        if (response.status === 404) {
            resultadoDiv.innerHTML = '<div class="alert alert-warning"><i class="fas fa-exclamation-triangle"></i> No se encontro ningun usuario con ID ' + id + '</div>';
            return;
        }

        if (!response.ok) throw new Error('Error al buscar');
        var u = await response.json();

        resultadoDiv.innerHTML =
            '<div style="background:#f8f9fa; padding:15px; border-radius:8px; border:1px solid #dee2e6;">' +
            '<div style="display:grid; grid-template-columns:1fr 1fr; gap:8px; font-size:14px;">' +
            '<div><strong>ID:</strong> ' + u.id + '</div>' +
            '<div><strong>Email:</strong> ' + u.email + '</div>' +
            '<div><strong>Nombre:</strong> ' + u.nombreCompleto + '</div>' +
            '<div><strong>Identificacion:</strong> ' + u.numeroIdentificacion + '</div>' +
            '<div><strong>Tipo Identificacion:</strong> ' + u.tipoIdentificacion + '</div>' +
            '<div><strong>Tipo Usuario:</strong> ' + u.tipoUsuario + '</div>' +
            '<div><strong>Estado:</strong> <span class="' + (u.activo ? 'badge-success' : 'badge-danger') + '">' + (u.activo ? 'Activo' : 'Inactivo') + '</span></div>' +
            '<div><strong>Telefonos:</strong> ' + (u.telefonos ? u.telefonos.join(', ') : 'Ninguno') + '</div>' +
            '<div><strong>Carreras:</strong> ' + (u.carrerasIds ? u.carrerasIds.join(', ') : 'Ninguna') + '</div>' +
            '<div><strong>Areas:</strong> ' + (u.areasIds ? u.areasIds.join(', ') : 'Ninguna') + '</div>' +
            '</div>' +
            '<div style="margin-top:10px; display:flex; gap:8px;">' +
            '<button class="btn btn-warning btn-sm" onclick="editarUsuario(' + u.id + ')">Editar</button>' +
            '<button class="btn btn-danger btn-sm" onclick="abrirModalEliminar(' + u.id + ', \'' + u.nombreCompleto + '\')">Eliminar</button>' +
            '<button class="btn btn-secondary btn-sm" onclick="cerrarModalBuscar()">Cerrar</button>' +
            '</div>' +
            '</div>';

    } catch (error) {
        resultadoDiv.innerHTML = '<div class="alert alert-danger">Error al buscar: ' + error.message + '</div>';
    }
}

// ========================================
// APLICAR FILTROS
// ========================================

async function aplicarFiltros() {
    var filtro = {
        identificacion: document.getElementById('filtroIdentificacion').value.trim(),
        nombre: document.getElementById('filtroNombre').value.trim(),
        tipoUsuario: document.getElementById('filtroTipoUsuario').value
    };

    var headers = getHeaders();
    if (!headers) return;

    try {
        mostrarLoading(true);
        var response = await fetch(API_URL + '/buscar', {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(filtro)
        });

        if (response.status === 401) {
            window.location.href = 'https://localhost:7019/Login';
            return;
        }

        if (!response.ok) throw new Error('Error al filtrar');
        var data = await response.json();
        renderizarUsuarios(data);
        cerrarModalFiltros();
        mostrarAlerta('Filtros aplicados correctamente', 'success');
    } catch (error) {
        mostrarAlerta('Error al filtrar: ' + error.message, 'danger');
    } finally {
        mostrarLoading(false);
    }
}

// ========================================
// CRUD - LISTAR TODOS
// ========================================

function renderizarUsuarios(usuarios) {
    var container = document.getElementById('listaUsuarios');

    if (!usuarios || usuarios.length === 0) {
        container.innerHTML =
            '<div class="empty-state">' +
            '<i class="fas fa-users fa-4x"></i>' +
            '<p>No hay usuarios registrados</p>' +
            '<button class="btn btn-primary" onclick="abrirModalCrear()">' +
            '<i class="fas fa-plus"></i> Crear primer usuario' +
            '</button>' +
            '</div>';
        return;
    }

    var html = '';
    usuarios.forEach(function (u) {
        var telefonos = u.telefonos && u.telefonos.length > 0 ? u.telefonos.join(', ') : 'Ninguno';
        html +=
            '<div class="usuario-card">' +
            '<div class="usuario-card-header">' +
            '<h3>' + u.nombreCompleto + '</h3>' +
            '<span class="' + (u.activo ? 'badge-success' : 'badge-danger') + '">' + (u.activo ? 'Activo' : 'Inactivo') + '</span>' +
            '</div>' +
            '<div class="usuario-card-body">' +
            '<div class="info-row">' +
            '<span class="label"><i class="fas fa-envelope"></i> Email:</span>' +
            '<span class="value">' + u.email + '</span>' +
            '</div>' +
            '<div class="info-row">' +
            '<span class="label"><i class="fas fa-id-card"></i> Identificacion:</span>' +
            '<span class="value">' + u.numeroIdentificacion + '</span>' +
            '</div>' +
            '<div class="info-row">' +
            '<span class="label"><i class="fas fa-user-tag"></i> Tipo:</span>' +
            '<span class="value">' + u.tipoUsuario + '</span>' +
            '</div>' +
            '<div class="info-row">' +
            '<span class="label"><i class="fas fa-phone"></i> Telefonos:</span>' +
            '<span class="value">' + telefonos + '</span>' +
            '</div>' +
            '<div class="info-row">' +
            '<span class="label"><i class="fas fa-graduation-cap"></i> Carreras:</span>' +
            '<span class="value">' + (u.carrerasIds && u.carrerasIds.length > 0 ? u.carrerasIds.join(', ') : 'Ninguna') + '</span>' +
            '</div>' +
            '<div class="info-row">' +
            '<span class="label"><i class="fas fa-building"></i> Areas:</span>' +
            '<span class="value">' + (u.areasIds && u.areasIds.length > 0 ? u.areasIds.join(', ') : 'Ninguna') + '</span>' +
            '</div>' +
            '</div>' +
            '<div class="usuario-card-footer">' +
            '<button class="btn-sm btn-info" onclick="editarUsuario(' + u.id + ')">' +
            '<i class="fas fa-edit"></i> Editar' +
            '</button>' +
            '<button class="btn-sm btn-danger" onclick="abrirModalEliminar(' + u.id + ', \'' + u.nombreCompleto + '\')">' +
            '<i class="fas fa-trash"></i> Eliminar' +
            '</button>' +
            '</div>' +
            '</div>';
    });

    container.innerHTML = html;
}

async function cargarUsuarios() {
    var headers = getHeaders();
    if (!headers) return;

    try {
        mostrarLoading(true);
        var response = await fetch(API_URL, { headers: headers });

        if (response.status === 401) {
            window.location.href = 'https://localhost:7019/Login';
            return;
        }

        if (!response.ok) throw new Error('Error al cargar');
        var data = await response.json();
        renderizarUsuarios(data);
    } catch (error) {
        document.getElementById('listaUsuarios').innerHTML =
            '<div class="alert alert-danger">' +
            '<i class="fas fa-exclamation-circle"></i> Error al cargar usuarios' +
            '</div>';
    } finally {
        mostrarLoading(false);
    }
}

// ========================================
// CRUD - CREAR / EDITAR
// ========================================

async function guardarUsuario() {
    if (!validarFormulario()) return;

    var headers = getHeaders();
    if (!headers) return;

    var id = document.getElementById('idUsuario').value;
    var data = obtenerDatosFormulario();
    var isEdit = id !== '0';
    var url = isEdit ? API_URL + '/' + id : API_URL;
    var method = isEdit ? 'PUT' : 'POST';

    try {
        var response = await fetch(url, {
            method: method,
            headers: headers,
            body: JSON.stringify(data)
        });

        if (response.status === 401) {
            window.location.href = 'https://localhost:7019/Login';
            return;
        }

        if (response.ok) {
            mostrarAlerta(isEdit ? 'Usuario actualizado' : 'Usuario creado', 'success');
            cerrarModalFormulario();
            cargarUsuarios();
            cerrarModalBuscar();
        } else {
            var error = await response.json();
            mostrarAlerta(error.error || 'Error al guardar', 'danger');
        }
    } catch (error) {
        mostrarAlerta('Error de conexion: ' + error.message, 'danger');
    }
}

// ========================================
// CRUD - EDITAR (OBTENER POR ID)
// ========================================

async function editarUsuario(id) {
    var headers = getHeaders();
    if (!headers) return;

    try {
        var response = await fetch(API_URL + '/' + id, { headers: headers });

        if (response.status === 401) {
            window.location.href = 'https://localhost:7019/Login';
            return;
        }

        if (!response.ok) throw new Error('Error al obtener datos');
        var u = await response.json();

        document.getElementById('tituloModal').textContent = 'Editar Usuario';
        document.getElementById('idUsuario').value = u.id;
        document.getElementById('email').value = u.email;
        document.getElementById('contrasena').value = '';
        document.getElementById('tipoIdentificacionId').value = u.tipoIdentificacionId || '';
        document.getElementById('numeroIdentificacion').value = u.numeroIdentificacion;
        document.getElementById('nombreCompleto').value = u.nombreCompleto;
        document.getElementById('tipoUsuarioId').value = u.tipoUsuarioId || '';
        document.getElementById('activo').value = u.activo ? 'true' : 'false';

        // Resetear telefonos
        var telContainer = document.getElementById('telefonosContainer');
        telContainer.innerHTML = '';
        if (u.telefonos && u.telefonos.length > 0) {
            u.telefonos.forEach(function (tel) {
                var div = document.createElement('div');
                div.className = 'input-group';
                div.style.marginBottom = '5px';
                div.innerHTML = '<input type="text" class="form-control telefono-input" value="' + tel + '" placeholder="Ej: 8888-8888"><button class="btn btn-danger btn-sm" onclick="eliminarTelefono(this)">✕</button>';
                telContainer.appendChild(div);
            });
        } else {
            telContainer.innerHTML = '<div class="input-group" style="margin-bottom:5px;"><input type="text" class="form-control telefono-input" placeholder="Ej: 8888-8888"><button class="btn btn-danger btn-sm" onclick="eliminarTelefono(this)">✕</button></div>';
        }

        // Resetear carreras
        var carrerasList = document.getElementById('carrerasList');
        carrerasList.innerHTML = '';
        if (u.carrerasIds && u.carrerasIds.length > 0) {
            u.carrerasIds.forEach(function (carreraId) {
                var div = document.createElement('div');
                div.className = 'input-group';
                div.style.marginBottom = '5px';
                div.innerHTML = '<input type="number" class="form-control carrera-input" value="' + carreraId + '" placeholder="ID de Carrera"><button class="btn btn-danger btn-sm" onclick="eliminarCarrera(this)">✕</button>';
                carrerasList.appendChild(div);
            });
        } else {
            carrerasList.innerHTML = '<div class="input-group" style="margin-bottom:5px;"><input type="number" class="form-control carrera-input" placeholder="ID de Carrera"><button class="btn btn-danger btn-sm" onclick="eliminarCarrera(this)">✕</button></div>';
        }

        // Resetear areas
        var areasList = document.getElementById('areasList');
        areasList.innerHTML = '';
        if (u.areasIds && u.areasIds.length > 0) {
            u.areasIds.forEach(function (areaId) {
                var div = document.createElement('div');
                div.className = 'input-group';
                div.style.marginBottom = '5px';
                div.innerHTML = '<input type="number" class="form-control area-input" value="' + areaId + '" placeholder="ID de Area"><button class="btn btn-danger btn-sm" onclick="eliminarArea(this)">✕</button>';
                areasList.appendChild(div);
            });
        } else {
            areasList.innerHTML = '<div class="input-group" style="margin-bottom:5px;"><input type="number" class="form-control area-input" placeholder="ID de Area"><button class="btn btn-danger btn-sm" onclick="eliminarArea(this)">✕</button></div>';
        }

        toggleCamposSegunTipo();
        limpiarErrores();
        document.getElementById('modalFormulario').style.display = 'flex';
    } catch (error) {
        mostrarAlerta('Error al cargar datos: ' + error.message, 'danger');
    }
}

// ========================================
// CRUD - ELIMINAR
// ========================================

async function eliminarUsuario() {
    var headers = getHeaders();
    if (!headers) return;

    try {
        var response = await fetch(API_URL + '/' + idEliminar, {
            method: 'DELETE',
            headers: headers
        });

        if (response.status === 401) {
            window.location.href = 'https://localhost:7019/Login';
            return;
        }

        if (response.ok) {
            cerrarModalEliminar();
            mostrarAlerta('Usuario eliminado correctamente', 'success');
            cargarUsuarios();
            cerrarModalBuscar();
        } else {
            var error = await response.json();
            mostrarAlerta(error.error || 'Error al eliminar', 'danger');
        }
    } catch (error) {
        mostrarAlerta('Error de conexion: ' + error.message, 'danger');
    }
}

// ========================================
// INICIALIZAR
// ========================================

document.addEventListener('DOMContentLoaded', function () {
    cargarUsuarios();
});
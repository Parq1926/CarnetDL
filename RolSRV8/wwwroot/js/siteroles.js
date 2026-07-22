// ========================================
// ROLES - JAVASCRIPT
// ========================================

const API_URL = window.location.origin + '/api/Rol';
let idEliminar = 0;
let paginaActual = 1;
const registrosPorPagina = 15;
let todosLosRoles = [];


// ========================================
// OBTENER TOKEN
// ========================================

function getHeaders() {
    return {
        'Content-Type': 'application/json'
    };
}

// ========================================
// FUNCIONES DE MODALES
// ========================================

function abrirModalCrear() {
    document.getElementById('tituloModal').textContent = 'Nuevo Rol';
    document.getElementById('idRol').value = '0';
    document.getElementById('nombre').value = '';
    document.getElementById('pantallas').value = '';

    limpiarErrores();

    document.getElementById('modalFormulario').classList.add('show');
}

function cerrarModalFormulario() {
    document.getElementById('modalFormulario').classList.remove('show');
    limpiarErrores();
}

function abrirModalBuscar() {
    document.getElementById('buscarId').value = '';
    document.getElementById('buscarResultado').style.display = 'none';
    document.getElementById('buscarResultado').innerHTML = '';
    document.getElementById('modalBuscar').classList.add('show');
}

function cerrarModalBuscar() {
    document.getElementById('modalBuscar').classList.remove('show');
}

function abrirModalEliminar(id, nombre) {
    idEliminar = id;
    document.getElementById('nombreEliminar').textContent = nombre;
    document.getElementById('modalEliminar').classList.add('show');
}

function cerrarModalEliminar() {
    document.getElementById('modalEliminar').classList.remove('show');
}

// ========================================
// ALERTAS Y LOADING
// ========================================

function mostrarAlerta(mensaje, tipo) {
    const div = document.getElementById('alertas');
    div.innerHTML = `
        <div class="alert alert-${tipo} alert-dismissible">
            ${mensaje}
            <button onclick="this.parentElement.remove()" style="float:right; background:none; border:none; font-size:20px; cursor:pointer;">&times;</button>
        </div>
    `;
    setTimeout(() => { div.innerHTML = ''; }, 4000);
}

function mostrarLoading(show) {
    document.getElementById('loading').style.display = show ? 'block' : 'none';
}

// ========================================
// PAGINACION
// ========================================

function actualizarPaginacion(total) {
    const totalPaginas = Math.ceil(total / registrosPorPagina);
    const container = document.getElementById('paginacionContainer');

    if (totalPaginas <= 1) {
        container.style.display = 'none';
        return;
    }

    container.style.display = 'block';

    const inicio = (paginaActual - 1) * registrosPorPagina + 1;
    const fin = Math.min(paginaActual * registrosPorPagina, total);

    document.getElementById('inicioMostrar').textContent = inicio;
    document.getElementById('finMostrar').textContent = fin;
    document.getElementById('totalRegistros').textContent = total;
    document.getElementById('numeroPagina').textContent = paginaActual;

    document.getElementById('btnAnterior').disabled = paginaActual === 1;
    document.getElementById('btnSiguiente').disabled = paginaActual === totalPaginas;
}

function paginaAnterior() {
    if (paginaActual > 1) {
        paginaActual--;
        renderizarRoles();
    }
}

function paginaSiguiente() {
    const totalPaginas = Math.ceil(todosLosRoles.length / registrosPorPagina);
    if (paginaActual < totalPaginas) {
        paginaActual++;
        renderizarRoles();
    }
}

function renderizarRoles() {
    const inicio = (paginaActual - 1) * registrosPorPagina;
    const fin = inicio + registrosPorPagina;
    const rolesPagina = todosLosRoles.slice(inicio, fin);
    mostrarRoles(rolesPagina);
    actualizarPaginacion(todosLosRoles.length);
}

// ========================================
// BUSCAR POR ID
// ========================================

async function buscarRolPorId() {
    const id = document.getElementById('buscarId').value.trim();

    if (!id || parseInt(id) <= 0) {
        mostrarAlerta('Ingrese un ID valido mayor a 0', 'warning');
        return;
    }

    const resultadoDiv = document.getElementById('buscarResultado');
    resultadoDiv.style.display = 'block';
    resultadoDiv.innerHTML = '<div class="loading"><div class="spinner-border"></div><p>Buscando...</p></div>';

    try {
        const response = await fetch(`${API_URL}/${id}`, {
            headers: getHeaders()
        });

        if (response.status === 404) {
            resultadoDiv.innerHTML = `
                <div class="alert alert-warning">
                    <i class="fas fa-exclamation-triangle"></i> No se encontro ningun rol con ID ${id}
                </div>
            `;
            return;
        }

        if (!response.ok) {
            throw new Error('Error al buscar');
        }

        let r = await response.json();

        if (r.data) {
            r = r.data;
        }

        resultadoDiv.innerHTML = `
            <div style="background:#f8f9fa; padding:15px; border-radius:8px; border:1px solid #dee2e6;">
                <div style="display:grid; grid-template-columns:1fr 1fr; gap:8px; font-size:14px;">
                    <div><strong>ID:</strong> ${r.id}</div>
                    <div><strong>Nombre:</strong> ${r.nombre}</div>
                    <div><strong>Pantallas:</strong> ${r.pantallas}</div>
                </div>
                <div style="margin-top:10px; display:flex; gap:8px;">
                    <button class="btn btn-warning btn-sm" onclick="editarRol(${r.id})">Editar</button>
                    <button class="btn btn-danger btn-sm" onclick="abrirModalEliminar(${r.id}, '${r.nombre}')">Eliminar</button>
                    <button class="btn btn-secondary btn-sm" onclick="cerrarModalBuscar()">Cerrar</button>
                </div>
            </div>
        `;

    } catch (error) {
        resultadoDiv.innerHTML = `
            <div class="alert alert-danger">Error al buscar: ${error.message}</div>
        `;
    }
}

// ========================================
// VALIDACIONES
// ========================================

function limpiarErrores() {
    document.querySelectorAll('.is-invalid').forEach(el => el.classList.remove('is-invalid'));
    document.querySelectorAll('.invalid-feedback').forEach(el => el.textContent = '');
}

function mostrarError(campo, mensaje) {
    const el = document.getElementById(campo);
    const err = document.getElementById('err' + campo.charAt(0).toUpperCase() + campo.slice(1));
    el.classList.add('is-invalid');
    err.textContent = mensaje;
}

function validarFormulario() {
    let valido = true;
    limpiarErrores();

    const nombre = document.getElementById('nombre').value.trim();
    const pantallas = document.getElementById('pantallas').value.trim();

    if (!nombre) { mostrarError('nombre', 'El nombre es requerido'); valido = false; }
    if (!pantallas) { mostrarError('pantallas', 'Las pantallas son requeridas'); valido = false; }

    return valido;
}

function obtenerDatosFormulario() {
    const data = {
        nombre: document.getElementById('nombre').value.trim(),
        pantallas: document.getElementById('pantallas').value.trim()
    };

    const id = document.getElementById('idRol').value;
    if (id !== '0') {
        data.id = parseInt(id);
    }

    return data;
}

// ========================================
// CRUD - LISTAR TODOS
// ========================================

function mostrarRoles(roles) {
    const container = document.getElementById('listaRoles');

    if (!roles || roles.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                <i class="fas fa-user-tag fa-4x"></i>
                <p>No hay roles registrados</p>
                <button class="btn btn-primary" onclick="abrirModalCrear()">
                    <i class="fas fa-plus"></i> Crear primer rol
                </button>
            </div>
        `;
        return;
    }

    let html = '';
    roles.forEach(function (r) {
        const pantallasList = r.pantallas.split(',').map(p => p.trim()).filter(p => p);
        let pantallasHtml = '';
        if (pantallasList.length > 3) {
            pantallasHtml = pantallasList.slice(0, 3).map(p => `<span class="badge-items">${p}</span>`).join(' ') +
                ` <span class="badge-items-more">+${pantallasList.length - 3}</span>`;
        } else {
            pantallasHtml = pantallasList.map(p => `<span class="badge-items">${p}</span>`).join(' ');
        }

        html += `
            <div class="card-bordered">
                <div class="card-bordered-header">
                    <h3><i class="fas fa-user-tag"></i> ${r.nombre}</h3>
                    <span class="rol-id">ID: ${r.id}</span>
                </div>
                <div class="card-bordered-body">
                    <div class="info-row">
                        <span class="label"><i class="fas fa-desktop"></i> Pantallas:</span>
                        <span class="value">${pantallasHtml || '<span class="text-muted">Ninguna</span>'}</span>
                    </div>
                </div>
                <div class="card-bordered-footer">
                    <button class="btn-sm btn-info" onclick="editarRol(${r.id})">
                        <i class="fas fa-edit"></i> Editar
                    </button>
                    <button class="btn-sm btn-danger" onclick="abrirModalEliminar(${r.id}, '${r.nombre}')">
                        <i class="fas fa-trash"></i> Eliminar
                    </button>
                </div>
            </div>
        `;
    });

    container.innerHTML = html;
}

async function cargarRoles() {
    const container = document.getElementById('listaRoles');
    mostrarLoading(true);

    try {
        const response = await fetch(API_URL, {
            headers: getHeaders()
        });

        if (!response.ok) {
            throw new Error('Error al cargar');
        }

        const respuesta = await response.json();

        console.log("Respuesta roles:", respuesta);


        if (Array.isArray(respuesta)) {
            todosLosRoles = respuesta;
        }
        else if (respuesta.data && Array.isArray(respuesta.data)) {
            todosLosRoles = respuesta.data;
        }
        else {
            throw new Error("Formato de respuesta de roles no válido");
        }

        if (!todosLosRoles || todosLosRoles.length === 0) {
            container.innerHTML = `
                <div class="empty-state">
                    <i class="fas fa-user-tag fa-4x"></i>
                    <p>No hay roles registrados</p>
                    <button class="btn btn-primary" onclick="abrirModalCrear()">
                        <i class="fas fa-plus"></i> Crear primer rol
                    </button>
                </div>
            `;
            document.getElementById('paginacionContainer').style.display = 'none';
            mostrarLoading(false);
            return;
        }

        paginaActual = 1;
        renderizarRoles();
    } catch (error) {
        container.innerHTML = `
            <div class="alert alert-danger">
                <i class="fas fa-exclamation-circle"></i> Error al cargar roles: ${error.message}
            </div>
        `;
        document.getElementById('paginacionContainer').style.display = 'none';
    }

    mostrarLoading(false);
}

// ========================================
// CRUD - CREAR / EDITAR
// ========================================

async function guardarRol() {
    if (!validarFormulario()) return;

    const id = document.getElementById('idRol').value;
    const data = obtenerDatosFormulario();
    const isEdit = id !== '0';
    const url = isEdit ? `${API_URL}/${id}` : API_URL;
    const method = isEdit ? 'PUT' : 'POST';

    try {
        const response = await fetch(url, {
            method: method,
            headers: getHeaders(),
            body: JSON.stringify(data)
        });

        if (response.ok) {
            mostrarAlerta(isEdit ? 'Rol actualizado' : 'Rol creado', 'success');
            cerrarModalFormulario();
            cargarRoles();
            cerrarModalBuscar();
        } else {
            const error = await response.json();
            mostrarAlerta(error.mensaje || error.error || 'Error al guardar', 'danger');
        }
    } catch (error) {
        mostrarAlerta('Error de conexion: ' + error.message, 'danger');
    }
}

// ========================================
// CRUD - EDITAR (OBTENER POR ID + LLENAR MODAL)
// ========================================

async function editarRol(id) {
    try {
        const response = await fetch(`${API_URL}/${id}`, {
            headers: getHeaders()
        });

        if (!response.ok) throw new Error('Error al obtener datos');

        let r = await response.json();

        if (r.data) {
            r = r.data;
        }

        document.getElementById('tituloModal').textContent = 'Editar Rol';
        document.getElementById('idRol').value = r.id;
        document.getElementById('nombre').value = r.nombre;
        document.getElementById('pantallas').value = r.pantallas;
        limpiarErrores();
        document.getElementById('modalFormulario').classList.add('show');
    } catch (error) {
        mostrarAlerta('Error al cargar datos: ' + error.message, 'danger');
    }
}

// ========================================
// CRUD - ELIMINAR
// ========================================

async function eliminarRol() {
    try {
        const response = await fetch(`${API_URL}/${idEliminar}`, {
            method: 'DELETE',
            headers: getHeaders()
        });

        if (response.ok) {
            cerrarModalEliminar();
            mostrarAlerta('Rol eliminado', 'success');
            cargarRoles();
            cerrarModalBuscar();
        } else {
            const error = await response.json();
            mostrarAlerta(error.mensaje || error.error || 'Error al eliminar', 'danger');
        }
    } catch (error) {
        mostrarAlerta('Error de conexion: ' + error.message, 'danger');
    }
}

// ========================================
// INICIALIZAR
// ========================================

document.addEventListener('DOMContentLoaded', function () {
    cargarRoles();
});
// ========================================
// BITACORA - JAVASCRIPT
// ========================================

const API_URL = '/bitacora';
let paginaActual = 1;
const registrosPorPagina = 15;
let todosLosRegistros = [];
let totalRegistros = 0;
let totalPaginas = 0;

// Filtros
let filtrosActuales = {
    fechaInicio: '',
    fechaFin: '',
    usuario: '',
    accion: '',
    soloErrores: false
};

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

function actualizarPaginacion() {
    const container = document.getElementById('paginacionContainer');

    if (totalPaginas <= 1) {
        container.style.display = 'none';
        return;
    }

    container.style.display = 'block';

    const inicio = (paginaActual - 1) * registrosPorPagina + 1;
    const fin = Math.min(paginaActual * registrosPorPagina, totalRegistros);

    document.getElementById('inicioMostrar').textContent = inicio;
    document.getElementById('finMostrar').textContent = fin;
    document.getElementById('totalRegistros').textContent = totalRegistros;
    document.getElementById('numeroPagina').textContent = paginaActual;

    document.getElementById('btnAnterior').disabled = paginaActual === 1;
    document.getElementById('btnSiguiente').disabled = paginaActual === totalPaginas;
}

function paginaAnterior() {
    if (paginaActual > 1) {
        paginaActual--;
        cargarBitacoraConFiltros();
    }
}

function paginaSiguiente() {
    if (paginaActual < totalPaginas) {
        paginaActual++;
        cargarBitacoraConFiltros();
    }
}

// ========================================
// FUNCIONES DE MODALES
// ========================================

function abrirModalDetalle(registro) {
    const contenido = document.getElementById('detalleContenido');

    let accionHtml = registro.accion;
    let detalleHtml = '';

    // Si hay detalle JSON, formatearlo
    if (registro.detalleJson) {
        try {
            const jsonObj = JSON.parse(registro.detalleJson);
            detalleHtml = `
                <div style="margin-top:8px;">
                    <strong>Detalle JSON:</strong>
                    <pre style="background:#f5f5f5; padding:10px; border-radius:8px; overflow-x:auto; font-size:12px; max-height:300px; overflow-y:auto;">${JSON.stringify(jsonObj, null, 2)}</pre>
                </div>
            `;
        } catch (e) {
            detalleHtml = `
                <div style="margin-top:8px;">
                    <strong>Detalle:</strong>
                    <div style="background:#f5f5f5; padding:10px; border-radius:8px; font-size:12px;">${registro.detalleJson}</div>
                </div>
            `;
        }
    }

    // Si es error, mostrar con estilo diferente
    const errorBadge = registro.esError ?
        '<span class="badge-items-danger" style="margin-left:8px;">ERROR</span>' : '';

    contenido.innerHTML = `
        <div style="display:grid; grid-template-columns:1fr 1fr; gap:12px; font-size:14px;">
            <div><strong>ID:</strong> ${registro.id}</div>
            <div>
                <strong>Usuario:</strong> 
                <span class="badge-items">${registro.usuario}</span>
                ${errorBadge}
            </div>
            <div style="grid-column: span 2;">
                <strong>Acción:</strong>
                <div style="margin-top:4px; background:#f8f9fa; padding:12px; border-radius:8px; border:1px solid #dee2e6; font-size:13px;">
                    ${accionHtml}
                </div>
            </div>
            ${detalleHtml}
            <div style="grid-column: span 2;">
                <strong>Fecha:</strong>
                <div style="margin-top:4px;">${new Date(registro.fecha).toLocaleString('es-CR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit'
    })}</div>
            </div>
        </div>
    `;

    document.getElementById('modalDetalle').style.display = 'flex';
}

function cerrarModalDetalle() {
    document.getElementById('modalDetalle').style.display = 'none';
}

// ========================================
// FUNCIONES DE FILTROS
// ========================================

function aplicarFiltros() {
    const fechaInicio = document.getElementById('filtroFechaInicio').value;
    const fechaFin = document.getElementById('filtroFechaFin').value;
    const usuario = document.getElementById('filtroUsuario').value.trim();
    const accion = document.getElementById('filtroAccion').value.trim();
    const soloErrores = document.getElementById('filtroSoloErrores').checked;

    filtrosActuales = {
        fechaInicio,
        fechaFin,
        usuario,
        accion,
        soloErrores
    };

    paginaActual = 1;
    cargarBitacoraConFiltros();
}

function limpiarFiltros() {
    document.getElementById('filtroFechaInicio').value = '';
    document.getElementById('filtroFechaFin').value = '';
    document.getElementById('filtroUsuario').value = '';
    document.getElementById('filtroAccion').value = '';
    document.getElementById('filtroSoloErrores').checked = false;

    filtrosActuales = {
        fechaInicio: '',
        fechaFin: '',
        usuario: '',
        accion: '',
        soloErrores: false
    };

    paginaActual = 1;
    cargarBitacoraConFiltros();
}

// ========================================
// CRUD - LISTAR CON FILTROS
// ========================================

function mostrarRegistros(registros) {
    const container = document.getElementById('listaBitacora');

    if (!registros || registros.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                <i class="fas fa-history fa-4x"></i>
                <p>No hay registros en la bitacora que coincidan con los filtros</p>
            </div>
        `;
        return;
    }

    let html = '';
    registros.forEach(function (r) {

        let accionCompleta = r.accion;

        if (r.detalleJson) {
            accionCompleta += "\n\n" + r.detalleJson;
        }

        const fecha = new Date(r.fecha);
        const fechaFormateada = fecha.toLocaleString('es-CR', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });

        // Determinar el color según el tipo de acción
        let badgeColor = 'badge-items';
        if (r.esError) {
            badgeColor = 'badge-items-danger';
        } else if (r.accion.toLowerCase().includes('creó') || r.accion.toLowerCase().includes('creado')) {
            badgeColor = 'badge-items-success';
        } else if (r.accion.toLowerCase().includes('eliminó') || r.accion.toLowerCase().includes('eliminado')) {
            badgeColor = 'badge-items-danger';
        } else if (r.accion.toLowerCase().includes('modificó') || r.accion.toLowerCase().includes('actualizó')) {
            badgeColor = 'badge-items-warning';
        } else if (r.accion.toLowerCase().includes('consulta')) {
            badgeColor = 'badge-items-info';
        }

        const errorIcon = r.esError ? '<i class="fas fa-exclamation-triangle" style="color:#dc3545; margin-right:4px;"></i>' : '';

        html += `
            <div class="card-bordered">
                <div class="card-bordered-header">
                    <h3><i class="fas fa-user"></i> ${r.usuario}</h3>
                    <span class="${badgeColor}">${fechaFormateada}</span>
                </div>
                <div class="card-bordered-body">
                    <div class="info-row">
                        <span class="label"><i class="fas fa-info-circle"></i> Acción:</span>
                            <span class="value">
                                ${errorIcon}
                                <pre class="json-bitacora">${accionCompleta}</pre>
                            </span>
                    </div>
                </div>
            </div>
        `;
    });

    container.innerHTML = html;
}

async function cargarBitacoraConFiltros() {
    const container = document.getElementById('listaBitacora');
    mostrarLoading(true);

    try {
        // Construir URL con parámetros
        let url = `${API_URL}/filtros?`;
        url += `pagina=${paginaActual}`;
        url += `&tamanoPagina=${registrosPorPagina}`;
        url += `&soloErrores=${filtrosActuales.soloErrores}`;

        if (filtrosActuales.fechaInicio) {
            url += `&fechaInicio=${encodeURIComponent(filtrosActuales.fechaInicio)}`;
        }
        if (filtrosActuales.fechaFin) {
            url += `&fechaFin=${encodeURIComponent(filtrosActuales.fechaFin)}`;
        }
        if (filtrosActuales.usuario) {
            url += `&usuario=${encodeURIComponent(filtrosActuales.usuario)}`;
        }
        if (filtrosActuales.accion) {
            url += `&accion=${encodeURIComponent(filtrosActuales.accion)}`;
        }

        const response = await fetch(url);

        if (!response.ok) throw new Error('Error al cargar');

        const data = await response.json();

        todosLosRegistros = data.registros || [];
        totalRegistros = data.totalRegistros || 0;
        totalPaginas = data.totalPaginas || 0;
        paginaActual = data.paginaActual || 1;

        if (!todosLosRegistros || todosLosRegistros.length === 0) {
            container.innerHTML = `
                <div class="empty-state">
                    <i class="fas fa-history fa-4x"></i>
                    <p>No hay registros en la bitacora que coincidan con los filtros</p>
                </div>
            `;
            document.getElementById('paginacionContainer').style.display = 'none';
            mostrarLoading(false);
            return;
        }

        mostrarRegistros(todosLosRegistros);
        actualizarPaginacion();
    } catch (error) {
        container.innerHTML = `
            <div class="alert alert-danger">
                <i class="fas fa-exclamation-circle"></i> Error al cargar bitacora: ${error.message}
            </div>
        `;
        document.getElementById('paginacionContainer').style.display = 'none';
    }

    mostrarLoading(false);
}

async function cargarBitacora() {
    // Resetear filtros y cargar
    limpiarFiltros();
}

// ========================================
// INICIALIZAR
// ========================================

document.addEventListener('DOMContentLoaded', function () {
    cargarBitacora();
});
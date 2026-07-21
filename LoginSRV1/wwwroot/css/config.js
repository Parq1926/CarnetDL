// ========================================
// 📡 CONFIGURACIÓN DE MICROSERVICIOS
// ========================================

let MICROSERVICES = {};

// ========================================
// 📡 CARGAR CONFIGURACIÓN DESDE EL BACKEND
// ========================================

async function loadMicroservicesConfig() {
    try {
        const response = await fetch('/api/microservices');
        if (!response.ok) {
            throw new Error('Error al cargar configuración de microservicios');
        }
        const config = await response.json();
        MICROSERVICES = config;
        console.log('✅ Configuración de microservicios cargada:', MICROSERVICES);
        return MICROSERVICES;
    } catch (error) {
        console.error('❌ Error al cargar configuración:', error);
        return null;
    }
}

// ========================================
// 📡 FUNCIÓN PARA ABRIR MICROSERVICIOS
// ========================================

function abrirMicroservicio(serviceName) {
    const token = localStorage.getItem('accessToken');
    if (!token) {
        alert('Debe iniciar sesión primero');
        window.location.href = '/Login';
        return;
    }

    const service = MICROSERVICES[serviceName];
    if (!service) {
        console.error(`❌ Microservicio "${serviceName}" no encontrado`);
        return;
    }

    const url = service.url + service.path;
    console.log(`📡 Abriendo ${serviceName}: ${url}`);
    window.open(url, '_blank');
}

// ========================================
// 📡 FUNCIONES ABREVIADAS
// ========================================

function abrirUsuarios() { abrirMicroservicio('usuariosSRV4'); }
function abrirTiposUsuario() { abrirMicroservicio('tiposUsuarioSRV5'); }
function abrirTiposIdentificacion() { abrirMicroservicio('tipoIdentificacionSRV6'); }
function abrirCarreras() { abrirMicroservicio('carrerasSRV3'); }
function abrirAreas() { abrirMicroservicio('areasSRV4'); }
function abrirInstituciones() { abrirMicroservicio('institucionesSRV'); }
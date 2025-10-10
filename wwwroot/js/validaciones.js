// Validaciones adicionales para formularios
function mostrarMensajeError(mensaje) {
    console.error('Error de validación:', mensaje);
}

function validarFormulario(form) {
    const inputs = form.querySelectorAll('input[required]');
    let isValid = true;
    let errorCount = 0;
    
    inputs.forEach(input => {
        if (!input.value.trim()) {
            isValid = false;
            errorCount++;
            input.classList.add('error');
        } else {
            input.classList.remove('error');
        }
    });
    
    if (errorCount > 0) {
        console.log(`Se encontraron ${errorCount} errores`);
    }
    return isValid;
}
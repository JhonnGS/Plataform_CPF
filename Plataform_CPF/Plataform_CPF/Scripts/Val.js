function Mensaje() {
    if ($("#mensaje").val() == "1") {
        InfoTime("Mensaje", "Información enviada, ha sido registrado en CPF digital");
    } if ($("#mensaje").val == "0") {
        InfoTime("Mensaje", "Error del servidor información no enviada");
    }
}

function comprueba_extension(archivo) {

    var file = document.getElementById(archivo).value;
    extensiones_permitidas = new Array(".jpg");

    mierror = "";
    if (!file) {
    } else {

        extension = (file.substring(file.lastIndexOf("."))).toLowerCase();
        permitida = false;
        for (var i = 0; i < extensiones_permitidas.length; i++) {
            if (extensiones_permitidas[i] == extension) {
                permitida = true;
            }
        }
        if (!permitida) {
            InfoTime("Alerta", 'Sólo se permiten cargar archivos con extensiones: ".jpg"');
            var vacio = document.getElementById(archivo).value = "";
        } else {
            return 1;
        }
    }
    return 0;

}

function ValidarND(input, id) {
    var valor = input;
    const re = /^[a-z0-9A-ZÀ-ÿ\'\u00f1\u00d1]+(\s*[a-z0-9A-ZÀ-ÿ\'\u00f1\u00d1]*)(\s*[a-z0-9A-ZÀ-ÿ\'\u00f1\u00d1]*)+$/g;
    var validado = valor.match(re);
    if (!validado) {
        document.getElementById(id).value = "";
        InfoTime("Advertencia", "El campo solo acepta letras, numeros y letras con acentos, no debe ir vacío, no acepta signos, ni puntuaciones($#%.,;)");
        return false;
    } else {

    }
}

function Tel(input, id) {
    var tel = input;
    const re = /^([0-9])*$/;
    var validado = tel.match(re);

    if (tel.length == 10 && validado || tel.length == 12 && validado || tel.length == 13 && validado) {

    } else {
        document.getElementById(id).value = "";
        InfoTime("Advertencia", "DEBE INGRESAR UN TELÉFONO CORRECTO (SOLO SE ACEPTAN NUMEROS)");
        return false;
    }
}

function cal(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    if (tecla == 8) { return true; }
    patron = /[A-Za-z0-9ñÑ\u0020\À-ÿ]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

function ValidaCorreo(input, id) {
    var valor = input;
    const re = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    var validado = valor.match(re);
    if (!validado) {
        document.getElementById(id).value = "";
        InfoTime("Advertencia", " CORREO INCORRECTO");
        return false;
    } else {

    }
}

function Password(input, id) {
    var valor = input;
    const re = /^[a-z0-9_-]{6,8}$/
    var validado = valor.match(re);
    if (!validado) {
        document.getElementById(id).value = "";
        InfoTime("Advertencia", "Este campo solo permite de 6 a 8 caracteres, no permite espacios en blancos($#%.,;)");
        return false;
    } else {
    }
}

function UsuarioSol() {
    var user = $("#idUsuario").val();
    
    $.getJSON('/Alumnos/obtenerUser', { Usuarios: user }, function (data) {
        $.each(data, function () {
            console.log(data[0].usuario);
        });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        InfoTime("Advertencia", "Error al consultar datos");
    });
    console.log(user);
}

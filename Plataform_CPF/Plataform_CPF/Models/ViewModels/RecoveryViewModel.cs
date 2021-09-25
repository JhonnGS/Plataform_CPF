using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Plataform_CPF.Models.ViewModels
{
    //ESTE METODO ES EL QUE FUNCIONA COMO TABLA DE UN VIEWMODELS PARA DESPUES OCUPAR LOS PARAMETROS EN LA VISTA PRIMERA DE RECUPERA CONTRASEÑA
    public class RecoveryViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
        public string Email { get; set; }
    }
}
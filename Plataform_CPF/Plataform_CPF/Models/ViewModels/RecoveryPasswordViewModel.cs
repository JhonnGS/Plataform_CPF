using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Plataform_CPF.Models.ViewModels
{
    //ESTA ES LA CLASE QUE FUNCIONA COMO UNA TABLA DE VIEWMODEL AHI LA OCUPAMOS PARA LA VISTA DE RECUPERA CONTRASEÑA (CAMBIO DE DATOS)
    public class RecoveryPasswordViewModel
    {
        public string token { get; set; }
        [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
        public string Password { get; set; }
        [Compare("Password")]
        [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
        public string Password2 { get; set; }
    }
}
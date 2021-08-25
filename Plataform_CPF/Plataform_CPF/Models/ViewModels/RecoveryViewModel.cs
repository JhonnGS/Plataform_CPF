using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Plataform_CPF.Models.ViewModels
{
    public class RecoveryViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
        public string Email { get; set; }
    }
}
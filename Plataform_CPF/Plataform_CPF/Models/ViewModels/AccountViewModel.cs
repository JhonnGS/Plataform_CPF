using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Plataform_CPF.Models.ViewModels
{
      public class VerifyCodeViewModel
        {
            [Required]
            public string Provider { get; set; }

            [Required]
            [Display(Name = "Código")]
            public string Code { get; set; }
            public string ReturnUrl { get; set; }

            [Display(Name = "¿Recordar este explorador?")]
            public bool RememberBrowser { get; set; }

            public bool RememberMe { get; set; }
        }

        public class ForgotViewModel
        {
            [Required]
            [Display(Name = "Correo electrónico")]
            public string Email { get; set; }
        }

        public class LoginViewModel
        {
            [Required]
            [Display(Name = "Correo electrónico")]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [Display(Name = "¿Recordar cuenta?")]
            public bool RememberMe { get; set; }
        }

        public class RegisterViewModel
        {
            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "NOMBRE")]
            public string Name { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "APELLIDO PATERNO")]
            public string App { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "APELLIDO MATERNO")]
            public string Apm { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "DIRECCIÓN")]
            public string Address { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "TELEFONO")]
            public string Telephone { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [EmailAddress]
            [Display(Name = "CORREO ELECTRÓNICO")]
            public string Email { get; set; }


            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "SEXO")]
            public string Sex { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "GRADO")]
            public string Grade { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "GRUPO")]
            public string Group { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "SECCIÓN")]
            public string Seccion { get; set; }

        }

        public class UserMD
        {
            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "NOMBRE")]
            public string Name { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "APELLIDO PATERNO")]
            public string App { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "APELLIDO MATERNO")]
            public string Apm { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "SEXO")]
            public string Sex { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [EmailAddress]
            [Display(Name = "CORREO ELECTRÓNICO")]
            public string Email { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "DIRECCIÓN")]
            public string Address { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "TELEFONO")]
            public string Telephone { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "SECCIÓN")]
            public string Seccion { get; set; }
        }

        public class UserAT
        {
            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "NOMBRE")]
            public string Name { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "APELLIDO PATERNO")]
            public string App { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "APELLIDO MATERNO")]
            public string Apm { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "SEXO")]
            public string Sex { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [EmailAddress]
            [Display(Name = "CORREO ELECTRÓNICO")]
            public string Email { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "DIRECCIÓN")]
            public string Address { get; set; }

            [Required(ErrorMessage = "EL CAMPO ES REQUERIDO")]
            [Display(Name = "TELEFONO")]
            public string Telephone { get; set; }

        }

        public class ResetPasswordViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Correo electrónico")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public class ForgotPasswordViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Correo electrónico")]
            public string Email { get; set; }
        }

    }

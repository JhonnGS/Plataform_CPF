using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Plataform_CPF.Models.ViewModels
{
      //CLASE METODO QUE FUNCIONA COMO UNA TABLA DENTRO DEL SISTEMA DEL CUAL SE LLAMAN SUS PARAMETROS EN UNA VISTA EN ESTE CASO LA VISTA DE LOGUE
    
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
    //METODO QUE FUNCIONA COMO UNA TABLA DEL MODELO ES PARA LOS FORMULARIOS DE REGISTRO ALUMNO EN EL SE LLAMAN LOS PARAMETROS COMO EL NOMBRE O EL APP
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

    //METODO QUE FUNCIONA COMO UNA TABLA DEL MODELO ES PARA LOS FORMULARIOS DE REGISTRO MAESTRO O DIRECTOR
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

    //METODO QUE FUNCIONA COMO UNA TABLA DEL MODELO ES PARA LOS FORMULARIOS DE REGISTRO ADMINISTRADOR TUTOR
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


    }

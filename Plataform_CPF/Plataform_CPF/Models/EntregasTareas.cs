//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Plataform_CPF.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EntregasTareas
    {
        public int idEntregasTarea { get; set; }
        public int idTarea { get; set; }
        public System.DateTime fecha_E { get; set; }
        public byte[] tarea { get; set; }
        public int puntos { get; set; }
        public int idAlumno { get; set; }
    
        public virtual Alumnos Alumnos { get; set; }
        public virtual Tareas Tareas { get; set; }
    }
}
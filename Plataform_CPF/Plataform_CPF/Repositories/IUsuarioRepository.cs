using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plataform_CPF.Models;

namespace Plataform_CPF.Repositories
{
    public interface IUsuarioRepository
    {
       //CLASE QUE CON LA CUAL HACEMOS CONSULTA A LA BASE DE DATOS ATRAVEZ DE UN PARAMETRO (CORREO)
       //CON EL CUAL OBTENEMOS LOS DATOS GENERALES DE DICHO USUARIO PARA DESPUES HACER UNA COMPARACION
        /// <summary>
        /// Busca por correo 
        /// </summary>
        /// <param name="correo">correo</param>
        /// <returns>Objeto usuario</returns>
        Usuarios getPorCorreo(string correo);
        //OBTIENE LOS DATOS DEL USUARIO ATRAVEZ DEL CORREO                    
        
    }
}
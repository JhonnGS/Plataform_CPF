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
       
        /// <summary>
        /// Busca por correo 
        /// </summary>
        /// <param name="correo">correo</param>
        /// <returns>Objeto usuario</returns>
        Usuarios getPorCorreo(string correo);
        //UsuariosSet getPorUserid(int? id);                     
        
    }
}
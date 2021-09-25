using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Plataform_CPF.Models;

namespace Plataform_CPF.Repositories
{

    public class UsuarioRepository : IUsuarioRepository
    {
        public BDCPFORIEntities db;
         //DECLARA UNA VARIABLE DONDE HACE UNA INSTANCIA DE LA BD
        public UsuarioRepository()
        {
            this.db = new BDCPFORIEntities();
        }
    
        //METODO QUE NOS AYUDA A OBTENER LOS DATOS DEL USUARIO ATRAVEZ DEL CORREO PARA DESPUES COMPARAR LOS DATOS CON UNA VARIABLE CREADA
        public Usuarios getPorCorreo(string correo)
        {
            var query = (from u in db.Usuarios
                         where u.correo == correo
                         select u);
            return query.FirstOrDefault<Usuarios>();
        }
        
    }
}
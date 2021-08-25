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

        public UsuarioRepository()
        {
            this.db = new BDCPFORIEntities();
        }
    
        public Usuarios getPorCorreo(string correo)
        {
            var query = (from u in db.Usuarios
                         where u.correo == correo
                         select u);
            return query.FirstOrDefault<Usuarios>();
        }
        
    }
}
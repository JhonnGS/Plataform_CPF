using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Net;
using Plataform_CPF.Models;

using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Plataform_CPF.Models.ViewModels;
using Plataform_CPF.Repositories;
using System.Data.Entity.Validation;
using System.Net.Mail;
using System.Security.Cryptography;
using System.IO;
namespace Plataform_CPF.Controllers
{
    public class DirectoresController : Controller
    {
        private BDCPFORIEntities db = new BDCPFORIEntities();
        private IUsuarioRepository _repoUsuario;

        //CLASE QUE ESTANCIA A OTERA CLASE DE LA CARPETA REPOSITORIES POR MEDIO DE LA VARIABLE repository
        public DirectoresController(IUsuarioRepository repository)
        {
            _repoUsuario = repository;
        }

        public DirectoresController() : this(new UsuarioRepository())
        {

        }

        //METODO PARA CREAR VISTA PARCIAL MASTERPAGE LA CUAL UTILIZAREMOS EN TODAS LAS INTERFACES 
        //SI IDENTIFICA A LA SESION DIFERENTE DE NULLO OSEA SEA HAY ALGO RETORNA ESTA VISTA DE LO CONTRARIO REGRESA AL LOGIN
        public ActionResult _LayoutMD(int? id)
        {
            if (Session["UserD"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //ESTE ES EL METODO PARA CREAR LA VISTA DE BIENVENIDO QUE SERA LA QUE MUESTRE PRIMERO AL ENTRAR A SU SESION
        public ActionResult HomeD(int? mesg, int? id)
        {
            return View();
        }

        // GET: ADMINISTRADOR   
        //METODO QUE CREA LA VISTA PARA EDITAR LOS DATOS DEL USUARIO POR MEDIO DEL ID
        public ActionResult DPerfil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios u = db.Usuarios.Find(id);
            if (u == null)
            {
                return HttpNotFound();
            }
            return View(u);
        }

        //METODO QUE COMUNICA AL MODELO LA ACTULIZACION DE LOS DATOS DEL USUARIO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DPerfil([Bind(Include = "idUsuario,usuario,correo,contraseña,perfil,status,TokenRecovery")] Usuarios u)
        {
            if (ModelState.IsValid)
            {
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("HomeD");
            }
            return View(u);
        }

        //METODO QUE CREA LA VISTA PARA EDITAR LOS DATOS POR MEDIO DE UN ID
        public ActionResult DConfiguracion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directores d = db.Directores.Find(id);
            if (d == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario = new SelectList(db.Usuarios, "idUsuario", "usuario", d.idUsuario);
            return View(d);
        }

        //METODO QUE HACE EL ENVIO DE DATOS PARA ACTUALIZAR EN LA TABLA DEL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DConfiguracion([Bind(Include = "idDirector,nombre,app,apm,sexo,direccion,telefono,seccion,idUsuario")] Directores d)
        {
            if (ModelState.IsValid)
            {
                db.Entry(d).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("HomeD");
            }
            ViewBag.idUsuario = new SelectList(db.Usuarios, "idUsuario", "usuario", d.idUsuario);
            return View(d);
        }

        //METODO POR DEFAULT QUE SE ENCARGA DE VERIFICAR SI LA BD ESTA DISPONIBLE
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //METODO QUE NOS CERRARA LA SESION CUANDO PUCHEMOS EN LA VISTA EL BOTON CERRAR SESION
        public ActionResult CerrarSesion()
        {
            Session["idUs"] = null;
            Session["nombre"] = null;
            Session["UserAD"] = null;
            ViewBag.M = "USTED HA SALIDO DE SU SESIÓN";
            return RedirectToAction("Login", "Account");
        }
    }
}
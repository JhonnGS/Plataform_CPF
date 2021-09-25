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
    public class AlumnosController : Controller
    {
        private BDCPFORIEntities db = new BDCPFORIEntities();
        private IUsuarioRepository _repoUsuario;

        //CLASE QUE ESTANCIA A OTERA CLASE DE LA CARPETA REPOSITORIES POR MEDIO DE LA VARIABLE repository
        public AlumnosController(IUsuarioRepository repository)
        {
            _repoUsuario = repository;
        }

        public AlumnosController() : this(new UsuarioRepository())
        {

        }

        //METODO PARA CREAR VISTA PARCIAL MASTERPAGE LA CUAL UTILIZAREMOS EN TODAS LAS INTERFACES 
        //SI IDENTIFICA A LA SESION DIFERENTE DE NULLO OSEA SI HAY ALGO RETORNA ESTA VISTA DE LO CONTRARIO REGRESA AL LOGIN
        public ActionResult _LayoutMA(int? id)
        {
            if (Session["UserA"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //ESTE ES EL METODO PARA CREAR LA VISTA DE BIENVENIDO QUE SERA LA QUE MUESTRE PRIMERO AL ENTRAR A SU SESION
        public ActionResult HomeA(int? mesg, int? id)
        {
            return View();
        }

        
        //METODO QUE CREA LA VISTA PARA EDITAR LOS DATOS DEL USUARIO POR MEDIO DEL ID
        public ActionResult APerfil(int? id)
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

        //METODO QUE COMUNICA AL MODELO LA ACTUALIZACION DE LOS DATOS DEL USUARIO(SE TRATA DE UNA VISTA EDITAR(EDIT))
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult APerfil([Bind(Include = "idUsuario,usuario,correo,contraseña,perfil,status,TokenRecovery")] Usuarios u)
        {
            if (ModelState.IsValid)
            {
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("HomeA");
            }
            return View(u);
        }

        //METODO QUE CREA LA VISTA PARA EDITAR LOS DATOS POR MEDIO DE UN ID DE LA TABLA ALUMNOS
        public ActionResult AConfiguracion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumnos A = db.Alumnos.Find(id);
            if (A == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario = new SelectList(db.Usuarios, "idUsuario", "usuario", A.idUsuario);
            return View(A);
        }

        //METODO QUE HACE EL ENVIO DE DATOS PARA ACTUALIZAR EN LA TABLA CORRESPONDIENTE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AConfiguracion([Bind(Include = "idAlumno,nombre,app,apm,sexo,direccion,telefono,seccion,grado,grupo,idUsuario")] Alumnos a)
        {
            if (ModelState.IsValid)
            {
                db.Entry(a).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("HomeA");
            }
            ViewBag.idUsuario = new SelectList(db.Usuarios, "idUsuario", "usuario", a.idUsuario);
            return View(a);
        }

        //ESTE METODO LO QUE HACE ES MOSTRAR UNA VISTA CON UNA LISTA DE EKLEMNTOS CARGADOS EN SU SESION
        public ActionResult Mochila(int? id)
        {
            //ViewBag.idAlumno = new SelectList(db.Usuarios, "idUsuario", "nombre");
            return View(db.Mochila.ToList());
        }
        //ESTE METODO LO QUE HACE ES CREAR LA VISTA QUE RECIBIRA LOS DATOS A CARGAR 
        public ActionResult DocMochila(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mochila DOC = db.Mochila.Find(id);
            if (DOC == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUSERAL = new SelectList(db.Usuarios, "idUsuario", "usuario", DOC.idUsuario);
            List<Usuarios> user = db.Usuarios.ToList(); /*new List<usuarios>();*/
            ViewBag.usuarios = user;                     //Usuarios.Add(new usuario());
            return View(DOC);
        }
        
        //METODO QUE RECIBE LOS DATOS ASI COMO EL ELEMENTO CARGADO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DocMochila([Bind(Include = "idMochila,nombre,elemento,fecha_subido,descripcion,status,idUsuario")] Mochila Doc, HttpPostedFileBase file)
        {
            Mochila doc = new Mochila();
            doc = db.Mochila.Find(Doc.idMochila);

            if (file != null)
            {
                var filecarga = System.IO.Path.GetFileName(file.FileName);

                var extension = System.IO.Path.GetExtension(filecarga);

                if (extension == ".pdf")
                {


                    doc.elemento = new byte[file.ContentLength];
                    //doc.extension = extension;
                    doc.fecha_subido = DateTime.Now;

                    file.InputStream.Read(doc.elemento, 0, file.ContentLength);
                    System.Diagnostics.Debug.Write("Extensión..." + extension);

                    try
                    {
                        doc.status = "CARGADO";
                        db.Entry(doc).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("Mochila", new { mensaje = 1 });
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.Write(e);
                        System.Diagnostics.Debug.Write(" - Error al actualizar el registro");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.Write("El archivo no es aceptable");
                }
            }
            else
            {
                System.Diagnostics.Debug.Write("No se cargo ningun archivo");
            }
            ViewBag.idproveedor = new SelectList(db.Usuarios, "idUsuario", "usuario", doc.idUsuario);
            return View(doc);
        }

        //METODO QUE CREA UNA LISTA DE USUARIOS
        public ActionResult obtenerUS(int usuarios)
        {
            List<Usuarios> valor = new List<Usuarios>();
            Usuarios US = new Usuarios();
            US = db.Usuarios.Find(usuarios);//Hacemos la busqueda en la BD de la tabla
                                            //El metodo find hace la busqueda por el id de la tabla
            valor.Add(US);

            return Json(valor, JsonRequestBehavior.AllowGet);
        }

        //METODO QUE CREA LA VISTA DONDE VEREEMOS UNA LISTA DE MATERIAS CARGADAS
        public ActionResult Materia()
        {
            //ViewBag.idAlumno = new SelectList(db.Usuarios, "idUsuario", "nombre");
            return View();
        }

        //METODO PARA CARGAR UNA MATERIA NUEVA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult cargarMateria([Bind(Include = "idMateria,nombre")] Materias ma)
        {
            if (ModelState.IsValid)
            {
                db.Materias.Add(ma);
                db.SaveChanges();

                return RedirectToAction("Materia");
            }

            return View(ma);
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
            Session["UserA"] = null;
            ViewBag.M = "USTED HA SALIDO DE SU SESIÓN";
            return RedirectToAction("Login", "Account");
        }
    }
}
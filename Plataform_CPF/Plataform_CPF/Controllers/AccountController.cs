using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Plataform_CPF.Models;
using Plataform_CPF.Models.ViewModels;
using Plataform_CPF.Repositories;
using System.Data.Entity.Validation;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Data.Entity;
using System.Net;
using System.Data;

namespace Plataform_CPF.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //private BDCPFprEntities db = new BDCPFprEntities();       
        string urlDomain = "http://localhost:50260/";

        private IUsuarioRepository _repoUsuario;

        public AccountController(IUsuarioRepository repository)
        {
            _repoUsuario = repository;
        }

        public AccountController() : this(new UsuarioRepository())
        {

        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(int? mesg)
        {
            ViewBag.mensaje = mesg;

            if (ViewBag.mensaje == 1)
            {
                ViewBag.Error = "EL USUARIO NO EXISTE";
            }
            if (ViewBag.mensaje == 2)
            {
                ViewBag.Error = "LA CONTRASEÑA ES INCORRECTA";
            }
            if (ViewBag.mensaje == 3)
            {
                ViewBag.Error = "USUARIO BLOQUEADO CONTACTE AL ADMINISTRADOR <23........>";
            }
            if (ViewBag.mensaje == 4)
            {
                ViewBag.Message = "CONTRASEÑA MODIFICADA CON EXITO";
            }

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, FormCollection frm, Usuarios objUser)
        {
            string E = frm["Email"];
            string pass = frm["password"];

            try
            {
                using (BDCPFORIEntities db = new BDCPFORIEntities())
                {
                    var lst = from d in db.Usuarios
                              where d.correo == E.Trim() && d.contraseña == pass.Trim()
                              select d;

                    Usuarios objUsuario = _repoUsuario.getPorCorreo(E);
                    if (objUsuario == null)
                    {
                        return RedirectToAction("Login", "Account", new { mesg = 1 });

                    }
                    if ((objUsuario.contraseña) != pass)
                    {
                        return RedirectToAction("Login", "Account", new { mesg = 2 });
                    }
                    if (objUsuario.status == "BLOQUEADO")
                    {
                        return RedirectToAction("Login", "Account", new { mesg = 3 });
                    }

                    else
                    {
                        if (objUsuario.perfil == "ALUMNO")
                        {
                            if (lst.Count() > 0)
                            {
                                Usuarios oUserA = lst.FirstOrDefault();
                                Session["UserA"] = oUserA.usuario;
                                return RedirectToAction("HomeA", "Alumnos", new { mesg = 0 });
                            }

                        }
                        if (objUsuario.perfil == "MAESTRO")
                        {
                            if (lst.Count() > 0)
                            {
                                Usuarios oUserM = lst.FirstOrDefault();
                                Session["UserM"] = oUserM.usuario;
                                return RedirectToAction("HomeT", "Teacher", new { mesg = 0 });
                            }
                        }
                        if (objUsuario.perfil == "TUTOR")
                        {
                            if (lst.Count() > 0)
                            {
                                Usuarios oUserT = lst.FirstOrDefault();
                                Session["UserT"] = oUserT.usuario;
                                return RedirectToAction("", "tutor", new { mesg = 0 });
                            }
                        }
                        if (objUsuario.perfil == "DIRECTOR")
                        {
                            if (lst.Count() > 0)
                            {
                                Usuarios oUserD = lst.FirstOrDefault();
                                Session["UserD"] = oUserD.usuario;
                                return RedirectToAction("HomeD", "Directores", new { mesg = 0 });
                            }
                        }
                        if (objUsuario.perfil == "ADMINISTRADOR")
                        {
                            if (lst.Count() > 0)
                            {
                                Usuarios oUserAD = lst.FirstOrDefault();
                                Session["UserAD"] = oUserAD.usuario;
                                return RedirectToAction("HomeAd", "Administrador", new { mesg = 0 });
                            }
                        }
                    }
                }
                return View(objUser);
            }
            catch (Exception ex)
            {
                return Content("Ocurrio un error :(  " + ex.Message);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult StartRecovery()
        {
            Models.ViewModels.RecoveryViewModel model = new Models.ViewModels.RecoveryViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult StartRecovery(Models.ViewModels.RecoveryViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                string token = GetSha256(Guid.NewGuid().ToString());

                using (Models.BDCPFORIEntities db = new Models.BDCPFORIEntities())
                {
                    var oUser = db.Usuarios.Where(d => d.correo == model.Email).FirstOrDefault();
                    if (oUser != null)
                    {
                        oUser.TokenRecovery = token;
                        db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        //enviamos email
                        SendEmail(oUser.correo, token);
                    }
                }
                return View();
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Recovery(string token)
        {
            Models.ViewModels.RecoveryPasswordViewModel model = new Models.ViewModels.RecoveryPasswordViewModel();
            model.token = token;
            using (Models.BDCPFORIEntities db = new Models.BDCPFORIEntities())
            {
                if (model.token == null || model.token.Trim().Equals(""))
                {
                    return View("Login", "Account");
                }
                var oUser = db.Usuarios.Where(d => d.TokenRecovery == model.token).FirstOrDefault();
                if (oUser == null)
                {
                    ViewBag.Error = "TOKEN INCORRECTO CONTACTE AL ADMINISTRADOR<<>>";
                    return View("Login", "Account");
                }
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Recovery(Models.ViewModels.RecoveryPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                using (Models.BDCPFORIEntities db = new Models.BDCPFORIEntities())
                {
                    var oUser = db.Usuarios.Where(d => d.TokenRecovery == model.token).FirstOrDefault();

                    if (oUser != null)
                    {
                        oUser.contraseña = model.Password;
                        oUser.TokenRecovery = null;
                        db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return View("Login", "Account", new { mesg = 4 });
                    }
                }

            }
            catch (Exception ex)
            {
                return View(model);
                throw new Exception(ex.Message);
            }

            return View();
        }

        // GET: /Account/RA
        [AllowAnonymous]
        public ActionResult RA(int? mesg)
        {
            ViewBag.mensaje = mesg;

            if (ViewBag.mensaje == 1)
            {
                ViewBag.Message = "LOS DATOS FUERON REGISTRADOS";
                ViewBag.M = "INICIE SESIÓN EN EL ENLACE YA TENGO CUENTA QUE ESTA ABAJO DE LA PAGINA"; ;
            }
            if (ViewBag.mensaje == 2)
            {
                ViewBag.Error = "EL CORREO QUE INGRESO YA EXISTE EN EL SISTEMA";
            }
            if (ViewBag.mensaje == 0)
            {
                ViewBag.Error = "ERROR AL REGISTRAR";
            }

            return View();
        }

        // POST: /Account/RA
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RA(RegisterViewModel model, FormCollection frm)
        {
            string Nombre = frm["Name"];
            string App = frm["App"];
            string Apm = frm["Apm"];
            string Sexo = frm["Sex"];
            string Direccion = frm["Address"];
            string Telefono = frm["Telephone"];
            string Correo = frm["Email"];
            string Contraseña = frm["PASSWORD"];
            //string Foto = frm["foto"];
            string Seccion = frm["Seccion"];
            string Grado = frm["Grade"];
            string Grupo = frm["Group"];

            try
            {
                using (BDCPFORIEntities db = new BDCPFORIEntities())
                {
                    Usuarios objUsuario = _repoUsuario.getPorCorreo(Correo);
                    if (objUsuario == null)
                    {
                        Usuarios oUser = new Usuarios();
                        oUser.correo = Correo;
                        oUser.contraseña = Contraseña;
                        oUser.usuario = Nombre + " - " + App;
                        oUser.perfil = "ALUMNO";
                        oUser.status = "HABILITADO";
                        db.Usuarios.Add(oUser);
                        db.SaveChanges();

                        // Insertar un nuevo ALUMNO
                        Alumnos a = new Alumnos();
                        a.nombre = Nombre;
                        a.app = App;
                        a.apm = Apm;
                        a.sexo = Sexo;
                        a.direccion = Direccion;
                        a.telefono = Telefono;
                        a.seccion = Seccion;
                        a.grado = Grado;
                        a.grupo = Grupo;
                        a.idUsuario = oUser.idUsuario;

                        db.Alumnos.Add(a);
                        db.SaveChanges();
                        return RedirectToAction("RA", "Account", new { mesg = 1 });
                    }
                    return RedirectToAction("RA", "Account", new { mesg = 2 });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("RA", "Account", new { mesg = 0 });
            }
            return View(model);
        }

        // GET: /Account/RM
        [AllowAnonymous]
        public ActionResult RM(int? mesg)
        {
            ViewBag.mensaje = mesg;

            if (ViewBag.mensaje == 1)
            {
                ViewBag.Message = "LOS DATOS FUERON REGISTRADOS";
                ViewBag.M = "INICIE SESIÓN EN EL ENLACE YA TENGO CUENTA QUE ESTA ABAJO DE LA PAGINA"; ;
            }
            if (ViewBag.mensaje == 2)
            {
                ViewBag.Error = "EL CORREO QUE INGRESO YA EXISTE EN EL SISTEMA";
            }
            if (ViewBag.mensaje == 0)
            {
                ViewBag.Error = "ERROR AL REGISTRAR";
            }

            return View();
        }

        // POST: /Account/RM
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RM(UserMD model, FormCollection frm)
        {
            string Nombre = frm["Name"];
            string App = frm["App"];
            string Apm = frm["Apm"];
            string Sexo = frm["Sex"];
            string Direccion = frm["Address"];
            string Telefono = frm["Telephone"];
            string Correo = frm["Email"];
            string Contraseña = frm["PASSWORD"];
            //string Foto = frm["foto"];
            string Seccion = frm["Seccion"];

            try
            {
                using (BDCPFORIEntities db = new BDCPFORIEntities())
                {
                    Usuarios objUsuario = _repoUsuario.getPorCorreo(Correo);
                    if (objUsuario == null)
                    {
                        Usuarios oUser = new Usuarios();
                        oUser.correo = Correo;
                        oUser.contraseña = Contraseña;
                        oUser.usuario = Nombre + " - " + App;
                        oUser.perfil = "MAESTRO";
                        oUser.status = "HABILITADO";
                        db.Usuarios.Add(oUser);
                        db.SaveChanges();

                        // Insertar un nuevo MAESTRO
                        Maestros M = new Maestros();
                        M.nombre = Nombre;
                        M.app = App;
                        M.apm = Apm;
                        M.sexo = Sexo;
                        M.direccion = Direccion;
                        M.telefono = Telefono;
                        M.seccion = Seccion;
                        M.idUsuario = oUser.idUsuario;

                        db.Maestros.Add(M);
                        db.SaveChanges();
                        return RedirectToAction("RM", "Account", new { mesg = 1 });
                    }
                    return RedirectToAction("RM", "Account", new { mesg = 2 });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("RM", "Account", new { mesg = 0 });
            }
            return View(model);
        }

        // GET: /Account/RT
        [AllowAnonymous]
        public ActionResult RT(int? mesg)
        {
            ViewBag.mensaje = mesg;

            if (ViewBag.mensaje == 1)
            {
                ViewBag.Message = "LOS DATOS FUERON REGISTRADOS";
                ViewBag.M = "INICIE SESIÓN EN EL ENLACE YA TENGO CUENTA QUE ESTA ABAJO DE LA PAGINA"; ;
            }
            if (ViewBag.mensaje == 2)
            {
                ViewBag.Error = "EL CORREO QUE INGRESO YA EXISTE EN EL SISTEMA";
            }
            if (ViewBag.mensaje == 0)
            {
                ViewBag.Error = "ERROR AL REGISTRAR";
            }

            return View();
        }

        // POST: /Account/RT
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RT(UserAT model, FormCollection frm)
        {
            string Nombre = frm["Name"];
            string App = frm["App"];
            string Apm = frm["Apm"];
            string Sexo = frm["Sex"];
            string Direccion = frm["Address"];
            string Telefono = frm["Telephone"];
            string Correo = frm["Email"];
            string Contraseña = frm["PASSWORD"];
            //string Foto = frm["foto"];

            try
            {
                using (BDCPFORIEntities db = new BDCPFORIEntities())
                {
                    Usuarios objUsuario = _repoUsuario.getPorCorreo(Correo);
                    if (objUsuario == null)
                    {
                        Usuarios oUser = new Usuarios();
                        oUser.correo = Correo;
                        oUser.contraseña = Contraseña;
                        oUser.usuario = Nombre + " - " + App;
                        oUser.perfil = "TUTOR";
                        oUser.status = "HABILITADO";
                        db.Usuarios.Add(oUser);
                        db.SaveChanges();

                        // Insertar un nuevo TUTOR
                        Tutores T = new Tutores();
                        T.nombre = Nombre;
                        T.app = App;
                        T.apm = Apm;
                        T.sexo = Sexo;
                        T.direccion = Direccion;
                        T.telefono = Telefono;
                        T.idUsuario = oUser.idUsuario;

                        db.Tutores.Add(T);
                        db.SaveChanges();
                        return RedirectToAction("RT", "Account", new { mesg = 1 });

                    }
                    return RedirectToAction("RT", "Account", new { mesg = 2 });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("RT", "Account", new { mesg = 0 });
            }
            return View(model);
        }

        // GET: /Account/RD
        [AllowAnonymous]
        public ActionResult RD(int? mesg)
        {
            ViewBag.mensaje = mesg;

            if (ViewBag.mensaje == 1)
            {
                ViewBag.Message = "LOS DATOS FUERON REGISTRADOS";
                ViewBag.M = "INICIE SESIÓN EN EL ENLACE YA TENGO CUENTA QUE ESTA ABAJO DE LA PAGINA"; ;
            }
            if (ViewBag.mensaje == 2)
            {
                ViewBag.Error = "EL CORREO QUE INGRESO YA EXISTE EN EL SISTEMA";
            }
            if (ViewBag.mensaje == 0)
            {
                ViewBag.Error = "ERROR AL REGISTRAR";
            }

            return View();
        }

        // POST: /Account/RD
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RD(UserMD model, FormCollection frm)
        {
            string Nombre = frm["Name"];
            string App = frm["App"];
            string Apm = frm["Apm"];
            string Sexo = frm["Sex"];
            string Direccion = frm["Address"];
            string Telefono = frm["Telephone"];
            string Correo = frm["Email"];
            string Contraseña = frm["PASSWORD"];
            //string Foto = frm["foto"];
            string Seccion = frm["Seccion"];

            try
            {
                using (BDCPFORIEntities db = new BDCPFORIEntities())
                {
                    Usuarios objUsuario = _repoUsuario.getPorCorreo(Correo);
                    if (objUsuario == null)
                    {
                        Usuarios oUser = new Usuarios();
                        oUser.correo = Correo;
                        oUser.contraseña = Contraseña;
                        oUser.usuario = Nombre + " - " + App;
                        oUser.perfil = "DIRECTOR";
                        oUser.status = "HABILITADO";
                        db.Usuarios.Add(oUser);
                        db.SaveChanges();

                        // Insertar un nuevo DIRECTOR
                        Directores D = new Directores();
                        D.nombre = Nombre;
                        D.app = App;
                        D.apm = Apm;
                        D.sexo = Sexo;
                        D.direccion = Direccion;
                        D.telefono = Telefono;
                        D.seccion = Seccion;
                        D.idUsuario = oUser.idUsuario;

                        db.Directores.Add(D);
                        db.SaveChanges();
                        return RedirectToAction("RD", "Account", new { mesg = 1 });

                    }
                    return RedirectToAction("RD", "Account", new { mesg = 2 });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("RD", "Account", new { mesg = 0 });
            }
            return View(model);
        }

        // GET: /Account/IdentificarAdmin
        [AllowAnonymous]
        public ActionResult IdentificarAdmin(int? mesg)
        {
            ViewBag.mensaje = mesg;

            if (ViewBag.mensaje == 1)
            {
                ViewBag.error = "ERROR LA CLAVE NO COINCIDE!";
            }
            return View();
        }

        //POST: /Account/IdentificarAdmin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult IdentificarAdmin(FormCollection frm)
        {
            string clave = frm["clave"];
            if (clave == "TJK2508736")
            {
                return RedirectToAction("RAD", "Account", new { mesg = 0 });
            }
            else
            {
                return RedirectToAction("IdentificarAdmin", "Account", new { mesg = 1 });
            }

            return View();
        }

        // GET: /Account/RAD
        [AllowAnonymous]
        public ActionResult RAD(int? mesg)
        {
            ViewBag.mensaje = mesg;
            if (ViewBag.mensaje == 0)
            {
                ViewBag.M = "BIENVENIDO USUARIO ADMINISTRADOR ACONTINUACIÓN REGISTRAREMOS SUS DATOS";
            }
            if (ViewBag.mensaje == 1)
            {
                ViewBag.Message = "LOS DATOS FUERON REGISTRADOS";
                ViewBag.Mess = "INICIE SESIÓN EN EL ENLACE YA TENGO CUENTA QUE ESTA ABAJO";
            }
            if (ViewBag.mensaje == 2)
            {
                ViewBag.Error = "EL CORREO QUE INGRESO YA EXISTE EN EL SISTEMA";
            }
            if (ViewBag.mensaje == 3)
            {
                ViewBag.Error = "ERROR AL REGISTRAR LOS DATOS CONTACTE AL ADMINISTRADOR<<>>";
            }
            return View();
        }

        // POST: /Account/RAD
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RAD(UserAT model, FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                string Nombre = frm["Name"];
                string App = frm["App"];
                string Apm = frm["Apm"];
                string Sexo = frm["Sex"];
                string Direccion = frm["Address"];
                string Telefono = frm["Telephone"];
                string Correo = frm["Email"];
                string Contraseña = frm["PASSWORD"];
                //string Foto = frm["foto"];

                try
                {
                    using (BDCPFORIEntities db = new BDCPFORIEntities())
                    {
                        Usuarios objUsuario = _repoUsuario.getPorCorreo(Correo);

                        if (objUsuario == null)
                        {
                            Usuarios oUser = new Usuarios();
                            oUser.correo = Correo;
                            oUser.contraseña = Contraseña;
                            oUser.usuario = Nombre + " - " + App;
                            oUser.perfil = "ADMINISTRADOR";
                            oUser.status = "HABILITADO";
                            db.Usuarios.Add(oUser);
                            db.SaveChanges();

                            // Insertar un nuevo Admin
                            Administrador ad = new Administrador();
                            ad.nombre = Nombre;
                            ad.app = App;
                            ad.apm = Apm;
                            ad.sexo = Sexo;
                            ad.direccion = Direccion;
                            ad.telefono = Telefono;
                            ad.claveAdmin = "TJK2508736";
                            ad.idUsuario = oUser.idUsuario;

                            db.Administrador.Add(ad);
                            db.SaveChanges();

                            return RedirectToAction("RAD", "Account", new { mesg = 1 });

                        }

                        return RedirectToAction("RAD", "Account", new { mesg = 2 });
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("RAD", "Account", new { mesg = 3 });

                }
            }
            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }


        #region HELPERS
        private string GetSha256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding enconding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(enconding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        private void SendEmail(string EmailDestino, string token)
        {
            string EmailOrigen = "cescuela496@gmail.com";
            string Contraseña = "CPFD1617AL";
            string url = urlDomain + "/Account/Recovery/?token=" + token;
            MailMessage oMailMessage = new MailMessage(EmailOrigen, EmailDestino, "Recuperación de contraseña",
            "<p>Correo para recuperar su contraseña</p><br>" +
            "<a href='" + url + "'>Click para recuperarla<a/>");


            oMailMessage.IsBodyHtml = true;

            SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com");
            oSmtpClient.EnableSsl = true;
            oSmtpClient.UseDefaultCredentials = false;
            oSmtpClient.Port = 587;
            oSmtpClient.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);

            oSmtpClient.Send(oMailMessage);
            oSmtpClient.Dispose();
        }

        #endregion

          
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Plataform_CPF.Controllers;
using Plataform_CPF.Models;

namespace Plataforma_CPF.Filters
{
    public class VerifySesion : ActionFilterAttribute
    {
        private Usuarios oUsuario;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //private UsuariosSet oUsuario;

            var oUserA = (Usuarios)HttpContext.Current.Session["UserA"];
            var oUserM = (Usuarios)HttpContext.Current.Session["UserM"];
            var oUserT = (Usuarios)HttpContext.Current.Session["UserT"];
            var oUserD = (Usuarios)HttpContext.Current.Session["UserD"];
            var oUserAD = (Usuarios)HttpContext.Current.Session["UserAD"];

            try
            {
                base.OnActionExecuting(filterContext);

                if (oUsuario == null)
                {
                    if (filterContext.Controller is HomeController == true)
                    {
                        //filterContext.HttpContext.Response.Redirect("~/");
                    }
                    else
                    {
                        if (filterContext.Controller is AccountController == true)
                        {
                            //filterContext.HttpContext.Response.Redirect("~/Home/Index");
                            //filterContext.HttpContext.Response.Redirect("~/Account/Login");
                            //filterContext.HttpContext.Response.Redirect("~");
                        }
                    }
                    //if (filterContext.Controller is AccountController == false)
                    //{
                    //    filterContext.HttpContext.Response.Redirect("~/Account/Login");
                    //}
                }
                else
                {
                    if ((oUserA != null))
                    {
                        //if (filterContext.Controller is AccountController == false && filterContext.Controller is AdministradorController == false && filterContext.Controller is AlumnosController == true)
                        //{
                        //    filterContext.HttpContext.Response.Redirect("~/Alumnos/HomeA");
                        //}
                    }
                    //if ((UserM != null))
                    //{
                    //    if (filterContext.Controller is AccountController == true)
                    //    {
                    //        //filterContext.HttpContext.Response.Redirect("~/maestros/HomeM");
                    //    }
                    //}
                    //if ((UserT != null))
                    //{
                    //    if (filterContext.Controller is AccountController == true)
                    //    {
                    //        //filterContext.HttpContext.Response.Redirect("~/tutores/HomeT");
                    //    }
                    //}
                    //if ((oUserD != null))
                    //{
                    //    if (filterContext.Controller is AccountController == true)
                    //    {
                    //        //filterContext.HttpContext.Response.Redirect("~/Directores/HomeD");
                    //    }
                    //}
                    if ((oUserAD != null))
                    {
                        //if (filterContext.Controller is AccountController == false && filterContext.Controller is AlumnosController == true)
                        //{
                        //    filterContext.HttpContext.Response.Redirect("~/Administrador/HomeAD");
                        //}
                    }
                }
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~");
            }

            //base.OnActionExecuting(filterContext);
        }
    }
}
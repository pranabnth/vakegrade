using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Configuration;
using HTL.Grieskirchen.VaKEGrade.Utility;

namespace VaKEGrade.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login() {
            return Redirect("/Admin/");
        }

        public static string PrimaryDomain
        {
            get { return "localhost:4895"; }
        }



        static void CheckToSecurePage(System.Web.HttpContext Context)
        {
            bool SecurityEnabled = false; NameValueCollection nvc = (NameValueCollection)ConfigurationManager.GetSection("communityStarterKit/services"); try //use the try catch in case the key does not exist. 
            { SecurityEnabled = bool.Parse(nvc["forceUseOfSecureHTTP"]); }
            catch { SecurityEnabled = false; }
            if (SecurityEnabled)
            {
                string pagename = Context.Request.RawUrl.ToLower();
                bool needSecure = false;
                if (pagename.IndexOf("users_editprofile.aspx") > 0)
                {
                    needSecure = true;
                } if (pagename.IndexOf("users_login.aspx") > 0)
                {
                    needSecure = true;
                } if (pagename.IndexOf("users_register.aspx") > 0)
                {
                    needSecure = true;
                } if (needSecure && !Context.Request.IsSecureConnection)
                {
                    {
                        Context.Response.Redirect("https://" + PrimaryDomain + Context.Request.RawUrl);
                    }
                }
                else if (!needSecure && Context.Request.IsSecureConnection)
                {
                    {
                        Context.Response.Redirect("http://" + PrimaryDomain + Context.Request.RawUrl);
                    }
                }
            }
            else if (Context.Request.IsSecureConnection)
            {
                Context.Response.Redirect("http://" + PrimaryDomain + Context.Request.RawUrl);
            }
        }
    }
}

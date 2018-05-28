using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Django.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["userName"]==null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.userName = Session["userName"].ToString();
            return View();
        }

     
  
        public ActionResult Login()
        {
          string userName=  Request["userName"];
            Session["userName"] = userName;
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
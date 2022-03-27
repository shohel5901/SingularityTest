using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DFRL.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LogIn()
        {
            return View();
        }
        public ActionResult AccessPermission()
        {
            return View();
        }

        public ActionResult UserCreate()
        {
            return View();
        }

        public ActionResult EmployeeList()
        {
            return View();
        }

    }
}
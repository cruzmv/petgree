using Npgsql.Logging;
using sb_admin_2.Web.Database;
using sb_admin_2.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;

// Template
//https://github.com/lvasquez/sb-admin-2-bootstrap-template-asp-mvc
// Host
//https://www.hostgator.com/windows-hosting?gclsrc=aw.ds&&utm_source=google&utm_medium=genericsearch&kclickid=76d0a795-1cf1-4449-8593-462f8990f2cc&kenshoo_ida=Host%20Gator%20IDA&adid=436694164445&utm_term=&matchtype=b&addisttype=g&campaign=6455207938&adgroup=80111091787

namespace sb_admin_2.Web.Controllers
{
    public class HomeController : Controller
    {
        //public Util.Config config = new Util.Config();
        public ActionResult Index()
        {
            //List<Cats> gatos = listCats();
            //ViewBag.lastCats = gatos;
            
            List<Cats> cats = new List<Cats>();
            pgsql conn = new pgsql();
            conn.host = "localhost";
            conn.userName = "postgres";
            conn.password = "142536";
            conn.database = "petgree";
            if (conn.open())
                cats = conn.sqlListCats("limit 5");

            ViewBag.lastCats = cats;
            //config.SetConfig();

            List<Person> contacts = new List<Person>();
            conn.host = "localhost";
            conn.userName = "postgres";
            conn.password = "142536";
            conn.database = "petgree";
            if (conn.open())
                contacts = conn.sqlListPerson("limit 5");


            ViewBag.lastPeoples = contacts;

            /*
            var resxFile = @"C:\Onys\Petgree\lab\template\sbadmin\sb-admin-2-bootstrap-template-asp-mvc\sb-admin-2.Web\Resources\petgree.resx";
            using (ResXResourceReader resxReader = new ResXResourceReader(resxFile))
            {
                foreach (DictionaryEntry entry in resxReader)
                {
                    Console.WriteLine( (string)entry.Key );
                    //if (((string)entry.Key).StartsWith("EarlyAuto"))
                    //    autos.Add((Automobile)entry.Value);
                    //else if (((string)entry.Key).StartsWith("Header"))
                    //    headers.Add((string)entry.Key, (string)entry.Value);
                }
            }
            */



            return View();
        }

        public ActionResult Admexample()
        {
            return View();
        }

        public ActionResult FlotCharts()
        {
            return View("FlotCharts");
        }

        public ActionResult MorrisCharts()
        {
            return View("MorrisCharts");
        }

        public ActionResult Tables()
        {
            return View("Tables");
        }

        public ActionResult Forms()
        {
            return View("Forms");
        }

        public ActionResult Panels()
        {
            return View("Panels");
        }

        public ActionResult Buttons()
        {
            return View("Buttons");
        }

        public ActionResult Notifications()
        {
            return View("Notifications");
        }

        public ActionResult Typography()
        {
            return View("Typography");
        }

        public ActionResult Icons()
        {
            return View("Icons");
        }

        public ActionResult Grid()
        {
            return View("Grid");
        }

        public ActionResult Blank()
        {
            return View("Blank");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

    }
}
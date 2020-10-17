using sb_admin_2.Web.Database;
using sb_admin_2.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sb_admin_2.Web.Controllers
{
    public class PedigreeController : Controller
    {
        // GET: Pedigree
        public ActionResult Index(int id)
        {

            Pedigree pedigree = new Pedigree();
            pgsql conn = new pgsql();
            if (conn.open())
                pedigree = conn.sqlPedigree(id);
            ViewBag.pedigree = pedigree;

            //return Json(pedigree);

            return View();
        }
    }
}
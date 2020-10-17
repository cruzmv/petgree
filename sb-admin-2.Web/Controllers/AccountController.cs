using Newtonsoft.Json;
using sb_admin_2.Web.Database;
using sb_admin_2.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace sb_admin_2.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult People()
        {
            List<Person> people = new List<Person>();
            pgsql conn = new pgsql();
            //if (conn.open())
            //    people = conn.sqlListPerson(null);

            //ViewBag.people = people;

            return View();
        }

        public ActionResult Cats()
        {

            //string gatos = listCats();
            //var deserializer = new JavaScriptSerializer();
            //deserializer.MaxJsonLength = Int32.MaxValue;
            //List<Cats> gatosObj = deserializer.Deserialize<List<Cats>>(gatos);
            //ViewBag.cats = gatosObj;

            //JsonResult gatos = listCats();
            //var gatosObj = gatos.Data as List<Cats>;
            //ViewBag.cats = gatosObj;
            return View();
        }

        public string listCats()
        {
            List<Cats> cats = new List<Cats>();
            pgsql conn = new pgsql();
            if (conn.open())
                cats = conn.sqlListCats("");

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            return serializer.Serialize(cats); ;
        }

        public JsonResult getCat(int id)
        {


            Cats cat = findACatById(id);


            return Json( cat );
        }

        public JsonResult listRace()
        {
            List<cat_general_sigla> races = new List<cat_general_sigla>();
            pgsql conn = new pgsql();
            if (conn.open())
                races = conn.sqlListRace();

            return Json(races);
        }

        public JsonResult listProperties(string key)
        {

            List<cat_general_sigla> props = new List<cat_general_sigla>();
            pgsql conn = new pgsql();
            if (conn.open())
                props = conn.sqlListProperties(key);

            return Json(props);

        }



        public JsonResult delProperties(string key, int id)
        {
            bool ret = false;
            pgsql conn = new pgsql();
            if (conn.open())
                ret = conn.sqlDeleteProperties(key,id);

            return Json(ret);
        }


        public JsonResult savePerson(Person person)
        {
            bool ret = false;
            pgsql conn = new pgsql();
            if (conn.open())
                ret = conn.savePerson(person);

            return Json(ret);
        }


        public JsonResult saveProperties(string key, int id, string description, string sigla, bool edit)
        {
            bool ret = false;
            pgsql conn = new pgsql();
            if (conn.open())
                ret = conn.sqlSaveProperties(key, id, description, sigla, edit);

            return Json(ret);
        }


        public JsonResult listContacts(string key)
        {
            List<Person> contacts = new List<Person>();
            pgsql conn = new pgsql();
            if (conn.open())
                contacts = conn.sqlListPerson(key);

            return Json(contacts);
        }


        public JsonResult peopleProfile(int id)
        {
            Person contact = new Person();
            pgsql conn = new pgsql();
            if (conn.open())
                contact = conn.sqlPersonProfile(id);

            return Json(contact);
        }


        public JsonResult saveCat(Cats cat)
        {
            bool catSaved = false;
            pgsql conn = new pgsql();
            if (conn.open())
                catSaved = conn.sqlSaveCat(cat);

            return Json(catSaved);
        }

        public JsonResult pedigree(int id)
        {

            Pedigree pedigree = new Pedigree();
            pgsql conn = new pgsql();
            if (conn.open())
                pedigree = conn.sqlPedigree(id);

            return Json(pedigree);
        }


        /*
        public ActionResult CatCard(int id)
        {

            ViewBag.Catcard_id = id;
            return View();

        }
        */

        private Cats findACatById(int id)
        {
            Cats cat = null;
            pgsql conn = new pgsql();
            if(conn.open())
                cat = conn.sqlCat(id);


            return cat;

        }


    }
}
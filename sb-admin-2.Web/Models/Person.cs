using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace sb_admin_2.Web.Models
{
    public class Person
    {
        public int id { get; set; }
        public string name{ get; set; }
        public string type { get; set; }
        public string sex{ get; set; }
        public string nick { get; set; }
        public string caterry { get; set; }
        public string cnpj { get; set; }
        public string ie { get; set; }
        public DateTime birthday { get; set; }
        public DateTime registered { get; set; }
        public string caterry_register { get; set; }
        public string street { get; set; }
        public int door_number { get; set; }
        public string district { get; set; }
        public string complement { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string post_code { get; set; }
        public string country { get; set; }
        public List<contact> contact { get; set; }
        public List<attached> attached { get; set; }
        public string obs { get; set; }

    }

    public class attached
    {
        public int id { get; set; }
        public FileStream file { get; set; }
    }


    public class contact
    {
        public int id { get; set; }
        public string type { get; set; }
        public string contacts_contact { get; set; }
        public string obs { get; set; }

    }



}
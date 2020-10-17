using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace sb_admin_2.Web.Util
{
    public class Config
    {
        // General
        public bool Debug { get; set; }

        // Database
        public string host { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string database { get; set; }

        public Config()
        {
            // General
            Debug = bool.Parse(ConfigurationManager.AppSettings["General.Debug"]);

            //Database
            host = ConfigurationManager.AppSettings["Database.host"];
            user = ConfigurationManager.AppSettings["Database.User"];
            password = ConfigurationManager.AppSettings["Database.Password"];
            database = ConfigurationManager.AppSettings["Database.Database"];

        }

    }
}
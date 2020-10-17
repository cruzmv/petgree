using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace sb_admin_2.Web.Util
{
    public static class Logger
    {
        #region Attributs

        public static StreamWriter file { get; set; }
        public static readonly DateTime _dateLogger = DateTime.Now;

        #endregion

        #region Méthodes

        /// <summary>
        /// Création du logger.
        /// </summary>
        public static void CreateLogger(TypeLog typeLog)
        {
            /*
            string date = _dateLogger.ToString("yyyyMMdd");
            string nameLog = String.Format("{0}_{1}", date, "LOG");
            //var reader = new AppSettingsReader();


            //string pathLog = reader.GetValue("General.PathLog", typeof(string)).ToString();
            string pathLog = "log";
            //var x = HttpContext.Current.Server.MapPath($"~/{pathLog}/");
            if (!Directory.Exists(pathLog))
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath($"~/{pathLog}/"));
            //Directory.CreateDirectory(pathLog);

            if (typeLog == TypeLog.ERROR)
            {
                file = new StreamWriter(String.Format("{0}{1}.Error", pathLog, nameLog), true);
            }
            else if (typeLog == TypeLog.WARNING)
            {
                file = new StreamWriter(String.Format("{0}{1}.Warn", pathLog, nameLog), true);
            }
            else
            {
                file = new StreamWriter(String.Format("{0}{1}.Info", pathLog, nameLog), true);
            }
            */
        }

        /// <summary>
        /// Permet d'ecrire une ligne dans le fichier de log.
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLine(string message, TypeLog typeLog, Boolean bConsole = true)
        {
            if (bConsole)
            {
                if (typeLog == TypeLog.ERROR)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(message);
                    Console.ResetColor();
                }
                else if (typeLog == TypeLog.WARNING)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(message);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(message);
                }

            }

            //var reader = new AppSettingsReader();

            //Boolean bGeneral_WriteLog = Convert.ToBoolean(reader.GetValue("General.WriteLog", typeof(Boolean)));

            if (true)
            {
                //OUVERTURE DU LOGGER
                CreateLogger(typeLog);
                //file.WriteLine(String.Format("[{0}] [{1}] || {2}", DateTime.Now, typeLog, message));
                //FERMETURE DU LOGGER
                shutdown();
            }
        }

        /// <summary>
        /// Fermture du fichier
        /// </summary>
        public static void shutdown()
        {
            //file.Close();
        }
        #endregion
    }


}
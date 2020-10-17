using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace sb_admin_2.Web.Models
{
    public class Cats
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime birth_date { get; set; }
        public DateTime arrival_date { get; set; }
        public float? value_payd { get; set; }
        public cat_general_sigla race { get; set; }
        public parent father { get; set; }
        public parent mother { get; set; }
        public cat_general color { get; set; }
        public cat_general marks { get; set; }
        public cat_general partcolor { get; set; }
        public cat_general pather { get; set; }
        public string sex { get; set; }
        public cat_general_sigla eye { get; set; }
        public string country_origin { get; set; }
        public string microship { get; set; }
        public string breeder { get; set; }
        public string homologation { get; set; }
        public Person cattery { get; set; }
        public Person creator { get; set; }
        public Person proprietary { get; set; }
        public documents document {get; set;}
        public string obs { get; set; }
        public Util.Action action { get; set; }

    }

    public class Pedigree
    {



        /*
                                                                                             Cat
        
                                                        Pai                                   |                                                    Mae

                               avô                      |                    avó                                             avô                    |                    avó

                 bisavô        |      bisavó                  bisavô          |       bisavó                   bisavô          |       bisavó             bisavô          |         bisavó 

          tataravô | tataravó   tataravô | tataravó      tataravô | tataravó    tataravô | tataravó     tataravô | tataravó     tataravô | tataravó     tataravô | tataravó     tataravô | tataravó
                          
                                   Tataravô
                        |---Bisavô Tataravó
                   |----Avô
                   |    |---Bisavó Tataravô
                   |               Tataravó
            |------Pai        
            |      |
            |      |    |---Bisavô Tataravô
            |      |    |          Tataravó
            |      |----Avó
            |           |---Bisavó Tataravô
            |                      Tataravó
            Cat
            |
            |           |---Bisavô Tataravô
            |           |          Tataravó
            |       |---Avô
            |       |   |---Bisavó Tataravô
            |       |              Tataravó
            |-------Mae
                    |   |---Bisavô Tataravô
                    |   |          Tataravó
                    |---Avó 
                        |---Bisavó Tataravô
                                   Tataravó
         
        */

        // Cat
        public Cats cat{ get; set; }

            // Pai 
            public Cats pai { get; set; }
                // Avôs paternos
                public Cats pai_avo { get; set; }
                    public Cats pai_avo_bisavo { get; set; }
                        public Cats pai_avo_bisavo_tataravo { get; set; }
                        public Cats pai_avo_bisavo_tataravoo { get; set; }

                    public Cats pai_avo_bisavoo { get; set; }
                        public Cats pai_avo_bisavoo_tataravo { get; set; }
                        public Cats pai_avo_bisavoo_tataravoo { get; set; }        

                // Avós paterno
                public Cats pai_avoo { get; set; }
                    public Cats pai_avoo_bisavo { get; set; }
                        public Cats pai_avoo_bisavo_tataravo { get; set; }
                        public Cats pai_avoo_bisavo_tataravoo { get; set; }

                    public Cats pai_avoo_bisavoo { get; set; }
                        public Cats pai_avoo_bisavoo_tataravo { get; set; }
                        public Cats pai_avoo_bisavoo_tataravoo { get; set; }



            // Mae
            public Cats mae{ get; set; }

                
                // Avôs Maternos
                public Cats mae_avo { get; set; }
                    public Cats mae_avo_bisavo { get; set; }
                        public Cats mae_avo_bisavo_tataravo { get; set; }
                        public Cats mae_avo_bisavo_tataravoo { get; set; }

                    public Cats mae_avo_bisavoo { get; set; }
                        public Cats mae_avo_bisavoo_tataravo { get; set; }
                        public Cats mae_avo_bisavoo_tataravoo { get; set; }

                // Avós Maternos
                public Cats mae_avoo { get; set; }
                    public Cats mae_avoo_bisavo { get; set; }
                        public Cats mae_avoo_bisavo_tataravo { get; set; }
                        public Cats mae_avoo_bisavo_tataravoo { get; set; }

                    public Cats mae_avoo_bisavoo { get; set; }
                        public Cats mae_avoo_bisavoo_tataravo { get; set; }
                        public Cats mae_avoo_bisavoo_tataravoo { get; set; }


    }






    public class documents
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string description { get; set; }
        public string file { get; set; }
    }

    public class cat_general_sigla
    {
        public int id { get; set; }
        public string description { get; set; }
        public string sigla { get; set; }
    }

    public class cat_general
    {
        public int id { get; set; }
        public string description { get; set; }
    }

    public class parent
    {
        public int id { get; set; }
        public string name { get; set; }
        public cat_general_sigla race { get; set; }
        public Cats father { get; set; }
        public Cats mother { get; set; }
    }

    public class cat2
    {
        public int id { get; set; }
        public string name { get; set; }

    }


}
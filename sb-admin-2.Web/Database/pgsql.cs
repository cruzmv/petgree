using sb_admin_2.Web.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using sb_admin_2.Web.Util;
using sb_admin_2.Web.Models;
using Microsoft.Ajax.Utilities;
using System.Threading;

namespace sb_admin_2.Web.Database
{
    public class pgsql
    {
        #region properties
        public string host { get; set; }
        public string userName { get; set; }
        public string password{ get; set; }
        public string database { get; set; }
        #endregion
        

        public pgsql()
        {
            Config conf = new Config();
            host = conf.host;
            userName = conf.user;
            password = conf.password;
            database = conf.database;
        }

        private NpgsqlConnection conn { get; set; }



        public bool open()
        {
            bool ret = false;
            string connstring = $"Host={host};Username={userName};Password={password};Database={database}";
            try
                {
                conn = new NpgsqlConnection(connstring);
                conn.Open();
                ret = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return ret;
        }

        public NpgsqlDataReader sqlcmd(string sql)
        {
            NpgsqlDataReader ret = null;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql,conn);
                ret = cmd.ExecuteReader();
            } catch (Exception ex)
                {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return ret;
        }

        public List<Cats> sqlListCats(string limit)
        {
            List<Cats> cats = new List<Cats>();
            string sqlString = "select a.codgat,a.homo,a.chegat,a.nomgat,a.nasgat,b.desras,c.descor,d.despad,e.desptc,a.sexgat,a.rasgat " +
                               "  from cad_gat a " +
                               "  left join cad_ras b on b.codras=a.rasgat " +
                               "  left join cad_cor c on c.codcor=a.corgat " +
                               "  left join cad_pad d on d.codpad=a.padgat " +
                               "  left join cad_ptc e on e.codptc=a.ptcgat " +
                               " order by a.nomgat " + limit;
            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    Cats cat = new Cats();
                    cat.id = dr.GetInt32(0);

                    try { cat.homologation = dr.GetString(1); } catch { }
                    try { cat.arrival_date = dr.GetDateTime(2); } catch { }
                    try { cat.name = dr.GetString(3).Replace("'","`"); } catch { }
                    try { cat.birth_date = dr.GetDateTime(4); } catch { }
                    try 
                    { 
                        if(dr.GetString(5) != null)
                        {
                            cat_general_sigla race = new cat_general_sigla();
                            race.description = dr.GetString(5).Replace("'","`");
                            race.id = dr.GetInt32(10);
                            cat.race = race;
                        }
                    } catch { }
                    try 
                    {
                        if (dr.GetString(6) != null) 
                        { 
                            cat_general color = new cat_general();
                            color.description = dr.GetString(6).Replace("'","`");
                            cat.color = color;
                        }
                    } catch { }
                    try
                    {
                        if (dr.GetString(7) != null)
                        {
                            cat_general pather = new cat_general();
                            pather.description = dr.GetString(7);
                            cat.pather = pather;
                        }
                    }
                    catch { }
                    try
                    {
                        if (dr.GetString(8) != null)
                        {
                            cat_general partcolor = new cat_general();
                            partcolor.description = dr.GetString(8);
                            cat.partcolor = partcolor;
                        }
                    }
                    catch { }

                    try{ cat.sex = dr.GetString(9); } catch { }

                    

                    /*
                    try
                    {
                        if (dr.GetInt32(10) > 0)
                        {
                            parent father = new parent()
                            {
                                id = dr.GetInt32(10),
                                name = dr.GetString(11),
                                race = new cat_general_sigla()
                                {
                                    id = dr.GetInt32(12),
                                    description = dr.GetString(13)
                                }

                            };

                            cat.father = father;
                        }
                    }
                    catch { }

                    try
                    {
                        if (dr.GetInt32(14) > 0 )
                        {
                            parent mother = new parent()
                            {
                                id = dr.GetInt32(14),
                                name = dr.GetString(15),
                                race = new cat_general_sigla()
                                {
                                    id = dr.GetInt32(16),
                                    description = dr.GetString(17)
                                }
                            };

                            cat.mother = mother;
                        }
                    }
                    catch { }
                    */

                    cats.Add(cat);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return cats;
        }

        public List<cat_general_sigla> sqlListProperties(string key)
        {
            List<cat_general_sigla> props = new List<cat_general_sigla>();
            string sqlString = "";

            switch (key)
            {
                case "Color":
                    sqlString = "select codcor,descor,'' from cad_cor order by descor";
                    break;
                case "Mark":
                    sqlString = "select codmar,desmar,'' from cad_mar order by desmar";
                    break;
                case "Particolor":
                    sqlString = "select codptc,desptc,'' from cad_ptc order by desptc";
                    break;
                case "Pather":
                    sqlString = "select codpad,despad,'' from cad_pad order by despad";
                    break;
                case "Eyes":
                    sqlString = "select olho_codigo,olho_descricao, sigla from cat_olho order by olho_descricao";
                    break;
                default:
                    sqlString = "";
                    break;
            }

            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    cat_general_sigla prop = new cat_general_sigla();
                    prop.id = dr.GetInt32(0);
                    prop.description = dr.GetString(1);
                    prop.sigla = dr.GetString(2);

                    props.Add(prop);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return props;

        }

        public Cats sqlCat(int id)
        {
            Cats cat = new Cats();
            int race = 0, color = 0, mark = 0, father = 0, mother = 0, eye = 0, partcolor = 0, patter = 0, cattery = 0, creator = 0, proprietary = 0;
            string sqlString = "select codgat,nomgat,nasgat,chegat,valgat,rasgat,cpggat," +
                                       "corgat,margat,ptcgat,sexgat,olho_gato,pais_origem," +
                                       "microchip,breeder,homo,cmggat,cod_gatil,cod_criador,cntgat,obs,padgat,paigat,rpggat,maegat,rmggat " +
                                $" from cad_gat where codgat = {id}";

            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    cat.id = dr.GetInt32(0);
                    cat.name = dr.GetString(1);
                    cat.birth_date = dr.GetDateTime(2);
                    cat.arrival_date = dr.GetDateTime(3);
                    cat.value_payd = dr.GetFloat(4);
                    race = dr.GetInt32(5);
                    father = dr.GetInt32(6);
                    color = dr.GetInt32(7);
                    mark = dr.GetInt32(8);
                    partcolor = dr.GetInt32(9);
                    cat.sex = dr.GetString(10);
                    eye = dr.GetInt32(11);
                    cat.country_origin = dr.GetString(12);
                    cat.microship = dr.GetString(13);
                    cat.breeder = dr.GetString(14);
                    cat.homologation = dr.GetString(15);
                    mother = dr.GetInt32(16);
                    cattery = dr.GetInt32(17);
                    creator = dr.GetInt32(18);
                    proprietary = dr.GetInt32(19);
                    try { cat.obs = dr.GetString(20); } catch { }
                    patter = dr.GetInt32(21);
                }
                dr.Close();

                if (father > 0)
                    cat.father = sqlParent(father);

                if (mother > 0)
                    cat.mother = sqlParent(mother);

                if (race > 0)
                    cat.race = sqlRace(race);

                if (color > 0)
                    cat.color = sqlCatColor(color);

                if (mark > 0)
                    cat.marks = sqlCatMarks(mark);

                if (eye > 0)
                    cat.eye = sqlCatEye(eye);

                if (partcolor > 0)
                    cat.partcolor = sqlCatPartcolor(partcolor);

                if (patter > 0)
                    cat.pather = sqlPatter(patter);

                if (cattery > 0)
                    cat.cattery = sqlPerson(cattery);

                if (creator > 0)
                    cat.creator = sqlPerson(creator);

                if (proprietary > 0)
                    cat.proprietary = sqlPerson(proprietary);

            }
            catch (Exception ex) 
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }

            return cat;
        }

        public List<cat_general_sigla> sqlListRace()
        {
            List<cat_general_sigla> listRet = new List<cat_general_sigla>();
            string sqlString = "select codras,desras,sigla from cad_ras order by desras";
            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    cat_general_sigla race = new cat_general_sigla();
                    race.id = dr.GetInt32(0);
                    if(dr.GetString(1) != null)
                        race.description = dr.GetString(1);

                    try
                    {
                        race.sigla = dr.GetString(2);
                    }
                    catch { }
                    
                        

                    listRet.Add(race);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return listRet;
        }

        public cat_general_sigla sqlRace(int id)
        {
            cat_general_sigla race = new cat_general_sigla();
            string sqlString = $"select codras,desras,sigla from cad_ras where codras = {id}";
            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    race.id = dr.GetInt32(0);
                    race.description = dr.GetString(1);
                    race.sigla = dr.GetString(2);
                }
                dr.Close();

            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return race;
        }

        public cat_general_sigla sqlCatEye(int id)
        {
            cat_general_sigla eye = new cat_general_sigla();
            string sqlString = $"select olho_codigo,olho_descricao,sigla from cat_olho where olho_codigo = {id}";
            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    eye.id = dr.GetInt32(0);
                    eye.description = dr.GetString(1);
                    eye.sigla = dr.GetString(2);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return eye;
        }

        public cat_general sqlCatColor(int id)
        {
            cat_general catColor = new cat_general();
            string sqlString = $"select codcor,descor from cad_cor where codcor = {id}";
            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    catColor.id = dr.GetInt32(0);
                    catColor.description = dr.GetString(1);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return catColor;
        }

        public cat_general sqlCatPartcolor(int id)
        {
            cat_general partcolor = new cat_general();
            string sqlString = $"select codptc,desptc from cad_ptc where codptc = {id}";
            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    partcolor.id = dr.GetInt32(0);
                    partcolor.description = dr.GetString(1);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return partcolor;
        }



        public cat_general sqlPatter(int id)
        {
            cat_general patter = new cat_general();
            string sqlString = $"select codpad,despad from cad_pad where codpad = {id}";
            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    patter.id = dr.GetInt32(0);
                    patter.description = dr.GetString(1);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return patter;

        }

        public Person sqlPerson(int id)
        {
            Person person = new Person();
            string sqlString = "select a.contato_codigo, " +
                               "       a.contato_nome, " +
                               "       a.contato_apelido, " +
                               "       a.contato_cnp, " +
                               "       a.contato_inscricao, " +
                               "	   a.contato_registro_gatil, " +
                               "	   a.contato_cidade, " +
                               "	   a.contato_uf  " +
                               "from cat_cad_contato a " +
                               $"where a.contato_codigo = {id} ";
            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    person.id = dr.GetInt32(0);
                    person.name = dr.GetString(1);
                    person.nick = dr.GetString(2);
                    person.cnpj = dr.GetString(3);
                    person.ie = dr.GetString(4);
                    person.caterry_register = dr.GetString(5);
                    person.city = dr.GetString(6);
                    person.district = dr.GetString(7);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return person;

        }

        




        public cat_general sqlCatMarks(int id)
        {
            cat_general catMark = new cat_general();
            string sqlString = $"select codmar,desmar from cad_mar where codmar = {id}";
            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    catMark.id = dr.GetInt32(0);
                    catMark.description = dr.GetString(1);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return catMark;
        }

        public parent sqlParent(int id)
        {
            parent parent = new parent();
            string sqlString = "select a.codgat, " +
                               "       a.nomgat, " +
                               "       b.codras, " +
                               "       b.desras, " +
                               "       b.sigla  " +
                               "  from cad_gat a " +
                               "  left join cad_ras b on b.codras = a.rasgat " +
                               $" where codgat = {id} ";

            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    parent.id = dr.GetInt32(0);
                    parent.name = dr.GetString(1);
                    parent.race = new cat_general_sigla()
                    {
                        id = dr.GetInt32(2),
                        description = dr.GetString(3),
                        sigla = dr.GetString(4)
                    };
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return parent;
        }

        public bool sqlDeleteProperties(string key, int id)
        {
            bool ret = false;
            string sqlString = "";
            switch (key)
            {
                case "Color":
                    sqlString = "delete from cad_cor where codcor=" + id.ToString();
                    break;
                case "Mark":
                    sqlString = "delete from cad_mar where codmar=" + id.ToString();
                    break;
                case "Particolor":
                    sqlString = "delete from cad_ptc where codptc=" + id.ToString();
                    break;
                case "Pather":
                    sqlString = "delete from cad_pad where codpad=" + id.ToString();
                    break;
                case "Eyes":
                    sqlString = "delete from cat_olho where olho_codigo=" + id.ToString();
                    break;
                default:
                    sqlString = "";
                    break;
            }

            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                dr.Close();
                ret = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }

            return ret;
        }

        public bool sqlSaveProperties(string key, int id, string description, string sigla, bool edit)
        {
            bool ret = false;
            string sqlString = "";
            switch (key)
            {
                case "Color":
                    if (edit)
                        sqlString = "update cad_cor set descor = '" + description + "' where codcor=" + id.ToString();
                    else
                        sqlString = "insert into cad_cor(descor) values('" + description + "')";
                    break;
                case "Mark":
                    if (edit)
                        sqlString = "update cad_mar set desmar = '" + description + "' where codmar=" + id.ToString();
                    else
                        sqlString = "insert into cad_mar(desmar) values('" + description + "')";
                    break;
                case "Particolor":
                    if (edit)
                        sqlString = "update cad_ptc set desptc = '" + description + "' where codptc=" + id.ToString();
                    else
                        sqlString = "insert into cad_ptc(desptc) values('" + description + "')";
                    break;
                case "Pather":
                    if (edit)
                        sqlString = "update cad_pad set despad= '" + description + "' where codpad=" + id.ToString();
                    else
                        sqlString = "insert into cad_pad(despad) values('" + description + "')";
                    break;
                case "Eyes":
                    sqlString = "delete from cat_olho where olho_codigo=" + id.ToString();
                    if (edit)
                        sqlString = "update cat_olho set olho_descricao= '" + description + "' where olho_codigo=" + id.ToString();
                    else
                        sqlString = "insert into cat_olho(olho_descricao,sigla) values('" + description + "','"+sigla+"')";
                    break;
                default:
                    sqlString = "";
                    break;
            }

            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                //#TODO: testar rows afected aqui para valida....
                dr.Close();
                ret = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }

            return ret;
        }

        public List<Person> sqlListPerson(string limit)
        {
            List<Person> listPerson = new List<Person>();
            string sqlString = "select a.contato_codigo, " +
                               "       a.contato_nome, " +
                               "       a.contato_apelido, " +
                               "       a.contato_cnp, " +
                               "       a.contato_inscricao, " +
                               "	   a.contato_registro_gatil, " +
                               "	   a.contato_cidade, " +
                               "	   a.contato_uf  " +
                               "from cat_cad_contato a " +
                               "order by a.contato_nome  " + limit;
            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                while (dr.Read())
                {
                    Person person = new Person();
                    person.id = dr.GetInt32(0);
                    person.name = dr.GetString(1);
                    person.nick = dr.GetString(2);
                    person.cnpj = dr.GetString(3);
                    person.ie = dr.GetString(4);
                    person.caterry_register = dr.GetString(5);
                    person.city = dr.GetString(6);
                    person.region = dr.GetString(7);

                    listPerson.Add(person);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }
            return listPerson;

        }


        public Person sqlPersonProfile(int id)
        {
            Person p = new Person();
            string sqlString = "select contato_codigo, " +
                               "       contato_nome, " +
                               "       contato_tipo_pessoa, " +
                               "       contato_sexo, " +
                               "       contato_apelido, " +
                               "       contato_gatil, " +
                               "       contato_registro_gatil, " +
                               "       contato_cnp, " +
                               "       contato_inscricao, " +
                               "       contato_aniversario, " +
                               "       contato_cadastro, " +
                               "       contato_rua, " +
                               "       contato_numero, " +
                               "       contato_bairro, " +
                               "       contato_cidade, " +
                               "       contato_uf, " +
                               "       contato_cep, " +
                               "       contato_pais, " +
                               "       contato_observa " +
                               "  from cat_cad_contato " +
                              $" where contato_codigo = {id}";
            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        p.id = dr.GetInt32(0);
                        p.name = dr.GetString(1);
                        p.type = dr.GetString(2);
                        p.sex = dr.GetString(3);
                        try { p.nick = dr.GetString(4); } catch { }
                        try { p.caterry = dr.GetString(5); } catch { }
                        try { p.caterry_register = dr.GetString(6); } catch { }
                        try { p.cnpj = dr.GetString(7); } catch { }
                        try { p.ie= dr.GetString(8); } catch { }

                        p.birthday = dr.GetDateTime(9);
                        p.registered = dr.GetDateTime(10);
                        
                        try { p.street = dr.GetString(11); } catch { }
                        try { p.door_number = dr.GetInt32(12); } catch { }
                        try { p.district = dr.GetString(13); } catch { }
                        try { p.city = dr.GetString(14); } catch { }
                        try { p.region = dr.GetString(15); } catch { }
                        try { p.post_code = dr.GetString(17); } catch { }
                        try { p.country = dr.GetString(17); } catch { }
                        try { p.obs = dr.GetString(18); } catch { }

                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }


            if (p.id > 0)
            {
                List<contact> listContact = new List<contact>();
                sqlString = "select contato_codigo, contato_tipo, contato_contato, contato_observa " +
                    $"from cat_cad_contato_contato where contato_codigo = {p.id}";
                try
                {
                    NpgsqlDataReader dr = sqlcmd(sqlString);
                    if (dr != null)
                    {
                        int countID = 0;
                        while (dr.Read())
                        {
                            contact c = new contact
                            {
                                id = ++countID,
                                type = dr.GetString(1),
                                contacts_contact = dr.GetString(2),
                            };
                            try { c.obs = dr.GetString(3); } catch (Exception) { }

                            listContact.Add(c);
                        }
                    }
                    p.contact = listContact;
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
                }
            }


            return (p);
        }


        public bool savePerson(Person person)
        {
            bool ret = false;
            string sqlString = "";

            string birth = person.birthday.Year.ToString() + "-" +
                           person.birthday.Month.ToString() + "-" +
                           person.birthday.Day.ToString();

            string registered = person.registered.Year.ToString() + "-" +
                           person.registered.Month.ToString() + "-" +
                           person.registered.Day.ToString();

            if (person.id > 0)
            {
                sqlString = "update cat_cad_contato set " +
                              $"contato_nome='{person.name}'," +
                              $"contato_tipo_pessoa='{person.type}'," +
                              $"contato_sexo='{person.sex}', " +
                              $"contato_apelido='{person.nick}', " +
                              $"contato_gatil='{person.caterry}', " +
                              $"contato_registro_gatil='{person.caterry_register}', " +
                              $"contato_cnp='{person.cnpj}', " +
                              $"contato_inscricao='{person.ie}', " +
                              $"contato_aniversario='{birth}', " +
                              $"contato_cadastro='{registered}', " +
                              $"contato_rua='{person.street}', " +
                              $"contato_numero={person.door_number}, " +
                              $"contato_bairro='{person.district}', " +
                              $"contato_cidade='{person.city}', " +
                              $"contato_uf='{person.region}', " +
                              $"contato_cep='{person.post_code}', " +
                              $"contato_pais={1058}, " +  //person.country
                              $"contato_observa='{person.obs}'  " +
                      $" where contato_codigo = {person.id}";
            }
            else
            {
                sqlString = "insert into cat_cad_contato(contato_nome,contato_tipo_pessoa,contato_sexo,contato_apelido,contato_gatil,contato_registro_gatil,contato_cnp,contato_inscricao,contato_aniversario,contato_cadastro,contato_rua,contato_numero,contato_bairro,contato_cidade,contato_uf,contato_cep,contato_pais,contato_observa) " +
                              $"values ('{person.name}'," +
                                $"'{person.type}'," +
                                $"'{person.sex}', " +
                                $"'{person.nick}', " +
                                $"'{person.caterry}', " +
                                $"'{person.caterry_register}', " +
                                $"'{person.cnpj}', " +
                                $"'{person.ie}', " +
                                $"'{birth}', " +
                                $"'{registered}', " +
                                $"'{person.street}', " +
                                $"{ person.door_number}, " +
                                $"'{person.district}', " +
                                $"'{person.city}', " +
                                $"'{person.region}', " +
                                $"'{person.post_code}', " +
                                $"{ 1058}," + //person.country
                                $"'{person.obs}' )";
            }

            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                if (dr != null)
                {
                    ret = dr.RecordsAffected > 0;
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }


            if (ret)
            {
                sqlString = $"delete from cat_cad_contato_contato where contato_codigo = {person.id}; ";
                sqlString += "insert into cat_cad_contato_contato(contato_codigo,contato_tipo, contato_contato) values ";
                foreach (contact cto in person.contact)
                {
                    sqlString += $" ({person.id},'{cto.type}','{cto.contacts_contact}' ),";
                }
                sqlString = sqlString.Substring(0, sqlString.Length - 1) + ";";
                try
                {
                    NpgsqlDataReader dr = sqlcmd(sqlString);
                    if (dr != null)
                    {
                        ret = dr.RecordsAffected > 0;
                        dr.Close();
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
                }

            }



            return ret;
        }


        public bool sqlSaveCat(Cats cat)
        {
            bool ret = false;
            string sqlString = "";

            string birth = cat.birth_date.Year.ToString() + "-" +
                           cat.birth_date.Month.ToString() + "-" +
                           cat.birth_date.Day.ToString();

            string arrivel = cat.arrival_date.Year.ToString() + "-" +
                           cat.arrival_date.Month.ToString() + "-" +
                           cat.arrival_date.Day.ToString();

            if (cat.action == Util.Action.EDIT)
                sqlString = "update cad_gat " +
                              $"set nomgat = '{cat.name.Replace("'", "`")}'," +
                                  $"nasgat = '{birth}'," +
                                  $"chegat = '{arrivel}'," +
                                  $"valgat = {cat.value_payd}," +
                                  $"rasgat = {cat.race.id.ToString()}," +
                                  
                                  $"cpggat = {cat.father.id.ToString() }," +
                                  $"paigat = '{cat.father.name }'," +
                                  $"rpggat = {cat.father.race.id.ToString() }," +
                                  
                                  $"cmggat = {cat.mother.id.ToString() }," +
                                  $"maegat = '{cat.mother.name }'," +
                                  $"rmggat = {cat.mother.race.id.ToString() }," +

                                  $"corgat = {cat.color.id.ToString() }," +
                                  $"margat = {cat.marks.id.ToString() }," +
                                  $"padgat = {cat.pather.id.ToString() }," +
                                  $"ptcgat = {cat.partcolor.id.ToString() }," +
                                  $"sexgat = '{cat.sex.Substring(0, 1) }'," +
                                  $"olho_gato = {cat.eye.id.ToString() }," +
                                  $"pais_origem = '{(cat.country_origin == null ? "" : cat.country_origin)}'," +
                                  $"microchip = '{(cat.microship == null ? "" : cat.microship)}'," +
                                  $"breeder = '{(cat.breeder == null ? "" : cat.breeder) }'," +
                                  $"homo = '{(cat.homologation == null ? "" : cat.homologation) }'," +
                                  $"cod_gatil = {cat.cattery.id.ToString() }," +
                                  $"cod_criador = {cat.creator.id.ToString()}," +
                                  $"cntgat = {cat.proprietary.id.ToString() }," +
                                  $"obs = '{  (cat.obs == null ? "" : cat.obs.Replace("'", "`"))  }'" +
                          $" where codgat = {cat.id}";

            if (cat.action == Util.Action.INSERT)
                sqlString = "insert into cad_gat(nomgat,nasgat,chegat,valgat,rasgat,cpggat,corgat,margat,ptcgat,sexgat,olho_gato,pais_origem,microchip,breeder,homo  ,cmggat,cod_gatil,cod_criador,cntgat,obs   ,padgat,paigat,rpggat,maegat,rmggat ) " +
                           string.Format("values('{0}' ,'{1}' ,'{2}' ,{3}   ,{4}   ,{5}   ,{6}   ,{7}   ,{8}   ,'{9}' ,{10}     ,'{11}'     ,'{12}'   ,'{13}' ,'{14}',{15}  ,{16}     ,{17}       ,{18}  ,'{19}',{20}  ,'{21}',{22}  ,'{23}',{24}    ) ",
                                         cat.name.Replace("'", "`"),
                                         birth,
                                         arrivel,
                                         cat.value_payd,
                                         cat.race.id.ToString(),
                                         cat.father.id.ToString(),
                                         cat.color.id.ToString(),
                                         cat.marks.id.ToString(),
                                         cat.partcolor.id.ToString(),
                                         cat.sex.Substring(0,1),
                                         cat.eye.id.ToString(),
                                         (cat.country_origin == null ? "" : cat.country_origin),
                                         (cat.microship == null ? "" : cat.microship),
                                         (cat.breeder == null ? "" : cat.breeder),
                                         (cat.homologation == null ? "" : cat.homologation),
                                         cat.mother.id.ToString(),
                                         cat.cattery.id.ToString(),
                                         cat.creator.id.ToString(),
                                         cat.proprietary.id.ToString(),
                                         (cat.obs == null ? "" : cat.obs.Replace("'", "`")),
                                         cat.pather.id,
                                         cat.father.name,
                                         cat.father.race.id.ToString(),
                                         cat.mother.name,
                                         cat.mother.race.id.ToString());

            if (cat.action == Util.Action.DELETE)
                sqlString = $"delete from cad_gat where codgat = {cat.id}";


            try
            {
                NpgsqlDataReader dr = sqlcmd(sqlString);
                if(dr != null)
                {
                    ret = dr.RecordsAffected > 0;
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(String.Format("{0} ; {1}", ex.Message, ex.StackTrace), TypeLog.WARNING);
            }

            return ret;
        }


        public Pedigree sqlPedigree(int id)
        {
            Pedigree pedigree = new Pedigree();

            // Gato
            Cats cat = new Cats();
            cat.race = new cat_general_sigla();
            cat.color = new cat_general();
            cat.father = new parent();
            cat.mother = new parent();
            cat.eye = new cat_general_sigla();

            string sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {id}";
            NpgsqlDataReader dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    cat.name = dr.GetString(0);
                    cat.homologation = dr.GetString(1);
                    cat.race.description = dr.GetString(2);
                    cat.color.description = dr.GetString(3);
                    cat.father.id = dr.GetInt32(4);
                    cat.mother.id = dr.GetInt32(5);
                    try { cat.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { cat.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.cat = cat;


            // Pai
            Cats pai = new Cats();
            pai.race = new cat_general_sigla();
            pai.color = new cat_general();
            pai.father = new parent();
            pai.mother = new parent();
            pai.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {cat.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai.name = dr.GetString(0);
                    pai.homologation = dr.GetString(1);
                    pai.race.description = dr.GetString(2);
                    pai.color.description = dr.GetString(3);
                    pai.father.id = dr.GetInt32(4);
                    pai.mother.id = dr.GetInt32(5);
                    try { pai.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai = pai;

            // Avô paterno
            Cats pai_avo = new Cats();
            pai_avo.race = new cat_general_sigla();
            pai_avo.color = new cat_general();
            pai_avo.father = new parent();
            pai_avo.mother = new parent();
            pai_avo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avo.name = dr.GetString(0);
                    pai_avo.homologation = dr.GetString(1);
                    pai_avo.race.description = dr.GetString(2);
                    pai_avo.color.description = dr.GetString(3);
                    pai_avo.father.id = dr.GetInt32(4);
                    pai_avo.mother.id = dr.GetInt32(5);
                    try { pai_avo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avo = pai_avo;


            // Bisavô paterno
            Cats pai_avo_bisavo = new Cats();
            pai_avo_bisavo.race = new cat_general_sigla();
            pai_avo_bisavo.color = new cat_general();
            pai_avo_bisavo.father = new parent();
            pai_avo_bisavo.mother = new parent();
            pai_avo_bisavo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai_avo.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avo_bisavo.name = dr.GetString(0);
                    pai_avo_bisavo.homologation = dr.GetString(1);
                    pai_avo_bisavo.race.description = dr.GetString(2);
                    pai_avo_bisavo.color.description = dr.GetString(3);
                    pai_avo_bisavo.father.id = dr.GetInt32(4);
                    pai_avo_bisavo.mother.id = dr.GetInt32(5);
                    try { pai_avo_bisavo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avo_bisavo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avo_bisavo = pai_avo_bisavo;

            // Tataravô paterno
            Cats pai_avo_bisavo_tataravo = new Cats();
            pai_avo_bisavo_tataravo.race = new cat_general_sigla();
            pai_avo_bisavo_tataravo.color = new cat_general();
            pai_avo_bisavo_tataravo.father = new parent();
            pai_avo_bisavo_tataravo.mother = new parent();
            pai_avo_bisavo_tataravo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai_avo_bisavo.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avo_bisavo_tataravo.name = dr.GetString(0);
                    pai_avo_bisavo_tataravo.homologation = dr.GetString(1);
                    pai_avo_bisavo_tataravo.race.description = dr.GetString(2);
                    pai_avo_bisavo_tataravo.color.description = dr.GetString(3);
                    pai_avo_bisavo_tataravo.father.id = dr.GetInt32(4);
                    pai_avo_bisavo_tataravo.mother.id = dr.GetInt32(5);
                    try { pai_avo_bisavo_tataravo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avo_bisavo_tataravo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avo_bisavo_tataravo = pai_avo_bisavo_tataravo;

            // Tataravó paterno
            Cats pai_avo_bisavo_tataravoo = new Cats();
            pai_avo_bisavo_tataravoo.race = new cat_general_sigla();
            pai_avo_bisavo_tataravoo.color = new cat_general();
            pai_avo_bisavo_tataravoo.father = new parent();
            pai_avo_bisavo_tataravoo.mother = new parent();
            pai_avo_bisavo_tataravoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai_avo_bisavo.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avo_bisavo_tataravoo.name = dr.GetString(0);
                    pai_avo_bisavo_tataravoo.homologation = dr.GetString(1);
                    pai_avo_bisavo_tataravoo.race.description = dr.GetString(2);
                    pai_avo_bisavo_tataravoo.color.description = dr.GetString(3);
                    pai_avo_bisavo_tataravoo.father.id = dr.GetInt32(4);
                    pai_avo_bisavo_tataravoo.mother.id = dr.GetInt32(5);
                    try { pai_avo_bisavo_tataravoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avo_bisavo_tataravoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avo_bisavo_tataravoo = pai_avo_bisavo_tataravoo;










            // Bisavó paterno
            Cats pai_avo_bisavoo = new Cats();
            pai_avo_bisavoo.race = new cat_general_sigla();
            pai_avo_bisavoo.color = new cat_general();
            pai_avo_bisavoo.father = new parent();
            pai_avo_bisavoo.mother = new parent();
            pai_avo_bisavoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai_avo.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avo_bisavoo.name = dr.GetString(0);
                    pai_avo_bisavoo.homologation = dr.GetString(1);
                    pai_avo_bisavoo.race.description = dr.GetString(2);
                    pai_avo_bisavoo.color.description = dr.GetString(3);
                    pai_avo_bisavoo.father.id = dr.GetInt32(4);
                    pai_avo_bisavoo.mother.id = dr.GetInt32(5);
                    try { pai_avo_bisavoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avo_bisavoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avo_bisavoo = pai_avo_bisavoo;


            // Tataravô paterno
            Cats pai_avo_bisavoo_tataravo = new Cats();
            pai_avo_bisavoo_tataravo.race = new cat_general_sigla();
            pai_avo_bisavoo_tataravo.color = new cat_general();
            pai_avo_bisavoo_tataravo.father = new parent();
            pai_avo_bisavoo_tataravo.mother = new parent();
            pai_avo_bisavoo_tataravo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai_avo_bisavoo.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avo_bisavoo_tataravo.name = dr.GetString(0);
                    pai_avo_bisavoo_tataravo.homologation = dr.GetString(1);
                    pai_avo_bisavoo_tataravo.race.description = dr.GetString(2);
                    pai_avo_bisavoo_tataravo.color.description = dr.GetString(3);
                    pai_avo_bisavoo_tataravo.father.id = dr.GetInt32(4);
                    pai_avo_bisavoo_tataravo.mother.id = dr.GetInt32(5);
                    try { pai_avo_bisavoo_tataravo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avo_bisavoo_tataravo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avo_bisavoo_tataravo = pai_avo_bisavoo_tataravo;

            // Tataravó paterno
            Cats pai_avo_bisavoo_tataravoo = new Cats();
            pai_avo_bisavoo_tataravoo.race = new cat_general_sigla();
            pai_avo_bisavoo_tataravoo.color = new cat_general();
            pai_avo_bisavoo_tataravoo.father = new parent();
            pai_avo_bisavoo_tataravoo.mother = new parent();
            pai_avo_bisavoo_tataravoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai_avo_bisavoo.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avo_bisavoo_tataravoo.name = dr.GetString(0);
                    pai_avo_bisavoo_tataravoo.homologation = dr.GetString(1);
                    pai_avo_bisavoo_tataravoo.race.description = dr.GetString(2);
                    pai_avo_bisavoo_tataravoo.color.description = dr.GetString(3);
                    pai_avo_bisavoo_tataravoo.father.id = dr.GetInt32(4);
                    pai_avo_bisavoo_tataravoo.mother.id = dr.GetInt32(5);
                    try { pai_avo_bisavoo_tataravoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avo_bisavoo_tataravoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avo_bisavoo_tataravoo = pai_avo_bisavoo_tataravoo;








            // Avó paterno
            Cats pai_avoo = new Cats();
            pai_avoo.race = new cat_general_sigla();
            pai_avoo.color = new cat_general();
            pai_avoo.father = new parent();
            pai_avoo.mother = new parent();
            pai_avoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avoo.name = dr.GetString(0);
                    pai_avoo.homologation = dr.GetString(1);
                    pai_avoo.race.description = dr.GetString(2);
                    pai_avoo.color.description = dr.GetString(3);
                    pai_avoo.father.id = dr.GetInt32(4);
                    pai_avoo.mother.id = dr.GetInt32(5);
                    try { pai_avoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avoo = pai_avoo;


            // Bisavô paterno
            Cats pai_avoo_bisavo = new Cats();
            pai_avoo_bisavo.race = new cat_general_sigla();
            pai_avoo_bisavo.color = new cat_general();
            pai_avoo_bisavo.father = new parent();
            pai_avoo_bisavo.mother = new parent();
            pai_avoo_bisavo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai_avoo.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avoo_bisavo.name = dr.GetString(0);
                    pai_avoo_bisavo.homologation = dr.GetString(1);
                    pai_avoo_bisavo.race.description = dr.GetString(2);
                    pai_avoo_bisavo.color.description = dr.GetString(3);
                    pai_avoo_bisavo.father.id = dr.GetInt32(4);
                    pai_avoo_bisavo.mother.id = dr.GetInt32(5);
                    try { pai_avoo_bisavo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avoo_bisavo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avoo_bisavo = pai_avoo_bisavo;

            // Tataravô paterno
            Cats pai_avoo_bisavo_tataravo = new Cats();
            pai_avoo_bisavo_tataravo.race = new cat_general_sigla();
            pai_avoo_bisavo_tataravo.color = new cat_general();
            pai_avoo_bisavo_tataravo.father = new parent();
            pai_avoo_bisavo_tataravo.mother = new parent();
            pai_avoo_bisavo_tataravo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai_avoo_bisavo.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avoo_bisavo_tataravo.name = dr.GetString(0);
                    pai_avoo_bisavo_tataravo.homologation = dr.GetString(1);
                    pai_avoo_bisavo_tataravo.race.description = dr.GetString(2);
                    pai_avoo_bisavo_tataravo.color.description = dr.GetString(3);
                    pai_avoo_bisavo_tataravo.father.id = dr.GetInt32(4);
                    pai_avoo_bisavo_tataravo.mother.id = dr.GetInt32(5);
                    try { pai_avoo_bisavo_tataravo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avoo_bisavo_tataravo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avoo_bisavo_tataravo = pai_avoo_bisavo_tataravo;

            // Tataravó paterno
            Cats pai_avoo_bisavo_tataravoo = new Cats();
            pai_avoo_bisavo_tataravoo.race = new cat_general_sigla();
            pai_avoo_bisavo_tataravoo.color = new cat_general();
            pai_avoo_bisavo_tataravoo.father = new parent();
            pai_avoo_bisavo_tataravoo.mother = new parent();
            pai_avoo_bisavo_tataravoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai_avoo_bisavo.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avoo_bisavo_tataravoo.name = dr.GetString(0);
                    pai_avoo_bisavo_tataravoo.homologation = dr.GetString(1);
                    pai_avoo_bisavo_tataravoo.race.description = dr.GetString(2);
                    pai_avoo_bisavo_tataravoo.color.description = dr.GetString(3);
                    pai_avoo_bisavo_tataravoo.father.id = dr.GetInt32(4);
                    pai_avoo_bisavo_tataravoo.mother.id = dr.GetInt32(5);
                    try { pai_avoo_bisavo_tataravoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avoo_bisavo_tataravoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avoo_bisavo_tataravoo = pai_avoo_bisavo_tataravoo;



            // Bisavó paterno
            Cats pai_avoo_bisavoo = new Cats();
            pai_avoo_bisavoo.race = new cat_general_sigla();
            pai_avoo_bisavoo.color = new cat_general();
            pai_avoo_bisavoo.father = new parent();
            pai_avoo_bisavoo.mother = new parent();
            pai_avoo_bisavoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai_avoo.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avoo_bisavoo.name = dr.GetString(0);
                    pai_avoo_bisavoo.homologation = dr.GetString(1);
                    pai_avoo_bisavoo.race.description = dr.GetString(2);
                    pai_avoo_bisavoo.color.description = dr.GetString(3);
                    pai_avoo_bisavoo.father.id = dr.GetInt32(4);
                    pai_avoo_bisavoo.mother.id = dr.GetInt32(5);
                    try { pai_avoo_bisavoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avoo_bisavoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avoo_bisavoo = pai_avoo_bisavoo;


            // Tataravô paterno
            Cats pai_avoo_bisavoo_tataravo = new Cats();
            pai_avoo_bisavoo_tataravo.race = new cat_general_sigla();
            pai_avoo_bisavoo_tataravo.color = new cat_general();
            pai_avoo_bisavoo_tataravo.father = new parent();
            pai_avoo_bisavoo_tataravo.mother = new parent();
            pai_avoo_bisavoo_tataravo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai_avoo_bisavoo.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avoo_bisavoo_tataravo.name = dr.GetString(0);
                    pai_avoo_bisavoo_tataravo.homologation = dr.GetString(1);
                    pai_avoo_bisavoo_tataravo.race.description = dr.GetString(2);
                    pai_avoo_bisavoo_tataravo.color.description = dr.GetString(3);
                    pai_avoo_bisavoo_tataravo.father.id = dr.GetInt32(4);
                    pai_avoo_bisavoo_tataravo.mother.id = dr.GetInt32(5);
                    try { pai_avoo_bisavoo_tataravo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avoo_bisavoo_tataravo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avoo_bisavoo_tataravo = pai_avoo_bisavoo_tataravo;



            // Tataravó paterno
            Cats pai_avoo_bisavoo_tataravoo = new Cats();
            pai_avoo_bisavoo_tataravoo.race = new cat_general_sigla();
            pai_avoo_bisavoo_tataravoo.color = new cat_general();
            pai_avoo_bisavoo_tataravoo.father = new parent();
            pai_avoo_bisavoo_tataravoo.mother = new parent();
            pai_avoo_bisavoo_tataravoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {pai_avoo_bisavoo.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    pai_avoo_bisavoo_tataravoo.name = dr.GetString(0);
                    pai_avoo_bisavoo_tataravoo.homologation = dr.GetString(1);
                    pai_avoo_bisavoo_tataravoo.race.description = dr.GetString(2);
                    pai_avoo_bisavoo_tataravoo.color.description = dr.GetString(3);
                    pai_avoo_bisavoo_tataravoo.father.id = dr.GetInt32(4);
                    pai_avoo_bisavoo_tataravoo.mother.id = dr.GetInt32(5);
                    try { pai_avoo_bisavoo_tataravoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { pai_avoo_bisavoo_tataravoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.pai_avoo_bisavoo_tataravoo = pai_avoo_bisavoo_tataravoo;






            // Mae
            Cats mae = new Cats();
            mae.race = new cat_general_sigla();
            mae.color = new cat_general();
            mae.father = new parent();
            mae.mother = new parent();
            mae.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {cat.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae.name = dr.GetString(0);
                    mae.homologation = dr.GetString(1);
                    mae.race.description = dr.GetString(2);
                    mae.color.description = dr.GetString(3);
                    mae.father.id = dr.GetInt32(4);
                    mae.mother.id = dr.GetInt32(5);
                    try { mae.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae = mae;




            // Avô Materno
            Cats mae_avo = new Cats();
            mae_avo.race = new cat_general_sigla();
            mae_avo.color = new cat_general();
            mae_avo.father = new parent();
            mae_avo.mother = new parent();
            mae_avo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avo.name = dr.GetString(0);
                    mae_avo.homologation = dr.GetString(1);
                    mae_avo.race.description = dr.GetString(2);
                    mae_avo.color.description = dr.GetString(3);
                    mae_avo.father.id = dr.GetInt32(4);
                    mae_avo.mother.id = dr.GetInt32(5);
                    try { mae_avo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avo = mae_avo;



            // Bisavô Maternos
            Cats mae_avo_bisavo = new Cats();
            mae_avo_bisavo.race = new cat_general_sigla();
            mae_avo_bisavo.color = new cat_general();
            mae_avo_bisavo.father = new parent();
            mae_avo_bisavo.mother = new parent();
            mae_avo_bisavo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae_avo.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avo_bisavo.name = dr.GetString(0);
                    mae_avo_bisavo.homologation = dr.GetString(1);
                    mae_avo_bisavo.race.description = dr.GetString(2);
                    mae_avo_bisavo.color.description = dr.GetString(3);
                    mae_avo_bisavo.father.id = dr.GetInt32(4);
                    mae_avo_bisavo.mother.id = dr.GetInt32(5);
                    try { mae_avo_bisavo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avo_bisavo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avo_bisavo = mae_avo_bisavo;



            // Tataravô Maternos
            Cats mae_avo_bisavo_tataravo = new Cats();
            mae_avo_bisavo_tataravo.race = new cat_general_sigla();
            mae_avo_bisavo_tataravo.color = new cat_general();
            mae_avo_bisavo_tataravo.father = new parent();
            mae_avo_bisavo_tataravo.mother = new parent();
            mae_avo_bisavo_tataravo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae_avo_bisavo.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avo_bisavo_tataravo.name = dr.GetString(0);
                    mae_avo_bisavo_tataravo.homologation = dr.GetString(1);
                    mae_avo_bisavo_tataravo.race.description = dr.GetString(2);
                    mae_avo_bisavo_tataravo.color.description = dr.GetString(3);
                    mae_avo_bisavo_tataravo.father.id = dr.GetInt32(4);
                    mae_avo_bisavo_tataravo.mother.id = dr.GetInt32(5);
                    try { mae_avo_bisavo_tataravo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avo_bisavo_tataravo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avo_bisavo_tataravo = mae_avo_bisavo_tataravo;

            // Tataravô Maternos
            Cats mae_avo_bisavo_tataravoo = new Cats();
            mae_avo_bisavo_tataravoo.race = new cat_general_sigla();
            mae_avo_bisavo_tataravoo.color = new cat_general();
            mae_avo_bisavo_tataravoo.father = new parent();
            mae_avo_bisavo_tataravoo.mother = new parent();
            mae_avo_bisavo_tataravoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae_avo_bisavo.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avo_bisavo_tataravoo.name = dr.GetString(0);
                    mae_avo_bisavo_tataravoo.homologation = dr.GetString(1);
                    mae_avo_bisavo_tataravoo.race.description = dr.GetString(2);
                    mae_avo_bisavo_tataravoo.color.description = dr.GetString(3);
                    mae_avo_bisavo_tataravoo.father.id = dr.GetInt32(4);
                    mae_avo_bisavo_tataravoo.mother.id = dr.GetInt32(5);
                    try { mae_avo_bisavo_tataravoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avo_bisavo_tataravoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avo_bisavo_tataravoo = mae_avo_bisavo_tataravo;





















            // Bisavó Materno
            Cats mae_avo_bisavoo = new Cats();
            mae_avo_bisavoo.race = new cat_general_sigla();
            mae_avo_bisavoo.color = new cat_general();
            mae_avo_bisavoo.father = new parent();
            mae_avo_bisavoo.mother = new parent();
            mae_avo_bisavoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae_avo.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avo_bisavoo.name = dr.GetString(0);
                    mae_avo_bisavoo.homologation = dr.GetString(1);
                    mae_avo_bisavoo.race.description = dr.GetString(2);
                    mae_avo_bisavoo.color.description = dr.GetString(3);
                    mae_avo_bisavoo.father.id = dr.GetInt32(4);
                    mae_avo_bisavoo.mother.id = dr.GetInt32(5);
                    try { mae_avo_bisavoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avo_bisavoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avo_bisavoo = mae_avo_bisavoo;


            // Tataravô Materno
            Cats mae_avo_bisavoo_tataravo = new Cats();
            mae_avo_bisavoo_tataravo.race = new cat_general_sigla();
            mae_avo_bisavoo_tataravo.color = new cat_general();
            mae_avo_bisavoo_tataravo.father = new parent();
            mae_avo_bisavoo_tataravo.mother = new parent();
            mae_avo_bisavoo_tataravo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae_avo_bisavoo.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avo_bisavoo_tataravo.name = dr.GetString(0);
                    mae_avo_bisavoo_tataravo.homologation = dr.GetString(1);
                    mae_avo_bisavoo_tataravo.race.description = dr.GetString(2);
                    mae_avo_bisavoo_tataravo.color.description = dr.GetString(3);
                    mae_avo_bisavoo_tataravo.father.id = dr.GetInt32(4);
                    mae_avo_bisavoo_tataravo.mother.id = dr.GetInt32(5);
                    try { mae_avo_bisavoo_tataravo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avo_bisavoo_tataravo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avo_bisavoo_tataravo = mae_avo_bisavoo_tataravo;


            // Tataravô Materno
            Cats mae_avo_bisavoo_tataravoo = new Cats();
            mae_avo_bisavoo_tataravoo.race = new cat_general_sigla();
            mae_avo_bisavoo_tataravoo.color = new cat_general();
            mae_avo_bisavoo_tataravoo.father = new parent();
            mae_avo_bisavoo_tataravoo.mother = new parent();
            mae_avo_bisavoo_tataravoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae_avo_bisavoo.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avo_bisavoo_tataravoo.name = dr.GetString(0);
                    mae_avo_bisavoo_tataravoo.homologation = dr.GetString(1);
                    mae_avo_bisavoo_tataravoo.race.description = dr.GetString(2);
                    mae_avo_bisavoo_tataravoo.color.description = dr.GetString(3);
                    mae_avo_bisavoo_tataravoo.father.id = dr.GetInt32(4);
                    mae_avo_bisavoo_tataravoo.mother.id = dr.GetInt32(5);
                    try { mae_avo_bisavoo_tataravoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avo_bisavoo_tataravoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avo_bisavoo_tataravoo = mae_avo_bisavoo_tataravoo;















            // Avó Materno
            Cats mae_avoo = new Cats();
            mae_avoo.race = new cat_general_sigla();
            mae_avoo.color = new cat_general();
            mae_avoo.father = new parent();
            mae_avoo.mother = new parent();
            mae_avoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avoo.name = dr.GetString(0);
                    mae_avoo.homologation = dr.GetString(1);
                    mae_avoo.race.description = dr.GetString(2);
                    mae_avoo.color.description = dr.GetString(3);
                    mae_avoo.father.id = dr.GetInt32(4);
                    mae_avoo.mother.id = dr.GetInt32(5);
                    try { mae_avoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avoo = mae_avoo;




            // Bisavô Materno
            Cats mae_avoo_bisavo = new Cats();
            mae_avoo_bisavo.race = new cat_general_sigla();
            mae_avoo_bisavo.color = new cat_general();
            mae_avoo_bisavo.father = new parent();
            mae_avoo_bisavo.mother = new parent();
            mae_avoo_bisavo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae_avoo.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avoo_bisavo.name = dr.GetString(0);
                    mae_avoo_bisavo.homologation = dr.GetString(1);
                    mae_avoo_bisavo.race.description = dr.GetString(2);
                    mae_avoo_bisavo.color.description = dr.GetString(3);
                    mae_avoo_bisavo.father.id = dr.GetInt32(4);
                    mae_avoo_bisavo.mother.id = dr.GetInt32(5);
                    try { mae_avoo_bisavo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avoo_bisavo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avoo_bisavo = mae_avoo_bisavo;


            // Tataravô Materno
            Cats mae_avoo_bisavo_tataravo = new Cats();
            mae_avoo_bisavo_tataravo.race = new cat_general_sigla();
            mae_avoo_bisavo_tataravo.color = new cat_general();
            mae_avoo_bisavo_tataravo.father = new parent();
            mae_avoo_bisavo_tataravo.mother = new parent();
            mae_avoo_bisavo_tataravo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae_avoo_bisavo.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avoo_bisavo_tataravo.name = dr.GetString(0);
                    mae_avoo_bisavo_tataravo.homologation = dr.GetString(1);
                    mae_avoo_bisavo_tataravo.race.description = dr.GetString(2);
                    mae_avoo_bisavo_tataravo.color.description = dr.GetString(3);
                    mae_avoo_bisavo_tataravo.father.id = dr.GetInt32(4);
                    mae_avoo_bisavo_tataravo.mother.id = dr.GetInt32(5);
                    try { mae_avoo_bisavo_tataravo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avoo_bisavo_tataravo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avoo_bisavo_tataravo = mae_avoo_bisavo_tataravo;



            // Tataravô Materno
            Cats mae_avoo_bisavo_tataravoo = new Cats();
            mae_avoo_bisavo_tataravoo.race = new cat_general_sigla();
            mae_avoo_bisavo_tataravoo.color = new cat_general();
            mae_avoo_bisavo_tataravoo.father = new parent();
            mae_avoo_bisavo_tataravoo.mother = new parent();
            mae_avoo_bisavo_tataravoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae_avoo_bisavo.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avoo_bisavo_tataravoo.name = dr.GetString(0);
                    mae_avoo_bisavo_tataravoo.homologation = dr.GetString(1);
                    mae_avoo_bisavo_tataravoo.race.description = dr.GetString(2);
                    mae_avoo_bisavo_tataravoo.color.description = dr.GetString(3);
                    mae_avoo_bisavo_tataravoo.father.id = dr.GetInt32(4);
                    mae_avoo_bisavo_tataravoo.mother.id = dr.GetInt32(5);
                    try { mae_avoo_bisavo_tataravoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avoo_bisavo_tataravoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avoo_bisavo_tataravoo = mae_avoo_bisavo_tataravoo;




            // Bisavó Materno
            Cats mae_avoo_bisavoo = new Cats();
            mae_avoo_bisavoo.race = new cat_general_sigla();
            mae_avoo_bisavoo.color = new cat_general();
            mae_avoo_bisavoo.father = new parent();
            mae_avoo_bisavoo.mother = new parent();
            mae_avoo_bisavoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae_avoo.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avoo_bisavoo.name = dr.GetString(0);
                    mae_avoo_bisavoo.homologation = dr.GetString(1);
                    mae_avoo_bisavoo.race.description = dr.GetString(2);
                    mae_avoo_bisavoo.color.description = dr.GetString(3);
                    mae_avoo_bisavoo.father.id = dr.GetInt32(4);
                    mae_avoo_bisavoo.mother.id = dr.GetInt32(5);
                    try { mae_avoo_bisavoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avoo_bisavoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avoo_bisavoo = mae_avoo_bisavoo;


            // Tataravô Materno
            Cats mae_avoo_bisavoo_tataravo = new Cats();
            mae_avoo_bisavoo_tataravo.race = new cat_general_sigla();
            mae_avoo_bisavoo_tataravo.color = new cat_general();
            mae_avoo_bisavoo_tataravo.father = new parent();
            mae_avoo_bisavoo_tataravo.mother = new parent();
            mae_avoo_bisavoo_tataravo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae_avoo_bisavoo.father.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avoo_bisavoo_tataravo.name = dr.GetString(0);
                    mae_avoo_bisavoo_tataravo.homologation = dr.GetString(1);
                    mae_avoo_bisavoo_tataravo.race.description = dr.GetString(2);
                    mae_avoo_bisavoo_tataravo.color.description = dr.GetString(3);
                    mae_avoo_bisavoo_tataravo.father.id = dr.GetInt32(4);
                    mae_avoo_bisavoo_tataravo.mother.id = dr.GetInt32(5);
                    try { mae_avoo_bisavoo_tataravo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avoo_bisavoo_tataravo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avoo_bisavoo_tataravo = mae_avoo_bisavoo_tataravo;


            // Tataravô Materno
            Cats mae_avoo_bisavoo_tataravoo = new Cats();
            mae_avoo_bisavoo_tataravoo.race = new cat_general_sigla();
            mae_avoo_bisavoo_tataravoo.color = new cat_general();
            mae_avoo_bisavoo_tataravoo.father = new parent();
            mae_avoo_bisavoo_tataravoo.mother = new parent();
            mae_avoo_bisavoo_tataravoo.eye = new cat_general_sigla();

            sqlString = $"select a.nomgat, a.pedgat, b.desras, c.descor, a.cpggat, a.cmggat, d.sigla, a.pais_origem from cad_gat a left join cad_ras b on b.codras = a.rasgat left join cad_cor c on c.codcor = a.corgat left join cat_olho d on d.olho_codigo = a.olho_gato where a.codgat = {mae_avoo_bisavoo.mother.id}";
            dr = sqlcmd(sqlString);
            if (dr != null)
            {
                while (dr.Read())
                {
                    mae_avoo_bisavoo_tataravoo.name = dr.GetString(0);
                    mae_avoo_bisavoo_tataravoo.homologation = dr.GetString(1);
                    mae_avoo_bisavoo_tataravoo.race.description = dr.GetString(2);
                    mae_avoo_bisavoo_tataravoo.color.description = dr.GetString(3);
                    mae_avoo_bisavoo_tataravoo.father.id = dr.GetInt32(4);
                    mae_avoo_bisavoo_tataravoo.mother.id = dr.GetInt32(5);
                    try { mae_avoo_bisavoo_tataravoo.eye.sigla = dr.GetString(6); } catch (Exception) { }
                    try { mae_avoo_bisavoo_tataravoo.country_origin = dr.GetString(7); } catch (Exception) { }
                }
                dr.Close();
            }
            pedigree.mae_avoo_bisavoo_tataravoo = mae_avoo_bisavoo_tataravoo;














            return pedigree;
        }




    }
}
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public static class Database
    {
        #region Private Fields

        private const string COL_ADMIN = "Admin";
        private const string COL_IDELEVE = "idEleve";
        private const string COL_IDSKILL = "idCompetence";
        private const string COL_LOGIN = "Login";
        private const string COL_MAXNOTE = "maxNote";
        private const string COL_NOTE = "Note";
        private const string COL_PASS = "Password";
        private const string COL_PROMO = "Promotion";
        private const string TAB_CLASSE = "classe";
        private const string TAB_ELEVE = "eleve";
        private const string TAB_TP = "tp";
        private const string TAB_USER = "user";
        private static MySqlConnection _conn;

        #endregion Private Fields

        #region Public Properties

        public static string DatabaseName { get; private set; }
        public static string Password { get; private set; }
        public static string Server { get; private set; }

        public static string Username { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static int addCPMax(List<string> listCP)
        {
            if (listCP.Count != 0)
            {
                var command = _conn.CreateCommand();
                foreach (var idCP in listCP)
                {
                    ("INSERT INTO competence (idCompetence) VALUES ('" + idCP + "')").SimpleRequest();
                }
                return 0;
            }
            return 1;
        }

        public static void AddNote(string idPdf, string idCompetence, string Note, string maxNote)
        {
            //MessageBox.Show("INSERT INTO note (idPdf, idCompetence, note, maxnote) VALUES (\"" + idPdf + "\", \"" +
            // idCompetence +
            //      "\", \"" + Note + "\", \"" + maxNote + "\")");
            ("INSERT INTO note (idPdf, idCompetence, note, maxnote) VALUES (\"" + idPdf + "\", \"" + idCompetence +
             "\", \"" + Note + "\", \"" + maxNote + "\")").SimpleRequest();
        }

        public static void AddTp(string tpname, string idEleve, string login_correcteur, string hash)
        {
            /*MessageBox.Show("INSERT INTO tp (idTp, idEleve, idcorrecteur, hashTp) VALUES(\"" + tpname + "\", \"" +
                            idEleve + "\", (SELECT idUser FROM user WHERE Login = \"" + login_correcteur + "\"), \"" +
                            hash + "\")"); */
            ("INSERT INTO tp (idTp, idEleve, idcorrecteur, hashTp) VALUES(\"" + tpname + "\", \"" + idEleve +
             "\", (SELECT idUser FROM user WHERE Login = \"" + login_correcteur + "\"), \"" + hash + "\")")
                .SimpleRequest();
        }

        public static int AddUser(string login, string mdp, int statut)
        {
            var command = _conn.CreateCommand();
            command.CommandText = "INSERT INTO " + TAB_USER + "(" + COL_LOGIN + "," + COL_PASS + "," + COL_ADMIN +
                                  ") VALUES ('" + login + "','" + mdp + "','" + statut + "')";
            var retour = command.ExecuteReader();

            if (retour.Read()) // si erreur il y a
            {
                retour.Close();
                return 1;
            }
            retour.Close();
            return 1;
        }

        public static void AjouteEleve(string nom, string prenom, string promo)
        {
            //MessageBox.Show("INSERT INTO " + TAB_ELEVE + " (Nom, Prenom, idClasse) VALUES (\"" + nom + "\", \"" + prenom + "\", (SELECT idClasse FROM classe WHERE Promotion = \"" + promo + "\"))");
            ("INSERT INTO " + TAB_ELEVE + " (Nom, Prenom, idClasse) VALUES (\"" + nom + "\", \"" + prenom +
             "\", ( SELECT idClasse FROM classe WHERE Promotion = \"" + promo + "\"))").SimpleRequest();
        }

        public static void ajouterPromo(string promo)
        {
            var command = _conn.CreateCommand();
            command.CommandText = "INSERT INTO classe (Promotion, hashClasse) VALUES ('" + promo + "', '0')";
            var reader = command.ExecuteReader();
            reader.Close();
        }

        public static void BackupDatabase(string backUpFile = "C:/databackup/database.sql")
        {
            using (_conn)
            {
                using (var cmd = new MySqlCommand())
                {
                    using (var mb = new MySqlBackup(cmd))
                    {
                        var fileData = new SaveFileDialog();
                        fileData.Title = "Exporter la base de données";
                        fileData.DefaultExt = "sql";
                        fileData.Filter = "Fichier SQL (*.sql)|*.sql";
                        var result = fileData.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            backUpFile = fileData.FileName;
                        }
                        cmd.Connection = _conn;
                        mb.ExportToFile(backUpFile);
                        Connect();
                    }
                }
            }
        }

        public static string ChangerLogin(string login, string pass, string ancienlog)
        {
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT " + COL_PASS + " FROM " + TAB_USER + " WHERE " + COL_LOGIN + "='" + ancienlog +
                                  "'";

            var pass2 = "";
            var retour = command.ExecuteReader();

            while (retour.Read())
            {
                pass2 = retour["Password"].ToString();
            }
            retour.Close();

            if ((pass == pass2) && (login != ""))
            {
                var Request = "UPDATE " + TAB_USER + " SET " + COL_LOGIN + "='" + login + "' WHERE " + COL_LOGIN + "='" +
                              ancienlog + "'";
                var command2 = _conn.CreateCommand();
                command2.CommandText = Request;
                command2.ExecuteNonQuery();

                MessageBox.Show("Changement réussi");
                return login;
            }

            MessageBox.Show("Mot de passe actuel incorrect");
            return "";
        }

        public static int ChangerMdp(string login, string pass, string ancienmdp)
        {
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT " + COL_PASS + " FROM " + TAB_USER + " WHERE " + COL_LOGIN + "='" + login +
                                  "'";

            var pass2 = "";
            var retour = command.ExecuteReader();

            while (retour.Read())
            {
                pass2 = retour["Password"].ToString(); // mot de passe récuperé de la BDD
            }
            retour.Close();

            if ((ancienmdp == pass2) && (pass != ""))
                // on compare le mdp récuperé avec celui tapé dans le champ "ancien mot de passe"
            {
                // on vérifie que le nouveau mdp ne soit pas vide
                var Request = "UPDATE " + TAB_USER + " SET " + COL_PASS + "='" + pass + "' WHERE " + COL_LOGIN + "='" +
                              login + "' AND " + COL_PASS + "='" + ancienmdp + "'";
                var command2 = _conn.CreateCommand();
                command2.CommandText = Request;
                command2.ExecuteNonQuery();

                MessageBox.Show("Changement réussi");
                return 1;
            }

            MessageBox.Show("Ancien mot de passe incorrect");
            return 0;
        }

        public static void Connect(string ip = "localhost", string login = "root", string pass = "",
            string database = "mydb")
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = ip,
                UserID = login,
                Password = pass,
                Database = database
            };

            Server = ip;
            Username = login;
            Password = pass;
            DatabaseName = database;

            _conn = new MySqlConnection(builder.ToString());
            try
            {
                _conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static List<string> CPMaxIsNotinNote()
        {
            var listToRemoveCP = new List<string>();
            var command = _conn.CreateCommand();
            command.CommandText =
                "SELECT DISTINCT competence.idCompetence FROM competence LEFT JOIN note ON competence.idCompetence = note.idCompetence WHERE note.idCompetence IS NULL";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                listToRemoveCP.Add(reader["idCompetence"].ToString());
            }
            reader.Close();

            return listToRemoveCP;
        }

        public static List<string> CPsNewInNote()
        {
            var listnewCP = new List<string>();
            var command = _conn.CreateCommand();
            command.CommandText =
                "SELECT DISTINCT note.idCompetence FROM note LEFT JOIN competence ON note.idCompetence = competence.idCompetence WHERE competence.idCompetence IS NULL";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                listnewCP.Add(reader["idCompetence"].ToString());
            }
            reader.Close();

            return listnewCP;
        }

        public static int Delete(string user)
        {
            var command = _conn.CreateCommand();
            command.CommandText = "DELETE FROM " + TAB_USER + " WHERE " + COL_LOGIN + "='" + user + "'";
            var retour = command.ExecuteReader();

            if (retour.Read()) // si Erreur il y a
            {
                retour.Close();
                return -1;
            }
            retour.Close();
            return 1;
        }

        public static int DeleteElv(string eleve)
        {
            var eleve2 = eleve.Split(' ');
            MessageBox.Show(eleve2[0] + "   " + eleve2[1]);
            var command = _conn.CreateCommand();
            command.CommandText = "DELETE FROM " + TAB_ELEVE + " WHERE Nom ='" + eleve2[0] + "'AND Prenom='" + eleve2[1] +
                                  "'";
            var retour = command.ExecuteReader();

            if (retour.Read()) // si Erreur il y a
            {
                retour.Close();
                return -1;
            }
            retour.Close();
            return 1;
        }

        public static void DeletePromo(string promo)
        {
            MessageBox.Show("DELETE FROM " + TAB_CLASSE + " WHERE " + COL_PROMO + " = " + promo);
            ("DELETE FROM " + TAB_CLASSE + " WHERE " + COL_PROMO + " = " + promo).SimpleRequest();
        }

        public static void DeleteTp(string hashes)
        {
            ("DELETE FROM " + TAB_TP + " WHERE hashTp = " + hashes).SimpleRequest();
        }

        public static void DelRequest(string table, Tuple<string, string>[] where_clause)
        {
            var request = "DELETE FROM " + table + " WHERE ";

            var secondLine = false;
            foreach (var a in where_clause)
            {
                if (secondLine)
                    request += " AND ";
                request += a.Item1 + "='" + a.Item2 + "'";
                secondLine = true;
            }

            var command = _conn.CreateCommand();
            command.CommandText = request;
            command.ExecuteNonQuery();
        }

        public static List<string> GetHashList(string promo)
        {
            var a = new List<string>();
            var command = _conn.CreateCommand();

            command.CommandText = "SELECT hashTp FROM " + TAB_CLASSE + " NATURAL JOIN " + TAB_ELEVE + " NATURAL JOIN " +
                                  TAB_TP + " WHERE " + COL_PROMO + " = " + promo;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                a.Add(reader["hashTp"].ToString());
            }
            reader.Close();
            return a;
        }

        public static string getidClasse(string promo)
        {
            var idclasse = "";
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT idClasse FROM classe WHERE Promotion ='" + promo + "'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                idclasse = reader["idClasse"].ToString();
            }
            var id = idclasse;
            //MessageBox.Show(id);
            reader.Close();
            //var Location = 1;
            if (id == "")
            {
                ("INSERT INTO classe (Promotion,Location) VALUES ('" + promo + "')").SimpleRequest();

                var command2 = _conn.CreateCommand();
                command2.CommandText = "SELECT idClasse FROM classe WHERE Promotion ='" + promo + "'";
                var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    idclasse = reader2["idClasse"].ToString();
                }
                id = idclasse;
                reader2.Close();
                MessageBox.Show(id);
            }
            return id;
        }

        public static string GetIdEleveFromName(string nom, string prenom)
        {
            var b = "";
            foreach (
                var a in
                    GetListRequest(TAB_ELEVE, new[] {COL_IDELEVE},
                        "Nom = \"" + nom + "\" AND Prenom = \"" + prenom + "\""))
            {
                b = a;
                return b;
            }
            return null;
        }

        public static string GetLastPdfId()
        {
            var retour = string.Empty;
            var req = "SELECT max(idPdf) FROM tp";
            var command = _conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                retour = r["max(idPdf)"].ToString();
            }

            r.Close();
            return retour;
        }

        public static IEnumerable<string> GetListRequest(string table, string[] columns,
            string additional_where_clause = "1")
        {
            var retour = new List<string>();
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT * FROM " + table + " WHERE " + additional_where_clause;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                try
                {
                    var toReturn = columns.Aggregate(string.Empty, (current, c) => current + r[c].ToString() + " ");
                    retour.Add(toReturn);
                }
                catch (Exception)
                {
                    r.Close();
                    return retour;
                }
            }
            r.Close();
            return retour;
        }

        public static IEnumerable<string> GetDistinctRequest(string table,string element, string[] columns,
            string additional_where_clause = "1")
        {
            var retour = new List<string>();
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT DISTINCT " + element + " FROM " + table + " WHERE " + additional_where_clause;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                try
                {
                    var toReturn = columns.Aggregate(string.Empty, (current, c) => current + r[c].ToString() + " ");
                    retour.Add(toReturn);
                }
                catch (Exception)
                {
                    r.Close();
                    return retour;
                }
            }
            r.Close();
            return retour;
        }

        public static decimal getMaxCP(string idCompetence)
        {
            var comp = "";
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT maxEchelle FROM competence WHERE idCompetence ='" + idCompetence + "'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                comp = reader["maxEchelle"].ToString();
            }
            var comp2 = comp;
            //MessageBox.Show(id);
            reader.Close();

            return decimal.Parse(comp2);
        }

        public static string getpromo(string idClasse)
        {
            var promo = "";
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT Promotion FROM classe WHERE idClasse ='" + idClasse + "'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                promo = reader["Promotion"].ToString();
            }
            var promo2 = promo;
            //MessageBox.Show(id);
            reader.Close();

            return promo2;
        }

        public static List<Tuple<string, float>> GetWebClasseRequest(string idPromo = "2017")
        {
            var retour = new List<Tuple<string, float>>();
            var req =
                "SELECT " + COL_IDSKILL + ", SUM(" + COL_NOTE + ")/count(DISTINCT " + COL_IDELEVE + ") AS somme FROM " +
                TAB_CLASSE + " NATURAL JOIN " + TAB_ELEVE + " NATURAL JOIN " + TAB_TP + " NATURAL JOIN " + COL_NOTE +
                " WHERE " + COL_PROMO + "=" + idPromo + " GROUP BY " + COL_IDSKILL;
            var command = _conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                retour.Add(new Tuple<string, float>(r[COL_IDSKILL].ToString(), float.Parse(r["somme"].ToString())));
            }

            r.Close();
            return retour;
        }

        public static List<Tuple<string, float>> GetWebMax()
        {
            var retour = new List<Tuple<string, float>>();
            var req = "SELECT " + COL_IDSKILL + ", maxEchelle FROM competence";
            var command = _conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                retour.Add(new Tuple<string, float>(r[COL_IDSKILL].ToString(), float.Parse(r["maxEchelle"].ToString())));
            }

            r.Close();

            return retour;
        }

        public static List<Tuple<string, float>> GetWebRequest(CheckBox checkBox, string idEleve = "2")
        {
            var req = "";
            var retour = new List<Tuple<string, float>>();
            if (checkBox.Checked == false)
            {
                req =
                    "SELECT " + COL_IDSKILL + ", SUM(" + COL_NOTE + ") FROM " + TAB_ELEVE + " NATURAL JOIN " + TAB_TP +
                    " NATURAL JOIN " + COL_NOTE + " WHERE " + COL_IDELEVE + " = '" + idEleve + "' GROUP BY " + COL_IDSKILL;
            }
            else if (checkBox.Checked == false)
            {
                req =
                    "SELECT " + COL_IDSKILL + ", SUM(" + COL_NOTE + ") FROM " + TAB_ELEVE + " NATURAL JOIN " + TAB_TP +
                    " NATURAL JOIN " + COL_NOTE + " WHERE " + COL_IDELEVE + " = '" + idEleve + "' GROUP BY " + COL_IDSKILL;
            }

            var command = _conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                retour.Add(new Tuple<string, float>(r[COL_IDSKILL].ToString(),
                    float.Parse(r["SUM(" + COL_NOTE + ")"].ToString())));
            }

            r.Close();
            return retour;
        }

        public static List<Tuple<string, float, int>> GetWtfRequest(string idEleve = "2")
        {
            var req =
                "SELECT " + COL_IDSKILL + ",COUNT(" + COL_NOTE + ") AS quantite, ((SUM(" + COL_NOTE + "/" + COL_MAXNOTE +
                ")/COUNT(" + COL_NOTE + "))*100) AS moyenne FROM " + TAB_ELEVE + " NATURAL JOIN " + TAB_TP +
                " NATURAL JOIN " + COL_NOTE + " WHERE " + COL_IDELEVE + " = " + idEleve + " GROUP BY " + COL_IDSKILL;
            var command = _conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            var a = new List<Tuple<string, float, int>>();

            while (r.Read())
            {
                a.Add(new Tuple<string, float, int>(r[COL_IDSKILL].ToString(), float.Parse(r["moyenne"].ToString()),
                    int.Parse(r["quantite"].ToString())));
            }

            r.Close();
            return a;
        }

        public static List<Tuple<float,DateTime>> GetCourbeRequest(string idEleve, string idComp ="C1.1")
        {
            var req = "SELECT date, note FROM tp NATURAL JOIN note WHERE idEleve='" + idEleve + "' AND idCompetence ='" + idComp + "'";
            var command = _conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            var a = new List<Tuple<float,DateTime>>();

            while (r.Read())
            {
                a.Add(new Tuple<float,DateTime>(float.Parse(r["note"].ToString()),DateTime.Parse(r["date"].ToString())));
            }

            a.OrderByDescending(b => b.Item2);

            r.Close();
            return a;
        }
        

        public static List<Tuple<string, float, int>> GetWthRequest(string idPromo)
        {
            //var req2 =
            // "SELECT " + COL_IDSKILL + ",COUNT(" + COL_NOTE + ") AS quantite, ((SUM(" + COL_NOTE + "/" + COL_MAXNOTE + ")/COUNT(" + COL_NOTE + "))*100) AS moyenne FROM " + TAB_ELEVE + " NATURAL JOIN " + TAB_TP + " NATURAL JOIN " + COL_NOTE + " WHERE " + COL_PROMO + " = " + idEleve + " GROUP BY " + COL_IDSKILL;
            var req =
                "SELECT " + COL_IDSKILL + ", count(*) AS nbTp, ((SUM(" + COL_NOTE + "/" + COL_MAXNOTE + ")/COUNT(" +
                COL_NOTE + "))*100) AS moyenne FROM " + TAB_CLASSE + " NATURAL JOIN " + TAB_ELEVE + " NATURAL JOIN " +
                TAB_TP + " NATURAL JOIN " + COL_NOTE + " WHERE " + COL_PROMO + "=" + idPromo + " GROUP BY " +
                COL_IDSKILL;

            var command = _conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            var a = new List<Tuple<string, float, int>>();

            while (r.Read())
            {
                a.Add(new Tuple<string, float, int>(r[COL_IDSKILL].ToString(), float.Parse(r["moyenne"].ToString()),
                    int.Parse(r["nbTp"].ToString())));
            }

            r.Close();
            return a;
        }

        public static int Login(string login, string pass)
        {
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT " + COL_LOGIN + "," + COL_PASS + "," + COL_ADMIN + " FROM " + TAB_USER +
                                  " WHERE " + COL_LOGIN + "='" + login + "' AND " + COL_PASS + "='" + pass + "'";
            var retour = command.ExecuteReader();

            if (login.Equals("admin") && pass.Equals("29042016"))
            {
                retour.Close();
                return 1; //Compte admin en dur pour pouvoir débug si BDD plante
            }

            if (!retour.Read())
            {
                retour.Close();
                return -1;
            }

            var r = retour[COL_ADMIN].ToString().Equals("True") ? 1 : 0;
            retour.Close();
            return r;
        }

        public static string[] PromoEleve(string promo)
        {
            var retour = new string[1000];
            var command = _conn.CreateCommand();
            var idClasse = "";
            command.CommandText = "SELECT idClasse FROM classe WHERE Promotion ='" + promo + "'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                idClasse = reader["idClasse"].ToString();
            }
            var id = idClasse;
            //MessageBox.Show(id);
            reader.Close();
            var command2 = _conn.CreateCommand();
            var i = 0;
            command2.CommandText = "SELECT Prenom,Nom FROM eleve WHERE idClasse ='" + id + "'";
            var reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {
                var concatenate = reader2["Prenom"] + " " + reader2["Nom"];
                retour[i] = concatenate;
                i++;
            }
            reader2.Close();
            return retour;
        }

        public static string[] RecupEleveAvecPromo(string promo)
        {
            var retour = new string[1000];
            var i = 0;
            var command = _conn.CreateCommand();
            command.CommandText = "SELECT Nom, Prenom FROM `classe` NATURAL JOIN eleve WHERE Promotion = " + promo;
            var r = command.ExecuteReader();
            while (r.Read())
            {
                var concatenate = r["Prenom"] + " " + r["Nom"];
                //MessageBox.Show(concatenate);
                retour[i] = concatenate;
                i = i + 1;
            }
            r.Close();
            return retour;
        }

        public static void removeCPFromWeb(List<Tuple<string, float>> listCP, string prenom, string nom)
        {
            var command = _conn.CreateCommand();
            var listCP2 = new List<Tuple<string, float>>(listCP);
            foreach (var idCPMax in listCP2)
            {
                var boule = false;
                if (Program.ac.graphic.isNameSelected)
                    command.CommandText =
                        "SELECT DISTINCT idCompetence FROM eleve NATURAL JOIN tp NATURAL JOIN note WHERE nom = '" + nom +
                        "' AND prenom='" + prenom + "' AND idCompetence='" + idCPMax.Item1 + "'";
                if (Program.ac.graphic.isNameSelected == false)
                    command.CommandText =
                        "SELECT DISTINCT idCompetence FROM eleve NATURAL JOIN tp NATURAL JOIN note WHERE idClasse='" +
                        getidClasse(Program.ac.graphic.promotionSelected) + "' AND idCompetence='" + idCPMax.Item1 + "'";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    boule = true;
                }
                if (!boule)
                    listCP.Remove(idCPMax);
                reader.Close();
            }
        }

        public static int removeCPMax(List<string> listCP)
        {
            if (listCP.Count != 0)
            {
                var command = _conn.CreateCommand();
                foreach (var idCP in listCP)
                {
                    ("DELETE FROM `mydb`.`competence` WHERE `competence`.`idCompetence` = \'" + idCP + "'")
                        .SimpleRequest();
                }
                return 0;
            }
            return 1;
        }

        public static int RestoreDatabase(string restoredFile = "C:/databackup/database.sql")
        {
            using (_conn)
            {
                using (var cmd = new MySqlCommand())
                {
                    using (var mb = new MySqlBackup(cmd))
                    {
                        MessageBox.Show(
                            "Attention toutes modification de base de données est irréversible ! Le logiciel rédemarrera après importation.");
                        var fileData = new OpenFileDialog();
                        fileData.Title = "Importer une base de données";
                        fileData.DefaultExt = "sql";
                        fileData.Filter = "Fichier SQL (*.sql)|*.sql";
                        var result = fileData.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            restoredFile = fileData.FileName;
                        }
                        cmd.Connection = _conn;
                        try
                        {
                            mb.ImportFromFile(restoredFile);
                            return 0;
                        }
                        catch
                        {
                            MessageBox.Show("L'importation a échouée");
                            return 1;
                        }
                    }
                }
            }
        }

        public static void SetHash(string nomColonne, string hash, string condition)
        {
            ("UPDATE " + TAB_CLASSE + " SET" + nomColonne + " = '" + hash + "' WHERE " + nomColonne + "=" + condition)
                .SimpleRequest();
        }

        public static void setMaxCP(string idCompetence, decimal maxCP)
        {
            var command = _conn.CreateCommand();
            command.CommandText = "UPDATE competence SET maxEchelle = " + maxCP + " WHERE idCompetence = '" +
                                  idCompetence + "'";
            var reader = command.ExecuteReader();
            //MessageBox.Show(id);
            reader.Close();
        }

        public static bool SimpleRequest(this string r)
        {
            try
            {
                var neweleve = new MySqlCommand(r, _conn) {CommandText = r};
                neweleve.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion Public Methods
    }
}

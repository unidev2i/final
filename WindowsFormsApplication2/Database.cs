// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Database.cs" company="ig2i">
//   Unidev
// </copyright>
// <summary>
//   The database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WindowsFormsApplication2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using MySql.Data.MySqlClient;
    using System.Globalization;

    /// <summary>
    /// The database.
    /// </summary>
    public static class Database
    {
        #region Private Fields

        /// <summary>
        /// The co l_ admin.
        /// </summary>
        private const string ColAdmin = "Admin";

        /// <summary>
        /// The co l_ ideleve.
        /// </summary>
        private const string ColIdeleve = "idEleve";

        /// <summary>
        /// The co l_ idskill.
        /// </summary>
        private const string ColIdskill = "idCompetence";

        /// <summary>
        /// The co l_ login.
        /// </summary>
        private const string ColLogin = "Login";

        /// <summary>
        /// The co l_ maxnote.
        /// </summary>
        private const string ColMaxnote = "maxNote";

        /// <summary>
        /// The co l_ note.
        /// </summary>
        private const string ColNote = "Note";

        /// <summary>
        /// The co l_ pass.
        /// </summary>
        private const string ColPass = "Password";

        /// <summary>
        /// The co l_ promo.
        /// </summary>
        private const string ColPromo = "Promotion";

        /// <summary>
        /// The ta b_ classe.
        /// </summary>
        private const string TabClasse = "classe";

        /// <summary>
        /// The ta b_ eleve.
        /// </summary>
        private const string TabEleve = "eleve";

        /// <summary>
        /// The ta b_ tp.
        /// </summary>
        private const string TabTp = "tp";

        /// <summary>
        /// The ta b_ user.
        /// </summary>
        private const string TabUser = "user";

        /// <summary>
        /// The conn.
        /// </summary>
        private static MySqlConnection conn;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the database name.
        /// </summary>
        public static string DatabaseName { get; private set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public static string Password { get; private set; }

        /// <summary>
        /// Gets the server.
        /// </summary>
        public static string Server { get; private set; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public static string Username { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// The add cp max.
        /// </summary>
        /// <param name="listCp">
        /// The list cp.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int AddCpMax(List<string> listCp)
        {
            if (listCp.Count == 0)
            {
                return 1;
            }
            var command = conn.CreateCommand();
            foreach (var idCp in listCp)
            {
                ("INSERT INTO competence (idCompetence) VALUES ('" + idCp + "')").SimpleRequest();
            }

            return 0;
        }

        /// <summary>
        /// The add note.
        /// </summary>
        /// <param name="idPdf">
        /// The id pdf.
        /// </param>
        /// <param name="idCompetence">
        /// The id competence.
        /// </param>
        /// <param name="note">
        /// The note.
        /// </param>
        /// <param name="maxNote">
        /// The max note.
        /// </param>
        public static void AddNote(string idPdf, string idCompetence, string note, string maxNote)
        {
            // MessageBox.Show("INSERT INTO note (idPdf, idCompetence, note, maxnote) VALUES (\"" + idPdf + "\", \"" +
            // idCompetence +
            // "\", \"" + Note + "\", \"" + maxNote + "\")");
            ("INSERT INTO note (idPdf, idCompetence, note, maxnote) VALUES (\"" + idPdf + "\", \"" + idCompetence +
             "\", \"" + note + "\", \"" + maxNote + "\")").SimpleRequest();
        }

        /// <summary>
        /// The add tp.
        /// </summary>
        /// <param name="tpname">
        /// The tpname.
        /// </param>
        /// <param name="idEleve">
        /// The id eleve.
        /// </param>
        /// <param name="loginCorrecteur">
        /// The login_correcteur.
        /// </param>
        /// <param name="hash">
        /// The hash.
        /// </param>
        /// <param name="dt">
        /// The dt.
        /// </param>
        public static void AddTp(string tpname, string idEleve, string loginCorrecteur, string hash, DateTime dt)
        {
            /* MessageBox.Show("INSERT INTO tp (idTp, idEleve, idcorrecteur, hashTp) VALUES(\"" + tpname + "\", \"" +
                            idEleve + "\", (SELECT idUser FROM user WHERE Login = \"" + login_correcteur + "\"), \"" +
                            hash + "\")"); */
            //MessageBox.Show(dt.ToString("yyyy-MM-dd HH:mm:ss"));

            ("INSERT INTO tp (idTp, idEleve, idcorrecteur, hashTp, date) VALUES(\"" + tpname + "\", \"" + idEleve +
             "\", (SELECT idUser FROM user WHERE Login = \"" + loginCorrecteur + "\"), \"" + hash + "\",'" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "')")
                .SimpleRequest();
        }

        /// <summary>
        /// The add user.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="mdp">
        /// The mdp.
        /// </param>
        /// <param name="statut">
        /// The statut.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int AddUser(string login, string mdp, int statut)
        {
            var command = conn.CreateCommand();
            command.CommandText = "INSERT INTO " + TabUser + "(" + ColLogin + "," + ColPass + "," + ColAdmin +
                                  ") VALUES ('" + login + "','" + mdp + "','" + statut + "')";
            var retour = command.ExecuteReader();

            if (retour.Read())
            {
// si erreur il y a
                retour.Close();
                return 1;
            }

            retour.Close();
            return 1;
        }

        /// <summary>
        /// The ajoute eleve.
        /// </summary>
        /// <param name="nom">
        /// The nom.
        /// </param>
        /// <param name="prenom">
        /// The prenom.
        /// </param>
        /// <param name="promo">
        /// The promo.
        /// </param>
        public static void AjouteEleve(string nom, string prenom, string promo)
        {
            // MessageBox.Show("INSERT INTO " + TAB_ELEVE + " (Nom, Prenom, idClasse) VALUES (\"" + nom + "\", \"" + prenom + "\", (SELECT idClasse FROM classe WHERE Promotion = \"" + promo + "\"))");
            ("INSERT INTO " + TabEleve + " (Nom, Prenom, idClasse) VALUES (\"" + nom + "\", \"" + prenom +
             "\", ( SELECT idClasse FROM classe WHERE Promotion = \"" + promo + "\"))").SimpleRequest();
        }

        /// <summary>
        /// The ajouter promo.
        /// </summary>
        /// <param name="promo">
        /// The promo.
        /// </param>
        public static void AjouterPromo(string promo)
        {
            var command = conn.CreateCommand();
            command.CommandText = "INSERT INTO classe (Promotion, hashClasse) VALUES ('" + promo + "', '0')";
            var reader = command.ExecuteReader();
            reader.Close();
        }

        /// <summary>
        /// The backup database.
        /// </summary>
        /// <param name="backUpFile">
        /// The back up file.
        /// </param>
        public static void BackupDatabase(string backUpFile = "C:/databackup/database.sql")
        {
            using (conn)
            {
                using (var cmd = new MySqlCommand())
                {
                    using (var mb = new MySqlBackup(cmd))
                    {
                        var fileData = new SaveFileDialog();
                        fileData.Title = @"Exporter la base de données";
                        fileData.DefaultExt = "sql";
                        fileData.Filter = @"Fichier SQL (*.sql)|*.sql";
                        var result = fileData.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            backUpFile = fileData.FileName;
                        }

                        cmd.Connection = conn;
                        mb.ExportToFile(backUpFile);
                        Connect();
                    }
                }
            }
        }

        /// <summary>
        /// The changer login.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="pass">
        /// The pass.
        /// </param>
        /// <param name="ancienlog">
        /// The ancienlog.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ChangerLogin(string login, string pass, string ancienlog)
        {
            var command = conn.CreateCommand();
            command.CommandText = "SELECT " + ColPass + " FROM " + TabUser + " WHERE " + ColLogin + "='" + ancienlog +
                                  "'";

            var pass2 = string.Empty;
            var retour = command.ExecuteReader();

            while (retour.Read())
            {
                pass2 = retour["Password"].ToString();
            }

            retour.Close();

            if ((pass == pass2) && (login != string.Empty))
            {
                var request = string.Format("UPDATE {0} SET {1}='{2}' WHERE {1}='{3}'", TabUser, ColLogin, login, ancienlog);
                var command2 = conn.CreateCommand();
                command2.CommandText = request;
                command2.ExecuteNonQuery();

                MessageBox.Show(@"Changement réussi");
                return login;
            }

            MessageBox.Show(@"Mot de passe actuel incorrect");
            return string.Empty;
        }

        /// <summary>
        /// The changer mdp.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="pass">
        /// The pass.
        /// </param>
        /// <param name="ancienmdp">
        /// The ancienmdp.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ChangerMdp(string login, string pass, string ancienmdp)
        {
            var command = conn.CreateCommand();
            command.CommandText = "SELECT " + ColPass + " FROM " + TabUser + " WHERE " + ColLogin + "='" + login +
                                  "'";

            var pass2 = string.Empty;
            var retour = command.ExecuteReader();

            while (retour.Read())
            {
                pass2 = retour["Password"].ToString(); // mot de passe récuperé de la BDD
            }

            retour.Close();

            if ((ancienmdp == pass2) && (pass != string.Empty))
            {
// on compare le mdp récuperé avec celui tapé dans le champ "ancien mot de passe"
                // on vérifie que le nouveau mdp ne soit pas vide
                var request = "UPDATE " + TabUser + " SET " + ColPass + "='" + pass + "' WHERE " + ColLogin + "='" +
                              login + "' AND " + ColPass + "='" + ancienmdp + "'";
                var command2 = conn.CreateCommand();
                command2.CommandText = request;
                command2.ExecuteNonQuery();

                MessageBox.Show(@"Changement réussi");
                return 1;
            }

            MessageBox.Show(@"Ancien mot de passe incorrect");
            return 0;
        }

        /// <summary>
        /// The connect.
        /// </summary>
        /// <param name="ip">
        /// The ip.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="pass">
        /// The pass.
        /// </param>
        /// <param name="database">
        /// The database.
        /// </param>
        public static void Connect(string ip = "localhost", string login = "root", string pass = "", string database = "mydb")
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

            conn = new MySqlConnection(builder.ToString());
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// The cp max is notin note.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static List<string> CpMaxIsNotinNote()
        {
            var listToRemoveCp = new List<string>();
            var command = conn.CreateCommand();
            command.CommandText =
                "SELECT DISTINCT competence.idCompetence FROM competence LEFT JOIN note ON competence.idCompetence = note.idCompetence WHERE note.idCompetence IS NULL";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                listToRemoveCp.Add(reader["idCompetence"].ToString());
            }

            reader.Close();

            return listToRemoveCp;
        }

        /// <summary>
        /// The c ps new in note.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static List<string> CPsNewInNote()
        {
            var listnewCp = new List<string>();
            var command = conn.CreateCommand();
            command.CommandText =
                "SELECT DISTINCT note.idCompetence FROM note LEFT JOIN competence ON note.idCompetence = competence.idCompetence WHERE competence.idCompetence IS NULL";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                listnewCp.Add(reader["idCompetence"].ToString());
            }

            reader.Close();

            return listnewCp;
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int Delete(string user)
        {
            var command = conn.CreateCommand();
            command.CommandText = "DELETE FROM " + TabUser + " WHERE " + ColLogin + "='" + user + "'";
            var retour = command.ExecuteReader();

            if (retour.Read())
            {
// si Erreur il y a
                retour.Close();
                return -1;
            }

            retour.Close();
            return 1;
        }

        /// <summary>
        /// The delete elv.
        /// </summary>
        /// <param name="eleve">
        /// The eleve.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int DeleteElv(string eleve)
        {
            var eleve2 = eleve.Split(' ');
            MessageBox.Show(eleve2[0] + @"   " + eleve2[1]);
            var command = conn.CreateCommand();
            command.CommandText = "DELETE FROM " + TabEleve + " WHERE Nom ='" + eleve2[0] + "'AND Prenom='" + eleve2[1] +
                                  "'";
            var retour = command.ExecuteReader();

            if (retour.Read())
            {
// si Erreur il y a
                retour.Close();
                return -1;
            }

            retour.Close();
            return 1;
        }

        /// <summary>
        /// The delete promo.
        /// </summary>
        /// <param name="promo">
        /// The promo.
        /// </param>
        public static void DeletePromo(string promo)
        {
            MessageBox.Show(string.Format("DELETE FROM {0} WHERE {1} = {2}", TabClasse, ColPromo, promo));
            ("DELETE FROM " + TabClasse + " WHERE " + ColPromo + " = " + promo).SimpleRequest();
        }

        /// <summary>
        /// The delete tp.
        /// </summary>
        /// <param name="hashes">
        /// The hashes.
        /// </param>
        public static void DeleteTp(string hashes)
        {
            ("DELETE FROM " + TabTp + " WHERE hashTp = " + hashes).SimpleRequest();
        }

        /// <summary>
        /// The del request.
        /// </summary>
        /// <param name="table">
        /// The table.
        /// </param>
        /// <param name="whereClause">
        /// The where_clause.
        /// </param>
        public static void DelRequest(string table, Tuple<string, string>[] whereClause)
        {
            var request = "DELETE FROM " + table + " WHERE ";

            var secondLine = false;
            foreach (var a in whereClause)
            {
                if (secondLine)
                {
                    request += " AND ";
                }

                request += a.Item1 + "='" + a.Item2 + "'";
                secondLine = true;
            }

            var command = conn.CreateCommand();
            command.CommandText = request;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// The get hash list.
        /// </summary>
        /// <param name="promo">
        /// The promo.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static List<string> GetHashList(string promo)
        {
            var a = new List<string>();
            var command = conn.CreateCommand();

            command.CommandText = "SELECT hashTp FROM " + TabClasse + " NATURAL JOIN " + TabEleve + " NATURAL JOIN " +
                                  TabTp + " WHERE " + ColPromo + " = " + promo;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                a.Add(reader["hashTp"].ToString());
            }

            reader.Close();
            return a;
        }

        /// <summary>
        /// The getid classe.
        /// </summary>
        /// <param name="promo">
        /// The promo.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetidClasse(string promo)
        {
            var idclasse = string.Empty;
            var command = conn.CreateCommand();
            command.CommandText = "SELECT idClasse FROM classe WHERE Promotion ='" + promo + "'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                idclasse = reader["idClasse"].ToString();
            }

            var id = idclasse;

// MessageBox.Show(id);
            reader.Close();

// var Location = 1;
            if (id == string.Empty)
            {
                ("INSERT INTO classe (Promotion,Location) VALUES ('" + promo + "')").SimpleRequest();

                var command2 = conn.CreateCommand();
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

        /// <summary>
        /// The get id eleve from name.
        /// </summary>
        /// <param name="nom">
        /// The nom.
        /// </param>
        /// <param name="prenom">
        /// The prenom.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetIdEleveFromName(string nom, string prenom)
        {
            return GetListRequest(TabEleve, new[] { ColIdeleve }, "Nom = \"" + nom + "\" AND Prenom = \"" + prenom + "\"").FirstOrDefault();
        }

        /// <summary>
        /// The get last pdf id.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetLastPdfId()
        {
            var retour = string.Empty;
            var req = "SELECT max(idPdf) FROM tp";
            var command = conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                retour = r["max(idPdf)"].ToString();
            }

            r.Close();
            return retour;
        }

        /// <summary>
        /// The get list request.
        /// </summary>
        /// <param name="table">
        /// The table.
        /// </param>
        /// <param name="columns">
        /// The columns.
        /// </param>
        /// <param name="additionalWhereClause">
        /// The additional_where_clause.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IEnumerable<string> GetListRequest(string table, string[] columns, string additionalWhereClause = "1")
        {
            var retour = new List<string>();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM " + table + " WHERE " + additionalWhereClause;
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

        /// <summary>
        /// The get distinct request.
        /// </summary>
        /// <param name="table">
        /// The table.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="columns">
        /// The columns.
        /// </param>
        /// <param name="additionalWhereClause">
        /// The additional_where_clause.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IEnumerable<string> GetDistinctRequest(
            string table,
            string element,
            string[] columns,
            string additionalWhereClause = "1")
        {
            var retour = new List<string>();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT DISTINCT " + element + " FROM " + table + " WHERE " + additionalWhereClause;
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

        /// <summary>
        /// The get max cp.
        /// </summary>
        /// <param name="idCompetence">
        /// The id competence.
        /// </param>
        /// <returns>
        /// The <see cref="decimal"/>.
        /// </returns>
        public static decimal GetMaxCp(string idCompetence, string promo)
        {
            var idClasse = Database.GetidClasse(promo);
            var comp = string.Empty;
            var command = conn.CreateCommand();
            command.CommandText = "SELECT maxEchelle FROM competence WHERE idCompetence ='" + idCompetence + "' AND idClasse='" + idClasse +"'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                comp = reader["maxEchelle"].ToString();
            }

            var comp2 = comp;

// MessageBox.Show(id);
            reader.Close();
            try
            {
                return decimal.Parse(comp2);
            }
            catch
            {
                MessageBox.Show("Maximum de la promotion non géré par la BDD !");
                return 0;
            }
        }

        /// <summary>
        /// The getpromo.
        /// </summary>
        /// <param name="idClasse">
        /// The id classe.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Getpromo(string idClasse)
        {
            var promo = string.Empty;
            var command = conn.CreateCommand();
            command.CommandText = "SELECT Promotion FROM classe WHERE idClasse ='" + idClasse + "'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                promo = reader["Promotion"].ToString();
            }

            var promo2 = promo;

// MessageBox.Show(id);
            reader.Close();

            return promo2;
        }

        /// <summary>
        /// The get web classe request.
        /// </summary>
        /// <param name="idPromo">
        /// The id promo.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static List<Tuple<string, float>> GetWebClasseRequest(string idPromo = "2017")
        {
            var retour = new List<Tuple<string, float>>();
            var req =
                "SELECT " + ColIdskill + ", SUM(" + ColNote + ")/count(DISTINCT " + ColIdeleve + ") AS somme FROM " +
                TabClasse + " NATURAL JOIN " + TabEleve + " NATURAL JOIN " + TabTp + " NATURAL JOIN " + ColNote +
                " WHERE " + ColPromo + "=" + idPromo + " GROUP BY " + ColIdskill;
            var command = conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                retour.Add(new Tuple<string, float>(r[ColIdskill].ToString(), float.Parse(r["somme"].ToString())));
            }

            r.Close();
            return retour;
        }

        /// <summary>
        /// The get web max.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static List<Tuple<string, float>> GetWebMax()
        {
            var retour = new List<Tuple<string, float>>();
            var req = "SELECT " + ColIdskill + ", maxEchelle FROM competence";
            var command = conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                retour.Add(new Tuple<string, float>(r[ColIdskill].ToString(), float.Parse(r["maxEchelle"].ToString())));
            }

            r.Close();

            return retour;
        }

        /// <summary>
        /// The get web request.
        /// </summary>
        /// <param name="checkBox">
        /// The check box.
        /// </param>
        /// <param name="idEleve">
        /// The id eleve.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static List<Tuple<string, float>> GetWebRequest(CheckBox checkBox, string idEleve = "2")
        {
            var req = string.Empty;
            var retour = new List<Tuple<string, float>>();
            if (checkBox.Checked == false)
            {
                req =
                    "SELECT " + ColIdskill + ", SUM(" + ColNote + ") FROM " + TabEleve + " NATURAL JOIN " + TabTp +
                    " NATURAL JOIN " + ColNote + " WHERE " + ColIdeleve + " = '" + idEleve + "' GROUP BY " +
                    ColIdskill;
            }
            else if (checkBox.Checked == false)
            {
                req =
                    "SELECT " + ColIdskill + ", SUM(" + ColNote + ") FROM " + TabEleve + " NATURAL JOIN " + TabTp +
                    " NATURAL JOIN " + ColNote + " WHERE " + ColIdeleve + " = '" + idEleve + "' GROUP BY " +
                    ColIdskill;
            }

            var command = conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            while (r.Read())
            {
                retour.Add(
                    new Tuple<string, float>(r[ColIdskill].ToString(), float.Parse(r["SUM(" + ColNote + ")"].ToString())));
            }

            r.Close();
            return retour;
        }

        /// <summary>
        /// The get wtf request.
        /// </summary>
        /// <param name="idEleve">
        /// The id eleve.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static List<Tuple<string, float, int>> GetWtfRequest(string idEleve = "2")
        {
            var req =
                "SELECT " + ColIdskill + ",COUNT(" + ColNote + ") AS quantite, ((SUM(" + ColNote + "/" + ColMaxnote +
                ")/COUNT(" + ColNote + "))*100) AS moyenne FROM " + TabEleve + " NATURAL JOIN " + TabTp +
                " NATURAL JOIN " + ColNote + " WHERE " + ColIdeleve + " = " + idEleve + " GROUP BY " + ColIdskill;
            var command = conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            var a = new List<Tuple<string, float, int>>();

            while (r.Read())
            {
                a.Add(new Tuple<string, float, int>(r[ColIdskill].ToString(), float.Parse(r["moyenne"].ToString()), int.Parse(r["quantite"].ToString())));
            }

            r.Close();
            return a;
        }

        /// <summary>
        /// The get courbe request.
        /// </summary>
        /// <param name="idEleve">
        /// The id eleve.
        /// </param>
        /// <param name="idComp">
        /// The id comp.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IOrderedEnumerable<Tuple<float, DateTime>> GetCourbeRequest(string idEleve, string idComp = "C1.1")
        {
            var req = "SELECT date, note FROM tp NATURAL JOIN note WHERE idEleve='" + idEleve + "' AND idCompetence ='" +
                      idComp + "'";
            var dateRaw = "";
            DateTime dateBash;
            var command = conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            var a = new List<Tuple<float, DateTime>>();

            while (r.Read())
            {
                dateRaw = r["date"].ToString();
                dateBash = DateTime.Parse(dateRaw);
                var strFloatValue = r["note"].ToString();
                strFloatValue = strFloatValue.Replace(" ", "");
                float itsgg = float.Parse(strFloatValue, CultureInfo.InvariantCulture);
                a.Add(new Tuple<float, DateTime>(itsgg, dateBash));
            }

            var x = a.OrderBy(b => b.Item2);
            r.Close();

            return x;
        }

        public static IOrderedEnumerable<Tuple<float, DateTime>> GetCourbeClasseRequest(string idComp = "C1.1", string idPromo = "2016")
        {
            idPromo = Database.GetidClasse(idPromo);
            var req = "SELECT date, note FROM tp NATURAL JOIN note NATURAL JOIN eleve WHERE idCompetence ='" +
                      idComp + "' AND idClasse='" + idPromo + "' ";
            var dateRaw = "";
            DateTime dateBash;
            var command = conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            var a = new List<Tuple<float, DateTime>>();

            while (r.Read())
            {
                dateRaw = r["date"].ToString();
                dateBash = DateTime.Parse(dateRaw);
                var strFloatValue = r["note"].ToString();
                strFloatValue = strFloatValue.Replace(" ", "");
                float itsgg = float.Parse(strFloatValue, CultureInfo.InvariantCulture);
                a.Add(new Tuple<float, DateTime>(itsgg, dateBash));
            }

            var x = a.OrderBy(b => b.Item2);
            r.Close();

            return x;
        }


        /// <summary>
        /// The get wth request.
        /// </summary>
        /// <param name="idPromo">
        /// The id promo.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<Tuple<string, float, int>> GetWthRequest(string idPromo)
        {
            // var req2 =
            // "SELECT " + COL_IDSKILL + ",COUNT(" + COL_NOTE + ") AS quantite, ((SUM(" + COL_NOTE + "/" + COL_MAXNOTE + ")/COUNT(" + COL_NOTE + "))*100) AS moyenne FROM " + TAB_ELEVE + " NATURAL JOIN " + TAB_TP + " NATURAL JOIN " + COL_NOTE + " WHERE " + COL_PROMO + " = " + idEleve + " GROUP BY " + COL_IDSKILL;
            var req =
                "SELECT " + ColIdskill + ", count(*) AS nbTp, ((SUM(" + ColNote + "/" + ColMaxnote + ")/COUNT(" +
                ColNote + "))*100) AS moyenne FROM " + TabClasse + " NATURAL JOIN " + TabEleve + " NATURAL JOIN " +
                TabTp + " NATURAL JOIN " + ColNote + " WHERE " + ColPromo + "=" + idPromo + " GROUP BY " +
                ColIdskill;

            var command = conn.CreateCommand();
            command.CommandText = req;
            var r = command.ExecuteReader();

            var a = new List<Tuple<string, float, int>>();

            while (r.Read())
            {
                a.Add(new Tuple<string, float, int>(r[ColIdskill].ToString(), float.Parse(r["moyenne"].ToString()), int.Parse(r["nbTp"].ToString())));
            }

            r.Close();
            return a;
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="pass">
        /// The pass.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int Login(string login, string pass)
        {
            var command = conn.CreateCommand();
            command.CommandText = "SELECT " + ColLogin + "," + ColPass + "," + ColAdmin + " FROM " + TabUser +
                                  " WHERE " + ColLogin + "='" + login + "' AND " + ColPass + "='" + pass + "'";
            var retour = command.ExecuteReader();

            if (login.Equals("admin") && pass.Equals("29042016"))
            {
                retour.Close();
                return 1; // Compte admin en dur pour pouvoir débug si BDD plante
            }

            if (!retour.Read())
            {
                retour.Close();
                return -1;
            }

            var r = retour[ColAdmin].ToString().Equals("True") ? 1 : 0;
            retour.Close();
            return r;
        }

        /// <summary>
        /// The promo eleve.
        /// </summary>
        /// <param name="promo">
        /// The promo.
        /// </param>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public static string[] PromoEleve(string promo)
        {
            var retour = new string[1000];
            var command = conn.CreateCommand();
            var idClasse = string.Empty;
            command.CommandText = "SELECT idClasse FROM classe WHERE Promotion ='" + promo + "'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                idClasse = reader["idClasse"].ToString();
            }

            var id = idClasse;

// MessageBox.Show(id);
            reader.Close();
            var command2 = conn.CreateCommand();
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

        /// <summary>
        /// The recup eleve avec promo.
        /// </summary>
        /// <param name="promo">
        /// The promo.
        /// </param>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public static string[] RecupEleveAvecPromo(string promo)
        {
            var retour = new string[1000];
            var i = 0;
            var command = conn.CreateCommand();
            command.CommandText = "SELECT Nom, Prenom FROM `classe` NATURAL JOIN eleve WHERE Promotion = " + promo;
            var r = command.ExecuteReader();
            while (r.Read())
            {
                var concatenate = r["Prenom"] + " " + r["Nom"];

// MessageBox.Show(concatenate);
                retour[i] = concatenate;
                i = i + 1;
            }

            r.Close();
            return retour;
        }

        /// <summary>
        /// The remove cp from web.
        /// </summary>
        /// <param name="listCP">
        /// The list cp.
        /// </param>
        /// <param name="prenom">
        /// The prenom.
        /// </param>
        /// <param name="nom">
        /// The nom.
        /// </param>
        public static void removeCPFromWeb(List<Tuple<string, float>> listCP, string prenom, string nom)
        {
            var command = conn.CreateCommand();
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
                        GetidClasse(Program.ac.graphic.promotionSelected) + "' AND idCompetence='" + idCPMax.Item1 + "'";
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

        /// <summary>
        /// The remove cp max.
        /// </summary>
        /// <param name="listCP">
        /// The list cp.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int removeCPMax(List<string> listCP)
        {
            if (listCP.Count != 0)
            {
                var command = conn.CreateCommand();
                foreach (var idCP in listCP)
                {
                    ("DELETE FROM `mydb`.`competence` WHERE `competence`.`idCompetence` = \'" + idCP + "'")
                        .SimpleRequest();
                }

                return 0;
            }

            return 1;
        }

        /// <summary>
        /// The restore database.
        /// </summary>
        /// <param name="restoredFile">
        /// The restored file.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int RestoreDatabase(string restoredFile = "C:/databackup/database.sql")
        {
            using (conn)
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

                        cmd.Connection = conn;
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

        /// <summary>
        /// The set hash.
        /// </summary>
        /// <param name="nomColonne">
        /// The nom colonne.
        /// </param>
        /// <param name="hash">
        /// The hash.
        /// </param>
        /// <param name="condition">
        /// The condition.
        /// </param>
        public static void SetHash(string nomColonne, string hash, string condition)
        {
            ("UPDATE " + TabClasse + " SET" + nomColonne + " = '" + hash + "' WHERE " + nomColonne + "=" + condition)
                .SimpleRequest();
        }

        /// <summary>
        /// The set max cp.
        /// </summary>
        /// <param name="idCompetence">
        /// The id competence.
        /// </param>
        /// <param name="maxCP">
        /// The max cp.
        /// </param>
        public static void setMaxCP(string idCompetence, decimal maxCP)
        {
            var command = conn.CreateCommand();
            command.CommandText = "UPDATE competence SET maxEchelle = " + maxCP + " WHERE idCompetence = '" +
                                  idCompetence + "'";
            var reader = command.ExecuteReader();

// MessageBox.Show(id);
            reader.Close();
        }

        /// <summary>
        /// The simple request.
        /// </summary>
        /// <param name="r">
        /// The r.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool SimpleRequest(this string r)
        {
            try
            {
                var neweleve = new MySqlCommand(r, conn) {CommandText = r};
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
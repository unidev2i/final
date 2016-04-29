using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WL;

namespace WindowsFormsApplication2
{
    public static class ImportTp
    {
        private static readonly string RootFolder;
        private static string _errMssg = string.Empty;
        private static string _logMssg = string.Empty;

        static ImportTp()
        {
            RootFolder = WindowsFormsApplication2.Properties.Settings.Default.repoPath;
        }

        private static List<string> CheckPromo()
        {
            var request0 = Directory.GetDirectories(RootFolder).Aggregate(string.Empty, (current, a) => current + ("\""+Crypt.CreateMd5ForFolder(a) + "\","));

            var retour1 = Database.GetListRequest("classe", new[] { "promotion" }/*,
                String.Format("`hashClasse` NOT IN ({0}0)", request0)*/);
            var retour2 = retour1.ToList();

            return retour2;
        }

        public static void Go()
        {
            _logMssg += "Traitement des dossiers démarré." + Environment.NewLine;

            if (Directory.GetFiles(RootFolder).Length != 0)
            {
                _errMssg +=
                    "<li>Plusieurs fichiers à la racine du répertoire ont étés détectés. Ils ne seront pas traités.</li>" +
                    Environment.NewLine;
            }

            #region firstCheck
            // First check
            int check = Directory.GetDirectories(RootFolder).SelectMany(Directory.GetFiles).Count();
            if (check == 0)
            {
                var dialogResult = MessageBox.Show(
                        @"Le dossier est vide, toute la base de données sera effacée si vous continuez. Continuer ? ", @"ATTENTION !", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                switch (dialogResult)
                {
                    case DialogResult.None:
                        break;
                    case DialogResult.Yes:
                        "TRUNCATE `classe`".SimpleRequest();
                        "TRUNCATE `competence`".SimpleRequest();
                        "TRUNCATE `eleve`".SimpleRequest();
                        "TRUNCATE `note`".SimpleRequest();
                        "TRUNCATE `tp`".SimpleRequest();
                        "TRUNCATE `user`".SimpleRequest();
                        return;
                    case DialogResult.No:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            #endregion


            var cp = CheckPromo();
            for (int i = 0; i < cp.Count; i++)
            {
                cp[i] = cp[i].Substring(0, cp[i].Length - 1);
            }

            Program.ac.graphic.progressBar1.Invoke(
                (MethodInvoker)(() => Program.ac.graphic.progressBar1.Visible = true));
            Program.ac.graphic.progressBar1.Invoke(
                (MethodInvoker) (() => Program.ac.graphic.progressBar1.Maximum = cp.Count));

            // y = yes n = no t = traité
            int yt = 0;
            int nt = 0;

            foreach (var dir in Directory.GetDirectories(RootFolder))
            {
                var temp = dir.Split('\\')[dir.Split('\\').Length - 1];
                if (!cp.Contains(temp) && Directory.GetFiles(dir).Length != 0)
                {
                    Database.ajouterPromo(dir.Split('\\')[dir.Split('\\').Length - 1]);
                }
            }

            cp = CheckPromo();

            foreach (var dir in cp.Select(a => RootFolder + "\\" + a.Remove(a.Length - 1, 1)))
            {
                var x = Database.GetListRequest("note", new[] {"Promotion"});

                if (!Directory.Exists(dir) && Directory.GetFiles(dir).Length != 0)
                {
                    var dialogResult = MessageBox.Show(String.Format("Le répertoire \"{0}\" n'existe pas. Voulez-vous supprimer les données de la base de données ?", dir), @"ATTENTION !", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    switch (dialogResult)
                    {
                        case DialogResult.Yes:
                            Database.DeletePromo(dir.Split('\\')[dir.Split('\\').Length - 1]);
                            break;
                        case DialogResult.No:
                            _errMssg += "Le dossier " + dir + " n'existe pas, mais il n'a pas été supprimé de la base de données." + Environment.NewLine;
                            break;
                    }
                    goto fin;
                }

                // Récupérer la liste des hash en fonction du nom du dossier (nom dossier = id compétence)
                var a = Database.GetHashList(dir.Split('\\')[dir.Split('\\').Length - 1]);


                foreach (var file in Directory.GetFiles(dir))
                {
                    var cr = Crypt.Md5(file);
                    if (a.Contains(cr))
                    {
                        a.Remove(cr);
                        nt++;
                    }
                    else
                    {
                        TraiterFichier(file);
                        yt++;
                    }
                }

                foreach (var s in a)
                {
                    // Supprimer de la bdd les tp de la promo "a" qui ne sont plus dans le répertoire 
                    yt++;
                    Database.DeleteTp(s);
                }

                fin:
                try
                {
                    Program.ac.graphic.progressBar1.Invoke(
                        (MethodInvoker) (() => Program.ac.graphic.progressBar1.Value++));
                }
                catch
                {
                    // ignored
                }

                // TODO : Update this fcking hash

            }
            try
            {
                Program.ac.graphic.progressBar1.Invoke(
                    (MethodInvoker) (() => Program.ac.graphic.progressBar1.Visible = false));
            }
            catch
            {
                // ignored
            }

            if (!_errMssg.Equals(""))
            {
                MessageBox.Show(@"Terminé avec des erreurs : " + Environment.NewLine + _errMssg);
            }
            Program.ac.graphic.LBL_InfoAjoutTp.Invoke((MethodInvoker) (() => Program.ac.graphic.LBL_InfoAjoutTp.Text += Environment.NewLine + @"Traités : " + yt.ToString() + @"   Ignorés : " + nt.ToString()));
            Program.ac.graphic.LBL_InfoAjoutTp.Invoke((MethodInvoker) (() => Program.ac.graphic.LBL_InfoAjoutTp.Visible = true));
            System.Threading.Thread.Sleep(3000);
            Program.ac.graphic.LBL_InfoAjoutTp.Invoke((MethodInvoker) (() => Program.ac.graphic.LBL_InfoAjoutTp.Visible = false));
            //ShowLog();

            Database.addCPMax(Database.CPsNewInNote());
        }

        private static void TraiterFichier(string file)
        {
            if (!file.Contains(".pdf"))
            {
                _errMssg += file + " : Le fichier est au mauvais format. Attendu : pdf" + Environment.NewLine;
            }
            var infos = GetInfos(file);
            if (infos == null) return;
            var value = GetValue(file);

            var idEleve = Database.GetIdEleveFromName(infos.Item1, infos.Item2);

            if (idEleve == null)
            {
                Database.AjouteEleve(infos.Item1, infos.Item2, file.Split('\\')[file.Split('\\').Length - 2]);
                idEleve = Database.GetIdEleveFromName(infos.Item1, infos.Item2);
            }

            // ETAPE 1 : Créer le TP
            var mdr = Program.ac.graphic.login;
            Database.AddTp(infos.Item3, idEleve, mdr, Crypt.Md5(file));

            // ETAPE 1' : Recup id tp
            var idPdf = Database.GetLastPdfId();

            // ETAPE 2 : Insérer note
            foreach (var a in value)
            {
                Database.AddNote(idPdf, a.Item1, a.Item2, a.Item3);
            }
        }

        private static Tuple<string, string, string> GetInfos(string file)
        {
            try
            {
                // Retirer le chemin
                var pre = file.Split('\\');
                var deux = file.Split('\\')[pre.Length - 1];

                // Separer les infos
                var a = deux.Split(new[] {'_'}, StringSplitOptions.RemoveEmptyEntries);

                // Retirer le ".pdf" à la fin
                return new Tuple<string, string, string>(a[0], a[1], a[2].Remove(a[2].Length - 4, 4));
            }
            catch (Exception)
            {
                //MessageBox.Show(
                //    $@"Mauvais type de fichier. Veuillez vérifier qu'il est sous la forme{Environment.NewLine}NOM_PRENOM_TPXX.pdf");
                _errMssg += file + " : Nom du fichier non reconnu. Attendu : NOM_PRENOM_TPXX.pdf" + Environment.NewLine;
                return null;
            }
        }

        public static void ShowLog()
        {
            var a = new ImportTpInfo();
            a.ShowDialog();

            string message = "";
            if (_errMssg != string.Empty)
            {
                message += "<p style='color:red; font-size:50px align:center'>Liste d'erreurs</p>";
            }
        }

        private static List<Tuple<string, string, string>> GetValue(string file)
        {
            if (!file.Contains(".pdf"))
                return null;

            var b = file;
            var a = new pdfHandler(ref b);
            var c = (string) a.readPDF();

            const string strRegex = @"C[0-9].[0-9]";
            var myRegex = new Regex(strRegex, RegexOptions.None);
            const string strRegex2 = @"[0-9]{1,2}\.{0,1}[0-9]{0,3}\s{0,2}\/\s{0,2}[0-9]{1,2}\.{0,1}[0-9]{0,1}\s";
            var myRegex2 = new Regex(strRegex2, RegexOptions.None);

            var maxMark = new List<string>();

            var sortie = c;

            var skills = (from Match k in myRegex.Matches(sortie) select k.Value).ToList();

            var mark = (from Match l in myRegex2.Matches(sortie) select l.Value).ToList();

            var skills2 = new List<string>();

            skills2 = skills.Distinct().ToList();
            skills = skills2;
            GC.Collect();

            var tempReturn = new List<Tuple<string, string, string>>();

            for (var index = 0; index < skills.Count; index++)
            {
                var m = mark[index];

                mark[index] = mark[index].Split('/')[0];
                maxMark.Add(m.Split('/')[1]);
            }

            for (var index = 0; index < skills.Count; index++)
            {
                var z = mark[index] + "->" + maxMark[index];
                tempReturn.Add(new Tuple<string, string, string>(skills[index], mark[index], maxMark[index]));
            }

            a = null;
            GC.Collect();

            return tempReturn;
        }
    }

    public static class MyString
    {
        public static string RemoveChar(this string x, string b)
        {
            var a = x;
            while (a.Contains(b))
            {
                if (a.Length > a.IndexOf(b, StringComparison.Ordinal))
                    a.Remove(a.IndexOf(b, StringComparison.Ordinal));
            }
            return a;
        }

        public static string Md5(this string x)
        {

            // byte array representation of that string
            byte[] encodedPassword = new UTF8Encoding().GetBytes(x);

            // need MD5 to calculate the hash
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            // string representation (similar to UNIX format)
            return BitConverter.ToString(hash)
               // without dashes
               .Replace("-", string.Empty)
               // make lowercase
               .ToLower();
        }
    }
}
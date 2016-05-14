// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportTp.cs" company="ig2i">
//   ARR
// </copyright>
// <summary>
//   Defines the ImportTp type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WindowsFormsApplication2
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;
    using Properties;
    using WL;

    /// <summary>
    /// The import TP class, who allows to import and update TPs
    /// </summary>
    public static class ImportTp
    {
        #region Private Fields

        /// <summary>
        /// The root folder for TPs.
        /// </summary>
        private static readonly string RootFolder;

        /// <summary>
        /// The _err mssg.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private static string errMssg = string.Empty;

        /// <summary>
        /// The _log mssg.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once NotAccessedField.Local
        private static string logMssg = string.Empty;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes static members of the <see cref="ImportTp"/> class.
        /// </summary>
        static ImportTp()
        {
            RootFolder = Settings.Default.repoPath;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Execute treatements
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// folder contains only empty folders
        /// </exception>
        // ReSharper disable once CyclomaticComplexity
        public static void Go()
        {
            logMssg += "Traitement des dossiers démarré." + Environment.NewLine;

            if (Directory.GetFiles(RootFolder).Length != 0)
            {
                errMssg +=
                    "<li>Plusieurs fichiers à la racine du répertoire ont étés détectés. Ils ne seront pas traités.</li>" +
                    Environment.NewLine;
            }

            // First check
            var check = Directory.GetDirectories(RootFolder).SelectMany(Directory.GetFiles).Count();
            if (check == 0)
            {
                var dialogResult = MessageBox.Show(
                    @"Le dossier est vide, toute la base de données sera effacée si vous continuez. Continuer ? ", @"ATTENTION !", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                switch (dialogResult)
                {
                    case DialogResult.None:
                        break;

                    case DialogResult.Yes:
                        "DELETE FROM `tp` WHERE 1".SimpleRequest();
                        "DELETE FROM `eleve` WHERE 1".SimpleRequest();
                        "DELETE FROM `competence` WHERE 1".SimpleRequest();
                        "DELETE FROM `classe` WHERE 1".SimpleRequest();
                        Thread.CurrentThread.Abort();
                        return;

                    case DialogResult.No:
                        return;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            "DELETE FROM `tp` WHERE 1".SimpleRequest();
            "DELETE FROM `eleve` WHERE 1".SimpleRequest();
            "DELETE FROM `competence` WHERE 1".SimpleRequest();
            "DELETE FROM `classe` WHERE 1".SimpleRequest();
            var cp = CheckPromo();
            for (var i = 0; i < cp.Count; i++)
            {
                cp[i] = cp[i].Substring(0, cp[i].Length - 1);
            }

            Program.ac.graphic.progressBar1.Invoke(
                (MethodInvoker)(() => Program.ac.graphic.progressBar1.Value = 0));
            Program.ac.graphic.progressBar1.Invoke(
                (MethodInvoker)(() => Program.ac.graphic.progressBar1.Visible = true));
            var cp1 = cp;
            Program.ac.graphic.progressBar1.Invoke(
                (MethodInvoker)(() => Program.ac.graphic.progressBar1.Maximum = cp1.Count));

            // y = yes n = no t = traité
            var yt = 0;
            var nt = 0;
            var dt = 0;

            var cp2 = cp;
            foreach (var dir in from dir in Directory.GetDirectories(RootFolder) let temp = dir.Split('\\')[dir.Split('\\').Length - 1] where !cp2.Contains(temp) && (Directory.GetFiles(dir).Length != 0) select dir)
            {
                Database.ajouterPromo(dir.Split('\\')[dir.Split('\\').Length - 1]);
            }

            cp = CheckPromo();

            foreach (var dir in cp.Select(a => RootFolder + "\\" + a.Remove(a.Length - 1, 1)))
            {
                // var x = Database.GetListRequest("note", new[] { "Promotion" });
                if (!Directory.Exists(dir) && (Directory.GetFiles(dir).Length != 0))
                {
                    var dialogResult =
                        MessageBox.Show(
                            string.Format(
                                "Le répertoire \"{0}\" n'existe pas. Voulez-vous supprimer les données de la base de données ?",
                                dir),
                            @"ATTENTION !",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                    switch (dialogResult)
                    {
                        case DialogResult.Yes:
                            Database.DeletePromo(dir.Split('\\')[dir.Split('\\').Length - 1]);
                            break;

                        case DialogResult.No:
                            errMssg += "Le dossier " + dir +
                                        " n'existe pas, mais il n'a pas été supprimé de la base de données." +
                                        Environment.NewLine;
                            break;
                        case DialogResult.None:
                            break;
                        case DialogResult.OK:
                            break;
                        case DialogResult.Cancel:
                            break;
                        case DialogResult.Abort:
                            break;
                        case DialogResult.Retry:
                            break;
                        case DialogResult.Ignore:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
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
                    dt++;
                    Database.DeleteTp(s);
                }

                fin:
                try
                {
                    Program.ac.graphic.progressBar1.Invoke((MethodInvoker)(() => Program.ac.graphic.progressBar1.Value++));
                }
                catch
                {
                    // ignored
                }

                // If you want update hashs do it here
            }

            try
            {
                Program.ac.graphic.progressBar1.Invoke((MethodInvoker)(() => Program.ac.graphic.progressBar1.Visible = false));
            }
            catch
            {
                // ignored
            }

            if (!errMssg.Equals(string.Empty))
            {
                MessageBox.Show(@"Terminé avec des erreurs : " + Environment.NewLine + errMssg);
            }

            Program.ac.graphic.LBL_InfoAjoutTp.Invoke(
                (MethodInvoker)
                    (() =>
                        Program.ac.graphic.LBL_InfoAjoutTp.Text +=
                            Environment.NewLine + @"Traités : " + yt + @"   Ignorés : " + nt + @"   Supprimés : " + dt));
            Program.ac.graphic.LBL_InfoAjoutTp.Invoke(
                (MethodInvoker)(() => Program.ac.graphic.LBL_InfoAjoutTp.Visible = true));
            Thread.Sleep(3000);
            Program.ac.graphic.LBL_InfoAjoutTp.Invoke(
                (MethodInvoker)(() => Program.ac.graphic.LBL_InfoAjoutTp.Visible = false));

            // ShowLog();
            Database.addCPMax(Database.CPsNewInNote());
            Database.removeCPMax(Database.CPMaxIsNotinNote());
        }

        /// <summary>
        /// Show the log at the end (NOT USED ACTUALLY)
        /// </summary>
        public static void ShowLog()
        {
            var a = new ImportTpInfo();
            a.ShowDialog();

            // ReSharper disable once NotAccessedVariable
            var message = string.Empty;
            if (errMssg != string.Empty)
            {
                // ReSharper disable once RedundantAssignment
                message += "<p style='color:red; font-size:50px align:center'>Liste d'erreurs</p>";
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Check promo
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     of promos
        /// </returns>
        private static List<string> CheckPromo()
        {
            // ReSharper disable once UnusedVariable
            var request0 = Directory.GetDirectories(RootFolder).Aggregate(string.Empty, (current, a) => current + "\"" + Crypt.CreateMd5ForFolder(a) + "\",");

            var retour1 = Database.GetListRequest("classe", new[] { "promotion" } /*,String.Format("`hashClasse` NOT IN ({0}0)", request0)*/);
            var retour2 = retour1.ToList();

            return retour2;
        }

        /// <summary>
        /// The get infos.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <returns>
        /// The <see cref="Tuple"/>.
        /// </returns>
        private static Tuple<string, string, string> GetInfos(string file)
        {
            try
            {
                // Retirer le chemin
                var pre = file.Split('\\');
                var deux = file.Split('\\')[pre.Length - 1];

                // Separer les infos
                var a = deux.Split('_');
                var b = a[0].Split('.');

                // Retirer le ".pdf" à la fin
                return new Tuple<string, string, string>(b[0], b[1], a[1].Remove(a[1].Length - 4, 4));
            }
            catch (Exception)
            {
                // MessageBox.Show(
                //    $@"Mauvais type de fichier. Veuillez vérifier qu'il est sous la forme{Environment.NewLine}NOM_PRENOM_TPXX.pdf");
                errMssg += "<li>" + file + " : Nom du fichier non reconnu. Attendu : NOM.PRENOM_NOMDUTP.pdf</li>" + Environment.NewLine;
                return null;
            }
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>
        /// values
        /// </returns>
        private static IEnumerable<Tuple<string, string, string>> GetValue(string file)
        {
            if (!file.Contains(".pdf"))
            {
                return null;
            }

            if (File.Exists("temp.pdf"))
            {
                File.Delete("temp.pdf");
            }

            File.Copy(file, "temp.pdf");

            var b = "temp.pdf";
            var x = Directory.GetCurrentDirectory() + @"\" + b;
            var a = new pdfHandler(ref x);
            var c = (string)a.readPDF();

            const string strRegex = @"C[0-9].[0-9]";
            var myRegex = new Regex(strRegex, RegexOptions.None);
            const string strRegex2 = @"[0-9]{1,2}\.{0,1}[0-9]{0,3}\s{0,2}\/\s{0,2}[0-9]{1,2}\.{0,1}[0-9]{0,1}\s";
            var myRegex2 = new Regex(strRegex2, RegexOptions.None);

            var maxMark = new List<string>();

            var sortie = c;

            var skills = (from Match k in myRegex.Matches(sortie) select k.Value).ToList();

            var mark = (from Match l in myRegex2.Matches(sortie) select l.Value).ToList();

            var skills2 = skills.Distinct().ToList();
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

        /// <summary>
        /// The traiter fichier.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        private static void TraiterFichier(string file)
        {
            if (!file.Contains(".pdf"))
            {
                errMssg += file + " : Le fichier est au mauvais format. Attendu : pdf" + Environment.NewLine;
            }
            ////? -----------------------------------------
            /*
            if (file.IndexOf(".", StringComparison.Ordinal) != file.IndexOf(".pdf", StringComparison.Ordinal))
            {
                var filetempo = file;
                String nomFichier = file.Split('\\')[file.Split('\\').Length - 1];
                String nomFictempo = nomFichier;
                var regex = new Regex(Regex.Escape("."));
                nomFichier = regex.Replace(nomFichier,"_", 1);
                file=file.Replace(nomFictempo, nomFichier);
                File.Move(filetempo,file);
            }
            */

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

        #endregion Private Methods
    }

    public static class MyString
    {
        #region Public Methods

        public static string Md5(this string x)
        {
            // byte array representation of that string
            var encodedPassword = new UTF8Encoding().GetBytes(x);

            // need MD5 to calculate the hash
            var hash = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            // string representation (similar to UNIX format)
            return BitConverter.ToString(hash)
                // without dashes
                .Replace("-", string.Empty)
                // make lowercase
                .ToLower();
        }

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

        #endregion Public Methods
    }
}
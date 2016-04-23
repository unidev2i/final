using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WL;

namespace WindowsFormsApplication2
{
    public static class ImportTp
    {
        private static readonly string _rootFolder;
        private static string _errMssg = string.Empty;

        static ImportTp()
        {
            _rootFolder = WindowsFormsApplication2.Properties.Settings.Default.repoPath;
        }

        private static List<string> CheckPromo()
        {
            var request0 = Directory.GetDirectories(_rootFolder).Aggregate(string.Empty, (current, a) => current + ("\""+Crypt.CreateMd5ForFolder(a) + "\","));

            var retour1 = Database.GetListRequest("classe", new[] { "promotion" },
                String.Format("`hash` NOT IN ({0}0)", request0));
            var retour2 = retour1.ToList();

            return retour2;
        }

        public static void Go()
        {
            var cp = CheckPromo();
            Program.ac.graphic.setBarmax(cp.Count);
            foreach (var a in cp)
            {
                // MessageBox.Show(a);
                // pour tous les dossiers qui ont étés modifiés

                var dir = _rootFolder + "\\" + a.Remove(a.Length-1, 1);
                // var dic = "C:\\Users\\geekg\\Desktop\\PDF\\2017";

                //foreach (var file in Directory.GetFiles("C:\\Users\\geekg\\Desktop\\PDF\\2017"))
                if (!Directory.Exists(dir))
                {
                    var dialogResult = MessageBox.Show(
                        String.Format(
                            "Le répertoire \"{0}\" n'existe pas. Voulez-vous supprimer les données de la base de données ?",
                            dir), @"ATTENTION !", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        // TODO : supprimer promo
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        // do nothing
                    }
                }
                foreach (var file in Directory.GetFiles(dir))
                {
                    if (!file.Contains(".pdf"))
                    {
                        _errMssg += file + " : Le fichier est au mauvais format. Attendu : pdf"+Environment.NewLine;
                    }
                    var infos = GetInfos(file);
                    if (infos == null) continue;
                    var value = GetValue(file);
                }
                Program.ac.graphic.setBarval(Program.ac.graphic.getBarValue()+1);
            }
        }

        private static Tuple<string, string, string> GetInfos(string file)
        {
            try
            {
                // Retirer le chemin
                var pre = file.Split('\\');
                var deux = file.Split('\\')[pre.Length-1];

                // Separer les infos
                var a = deux.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

                // Retirer le ".pdf" à la fin
                return new Tuple<string, string, string>(a[0], a[1], a[2].Remove(a[2].Length-4,4));
            }
            catch (Exception)
            {
                //MessageBox.Show(
                //    $@"Mauvais type de fichier. Veuillez vérifier qu'il est sous la forme{Environment.NewLine}NOM_PRENOM_TPXX.pdf");
                _errMssg += file + " : Nom du fichier non reconnu. Attendu : NOM_PRENOM_TPXX.pdf"+Environment.NewLine;
                return null;
            }
        }

        private static List<Tuple<string, string>> GetValue(string file)
        {
            if (!file.Contains(".pdf"))
                return null;

            var b = file;
            var a = new pdfHandler(ref b);
            var c = (string)a.readPDF();

            //MessageBox.Show(c);

            const string strRegex = @"C[0-9].[0-9]";
            var myRegex = new Regex(strRegex, RegexOptions.None);
            const string strRegex2 = @"[0-9]{1,2}\.{0,1}[0-9]{0,3}\s{0,2}\/[0-9]{1,2}\.{0,1}[0-9]{0,1}\s";
            var myRegex2 = new Regex(strRegex2, RegexOptions.None);

            var maxMark = new List<string>();

            var sortie = c;

            var skills = (from Match k in myRegex.Matches(sortie) select k.Value).ToList();

            var mark = (from Match l in myRegex2.Matches(sortie) select l.Value).ToList();

            var tempReturn = new List<Tuple<string, string>>();

            for (var index = 0; index < mark.Count; index++)
            {
                var m = mark[index];

                mark[index] = mark[index].Split('/')[0];
                maxMark.Add(m.Split('/')[1]);
            }

            for (var index = 0; index < skills.Count; index++)
            {
                var z = mark[index] + "->" + maxMark[index];
                //MessageBox.Show(z+"aa");

                tempReturn.Add(new Tuple<string, string>(mark[index], maxMark[index]));
            }

            // Ceci est tellement horrible MDR
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
    }
}
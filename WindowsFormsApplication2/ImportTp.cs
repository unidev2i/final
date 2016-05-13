// Decompiled with JetBrains decompiler
// Type: WindowsFormsApplication2.ImportTp
// Assembly: WindowsFormsApplication2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6A6DE8D1-7471-4F08-B320-CF44F5361467
// Assembly location: F:\__P I N F\program\WindowsFormsApplication2.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApplication2.Properties;
using WL;

namespace WindowsFormsApplication2
{
  public static class ImportTp
  {
    private static string _errMssg = string.Empty;
    private static string _logMssg = string.Empty;
    private static readonly string RootFolder = Settings.Default.repoPath;

    public static void Go()
    {
      ImportTp._logMssg = ImportTp._logMssg + "Traitement des dossiers démarré." + Environment.NewLine;
      if ((uint) Directory.GetFiles(ImportTp.RootFolder).Length > 0U)
        ImportTp._errMssg = ImportTp._errMssg + "<li>Plusieurs fichiers à la racine du répertoire ont étés détectés. Ils ne seront pas traités.</li>" + Environment.NewLine;
      if (Enumerable.Count<string>(Enumerable.SelectMany<string, string>((IEnumerable<string>) Directory.GetDirectories(ImportTp.RootFolder), new Func<string, IEnumerable<string>>(Directory.GetFiles))) == 0)
      {
        switch (MessageBox.Show("Le dossier est vide, toute la base de données sera effacée si vous continuez. Continuer ? ", "ATTENTION !", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        {
          case DialogResult.None:
            break;
          case DialogResult.Yes:
            Database.SimpleRequest("DELETE FROM `tp` WHERE 1");
            Database.SimpleRequest("DELETE FROM `eleve` WHERE 1");
            Database.SimpleRequest("DELETE FROM `competence` WHERE 1");
            Database.SimpleRequest("DELETE FROM `classe` WHERE 1");
            Thread.CurrentThread.Abort();
            break;
          case DialogResult.No:
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      else
      {
        Database.SimpleRequest("DELETE FROM `tp` WHERE 1");
        Database.SimpleRequest("DELETE FROM `eleve` WHERE 1");
        Database.SimpleRequest("DELETE FROM `competence` WHERE 1");
        Database.SimpleRequest("DELETE FROM `classe` WHERE 1");
        List<string> cp = ImportTp.CheckPromo();
        for (int index = 0; index < cp.Count; ++index)
          cp[index] = cp[index].Substring(0, cp[index].Length - 1);
        Program.ac.graphic.progressBar1.Invoke((Delegate) (() => Program.ac.graphic.progressBar1.Value = 0));
        Program.ac.graphic.progressBar1.Invoke((Delegate) (() => Program.ac.graphic.progressBar1.Visible = true));
        Program.ac.graphic.progressBar1.Invoke((Delegate) (() => Program.ac.graphic.progressBar1.Maximum = cp.Count));
        int yt = 0;
        int nt = 0;
        int dt = 0;
        foreach (string path in Directory.GetDirectories(ImportTp.RootFolder))
        {
          string str = path.Split('\\')[path.Split('\\').Length - 1];
          if (!cp.Contains(str) && (uint) Directory.GetFiles(path).Length > 0U)
            Database.ajouterPromo(path.Split('\\')[path.Split('\\').Length - 1]);
        }
        cp = ImportTp.CheckPromo();
        foreach (string path in Enumerable.Select<string, string>((IEnumerable<string>) cp, (Func<string, string>) (a => ImportTp.RootFolder + "\\" + a.Remove(a.Length - 1, 1))))
        {
          Database.GetListRequest("note", new string[1]
          {
            "Promotion"
          }, "1");
          if (!Directory.Exists(path) && (uint) Directory.GetFiles(path).Length > 0U)
          {
            switch (MessageBox.Show(string.Format("Le répertoire \"{0}\" n'existe pas. Voulez-vous supprimer les données de la base de données ?", (object) path), "ATTENTION !", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
            {
              case DialogResult.Yes:
                Database.DeletePromo(path.Split('\\')[path.Split('\\').Length - 1]);
                break;
              case DialogResult.No:
                ImportTp._errMssg = ImportTp._errMssg + "Le dossier " + path + " n'existe pas, mais il n'a pas été supprimé de la base de données." + Environment.NewLine;
                break;
            }
          }
          else
          {
            List<string> hashList = Database.GetHashList(path.Split('\\')[path.Split('\\').Length - 1]);
            foreach (string file in Directory.GetFiles(path))
            {
              string str = Crypt.Md5(file);
              if (hashList.Contains(str))
              {
                hashList.Remove(str);
                ++nt;
              }
              else
              {
                ImportTp.TraiterFichier(file);
                ++yt;
              }
            }
            foreach (string hashes in hashList)
            {
              ++dt;
              Database.DeleteTp(hashes);
            }
          }
          try
          {
            Program.ac.graphic.progressBar1.Invoke((Delegate) (() => ++Program.ac.graphic.progressBar1.Value));
          }
          catch
          {
          }
        }
        try
        {
          Program.ac.graphic.progressBar1.Invoke((Delegate) (() => Program.ac.graphic.progressBar1.Visible = false));
        }
        catch
        {
        }
        if (!ImportTp._errMssg.Equals(""))
        {
          int num = (int) MessageBox.Show("Terminé avec des erreurs : " + Environment.NewLine + ImportTp._errMssg);
        }
        try
        {
          Program.ac.graphic.LBL_InfoAjoutTp.Invoke((Delegate) (() =>
          {
            Label label = Program.ac.graphic.LBL_InfoAjoutTp;
            label.Text = label.Text + (object) Environment.NewLine + "Traités : " + (string) (object) yt + "   Ignorés : " + (string) (object) nt + "   Supprimés : " + (string) (object) dt;
          }));
          Program.ac.graphic.LBL_InfoAjoutTp.Invoke((Delegate) (() => Program.ac.graphic.LBL_InfoAjoutTp.Visible = true));
          Thread.Sleep(3000);
          Program.ac.graphic.LBL_InfoAjoutTp.Invoke((Delegate) (() => Program.ac.graphic.LBL_InfoAjoutTp.Visible = false));
        }
        catch
        {
        }
        Database.addCPMax(Database.CPsNewInNote());
        Database.removeCPMax(Database.CPMaxIsNotinNote());
        Program.ac.graphic.LBL_InfoAjoutTp.Invoke((Delegate) (() => Program.ac.graphic.comboBox1.Items.Clear()));
        Program.ac.graphic.LBL_InfoAjoutTp.Invoke((Delegate) (() => Program.ac.graphic.comboBox3.Items.Clear()));
        foreach (string str in Database.GetListRequest("eleve", new string[2]
        {
          "Prenom",
          "Nom"
        }, "1"))
        {
          string a = str;
          Program.ac.graphic.LBL_InfoAjoutTp.Invoke((Delegate) (() => Program.ac.graphic.comboBox1.Items.Add((object) a)));
        }
        foreach (string str in Database.GetListRequest("classe", new string[1]
        {
          "Promotion"
        }, "1"))
        {
          string a = str;
          Program.ac.graphic.LBL_InfoAjoutTp.Invoke((Delegate) (() => Program.ac.graphic.comboBox3.Items.Add((object) a)));
        }
        Thread.CurrentThread.Abort();
      }
    }

    public static void ShowLog()
    {
      int num = (int) new ImportTpInfo().ShowDialog();
      string str1 = "";
      if (!(ImportTp._errMssg != string.Empty))
        return;
      string str2 = str1 + "<p style='color:red; font-size:50px align:center'>Liste d'erreurs</p>";
    }

    private static List<string> CheckPromo()
    {
      Enumerable.Aggregate<string, string>((IEnumerable<string>) Directory.GetDirectories(ImportTp.RootFolder), string.Empty, (Func<string, string, string>) ((current, a) => current + "\"" + Crypt.CreateMd5ForFolder(a) + "\","));
      return Enumerable.ToList<string>(Database.GetListRequest("classe", new string[1]
      {
        "promotion"
      }, "1"));
    }

    private static Tuple<string, string, string> GetInfos(string file)
    {
      try
      {
        string[] strArray1 = file.Split('\\');
        string[] strArray2 = file.Split('\\')[strArray1.Length - 1].Split(new char[1]
        {
          '_'
        }, StringSplitOptions.RemoveEmptyEntries);
        return new Tuple<string, string, string>(strArray2[0], strArray2[1], strArray2[2].Remove(strArray2[2].Length - 4, 4));
      }
      catch (Exception ex)
      {
        ImportTp._errMssg = ImportTp._errMssg + file + " : Nom du fichier non reconnu. Attendu : NOM_PRENOM_TPXX.pdf" + Environment.NewLine;
        return (Tuple<string, string, string>) null;
      }
    }

    private static List<Tuple<string, string, string>> GetValue(string file)
    {
      if (!file.Contains(".pdf"))
        return (List<Tuple<string, string, string>>) null;
      string sFile = file;
      string str1 = (string) new pdfHandler(ref sFile).readPDF();
      Regex regex1 = new Regex("C[0-9].[0-9]", RegexOptions.None);
      Regex regex2 = new Regex("[0-9]{1,2}\\.{0,1}[0-9]{0,3}\\s{0,2}\\/\\s{0,2}[0-9]{1,2}\\.{0,1}[0-9]{0,1}\\s", RegexOptions.None);
      List<string> list1 = new List<string>();
      string input = str1;
      List<string> list2 = Enumerable.ToList<string>(Enumerable.Select<Match, string>(Enumerable.Cast<Match>((IEnumerable) regex1.Matches(input)), (Func<Match, string>) (k => k.Value)));
      List<string> list3 = Enumerable.ToList<string>(Enumerable.Select<Match, string>(Enumerable.Cast<Match>((IEnumerable) regex2.Matches(input)), (Func<Match, string>) (l => l.Value)));
      List<string> list4 = new List<string>();
      List<string> list5 = Enumerable.ToList<string>(Enumerable.Distinct<string>((IEnumerable<string>) list2));
      GC.Collect();
      List<Tuple<string, string, string>> list6 = new List<Tuple<string, string, string>>();
      for (int index = 0; index < list5.Count; ++index)
      {
        string str2 = list3[index];
        list3[index] = list3[index].Split('/')[0];
        list1.Add(str2.Split('/')[1]);
      }
      for (int index = 0; index < list5.Count; ++index)
      {
        string str2 = list3[index] + "->" + list1[index];
        list6.Add(new Tuple<string, string, string>(list5[index], list3[index], list1[index]));
      }
      GC.Collect();
      return list6;
    }

    private static void TraiterFichier(string file)
    {
      if (!file.Contains(".pdf"))
        ImportTp._errMssg = ImportTp._errMssg + file + " : Le fichier est au mauvais format. Attendu : pdf" + Environment.NewLine;
      Tuple<string, string, string> infos = ImportTp.GetInfos(file);
      if (infos == null)
        return;
      List<Tuple<string, string, string>> list = ImportTp.GetValue(file);
      string idEleveFromName = Database.GetIdEleveFromName(infos.Item1, infos.Item2);
      if (idEleveFromName == null)
      {
        Database.AjouteEleve(infos.Item1, infos.Item2, file.Split('\\')[file.Split('\\').Length - 2]);
        idEleveFromName = Database.GetIdEleveFromName(infos.Item1, infos.Item2);
      }
      string login_correcteur = Program.ac.graphic.login;
      Database.AddTp(infos.Item3, idEleveFromName, login_correcteur, Crypt.Md5(file));
      string lastPdfId = Database.GetLastPdfId();
      foreach (Tuple<string, string, string> tuple in list)
        Database.AddNote(lastPdfId, tuple.Item1, tuple.Item2, tuple.Item3);
    }
  }
}

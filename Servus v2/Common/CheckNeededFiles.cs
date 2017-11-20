using Servus_v2.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Servus_v2.Common
{
    public class CheckNeededFiles
    {
        private string a = "";
        private string b = "";
        private readonly string c = "EliteAPI.dll";
        private readonly string d = "EliteMMO.API.dll";
        private string j = "";
        private string k = "";

        public CheckNeededFiles(MainForm mf)
        {
            MF = mf;
            Client = new WebClient();
        }

        private WebClient Client { get; }
        private MainForm MF { get; }

        public void CheckAPI()
        {
            try
            {
                if (File.Exists(c))
                    a = FileVersionInfo.GetVersionInfo(c).FileVersion;
                if (File.Exists(d))
                    b = FileVersionInfo.GetVersionInfo(d).FileVersion;
                j = GetStringFromUrl("http://ext.elitemmonetwork.com/downloads/eliteapi/index.php?v");
                k = GetStringFromUrl("http://ext.elitemmonetwork.com/downloads/elitemmo_api/index.php?v");

                if (a == "" || a != j)
                {
                    MF.Logger.AddDebugText(MF.CheckedItemsRTB, "Getting Latest EliteAPI.dll");
                    Client.DownloadFile("http://ext.elitemmonetwork.com/downloads/eliteapi/EliteAPI.dll", c);
                }

                if (b == "" || b != k)
                {
                    MF.Logger.AddDebugText(MF.CheckedItemsRTB, "Getting Latest EliteMMO.API.dll");
                    Client.DownloadFile("http://ext.elitemmonetwork.com/downloads/elitemmo_api/EliteMMO.API.dll", d);
                }

                Client.Dispose();
                DirSearch(Application.StartupPath);
                FileUnblocker.UnblockFile(c);
                FileUnblocker.UnblockFile(d);
                MF.Logger.AddDebugText(MF.CheckedItemsRTB, "Finished Checking EliteMMO.dlls");
            }
            catch (Exception ex)
            {
                MF.Logger.LogFile(ex.Message, "CheckneededFiles");
            }
        }

        public bool DoWeHaveAllNeededFiles()
        {
            try
            {
                string NetVersion = Environment.Version.ToString();
                MF.Logger.AddDebugText(MF.CheckedItemsRTB, string.Format(@".NetFramework v  = ({0})", NetVersion));
                if (!NetVersion.Contains("4."))
                {
                    MF.Logger.AddDebugText(MF.CheckedItemsRTB, "Please Update your .Net framework, https://www.microsoft.com/en-us/download/details.aspx?id=53344");
                    return false;
                }

                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;
                MF.Logger.AddDebugText(MF.CheckedItemsRTB, string.Format(@"Servus Version ({0})", version));

                if (File.Exists("EliteAPI.dll"))
                {
                    FileVersionInfo EliteAPIVersion = FileVersionInfo.GetVersionInfo("EliteAPI.dll");
                    MF.Logger.AddDebugText(MF.CheckedItemsRTB, string.Format(@"EliteAPI Found: Version: ({0})", EliteAPIVersion.FileVersion));
                }
                if (!File.Exists("EliteAPI.dll"))
                {
                    MF.Logger.AddDebugText(MF.CheckedItemsRTB, string.Format(@"EliteAPI Missing"));
                    MF.Logger.AddDebugText(MF.CheckedItemsRTB, "Getting Latest EliteAPI.dll");
                    Client.DownloadFile("http://ext.elitemmonetwork.com/downloads/eliteapi/EliteAPI.dll", "EliteAPI.dll");
                }
                if (File.Exists("AStarPathFinder.dll"))
                {
                    FileVersionInfo AStarVersion = FileVersionInfo.GetVersionInfo("AStarPathFinder.dll");
                    MF.Logger.AddDebugText(MF.CheckedItemsRTB, string.Format(@"AStarPathFinder Found: Version: ({0})", AStarVersion.FileVersion));
                }
                else if (!File.Exists("AStarPathFinder.dll"))
                {
                    MF.Logger.AddDebugText(MF.CheckedItemsRTB, @"AStarPathFinder.dll is missing");
                }

                if (File.Exists("EliteMMO.API.dll"))
                {
                    FileVersionInfo EliteAPIVersion = FileVersionInfo.GetVersionInfo("EliteMMO.API.dll");
                    MF.Logger.AddDebugText(MF.CheckedItemsRTB, string.Format(@"EliteMMO.API Found: Version: ({0})", EliteAPIVersion.FileVersion));
                }
                else if (!File.Exists("EliteMMO.API.dll"))
                {
                    MF.Logger.AddDebugText(MF.CheckedItemsRTB, "EliteMMO.API MISSING");
                    MF.Logger.AddDebugText(MF.CheckedItemsRTB, "Getting Latest EliteMMO.API.dll");
                    Client.DownloadFile("http://ext.elitemmonetwork.com/downloads/elitemmo_api/EliteMMO.API.dll", "EliteMMO.API.dll");
                }
                //if (File.Exists(@"Resources-master\xml\job_Abilities.xml"))
                //{
                //    MF.Logger.AddDebugText(MF.CheckedItemsRTB, "Resources found");
                //}
                //else if (!File.Exists(@"Resources-master\xml\job_Abilities.xml"))
                //{
                //    MF.Logger.AddDebugText(MF.CheckedItemsRTB, "Resources MISSING");
                //    MF.Logger.AddDebugText(MF.CheckedItemsRTB, "Getting Latest Resources");
                //    MF.Logger.AddDebugText(MF.CheckedItemsRTB, "Downloaded resources master.zip from github/windower");
                //    Client.DownloadFile("https://github.com//Windower/Resources/archive/master.zip", "master.zip");
                //    string zipPath = "master.zip";
                //    string junk1 = @"Resources-master\lua";
                //    string junk2 = @"Resources-master\manifest.xml";
                //    string junk3 = @"Resources-master\README.md";
                //    var path = @"Resources-master";
                //    string junk4 = "LICENSE";
                //    string extractPath = Application.StartupPath;
                //    MF.Logger.AddDebugText(MF.CheckedItemsRTB, "Extracting Resources");
                //    ZipFile.ExtractToDirectory(zipPath, extractPath);
                //    MF.Logger.AddDebugText(MF.CheckedItemsRTB, string.Format(@"Deleting {0}", zipPath));
                //    File.Delete(zipPath);

                // MF.Logger.AddDebugText(MF.CheckedItemsRTB, @"Deleting files we do not need from
                // resources."); MF.Logger.AddDebugText(MF.CheckedItemsRTB, string.Format(@"Deleting
                // {0}", junk1)); Directory.Delete(junk1, true);
                // MF.Logger.AddDebugText(MF.CheckedItemsRTB, string.Format(@"Deleting {0}", junk2));
                // File.Delete(junk2); MF.Logger.AddDebugText(MF.CheckedItemsRTB,
                // string.Format(@"Deleting {0}", junk3)); File.Delete(junk3);
                // MF.Logger.AddDebugText(MF.CheckedItemsRTB, string.Format(@"Deleting {0}", junk4));
                // string[] Files = Directory.GetFiles(path);

                //    foreach (string file in Files)
                //    {
                //        if (file.ToUpper().Contains(junk4.ToUpper()))
                //        {
                //            File.Delete(file);
                //        }
                //    }
                //}
                MF.Logger.AddDebugText(MF.CheckedItemsRTB, @"Finished checking files");
            }
            catch (Exception ex)
            {
                MF.Logger.LogFile(ex.Message, "CheckNeededFiles");
                MF.Logger.AddDebugText(MF.CheckedItemsRTB, ex.ToString());
                return false;
            }
            Client.Dispose();
            return true;
        }

        private void DirSearch(string sDir)
        {
            foreach (string f in Directory.GetFiles(sDir))
            {
                if (f.Contains("EliteAPI.dll") && f != c)
                {
                    if (j != FileVersionInfo.GetVersionInfo(f).FileVersion)
                    {
                        File.Delete(f);
                        File.Copy(c, f);
                        MF.Logger.AddDebugText(MF.CheckedItemsRTB, "EliteAPI.dll updated");
                    }
                    else
                        MF.Logger.AddDebugText(MF.CheckedItemsRTB, "EliteAPI.dll is up to date");
                }
                else if (f.Contains("EliteMMO.API.dll") && f != d)
                {
                    if (k != FileVersionInfo.GetVersionInfo(f).FileVersion)
                    {
                        File.Delete(f);
                        File.Copy(d, f);
                        MF.Logger.AddDebugText(MF.CheckedItemsRTB, "EliteMMO.API.dll updated");
                    }
                    else
                        MF.Logger.AddDebugText(MF.CheckedItemsRTB, "EliteMMO.API.dll is up to date");
                }
            }
            foreach (string d in Directory.GetDirectories(sDir))
            {
                DirSearch(d);
            }
        }

        private string GetStringFromUrl(string location)
        {
            WebRequest request = WebRequest.Create(location);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            return responseFromServer;
        }

        public class FileUnblocker
        {
            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool DeleteFile(string name);

            public static bool UnblockFile(string fileName)
            {
                return DeleteFile(fileName + ":Zone.Identifier");
            }
        }
    }
}
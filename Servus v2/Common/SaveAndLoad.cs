using DeenGames.Utils.AStarPathFinder;
using Servus_v2.Characters;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Servus_v2.Common
{
    public class SaveAndLoad
    {
        public Character character { get; set; }

        public SaveAndLoad(Character Char)
        {
            character = Char;
        }

        public void LoadWaypoints()
        {
            OpenFileDialog OpenDialog = new OpenFileDialog();

            string startpath = Path.GetDirectoryName(Application.ExecutablePath);
            string PATH = (String.Format(@"{0}Documents\\{1}\\Nav\\", startpath, character.Api.Player.Name));
            OpenDialog.InitialDirectory = PATH;
            OpenDialog.FilterIndex = 0;

            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                character.Navi.Reset();
                character.Tc.WayPointListbox.Items.Clear();

                string Waypoint_Filename = OpenDialog.FileName;
                character.Logger.AddDebugText(character.Tc.rtbDebug, string.Format(@"Nav file loaded = {0}", Waypoint_Filename));
                string[] break_filename = OpenDialog.FileName.Split('\\');
                int name_pos = (break_filename.Count() - 1);
                character.Tc.WPLoadedLB.Text = break_filename.ElementAt(name_pos);

                FileStream fs = new FileStream(Waypoint_Filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs);

                String Line;
                while ((Line = sr.ReadLine()) != null)
                {
                    string[] Positions = Line.Split('|');

                    // Add WP
                    float _X = float.Parse(Positions.ElementAt(0));
                    float _Y = float.Parse(Positions.ElementAt(1));
                    float _Z = float.Parse(Positions.ElementAt(2));
                    character.Tc.WayPointListbox.Items.Add(_X + ", " + _Y + ", " + _Z);
                    Node Waypoint = new Node { X = _X, Y = _Y, Z = _Z };
                    if (character.Navi.Grid[Convert.ToInt32(Waypoint.X) + 1000, Convert.ToInt32(Waypoint.Z) + 1000] == PathFinderHelper.BLOCKED_TILE)
                    {
                        character.Navi.Waypoints.Add(Waypoint);
                        character.Navi.Grid[Convert.ToInt32(Waypoint.X) + 1000, Convert.ToInt32(Waypoint.Z) + 1000] = PathFinderHelper.EMPTY_TILE;
                        character.Logger.AddDebugText(character.Tc.rtbDebug, string.Format(@"Added tile {0},{1}", (Convert.ToInt32(Waypoint.X) + 1000).ToString(),
                        (Convert.ToInt32(Waypoint.Z) + 1000).ToString()));
                    }
                }
                sr.Dispose();
                sr.Close();
            }
        }

        private string GetXMLString(string elementName, XmlElement parent, XmlDocument document)
        {
            XmlNodeList nodes = document.GetElementsByTagName(elementName);
            if (nodes.Count != 0)
            {
                return nodes[0].InnerText;
            }
            return "";
        }

        private int GetXMLInt(string elementName, XmlElement parent, XmlDocument document)
        {
            string str = GetXMLString(elementName, parent, document);
            if (str != "")
            {
                return (int)Decimal.Parse(str);
            }
            return 0;
        }

        private bool GetXMLTrueFalse(string elementName, XmlElement parent, XmlDocument document)
        {
            return GetXMLString(elementName, parent, document) == "True" ? true : false;
        }

        // Given a document, a parent in that document to attach to, a new name for this element and
        // what the string value should be. Create the new element, set its value and then attach to
        // the parent.
        private void SetXMLString(string elementName, string innertext, XmlElement parent, XmlDocument document)
        {
            XmlElement element = document.CreateElement(elementName);
            element.InnerText = innertext;
            parent.AppendChild(element);
        }

        // Given a document, a parent in that document to attach to, a new name for this element and
        // what the int value should be. Create the new element, set its value and then attach to the parent.
        private void SetXMLInt(string elementName, int value, XmlElement parent, XmlDocument document)
        {
            SetXMLString(elementName, value.ToString(), parent, document);
        }

        // Given a document, a parent in that document to attach to, a new name for this element and
        // what the boolean value should be. Create the new element, set its value and then attach to
        // the parent.
        private void SetXMLTrueFalse(string elementName, bool truefalse, XmlElement parent, XmlDocument document)
        {
            SetXMLString(elementName, truefalse ? "True" : "False", parent, document);
        }

        public void SaveWayPoints()
        {
            SaveFileDialog fdgSave = new SaveFileDialog();

            string startpath = Path.GetDirectoryName(Application.ExecutablePath);
            string PATH = (String.Format(@"{0}Documents\\{1}\\Nav\\", startpath, character.Api.Player.Name));

            SaveFileDialog SaveDialog = new SaveFileDialog();
            SaveDialog.InitialDirectory = PATH;
            SaveDialog.Filter = "FFXI Way Points|*.wps";
            SaveDialog.FilterIndex = 1;
            string Waypoint_Filename;
            if (SaveDialog.ShowDialog() == DialogResult.OK)
            {
                if (SaveDialog.FileName.Contains(".wps"))
                    Waypoint_Filename = SaveDialog.FileName;
                else
                    Waypoint_Filename = SaveDialog.FileName + ".wps";

                string[] break_filename = SaveDialog.FileName.Split('\\');
                int name_pos = (break_filename.Count() - 1);
                character.Tc.WPLoadedLB.Text = break_filename.ElementAt(name_pos);

                // Set File Info
                FileInfo fi = new FileInfo(Waypoint_Filename);
                FileStream fs = fi.Create();
                StreamWriter sw = new StreamWriter(fs);

                // Write out each text line
                int MaxPos = (character.Navi.Waypoints.Count);
                int i = 0;

                while (i < MaxPos)
                {
                    string Line = character.Navi.Waypoints.ElementAt(i).X + "|" + character.Navi.Waypoints.ElementAt(i).Y + "|" + character.Navi.Waypoints.ElementAt(i).Z;
                    sw.WriteLine(Line.Trim());
                    i++;
                }

                // Close
                sw.Dispose();
                sw.Close();
                fs.Dispose();
                fs.Close();
                character.Logger.AddDebugText(character.Tc.rtbDebug, "Nav file saved");
            }
        }

        public void SaveSettings()
        {
            SaveFileDialog fdgSave = new SaveFileDialog();

            string startpath = Path.GetDirectoryName(Application.ExecutablePath);
            string PATH = (String.Format(@"{0}Documents\\{1}\\Config\\", startpath, character.Api.Player.Name));

            fdgSave.InitialDirectory = PATH;
            fdgSave.Filter = "XML (*.XML)|*.xml|All Files (*.*)|*.*";
            fdgSave.FilterIndex = 1;

            XmlDocument doc;

            //Create an xml document
            doc = new XmlDocument();

            //Create neccessary nodes
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            XmlComment comment = doc.CreateComment("This is an XML Generated File");
            XmlElement root = doc.CreateElement("Settings");
            XmlElement Settings = doc.CreateElement("Settings");
            XmlElement Targets = doc.CreateElement("Targets");

            //Construct the document
            doc.AppendChild(declaration);
            doc.AppendChild(comment);
            doc.AppendChild(root);
            root.AppendChild(Settings);
            root.AppendChild(Targets);

            #region Hunter:targets

            if (character.Tasks.Huntertask.Options.Targets.Count > 0)
            {
                foreach (var str in character.Tasks.Huntertask.Options.Targets)
                {
                    SetXMLString("Targets", str, Targets, doc);
                }
            }

            #endregion Hunter:targets

            if (fdgSave.ShowDialog() == DialogResult.OK)
            {
                doc.Save(fdgSave.FileName);
                character.Logger.AddDebugText(character.Tc.rtbDebug, fdgSave.FileName);//Do what you want here
            }

            #region settings

            //SetXMLTrueFalse("Encure", tc.EnableCure.Checked, Settings, doc);
            //SetXMLTrueFalse("EnCureII", tc.EnableCureII.Checked, Settings, doc);
            //SetXMLTrueFalse("EnCureIII", tc.EnableCureIII.Checked, Settings, doc);
            //SetXMLTrueFalse("EnCureIV", tc.EnableCureIV.Checked, Settings, doc);
            //SetXMLTrueFalse("EnCureV", tc.EnableCureV.Checked, Settings, doc);
            //SetXMLTrueFalse("EnCureVI", tc.EnableCureVI.Checked, Settings, doc);
            //SetXMLString("valueCure", tc.ValueCure.Text, Settings, doc);
            //SetXMLString("ValueCure2", tc.CureII.Text, Settings, doc);

            #endregion settings
        }
    }
}
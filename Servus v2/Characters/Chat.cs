using EliteMMO.API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Servus_v2.Characters
{
    public class Chat
    {
        public List<EliteAPI.ChatEntry> BattleLog
        {
            get
            {
                return battleLog;
            }
        }

        public int BattleLogAdded { get; private set; }

        public List<EliteAPI.ChatEntry> ChatLog
        {
            get
            {
                return chatLog;
            }
        }

        public int ChatLogAdded { get; private set; }
        public EliteAPI.ChatEntry CurrentLine { get; set; }

        public List<EliteAPI.ChatEntry> FishLog
        {
            get
            {
                return fishLog;
            }
        }

        public int FishLogAdded { get; private set; }

        public List<EliteAPI.ChatEntry> PartyLog
        {
            get
            {
                return chatLog;
            }
        }

        public int PartyLogAdded { get; private set; }

        public List<EliteAPI.ChatEntry> SayLog
        {
            get
            {
                return chatLog;
            }
        }

        public int SayLogAdded { get; private set; }

        public List<EliteAPI.ChatEntry> ShellLog
        {
            get
            {
                return chatLog;
            }
        }

        public int ShellLogAdded { get; private set; }

        public List<EliteAPI.ChatEntry> ShoutLog
        {
            get
            {
                return shoutLog;
            }
        }

        public int ShoutLogAdded { get; private set; }

        public List<EliteAPI.ChatEntry> TellLog
        {
            get
            {
                return chatLog;
            }
        }

        public int TellLogAdded { get; private set; }

        public List<EliteAPI.ChatEntry> YellLog
        {
            get
            {
                return yellLog;
            }
        }

        public Chat(Character character)
        {
            Char = character;
        }

        public int YellLogAdded { get; private set; }
        public Character Char { get; set; }
        private const int logMaxLength = 100;
        private List<EliteAPI.ChatEntry> battleLog = new List<EliteAPI.ChatEntry>();
        private List<EliteAPI.ChatEntry> chatLog = new List<EliteAPI.ChatEntry>();
        private List<EliteAPI.ChatEntry> fishLog = new List<EliteAPI.ChatEntry>();
        private List<EliteAPI.ChatEntry> partyLog = new List<EliteAPI.ChatEntry>();
        private List<EliteAPI.ChatEntry> sayLog = new List<EliteAPI.ChatEntry>();
        private List<EliteAPI.ChatEntry> shellLog = new List<EliteAPI.ChatEntry>();
        private List<EliteAPI.ChatEntry> shoutLog = new List<EliteAPI.ChatEntry>();
        private List<EliteAPI.ChatEntry> tellLog = new List<EliteAPI.ChatEntry>();
        private List<EliteAPI.ChatEntry> yellLog = new List<EliteAPI.ChatEntry>();

        public Color BrightenColor(EliteAPI.ChatEntry chatLine)
        {
            Color brighterColor = new Color();
            //how many steps away can the colors be away from each other to be close enough to grayscale
            //this variable's value is subjective, and chosen by the programmer
            int steps = 3;
            //tolerance = 256 colors / 64 in-game 'step' choices * steps
            int tolerance = 256 / 64 * steps;
            int closeEnoughRG = Math.Abs(chatLine.ChatColor.R - chatLine.ChatColor.G);
            int closeEnoughGB = Math.Abs(chatLine.ChatColor.G - chatLine.ChatColor.B);
            int closeEnoughRB = Math.Abs(chatLine.ChatColor.R - chatLine.ChatColor.B);

            if ((closeEnoughRG <= tolerance) && (closeEnoughGB <= tolerance) && (closeEnoughRB <= tolerance))
            {
                //greatly brighten white and gray text
                brighterColor = RGBHSL.ModifyBrightness(chatLine.ChatColor, 1.85);
            }
            else
            {
                //only slighty brighten color text
                brighterColor = RGBHSL.ModifyBrightness(chatLine.ChatColor, 1.25);
            }

            return brighterColor;
        }

        public void ChatWork()
        {
            int workdone = NewChat();
            if (0 < workdone)
            {
                UpdateChatLogs(Char.Tc.rtbChat, ChatLog, ChatLogAdded);

                if (0 < SayLogAdded)
                {
                    UpdateChatLogs(Char.Tc.rtbSay, SayLog, SayLogAdded);
                }

                if (0 < PartyLogAdded)
                {
                    UpdateChatLogs(Char.Tc.rtbParty, PartyLog, PartyLogAdded);
                }

                if (0 < ShellLogAdded)
                {
                    UpdateChatLogs(Char.Tc.rtbShell, ShellLog, ShellLogAdded);
                }

                if (0 < TellLogAdded)
                {
                    UpdateChatLogs(Char.Tc.rtbTell, TellLog, TellLogAdded);
                    if (Char.Tasks.Huntertask.Options.StopOnTell)
                    {
                        Char.Tasks.Huntertask.Stop();
                        Char.Navi.Reset();
                        Char.Logger.AddDebugText(Char.Tc.rtbDebug, "Stop on Tell!, HunterTask Stopped, Nav reset");
                    }
                }
                if (0 < YellLogAdded)
                {
                    UpdateChatLogs(Char.Tc.rtbYell, YellLog, YellLogAdded);
                }
                if (0 < ShoutLogAdded)
                {
                    UpdateChatLogs(Char.Tc.rtbShout, ShoutLog, ShoutLogAdded);
                }
                Clear();
            }
        }

        public void Clear()
        {
            ChatLogAdded = 0;
            SayLogAdded = 0;
            PartyLogAdded = 0;
            ShellLogAdded = 0;
            TellLogAdded = 0;
            FishLogAdded = 0;
            ShoutLogAdded = 0;
            YellLogAdded = 0;
            BattleLogAdded = 0;
        }

        public void ClearChatLogs()
        {
            // character.Api.Chat.Clear();
        }

        /// <summary> Grab all new chat lines and put them in the appropriate List<>s, Increase line
        /// added counters as chat lines are added to a List<>, Keep the List<>s to a maximum size
        /// </summary> <returns> How many lines were added at the beginning of the chatLog so we know
        /// how many to parse through for the logs </returns>
        public int NewChat()
        {
            try
            {
                CurrentLine = Char.Api.Chat.GetNextChatLine();

                while (CurrentLine != null)
                {
                    if (CurrentLine.ChatType != -1)
                    {
                        chatLog.Insert(0, CurrentLine);
                        ChatLogAdded++;

                        switch (CurrentLine.ChatType)
                        {
                            case 9:
                            case 1:
                                sayLog.Insert(0, CurrentLine);
                                SayLogAdded++;
                                break;

                            case 5:
                            case 13:
                                partyLog.Insert(0, CurrentLine);
                                PartyLogAdded++;
                                break;

                            case 2:
                            case 10:
                                shoutLog.Insert(0, CurrentLine);
                                ShoutLogAdded++;
                                break;

                            case 6:
                            case 14:
                                shellLog.Insert(0, CurrentLine);
                                ShellLogAdded++;
                                break;

                            case 4:
                            case 12:
                                tellLog.Insert(0, CurrentLine);
                                TellLogAdded++;
                                break;

                            case 11:
                                yellLog.Insert(0, CurrentLine);
                                YellLogAdded++;
                                break;

                            case 146:
                            case 148:
                                FishLog.Insert(0, CurrentLine);
                                FishLogAdded++;
                                break;

                            case 36:
                                BattleLog.Insert(0, CurrentLine);
                                BattleLogAdded++;
                                break;

                            case 20:
                                BattleLog.Insert(0, CurrentLine);
                                BattleLogAdded++;
                                break;

                            case 101:
                                BattleLog.Insert(0, CurrentLine);
                                BattleLogAdded++;
                                break;

                            case 21:
                                BattleLog.Insert(0, CurrentLine);
                                BattleLogAdded++;
                                break;

                            case 50:
                                BattleLog.Insert(0, CurrentLine);
                                BattleLogAdded++;
                                break;

                            case 110:
                                BattleLog.Insert(0, CurrentLine);
                                BattleLogAdded++;
                                break;

                            case 52:
                                BattleLog.Insert(0, CurrentLine);
                                BattleLogAdded++;
                                break;

                            case 114:
                                BattleLog.Insert(0, CurrentLine);
                                BattleLogAdded++;
                                break;

                            case 56:
                                BattleLog.Insert(0, CurrentLine);
                                BattleLogAdded++;
                                break;

                            default:
                                break;
                        }

                        CurrentLine = Char.Api.Chat.GetNextChatLine();
                    }
                    else
                    {
                        return -1;  //ChatMode.Error - try to recover
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }

            //only need to trim the logs if something was added
            if (0 < ChatLogAdded)
            {
                TrimLogs();
            }

            return ChatLogAdded;
        }

        // @ internal static Color BrightenColor(FFACE.ChatTools.ChatLine chatLine)

        public void SendTextToFFxi(string Text)
        {
            Char.Api.ThirdParty.SendString(Text);
        }

        public void UpdateChatLogs(RichTextBox rtb, List<EliteAPI.ChatEntry> log, int linesToParse)
        {
            if (0 < linesToParse)
            {
                for (int i = (linesToParse - 1); i >= 0; i--)
                {
                    try
                    {
                        if (log[i] != null && !string.IsNullOrEmpty(log[i].Text))
                        {
                            rtb.SelectionStart = rtb.Text.Length;
                            rtb.SelectionColor = BrightenColor(log[i]);
                            rtb.SelectedText = log[i].Text + Environment.NewLine;
                            rtb.SelectionStart = rtb.Text.Length - 1;
                            rtb.ScrollToCaret();
                        }
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Char.Logger.AddDebugText(Char.Tc.rtbDebug, ex.ToString());
                        break;
                    }
                }
            }
        }

        private void TrimLogs()
        {
            //keep the logs to a size of 'logMaxLength'
            if (chatLog.Count > logMaxLength)
            {
                chatLog.RemoveRange(logMaxLength, (chatLog.Count - logMaxLength));
                chatLog.TrimExcess();
            }

            if (sayLog.Count > logMaxLength)
            {
                sayLog.RemoveRange(logMaxLength, (sayLog.Count - logMaxLength));
                sayLog.TrimExcess();
            }

            if (partyLog.Count > logMaxLength)
            {
                partyLog.RemoveRange(logMaxLength, (partyLog.Count - logMaxLength));
                partyLog.TrimExcess();
            }

            if (shellLog.Count > logMaxLength)
            {
                shellLog.RemoveRange(logMaxLength, (shellLog.Count - logMaxLength));
                shellLog.TrimExcess();
            }

            if (tellLog.Count > logMaxLength)
            {
                tellLog.RemoveRange(logMaxLength, (tellLog.Count - logMaxLength));
                tellLog.TrimExcess();
            }

            if (fishLog.Count > logMaxLength)
            {
                fishLog.RemoveRange(logMaxLength, (fishLog.Count - logMaxLength));
                fishLog.TrimExcess();
            }

            if (shoutLog.Count > logMaxLength)
            {
                shoutLog.RemoveRange(logMaxLength, (shoutLog.Count - logMaxLength));
                shoutLog.TrimExcess();
            }
            if (BattleLog.Count > logMaxLength)
            {
                BattleLog.RemoveRange(logMaxLength, (BattleLog.Count - logMaxLength));
                BattleLog.TrimExcess();
            }
        }

        private delegate void ChatLogsDelegate(RichTextBox param1, List<string> param2, int param3);

        private delegate void VoidNoParamDelegate();

        // @ internal static int NewChat()

        // @ private static void Clear()
        public static class RGBHSL
        {
            /// <summary>
            /// Converts a colour from HSL to RGB
            /// </summary>
            /// <remarks>Adapted from the algoritm in Foley and Van-Dam</remarks>
            /// <param name="hsl">The HSL value</param>
            /// <returns>A Color structure containing the equivalent RGB values</returns>
            public static Color HSL_to_RGB(HSL hsl)
            {
                double r = 0, g = 0, b = 0;
                double temp1, temp2;

                if (hsl.L == 0)
                {
                    r = g = b = 0;
                }
                else
                {
                    if (hsl.S == 0)
                    {
                        r = g = b = hsl.L;
                    }
                    else
                    {
                        temp2 = ((hsl.L <= 0.5) ? hsl.L * (1.0 + hsl.S) : hsl.L + hsl.S - (hsl.L * hsl.S));
                        temp1 = 2.0 * hsl.L - temp2;

                        double[] t3 = new double[] { hsl.H + 1.0 / 3.0, hsl.H, hsl.H - 1.0 / 3.0 };
                        double[] clr = new double[] { 0, 0, 0 };
                        for (int i = 0; i < 3; i++)
                        {
                            if (t3[i] < 0)
                                t3[i] += 1.0;
                            if (t3[i] > 1)
                                t3[i] -= 1.0;
                            if (6.0 * t3[i] < 1.0)
                                clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
                            else if (2.0 * t3[i] < 1.0)
                                clr[i] = temp2;
                            else if (3.0 * t3[i] < 2.0)
                                clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
                            else
                                clr[i] = temp1;
                        }
                        r = clr[0];
                        g = clr[1];
                        b = clr[2];
                    }
                }

                return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
            }

            /// <summary>
            /// Modifies an existing brightness level
            /// </summary>
            /// <remarks>
            /// To reduce brightness use a number smaller than 1. To increase brightness use a number
            /// larger tnan 1
            /// </remarks>
            /// <param name="c">The original colour</param>
            /// <param name="brightness">The luminance delta</param>
            /// <returns>An adjusted colour</returns>
            public static Color ModifyBrightness(Color c, double brightness)
            {
                HSL hsl = RGB_to_HSL(c);
                hsl.L *= brightness;

                return HSL_to_RGB(hsl);
            }

            /// <summary>
            /// Converts RGB to HSL
            /// </summary>
            /// <remarks>
            /// Takes advantage of whats already built in to .NET by using the Color.GetHue,
            /// Color.GetSaturation and Color.GetBrightness methods
            /// </remarks>
            /// <param name="c">A Color to convert</param>
            /// <returns>An HSL value</returns>
            public static HSL RGB_to_HSL(Color c)
            {
                HSL hsl = new HSL
                {
                    H = c.GetHue() / 360.0, // we store hue as 0-1 as opposed to 0-360
                    L = c.GetBrightness(),
                    S = c.GetSaturation()
                };

                return hsl;
            }

            public class HSL
            {
                public double H  //Hue
                {
                    get { return _h; }
                    set
                    {
                        _h = value;
                        _h = _h > 1 ? 1 : _h < 0 ? 0 : _h;
                    }
                }

                public double L  //Luminance (Brightness)
                {
                    get { return _l; }
                    set
                    {
                        _l = value;
                        _l = _l > 1 ? 1 : _l < 0 ? 0 : _l;
                    }
                }

                public double S  //Saturation
                {
                    get { return _s; }
                    set
                    {
                        _s = value;
                        _s = _s > 1 ? 1 : _s < 0 ? 0 : _s;
                    }
                }

                private double _h;

                private double _l;

                private double _s;

                public HSL()
                {
                    _h = 0;
                    _s = 0;
                    _l = 0;
                }
            }
        }
    }
}
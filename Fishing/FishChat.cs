using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FFACETools;

namespace Fishing
{
    internal static class FishChat
    {
        private const int logMaxLength = 100;
        private static FFACE.ChatTools.ChatLine currentLine = new FFACE.ChatTools.ChatLine();

        internal static int chatLogAdded = 0;
        internal static int fishLogAdded = 0;
        internal static int partyLogAdded = 0;
        internal static int shellLogAdded = 0;
        internal static int tellLogAdded = 0;
        internal static int sayLogAdded = 0;
        internal static List<FFACE.ChatTools.ChatLine> chatLog = new List<FFACE.ChatTools.ChatLine>();
        internal static List<FFACE.ChatTools.ChatLine> fishLog = new List<FFACE.ChatTools.ChatLine>();
        internal static List<FFACE.ChatTools.ChatLine> partyLog = new List<FFACE.ChatTools.ChatLine>();
        internal static List<FFACE.ChatTools.ChatLine> shellLog = new List<FFACE.ChatTools.ChatLine>();
        internal static List<FFACE.ChatTools.ChatLine> tellLog = new List<FFACE.ChatTools.ChatLine>();
        internal static List<FFACE.ChatTools.ChatLine> sayLog = new List<FFACE.ChatTools.ChatLine>();

        /// <summary>
        /// Grab all new chat lines and put them in the appropriate List<>s, 
        /// Increase line added counters as chat lines are added to a List<>,
        /// Keep the List<>s to a maximum size
        /// </summary>
        /// <returns>
        /// How many lines were added at the beginning of the chatLog
        /// so we know how many to parse through for the logs
        /// </returns>
        internal static int NewChat()
        {
            currentLine = FishingForm._FFACE.Chat.GetNextLine();

            while(currentLine != null)
            //while (!string.IsNullOrEmpty(currentLine.Text))
            {
                if(ChatMode.Error != currentLine.Type)
                {
                    chatLog.Insert(0, currentLine);
                    chatLogAdded++;

                    switch(currentLine.Type)
                    {
                        case ChatMode.FishObtained:
                        case ChatMode.FishResult:
                            fishLog.Insert(0, currentLine);
                            fishLogAdded++;
                            break;
                        case ChatMode.SentParty:
                        case ChatMode.RcvdParty:
                            partyLog.Insert(0, currentLine);
                            partyLogAdded++;
                            break;
                        case ChatMode.SentSay:
                        case ChatMode.RcvdSay:
                            sayLog.Insert(0, currentLine);
                            sayLogAdded++;
                            break;
                        case ChatMode.SentLinkShell:
                        case ChatMode.RcvdLinkShell:
                            shellLog.Insert(0, currentLine);
                            shellLogAdded++;
                            break;
                        case ChatMode.SentTell:
                        case ChatMode.RcvdTell:
                            tellLog.Insert(0, currentLine);
                            tellLogAdded++;
                            break;
                        default:
                            break;
                    }

                    currentLine = FishingForm._FFACE.Chat.GetNextLine();
                }
                else
                {
                    return -1;  //ChatMode.Error - try to recover
                }
            }

            //only need to trim the logs if something was added
            if(0 < chatLogAdded)
            {
                TrimLogs();
            }

            return chatLogAdded;

        } // @ internal static int NewChat()

        internal static void Clear()
        {
            chatLogAdded = 0;
            fishLogAdded = 0;
            partyLogAdded = 0;
            shellLogAdded = 0;
            tellLogAdded = 0;
            sayLogAdded = 0;

        } // @ private static void Clear()

        internal static Color BrightenColor(FFACE.ChatTools.ChatLine chatLine)
        {
            Color brighterColor = new Color();
            //how many steps away can the colors be away from each other to be close enough to grayscale
            //this variable's value is subjective, and chosen by the programmer
            int steps = 3;
            //tolerance = 256 colors / 64 in-game 'step' choices * steps
            int tolerance = 256 / 64 * steps;
            int closeEnoughRG = Math.Abs(chatLine.Color.R - chatLine.Color.G);
            int closeEnoughGB = Math.Abs(chatLine.Color.G - chatLine.Color.B);
            int closeEnoughRB = Math.Abs(chatLine.Color.R - chatLine.Color.B);

            if((closeEnoughRG <= tolerance) && (closeEnoughGB <= tolerance) && (closeEnoughRB <= tolerance))
            {
                //greatly brighten white and gray text
                brighterColor = RGBHSL.ModifyBrightness(chatLine.Color, 1.85);
            }
            else
            {
                //only slighty brighten color text
                brighterColor = RGBHSL.ModifyBrightness(chatLine.Color, 1.25);
            }

            return brighterColor;

        } // @ internal static Color BrightenColor(FFACE.ChatTools.ChatLine chatLine)

        private static void TrimLogs()
        {
            //keep the logs to a size of 'logMaxLength'
            if(chatLog.Count > logMaxLength)
            {
                chatLog.RemoveRange(logMaxLength, (chatLog.Count - logMaxLength));
                chatLog.TrimExcess();
            }

            if(fishLog.Count > logMaxLength)
            {
                fishLog.RemoveRange(logMaxLength, (fishLog.Count - logMaxLength));
                fishLog.TrimExcess();
            }

            if(partyLog.Count > logMaxLength)
            {
                partyLog.RemoveRange(logMaxLength, (partyLog.Count - logMaxLength));
                partyLog.TrimExcess();
            }

            if(shellLog.Count > logMaxLength)
            {
                shellLog.RemoveRange(logMaxLength, (shellLog.Count - logMaxLength));
                shellLog.TrimExcess();
            }

            if(tellLog.Count > logMaxLength)
            {
                tellLog.RemoveRange(logMaxLength, (tellLog.Count - logMaxLength));
                tellLog.TrimExcess();
            }
            if (sayLog.Count > logMaxLength)
            {
                sayLog.RemoveRange(logMaxLength, (sayLog.Count - logMaxLength));
                sayLog.TrimExcess();
            }

        } // @ private static void TrimLogs()

    } // @ internal static class FishChat

    public static class RGBHSL
    {
        public class HSL
        {
            public HSL()
            {
                _h = 0;
                _s = 0;
                _l = 0;
            }

            double _h;
            double _s;
            double _l;

            public double H  //Hue
            {
                get { return _h; }
                set
                {
                    _h = value;
                    _h = _h > 1 ? 1 : _h < 0 ? 0 : _h;
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

            public double L  //Luminance (Brightness)
            {
                get { return _l; }
                set
                {
                    _l = value;
                    _l = _l > 1 ? 1 : _l < 0 ? 0 : _l;
                }
            }
        }

        /// <summary>
        /// Modifies an existing brightness level
        /// </summary>
        /// <remarks>
        /// To reduce brightness use a number smaller than 1. To increase brightness use a number larger tnan 1
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
        /// Converts a colour from HSL to RGB
        /// </summary>
        /// <remarks>Adapted from the algoritm in Foley and Van-Dam</remarks>
        /// <param name="hsl">The HSL value</param>
        /// <returns>A Color structure containing the equivalent RGB values</returns>
        public static Color HSL_to_RGB(HSL hsl)
        {
            double r = 0, g = 0, b = 0;
            double temp1, temp2;

            if(hsl.L == 0)
            {
                r = g = b = 0;
            }
            else
            {
                if(hsl.S == 0)
                {
                    r = g = b = hsl.L;
                }
                else
                {
                    temp2 = ((hsl.L <= 0.5) ? hsl.L * (1.0 + hsl.S) : hsl.L + hsl.S - (hsl.L * hsl.S));
                    temp1 = 2.0 * hsl.L - temp2;

                    double[] t3 = new double[] { hsl.H + 1.0 / 3.0, hsl.H, hsl.H - 1.0 / 3.0 };
                    double[] clr = new double[] { 0, 0, 0 };
                    for(int i = 0; i < 3; i++)
                    {
                        if(t3[i] < 0)
                            t3[i] += 1.0;
                        if(t3[i] > 1)
                            t3[i] -= 1.0;
                        if(6.0 * t3[i] < 1.0)
                            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
                        else if(2.0 * t3[i] < 1.0)
                            clr[i] = temp2;
                        else if(3.0 * t3[i] < 2.0)
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
        /// Converts RGB to HSL
        /// </summary>
        /// <remarks>Takes advantage of whats already built in to .NET by using the Color.GetHue, Color.GetSaturation and Color.GetBrightness methods</remarks>
        /// <param name="c">A Color to convert</param>
        /// <returns>An HSL value</returns>
        public static HSL RGB_to_HSL(Color c)
        {
            HSL hsl = new HSL();

            hsl.H = c.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360
            hsl.L = c.GetBrightness();
            hsl.S = c.GetSaturation();

            return hsl;
        }

    } // @ internal class RGBHSL
    /* This tool is part of the xRay Toolkit and is provided free of charge by Bob Powell.
     * This code is not guaranteed to be free from defects or fit for merchantability in any way.
     * By using this tool in your own programs you agree to hold Robert W. Powell free from all
     * damages direct or incidental that arise from such use.
     * You may use this code free of charge in your own projects on condition that you place the
     * following paragraph (enclosed in quotes below) in your applications help or about dialog.
     * "Portions of this code provided by Bob Powell. http://www.bobpowell.net"
     */
}

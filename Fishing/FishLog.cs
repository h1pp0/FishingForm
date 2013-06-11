using System.Collections.Generic;
using System.Text;

namespace Fishing
{
    public class FishLog
    {
        internal delegate void FishLogChanged();
        internal static event FishLogChanged OnChanged;
        internal static int totalCastCount = 0;
        internal static int noCatchCount = 0;
        internal static int caughtCount = 0;
        internal static int monsterCount = 0;
        internal static int lostCatchCount = 0;
        internal static int lackSkillCount = 0;
        internal static int tooSmallCount = 0;
        internal static int tooLargeCount = 0;
        internal static int lineBreakCount = 0;
        internal static int rodBreakCount = 0;
        internal static int releasedCount = 0;
        internal static SortedDictionary<string, int> caughtFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> lostCatchFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> lackSkillFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> tooSmallFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> tooLargeFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> lineBreakFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> rodBreakFishes = new SortedDictionary<string, int>();
        internal static SortedDictionary<string, int> releasedFishes = new SortedDictionary<string, int>();

        internal static void Clear()
        {
            totalCastCount = 0;
            noCatchCount = 0;
            caughtCount = 0;
            monsterCount = 0;
            lostCatchCount = 0;
            lackSkillCount = 0;
            tooSmallCount = 0;
            tooLargeCount = 0;
            lineBreakCount = 0;
            rodBreakCount = 0;
            releasedCount = 0;
            caughtFishes.Clear();
            lostCatchFishes.Clear();
            lackSkillFishes.Clear();
            tooSmallFishes.Clear();
            tooLargeFishes.Clear();
            lineBreakFishes.Clear();
            rodBreakFishes.Clear();
            releasedFishes.Clear();
        }

        internal static void AddFish(SortedDictionary<string, int> dictionary, string fish)
        {
            if(!dictionary.ContainsKey(fish))
            {
                dictionary.Add(fish, 0);
            }
            dictionary[fish]++;
            if(null != OnChanged)
            {
                OnChanged();
            }
        }

        internal static string PrintLog()
        {
            // stats log is built from scratch each time PrintLog() is called 
            StringBuilder log = new StringBuilder();

            log.AppendLine("Total casts:  " + totalCastCount.ToString()).AppendLine();

            if(noCatchCount > 0)
            {
                log.AppendLine("No catch  ( " + noCatchCount.ToString() + " / " + 
                            GetPercentage(noCatchCount, totalCastCount, 1) + "% )").AppendLine();
            }
            if(caughtCount > 0)
            {
                log.AppendLine(LogSection("Caught", caughtCount, caughtFishes));
            }
            if(lostCatchCount > 0)
            {
                log.AppendLine(LogSection("Lost catch", lostCatchCount, lostCatchFishes));
            }
            if(lackSkillCount > 0)
            {
                log.AppendLine(LogSection("Lack skill", lackSkillCount, lackSkillFishes));
            }
            if(tooSmallCount > 0)
            {
                log.AppendLine(LogSection("Too small", tooSmallCount, tooSmallFishes));
            }
            if(tooLargeCount > 0)
            {
                log.AppendLine(LogSection("Too large", tooLargeCount, tooLargeFishes));
            }
            if(lineBreakCount > 0)
            {
                log.AppendLine(LogSection("Line break", lineBreakCount, lineBreakFishes));
            }
            if(rodBreakCount > 0)
            {
                log.AppendLine(LogSection("Rod break", rodBreakCount, rodBreakFishes));
            }
            if(releasedCount > 0)
            {
                log.AppendLine(LogSection("Released", releasedCount, releasedFishes));
            }
            if(monsterCount > 0)
            {
                log.AppendLine("Monster  ( " + monsterCount.ToString() + " / " + GetPercentage(monsterCount, totalCastCount, 1) + "% )");
            }
            
            return log.ToString();
        }

        private static string LogSection(string header, int count, SortedDictionary<string, int> dictionary)
        {
            if(dictionary.Count > 0)
            {
                StringBuilder logSection = new StringBuilder();
                header = header + "  ( " + count + " / " + GetPercentage(count, totalCastCount, 1) + "% )";
                logSection.AppendLine(header);
                foreach (KeyValuePair<string, int> fishStats in dictionary)
                {
                    string items = "   " + fishStats.Key + " : " + fishStats.Value.ToString();
                    logSection.AppendLine(items);
                }

                return logSection.ToString();
            }

            return string.Empty;
        }

        private static string GetPercentage(int value, int total, int places)
        {
            decimal percent;
            string stringPercentage = string.Empty;
            string stringPlaces = new string('0', places);
            if((value == 0) || (total == 0))
            {
                percent = 0;
            }
            else
            {
                percent = decimal.Divide(value, total) * 100;
                if(places > 0)
                {
                    stringPlaces = "." + stringPlaces;
                }
            }
            stringPercentage = percent.ToString("#" + stringPlaces);

            return stringPercentage;
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Fishing
{
    internal struct Fishie
    {
        //old: internal Fishie(string fishName, string rodName, string i1, string i2, string i3, string i4)
        internal Fishie(string fishName, string rodName, string i1, string i2, string i3)
        {
            name = fishName;
            rod = rodName;
            ID1 = i1;
            ID2 = i2;
            ID3 = i3;
            //old: ID4 = i4;
        }

        //old: internal string name, rod, ID1, ID2, ID3, ID4;
        internal string name, rod, ID1, ID2, ID3;
        public override string ToString() { return name; }

    } // @ internal struct Fishie

    internal static class FishDB
    {
        #region Members

        private const string dbFolder = "FishDB";
        private static Dictionary<string, XmlDocument> DBByRod = new Dictionary<string, XmlDocument>();

        #endregion //Members

        #region Methods

        //old: internal static void AddNewFish(ref string fish, string zone, string bait, string rod, string ID1, string ID2, string ID3, string ID4, bool wanted)
        internal static void AddNewFish(ref string fish, string zone, string bait, string rod, string ID1, string ID2, string ID3, bool wanted)
        {
            XmlDocument xmlDoc = GetFishDB(rod);

            //generate non duplicate name if it is an unknown monster
            if("Monster" == fish)
            {
                int count = 1;
                fish = "Mob (_" + (count++).ToString() + "_)";

                while(xmlDoc.SelectSingleNode(string.Format("/Rod/Fish[@name=\"{1}\"]", rod, fish)) != null)
                {
                    fish = "Mob (_" + (count++).ToString() + "_)";
                }
            }

            //old: XmlNode fishNode = xmlDoc.SelectSingleNode(string.Format("/Rod/Fish[@name=\"{0}\"][@ID1=\"{1}\"][@ID2=\"{2}\"][@ID3=\"{3}\"][@ID4=\"{4}\"]", fish, ID1, ID2, ID3, ID4));
            XmlNode fishNode = xmlDoc.SelectSingleNode(string.Format("/Rod/Fish[@name=\"{0}\"][@ID1=\"{1}\"][@ID2=\"{2}\"][@ID3=\"{3}\"]", fish, ID1, ID2, ID3));

            if(null == fishNode)
            {
                fishNode = xmlDoc["Rod"].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Fish", xmlDoc.NamespaceURI));
                XmlAttribute fishName = fishNode.Attributes.Append(xmlDoc.CreateAttribute("name"));
                XmlAttribute fishWanted = fishNode.Attributes.Append(xmlDoc.CreateAttribute("wanted"));
                XmlAttribute ID1Node = fishNode.Attributes.Append(xmlDoc.CreateAttribute("ID1"));
                XmlAttribute ID2Node = fishNode.Attributes.Append(xmlDoc.CreateAttribute("ID2"));
                XmlAttribute ID3Node = fishNode.Attributes.Append(xmlDoc.CreateAttribute("ID3"));
                //XmlAttribute ID4Node = fishNode.Attributes.Append(xmlDoc.CreateAttribute("ID4"));
                fishName.Value = fish;
                fishWanted.Value = wanted ? "Yes" : "No";
                ID1Node.Value = ID1;
                ID2Node.Value = ID2;
                ID3Node.Value = ID3;
                //ID4Node.Value = ID4;

                fishNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Zones", xmlDoc.NamespaceURI));
                fishNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Baits", xmlDoc.NamespaceURI));
            }

            if(null != zone)
            {
                if(null == fishNode.SelectSingleNode("Zones/Zone[text()=\"" + zone + "\"]"))
                {
                    XmlNode zoneNode = fishNode["Zones"].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Zone", xmlDoc.NamespaceURI));
                    zoneNode.InnerText = zone;
                }
            }

            if(null != bait)
            {
                if(null == fishNode.SelectSingleNode("Baits/Bait[text()=\"" + bait + "\"]"))
                {
                    XmlNode baitNode = fishNode["Baits"].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Bait", xmlDoc.NamespaceURI));
                    baitNode.InnerText = bait;
                }
            }

            FishDBChanged(rod);

        } // @ internal static void AddNewFish(ref string fish, string zone, string bait, string rod, string ID1, string ID2, string ID3, string ID4, bool wanted)

        internal static void ChangeName(Fishie fish, string newName)
        {
            XmlDocument xmlDoc = GetFishDB(fish.rod);
            //old: string xpathQuery = string.Format("/Rod/Fish[@name=\"{0}\"][@ID1=\"{1}\"][@ID2=\"{2}\"][@ID3=\"{3}\"][@ID4=\"{4}\"]", fish.name, fish.ID1, fish.ID2, fish.ID3, fish.ID4);
            //old: string xpathOldQuery = string.Format("/Rod/Fish[@name=\"{0}\"][@ID1=\"{1}\"][@ID2=\"{2}\"][@ID3=\"{3}\"][@ID4=\"{4}\"]", newName, fish.ID1, fish.ID2, fish.ID3, fish.ID4);
            string xpathQuery = string.Format("/Rod/Fish[@name=\"{0}\"][@ID1=\"{1}\"][@ID2=\"{2}\"][@ID3=\"{3}\"]", fish.name, fish.ID1, fish.ID2, fish.ID3);
            string xpathOldQuery = string.Format("/Rod/Fish[@name=\"{0}\"][@ID1=\"{1}\"][@ID2=\"{2}\"][@ID3=\"{3}\"]", newName, fish.ID1, fish.ID2, fish.ID3);
            //check if there is already an entry with same ID and name = newName, if there is, merge the 2 entries
            XmlNode oldFishNode = xmlDoc.SelectSingleNode(xpathOldQuery);
            XmlNode fishNode = xmlDoc.SelectSingleNode(xpathQuery);

            if(null == oldFishNode)
            {
                fishNode.Attributes["name"].Value = newName;
                FishDBChanged(fish.rod);
            }
            else
            {
                //merging (union of Zones and Baits)
                XmlNodeList zones = fishNode["Zones"].ChildNodes;
                XmlNodeList baits = fishNode["Baits"].ChildNodes;

                foreach(XmlNode zone in zones)
                {
                    if(null == oldFishNode.SelectSingleNode("Zones/Zone[text()=\"" + zone.InnerText + "\"]"))
                    {
                        XmlNode zoneNode = oldFishNode["Zones"].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Zone", xmlDoc.NamespaceURI));
                        zoneNode.InnerText = zone.InnerText;
                    }
                }

                foreach(XmlNode bait in baits)
                {
                    if(null == oldFishNode.SelectSingleNode("Baits/Bait[text()=\"" + bait.InnerText + "\"]"))
                    {
                        XmlNode baitNode = oldFishNode["Baits"].AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Bait", xmlDoc.NamespaceURI));
                        baitNode.InnerText = bait.InnerText;
                    }
                }

                xmlDoc["Rod"].RemoveChild(fishNode);
                FishDBChanged(fish.rod);
            }

        } // @ internal static void ChangeName(Fishie fish, string newName)

        //old: internal static bool FishAccepted(out string name, out bool isNew, bool fishUnknown, string rod, string zone, string bait, string ID1, string ID2, string ID3, string ID4)
        internal static bool FishAccepted(out string name, out bool isNew, bool fishUnknown, string rod, string zone, string bait, string ID1, string ID2, string ID3)
        {
            XmlDocument xmlDoc = GetFishDB(rod);
            //old: string xpathQuery = string.Format("/Rod/Fish[@ID1=\"{0}\"][@ID2=\"{1}\"][@ID3=\"{2}\"][@ID4=\"{3}\"][Zones/Zone=\"{4}\"]", ID1, ID2, ID3, ID4, zone);
            string xpathQuery = string.Format("/Rod/Fish[@ID1=\"{0}\"][@ID2=\"{1}\"][@ID3=\"{2}\"][Zones/Zone=\"{3}\"]", ID1, ID2, ID3, zone);
            XmlNode fishNode = xmlDoc.SelectSingleNode(xpathQuery);

            if(null == fishNode)
            {
                name = "Unknown";
                isNew = true;

                return fishUnknown;
            }
            else
            {
                isNew = false;
                name = fishNode.Attributes["name"].Value;

                if(null == fishNode.SelectSingleNode("Baits/Bait[text()=\"" + bait + "\"]"))
                {
                    XmlNode newBaitNode = fishNode["Baits"].AppendChild(xmlDoc.CreateElement("Bait"));
                    newBaitNode.InnerText = bait;
                    FishDBChanged(rod);
                }

                return ("Yes" == fishNode.Attributes["wanted"].Value) ? true : false;
            }

        } // @ internal static bool FishAccepted(out string name, out bool isNew, bool fishUnknown, string rod, string zone, string bait, string ID1, string ID2, string ID3, string ID4)

        private static string GetFileName(string rod)
        {
            switch(rod)
            {
                case "Comp. Fishing Rod":
                    return dbFolder + @"\composite.xml";
                case "Bamboo Fish. Rod":
                    return dbFolder + @"\bamboo.xml";
                case "Carbon Fish. Rod":
                    return dbFolder + @"\carbon.xml";
                case "Ebisu Fishing Rod":
                    return dbFolder + @"\ebisu.xml";
                case "Clothespole":
                    return dbFolder + @"\clothespole.xml";
                case "Fastwater F. Rod":
                    return dbFolder + @"\fastwater.xml";
                case "Glass Fiber F. Rod":
                    return dbFolder + @"\glassfiber.xml";
                case "Halcyon Rod":
                    return dbFolder + @"\halcyon.xml";
                case "Hume Fishing Rod":
                    return dbFolder + @"\hume.xml";
                case "Lu Shang's F. Rod":
                    return dbFolder + @"\lushang.xml";
                case "MMM Fishing Rod":
                    return dbFolder + @"\mmm.xml";
                case "Mithran Fish. Rod":
                    return dbFolder + @"\mithran.xml";
                case "S.H. Fishing Rod":
                    return dbFolder + @"\singlehook.xml";
                case "Tarutaru F. Rod":
                    return dbFolder + @"\tarutaru.xml";
                case "Willow Fish. Rod":
                    return dbFolder + @"\willow.xml";
                case "Yew Fishing Rod":
                    return dbFolder + @"\yew.xml";
                default:
                    return null;
            }

        } // @ private static string GetFileName(string rod)

        private static XmlDocument GetFishDB(string rod)
        {
            if(!DBByRod.ContainsKey(rod))
            {
                //fish db for this rod is not loaded, check if the xml file is available, create if needed
                string fishDBFile = GetFileName(rod);

                if(!File.Exists(fishDBFile))
                {
                    //file does not exist, create and add the root node
                    if(!Directory.Exists(dbFolder))
                    {
                        Directory.CreateDirectory(dbFolder);
                    }

                    TextWriter writer = new StreamWriter(System.IO.File.Create(fishDBFile));

                    try
                    {
                        writer.WriteLine(string.Format("<Rod name=\"{0}\">\n</Rod>", rod));
                        writer.Flush();
                    }
                    finally
                    {
                        writer.Close();
                    }
                }

                //xml file is ready, load it into the dictionary
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fishDBFile);
                DBByRod.Add(rod, xmlDoc);
            }

            return DBByRod[rod];

        } // @ private static XmlDocument GetFishDB(string rod)

        internal static Fishie[] GetFishes(string rod, string zone, string bait, bool wanted)
        {
            XmlDocument xmlDoc = GetFishDB(rod);
            string xpathQuery = string.Format("/Rod/Fish[Zones/Zone=\"{0}\"][Baits/Bait=\"{1}\"][@wanted=\"{2}\"]", zone, bait, wanted ? "Yes" : "No");
            XmlNodeList nodes = xmlDoc.SelectNodes(xpathQuery);
            Fishie[] fishes = new Fishie[nodes.Count];
            int i = 0;

            foreach(XmlNode node in nodes)
            {
                //old: fishes[i++] = new Fishie(node.Attributes["name"].Value, rod, node.Attributes["ID1"].Value, node.Attributes["ID2"].Value, node.Attributes["ID3"].Value, node.Attributes["ID4"].Value);
                fishes[i++] = new Fishie(node.Attributes["name"].Value, rod, node.Attributes["ID1"].Value, node.Attributes["ID2"].Value, node.Attributes["ID3"].Value);
            }

            return fishes;

        } // @ internal static Fishie[] GetFishes(string rod, string zone, string bait, bool wanted)

        internal static void ToggleWanted(Fishie fish)
        {
            XmlDocument xmlDoc = GetFishDB(fish.rod);
            string xpathQuery = string.Format("/Rod/Fish[@name=\"{0}\"]", fish.name);
            XmlNode fishNode = xmlDoc.SelectSingleNode(xpathQuery);
            fishNode.Attributes["wanted"].Value = ("Yes" == fishNode.Attributes["wanted"].Value) ? "No" : "Yes";
            FishDBChanged(fish.rod);

        } // @ internal static void ToggleWanted(Fishie fish)

        #endregion //Methods

        #region Events_Custom

        internal delegate void DBChanged();
        internal static event DBChanged OnChanged;

        private static void FishDBChanged(string rod)
        {
            DBByRod[rod].Save(GetFileName(rod));

            if(null != OnChanged)
            {
                OnChanged();
            }

        } // @ private static void FishDBChanged(string rod)

        #endregion //Events_Custom

    } // @ internal static class FishDB
}

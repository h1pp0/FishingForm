using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using FFACETools;
using Timer = System.Windows.Forms.Timer;

namespace Fishing
{
    public static class Navigation
    {
        public static List<WaypointsXML> WaypointFatigueList { get; set; }
        public static List<WaypointsXML> WaypointListXML { get; set; }
        internal static FFACE _FFACE = FishingForm._FFACE;
        public static Timer WayPointTimer { get; set; }
        public static volatile bool StopRunningToWaypoint = false;
        public static bool isPathReversed = false;

        static Navigation()
        {
            
        }

        public enum WaypointListEnum
        {
            WaypointFatigueList,
            WaypointListXML
        }

        public static void CreateWaypointTimer()
        {
            _FFACE.Navigator.DistanceTolerance = 2f;
            WaypointListXML = new List<WaypointsXML> { new WaypointsXML { X = _FFACE.Player.Position.X, Y = _FFACE.Player.Position.Y, Z = _FFACE.Player.Position.Z, Zone = (int) _FFACE.Player.Zone } };

            WayPointTimer = new Timer { Interval = 250, Enabled = true };
            WayPointTimer.Tick += _waypointTimer_Tick;
            WayPointTimer.Start();
        }

        /// <summary>
        /// Stops the waypoint timer and resets the running status and presses key up.
        /// </summary>
        public static void StopWaypointTimer()
        {
            if (WayPointTimer != null)
            {
                WayPointTimer.Enabled = false;
                WayPointTimer.Tick -= _waypointTimer_Tick;
                WayPointTimer.Stop();
            }
            _FFACE.Navigator.Reset();
        }
       
        public static void RunToWaypointsFromFatigueXML (bool isfullinventoryselected)
        {
            if (WaypointFatigueList.Count > 0)
            {
                if (WaypointFatigueList.FirstOrDefault().Zone != (int) _FFACE.Player.Zone)
                {
                    MessageBox.Show("Wrong starting zone for current navi file.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (
                    _FFACE.Navigator.DistanceTo(WaypointFatigueList.FirstOrDefault().X,
                        WaypointFatigueList.FirstOrDefault().Z) > 20f)
                {
                    MessageBox.Show("First waypoint is over 20 yalms away.\nMove closer to the waypoint.",
                        "First Waypoint Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Run to waypoints from list
                RunWaypointsFromList(WaypointFatigueList);

                _FFACE.Navigator.Reset();
                if (isfullinventoryselected)
                    return;

                WaypointFatigueList.Reverse();
                if (isPathReversed)
                {
                    isPathReversed = false;
                    return;
                }
                Thread.Sleep(new Random().Next(2000, 5000));
                // Reverse waypoints and run again
                isPathReversed = true;
                RunToWaypointsFromFatigueXML(isfullinventoryselected);
            }
        }

        /// <summary>
        /// Code for running to a waypoint.
        /// </summary>
        /// <param name="waypointslist">Waypoint List</param>
        public static void RunWaypointsFromList (List<WaypointsXML> waypointslist)
        {
            foreach (var waypoint in waypointslist)
            {
                while (waypoint.Zone != (int)_FFACE.Player.Zone || _FFACE.Player.GetLoginStatus == LoginStatus.CharacterLoginScreen || _FFACE.Player.GetLoginStatus == LoginStatus.Loading)
                {
                    Thread.Sleep(100);
                }
                if (StopRunningToWaypoint)
                {
                    return;
                }
                _FFACE.Navigator.Goto(waypoint.X, waypoint.Y, waypoint.Z, true);
            }
            RunToExactLastPoint(waypointslist.LastOrDefault());
            _FFACE.Navigator.Reset();
        }

        private static void RunToExactLastPoint(WaypointsXML lastwaypoint)
        {
            _FFACE.Navigator.DistanceTolerance = 0.5f;
            _FFACE.Navigator.Goto(lastwaypoint.X, lastwaypoint.Y, lastwaypoint.Z, false);
            _FFACE.Navigator.DistanceTolerance = 3f;
        }

        static void _waypointTimer_Tick (object sender, EventArgs e)
        {
            if (_FFACE.Player.GetLoginStatus != LoginStatus.LoggedIn || (_FFACE.Player.PosX == 0 && _FFACE.Player.PosY == 0 && _FFACE.Player.PosZ == 0))
                return;

            if (WaypointListXML != null && ( _FFACE.Navigator.DistanceTo(WaypointListXML.LastOrDefault().X, WaypointListXML.LastOrDefault().Y, WaypointListXML.LastOrDefault().Z) > _FFACE.Navigator.DistanceTolerance))
                WaypointListXML.Add(new WaypointsXML { X = _FFACE.Player.PosX, Y =_FFACE.Player.PosY, Z = _FFACE.Player.PosZ, Zone = (int)_FFACE.Player.Zone});
        }
        
        /// <summary>
        /// Reads waypoints from the user selected Navi path file.
        /// </summary>
        /// <param name="waypointFatigueList">Waypoint list to reference</param>
        /// <returns>Number of entries in the Navi path file</returns>
        public static int ReadWaypointsFromXML (WaypointListEnum waypointFatigueList)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    var xdoc = XDocument.Load(ofd.FileName, LoadOptions.None);

                    var WaypointField = new List<WaypointsXML>();
                    foreach (var waypoint in xdoc.Descendants("Path").Descendants())
                    {
                        if (waypoint.Attribute("x") != null)
                        {
                            WaypointsXML _position = new WaypointsXML();
                            _position.X = float.Parse(waypoint.Attribute("x").Value);
                            _position.Y = float.Parse(waypoint.Attribute("y").Value);
                            _position.Z = float.Parse(waypoint.Attribute("z").Value);
                            _position.Zone = Int32.Parse(waypoint.Attribute("zone").Value);
                            WaypointField.Add(_position);
                        }
                    }
                    if (waypointFatigueList == WaypointListEnum.WaypointFatigueList)
                        WaypointFatigueList = WaypointField;
                    else if (waypointFatigueList == WaypointListEnum.WaypointListXML)
                        WaypointListXML = WaypointField;

                    return WaypointField.Count;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    throw;
                }
            }
            return 0;
        }

        public static void WriteWaypointsToXML(string fileName)
        {
            if (WaypointListXML == null)
                return;

            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
            XmlNode _mainnode = doc.CreateElement("Path");
            doc.AppendChild(_mainnode);

            int previouszone = WaypointListXML.FirstOrDefault().Zone;
            foreach (var waypoints in WaypointListXML)
            {
                string waypointtype;
                int zoneid = waypoints.Zone;

                if (previouszone != zoneid)
                {
                    XmlNode _zonenode =  doc.CreateElement("Waypoint");
                    waypointtype = "1";
                    XmlAttribute _waypointstype = doc.CreateAttribute("type");
                    _waypointstype.Value = waypointtype;
                    _zonenode.Attributes.Append(_waypointstype);
                    _mainnode.AppendChild(_zonenode);
                    // Reset the type
                    waypointtype = "0";
                    previouszone = zoneid;
                }
                else
                    waypointtype = "0";
                try
                {

                    XmlNode _newentry = doc.CreateElement("Waypoint");

                    XmlAttribute _waypointstype = doc.CreateAttribute("type");
                    _waypointstype.Value = waypointtype;
                    _newentry.Attributes.Append(_waypointstype);


                    XmlAttribute _zone = doc.CreateAttribute("zone");
                    _zone.Value = waypoints.Zone.ToString();
                    _newentry.Attributes.Append(_zone);

                    XmlAttribute _x = doc.CreateAttribute("x");
                    _x.Value = waypoints.X.ToString();
                    _newentry.Attributes.Append(_x);

                    XmlAttribute _y = doc.CreateAttribute("y");
                    _y.Value = waypoints.Y.ToString();
                    _newentry.Attributes.Append(_y);

                    XmlAttribute _z = doc.CreateAttribute("z");
                    _z.Value = waypoints.Z.ToString();
                    _newentry.Attributes.Append(_z);

                    _mainnode.AppendChild(_newentry);
                }
                catch (System.Exception ex)
                {
                    throw;
                }
            }
            doc.Save(fileName);
        }
    }

    public class WaypointsXML
    {
        public Single X { get; set; }
        public Single Y { get; set; }
        public Single Z { get; set; }
        public Int32 Zone { get; set; }

        public void Waypoints (Single x, Single y, Single z, Int32 zone)
        {
            X = x;
            Y = y;
            Z = z;
            Zone = zone;
        }
    }
}
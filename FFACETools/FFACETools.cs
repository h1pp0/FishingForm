﻿using System;
using System.Runtime.InteropServices;

namespace FFACETools
{
    public partial class FFACE
    {
        #region Constants

        /// <summary>
        /// Message for not implimented exceptions
        /// </summary>
        private const string NEED_v410_24_OR_HIGHER = "FFACETools requires FFACE v4.1.0.24 or higher";

        /// <summary>
        /// Name of the FFACE library
        /// </summary>
        private const string FFACE_LIBRARY = "FFACE.dll";

        #endregion

        #region Members

        /// <summary>
        /// The path to Windower's auto-generated resources.xml
        /// </summary>
        public static string WindowerPath { get; set; }

        /// <summary>
        /// Instance ID generated by FFACE
        /// </summary>
        public int _InstanceID { get; set; }

        /// <summary>
        /// Information about the current player
        /// </summary>
        public PlayerTools Player { get; set; }

        /// <summary>
        /// Information about the player's target
        /// </summary>
        public TargetTools Target { get; set; }

        /// <summary>
        /// Information relating to Party/Alliance
        /// </summary>
        public PartyTools Party { get; set; }

        /// <summary>
        /// Information relating to FFXI Dat Files.
        /// </summary>
        private ParseResources Resources { get; set; }

        /// <summary>
        /// Information about party members
        /// </summary>
        public System.Collections.Generic.Dictionary<byte, PartyMemberTools> PartyMember { get; set; }

        /// <summary>
        /// Information about FFACE timers
        /// </summary>
        public TimerTools Timer { get; set; }

        /// <summary>
        /// Information about the chat system
        /// </summary>
        public ChatTools Chat { get; set; }

        /// <summary>
        /// Information about fishing
        /// </summary>
        public FishTools Fish { get; set; }

        /// <summary>
        /// Information about items
        /// </summary>
        public ItemTools Item { get; set; }

        /// <summary>
        /// Informatin about NPC
        /// </summary>
        public NPCTools NPC { get; set; }

        /// <summary>
        /// Information about menus
        /// </summary>
        public MenuTools Menu { get; set; }

        /// <summary>
        /// Information about search results
        /// </summary>
        public SearchTools Search { get; set; }

        /// <summary>
        /// Link to sending information to windower
        /// </summary>
        public WindowerTools Windower { get; set; }

        /// <summary>
        /// Pyrolol's navigation system
        /// </summary>
        public NavigatorTools Navigator { get; set; }

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Constructor that instantiates FFACE
        /// </summary>
        /// <param name="processID">The Process ID of the POL Process you want to interface with.</param>
        public FFACE (int processID)
        {
            // create our FFACE instance
            _InstanceID = CreateInstance((UInt32)processID);

            #region Find Windower Plugin Path

            System.Diagnostics.Process[] Processes = System.Diagnostics.Process.GetProcessesByName("pol");
            if (Processes.Length > 0)
                foreach (System.Diagnostics.ProcessModule mod in Processes[0].Modules)
                {
                    if (mod.ModuleName.ToLower() == "hook.dll")
                    {
                        WindowerPath = mod.FileName.Substring(0, mod.FileName.Length - 8) + @"\plugins\";
                        ParseResources.UseFFXIDatFiles = false;
                        break;
                    }
                }
            // Fix for non-windower users
            if (String.IsNullOrEmpty(WindowerPath))
            {
                string ExePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                // If we have a resources folder, use the resource parser otherwise let it default to using .dat reader
                if (System.IO.Directory.Exists(ExePath + @"\resources\"))
                {
                    WindowerPath = ExePath;
                    ParseResources.UseFFXIDatFiles = false;
                }
            }

            #endregion

            // Find out if we should be using structs or not
            System.Diagnostics.FileVersionInfo fileInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(FFACE_LIBRARY);

            // Need 4, 1, 0, 14 or later.  Adjust these settings as needed.
            UInt64 version = ( (UInt64)fileInfo.FileMajorPart << 48 ) + ( (UInt64)fileInfo.FileMinorPart << 32 ) + ( (UInt64)fileInfo.FileBuildPart << 16 ) + (UInt64)fileInfo.FilePrivatePart;
            if (fileInfo.FileMajorPart != 4)
                throw new Exception(NEED_v410_24_OR_HIGHER);
            else if (version < 0x0004000100000018UL)			// 0004 0001 0000 000E (4, 1, 0, 14) // // 0004 0001 0000 0018 (4, 1, 0, 24)
                throw new Exception(NEED_v410_24_OR_HIGHER);

            /*if (fileInfo.FileMajorPart != 4)
                throw new Exception(NEED_v410_24_OR_HIGHER);
            else if (fileInfo.FileMinorPart < 1)
                throw new Exception(NEED_v410_24_OR_HIGHER);
            else if (fileInfo.FileBuildPart < 0)
                throw new Exception(NEED_v410_24_OR_HIGHER);
            else if (fileInfo.FilePrivatePart < 14)
                throw new Exception(NEED_v410_24_OR_HIGHER);*/


            // instantiate our classes
            Player = new PlayerTools(_InstanceID);
            Target = new TargetTools(_InstanceID);
            Party = new PartyTools(_InstanceID);
            Fish = new FishTools(_InstanceID);
            Windower = new WindowerTools(_InstanceID);
            Timer = new TimerTools(_InstanceID);
            Chat = new ChatTools(_InstanceID);
            Item = new ItemTools(this);
            NPC = new NPCTools(_InstanceID);
            Menu = new MenuTools(this);
            Search = new SearchTools(_InstanceID);
            Navigator = new NavigatorTools(this);
            Resources = ParseResources.Instance;

            #region Party Members

            // instantiate our party members
            PartyMember = new System.Collections.Generic.Dictionary<byte, PartyMemberTools>();
            PartyMember.Add(0, new PartyMemberTools(_InstanceID, 0));
            PartyMember.Add(1, new PartyMemberTools(_InstanceID, 1));
            PartyMember.Add(2, new PartyMemberTools(_InstanceID, 2));
            PartyMember.Add(3, new PartyMemberTools(_InstanceID, 3));
            PartyMember.Add(4, new PartyMemberTools(_InstanceID, 4));
            PartyMember.Add(5, new PartyMemberTools(_InstanceID, 5));
            PartyMember.Add(6, new PartyMemberTools(_InstanceID, 6));
            PartyMember.Add(7, new PartyMemberTools(_InstanceID, 7));
            PartyMember.Add(8, new PartyMemberTools(_InstanceID, 8));
            PartyMember.Add(9, new PartyMemberTools(_InstanceID, 9));
            PartyMember.Add(10, new PartyMemberTools(_InstanceID, 10));
            PartyMember.Add(11, new PartyMemberTools(_InstanceID, 11));
            PartyMember.Add(12, new PartyMemberTools(_InstanceID, 12));
            PartyMember.Add(13, new PartyMemberTools(_InstanceID, 13));
            PartyMember.Add(14, new PartyMemberTools(_InstanceID, 14));
            PartyMember.Add(15, new PartyMemberTools(_InstanceID, 15));
            PartyMember.Add(16, new PartyMemberTools(_InstanceID, 16));
            PartyMember.Add(17, new PartyMemberTools(_InstanceID, 17));

            #endregion

        } // @ public FFACEWrapper(uint processID)

        /// <summary>
        /// Destructor
        /// </summary>
        ~FFACE ()
        {
            if (_InstanceID != 0)
            {
                try
                {
                    DeleteInstance(_InstanceID);
                }
                catch
                {
                }
                try
                {
                    ParseResources.DeleteInstance();
                }
                catch
                {
                }
                _InstanceID = 0;
            }
        } // @ ~FFACEWrapper()

        #endregion

        #region Methods

        internal static bool IsSet (UInt32 value, UInt32 bit)
        {
            if (value == bit)
                return true;
            return ( ( value & bit ) != 0 ); // generic, means i don't have to be exact on settings.
        } // @ internal static bool IsSet(UInt32 value, UInt32 bit)

        internal static bool IsSet (LineSettings value, LineSettings bit)
        {
            if (value == bit)
                return true;
            return ( ( (UInt32)value & (UInt32)bit ) != 0 );
        } // @ internal static bool IsSet(LineSettings value, LineSettings bit)

        internal static bool IsSet (InventoryType value, InventoryType bit)
        {
            if (value == bit)
                return true;

            return ( ( (UInt32)value & (UInt32)bit ) != 0 );
        }

        #endregion

    } // @ public class FFACE
}
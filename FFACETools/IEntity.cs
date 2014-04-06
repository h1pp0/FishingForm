/**
 * IEntity.cs - Managed IEntity implementation.
 * -----------------------------------------------------------------------
 *
 *		This file is part of Ashita.
 *
 *		Ashita is free software: you can redistribute it and/or modify
 *		it under the terms of the GNU Lesser General Public License as published by
 *		the Free Software Foundation, either version 3 of the License, or
 *		(at your option) any later version.
 *
 *		Ashita is distributed in the hope that it will be useful,
 *		but WITHOUT ANY WARRANTY; without even the implied warranty of
 *		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *		GNU Lesser General Public License for more details.
 *
 *		You should have received a copy of the GNU Lesser General Public License
 *		along with Ashita.  If not, see <http://www.gnu.org/licenses/>.
 *
 */

namespace FFACETools
{
    using System;
    using System.Runtime.InteropServices;


    public enum GMFlags : byte
    {
        GM = 0x20,
        SGM = 0x28,
        LGM = 0x30,
        SEProducer = 0x38
    }

    /// <summary>
    /// Entity position structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MENTITYPOSITION
    {
        public uint pUnknown;
        public float X, Y, Z;
        public uint Unknown0;
        public uint Unknown1;
        public float H;
        public uint Unknown2;
    }

    /// <summary>
    /// Entity position move structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MENTITYPOSITIONMOVE
    {
        public uint pUnknown;
        public float X, Y, Z;
        public uint Unknown0;
        public float MoveX, MoveY, MoveZ; //may not be right but i don't see anyone using it anyway.
    }

    /// <summary>
    /// Entity visual data structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MENTITYLOOK
    {
        public ushort Face;
        public ushort Head, Body, Hands, Legs, Feet, Main, Sub, Ranged;
    }

    /// <summary>
    /// Main Entity structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MEntity
    {
        public MENTITYPOSITION LocalPosition;
        public MENTITYPOSITION LastPosition;
        public MENTITYPOSITIONMOVE Move;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)] public byte[] NoIdea;
        public uint Index;
        public uint ID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)] public String Name;
        public float unknown001;
        public float MovementSpeed;
        public float AnimationSpeed;
        public uint Warp;
        public uint unknown0_0001; // new 03/27/2013
        public uint unknown0;
        public uint NPCTalking;
        public float Distance; //^2
        public uint unknown1;
        public uint unknown2;
        public float Heading2;
        public uint Owner; //Jug pets only
        public uint TP; //10
        public byte HPP;
        public byte unknown3;
        public byte unknown4;
        public byte Type;
        public ushort Race;
        public ushort unknown05;
        public uint unknown5;
        public uint unknown6;
        public ushort unknown7;
        public ushort unknown08;
        public MENTITYLOOK Look;
        public ushort unknown8;
        public uint unknown9;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)] public byte[] unknown10;
        public byte MobType;
        public byte u32;
        public byte Render;
        public byte Flags2;
        public byte Flags3;
        public byte Flags4;
        public byte Flags5;
        public byte Flags6;
        public byte Flags7;
        public byte Flags8;
        public byte GMFlag;
        public byte Flags10;
        public byte Flags11;
        public byte Flags12;
        public byte Flags13;
        public byte Flags14;
        public byte Flags15;
        public byte Flags16;
        public byte Flags17;
        public byte Flags18;
        public byte Flags19;
        public byte Flags20;
        public byte Flags21;
        public byte Flags22;
        public byte Flags23;
        public byte Flags24;
        public uint unknown12;
        public int Unknown0002;
        public ushort unknown13;
        public ushort NPCSpeechLoop;
        public ushort NPCSpeechFrame;
        public ushort unknown14;
        public uint unknown15;
        public uint unknown16;
        public float MovementSpeed1;
        public ushort NPCWalkPos1;
        public ushort NPCWalkPos2;
        public ushort NPCWalkMode;
        public ushort unknown17;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] mou4; //always this.
        public uint Status; // 01 = aggro
        public uint StatusSvr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)] public byte[] unknown18;
        public uint u45;
        public uint u46;
        public uint ClaimID;
        public uint u47;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] Animation1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] Animation2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] Animation3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] Animation4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] Animation5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] Animation6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] Animation7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] Animation8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] Animation9;
        public ushort AnimationTime; //guessed, but something to do with the current animation
        public ushort AnimationStep; //guessed, but something to do with the current animation
        public ushort u48;
        public ushort u49;
        public uint EmoteID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] EmoteName;
        public byte SpawnType; //0x01 PC, 0x02 NPC, 0x10 MOB, 0x0D Me
        public byte u50;
        public ushort u51;
        public byte LSColorRed;
        public byte LSColorGreen;
        public byte LSColorBlue;
        public byte u52;
        public byte u53;
        public byte u54;
        public byte CampaignMode; //boolean value. 
        public byte u55;
        public byte u56;
        public byte u57;
        public ushort u58;
        public ushort u59;
        public ushort u60;
        public uint timer; //counts down durring fishing, sometimes goes 0xffffffff
        public ushort u63;
        public ushort u64;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] TalkAnimation;
        public ushort u65; // 5/13/13 update
        //public uint u66;
        public ushort PetIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public byte[] u68;
        public float ModelSize;
    }
}
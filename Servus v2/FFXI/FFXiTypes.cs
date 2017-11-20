using System.Drawing;
using System.Runtime.InteropServices;

namespace Servus_v2.FFXi
{
    public static class FFXiTypes
    {
        /// <summary>
        /// Item type enumeration.
        /// </summary>
        public enum ItemsType : short
        {
            Nothing = 0x0000,
            Item = 0x0001,
            QuestItem = 0x0002,
            Fish = 0x0003,
            Weapon = 0x0004,
            Armor = 0x0005,
            Linkshell = 0x0006,
            UsableItem = 0x0007,
            Crystal = 0x0008,
            Currency = 0x0009,
            Furnishing = 0x000A,
            Plant = 0x000B,
            Flowerpot = 0x000C,
            PuppetItem = 0x000D,
            Mannequin = 0x000E,
            Book = 0x000F,
            RacingForm = 0x0010,
            BettingSlip = 0x0011,
            SoulPlate = 0x0012,
            Reflector = 0x0013,
            Logs = 0x0014,
            LotteryTicket = 0x0015,
            TabulaM = 0x0016,
            TabulaR = 0x0017,
            Voucher = 0x0018,
            Rune = 0x0019,
            Evolith = 0x001A,
            StorageSlip = 0x001B,
            Type1 = 0x001C,
        };

        /// <summary>
        /// Armor item entry.
        /// </summary>
        public struct ItemArmor
        {
            public byte CastTime;
            public ushort Flags;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x96A)]
            public byte[] Image;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] ImageName;

            public uint ImageSize;
            public byte ImageType;
            public uint ItemId;
            public ushort ItemType;
            public uint Jobs;
            public ushort Level;
            public byte MaxCharges;
            public ushort Races;
            public ushort ResourceId;
            public uint ReuseDelay;
            public ushort ShieldSize;
            public ushort Slot;
            public ushort StackSize;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x258)]
            public byte[] StringData;

            public ushort Unknown0;
            public uint Unknown1;
            public ushort UseDelay;
            public ushort ValidTargets;
        }

        /// <summary>
        /// Common item entry.
        /// </summary>
        public struct ItemCommon
        {
            public ushort Flags;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x96A)]
            public byte[] Image;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] ImageName;

            public uint ImageSize;
            public byte ImageType;
            public uint ItemId;
            public ushort ItemType;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x1B)]
            public byte[] Padding;

            public ushort ResourceId;
            public ushort StackSize;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x250)]
            public byte[] StringData;

            public uint Unknown0;
            public ushort ValidTargets;

            public ushort Var1;

            public uint Var2;
        }

        /// <summary>
        /// Currency item entry.
        /// </summary>
        public struct ItemCurrency
        {
            public ushort Flags;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x96A)]
            public byte[] Image;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] ImageName;

            public uint ImageSize;
            public byte ImageType;
            public uint ItemId;
            public ushort ItemType;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x1B)]
            public byte[] Padding;

            public ushort ResourceId;
            public ushort StackSize;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x250)]
            public byte[] StringData;

            public ushort ValidTargets;

            public ushort Var1;
        }

        /// <summary>
        /// Instinct item entry.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ItemInstinct
        {
            public uint ItemId;

            public ushort Flags;

            public ushort StackSize;

            public ushort ItemType;

            public ushort ResourceID;

            public ushort ValidTargets;

            public uint Unknown1;

            public uint Unknown2;

            public ushort Unknown3;

            public ushort InstinctCost;

            public ushort Unknown4;

            public uint Unknown5;

            public uint Unknown6;

            public uint Unknown7;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x251)]
            public byte[] StringData;

            public uint ImageSize;

            public byte ImageType;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] ImageName;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x96A)]
            public byte[] Image;
        }

        /// <summary>
        /// Monipulator item entry.
        /// </summary>
        public struct ItemMonipulator
        {
            public ushort Flags;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x96A)]
            public byte[] Image;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] ImageName;

            public uint ImageSize;
            public byte ImageType;
            public uint ItemId;
            public ushort ItemType;
            public ushort ResourceId;
            public ushort StackSize;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x251)]
            public byte[] StringData;

            public ushort Unknown1;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x18)]
            public int[] Unknown2;

            public ushort ValidTargets;
        }

        /// <summary>
        /// Puppet item entry.
        /// </summary>
        public struct ItemPuppet
        {
            public uint Elements;
            public ushort Flags;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x96A)]
            public byte[] Image;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] ImageName;

            public uint ImageSize;
            public byte ImageType;
            public uint ItemId;
            public ushort ItemType;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x17)]
            public byte[] Padding;

            public ushort ResourceId;
            public ushort Slot;
            public ushort StackSize;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x250)]
            public byte[] StringData;

            public uint Unknown0;
            public ushort ValidTargets;
        }

        /// <summary>
        /// Slip item entry.
        /// </summary>
        public struct ItemSlip
        {
            public ushort Flags;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x96A)]
            public byte[] Image;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] ImageName;

            public uint ImageSize;
            public byte ImageType;
            public uint ItemId;
            public ushort ItemType;
            public ushort ResourceId;
            public ushort StackSize;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x250)]
            public byte[] StringData;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x11)]
            public int[] Unknown0;

            public ushort ValidTargets;

            public ushort Var1;
        }

        /// <summary>
        /// Weapon item entry.
        /// </summary>
        public struct ItemWeapon
        {
            public byte CastTime;
            public ushort Damage;
            public ushort Delay;
            public ushort Dps;
            public ushort Flags;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x96A)]
            public byte[] Image;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] ImageName;

            public uint ImageSize;
            public byte ImageType;
            public uint ItemId;
            public ushort ItemType;
            public uint Jobs;
            public byte JugSize;
            public ushort Level;
            public byte MaxCharges;
            public ushort Races;
            public ushort ResourceId;
            public uint ReuseDelay;
            public byte Skill;
            public ushort Slot;
            public ushort StackSize;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x251)]
            public byte[] StringData;

            public uint Unknown0;
            public uint Unknown1;
            public ushort UseDelay;
            public ushort ValidTargets;
        }

        /// <summary>
        /// Item wrapper to uniform all item types into one.
        /// </summary>
        public class ItemWrapper
        {
            public Bitmap Bitmap { get; set; }
            public byte CastTime { get; set; }
            public ushort Damage { get; set; }
            public ushort Delay { get; set; }
            public string Description { get; set; }
            public ushort Dps { get; set; }
            public uint Elements { get; set; }
            public ushort Flags { get; set; }
            public string ImageName { get; set; }
            public uint ImageSize { get; set; }
            public byte ImageType { get; set; }
            public uint ItemId { get; set; }
            public ushort ItemType { get; set; }
            public uint Jobs { get; set; }
            public byte JugSize { get; set; }
            public ushort Level { get; set; }
            public string LogNamePlural { get; set; }
            public string LogNameSingular { get; set; }
            public byte MaxCharges { get; set; }
            public string Name { get; set; }
            public ushort Races { get; set; }
            public ushort ResourceId { get; set; }
            public uint ReuseDelay { get; set; }
            public ushort ShieldSize { get; set; }
            public byte Skill { get; set; }
            public ushort Slot { get; set; }
            public ushort StackSize { get; set; }
            public ushort UseDelay { get; set; }
            public ushort ValidTargets { get; set; }
            public ushort Var1 { get; set; }

            public uint Var2 { get; set; }
        }
    }
}
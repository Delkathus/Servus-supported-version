using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Gambits.Model.FFXi
{
    public class FFXiInfo
    {
        /// <summary>
        /// Gets the instance of this class.
        /// </summary>
        public static FFXiInfo Instance
        {
            get
            {
                if (FFXiInfoInstance != null)
                {
                    return FFXiInfoInstance;
                }

                FFXiInfoInstance = new FFXiInfo();

                var itemFiles = new List<int> { 73, 74, 75, 76, 77, 91, 55667, 55668, 55669, 55670, 55671 };

                foreach (var item in itemFiles)
                {
                    var timer = new Stopwatch();
                    timer.Start();
                    Instance.ParseItemFile(item);
                    timer.Stop();

                    Debug.WriteLine("Took {0} to read {1}", timer.Elapsed.TotalSeconds, item);
                }

                Debug.WriteLine("Total items read: " + Instance.Items.Count);

                foreach (var item in FFXiInfoInstance.Items.Where(kvp => string.IsNullOrEmpty(kvp.Value.Name) || kvp.Value.Name == ".").ToList())
                {
                    FFXiInfoInstance.Items.Remove(item.Key);
                }

                FFXiInfoInstance.ItemList = Instance.Items.Values.Where(i => i.Name != "." && !string.IsNullOrEmpty(i.Name)).ToList();

                FFXiInfoInstance.ItemNames = Instance.Items.Values.OrderBy(i => i.Name)
                    .GroupBy(i => i.Name)
                    .Select(g => g.First().Name)
                    .ToList();
                return FFXiInfoInstance;
            }
        }

        public Dictionary<uint, FFXiTypes.ItemWrapper> Items
        {
            get { return _itemCache; }
        }

        public List<FFXiTypes.ItemWrapper> ItemList { get; set; }

        public List<string> ItemNames { get; set; }

        #region Private

        /// <summary>
        /// Private Constructor
        /// </summary>
        private FFXiInfo()
        {
            // Obtain the FFXi install path..
            var ffxiPath = GetFFXiInstallPath();

            if (string.IsNullOrEmpty(ffxiPath))
                throw new ArgumentException("Could not find registry key: PlayOnline/PlayOnlineUS/PlayOnlineEU");

            // Read the FTABLE.DAT file..
            using (var fStream = new FileStream(Path.Combine(ffxiPath, "FTABLE.DAT"), FileMode.Open, FileAccess.Read))
            {
                _fTable = new ushort[fStream.Length / 2];

                var index = -1;
                var buffer = new byte[2];
                while (fStream.Read(buffer, 0, 2) == 2)
                {
                    _fTable[++index] = BitConverter.ToUInt16(buffer, 0);
                }
            }

            // Read the VTABLE.DAT file..
            using (var fStream = new FileStream(Path.Combine(ffxiPath, "VTABLE.DAT"), FileMode.Open, FileAccess.Read))
            {
                _vTable = new byte[fStream.Length];
                fStream.Read(_vTable, 0, (int)fStream.Length);
            }
        }

        /// <summary>
        /// The internal instance of this class.
        /// </summary>
        public static FFXiInfo FFXiInfoInstance;

        /// <summary>
        /// The loaded item cache.
        /// </summary>
        private readonly Dictionary<uint, FFXiTypes.ItemWrapper> _itemCache = new Dictionary<uint, FFXiTypes.ItemWrapper>();

        /// <summary>
        /// The FTable file data that is loaded when this class is first used.
        /// </summary>
        private readonly ushort[] _fTable;

        /// <summary>
        /// The VTable file data that is loaded when this class is first used.
        /// </summary>
        private readonly byte[] _vTable;

        /// <summary>
        /// Gets a value from the system registry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyName"></param>
        /// <param name="valueName"></param>
        /// <returns></returns>
        private static T GetValue<T>(string keyName, string valueName)
        {
            try
            {
                return (T)Registry.GetValue(keyName, valueName, default(T));
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Gets the installation path of Final Fantasy XI.
        /// </summary>
        /// <returns></returns>
        private static string GetFFXiInstallPath()
        {
            var polRegKeys = new[] { "PlayOnline", "PlayOnlineUS", "PlayOnlineEU" };

            foreach (var result in polRegKeys.Select(key => GetValue<string>(string.Format("HKEY_LOCAL_MACHINE\\SOFTWARE\\{0}\\InstallFolder", key), "0001")).Where(result => !string.IsNullOrWhiteSpace(result)))
                return result;
            return string.Empty;
        }

        /// <summary>
        /// Converts a file ID to its full path.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string GetFilePath(int file)
        {
            if (_fTable == null || _vTable == null || _vTable[file] == 0x00)
                return string.Empty;

            var fileLocation = _fTable[file];
            return string.Format("{0}\\ROM\\{1}\\{2}.DAT", GetFFXiInstallPath(), fileLocation >> 7, fileLocation & 0x007F);
        }

        /// <summary>
        /// Generates a bitmap image from the given item data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Bitmap GenerateBitmap(byte[] data)
        {
            MemoryStream mStream = null;

            try
            {
                // Build Bitmap Header..
                var bitmapData = new List<byte>();
                bitmapData.AddRange(new byte[] { 0x42, 0x4D });
                bitmapData.AddRange(BitConverter.GetBytes(32 * 32));
                bitmapData.AddRange(BitConverter.GetBytes((short)0));
                bitmapData.AddRange(BitConverter.GetBytes((short)0));
                bitmapData.AddRange(BitConverter.GetBytes(0));

                // Pull bitmap info from data..
                var buffer = new byte[data.Length - 0x295];
                Array.Copy(data, 0x295, buffer, 0, buffer.Length);
                bitmapData.AddRange(buffer);

                // Create the bitmap..
                mStream = new MemoryStream(bitmapData.ToArray());
                var bmp = new Bitmap(mStream);
                bmp.MakeTransparent(Color.FromArgb(0, 0, 0, 0));
                return bmp;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (mStream != null)
                    mStream.Dispose();
            }
        }

        /// <summary>
        /// Converts a byte array to a structure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static T ArrayToStruct<T>(byte[] data) where T : struct
        {
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var ret = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return ret;
        }

        /// <summary>
        /// Rotates the given byte by the given size.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static byte Rotate(byte b, int size)
        {
            return (byte)((b >> size) | (b << (8 - size)));
        }

        /// <summary>
        /// Parses the given item dat file.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public void ParseItemFile(int fileId)
        {
            var filePath = GetFilePath(fileId);
            Debug.WriteLine(filePath);
            using (var fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Find the item count..
                var itemCount = fStream.Length / 0xC00;

                // Read the file into memory..
                var buffer = new byte[fStream.Length];
                fStream.Read(buffer, 0, (int)fStream.Length);

                // Rotate the file data.. (decoding)
                for (var x = 0; x < buffer.Length; x++)
                    buffer[x] = Rotate(buffer[x], 5);

                //File.WriteAllBytes("derp.dat", buffer);

                // Loop and process each item..
                for (var x = 0; x < itemCount; x++)
                {
                    // Pull the item bytes..
                    var itemBuffer = new byte[0xC00];
                    Array.Copy(buffer, x * 0xC00, itemBuffer, 0, 0xC00);

                    // Use the item id to determine the type..
                    var itemId = BitConverter.ToUInt32(itemBuffer, 0);
                    if (itemId == 0xFFFF)
                        ParseCurrency(itemBuffer);
                    else if (itemId < 0x1000)
                        ParseCommonItem(itemBuffer);
                    else if (itemId < 0x2000)
                        ParseCommonItem(itemBuffer);
                    else if (itemId < 0x2800)
                        ParsePuppet(itemBuffer);
                    else if (itemId < 0x4000)
                        ParseArmor(itemBuffer);
                    else if (itemId < 0x4000)
                        ParseArmor(itemBuffer);
                    else if (itemId < 0x6000)
                        ParseWeapon(itemBuffer);
                    else if (itemId < 0x7000)
                        ParseArmor(itemBuffer);
                    else if (itemId < 0x7400)
                        ParseSlip(itemBuffer);
                    else if (itemId < 0x7800)
                        ParseInstinct(itemBuffer);
                    else if (itemId < 0xF200)
                        ParseMonipulator(itemBuffer);
                    else
                        Debug.WriteLine("FAILED TO PARSE AN ITEM! {0}", BitConverter.ToUInt16(itemBuffer, 0x08));
                }
            }
        }

        /// <summary>
        /// Parses an armor item entry.
        /// </summary>
        /// <param name="data"></param>
        private void ParseArmor(byte[] data)
        {
            // Convert the item to an armor structure entry..
            var item = ArrayToStruct<FFXiTypes.ItemArmor>(data);
            if (item.ItemId == 0)
                return;

            // Build the item wrapper..
            var wrap = new FFXiTypes.ItemWrapper
            {
                ItemId = item.ItemId,
                Flags = item.Flags,
                StackSize = item.StackSize,
                ItemType = item.ItemType,
                ResourceId = item.ResourceId,
                ValidTargets = item.ValidTargets,
                Level = item.Level,
                Slot = item.Slot,
                Races = item.Races,
                Jobs = item.Jobs,
                ShieldSize = item.ShieldSize,
                MaxCharges = item.MaxCharges,
                CastTime = item.CastTime,
                UseDelay = item.UseDelay,
                ReuseDelay = item.ReuseDelay,
                ImageSize = item.ImageSize,
                ImageType = item.ImageType,

                ImageName = Encoding.ASCII.GetString(item.ImageName.Take(8).ToArray()).Trim(),
                Bitmap = GenerateBitmap(data)
            };

            ParseStrings(wrap, item.StringData);

            // Add the item to the cache..
            _itemCache[item.ItemId] = wrap;
        }

        /// <summary>
        /// Parses a common item entry.
        /// </summary>
        /// <param name="data"></param>
        private void ParseCommonItem(byte[] data)
        {
            // Convert the item to a common item structure entry..
            var item = ArrayToStruct<FFXiTypes.ItemCommon>(data);
            if (item.ItemId == 0)
                return;

            // Build the item wrapper..
            var wrap = new FFXiTypes.ItemWrapper
            {
                ItemId = item.ItemId,
                Flags = item.Flags,
                StackSize = item.StackSize,
                ItemType = item.ItemType,
                ResourceId = item.ResourceId,
                ValidTargets = item.ValidTargets,
                Var1 = item.Var1,
                Var2 = item.Var2,
                ImageSize = item.ImageSize,
                ImageType = item.ImageType,

                ImageName = Encoding.ASCII.GetString(item.ImageName.Take(8).ToArray()).Trim(),
                Bitmap = GenerateBitmap(data)
            };

            ParseStrings(wrap, item.StringData);

            // Add the item to the cache..
            _itemCache[item.ItemId] = wrap;
        }

        /// <summary>
        /// Parses a currency item entry.
        /// </summary>
        /// <param name="data"></param>
        private void ParseCurrency(byte[] data)
        {
            // Convert the item to a currency structure entry..
            var item = ArrayToStruct<FFXiTypes.ItemCurrency>(data);
            if (item.ItemId == 0)
                return;

            // Build the item wrapper..
            var wrap = new FFXiTypes.ItemWrapper
            {
                ItemId = item.ItemId,
                Flags = item.Flags,
                StackSize = item.StackSize,
                ItemType = item.ItemType,
                ResourceId = item.ResourceId,
                ValidTargets = item.ValidTargets,
                Var1 = item.Var1,
                ImageSize = item.ImageSize,
                ImageType = item.ImageType,

                ImageName = Encoding.ASCII.GetString(item.ImageName.Take(8).ToArray()).Trim(),
                Bitmap = GenerateBitmap(data)
            };

            ParseStrings(wrap, item.StringData);

            // Add the item to the cache..
            _itemCache[item.ItemId] = wrap;
        }

        /// <summary>
        /// Parses a puppet item entry.
        /// </summary>
        /// <param name="data"></param>
        private void ParsePuppet(byte[] data)
        {
            // Convert the item to a puppet structure entry..
            var item = ArrayToStruct<FFXiTypes.ItemPuppet>(data);
            if (item.ItemId == 0)
                return;

            // Build the item wrapper..
            var wrap = new FFXiTypes.ItemWrapper
            {
                ItemId = item.ItemId,
                Flags = item.Flags,
                StackSize = item.StackSize,
                ItemType = item.ItemType,
                ResourceId = item.ResourceId,
                ValidTargets = item.ValidTargets,
                Slot = item.Slot,
                Elements = item.Elements,
                ImageSize = item.ImageSize,
                ImageType = item.ImageType,

                ImageName = Encoding.ASCII.GetString(item.ImageName.Take(8).ToArray()).Trim(),
                Bitmap = GenerateBitmap(data)
            };

            ParseStrings(wrap, item.StringData);

            // Add the item to the cache..
            _itemCache[item.ItemId] = wrap;
        }

        /// <summary>
        /// Parses a slip item entry.
        /// </summary>
        /// <param name="data"></param>
        private void ParseSlip(byte[] data)
        {
            // Convert the item to a slip structure entry..
            var item = ArrayToStruct<FFXiTypes.ItemSlip>(data);
            if (item.ItemId == 0)
                return;

            // Build the item wrapper..
            var wrap = new FFXiTypes.ItemWrapper
            {
                ItemId = item.ItemId,
                Flags = item.Flags,
                StackSize = item.StackSize,
                ItemType = item.ItemType,
                ResourceId = item.ResourceId,
                ValidTargets = item.ValidTargets,
                Var1 = item.Var1,
                ImageSize = item.ImageSize,
                ImageType = item.ImageType,

                ImageName = Encoding.ASCII.GetString(item.ImageName.Take(8).ToArray()).Trim(),
                Bitmap = GenerateBitmap(data)
            };

            ParseStrings(wrap, item.StringData);

            // Add the item to the cache..
            _itemCache[item.ItemId] = wrap;
        }

        /// <summary>
        /// Parses a weapon item entry.
        /// </summary>
        /// <param name="data"></param>
        private void ParseWeapon(byte[] data)
        {
            // Convert the item to a weapon structure entry..
            var item = ArrayToStruct<FFXiTypes.ItemWeapon>(data);
            if (item.ItemId == 0)
                return;

            // Build the item wrapper..
            var wrap = new FFXiTypes.ItemWrapper
            {
                ItemId = item.ItemId,
                Flags = item.Flags,
                StackSize = item.StackSize,
                ItemType = item.ItemType,
                ResourceId = item.ResourceId,
                ValidTargets = item.ValidTargets,
                Level = item.Level,
                Slot = item.Slot,
                Races = item.Races,
                Jobs = item.Jobs,
                Damage = item.Damage,
                Delay = item.Delay,
                Dps = item.Dps,
                Skill = item.Skill,
                JugSize = item.JugSize,
                MaxCharges = item.MaxCharges,
                CastTime = item.CastTime,
                UseDelay = item.UseDelay,
                ReuseDelay = item.ReuseDelay,
                ImageSize = item.ImageSize,
                ImageType = item.ImageType,

                ImageName = Encoding.ASCII.GetString(item.ImageName.Take(8).ToArray()).Trim(),
                Bitmap = GenerateBitmap(data)
            };

            ParseStrings(wrap, item.StringData);

            // Add the item to the cache..
            _itemCache[item.ItemId] = wrap;
        }

        /// <summary>
        /// Parses a monipulator item entry.
        /// </summary>
        /// <param name="data"></param>
        private void ParseMonipulator(byte[] data)
        {
            // Convert the item to a weapon structure entry..
            var item = ArrayToStruct<FFXiTypes.ItemMonipulator>(data);
            if (item.ItemId == 0)
                return;

            // Build the item wrapper..
            var wrap = new FFXiTypes.ItemWrapper
            {
                ItemId = item.ItemId,
                Flags = item.Flags,
                StackSize = item.StackSize,
                ItemType = item.ItemType,
                ResourceId = item.ResourceId,
                ValidTargets = item.ValidTargets,
                ImageSize = item.ImageSize,
                ImageType = item.ImageType,

                ImageName = Encoding.ASCII.GetString(item.ImageName.Take(8).ToArray()).Trim(),
                Bitmap = GenerateBitmap(data)
            };

            ParseStrings(wrap, item.StringData);

            // Add the item to the cache..
            _itemCache[item.ItemId] = wrap;
        }

        /// <summary>
        /// Parses an instinct item.
        /// </summary>
        /// <param name="data"></param>
        private void ParseInstinct(byte[] data)
        {
            // Convert the item to a weapon structure entry..
            var item = ArrayToStruct<FFXiTypes.ItemInstinct>(data);
            if (item.ItemId == 0)
                return;

            // Build the item wrapper..
            var wrap = new FFXiTypes.ItemWrapper
            {
                ItemId = item.ItemId,
                Flags = item.Flags,
                StackSize = item.StackSize,
                ItemType = item.ItemType,
                ResourceId = item.ResourceID,
                ValidTargets = item.ValidTargets,
                ImageSize = item.ImageSize,
                ImageType = item.ImageType,

                ImageName = Encoding.ASCII.GetString(item.ImageName.Take(8).ToArray()).Trim(),
                Bitmap = GenerateBitmap(data)
            };

            ParseStrings(wrap, item.StringData);

            // Add the item to the cache..
            _itemCache[item.ItemId] = wrap;
        }

        /// <summary>
        /// Parses the strings of the given item.
        /// </summary>
        /// <param name="wrapper"></param>
        /// <param name="stringData"></param>
        private static void ParseStrings(FFXiTypes.ItemWrapper wrapper, byte[] stringData)
        {
            var stringCount = BitConverter.ToUInt32(stringData, 0);
            var stringOffset = BitConverter.ToUInt32(stringData, 4);

            if (stringCount > 9 || stringCount <= 0)
                return;

            var length = GetStringLength(stringData, stringOffset + 28);
            if (length < 1)
                return;

            // Read the item name..
            wrapper.Name = GetString(stringData, stringOffset + 28);

            switch (stringCount)
            {
                case 2: // Japense
                    {
                        // Read the description..
                        stringOffset = BitConverter.ToUInt32(stringData, 4 + (8 * 1));
                        length = GetStringLength(stringData, stringOffset + 28);
                        if (length > 2)
                            wrapper.Description = GetString(stringData, stringOffset + 28);

                        break;
                    }
                case 5: // English
                    {
                        // Read the log name (singular)..
                        stringOffset = BitConverter.ToUInt32(stringData, 4 + (8 * 2));
                        length = GetStringLength(stringData, stringOffset + 28);
                        if (length > 2)
                            wrapper.LogNameSingular = GetString(stringData, stringOffset + 28);

                        // Read the log name (plural)..
                        stringOffset = BitConverter.ToUInt32(stringData, 4 + (8 * 3));
                        length = GetStringLength(stringData, stringOffset + 28);
                        if (length > 2)
                            wrapper.LogNamePlural = GetString(stringData, stringOffset + 28);

                        // Read the description..
                        stringOffset = BitConverter.ToUInt32(stringData, 4 + (8 * 4));
                        length = GetStringLength(stringData, stringOffset + 28);
                        if (length > 2)
                            wrapper.Description = GetString(stringData, stringOffset + 28);

                        break;
                    }
                case 6: // French
                    {
                        // Read the log name (singular)..
                        stringOffset = BitConverter.ToUInt32(stringData, 4 + (8 * 3));
                        length = GetStringLength(stringData, stringOffset + 28);
                        if (length > 2)
                            wrapper.LogNameSingular = GetString(stringData, stringOffset + 28);

                        // Read the log name (plural)..
                        stringOffset = BitConverter.ToUInt32(stringData, 4 + (8 * 4));
                        length = GetStringLength(stringData, stringOffset + 28);
                        if (length > 2)
                            wrapper.LogNamePlural = GetString(stringData, stringOffset + 28);

                        // Read the description..
                        stringOffset = BitConverter.ToUInt32(stringData, 4 + (8 * 5));
                        length = GetStringLength(stringData, stringOffset + 28);
                        if (length > 2)
                            wrapper.Description = GetString(stringData, stringOffset + 28);

                        break;
                    }
                case 9: // German
                    {
                        // Read the log name (singular)..
                        stringOffset = BitConverter.ToUInt32(stringData, 4 + (8 * 4));
                        length = GetStringLength(stringData, stringOffset + 28);
                        if (length > 2)
                            wrapper.LogNameSingular = GetString(stringData, stringOffset + 28);

                        // Read the log name (plural)..
                        stringOffset = BitConverter.ToUInt32(stringData, 4 + (8 * 7));
                        length = GetStringLength(stringData, stringOffset + 28);
                        if (length > 2)
                            wrapper.LogNamePlural = GetString(stringData, stringOffset + 28);

                        // Read the description..
                        stringOffset = BitConverter.ToUInt32(stringData, 4 + (8 * 8));
                        length = GetStringLength(stringData, stringOffset + 28);
                        if (length > 2)
                            wrapper.Description = GetString(stringData, stringOffset + 28);

                        break;
                    }
                default:
                    return;
            }
        }

        /// <summary>
        /// Obtains a strings length.
        /// </summary>
        /// <param name="stringData"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static int GetStringLength(IEnumerable<byte> stringData, uint offset)
        {
            return stringData.ToList().Skip((int)offset).TakeWhile(b => b != 0x00).Count();
        }

        /// <summary>
        /// Obtains a string.
        /// </summary>
        /// <param name="stringData"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static string GetString(IEnumerable<byte> stringData, uint offset)
        {
            return Encoding.ASCII.GetString(stringData.ToList().Skip((int)offset).TakeWhile(b => b != 0x00).ToArray());
        }

        #endregion
    }
}

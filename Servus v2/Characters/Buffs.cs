using Servus_v2.FFXI;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Servus_v2.Characters
{
    public class Buffs
    {
        public Buffs(Character chars)
        {
            Character = chars;

            ParseAbilitiesFile();
        }

        public Character Character { get; set; }
        public SortedDictionary<string, List<Buff>> BuffsListDictionary = new SortedDictionary<string, List<Buff>>();
        public SortedDictionary<string, Buff> BuffListDictionary = new SortedDictionary<string, Buff>();

        private void ParseAbilitiesFile()
        {
            var doc = XDocument.Load(string.Format(@"Resources-master\xml\buffs.xml"));
            var buffQuery = from _buff in doc.Descendants("o")
                            select new Buff
                            {
                                id = int.Parse(_buff.Attribute("id").Value),
                                en = _buff.Attribute("en").Value,
                                enl = _buff.Attribute("enl").Value,
                            };

            foreach (var _buff in buffQuery)
            {
                if (!BuffListDictionary.ContainsKey(_buff.en))
                    BuffListDictionary.Add(_buff.en, _buff);
            }
        }
    }
}
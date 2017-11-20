using Servus_v2.FFXi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servus_v2.Characters
{
    public class spells
    {
        public spells(Character chars)
        {
            Character = chars;

            ParseAbilitiesFile();
        }

        public Character Character { get; set; }
        public SortedDictionary<string, Spell> spellListDictionary = new SortedDictionary<string, Spell>();

        private void ParseAbilitiesFile()
        {
            for (uint x = 1; x < 1020; x++)
            {
                Spell _spell = new Spell();
                _spell.en = Character.Api.Resources.GetSpell(x).Name[0];
                if (_spell.en != "")
                {
                    _spell.id = Character.Api.Resources.GetSpell(x).ID;
                    _spell.mp_cost = Character.Api.Resources.GetSpell(x).MPCost;
                    if (!spellListDictionary.ContainsKey(_spell.en))
                        spellListDictionary.Add(_spell.en, _spell);
                }

                //var doc = XDocument.Load(string.Format(@"Resources-master\xml\spells.xml"));
                //var spellQuery = from _spell in doc.Descendants("o")
                //                 select new Spell
                //                 {
                //                     id = int.Parse(_spell.Attribute("id").Value),
                //                     en = _spell.Attribute("en").Value,
                //                     prefix = _spell.Attribute("prefix").Value,
                //                     mp_cost = int.Parse(_spell.Attribute("mp_cost").Value),
                //                 };

                //foreach (var _spell in spellQuery)
                //{
                //    if (!spellListDictionary.ContainsKey(_spell.en))
                //        spellListDictionary.Add(_spell.en, _spell);
                //}
            }
        }

        public bool SpellMpCost(string SpellName)
        {
            var mainQuery = from _spell in spellListDictionary
                            where string.Compare(_spell.Value.en, SpellName, StringComparison.OrdinalIgnoreCase) == 0

                                  && _spell.Value.mp_cost <= Character.Api.Player.MP
                            select _spell;

            if (mainQuery.Any())
            {
                return true;
            }

            return false;
        }
    }
}
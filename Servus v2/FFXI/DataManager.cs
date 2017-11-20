using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Gambits.Model.FFXi
{
    public sealed class DataManager : ObservableObject
    {
        private static readonly Lazy<DataManager> Lazy = new Lazy<DataManager>(() => new DataManager());

        public static DataManager Instance { get { return Lazy.Value; } }

        public FFXiInfo FFXiInfo = FFXiInfo.Instance;

        public bool IsLoaded { get; private set; }

        private DataManager()
        {
            ParseSpells();
            ParseAbilities();
            ReadItemInfoDats();
            IsLoaded = true;
        }

        public event EventHandler Loaded = delegate { };

        public event Action<string> Error = delegate { };

        public SortedDictionary<string, List<Spell>> JobsSpellListDictionary = new SortedDictionary<string, List<Spell>>();

        public SortedDictionary<string, Spell> SpellDictionary = new SortedDictionary<string, Spell>();

        public SortedDictionary<string, List<Ability>> JobsAbilityListDictionary = new SortedDictionary<string, List<Ability>>();

        public SortedDictionary<string, Ability> AbilityDictionary = new SortedDictionary<string, Ability>();

        public List<string> ItemNames
        {
            get { return FFXiInfo.ItemNames; }
        }

        public List<FFXiTypes.ItemWrapper> ItemList
        {
            get { return FFXiInfo.ItemList; }
        }

        public Dictionary<uint, FFXiTypes.ItemWrapper> Items
        {
            get { return FFXiInfo.Items; }
        }

        public uint GetItemId(string name)
        {
            return FFXiInfo.Items.FirstOrDefault(i => string.Equals(i.Value.Name, name, StringComparison.CurrentCultureIgnoreCase)).Key;
        }

        private void ParseSpells()
        {
            if (!File.Exists(string.Format(@"Settings\Resources\spells.xml")))
                return;

            var doc = XDocument.Load(@"Settings\Resources\spells.xml");
            var spellQuery = from spell in doc.Descendants("s")
                             select new Spell
                             {
                                 Id = int.Parse(spell.Attribute("id").Value, CultureInfo.InvariantCulture),
                                 Index = int.Parse(spell.Attribute("index").Value, CultureInfo.InvariantCulture),
                                 Prefix = spell.Attribute("prefix").Value,
                                 Name = spell.Attribute("english").Value,
                                 German = spell.Attribute("german").Value,
                                 French = spell.Attribute("french").Value,
                                 Japanese = spell.Attribute("japanese").Value,
                                 Type = spell.Attribute("type").Value,
                                 Element = spell.Attribute("element").Value,
                                 Targets = new List<string> { spell.Attribute("targets").Value },
                                 Skill = spell.Attribute("skill").Value,
                                 MpCost = int.Parse(spell.Attribute("mpcost").Value, CultureInfo.InvariantCulture),
                                 CastTime = decimal.Parse(spell.Attribute("casttime").Value, CultureInfo.InvariantCulture),
                                 Recast = decimal.Parse(spell.Attribute("recast").Value, CultureInfo.InvariantCulture)
                             };

            foreach (var spell in spellQuery.Where(spell => !SpellDictionary.ContainsKey(spell.Name)))
            {
                SpellDictionary.Add(spell.Name, spell);
            }

            var spellFiles = Directory.GetFiles(@"Settings\Resources\Spells\", "*.txt");

            foreach (var file in spellFiles)
            {
                var spells = new List<Spell>();
                var fileName = Path.GetFileName(file);
                using (var sr = new StreamReader(file))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var split = line.Split(' ');
                        var level = int.Parse(split[0]);
                        var merit = false;
                        var name = string.Empty;
                        var addendumBlack = false;
                        var addendumWhite = false;
                        var tabulaRasa = false;
                        if (split[1] == "(Merit)")
                        {
                            merit = true;
                            for (int i = 2; i < split.Count(); i++)
                            {
                                name += split[i] + " ";
                            }
                        }
                        else
                        {
                            for (int i = 1; i < split.Count(); i++)
                            {
                                name += split[i] + " ";
                            }
                        }
                        name = name.Trim();
                        if (name.Contains("٭"))
                        {
                            name = name.Replace("٭", "");
                            tabulaRasa = true;
                        }
                        if (name.Contains("●"))
                        {
                            name = name.Replace("●", "");
                            addendumBlack = true;
                        }
                        if (name.Contains("○"))
                        {
                            name = name.Replace("○", "");
                            addendumWhite = true;
                        }
                        if (SpellDictionary.ContainsKey(name))
                        {
                            var spell = SpellDictionary[name];
                            spell.Level = level;
                            spell.Merit = merit;
                            spell.AddendumBlack = addendumBlack;
                            spell.AddendumWhite = addendumWhite;
                            spell.TabulaRasa = tabulaRasa;
                            spells.Add(spell);
                        }
                        else
                        {
                            var spell = new Spell
                            {
                                Name = name,
                                Level = level,
                                Merit = merit,
                                AddendumBlack = addendumBlack,
                                AddendumWhite = addendumWhite,
                                TabulaRasa = tabulaRasa
                            };
                            spells.Add(spell);
                        }
                    }

                    if (fileName != null)
                    {
                        JobsSpellListDictionary.Add(fileName.ToLower().Substring(0, 3), spells);
                    }
                }
            }
        }

        private void ParseAbilities()
        {
            if (!File.Exists(string.Format(@"Settings\Resources\abils.xml")))
                return;
            var doc = XDocument.Load(@"Settings\Resources\abils.xml");
            var abilityQuery = from ability in doc.Descendants("a")
                               select new Ability
                               {
                                   Id = int.Parse(ability.Attribute("id").Value),
                                   Index = int.Parse(ability.Attribute("index").Value),
                                   Prefix = ability.Attribute("id").Value,
                                   Name = ability.Attribute("english").Value,
                                   German = ability.Attribute("german").Value,
                                   French = ability.Attribute("french").Value,
                                   Japanese = ability.Attribute("japanese").Value,
                                   Type = ability.Attribute("type").Value,
                                   Element = ability.Attribute("element").Value,
                                   Targets = new List<string> { ability.Attribute("targets").Value },
                                   Skill = ability.Attribute("skill").Value,
                                   MpCost = int.Parse(ability.Attribute("mpcost").Value),
                                   TpCost = int.Parse(ability.Attribute("tpcost").Value),
                                   CastTime = decimal.Parse(ability.Attribute("casttime").Value),
                                   Recast = decimal.Parse(ability.Attribute("recast").Value),
                               };

            foreach (var ability in abilityQuery.Where(ability => !AbilityDictionary.ContainsKey(ability.Name)))
            {
                AbilityDictionary.Add(ability.Name, ability);
            }

            var abilityFiles = Directory.GetFiles(@"Settings\Resources\Abilities\", "*.txt");
            foreach (var file in abilityFiles)
            {
                using (var sr = new StreamReader(file))
                {
                    var fileName = Path.GetFileName(file);
                    var abilityList = new List<Ability>();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var split = line.Split(' ');
                        var level = int.Parse(split[0]);
                        var name = string.Empty;
                        for (int i = 1; i < split.Count(); i++)
                        {
                            name += split[i] + " ";
                        }
                        name = name.Trim();
                        if (AbilityDictionary.ContainsKey(name))
                        {
                            var ability = AbilityDictionary[name];
                            ability.Level = level;
                            abilityList.Add(ability);

                        }
                    }
                    if (fileName != null)
                    {
                        JobsAbilityListDictionary.Add(fileName.ToLower().Substring(0, 3), abilityList);
                    }
                }
            }

        }

        private void ReadItemInfoDats()
        {
            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
            {
                FFXiInfo = FFXiInfo.Instance;
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    Loaded(this, EventArgs.Empty);
                }
                else
                {
                    Error(e.Error.Message);
                }
            };

            worker.RunWorkerAsync();
        }
    }
}

using Servus_v2.Characters;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Servus_v2.Tasks.Hunter
{
    public class Options
    {
        public Options()
        {
            Targets = new List<string>();
            JobAbilityKeepActive = new List<string>();
            JobAbilityToUseOnlyDuringFights = new List<string>();
            WSScript = new List<string>();
        }

        public bool AcceptRaise { get; set; } = false;

        [XmlIgnore]
        public Character CharacterToAssist { get; set; }

        public bool GoHome { get; set; } = false;
        public List<string> JobAbilityKeepActive { get; set; }
        public string SilencedItem { get; set; }
        public List<string> JobAbilityToUseOnlyDuringFights { get; set; }
        public int LowHpValue { get; set; } = 20;
        public int LowMpValue { get; set; } = 20;
        public string DoomItem { get; set; }
        public int PullDistance { get; set; } = 12;
        public int Raisemobdistance { get; set; } = 21;
        public bool RestOnLowHpEnabled { get; set; } = false;
        public bool useSilenceItem { get; set; }
        public bool useDoomItem { get; set; }
        public bool RestOnLowMpEnabled { get; set; } = false;
        public bool RestOnWeakenedEnabled { get; set; } = false;
        public int SearchDistance { get; set; } = 50;
        public bool StopOnTellEnabled { get; set; } = false;
        public int TargetHpp { get; set; } = 80;
        public int TargetMpp { get; set; } = 80;
        public List<string> Targets { get; set; }
        public List<string> WSScript { get; set; }
        public int WStpValue { get; set; } = 1000;
        public bool StopOnTell { get; set; } = false;
        public bool InvIgnore { get; set; } = true;
        public bool InvStop { get; set; } = false;
        public bool InvWarpRing { get; set; } = false;
        public bool InvWarpCudgel { get; set; } = false;
        public bool InvInstant { get; set; } = false;
        public bool InvWarp { get; set; } = false;
        public bool InvLogOut { get; set; } = false;
        public bool RecordWaypoints { get; set; } = false;
    }
}
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

        public bool UseAttackDown { get; set; } = false;
        public bool UseAddle { get; set; } = false;
        public bool UseAccuracyDown { get; set; } = false;
        public bool UseAGIdown { get; set; } = false;
        public bool UseBlindness { get; set; } = false;
        public bool UseBurn { get; set; } = false;
        public bool UseBind { get; set; } = false;
        public bool UseBio { get; set; } = false;
        public bool UseBane { get; set; } = false;
        public bool UseChoke { get; set; } = false;
        public bool UseCHRdown { get; set; } = false;
        public bool UseCurse { get; set; } = false;
        public bool UseCurse2 { get; set; } = false;
        public bool UseDrown { get; set; } = false;
        public bool UseDoom { get; set; } = false;
        public bool UseDia { get; set; } = false;
        public bool UseDefenseDown { get; set; } = false;
        public bool UseDEXdown { get; set; } = false;
        public bool UseElegy { get; set; } = false;
        public bool UseEvaDown { get; set; } = false;
        public bool UseRefresh3 { get; set; } = false;
        public bool UseProtectra5 { get; set; } = false;
        public bool UseFlash { get; set; } = false;
        public bool UseFrost { get; set; } = false;
        public bool UseHelix { get; set; } = false;
        public bool UseIntDown { get; set; } = false;
        public bool UseMNDdown { get; set; } = false;
        public bool UseMagACC { get; set; } = false;
        public bool UseMagATK { get; set; } = false;
        public bool UseHPdown { get; set; } = false;
        public bool UseMPdown { get; set; } = false;
        public bool UseTPdown { get; set; } = false;
        public bool UseParalysis { get; set; } = false;
        public bool UsePlague { get; set; } = false;
        public bool UsePoison { get; set; } = false;
        public bool UseRasp { get; set; } = false;
        public bool UseRequiem { get; set; } = false;
        public bool UseSTRdown { get; set; } = false;
        public bool UseShock { get; set; } = false;
        public bool UseSilence { get; set; } = false;
        public bool UseSlow { get; set; } = false;
        public bool UseThrenody { get; set; } = false;
        public bool UseVitDown { get; set; } = false;
        public bool UseWeight { get; set; } = false;
        public bool UseAquaveil { get; set; } = false;
        public bool UseBlink { get; set; } = false;
        public bool UseHasteSamba { get; set; } = false;
        public bool UseHaste { get; set; } = false;
        public bool UseHaste2 { get; set; } = false;
        public bool UsePhalanx { get; set; } = false;
        public bool UsePro { get; set; } = false;
        public bool UsePro2 { get; set; } = false;
        public bool UsePro3 { get; set; } = false;
        public bool UsePro4 { get; set; } = false;
        public bool UsePro5 { get; set; } = false;
        public bool UseRefresh { get; set; } = false;
        public bool UseRefresh2 { get; set; } = false;
        public bool UseRegen { get; set; } = false;
        public bool UseRegen2 { get; set; } = false;
        public bool UseRegen3 { get; set; } = false;
        public bool UseRegen4 { get; set; } = false;
        public bool UseRegen5 { get; set; } = false;
        public bool UseReraise { get; set; } = false;
        public bool UseReraise2 { get; set; } = false;
        public bool UseReraise3 { get; set; } = false;
        public bool UseReraise4 { get; set; } = false;
        public bool UseShell { get; set; } = false;
        public bool UseShell2 { get; set; } = false;
        public bool UseShell3 { get; set; } = false;
        public bool UseShell4 { get; set; } = false;
        public bool UseShell5 { get; set; } = false;
        public bool UseStoneSkin { get; set; } = false;
        public bool UseBlazeSpikes { get; set; } = false;
        public bool UseIceSpikes { get; set; } = false;
        public bool UseShockSPikes { get; set; } = false;
        public bool UseShellra { get; set; } = false;
        public bool UseShellra2 { get; set; } = false;
        public bool UseShellra3 { get; set; } = false;
        public bool UseShellra4 { get; set; } = false;
        public bool UseShellra5 { get; set; } = false;
        public bool UseProtectra { get; set; } = false;
        public bool UseProtectra2 { get; set; } = false;
        public bool UseProtectra3 { get; set; } = false;
        public bool UseProtectra4 { get; set; } = false;
        public bool UsePrptectra5 { get; set; } = false;
        public bool UseBoostagi { get; set; } = false;
        public bool UseBoostchr { get; set; } = false;
        public bool UseBoostdex { get; set; } = false;
        public bool UseBoostint { get; set; } = false;
        public bool UseBooststr { get; set; } = false;
        public bool UseBoostmnd { get; set; } = false;
        public bool UseBoostvit { get; set; } = false;
        public bool UseIchi { get; set; } = false;
        public bool UseNi { get; set; } = false;

        public int IdleDelay { get; set; } = 60;

        public bool GoHome { get; set; } = false;
        public int FailedToPathCount { get; set; } = 10;
        public List<string> JobAbilityKeepActive { get; set; }
        public string SilencedItem { get; set; }
        public List<string> JobAbilityToUseOnlyDuringFights { get; set; }
        public int LowHpValue { get; set; } = 20;
        public int LowMpValue { get; set; } = 20;
        public string DoomItem { get; set; }
        public int PullDistance { get; set; } = 12;
        public string PullCommand { get; set; } = "/ja \"Provoke\" <t>";
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
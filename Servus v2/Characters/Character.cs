using EliteMMO.API;
using Servus_v2.Common;
using Servus_v2.Views;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Servus_v2.Characters
{
    public class Character
    {
        #region Fields

        public Dictionary<string, EliteAPI> _CharacterDictionary;
        public float LastcastingValue;
        private readonly Timer _timer = new Timer(1000);
        public SaveAndLoad SaveOrLoad { get; set; }

        #endregion Fields

        #region Constructors

        public spells spells { get; set; }
        public EliteAPI MonitoredAPI { get; set; }

        public Character(Log Log, ToonControl tc, Dictionary<string, EliteAPI> chars, EliteAPI api)
        {
            Logger = Log;
            Tc = tc;
            _CharacterDictionary = chars;
            Api = api;
            CreateFolders();
            MonitoredAPI = api;
            LastcastingValue = CastPercentEx;
            Navi = new Navigation(this);
            Chat = new Chat(this);

            Tasks = new Tasks(this);
            Target = new Target(this);
            _Abilities = new Abilities(this);
            spells = new spells(this);
            SaveOrLoad = new SaveAndLoad(this);
            _timer.Interval = 200;
            _timer.Elapsed += Tick;
            Start();
        }

        public void SetMonitoredAPI(int p)
        {
            MonitoredAPI = new EliteAPI(p);
        }

        #endregion Constructors

        #region Events

        public Abilities _Abilities { get; set; }

        public event EventHandler<NavigationEventArgs> Moved = delegate { };

        public event EventHandler StatusChanged = delegate { };

        public event EventHandler ZoneChanged = delegate { };

        #endregion Events

        #region Properties

        public EliteAPI Api { get; set; }

        public float CastPercentEx => (Api.CastBar.Percent);

        public Chat Chat { get; set; }
        //   public Buffs buffs { get; set; }

        public Position CurrentPosition => new Position { X = Api.Player.X, Y = Api.Player.Y, Z = Api.Player.Z, H = Api.Player.H };

        public Zone CurrentZone => (Zone)Api.Player.ZoneId;

        public bool HasPet => (Api.Player.MainJob == (byte)JobType.BeastMaster || Api.Player.MainJob == (byte)JobType.Summon)
                              && Api.Player.PetIndex != 0;

        public bool SafeToGetUP
        {
            get
            {
                for (var i = 0; i < 768; i++)
                {
                    if (Navi.DistanceTo(i) < Tc.RaiseTrB.Value)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool IsCasting { get; set; }

        public bool IsDead => Status == EntityStatus.Dead || Status == EntityStatus.DeadEngaged || Api.Player.HP <= 0;

        public bool Zoning()
        {
            return Api.Player.LoginStatus == (int)LoginStatus.Loading || Api.Player.LoginStatus == (int)LoginStatus.LoginScreen;
        }

        public bool IsAfflicteAmnesia => IsAfflicted(EliteMMO.API.StatusEffect.Amnesia);
        public bool IsSilenced => IsAfflicted(EliteMMO.API.StatusEffect.Silence);

        public bool Busy => IsDead || IsAfflicted(EliteMMO.API.StatusEffect.Charm1) ||
                            IsAfflicted(EliteMMO.API.StatusEffect.Charm2)
                            || IsAfflicted(EliteMMO.API.StatusEffect.Paralysis) || IsAfflicted(EliteMMO.API.StatusEffect.Sleep)
                            || IsAfflicted(EliteMMO.API.StatusEffect.Sleep2) || IsAfflicted(EliteMMO.API.StatusEffect.Stun)
                            || IsAfflicted(EliteMMO.API.StatusEffect.Stun)
                            || IsAfflicted(EliteMMO.API.StatusEffect.Petrification)
                            || IsAfflicted(EliteMMO.API.StatusEffect.Terror)
                            || IsCasting
                            || Zoning()
                            || Status == EntityStatus.Healing;

        public bool IsMoving { get; set; }
        public EliteAPI Leader { get; set; }
        public Log Logger { get; set; }
        public Navigation Navi { get; set; }
        public Position OldPosition { get; set; }

        public int PetsIndex => !HasPet ? 0 : Api.Player.PetIndex;

        public string PetsName
        {
            get
            {
                if (!HasPet)
                {
                    return string.Empty;
                }

                return Api.Entity.GetEntity(PetsIndex).Name;
            }
        }

        public EntityStatus Status { get; private set; }
        public Target Target { get; set; }
        public Tasks Tasks { get; set; }
        public ToonControl Tc { get; set; }
        private Zone Zone { get; set; }

        #endregion Properties

        #region Methods

        public void CastCheck()
        {
            if (CastPercentEx.Equals(0))
            {
                if (IsCasting)
                {
                    Logger.AddDebugText(Tc.rtbDebug, "Cast stop");
                }
                IsCasting = false; // Not casting
            }
            else if (CastPercentEx.Equals(LastcastingValue)) // we were interupted
            {
                if (IsCasting)
                {
                    Logger.AddDebugText(Tc.rtbDebug, "Cast stop");
                }
                IsCasting = false;
            }
            else // we are casting
            {
                if (!IsCasting)
                {
                    Logger.AddDebugText(Tc.rtbDebug, "Cast start");
                }
                LastcastingValue = CastPercentEx;
                IsCasting = true;
            }
        }

        public void CreateFolders()
        {
            if (!System.IO.Directory.Exists(string.Format(@"Documents\{0}\ChatLog", Api.Player.Name)))
            {
                System.IO.Directory.CreateDirectory(string.Format(@"Documents\{0}\ChatLog", Api.Player.Name));
            }
            if (!System.IO.Directory.Exists(string.Format(@"Documents\{0}\Nav", Api.Player.Name)))
            {
                System.IO.Directory.CreateDirectory(string.Format(@"Documents\{0}\Nav", Api.Player.Name));
            }
            if (!System.IO.Directory.Exists(string.Format(@"Documents\{0}\Config", Api.Player.Name)))
            {
                System.IO.Directory.CreateDirectory(string.Format(@"Documents\{0}\Config", Api.Player.Name));
            }
        }

        public bool IsAfflictedJA(EliteMMO.API.StatusEffect effect)
        {
            foreach (var s in Api.Player.Buffs)
            {
                var status = (EliteMMO.API.StatusEffect)s;
                if (status == effect)
                {
                    return true;
                }
            }
            return false;
        }

        public int SpellRecast(string spell)
        {
            var NeededSpell = Api.Resources.GetSpell(spell, 0);
            if (Api.Recast.GetSpellRecast(NeededSpell.Index) == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public bool IsAfflicted(EliteMMO.API.StatusEffect effect)
        {
            foreach (var s in Api.Player.Buffs)
            {
                var status = (EliteMMO.API.StatusEffect)s;
                if (status == effect)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEngaged()
        {
            return Api.Player.Status == (ulong)EntityStatus.Engaged;
        }

        public void Start()
        {
            _timer.Enabled = true;
        }

        private void Tick(object sender, EventArgs e)
        {
            if (Status != (EntityStatus)Api.Player.Status)
            {
                Status = (EntityStatus)Api.Player.Status;
                StatusChanged(this, new StatusChangedEventArgs { Status = Status });
            }
            if (Zone != CurrentZone)
            {
                Zone = CurrentZone;
                ZoneChanged(this, EventArgs.Empty);
            }
            CastCheck();
            if (OldPosition.Equals(CurrentPosition))
            {
                IsMoving = false;
            }
            else if (!OldPosition.Equals(CurrentPosition))
            {
                IsMoving = true;

                OldPosition = CurrentPosition;
                if (Tasks.Huntertask.Options.RecordWaypoints)
                {
                    Navi.LearnRoutine();
                }
            }
        }

        #endregion Methods

        public bool IsTargetLocked()
        {
            var _Target = Api.Target.GetTargetInfo();
            if (_Target.TargetIndex == (uint)Target.FindBestTarget() && _Target.LockedOn)
            {
                return true;
            }
            return false;
        }

        protected virtual void OnMoved(NavigationEventArgs e)
        {
            Moved(this, e);
        }
    }
}
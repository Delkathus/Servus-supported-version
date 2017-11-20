using Servus_v2.Characters;
using Servus_v2.Common;
using Servus_v2.Contracts;
using Servus_v2.Tasks.Hunter.States;
using System;

namespace Servus_v2.Tasks.Hunter
{
    public class Huntertask : TaskBase, ITask
    {
        public Huntertask(Character Character)
            : base(Character)
        {
            Options = XmlSerializationHelper.Deserialize<Options>(FileName) ?? new Options();
            TS = new Taskstate(Character, Options);
            TS.Stopped += Stop;
            Engine = new StateEngine();

            InitializeStateEngine();
        }

        public event EventHandler Started = delegate { };

        public event EventHandler Stopped = delegate { };

        public bool IsBusy => Engine.IsRunning;
        public Options Options { get; }

        private StateEngine Engine { get; }

        private Taskstate TS { get; }

        //public override void Save()
        //{
        //    XmlSerializationHelper.Serialize(FileName, Options);
        //}

        public override void Start()
        {
            if (Engine.IsRunning)
            {
                return;
            }
            TS.Start();
            Engine.Start(4);
            Started(this, EventArgs.Empty);
            Log.AddDebugText(TC.rtbDebug, "Started");
        }

        public override void Stop(string msg = null)
        {
            // Target.Reset();
            if (!Engine.IsRunning)
            {
                return;
            }

            TS.Stop();
            Engine.Stop();
            Log.AddDebugText(TC.rtbDebug, msg);
            Stopped(this, EventArgs.Empty);

            Log.AddDebugText(TC.rtbDebug, "Stopped");
        }

        private void InitializeStateEngine()
        {
            // Engine.AddState(new Idle(Character) { OnDeath = true, OnPartyDisband = true });
            Engine.AddState(new InventoryChecks(Character, Options, TS) { Priority = 1, Enabled = true });
            Engine.AddState(new Debuff(Character, Options, TS) { Priority = 0, Enabled = true });
            Engine.AddState(new BUFFS(Character, Options, TS) { Priority = 0, Enabled = true });
            Engine.AddState(new AcceptRaise(Character, Options, TS) { Priority = 0, });
            Engine.AddState(new GoHome(Character, Options, TS) { Priority = 0, });
            Engine.AddState(new HealOn(Character, Options, TS) { Priority = 1, });
            Engine.AddState(new HealOff(Character, Options, TS) { Priority = 1, Enabled = true });
            Engine.AddState(new JobAbilityKeepActive(Character, Options, TS) { Priority = 2, Enabled = true });
            Engine.AddState(new TrackNextTarget(Character, Options, TS) { Priority = 3 });
            Engine.AddState(new Pulling(Character, Options, TS) { Priority = 1 });
            Engine.AddState(new LockOn(Character, Options, TS) { Priority = 1 });
            Engine.AddState(new Engage(Character, Options, TS) { Priority = 3 });
            Engine.AddState(new TrackWhileFighting(Character, Options, TS) { Priority = 3 });
            Engine.AddState(new JobAbilityFightOnly(Character, Options, TS) { Priority = 3, Enabled = true });
            Engine.AddState(new WeaponSkill(Character, Options, TS) { Priority = 3 });
            Engine.AddState(new TrackWhileFighting(Character, Options, TS) { Priority = 3 });
        }
    }
}
using EliteMMO.API;
using Servus_v2.Characters;
using Servus_v2.Common;
using System;
using System.Threading;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class WeaponSkill : HunterState
    {
        public string WeaponSkillCommand { get; set; }

        public WeaponSkill(Character character, Options options, Taskstate Taskstate)
            : base(character, options, Taskstate)
        {
            Enabled = true;
        }

        public override int Frequency => 0;

        private int _priority;

        public override int Priority
        {
            get => _priority;
            set => _priority = int.MaxValue - value;
        }

        public override bool NeedToRun
        {
            get
            {
                var Mob = Api.Entity.GetEntity(TS.TargetMobId);

                return
                      Character.Status == EntityStatus.Engaged
                       && Api.Player.TP > Options.WStpValue
                       && Mob.HealthPercent > 2
                       && !Character.IsCasting
                          && !Character.Busy
                       && Navi.DistanceTo(TS.TargetMobId) < 3;
            }
        }

        public override void Enter()
        {
            Log.AddDebugText(TC.rtbDebug, string.Format("Entering {0} State", GetType().Name));
        }

        private delegate string[] GetRichTextBoxLinesInvoker();

        private string[] GetRichTextBoxLines()
        {
            string[] lines;

            if (TC.WStextBox.InvokeRequired)
            {
                lines = (string[])TC.WStextBox.Invoke(new GetRichTextBoxLinesInvoker(GetRichTextBoxLines));
            }
            else
            {
                lines = TC.WStextBox.Lines;
            }
            Options.WSScript.Clear();
            foreach (var str in lines)
            {
                Options.WSScript.Add(str);
            }

            return lines;
        }

        public override void Update()
        {
            try
            {
                var MobPos = new Node { X = Api.Entity.GetEntity(TS.TargetMobId).X, Z = Api.Entity.GetEntity(TS.TargetMobId).Z };
                Log.AddDebugText(TC.rtbDebug, "Using weapon skill command");
                Navi.FaceHeading(MobPos);
                GetRichTextBoxLines();
                foreach (string str in Options.WSScript)
                {
                    Api.ThirdParty.SendString(str);
                    Log.AddDebugText(TC.rtbDebug, string.Format("{0}", str));
                    Thread.Sleep(3000);
                }
                Log.AddDebugText(TC.rtbDebug, "Weapon skill command done");
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }

        public override void Exit()
        {
            Log.AddDebugText(TC.rtbDebug, string.Format("Exiting {0} State", GetType().Name));
        }
    }
}
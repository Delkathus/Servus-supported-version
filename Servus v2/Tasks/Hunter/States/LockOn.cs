﻿using EliteMMO.API;
using Servus_v2.Characters;
using Servus_v2.Common;
using System;
using System.Threading;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class LockOn : HunterState
    {
        private int _priority;

        public LockOn(Character Character, Options options, Taskstate Taskstate)
                    : base(Character, options, Taskstate)
        {
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun => Enabled
                                          && TS.TargetMobId != 0
                                          && !Character.IsTargetLocked()
                                          && Navi.DistanceTo(TS.TargetMobId)
                                          < Options.PullDistance;

        public override int Priority
        {
            get => _priority;
            set => _priority = int.MaxValue - value;
        }

        public override void Enter()
        {
            Log.AddDebugText(TC.rtbDebug, string.Format("Entering {0} State", GetType().Name));
        }

        public override void Exit()
        {
            Log.AddDebugText(TC.rtbDebug, string.Format("Exiting {0} State", GetType().Name));
        }

        public override void Update()
        {
            try
            {
                var Target = Api.Target.GetTargetInfo();
                var MobPos = new Node { X = Api.Entity.GetEntity(TS.TargetMobId).X, Z = Api.Entity.GetEntity(TS.TargetMobId).Z, Y = Api.Entity.GetEntity(TS.TargetMobId).Y };

                Navi.FaceHeading(MobPos);
                if (Target.TargetIndex != TS.TargetMobId)
                {
                    Api.ThirdParty.KeyPress(Keys.ESCAPE);
                    Thread.Sleep(1000);
                    Api.Target.SetTarget(TS.TargetMobId);
                    Thread.Sleep(1000);
                }

                if (Target.TargetIndex == TS.TargetMobId && !Target.LockedOn)
                {
                    Thread.Sleep(400);
                    Api.ThirdParty.SendString("/lockon <t>");
                    Log.AddDebugText(TC.rtbDebug, string.Format(@"Correct target../lockon"));
                    Thread.Sleep(2000);
                }
                if (Target.TargetIndex != TS.TargetMobId)
                {
                    Log.AddDebugText(TC.rtbDebug, "Unable to Target mob.");
                    Exit();
                    return;
                }
                Thread.Sleep(1000);
                Exit();
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}
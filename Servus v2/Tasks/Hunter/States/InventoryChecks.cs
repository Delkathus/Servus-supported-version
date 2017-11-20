using Servus_v2.Characters;
using System;
using System.Threading;

namespace Servus_v2.Tasks.Hunter.States
{
    internal class InventoryChecks : HunterState
    {
        public InventoryChecks(Character character, Options options, Taskstate Taskstate)
                    : base(character, options, Taskstate)
        {
            Priority = int.MinValue;
            Enabled = true;
        }

        public override int Frequency => 0;

        public override bool NeedToRun => Enabled

                                          && !Character.Busy
                                          && !Character.IsCasting
                                          && !Character.IsDead
                                          && !Options.InvIgnore
                                          && Character.Api.Inventory.GetContainerCount(0) == Character.Api.Inventory.GetContainerMaxCount(0);

        public sealed override int Priority { get; set; }

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
                if (!Options.InvIgnore)
                {
                    Log.AddDebugText(TC.rtbDebug, string.Format("Inventory Full, next steps...."));
                    if (Options.InvWarp)
                    {
                        Log.AddDebugText(TC.rtbDebug, string.Format("Inventory Full, Warping home. Stopping tasks."));
                        Character.Api.ThirdParty.SendString("/ma \"Warp\" <me>");
                        Thread.Sleep(5000);
                        Character.Tasks.Huntertask.Stop();
                    }
                    if (Options.InvInstant)
                    {
                        Log.AddDebugText(TC.rtbDebug, string.Format("Inventory Full, Instant Warping . Stopping tasks."));
                        Character.Api.ThirdParty.SendString("/item \"Instant Warp\" <me>");
                        Thread.Sleep(5000);
                        Character.Tasks.Huntertask.Stop();
                    }
                    if (Options.InvStop)
                    {
                        Log.AddDebugText(TC.rtbDebug, string.Format("Inventory Full, Stopping tasks."));
                        Thread.Sleep(1000);
                        Character.Tasks.Huntertask.Stop();
                    }
                    if (Options.InvLogOut)
                    {
                        Log.AddDebugText(TC.rtbDebug, string.Format("Inventory Full, Stopping tasks. Logging out"));
                        Thread.Sleep(1000);
                        Character.Api.ThirdParty.SendString("/logout");
                        Character.Tasks.Huntertask.Stop();
                    }
                    if (Options.InvWarpCudgel)
                    {
                        Log.AddDebugText(TC.rtbDebug, string.Format("Inventory Full, Using Warp Cudgel . Stopping tasks."));
                        Character.Api.ThirdParty.SendString("/equip main \"Warp Cudgel\" <me>");
                        Thread.Sleep(11000);
                        Character.Api.ThirdParty.SendString("/item \"Warp Cudgel\" <me>");
                        Thread.Sleep(5000);
                        Character.Tasks.Huntertask.Stop();
                    }
                    if (Options.InvWarpRing)
                    {
                        Log.AddDebugText(TC.rtbDebug, string.Format("Inventory Full, Using Warp Ring . Stopping tasks."));
                        Character.Api.ThirdParty.SendString("/equip Ring1 \"Warp Ring\" <me> ");
                        Thread.Sleep(31000);
                        Character.Api.ThirdParty.SendString("/item \"Warp Ring\" <me>");
                        Thread.Sleep(5000);
                        Character.Tasks.Huntertask.Stop();
                    }
                }
                Exit();
            }
            catch (Exception ex)
            {
                Log.AddDebugText(TC.rtbDebug, (string.Format(@"{0} , {1}", ex.Message, this)));
            }
        }
    }
}
using EliteMMO.API;
using Servus_v2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Servus_v2.Characters
{
    public class Target
    {
        public int _Distance = 50;
        public List<int> BlockedTargets = new List<int>();

        public Target(Character Api)
        {
            Character = Api;
            Targets = new List<string>();
            _Distance = Character.Tasks.Huntertask.Options.SearchDistance;
        }

        public Log _Log { get; set; }
        public Character Character { get; set; }
        public List<string> Targets { get; set; }

        private int BestTarget { get; set; }

        public int FindBestTarget()
        {
            if (Character.Tasks.Huntertask.Options.Targets.Count > 0)
            {
                var index = Enumerable.Range(0, 768)
                                 .Where(i => IsRendered(i) && IsAttackable(Character.Tasks.Huntertask.Options.Targets, BlockedTargets, i, Character.Tasks.Huntertask.Options.SearchDistance))
                                 .OrderBy(i => Character.Api.Entity.GetEntity(i).Distance)
                                 .Select(i => i).FirstOrDefault();

                return index;
            }
            else return 0;
        }

        public List<string> GetNpcNames()
        {
            var list = new List<string>();
            for (var i = 0; i < 768; i++)
            {
                if (!string.IsNullOrEmpty(Character.Api.Entity.GetEntity(i).Name))

                    list.Add(Character.Api.Entity.GetEntity(i).Name);
            }
            return list;
        }

        public bool HasAggro()
        {
            for (var i = 0; i < 768; i++)
            {
                if (Character.Api.Entity.GetEntity(i).Distance < 21 &&
                Character.Api.Entity.GetEntity(i).Status.Equals(EntityStatus.Engaged) &&
                (Character.Api.Entity.GetEntity(i).ClaimID == 0 || Character.Api.Entity.GetEntity(i).ClaimID.Equals(Character.Api.Player.ServerID)))
                {
                    BestTarget = i;
                    return true;
                }
            }
            return false;
        }

        public bool IsAggro(int mobIndex)
        {
            if (Character.Api.Entity.GetEntity(mobIndex).Distance < 21 &&
            Character.Api.Entity.GetEntity(mobIndex).Status.Equals(EntityStatus.Engaged) &&
            (Character.Api.Entity.GetEntity(mobIndex).ClaimID == 0 || Character.Api.Entity.GetEntity(mobIndex).ClaimID.Equals(Character.Api.Player.ServerID)))
            {
                return true;
            }
            return false;
        }

        public bool IsAttackable(ICollection<string> Targets, ICollection<int> Blocked, int mobIndex, int distance)
        {
            if (Blocked != null && Blocked.Contains(mobIndex))
            {
                return false;
            }

            if (Targets.Count == 0 || Targets.Contains(Character.Api.Entity.GetEntity(mobIndex).Name, StringComparer.InvariantCultureIgnoreCase))
            {
                return
                       Character.Api.Entity.GetEntity(mobIndex).HealthPercent == 100
                       && (Character.Api.Entity.GetEntity(mobIndex).HealthPercent < 100 || IsPartyClaim(mobIndex))
                       && IsAggro(mobIndex)
                       && !IsClaimedBySomeoneElse(mobIndex)
                       && (Character.Api.Entity.GetEntity(mobIndex).Y - Character.Api.Player.Y < 5)
                       && Character.Api.Entity.GetEntity(mobIndex).Status != (uint)EntityStatus.Dead || Character.Api.Entity.GetEntity(mobIndex).Status != (uint)EntityStatus.DeadEngaged
                       && Character.Api.Entity.GetEntity(mobIndex).Distance < Character.Tasks.Huntertask.Options.SearchDistance
                       && Character.Api.Entity.GetEntity(mobIndex).HealthPercent != 0;
            }

            return false;
        }

        public bool IsClaimedBySomeoneElse(int mobIndex)
        {
            var mob = Character.Api.Entity.GetEntity(mobIndex);
            if (mob.ClaimID != 0 && !IsPartyClaim(mobIndex))
            {
                return true;
            }
            return false;
        }

        public bool IsPartyClaim(int mobIndex)
        {
            var mob = Character.Api.Entity.GetEntity(mobIndex);
            for (byte i = 0; i < Character.Api.Party.GetPartyMembers().Count; i++)
            {
                if (Convert.ToBoolean(Character.Api.Party.GetPartyMember(i).Active) && mob.ClaimID.Equals(Character.Api.Party.GetPartyMember(i).ID) && mob.HealthPercent > 0)
                {
                    return true;
                }
            }
            return false;
        }

        // public bool IsPartyClaim(int mobIndex) { for (byte i = 0; i < 16; i++) { var ClaimedID =
        // Character.Api.Entity.GetEntity(mobIndex).ClaimID; var MemberID =
        // Character.Api.Party.GetPartyMember(i).ID; { if (ClaimedID == MemberID &&
        // Character.Api.Entity.GetEntity(mobIndex).HealthPercent > 0) { return true; } }

        // var playerid = Character.Api.Player.ServerID; { if (ClaimedID == playerid) { return true;
        // } } } return false; }

        public bool IsRendered(int id)
        {
            if ((Character.Api.Entity.GetEntity(id).Render0000 & 0x200) == 512)
            {
                return true;
            }
            else
                return false;
        }

        public bool TargetNpc(int id)
        {
            var Target = Character.Api.Target.GetTargetInfo();
            if (Target.TargetIndex != id)
            {
                Character.Api.Target.SetTarget(id);
                Character.Api.ThirdParty.SendString("/Target <t>");
                Thread.Sleep(10);
                Character.Api.ThirdParty.SendString("/lockon <t>");
                return true;
            }
            else return false;
        }
    }
}
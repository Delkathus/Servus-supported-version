using DeenGames.Utils.AStarPathFinder;
using Servus_v2.Common;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Servus_v2.Characters
{
    public class Navigation
    {
        public static Random rnd = new Random();
        public List<Node> CurrentPath = new List<Node>();
        public int offset = 1000;
        public int PullDistance = 10;
        public int SearchDistance = 50;
        public List<Node> Waypoints = new List<Node>();

        private const double TooCloseDistance = 1.5;

        public Navigation(Character chars)
        {
            Character = chars;
            CreateGrid();
            Reset();
        }

        public Character Character { get; set; }
        public double DistanceTolerance { get; set; } = 3;
        public int FailedToPath { get; set; }
        public int GotoDelay { get; set; }
        public byte[,] Grid { get; set; }
        public float StayRunningAmount { get; set; }

        public void CreateGrid()
        {
            try
            {
                Grid = new byte[PathFinderHelper.RoundToNearestPowerOfTwo(2000), PathFinderHelper.RoundToNearestPowerOfTwo(2000)];
                for (int i = 0; i < 2000; i++)
                {
                    for (int j = 0; j < 2000; j++)
                    {
                        Grid[i, j] = PathFinderHelper.BLOCKED_TILE;
                    }
                }
            }
            catch (Exception ex)
            {
                Character.Logger.LogFile(ex.Message, "Nav");
            }
        }

        public double DistanceTo(int mobid)
        {
            var start = new Node { X = Character.Api.Player.X, Z = Character.Api.Player.Z };
            var dest = new Node { X = Character.Api.Entity.GetEntity(mobid).X, Z = Character.Api.Entity.GetEntity(mobid).Z };
            return Math.Sqrt(Math.Pow(start.X - dest.X, 2) + Math.Pow(start.Z - dest.Z, 2));
        }

        public double DistancetoWaypoint(Node start, Node end)
        {
            return Math.Sqrt(Math.Pow(start.X - end.X, 2) + Math.Pow(start.Z - end.Z, 2));
        }

        public void FaceHeading(Node position)
        {
            var player = Character.Api.Entity.GetLocalPlayer();
            var angle = (byte)(Math.Atan((position.Z - player.Z) / (position.X - player.X)) * -(128.0f / Math.PI));
            if (player.X > position.X) angle += 128;
            var radian = (float)angle / 255 * 2 * Math.PI;
            Character.Api.Entity.SetEntityHPosition(Character.Api.Entity.LocalPlayerIndex, (float)radian);
        }

        public bool Desti()
        {
            if (Character.Target.FindBestTarget() != 0)
            {
                var mob = Character.Api.Entity.GetEntity(Character.Target.FindBestTarget());

                var End = Character.Navi.GetWaypointClosestTo(mob.X, mob.Z);

                if (mob != null || End != null)
                {
                    if (Character.Api.Player.X == End.X && Character.Api.Player.Z == End.Z)
                    {
                        return true;
                    }
                    else return false;
                }
            }
            return false;
        }

        public List<Node> GetPath(float X, float Z, int id)
        {
            List<Node> ReturnPath = new List<Node>();
            var Temp = GetWaypointClosestTo(Character.Api.Player.X, Character.Api.Player.Z);
            int StartingX = Convert.ToInt32(Temp.X) + offset;
            int StartingZ = Convert.ToInt32(Temp.Z) + offset;
            int DestinationX = Convert.ToInt32(X) + offset;
            int DestinationZ = Convert.ToInt32(Z) + offset;
            var mob = Character.Api.Entity.GetEntity(id);
            List<PathFinderNode> Path = new PathFinderFast(Grid).FindPath(new DeenGames.Utils.Point(StartingX, StartingZ), new DeenGames.Utils.Point(DestinationX, DestinationZ));
            if (Path != null && Path.Count > 5)
            {
                foreach (PathFinderNode point in Path)
                {
                    ReturnPath.Add(new Node { X = point.X - offset, Z = point.Y - offset });
                }
                // Waypoints are added backwards, so let's reverse.
                ReturnPath.Reverse();
                // Character.Logger.AddDebugText(Character.Tc.rtbDebug, string.Format(@"Path Found
                // Moving to {0} distance {1}y",
                // Character.Api.Entity.GetEntity(Character.hunter.MobID).Name, Character.Api.Entity.GetEntity(Character.hunter.MobID).Distance));

                ReturnPath = SmoothNodes(ReturnPath);
                Character.Api.ThirdParty.KeyDown(EliteMMO.API.Keys.NUMPAD8);
                Thread.Sleep(1);
                Character.Api.ThirdParty.KeyUp(EliteMMO.API.Keys.NUMPAD8);
                Thread.Sleep(1);
                FailedToPath = 0;
            }
            if (Path == null || Path.Count < 5)
            {
                FailedToPath++;

                Character.Logger.AddDebugText(Character.Tc.rtbDebug, string.Format(@"Failed To Find Path to {0} distance // {1}y", mob.Name.ToString(), mob.Distance.ToString()));
            }
            if (FailedToPath > 2)
            {
                Character.Target.BlockedTargets.Add(id);
                Character.Logger.AddDebugText(Character.Tc.rtbDebug, string.Format(@"failed to path to many times. added {0} id {1} to blocked target list", mob.Name, id.ToString()));
            }

            return ReturnPath;
        }

        public Node GetWaypointClosestTo(float x, float z)
        {
            Node ClosestWP = new Node();
            double ClosestDistance = 9999;
            foreach (Node wp in Waypoints)
            {
                Node start = new Node { X = x, Z = z };
                Node end = new Node { X = wp.X, Z = wp.Z };
                if (DistancetoWaypoint(start, end) < ClosestDistance)
                {
                    ClosestWP = wp;
                    ClosestDistance = DistancetoWaypoint(start, end);
                }
            }
            return ClosestWP;
        }

        public void GoTo(float x, float z)
        {
            SetViewMode(ViewMode.FirstPerson);
            FaceHeading(new Node { X = x, Z = z });
            Thread.Sleep(10);
            KeepRunningWithKeyboard();
            Thread.Sleep(30);
        }

        public void GotoNPC(int ID, bool useObjectAvoidance)
        {
            var entity = Character.Api.Entity.GetEntity(ID);
            var position = new Node { X = entity.X, Y = entity.Y, Z = entity.Z, H = entity.H };

            Reset();
            FaceHeading(position);
            MoveForwardTowardsPosition(position, useObjectAvoidance);
        }

        public void LearnRoutine()
        {
            try
            {
                if (Grid[Convert.ToInt32(Character.Api.Player.X) + offset, Convert.ToInt32(Character.Api.Player.Z) + offset] == PathFinderHelper.BLOCKED_TILE)
                {
                    Waypoints.Add(new Node { X = Character.Api.Player.X, Y = Character.Api.Player.Y, Z = Character.Api.Player.Z, H = Character.Api.Player.H, Zone = Character.CurrentZone });
                    Grid[Convert.ToInt32(Character.Api.Player.X) + offset, Convert.ToInt32(Character.Api.Player.Z) + offset] = PathFinderHelper.EMPTY_TILE;
                    Character.Logger.AddDebugText(Character.Tc.rtbDebug, string.Format(@"Added tile {0},{1}", (Convert.ToInt32(Character.Api.Player.X) + offset).ToString(),
                    (Convert.ToInt32(Character.Api.Player.Z) + offset).ToString()));
                }
            }
            catch (Exception ex)
            {
                Character.Logger.LogFile(ex.Message, "Nav");
                Character.Logger.AddDebugText(Character.Tc.rtbDebug, ex.Message);
            }
        }

        public void LoadWaypoints()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Character.Logger.LogFile(ex.Message, "Nav");
            }
        }

        public void ClearWaypointsAndGrid()
        {
            Waypoints.Clear();
            CreateGrid();
        }

        public void Reset()
        {
            Character.Api.AutoFollow.IsAutoFollowing = false;
            Character.Api.ThirdParty.KeyUp(EliteMMO.API.Keys.NUMPAD8);
            Character.Api.ThirdParty.KeyUp(EliteMMO.API.Keys.NUMPAD2);
        }

        public void SetViewMode(ViewMode viewMode)
        {
            if ((ViewMode)Character.Api.Player.ViewMode != viewMode)
            {
                Character.Api.Player.ViewMode = (int)viewMode;
            }
        }

        public List<Node> SmoothNodes(List<Node> nodes)
        {
            List<Node> SmoothLikeButter = new List<Node>();
            int Index = 0;
            foreach (Node node in nodes)
            {
                if (Index > 0 && Index != nodes.Count - 1)
                {
                    float nX = (nodes[Index - 1].X + nodes[Index].X + nodes[Index + 1].X) / 3;
                    float nZ = (nodes[Index - 1].Z + nodes[Index].Z + nodes[Index + 1].Z) / 3;
                    SmoothLikeButter.Add(new Node { X = nX, Z = nZ });
                }
                if (Index == 0)
                {
                    float nX = (nodes[Index].X + nodes[Index + 1].X) / 2;
                    float nZ = (nodes[Index].Z + nodes[Index + 1].Z) / 2;
                    SmoothLikeButter.Add(new Node { X = nX, Z = nZ });
                }
                if (Index == nodes.Count - 1)
                {
                    float nX = (nodes[Index - 1].X + nodes[Index].X) / 2;
                    float nZ = (nodes[Index - 1].Z + nodes[Index].Z) / 2;
                    SmoothLikeButter.Add(new Node { X = nX, Z = nZ });
                }
                Index++;
            }
            return SmoothLikeButter;
        }

        private void AvoidObstacles()
        {
            if (IsStuck())
            {
                if (Character.IsEngaged())
                    WiggleCharacter(attempts: 3);
            }
        }

        private double DistanceTo(Node position)
        {
            var player = Character.Api.Entity.GetLocalPlayer();

            return Math.Sqrt(
                Math.Pow(position.X - player.X, 2) +
                Math.Pow(position.Y - player.Y, 2) +
                Math.Pow(position.Z - player.Z, 2));
        }

        private bool IsStuck()
        {
            var firstX = Character.Api.Player.X;
            var firstZ = Character.Api.Player.Z;
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            var dchange = Math.Pow(firstX - Character.Api.Player.X, 2) + Math.Pow(firstZ - Character.Api.Player.Z, 2);
            return Math.Abs(dchange) < 1;
        }

        private void KeepOneYalmBack(Node position)
        {
            if (DistanceTo(position) > TooCloseDistance) return;

            DateTime duration = DateTime.Now.AddSeconds(5);
            Character.Api.ThirdParty.KeyDown(EliteMMO.API.Keys.NUMPAD2);

            while (DistanceTo(position) <= TooCloseDistance && DateTime.Now < duration)
            {
                SetViewMode(ViewMode.FirstPerson);
                FaceHeading(position);
                Thread.Sleep(30);
            }

            Character.Api.ThirdParty.KeyUp(EliteMMO.API.Keys.NUMPAD2);
        }

        private void KeepRunningWithKeyboard()
        {
            Character.Api.ThirdParty.KeyDown(EliteMMO.API.Keys.NUMPAD8);
        }

        private void MoveForwardTowardsPosition(
                                Node TargetPosition,
        bool useObjectAvoidance)
        {
            if (!(DistanceTo(TargetPosition) > DistanceTolerance)) return;

            if (DistanceTo(TargetPosition) > DistanceTolerance)
            {
                var player = Character.Api.Player;

                SetViewMode(ViewMode.FirstPerson);
                FaceHeading(TargetPosition);

                Character.Api.AutoFollow.SetAutoFollowCoords(
                    TargetPosition.X - player.X,
                    TargetPosition.Y - player.Y,
                    TargetPosition.Z - player.Z);

                Character.Api.AutoFollow.IsAutoFollowing = true;

                if (useObjectAvoidance) AvoidObstacles();
            }

            Character.Api.AutoFollow.IsAutoFollowing = false;
        }

        private void WiggleCharacter(int attempts)
        {
            int count = 0;
            float dir = -45;
            while (IsStuck() && attempts-- > 0)
            {
                Character.Api.Entity.GetLocalPlayer().H = Character.Api.Player.H + (float)(Math.PI / 180 * dir);
                Character.Api.ThirdParty.KeyDown(EliteMMO.API.Keys.NUMPAD8);
                Thread.Sleep(TimeSpan.FromSeconds(2));
                Character.Api.ThirdParty.KeyUp(EliteMMO.API.Keys.NUMPAD8);
                count++;
                if (count == 4)
                {
                    dir = (Math.Abs(dir - -45) < .001 ? 45 : -45);
                    count = 0;
                }
            }
            Character.Api.ThirdParty.KeyUp(EliteMMO.API.Keys.NUMPAD8);
        }
    }
}
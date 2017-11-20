using System;

namespace Servus_v2.Common
{
    public class NavigationEventArgs : EventArgs
    {
        public NavigationEventArgs(Position position)
        {
            Position = position;
        }

        public Position Position { get; }
    }
}
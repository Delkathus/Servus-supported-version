using EliteMMO.API;
using System;

namespace Servus_v2.Common
{
    public class StatusChangedEventArgs : EventArgs
    {
        public EntityStatus Status { get; set; }
    }
}
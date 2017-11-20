using EliteMMO.API;
using System;

namespace Servus_v2.Common
{
    public class ChatEventArgs : EventArgs
    {
        #region Constructors

        public ChatEventArgs(EliteAPI.ChatEntry line)
        {
            ChatLine = line;
        }

        #endregion Constructors

        #region Properties

        public EliteAPI.ChatEntry ChatLine { get; private set; }

        #endregion Properties
    }
}
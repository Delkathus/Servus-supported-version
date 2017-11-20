using EliteMMO.API;
using Servus_v2.Characters;
using Servus_v2.Common;
using Servus_v2.Views;

namespace Servus_v2.Tasks
{
    public class TaskBase
    {
        public TaskBase(Character character)
        {
            Character = character;
            Api = Character.Api;
            Log = Character.Logger;
            TC = Character.Tc;
            Navi = Character.Navi;
            Target = Character.Target;
        }

        internal EliteAPI Api { get; set; }
        internal Character Character { get; set; }
        internal string FileName { get; set; }
        internal Log Log { get; set; }
        internal Navigation Navi { get; set; }
        internal Target Target { get; set; }
        internal ToonControl TC { get; set; }

        public virtual void Save()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Stop(string msg = null)
        {
        }
    }
}
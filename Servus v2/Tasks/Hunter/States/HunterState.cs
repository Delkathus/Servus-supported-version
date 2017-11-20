using Servus_v2.Characters;

namespace Servus_v2.Tasks.Hunter.States
{
    internal abstract class HunterState : State
    {
        protected HunterState(Character Character, Options options, Taskstate Taskstate)
            : base(Character)
        {
            Options = options;
            TS = Taskstate;
        }

        public Options Options { get; }

        public Taskstate TS { get; }
    }
}
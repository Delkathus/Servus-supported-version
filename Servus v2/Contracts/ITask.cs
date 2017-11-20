namespace Servus_v2.Contracts
{
    internal interface ITask
    {
        bool IsBusy { get; }

        void Save();

        void Start();

        void Stop(string msg = null);
    }
}
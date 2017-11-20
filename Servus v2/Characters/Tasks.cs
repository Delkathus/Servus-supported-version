using Servus_v2.Contracts;
using Servus_v2.Tasks.Hunter;
using System.Collections.Generic;
using System.Linq;

namespace Servus_v2.Characters
{
    public class Tasks
    {
        public Tasks(Character Character)
        {
            Initialize(Character);
        }

        public Huntertask Huntertask { get; private set; }

        public bool IsBusy
        {
            get
            {
                return TaskList.Any(t => t.IsBusy);
            }
        }

        private List<ITask> TaskList { get; set; }

        public void Save()
        {
            foreach (var task in TaskList)
            {
                task.Save();
            }
        }

        public void Start()
        {
            foreach (var task in TaskList)
            {
                task.Start();
            }
        }

        public void Stop()
        {
            foreach (var task in TaskList)
            {
                task.Stop();
            }
        }

        private void Initialize(Character Character)
        {
            TaskList = new List<ITask>
            {
                (Huntertask = new Huntertask(Character)),
            };
        }
    }
}
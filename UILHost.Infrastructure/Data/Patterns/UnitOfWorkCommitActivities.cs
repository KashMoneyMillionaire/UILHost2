using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UILHost.Infrastructure.Data.Patterns
{
    public class UnitOfWorkCommitActivities
    {

        readonly List<Func<Task>> _commitActivities = new List<Func<Task>>();

        public void Add(Func<Task> activity) { _commitActivities.Add(activity); }

        public async Task PerformCommitActivities()
        {
            foreach (var activity in _commitActivities)
            {
                await activity();
            }
        }
    }
}

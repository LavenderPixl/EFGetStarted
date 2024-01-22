using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFGetStarted
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public List<TeamWorker> Workers { get; set; }
        public Task CurrentTask { get; set; }
        public List<Task> Tasks { get; set; }
        public Team()
        {

        }
        public Team(int teamId, string name, List<TeamWorker> workers)
        {
            TeamId = teamId;
            Name = name;
            Workers = workers;
        }
    }
}

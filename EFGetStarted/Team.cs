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
        public Tasks? CurrentTask { get; set; }
        public List<Tasks>? Tasks { get; set; }
        public Team()
        {

        }
        public Team(string name, List<TeamWorker> workers)
        {
            Name = name;
            Workers = workers;
        }
    }
}

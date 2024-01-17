using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFGetStarted
{
    public class Worker
    {
        public int WorkerId { get; set; }
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
        public Worker()
        {
            
        }
        public Worker(int workerId, string name, List<Team> teams)
        {
            WorkerId = workerId;
            Name = name;
            Teams = teams;
        }
    }
}

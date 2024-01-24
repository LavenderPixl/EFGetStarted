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
        public List<TeamWorker> Teams { get; set; }
        public Todo? CurrentTodo { get; set; }
        public List<Todo>? Todos { get; set; } = new();
        public Worker()
        {
            
        }
        public Worker(string name, List<TeamWorker> teams)
        {
            Name = name;
            Teams = teams;
        }

        
    }
}

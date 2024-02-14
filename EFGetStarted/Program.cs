using EFGetStarted;
using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.EntityFrameworkCore;
using Microsoft.Quantum.QsCompiler.CompilationBuilder;
using System.Text.Json;
using Microsoft.Quantum.QsCompiler.SyntaxProcessing;
using Microsoft.CodeAnalysis.CSharp.Syntax;


using var db = new BloggingContext();

//// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database path: {db.DbPath}.");

//STARTUP! 
if (db.Tasks.Count() < 1)
{
    seedTasks();
}

if (db.Teams.Count() < 1)
{
    seedWorkers();
}

// List<Worker> n = getWorkers(1);
giveTasks();

printIncompleteTasksAndTodos();

static void seedTasks()
{
    List<Todo> List1 = new List<Todo>
    {
        new Todo("Write code", false),
        new Todo("Compile source", false),
        new Todo("Test program", false)
    };
    Tasks T1 = new("Produce software", List1);

    List<Todo> List2 = new List<Todo>
    {
        new Todo("Pour water", false),
        new Todo("Pour coffee", false),
        new Todo("Turn on", false)
    };
    Tasks T2 = new Tasks("Brew coffee", List2) { };

    using (BloggingContext context = new())
    {
        context.Tasks.Add(T1);
        context.Tasks.Add(T2);

        context.SaveChanges();
    }
}

static List<Team> getTeams(BloggingContext context)
{
    using (context)
    {
        //Gets all teams.
        var team = context.Teams.Include(t => t.Workers).ThenInclude(tt => tt.Worker);

        List<Team> _teamList = new();

        foreach (var item in team)
        {
            _teamList.Add(item);
        }

        return _teamList;
    }
}

static List<Worker> getWorkers(int teamId)
{
    using (BloggingContext context = new())
    {
        var _team = context.Teams.Where(tw => tw.TeamId == teamId).Include(t => t.Workers).ThenInclude(tt => tt.Worker);

        List<TeamWorker> _twList = new();
        List<Worker> _workerList = new();

        foreach (var teamWorker in _team)
        {
            _twList = teamWorker.Workers.ToList();
        }

        var _ids = _twList.Select(r => r.WorkerId);
        var _workers = context.Workers.Where(r => _ids.Contains(r.WorkerId));

        foreach (var _workerBee in _workers)
        {
            _workerList.Add(_workerBee);
        }

        return _workerList;
    }
}

static List<Tasks> getTasks()
{
    using (BloggingContext context = new())
    {
        List<Tasks> _taskList = new();

        var task = context.Tasks.Include(t => t.Todos);

        foreach (var item in task)
        {
            _taskList.Add(item);
        }

        return _taskList;
    }
}


static void giveTasks()
{
    using (BloggingContext context = new())
    {
        //TEMPORARY!! v
        //Gets all teams...
        var team2 = context.Teams.Include(t => t.Workers).ThenInclude(tt => tt.Worker);
        List<Team> _teamList = new();
        foreach (var item in team2)
        {
            _teamList.Add(item);
        }
        //TEMPORARY!!^

        //TEMPORARY!!v
        //Gets all tasks...
        var t = context.Tasks.Include(t => t.Todos);
        List<Tasks> _tList = new();

        foreach (var item in t)
        {
            _tList.Add(item);
        }
        //TEMPORARY!!^
        
        List<Team> _allTeams = _teamList;
        List<Tasks> _allTasks = _tList;

        foreach (var team in _allTeams)
        {
            //TEMPORARY!!v
            //Gets all workers IN team...
            // var _team = context.Teams.Where(tw => tw.TeamId == teamId).Include(t => t.Workers).ThenInclude(tt => tt.Worker);
            var _tempTeam = context.Teams.Where(te => te.TeamId == team.TeamId)
                .Include(t => t.Workers)
                .ThenInclude(tt => tt.Worker);
            List<TeamWorker> _twList = new();
            List<Worker> _wList = new();

            foreach (var teamWorker in _tempTeam)
            {
                _twList = teamWorker.Workers.ToList();
            }

            var _ids = _twList.Select(r => r.WorkerId);
            var _workers = context.Workers.Where(r => _ids.Contains(r.WorkerId));

            foreach (var _workerBee in _workers)
            {
                _wList.Add(_workerBee);
            }
            
            //TEMPORARY!!^

            List<Worker> _workerList = _wList;
            // List<Worker> _workerList = getWorkers(team.TeamId);

            if (team.CurrentTask == null)
            {
                foreach (var task in _allTasks)
                {
                    if (task.Todos.Any(t => t.Worker == null))
                    {
                        //Team gets the task, as their WIP/Current task.
                        team.CurrentTask = task;
                        // context.Teams.Update(team);

                        foreach (var todo in task.Todos)
                        {
                            if (todo.Worker == null)
                            {
                                foreach (var person in _workerList)
                                {
                                    if (person.CurrentTodo == null && todo.Worker == null)
                                    {
                                        todo.Worker = person;
                                        person.CurrentTodo = todo;
                                        // todo.Worker = person;
                                        // context.Workers.Update(person);
                                        Console.WriteLine("Worker found: " + person.Name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        context.SaveChanges();
    }
}

static void seedWorkers()
{
    Worker Steen = new Worker("Steen Secher", new());
    Worker Ejvind = new Worker("Ejvind Møller", new());
    Worker Konrad = new Worker("Konrad Sommer", new());

    Team frontendTeam = new() { Name = "Frontend" };


    Worker Sofus = new Worker("Sofus Lotus", new());
    Worker Remo = new Worker("Remo Lademann", new());

    Team backendTeam = new() { Name = "Backend" };


    Worker Ella = new Worker("Ella Fanth", new());
    Worker Anne = new Worker("Anne Dam", new());

    Team testerTeam = new() { Name = "Testers" };

    using (BloggingContext context = new())
    {
        context.TeamWorkers.Add(new TeamWorker { Team = frontendTeam, Worker = Steen });
        context.TeamWorkers.Add(new TeamWorker { Team = frontendTeam, Worker = Ejvind });
        context.TeamWorkers.Add(new TeamWorker { Team = frontendTeam, Worker = Konrad });

        context.TeamWorkers.Add(new TeamWorker { Team = backendTeam, Worker = Konrad });
        context.TeamWorkers.Add(new TeamWorker { Team = backendTeam, Worker = Sofus });
        context.TeamWorkers.Add(new TeamWorker { Team = backendTeam, Worker = Remo });

        context.TeamWorkers.Add(new TeamWorker { Team = testerTeam, Worker = Ella });
        context.TeamWorkers.Add(new TeamWorker { Team = testerTeam, Worker = Anne });

        context.SaveChanges();
    }
}

static void printIncompleteTasksAndTodos()
{
    using (var context = new BloggingContext())
    {
        var tasks = context.Tasks.Include(task => task.Todos.Where(p => p.IsComplete == false));
        foreach (var task in tasks)
        {
            Console.WriteLine($"Uncompleted Tasks:  \nTask: {task.Name}");
            foreach (var todo in task.Todos)
            {
                if (todo.IsComplete == false)
                {
                    Console.WriteLine($"- {todo.Name} - {todo.IsComplete}");
                }
            }

            Console.WriteLine("\n");
        }
    }
}
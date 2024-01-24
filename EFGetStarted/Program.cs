using EFGetStarted;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Quantum.QsCompiler.CompilationBuilder;
using System.Text.Json;
using Microsoft.Quantum.QsCompiler.SyntaxProcessing;
using Microsoft.CodeAnalysis.CSharp.Syntax;


using var db = new BloggingContext();

//// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database path: {db.DbPath}.");

//// Create
//Console.WriteLine("Inserting a new blog");
//db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
//db.SaveChanges();

//// Read
//Console.WriteLine("Querying for a blog");
//var blog = db.Blogs
//    .OrderBy(b => b.BlogId)
//    .First();

//// Update
//Console.WriteLine("Updating the blog and adding a post");
//blog.Url = "https://devblogs.microsoft.com/dotnet";
//blog.Posts.Add(
//    new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
//db.SaveChanges();

//// Delete
//Console.WriteLine("Delete the blog");
//db.Remove(blog);
//db.SaveChanges();


//var team1 = db.Teams.OrderBy(t => t.TeamId).SingleOrDefault(team => team.TeamId == 1);
//var team2 = db.Teams.OrderBy(t => t.TeamId).SingleOrDefault(team => team.TeamId == 2);
//Console.WriteLine(team1);
//Console.WriteLine(team2);


//STARTUP! 
if (db.Tasks.Count() < 1)
{
    seedTasks();
}
if (db.Teams.Count() < 1)
{
    seedWorkers();
}
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
static List<Team> getTeams()
{
    using (BloggingContext context = new())
    {
        //Gets all teams.
        var team = context.Teams.
            Include(t => t.Workers).
            ThenInclude(tt => tt.Worker);

        List<Team> _teamList = new();

        foreach (var item in team)
        {
            _teamList.Add(item);
        }
        return _teamList;
    }
}
static List<Tasks> getTasks()
{
    using (BloggingContext context = new())
    {
        List<Tasks> _taskList = new();

        var task = context.Tasks.
            Include(t => t.Todos);

        foreach (var item in task)
        {
            _taskList.Add(item);
        }
        return _taskList;
    }
}
static List<Tasks> checkAvailableTasks()
{
    List<Team> _allTeams = getTeams();
    List<Tasks> _allTasks = getTasks();


    foreach (var _singleTeam in _allTeams)
    {
        foreach (var _singleTask in _allTasks)
        {

            if (_singleTeam.CurrentTask.TasksId == _singleTask.TasksId)
            {
                _allTeams.Remove(_singleTeam);
                _allTasks.Remove(_singleTask);
            }
        }
    }
    return _allTasks;
}
static List<Team> checkAvailableTeams()
{
    List<Team> _allTeams = getTeams();
    List<Tasks> _allTasks = getTasks();


    foreach (var _singleTeam in _allTeams)
    {
        foreach (var _singleTask in _allTasks)
        {

            if (_singleTeam.CurrentTask.TasksId == _singleTask.TasksId)
            {
                _allTeams.Remove(_singleTeam);
                _allTasks.Remove(_singleTask);
            }
        }
    }
    return _allTeams;
}
static void giveTasks()
{
    using (BloggingContext context = new())
    {
        List<Tasks> _availableTasks = checkAvailableTasks();
        List<Team> _availableTeams = checkAvailableTeams();



        //team.CurrentTask = _taskList.Where(t => t.Todos)
        //team.Tasks = _taskList[0];

        //Console.WriteLine(team.CurrentTask + team.Name);

        //foreach (var worker in team.Workers)
        //{
        //    if (worker.Worker.CurrentTodo == null)
        //    {
        //        worker.Worker.CurrentTodo = _toDoList.FirstOrDefault();
        //        worker.Worker.Todos = _toDoList;
        //        _toDoList.RemoveAt(0);

        //        Console.WriteLine(worker.Worker.Name);
        //        Console.WriteLine(worker.Worker.CurrentTodo);

        //    }
        //    //Need to

        //    //Todo todo = task.Todos.Where(t => t.Worker)
        //}

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
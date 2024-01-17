using EFGetStarted;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Quantum.QsCompiler.CompilationBuilder;
using System.Text.Json;
using Microsoft.Quantum.QsCompiler.SyntaxProcessing;


using var db = new BloggingContext();

//// Note: This sample requires the database to be created before running.
//Console.WriteLine($"Database path: {db.DbPath}.");

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

//seedTasks();
seedWorkers();
printIncompleteTasksAndTodos();

static void seedTasks()
{
    List<Todo> List1 = new List<Todo>
    {
        new Todo(1, "Write code", false),
        new Todo(2, "Compile source", false),
        new Todo(3, "Test program", false)
    };
    Tasks T1 = new(1, "Produce software", List1);

    List<Todo> List2 = new List<Todo>
    {
        new Todo(4, "Pour water", false),
        new Todo(5, "Pour coffee", false),
        new Todo(6, "Turn on", false)
    };
    Tasks T2 = new Tasks(2, "Brew coffee", List2) { };

    using (BloggingContext context = new())
    {
        context.Tasks.Add(T1);
        context.Tasks.Add(T2);

        context.SaveChanges();
    }
}

static void seedWorkers()
{
    Worker Steen = new Worker(0, "Steen Secher", new());
    Worker Ejvind = new Worker(0, "Ejvind Møller", new());
    Worker Konrad = new Worker(0, "Konrad Sommer", new());
    
    Team frontendTeam = new() { Name = "Frontend"};

    
    Worker Sofus = new Worker(0, "Sofus Lotus", new());
    Worker Remo = new Worker(0, "Remo Lademann", new());
    
    Team backendTeam = new() { Name = "Backend"};


    Worker Ella = new Worker(0, "Ella Fanth", new());
    Worker Anne = new Worker(0, "Anne Dam", new());

    Team testerTeam = new() { Name = "Testers"};

    using (BloggingContext context = new())
    {
        context.TeamWorkers.Add(new TeamWorker { Team = frontendTeam, Worker = Steen});
        context.TeamWorkers.Add(new TeamWorker { Team = frontendTeam, Worker = Ejvind});
        context.TeamWorkers.Add(new TeamWorker { Team = frontendTeam, Worker = Konrad});

        context.TeamWorkers.Add(new TeamWorker { Team = backendTeam, Worker = Konrad});
        context.TeamWorkers.Add(new TeamWorker { Team = backendTeam, Worker = Sofus});
        context.TeamWorkers.Add(new TeamWorker { Team = backendTeam, Worker = Remo});

        context.TeamWorkers.Add(new TeamWorker { Team = testerTeam, Worker = Ella});
        context.TeamWorkers.Add(new TeamWorker { Team = testerTeam, Worker = Anne});

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
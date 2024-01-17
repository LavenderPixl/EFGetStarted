using EFGetStarted;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Quantum.QsCompiler.CompilationBuilder;
using System.Text.Json;


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

printIncompleteTasksAndTodos();
//seedTasks();

//using (BloggingContext context = new())
//{
//    var tasks = context.Tasks.Include(task => task.Todos);
//    foreach (var task in tasks)
//    {
//        Console.WriteLine($"Task: {task.Name}");
//        foreach (var todo in task.Todos)
//        {
//            Console.WriteLine($"- {todo.Name}");
//        }
//    }
//}

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
            Console.WriteLine("\n\n");
        }

    }
}
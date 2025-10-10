using System;
using System.Collections.Generic;
using System.Linq;


/*
    top-level statements - poti scrie cod direct in Program.cs
                         - nu mai e nevoie sa creezi o clasa Program si metoda Main()
                         - tot codul din acest fisier este considerat "Main"
                         - deci poti crea obiecte, cere input si dupa sa afiseze datele aici
    
*/

var manager = new Manager
{
    Name = "Ioana",
    Team = "DevOps",
    Email = "ioana@example.com"
};

var project1 = new Project("Quizzy", new List<Task>
{
    new Task("Design UI", false, DateTime.Now.AddDays(2))
});

/*
    with - cloneaza
        - poti copia un record + sa modifci o parte din el
*/
var newTask = new Task("Implement Backend", false, new DateTime(2025, 10, 20));
var project2 = project1 with
{
    Tasks = project1.Tasks.Append(newTask).ToList()
};

// cerem utilizatorului sa introduca un task nou
Console.WriteLine("Enter task title:");
var title = Console.ReadLine();

// creem task-ul
var userTask = new Task(title ?? "Untitled", false, DateTime.Now.AddDays(3));
Console.WriteLine($"Task created: {userTask.Title} - Completed: {userTask.IsCompleted}");

/*
    PATTERN MATCHING - verifici tipul unui obiect -> actionezi diferit in functie de acel tip
 */
Utilities.DisplayInfo(project2);
Utilities.DisplayInfo(userTask);

/*
    STATIC LAMBDA FILTERING - o expresie lambda care nu acceseaza variabile din afara ei
                            - e mai sigur
 */
var overdueTasks = project2.Tasks.Where(static t => t.DueDate < DateTime.Now && !t.IsCompleted);

Console.WriteLine("\nOverdue tasks:");
foreach (var t in overdueTasks)
{
    Console.WriteLine($"⚠️ {t.Title} (Due: {t.DueDate})");
}
using System;

public static class Utilities
{
    public static void DisplayInfo(object obj)
    {
        switch (obj)
        {
            case Task t:
                Console.WriteLine($"Task: {t.Title} (Completed: {t.IsCompleted})");
                break;
            case Project p:
                Console.WriteLine($"Project: {p.Name}, Tasks: {p.Tasks.Count}");
                break;
            default:
                Console.WriteLine("Unknown type");
                break;
        }
    }
}
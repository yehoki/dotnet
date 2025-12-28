namespace dotnet.Events;
public class Events
{
    public event EventHandler FunctionCompleted;
    public void BeginFunction()
    {
        Console.WriteLine("Function started");
        Console.WriteLine("Function running");
        Console.WriteLine("Function completed");
        OnFunctionCompleted();
    }
    protected virtual void OnFunctionCompleted()
    {
        FunctionCompleted?.Invoke(this, EventArgs.Empty);
    }
}

public class Looper
{
    public static void Initialise()
    {
        Events events = new();
        events.FunctionCompleted += events_FunctionCompleted;
        while (true)
        {
            string? input = Console.ReadLine();
            if (input == null)
            {
                Console.WriteLine("Invalid input");
                continue;
            }
            if (input.ToLower() == "event")
            {
                events.BeginFunction();
            }
            if (input.ToLower() == "exit") return;
        }
    }
    public static void events_FunctionCompleted(object? sender, EventArgs args)
    {
        Console.WriteLine("Event function triggered");
    }
}
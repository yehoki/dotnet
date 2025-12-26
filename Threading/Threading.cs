using System.Diagnostics;

namespace dotnet.Threading;
public class Threading
{
    // Make Tea
    public async Task MakeTeaAsync()
    {
        // 1. Put water in kettle
        // 2. Start boiling kettle
        // 3. Wait for the kettle to boil (ASYNC)
        // 3. Take cups out for tea
        // 4. Put tea in cups
        // 5. Pour water in cups
        // 6. Finish
        Console.WriteLine("Begin making Tea");
        Task boilingWater = BoilWaterAsync();
        Console.WriteLine("Taking cups out");
        Console.WriteLine("Putting tea in cups");
        await boilingWater;
        Console.WriteLine("Pour boiling water in cups");
        Console.WriteLine("Finish making Tea");
    }
    private async Task BoilWaterAsync()
    {
        Console.WriteLine("Pouring water in kettle");
        Console.WriteLine("Start boiling kettle");
        await Task.Delay(2500);
        Console.WriteLine("Finish boiling kettle");
    }
    public void BlockingTasks()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        Console.WriteLine("Begin running Tasks");
        Task first = Task.Run(() =>
        {
            Console.WriteLine($"First running on thread: {Environment.CurrentManagedThreadId}");
            Thread.Sleep(2500);
            Console.WriteLine($"First finished running on thread: {Environment.CurrentManagedThreadId}");
        });
        
        Task second = Task.Run(() =>
        {
            Console.WriteLine($"Second running on thread: {Environment.CurrentManagedThreadId}");
            Thread.Sleep(3500);
            Console.WriteLine($"Second finished running on thread: {Environment.CurrentManagedThreadId}");
        });
        
        Task third = Task.Run(() =>
        {
            Console.WriteLine($"Third running on thread: {Environment.CurrentManagedThreadId}");
            Thread.Sleep(4500);
            Console.WriteLine($"Third finished running on thread: {Environment.CurrentManagedThreadId}");
        });
        List<Task> tasks = [first, second, third];
        Task allTasks = Task.WhenAll(tasks);
        allTasks.Wait();
        stopwatch.Stop();
        Console.WriteLine($"All tasks finished in {stopwatch.ElapsedMilliseconds}ms");
    }
}
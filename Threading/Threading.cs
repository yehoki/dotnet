using System.Collections.Concurrent;
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
    public async Task BlockingTasksAsync()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        Console.WriteLine("Begin running Tasks");
        Task first = Task.Run(async () =>
        {
            Console.WriteLine($"First running on thread: {Environment.CurrentManagedThreadId}");
            await Task.Delay(2500);
            Console.WriteLine($"First finished running on thread: {Environment.CurrentManagedThreadId}");
        });
        
        Task second = Task.Run(async () =>
        {
            Console.WriteLine($"Second running on thread: {Environment.CurrentManagedThreadId}");
            await Task.Delay(3500);
            Console.WriteLine($"Second finished running on thread: {Environment.CurrentManagedThreadId}");
        });
        
        Task third = Task.Run(async () =>
        {
            Console.WriteLine($"Third running on thread: {Environment.CurrentManagedThreadId}");
            await Task.Delay(4500);
            Console.WriteLine($"Third finished running on thread: {Environment.CurrentManagedThreadId}");
        });
        List<Task> tasks = [first, second, third];
        Task other = Task.Run(async () =>
        {
            Console.WriteLine($"short task on thread: {Environment.CurrentManagedThreadId}");
            await Task.Delay(2500);
            Console.WriteLine("short task on thread finished");
        });
        await Task.WhenAll(tasks);
        await other;
        stopwatch.Stop();
        Console.WriteLine($"All tasks finished in {stopwatch.ElapsedMilliseconds}ms");
    }
    public void LongLoop()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        for (int i = 0; i < 1_000_000; i++)
        {
            LoopMethod(i);
        }
        stopwatch.Stop();
        Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");
    }
    public void LongLoopParallel()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        Parallel.For(0, 1_000_000, i => { LoopMethod(i); });
        stopwatch.Stop();
        Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");
    }
    private int LoopMethod(int i)
    {
        // Console.WriteLine(Environment.CurrentManagedThreadId);
        return i * i;
    }
    public void SingleBetSimulation()
    {
        int balance = 0;
        // Multithread increment
        Parallel.For(0, 100000, i => { balance++; });
        Console.WriteLine($"Balance: {balance}");
    }
    public void MultipleBetSimulation()
    {
        Random random = new();
        Betting betting = new();
        int[] userIds = [1, 2, 3, 4, 5, 6, 7];
        Parallel.For(0, 1000, i =>
        {
            int userId = random.Next(userIds.Length);
            decimal betAmount = (decimal)(random.NextDouble() * 100.00);
            bool bet = betting.MakeBet(userId, betAmount);
            if (bet) Console.WriteLine($"Bet successful for user {userId} for amount {betAmount} on thread {Environment.CurrentManagedThreadId}");
        });
    }
    public void SingleBetSimulationMonitor()
    {
        Random random = new();
        Betting betting = new();
        int[] userIds = [1, 2, 3, 4, 5, 6, 7];
        Parallel.For(0, 1000, i =>
        {
            int userId = random.Next(userIds.Length);
            decimal betAmount = (decimal)(random.NextDouble() * 100.00);
            bool bet = betting.MakeBetSync(userId, betAmount);
            if (bet) Console.WriteLine($"Bet successful for user {userId} for amount {betAmount} on thread {Environment.CurrentManagedThreadId}");
        });
    }
}
public class Betting()
{
    private readonly ConcurrentDictionary<int, decimal> _userBetMap = [];
    private readonly Dictionary<int, decimal> _userBetMapSync = [];
    public bool MakeBet(int userId, decimal betAmount)
    {
        Console.WriteLine($"Making bet for user {userId} for amount {betAmount} on thread {Environment.CurrentManagedThreadId}");
        decimal newBalance;
        if (_userBetMap.TryGetValue(userId, out var balance))
            newBalance = balance - betAmount;
        else
            newBalance = 100 - betAmount;
        
        if (newBalance < 0) return false;
        Console.WriteLine($"New balance: {newBalance} for userId {userId} on thread {Environment.CurrentManagedThreadId}");
        _userBetMap[userId] = newBalance;
        return true;
    }
    public bool MakeBetSync(int userId, decimal betAmount)
    {
        Console.WriteLine($"Making bet for user {userId} for amount {betAmount} on thread {Environment.CurrentManagedThreadId}");
        Monitor.Enter(_userBetMapSync);
        try
        {
            decimal newBalance;
            if (_userBetMap.TryGetValue(userId, out var balance))
                newBalance = balance - betAmount;
            else
                newBalance = 100 - betAmount;
            
            if (newBalance < 0) return false;
            Console.WriteLine($"New balance: {newBalance} for userId {userId} on thread {Environment.CurrentManagedThreadId}");
            _userBetMap[userId] = newBalance;
            return true;
        }
        finally
        {
            Monitor.Exit(_userBetMapSync);
        }
    }
}
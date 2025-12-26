using System.Diagnostics;

namespace dotnet.Memory;
public class Memory
{
    public struct Snapshot
    {
        public int ValueOne { get; set; }
        public char ValueTwo { get; set; }
        public bool ValueThree { get; set; }
    }
    public class Account
    {
        public string Username { get; set; } = "";
        public List<int> IntList { get; set; } = [];
        public List<string> Groups { get; set; } = [];
    }
    public void MakeSnapshots()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        List<Snapshot> snapshots = [];
        for (int i = 0; i < 1_000_000; i++)
        {
            Snapshot snapshot = new()
            {
                ValueOne = i % 2 == 0 ? 0 : 1,
                ValueTwo = (char)(i % 100),
                ValueThree = i % 2 == 0
            };
            snapshots.Add(snapshot);
        }
        stopwatch.Stop();
        Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");
    }
    public void MakeAccounts()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        List<Account> accounts = [];
        for (int i = 0; i < 1_000_000; i++)
        {
            Account account = new()
            {
                Username = "Name" + i.ToString(),
                IntList = [i],
                Groups = ["Group" + i.ToString()]
            };
            accounts.Add(account);
        }
        stopwatch.Stop();
        Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");
    }
}
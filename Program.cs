using dotnet.Threading;

Threading threading = new();
// await threading.MakeTeaAsync();
// threading.BlockingTasks();
// await threading.BlockingTasksAsync();
threading.LongLoop();
threading.LongLoopParallel();
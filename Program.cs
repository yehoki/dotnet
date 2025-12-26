using dotnet.Memory;
using dotnet.Threading;

Threading threading = new();
// await threading.MakeTeaAsync();
// threading.BlockingTasks();
// await threading.BlockingTasksAsync();
// threading.LongLoop();
// threading.LongLoopParallel();

// threading.SingleBetSimulation();
// threading.MultipleBetSimulation();
// threading.SingleBetSimulationMonitor();
Memory memory = new();

// 10ms
memory.MakeSnapshots();
// 500ms
memory.MakeAccounts();
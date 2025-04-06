using FileReplicaApp.FileLogic;
using FileReplicaApp.Services;
using Serilog;

namespace FileReplicaApp.Controllers
{
    public class FileReplicaController : IFileReplicaController
    {
        private readonly IFileSyncService _fileSyncService;

        public FileReplicaController(IFileSyncService fileSyncService)
        {
            _fileSyncService = fileSyncService;
        }

        public void StartService(string[] args)
        {
            try
            {
                if (args.Length != 5)
                {
                    Console.WriteLine("Usage: Dotnet run <intervalSeconds> <sourceDir> <replicaDir> <logFile>");
                    return;
                }
                Console.WriteLine("Press [Enter] to exit application!");

                int intervalSeconds = int.Parse(args[1]);
                string sourceDir = Path.GetFullPath(args[2]);
                string replicaDir = Path.GetFullPath(args[3]);
                string logFile = args[4];

                string logFolder = ConstructDirectoryPath("logs");
                string logFilePath = Path.Combine(logFolder, logFile);
                CreateDirectory(sourceDir);
                CreateDirectory(replicaDir);

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                    .CreateLogger();

                Log.Information("Starting periodic folder synchronization...");
                
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        var keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            Log.Information("Exited Application.");
                            Log.CloseAndFlush();
                            break;
                        }
                    }
                    try
                    {
                        _fileSyncService.PerformSync(sourceDir, replicaDir);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Synchronization failed.");
                    }

                    Thread.Sleep(intervalSeconds * 1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Log.Error(ex, "An exception occurred during execution.");
                Console.WriteLine("Usage: Dotnet run <intervalSeconds> <sourceDir> <replicaDir> <logFile>");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private string ConstructDirectoryPath(string folderName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), folderName);
        }

        private void CreateDirectory(string dir)
        {

            if (!Directory.Exists(dir))
            {

                Directory.CreateDirectory(ConstructDirectoryPath(dir));

            }
          
        }
    }
}

using Serilog;
using FileReplicaApp.Controllers;
using FileReplicaApp.Services;
using System;
using FileReplicaApp.FileLogic;

namespace FileReplicaApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
              
                CreateBanner();

             
                IFileSyncService fileSyncService = new FileSyncService();
                IFileReplicaController fileReplicaController = new FileReplicaController(fileSyncService);

                fileReplicaController.StartService(args);

          
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Log.Error(ex, "An exception occurred during execution.");
            }
        }

        static void CreateBanner()
        {
            Console.Clear();
            int consoleWidth = Console.WindowWidth;
            string borderLine = new string('*', consoleWidth);

            string title = "[ FileReplicaApp ]";
            string version = "[ Version 1.0.0 ]";

            int titlePadding = (consoleWidth - title.Length) / 2;
            int versionPadding = (consoleWidth - version.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(borderLine);
            Console.WriteLine(new string(' ', titlePadding) + title);
            Console.WriteLine(new string(' ', versionPadding) + version);
            Console.WriteLine(borderLine);
            Console.ResetColor();
            Console.WriteLine("\nWelcome to the app!.");
        }
    }
}

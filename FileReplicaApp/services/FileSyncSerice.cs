using Serilog;

namespace FileReplicaApp.Services
{
    public class FileSyncService : IFileSyncService
    {
        public void PerformSync(string sourceDir, string replicaDir)
        {
            Log.Information("Performing synchronization...");

           
            SyncDirectories(sourceDir, replicaDir);

            SyncFiles(sourceDir, replicaDir);

            CleanUpReplica(sourceDir, replicaDir);
        }

        public void SyncDirectories(string sourceDir, string replicaDir)
        {
            foreach (var sourceSubDir in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                var relativePath = Path.GetRelativePath(sourceDir, sourceSubDir);
                var targetDir = Path.Combine(replicaDir, relativePath);

                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                    Log.Information($"Created directory: {targetDir}");
                }
            }
        }

        public void SyncFiles(string sourceDir, string replicaDir)
        {
            foreach (var srcPath in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                CopyFileToReplica(srcPath, sourceDir, replicaDir);
            }
        }

        public void CleanUpReplica(string sourceDir, string replicaDir)
        {
        
            foreach (var replicaPath in Directory.GetFiles(replicaDir, "*", SearchOption.AllDirectories))
            {
                var relative = Path.GetRelativePath(replicaDir, replicaPath);
                var sourcePath = Path.Combine(sourceDir, relative);

                if (!File.Exists(sourcePath))
                {
                    File.Delete(replicaPath);
                    Log.Information($"Deleted file: {replicaPath}");
                }
            }
            foreach (var replicaDirPath in Directory.GetDirectories(replicaDir, "*", SearchOption.AllDirectories))
            {
                var relative = Path.GetRelativePath(replicaDir, replicaDirPath);
                var sourceDirPath = Path.Combine(sourceDir, relative);

                if (!Directory.Exists(sourceDirPath))
                {
                    Directory.Delete(replicaDirPath, true);
                    Log.Information($"Deleted directory: {replicaDirPath}");
                }
            }
        }

        private void CopyFileToReplica(string sourcePath, string sourceDir, string replicaDir)
        {
            var relativePath = Path.GetRelativePath(sourceDir, sourcePath);
            var targetPath = Path.Combine(replicaDir, relativePath);
            var targetDir = Path.GetDirectoryName(targetPath);

            Directory.CreateDirectory(targetDir);

            if (!File.Exists(targetPath) || File.GetLastWriteTimeUtc(sourcePath) > File.GetLastWriteTimeUtc(targetPath))
            {
                File.Copy(sourcePath, targetPath, true);
                Log.Information($"Copied/Updated: {sourcePath} → {targetPath}");

                try
                {
                    using (var reader = new StreamReader(sourcePath))
                    {
                        string firstLine = reader.ReadLine();
                        if (!string.IsNullOrEmpty(firstLine))
                            Log.Information($"Preview (first line of {sourcePath}): {firstLine}");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Failed to preview {sourcePath}: {ex.Message}");
                }
            }
        }
    }
}


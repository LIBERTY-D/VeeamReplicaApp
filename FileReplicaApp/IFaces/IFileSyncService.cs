
namespace FileReplicaApp.Services
{
    public interface IFileSyncService
    {
        void SyncDirectories(string sourceDir, string replicaDir);
        void SyncFiles(string sourceDir, string replicaDir);
        void CleanUpReplica(string sourceDir, string replicaDir);
        void PerformSync(string sourceDir, string replicaDir);
    }
}

using System.Collections.Generic;
using Backups;
using BackupsExtra.Logging;

namespace BackupsExtra
{
    public interface IBackupExtraService
    {
        ExtraBackupJob AddExtraBackupJob(List<string> file, ICreateRestorePoint point, string path, bool localKeep, AbstractLogging logging);

        ExtraBackupJob DeserializeExtraBackupJob(string pathToJson);
        void SerializeExtraBackupJob(string pathToJson, ExtraBackupJob extraBackupJob);
        List<ExtraBackupJob> GetExtraBackupJobs();
    }
}
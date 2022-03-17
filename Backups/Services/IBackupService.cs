using System.Collections.Generic;

namespace Backups.Services
{
    public interface IBackupService
    {
        BackupJob CreateBackupJob(List<string> file, ICreateRestorePoint point, string path, bool localKeep);
    }
}
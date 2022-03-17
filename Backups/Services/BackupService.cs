using System.Collections.Generic;
using Backups.Services;

namespace Backups
{
    public class BackupService : IBackupService
    {
        private List<BackupJob> _backups = new List<BackupJob>();

        public BackupService()
        {
        }

        public BackupJob CreateBackupJob(List<string> file, ICreateRestorePoint point, string path, bool localKeep)
        {
            var backup = new BackupJob(file, point, path, localKeep);
            _backups.Add(backup);
            return backup;
        }
    }
}
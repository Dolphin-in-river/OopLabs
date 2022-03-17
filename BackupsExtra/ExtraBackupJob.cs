using System.Collections.Generic;
using Backups;
using BackupsExtra.CleanAlgorithmFactory;
using BackupsExtra.CleanRestorePoints;
using BackupsExtra.Logging;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    public class ExtraBackupJob
    {
        public ExtraBackupJob(List<string> file, ICreateRestorePoint point, string path, bool localKeep, AbstractLogging logging)
        {
            SimpleBackupsJob = new BackupJob(file, point, path, localKeep);
            PointsWithDateTime = new List<RestorePointWithDateCreation>();
            Logging = logging;
        }

        public bool Merge
        {
            get;
            set;
        }

        public List<RestorePointWithDateCreation> PointsWithDateTime
        {
            get;
            private set;
        }

        public AbstractLogging Logging
        {
            get;
            set;
        }

        public BackupJob SimpleBackupsJob
        {
            get;
            set;
        }

        public IRestorePoint CreateRestorePoint()
        {
            IRestorePoint newPoint = SimpleBackupsJob.CreateRestorePoint();
            var newExtraPoint = new RestorePointWithDateCreation(newPoint, Logging);
            PointsWithDateTime.Add(newExtraPoint);
            Logging.ExecuteLogging(newExtraPoint.GetMessageForLogging());
            return newPoint;
        }

        public void ExecuteCleanAlgorithm(ICleanAlgorithmFactory cleanAlgorithmFactory)
        {
            AbstractCleanAlgorithm cleanAlgorithm = cleanAlgorithmFactory.Create();
            List<RestorePointWithDateCreation> pointsAfterClean =
                cleanAlgorithm.GetPointsAfterClean(PointsWithDateTime);
            if (Merge && !IsEmptyResultListRestorePoint(pointsAfterClean) &&
                pointsAfterClean.Count < PointsWithDateTime.Count)
            {
                cleanAlgorithm.Merge(PointsWithDateTime, SimpleBackupsJob.LocalKeep);
            }
            else
            {
                if (IsEmptyResultListRestorePoint(pointsAfterClean))
                {
                    throw new BackupsExtraException(
                        "Error with executing algorithms, all restore point must be deleted and you won't to merge");
                }

                cleanAlgorithm.ExecuteClean(PointsWithDateTime, SimpleBackupsJob.LocalKeep);
            }

            PointsWithDateTime = pointsAfterClean;
        }

        public void RecoveryFile(IRestorePoint point, IRecovery recoveryInfo)
        {
            foreach (RestorePointWithDateCreation item in PointsWithDateTime)
            {
                if (item.Point.Equals(point))
                {
                    if (point.GetType() == typeof(SplitRestorePoint))
                        item.RecoverSplitPoint(recoveryInfo);
                    if (point.GetType() == typeof(SingleRestorePoint))
                        item.RecoverSinglePoint(recoveryInfo);
                }
            }
        }

        public void DeleteFileInBackupJob(string directoryFile)
        {
            SimpleBackupsJob.DeleteFileInBackupJob(directoryFile);
        }

        public int AmountPoints()
        {
            return PointsWithDateTime.Count;
        }

        public int AmountStorageAtPoint(IRestorePoint point)
        {
            return point.DirectoryFiles.Count;
        }

        private bool IsEmptyResultListRestorePoint(List<RestorePointWithDateCreation> resultList)
        {
            return resultList.Count == 0;
        }
    }
}
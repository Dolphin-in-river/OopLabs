using System.Collections.Generic;
using System.IO;
using Backups;
using BackupsExtra.CleanAlgorithmFactory;
using BackupsExtra.Logging;
using NUnit.Framework;

namespace BackupsExtra
{
    internal class Program
    {
        private static void CheckCorrectRecovery()
        {
            string directory1 = "C:/Users/Иван/Desktop/1.txt";
            string directory2 = "C:/Users/Иван/Desktop/2.txt";
            var listFile = new List<string>
            {
                directory1,
                directory2,
            };

            bool localKeep = true;
            ICreateRestorePoint point = new CreateSplitRestorePoint();
            var consoleLogging = new ConsoleLogging();
            consoleLogging.Configuration = true;
            var extraBackupService = new BackupsExtraService();
            ExtraBackupJob extraBackupJob = extraBackupService.AddExtraBackupJob(listFile, point, "C:/Users/Иван/Desktop/", localKeep, consoleLogging);

            extraBackupJob.CreateRestorePoint();
            extraBackupJob.CreateRestorePoint();
            extraBackupJob.DeleteFileInBackupJob(directory1);

            IRestorePoint point3 = extraBackupJob.CreateRestorePoint();

            extraBackupJob.Merge = true;
            extraBackupJob.ExecuteCleanAlgorithm(new AmountCleanAlgorithmFactory(1));

            extraBackupJob.RecoveryFile(point3, new RecoveryToDifferentLocation("C:/Users/Иван/Desktop/Restore/"));
            var directoryInfo = new DirectoryInfo("C:/Users/Иван/Desktop/Restore/");

            Assert.AreEqual(2, directoryInfo.GetFiles().Length);
            directoryInfo.Delete(true);
        }

        private static void CheckCorrectWorkWithJson()
        {
            string directory1 = "C:/Users/Иван/Desktop/1.txt";
            string directory2 = "C:/Users/Иван/Desktop/2.txt";
            var listFile = new List<string>
            {
                directory1,
                directory2,
            };

            bool localKeep = true;
            ICreateRestorePoint point = new CreateSplitRestorePoint();
            var consoleLogging = new ConsoleLogging();
            consoleLogging.Configuration = true;
            var extraBackupService = new BackupsExtraService();
            ExtraBackupJob extraBackupJob = extraBackupService.AddExtraBackupJob(listFile, point, "C:/Users/Иван/Desktop/", localKeep, consoleLogging);

            extraBackupJob.CreateRestorePoint();
            extraBackupJob.CreateRestorePoint();
            extraBackupJob.DeleteFileInBackupJob(directory1);

            extraBackupJob.CreateRestorePoint();
            extraBackupJob.Merge = true;
            extraBackupJob.ExecuteCleanAlgorithm(new AmountCleanAlgorithmFactory(1));
            extraBackupService.SerializeExtraBackupJob("C:/Users/Иван/Desktop/out.json", extraBackupJob);
            ExtraBackupJob newExtraBackupJob = extraBackupService.DeserializeExtraBackupJob("C:/Users/Иван/Desktop/out.json");
            Assert.AreEqual(extraBackupJob.PointsWithDateTime[0].Point.DirectoryFiles[0], newExtraBackupJob.PointsWithDateTime[0].Point.DirectoryFiles[0]);
            Assert.AreEqual(extraBackupJob.PointsWithDateTime[0].Point.CreateDataTime, newExtraBackupJob.PointsWithDateTime[0].Point.CreateDataTime);
        }

        private static void Main()
        {
            CheckCorrectRecovery();

            CheckCorrectWorkWithJson();
        }
    }
}

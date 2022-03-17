using System;
using System.Collections.Generic;
using Backups;
using BackupsExtra.CleanAlgorithmFactory;
using BackupsExtra.Logging;
using BackupsExtra.Tools;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    public class BackupsExtraTests
    {
        private IBackupExtraService _backupExtraService;

        [SetUp]
        public void Setup()
        {
            _backupExtraService = new BackupsExtraService();
        }

        [Test]
        public void CheckCorrectAmountCleanAlgorithm()
        {
            string directory1 = "./1.txt";
            string directory2 = "./2.txt";
            var listFile = new List<string>
            {
                directory1,
                directory2,
            };

            bool localKeep = false;
            ICreateRestorePoint point = new CreateSplitRestorePoint();
            var consoleLogging = new ConsoleLogging
            {
                Configuration = true
            };
            var extraBackupService = new BackupsExtraService();
            ExtraBackupJob extraBackupJob = extraBackupService.AddExtraBackupJob(listFile, point,
                "./", localKeep, consoleLogging);

            extraBackupJob.CreateRestorePoint();
            extraBackupJob.DeleteFileInBackupJob(directory1);

            extraBackupJob.CreateRestorePoint();
            extraBackupJob.ExecuteCleanAlgorithm(new AmountCleanAlgorithmFactory(1));
            Assert.AreEqual(1, extraBackupJob.AmountPoints());
        }

        [Test]
        public void CheckCorrectDateCleanAlgorithmWithCleanAllPoints()
        {
            Assert.Catch<BackupsExtraException>(() =>
            {
                string directory1 = "./1.txt";
                string directory2 = "./2.txt";
                var listFile = new List<string>
                {
                    directory1,
                    directory2,
                };

                bool localKeep = false;
                ICreateRestorePoint point = new CreateSplitRestorePoint();
                var consoleLogging = new ConsoleLogging
                {
                    Configuration = true
                };
                var extraBackupService = new BackupsExtraService();
                ExtraBackupJob extraBackupJob = extraBackupService.AddExtraBackupJob(listFile, point,
                    "./", localKeep, consoleLogging);

                extraBackupJob.CreateRestorePoint();
                extraBackupJob.DeleteFileInBackupJob(directory1);
                extraBackupJob.CreateRestorePoint();
                extraBackupJob.CreateRestorePoint();
                extraBackupJob.CreateRestorePoint();
                extraBackupJob.ExecuteCleanAlgorithm(new DateCleanAlgorithmFactory(DateTime.Now.AddDays(100)));
            });
        }

        
        [Test]
        public void CheckCorrectOnesHybridCleanAlgorithm()
        {
            Assert.Catch<BackupsExtraException>(() =>
            {
                string directory1 = "./1.txt";
                string directory2 = "./2.txt";
                var listFile = new List<string>
                {
                    directory1,
                    directory2,
                };

                bool localKeep = false;
                ICreateRestorePoint point = new CreateSplitRestorePoint();
                var consoleLogging = new ConsoleLogging
                {
                    Configuration = true
                };
                var extraBackupService = new BackupsExtraService();
                ExtraBackupJob extraBackupJob = extraBackupService.AddExtraBackupJob(listFile, point,
                    "./", localKeep, consoleLogging);

                extraBackupJob.CreateRestorePoint();
                extraBackupJob.DeleteFileInBackupJob(directory1);
                extraBackupJob.CreateRestorePoint();
                extraBackupJob.CreateRestorePoint();
                extraBackupJob.CreateRestorePoint();
                extraBackupJob.ExecuteCleanAlgorithm(new OnesRequirementHybridCleanAlgorithmFactory(2, DateTime.Now.AddDays(100)));
            });
        }
        
        [Test]
        public void CheckCorrectBothRequirementsHybridCleanAlgorithm()
        {
                string directory1 = "./1.txt";
                string directory2 = "./2.txt";
                var listFile = new List<string>
                {
                    directory1,
                    directory2,
                };

                bool localKeep = false;
                ICreateRestorePoint point = new CreateSplitRestorePoint();
                var consoleLogging = new ConsoleLogging
                {
                    Configuration = true
                };
                var extraBackupService = new BackupsExtraService();
                ExtraBackupJob extraBackupJob = extraBackupService.AddExtraBackupJob(listFile, point,
                    "./", localKeep, consoleLogging);

                extraBackupJob.CreateRestorePoint();
                extraBackupJob.DeleteFileInBackupJob(directory1);
                extraBackupJob.CreateRestorePoint();
                extraBackupJob.CreateRestorePoint();
                extraBackupJob.CreateRestorePoint();
                extraBackupJob.ExecuteCleanAlgorithm(new BothRequirementsHybridCleanAlgorithm(2, DateTime.Now.AddDays(100)));
                Assert.AreEqual(2, extraBackupJob.AmountPoints());
            }

        [Test]
        public void CheckCorrectMergePoint()
        {
            string directory1 = "./1.txt";
            string directory2 = "./2.txt";
            var listFile = new List<string>
            {
                directory1,
                directory2,
            };

            bool localKeep = false;
            ICreateRestorePoint point = new CreateSplitRestorePoint();
            var consoleLogging = new ConsoleLogging
            {
                Configuration = true
            };
            var extraBackupService = new BackupsExtraService();
            ExtraBackupJob extraBackupJob = extraBackupService.AddExtraBackupJob(listFile, point,
                "./", localKeep, consoleLogging);

            extraBackupJob.CreateRestorePoint();
            extraBackupJob.DeleteFileInBackupJob(directory1);

            IRestorePoint point3 = extraBackupJob.CreateRestorePoint();

            extraBackupJob.Merge = true;

            extraBackupJob.ExecuteCleanAlgorithm(new AmountCleanAlgorithmFactory(1));
            Assert.AreEqual(2, extraBackupJob.AmountStorageAtPoint(point3));
        }
    }
}
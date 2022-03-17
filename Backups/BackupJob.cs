using System;
using System.Collections.Generic;
using System.Diagnostics;
using Backups.Services;
using Backups.Tools;

namespace Backups
{
    public class BackupJob
    {
        public BackupJob(List<string> file, ICreateRestorePoint point, string path, bool localKeep)
        {
            Files = file;
            Point = point;
            LocalKeep = localKeep;
            Points = new List<IRestorePoint>();
            Repository = new Repository(path);
        }

        public List<IRestorePoint> Points
        {
            get;
            set;
        }

        public List<string> Files
        {
            get;
            set;
        }

        public ICreateRestorePoint Point
        {
            get;
            set;
        }

        public Repository Repository
        {
            get;
            set;
        }

        public bool LocalKeep
        {
            get;
            set;
        }

        public IRestorePoint CreateRestorePoint()
        {
            if (Repository == null)
            {
                throw new BackupsException("Not directory storage");
            }

            var files = new List<string>(Files);
            IRestorePoint resultPoint = Point.CreateRestorePoint(files, Points.Count + 1, Repository, LocalKeep);
            Points.Add(resultPoint);
            return resultPoint;
        }

        public void DeleteFileInBackupJob(string directoryFile)
        {
            Files.Remove(directoryFile);
        }

        public int AmountPoints()
        {
            return Points.Count;
        }

        public int AmountStorage()
        {
            return Repository.GetCount();
        }
    }
}
using System;
using System.Collections.Generic;

namespace Backups
{
    public abstract class IRestorePoint
    {
        public IRestorePoint()
        {
        }

        public IRestorePoint(List<string> directoryFiles, int numberRestorePoint, Repository repository, bool localKeep)
        {
            DirectoryFiles = directoryFiles;
            CreateDataTime = DateTime.Now;
            NumberRestorePoint = numberRestorePoint;
            CurrentRepository = repository;
            CurrentLocalKeep = localKeep;
        }

        public Repository CurrentRepository
        {
            get;
            set;
        }

        public DateTime CreateDataTime
        {
            get;
            set;
        }

        public List<string> DirectoryFiles
        {
            get;
            set;
        }

        public int NumberRestorePoint
        {
            get;
            set;
        }

        public bool CurrentLocalKeep
        {
            get;
            set;
        }
    }
}
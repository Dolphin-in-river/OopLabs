using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups
{
    public class SingleRestorePoint : IRestorePoint
    {
        private static int _nextId = 0;
        private int _id;

        public SingleRestorePoint()
        {
        }

        public SingleRestorePoint(List<string> directoryFiles, int numberRestorePoint, Repository repository, bool localKeep)
            : base(directoryFiles, numberRestorePoint, repository, localKeep)
        {
            _id = ++_nextId;
            CurrentRepository.CreateSingleStorage(localKeep, numberRestorePoint, _id, directoryFiles);
        }
    }
}
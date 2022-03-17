using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups
{
    public class SplitRestorePoint : IRestorePoint
    {
        private static int _nextId = 0;
        private int _id;

        public SplitRestorePoint()
        {
        }

        public SplitRestorePoint(List<string> directoryFiles, int numberRestorePoint, Repository repository, bool localKeep)
            : base(directoryFiles, numberRestorePoint, repository, localKeep)
        {
            _id = ++_nextId;
            CurrentRepository.CreateSplitStorage(localKeep, numberRestorePoint, _id, directoryFiles);
        }
    }
}
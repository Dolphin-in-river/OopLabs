using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups
{
    public class Repository
    {
        private List<IStorageFilesInMemory> _storages = new List<IStorageFilesInMemory>();
        private bool _localKeep;
        private int _countFilesInLocal;

        public Repository(string path)
        {
            Path = path;
        }

        public string Path
        {
            get;
            set;
        }

        public void CreateSingleStorage(bool localKeep, int numberRestorePoint, int id, List<string> directoryFiles)
        {
            _localKeep = localKeep;
            _countFilesInLocal += directoryFiles.Count;
            if (localKeep)
            {
                var dirInfo = new DirectoryInfo(Path);
                dirInfo.CreateSubdirectory(numberRestorePoint + "_SingleRestorePoint/");
                string archivePath =
                    Path + numberRestorePoint + "_SingleRestorePoint/" + id + ".zip";
                ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Create);
                foreach (string item in directoryFiles)
                {
                    archive.CreateEntryFromFile(item, numberRestorePoint + "_" + GetName(item));
                }

                archive.Dispose();
            }
            else
            {
                var archive = new Archive(id + ".zip", directoryFiles);
                _storages.Add(new SingleStorageFilesInMemory(archive));
            }
        }

        public void CreateSplitStorage(bool localKeep, int numberRestorePoint, int id, List<string> directoryFiles)
        {
            _localKeep = localKeep;
            _countFilesInLocal += directoryFiles.Count;
            if (localKeep)
            {
                var dirInfo = new DirectoryInfo(Path);
                dirInfo.CreateSubdirectory(numberRestorePoint + "_SplitRestorePoint/");
                foreach (string item in directoryFiles)
                {
                    string archivePath = Path + numberRestorePoint + "_SplitRestorePoint/" +
                                         GetName(item) + ".zip";
                    ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Create);
                    archive.CreateEntryFromFile(item, numberRestorePoint + "_" + GetName(item));
                    archive.Dispose();
                }
            }
            else
            {
                var archives = new List<Archive>();
                foreach (string item in directoryFiles)
                {
                    var bufferList = new List<string>();
                    bufferList.Add(item);
                    archives.Add(new Archive(GetName(item) + ".zip", bufferList));
                }

                _storages.Add(new SplitStorageFilesInMemory(archives));
            }
        }

        public int GetCount()
        {
            if (_localKeep)
            {
                return _countFilesInLocal;
            }

            int result = 0;
            foreach (IStorageFilesInMemory item in _storages)
            {
                result += item.GetAmountStorage();
            }

            return result;
        }

        private string GetName(string directoryName)
        {
            string s = null;
            for (int i = directoryName.LastIndexOf("/") + 1; i < directoryName.Length; i++)
            {
                s += directoryName[i];
            }

            return s;
        }
    }
}
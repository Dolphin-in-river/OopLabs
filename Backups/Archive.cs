using System.Collections.Generic;

namespace Backups
{
    public class Archive
    {
        private string _nameArchive;
        private List<string> _files;

        public Archive(string nameArchive, List<string> files)
        {
            _files = files;
            _nameArchive = nameArchive;
        }

        public int GetAmount()
        {
            return _files.Count;
        }
    }
}
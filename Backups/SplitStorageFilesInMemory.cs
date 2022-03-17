using System.Collections.Generic;

namespace Backups
{
    public class SplitStorageFilesInMemory : IStorageFilesInMemory
    {
        private List<Archive> _archives;

        public SplitStorageFilesInMemory(List<Archive> archives)
        {
            _archives = archives;
        }

        public int GetAmountStorage()
        {
            int result = 0;
            foreach (var item in _archives)
            {
                result += item.GetAmount();
            }

            return result;
        }
    }
}
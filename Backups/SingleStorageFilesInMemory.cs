namespace Backups
{
    public class SingleStorageFilesInMemory : IStorageFilesInMemory
    {
        private Archive _archive;

        public SingleStorageFilesInMemory(Archive archive)
        {
            _archive = archive;
        }

        public int GetAmountStorage()
        {
            return _archive.GetAmount();
        }
    }
}
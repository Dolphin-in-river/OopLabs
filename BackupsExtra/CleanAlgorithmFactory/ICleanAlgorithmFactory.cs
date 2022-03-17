using BackupsExtra.CleanRestorePoints;

namespace BackupsExtra.CleanAlgorithmFactory
{
    public interface ICleanAlgorithmFactory
    {
        TypeCleanPoints GetTypeClean();
        AbstractCleanAlgorithm Create();
    }
}
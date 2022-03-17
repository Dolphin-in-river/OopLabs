namespace IsuExtra
{
    public class MegaFaculty
    {
        private char _firstLetter;
        private string _name;

        public MegaFaculty(char firstLetter, string name)
        {
            _name = name;
            _firstLetter = firstLetter;
        }

        public char GetFirstLetter()
        {
            return _firstLetter;
        }
    }
}
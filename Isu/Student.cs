namespace Isu
{
    public class Student
    {
        private static int _nextId = 0;
        private string name;
        private int _id;

        private string _groupname;
        public Student(string name, string groupname)
        {
            this.name = name;
            this._groupname = groupname;
            this._id = ++_nextId;
        }

        public int GetID()
        {
            return this._id;
        }

        public string GetName()
        {
            return this.name;
        }

        public void SetGroup(string groupname)
        {
            _groupname = groupname;
        }

        public string GetGroup()
        {
            return _groupname;
        }
    }
}
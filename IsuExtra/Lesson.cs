namespace IsuExtra
{
    public class Lesson
    {
        private string _startTime;
        private string _endTime;
        private string _teacherName;
        private int _audienceNumber;

        public Lesson(string startTime, string endTime, string teacherName, int audienceNumber)
        {
            _startTime = startTime;
            _endTime = endTime;
            _teacherName = teacherName;
            _audienceNumber = audienceNumber;
        }

        public Lesson()
        {
        }

        public string GetStartTime()
        {
            return _startTime;
        }
    }
}
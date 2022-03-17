using Isu.Tools;

namespace Isu
{
    public class CourseNumber
    {
        private const int MaxValue = 6;
        private const int MinValue = 1;
        private int _course;

        public CourseNumber(int course)
        {
            if (course < MinValue || course > MaxValue)
            {
                throw new IsuException("Course number isn't correct");
            }

            _course = course;
        }

        public int GetCourseNumber()
        {
            return _course;
        }
    }
}
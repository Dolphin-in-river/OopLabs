using System.Collections.Generic;
namespace IsuExtra
{
    public class ScheduleDay
    {
        private const int MaxAmountLessons = 7;
        private List<Lesson> _scheduleDay = new List<Lesson>(MaxAmountLessons)
        {
            new Lesson(), new Lesson(), new Lesson(), new Lesson(), new Lesson(), new Lesson(), new Lesson(),
        };

        public ScheduleDay(List<Lesson> scheduleDay)
        {
            _scheduleDay = scheduleDay;
        }

        public ScheduleDay()
        {
        }

        public List<Lesson> Lessons()
        {
            return _scheduleDay;
        }
    }
}
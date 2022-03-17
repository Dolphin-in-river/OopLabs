using System.Collections.Generic;
using IsuExtra.Tools;

namespace IsuExtra
{
    public class Schedule
    {
        private const int LongWeek = 6;
        private List<ScheduleDay> _schedule = new List<ScheduleDay>(LongWeek)
        {
            new ScheduleDay(), new ScheduleDay(), new ScheduleDay(), new ScheduleDay(), new ScheduleDay(), new ScheduleDay(),
        };

        public Schedule(List<ScheduleDay> schedule)
        {
            if (schedule.Count != LongWeek)
            {
                throw new IsuExtraException("The size of work week is incorrect");
            }

            _schedule = schedule;
        }

        public Schedule()
        {
        }

        public int GetAmountDay()
        {
            return _schedule.Count;
        }

        public List<Lesson> Lessons(int day)
        {
            return _schedule[day].Lessons();
        }

        public void AddInfo(int day, ScheduleDay scheduleDay)
        {
            _schedule[day] = scheduleDay;
        }
    }
}
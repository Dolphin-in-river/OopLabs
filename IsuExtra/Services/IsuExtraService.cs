using System;
using System.Collections.Generic;
using Isu;
using IsuExtra.Tools;

namespace IsuExtra
{
    public class IsuExtraService : IIsuExtraService
    {
        private const int LongWeek = 6;
        private const int MaxNumberLesson = 7;
        private List<OGNP> _ognps;
        private List<GroupExtra> _groups;

        public IsuExtraService()
        {
            _groups = new List<GroupExtra>();
            _ognps = new List<OGNP>();
        }

        public GroupExtra AddGroup(Group group, Schedule scheduleGroup)
        {
            GroupExtra newGroup = new GroupExtra(group, scheduleGroup);
            _groups.Add(newGroup);
            return newGroup;
        }

        public Student AddStudent(GroupExtra group, string name)
        {
            Student newStudent = new Student(name, group.GetName());
            group.AddStudent(newStudent);
            return newStudent;
        }

        public void RegisterStudent(OGNP ognp, Flow flow, Student student)
        {
            Schedule flowSchedule = flow.GetSchedule();
            Schedule scheduleGroup = GetSchedule(student);
            Schedule firstOgnpSchedule = new Schedule();
            if (IsStudentInOGNP(student))
            {
                firstOgnpSchedule = GetScheduleOGNP(student);
            }

            for (int i = 0; i < LongWeek; i++)
            {
                for (int j = 0; j < MaxNumberLesson; j++)
                {
                    if ((MatchGroupScheduleAndFlowSchedule(scheduleGroup.Lessons(i)[j], flowSchedule.Lessons(i)[j]) ||
                         MatchFlowScheduleAndOgnpSchedule(flowSchedule.Lessons(i)[j], firstOgnpSchedule.Lessons(i)[j]))
                        && CheckNotNullLesson(flowSchedule.Lessons(i)[j]))
                    {
                        throw new IsuExtraException("Find a matches of schedules");
                    }
                }
            }

            ognp.AddStudent(student, flow);
        }

        public OGNP AddOgnp(MegaFaculty letterMegaFaculty)
        {
            var newOGNP = new OGNP(letterMegaFaculty);
            _ognps.Add(newOGNP);
            return newOGNP;
        }

        public List<Student> StudentsNotJoin(GroupExtra group)
        {
            var notJoin = new List<Student>();
            foreach (Student student in group.GetList())
            {
                short flag = 0;
                foreach (OGNP ognp in _ognps)
                {
                    if (ognp.ConsistStudent(student))
                    {
                        flag++;
                    }
                }

                if (flag == 0)
                {
                    notJoin.Add(student);
                }
            }

            return notJoin;
        }

        public bool CheckContainsOgnp(OGNP ognp)
        {
            foreach (var item in _ognps)
            {
                if (ognp.GetId() == item.GetId())
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsStudentInOGNP(Student student)
        {
            foreach (OGNP item in _ognps)
            {
                if (item.ConsistStudent(student))
                {
                    return true;
                }
            }

            return false;
        }

        public Schedule GetSchedule(Student student)
        {
            Console.WriteLine(_groups.Count);
            foreach (GroupExtra groupExtra in _groups)
            {
                if (groupExtra.IsConsistStudent(student))
                {
                    return groupExtra.GetSchedule();
                }
            }

            return null;
        }

        public Schedule GetScheduleOGNP(Student student)
        {
            foreach (OGNP item in _ognps)
            {
                if (item.ConsistStudent(student))
                {
                    foreach (var flow in item.GetFlows())
                    {
                        if (flow.IsStudentInFlow(student))
                        {
                            return flow.GetSchedule();
                        }
                    }
                }
            }

            return null;
        }

        private bool CheckNotNullLesson(Lesson lesson)
        {
            return lesson.GetStartTime() != null;
        }

        private bool MatchFlowScheduleAndOgnpSchedule(Lesson flowLesson, Lesson ognpLesson)
        {
            return flowLesson.GetStartTime() == ognpLesson.GetStartTime();
        }

        private bool MatchGroupScheduleAndFlowSchedule(Lesson scheduleLesson, Lesson flowLesson)
        {
            return scheduleLesson.GetStartTime() == flowLesson.GetStartTime();
        }
    }
}
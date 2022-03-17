using System;
using System.Collections.Generic;
using Isu;
using IsuExtra.Tools;
namespace IsuExtra
{
    public class Flow
    {
        private static int _idNext = 0;
        private int _id;
        private int _spots;
        private Schedule _schedule;
        private List<Student> _students = new List<Student>();

        public Flow(int spots)
        {
            _spots = spots;
            _id = ++_idNext;
        }

        public void SetSpots(int newSpots)
        {
            _spots = newSpots;
        }

        public int GetSpots()
        {
            return _spots;
        }

        public Schedule GetSchedule()
        {
            return _schedule;
        }

        public bool IsStudentInFlow(Student student)
        {
            foreach (var item in _students)
            {
                if (item.GetID() == student.GetID())
                {
                    return true;
                }
            }

            return false;
        }

        public void AddSchedule(Schedule schedule)
        {
            _schedule = schedule;
        }

        public int GetId()
        {
            return _id;
        }

        public List<Student> GetListStudents()
        {
            return _students;
        }

        public void AddStudent(Student student)
        {
            if (_schedule == null)
            {
                throw new IsuExtraException("This flow hasn't a schedule");
            }

            _students.Add(student);
        }

        public void DeleteStudent(Student student)
        {
            for (int i = 0; i < _students.Count; i++)
            {
                if (_students[i].GetID() == student.GetID())
                {
                    _students.RemoveAt(i);
                    break;
                }
            }
        }
    }
}

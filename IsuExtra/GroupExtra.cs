using System.Collections.Generic;
using Isu;
namespace IsuExtra
{
    public class GroupExtra
    {
        private Group _group;
        private Schedule _schedule;

        public GroupExtra(Group group, Schedule schedule)
        {
            _group = group;
            _schedule = schedule;
        }

        public Schedule GetSchedule()
        {
            return _schedule;
        }

        public string GetName()
        {
            return _group.GetName();
        }

        public void AddStudent(Student student)
        {
            _group.AddStudent(student);
        }

        public bool IsConsistStudent(Student student)
        {
            foreach (var item in _group.GetList())
            {
                if (student.GetID() == item.GetID())
                {
                    return true;
                }
            }

            return false;
        }

        public List<Student> GetList()
        {
            return _group.GetList();
        }
    }
}
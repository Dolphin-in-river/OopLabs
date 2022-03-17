using System.Collections.Generic;
using Isu;
namespace IsuExtra
{
    public interface IIsuExtraService
    {
        OGNP AddOgnp(MegaFaculty letterMegaFaculty);
        GroupExtra AddGroup(Group group, Schedule scheduleGroup);
        Student AddStudent(GroupExtra group, string name);
        Schedule GetSchedule(Student student);
        Schedule GetScheduleOGNP(Student student);
        bool IsStudentInOGNP(Student student);
        List<Student> StudentsNotJoin(GroupExtra group);
        bool CheckContainsOgnp(OGNP ognp);
        void RegisterStudent(OGNP ognp, Flow flow, Student student);
    }
}
using Isu;
namespace IsuExtra
{
    public class StudentProfile
    {
        private Student _student;
        private Schedule _schedule;

        public StudentProfile(Student student, Schedule schedule)
        {
            _student = student;
            _schedule = schedule;
        }

        public Schedule GetSchedule()
        {
            return _schedule;
        }

        public char GetMegaFaculty()
        {
            return _student.GetGroup()[0];
        }

        public int GetId()
        {
            return _student.GetID();
        }

        public Student GetStudent()
        {
            return _student;
        }

        public void SetShedule(Schedule schedule)
        {
            _schedule = schedule;
        }
    }
}
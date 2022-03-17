using Isu;
using System.Collections.Generic;
using IsuExtra.Tools;
using NUnit.Framework;
namespace IsuExtra.Tests
{
    public class IsuExtraTest
    {
        private IIsuExtraService _isuExtraService;
        
        [SetUp]
        public void Setup()
        {
            _isuExtraService = new IsuExtraService();
        }

        public Schedule GeneratorEmptyWeek()
        {
            return new Schedule();
        }
        public ScheduleDay GenerateScheduleDayStudentMonday()
        {
            var FirstLessonMonday = new Lesson("8:20", "9:50", "Egorov", 332);
            var SecondLessonMonday = new Lesson("10:00", "11:30", "Gert", 331);
            var LessonsMonday = new List<Lesson>();
            LessonsMonday.Add(FirstLessonMonday);
            LessonsMonday.Add(SecondLessonMonday);
            LessonsMonday.Add(new Lesson());
            LessonsMonday.Add(new Lesson());
            LessonsMonday.Add(new Lesson());
            LessonsMonday.Add(new Lesson());
            LessonsMonday.Add(new Lesson());
            return new ScheduleDay(LessonsMonday);
        }
        public ScheduleDay GenerateScheduleDayStudentTuesday()
        {
            var FourthLessonTuesday = new Lesson("13:30", "15:00", "Mayatin", 466);
            var LessonsTuesday = new List<Lesson>();
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(FourthLessonTuesday);
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(new Lesson());
            return new ScheduleDay(LessonsTuesday);
        }

        public Schedule GenerateScheduleStudent()
        {
            Schedule schedule = GeneratorEmptyWeek();
            schedule.AddInfo(0, GenerateScheduleDayStudentMonday());
            schedule.AddInfo(1, GenerateScheduleDayStudentTuesday());
            return schedule;
        }
        public Schedule GenerateScheduleSecondStudent()
        {
            Schedule schedule = GeneratorEmptyWeek();
            schedule.AddInfo(0, GenerateScheduleDayStudentMonday());
            return schedule;
        }
        
        public Schedule GenerateScheduleOGNP()
        {
            Schedule schedule = GeneratorEmptyWeek();
            var FourthLessonTuesday = new Lesson("13:30", "15:00", "Ivanov", 111);
            var LessonsTuesday = new List<Lesson>();
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(FourthLessonTuesday);
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(new Lesson());
            var scheduleTuesday = new ScheduleDay(LessonsTuesday);
            schedule.AddInfo(1, scheduleTuesday);
            return schedule;
        }
        public Schedule GenerateScheduleOGNPWithOutMatching()
        {
            Schedule schedule = GeneratorEmptyWeek();
            var ThirdLessonTuesday = new Lesson("11:40", "13:10", "Ivanov", 111);
            var LessonsTuesday = new List<Lesson>();
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(ThirdLessonTuesday);
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(new Lesson());
            LessonsTuesday.Add(new Lesson());
            var scheduleTuesday = new ScheduleDay(LessonsTuesday);
            schedule.AddInfo(1, scheduleTuesday);
            return schedule;
        }
        
        [Test]
        public void CheckAddOgnp()
        {
            Setup();
            var itip = new MegaFaculty('M', "ITIP");
            OGNP ognp = _isuExtraService.AddOgnp(itip);
            Assert.AreEqual(true, _isuExtraService.CheckContainsOgnp(ognp));
        }
        
        [Test]
        public void RecordStudentForOgnpWithMatching()
        {
            Setup();
            var itip = new MegaFaculty('M', "ITIP");
            OGNP ognp = _isuExtraService.AddOgnp(itip);
            
            Flow flow1 = ognp.AddFlow(10);
            flow1.AddSchedule(GenerateScheduleOGNP());
            
            var isuService = new IsuService();

            GroupExtra groupExtra = _isuExtraService.AddGroup(isuService.AddGroup("R3201"), GenerateScheduleStudent());
            
            Student student = _isuExtraService.AddStudent(groupExtra, "James");
            
            Assert.Catch<IsuExtraException>(() =>
            {
                _isuExtraService.RegisterStudent(ognp, flow1, student);
            });
            
        }
        
        [Test]
        public void AnnulRecordOgnp()
        {
            Setup();
            var itip = new MegaFaculty('M', "ITIP");
            OGNP ognp = _isuExtraService.AddOgnp(itip);
            
            Flow flow1 = ognp.AddFlow(10);
            flow1.AddSchedule(GenerateScheduleOGNP());
            
            var isuService = new IsuService();

            GroupExtra groupExtra = _isuExtraService.AddGroup(isuService.AddGroup("R3201"), GenerateScheduleStudent());
            
            Student student = _isuExtraService.AddStudent(groupExtra, "James");
            flow1.DeleteStudent(student);
            Assert.AreEqual(false, flow1.IsStudentInFlow(student));
        }
        
        [Test]
        public void GetFlow()
        {
            Setup();
            var itip = new MegaFaculty('M', "ITIP");
            OGNP ognp = _isuExtraService.AddOgnp(itip);
            
            Flow flow1 = ognp.AddFlow(10);
            Flow flow2 = ognp.AddFlow(20);
            Assert.AreEqual(2, ognp.GetFlows().Count);
        }
        
        [Test]
        public void GetListStudentByGroupNumber()
        {
            Setup();
            var itip = new MegaFaculty('M', "ITIP");
            OGNP ognp = _isuExtraService.AddOgnp(itip);
            
            Flow flow1 = ognp.AddFlow(10);
            flow1.AddSchedule(GenerateScheduleOGNP());
            
            var isuService = new IsuService();

            GroupExtra groupExtra = _isuExtraService.AddGroup(isuService.AddGroup("R3201"), GenerateScheduleStudent());

            Student student = _isuExtraService.AddStudent(groupExtra, "James");
            Student student2 = _isuExtraService.AddStudent(groupExtra, "Bond");

            ognp.AddStudent(student, flow1);
            ognp.AddStudent(student2, flow1);
            Assert.AreEqual(2, flow1.GetListStudents().Count);
        }
        
        [Test]
        public void GetListNotRecordStudentsInGroup()
        {
            Setup();
            var itip = new MegaFaculty('M', "ITIP");
            OGNP ognp = _isuExtraService.AddOgnp(itip);
            
            Flow flow1 = ognp.AddFlow(10);
            flow1.AddSchedule(GenerateScheduleOGNP());
            
            var isuService = new IsuService();

            GroupExtra groupExtra = _isuExtraService.AddGroup(isuService.AddGroup("R3201"), GenerateScheduleStudent());

            Student student = _isuExtraService.AddStudent(groupExtra, "James");
            Student student2 = _isuExtraService.AddStudent(groupExtra, "Bond");
            ognp.AddStudent(student, flow1);
            Assert.AreEqual(1, _isuExtraService.StudentsNotJoin(groupExtra).Count);
        }
        
    }
}
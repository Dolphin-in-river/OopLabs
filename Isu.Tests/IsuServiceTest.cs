using System;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Setup();
            Group group = _isuService.AddGroup("M3101");
            Student student = _isuService.AddStudent(group, "Ivan");
            if ((group.GetStudent(-1, "Ivan") != null) && (student.GetGroup().Equals(group.GetName())))
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                var isu = new IsuService();
                Group group = isu.AddGroup("M3203");
                for (int i = 0; i < 21; i++)
                {
                    var student = new Student($"Student {i}", group.GetName());
                    group.AddStudent(student);
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                Setup();
                Group group = _isuService.AddGroup("1310111");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Setup();
            Group groupFirst = _isuService.AddGroup("M3101");
            Group groupSecond = _isuService.AddGroup("M3102");
            Student student = _isuService.AddStudent(groupFirst, "Ivan"); 
            _isuService.ChangeStudentGroup(student, groupSecond);
            if (groupFirst.GetStudent(-1, "Ivan") == null && groupSecond.GetStudent(-1, "Ivan") != null )
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
    }
}
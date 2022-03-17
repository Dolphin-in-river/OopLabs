using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Services;
using Isu.Tools;

namespace Isu
{
    public class IsuService : IIsuService
    {
        private const char FirstSymbolGroupName = 'A';
        private const char LastSymbolGroupName = 'Z';
        private const char SecondSymbolGroupName = '1';
        private const char ThirdOrFourthMinNumberGroupName = '0';
        private const char ThirdOrFourthMaxNumberGroupName = '9';
        private List<Group> _groups;
        public IsuService()
        {
            _groups = new List<Group>();
        }

        public Group AddGroup(string name)
        {
            CorrectInput(name);
            var newGroup = new Group(name);
            _groups.Add(newGroup);
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            if (!CheckAlreadyExists(group.GetName()))
            {
                throw new IsuException("Group don't contains in list of group");
            }

            var newStudent = new Student(name, group.GetName());
            group.AddStudent(newStudent);
            return newStudent;
        }

        public Student GetStudent(int id)
        {
            Student findStudent = null;
            foreach (Group group in _groups)
            {
                if (findStudent != null)
                {
                    break;
                }

                findStudent = group.GetStudent(id, null);
            }

            if (findStudent == null)
            {
                throw new IsuException("Student with this Id not found");
            }

            return findStudent;
        }

        public Student FindStudent(string name)
        {
            Student findStudent = null;
            foreach (Group group in _groups)
            {
                findStudent = group.GetStudent(-1, name);
                if (findStudent != null)
                {
                    break;
                }
            }

            if (findStudent == null)
            {
                Console.WriteLine("Student was not found");
            }

            return findStudent;
        }

        public List<Student> FindStudents(string groupName)
        {
            foreach (Group group in _groups)
            {
                if (group.GetName().Equals(groupName))
                {
                    return group.GetList();
                }
            }

            return null;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var resultList = new List<Student>();
            foreach (Group group in _groups)
            {
                if (Convert.ToInt32(group.GetName()[2]) == courseNumber.GetCourseNumber())
                {
                    resultList = resultList.Concat(group.GetList()).ToList();
                }
            }

            return resultList;
        }

        public Group FindGroup(string groupName)
        {
            foreach (Group group in _groups)
            {
                if (group.GetName().Equals(groupName))
                {
                    return group;
                }
            }

            return null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            var resultList = new List<Group>();
            foreach (Group group in _groups)
            {
                if (Convert.ToInt32(group.GetName()[2]) == courseNumber.GetCourseNumber())
                {
                    resultList.Add(group);
                }
            }

            return resultList;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (!_groups.Contains(newGroup))
            {
                throw new IsuException("The new Student Group isn't contains in the group list");
            }

            Group oldGroup = FindGroup(student.GetGroup());
            newGroup.AddStudent(student);
            oldGroup.Delete(student);
            student.SetGroup(newGroup.GetName());
        }

        private bool InCorrectGroupName(string name)
        {
            return CheckFirstSymbolGroupNumber(name) ||
                   CheckSecondSymbolGroupNumber(name) ||
                   CheckThirdSymbolGroupNumber(name) ||
                   CheckFourthSymbolGroupNumber(name) ||
                   CheckFifthSymbolGroupNumber(name);
        }

        private bool CheckFirstSymbolGroupNumber(string name)
        {
            return (name[0] < FirstSymbolGroupName) || (name[0] > LastSymbolGroupName);
        }

        private bool CheckSecondSymbolGroupNumber(string name)
        {
            return (name[1] < SecondSymbolGroupName) || (name[1] > ThirdOrFourthMaxNumberGroupName);
        }

        private bool CheckThirdSymbolGroupNumber(string name)
        {
            return (name[2] < ThirdOrFourthMinNumberGroupName) || (name[2] > ThirdOrFourthMaxNumberGroupName);
        }

        private bool CheckFourthSymbolGroupNumber(string name)
        {
            return (name[3] < ThirdOrFourthMinNumberGroupName) || (name[3] > ThirdOrFourthMaxNumberGroupName);
        }

        private bool CheckFifthSymbolGroupNumber(string name)
        {
            return (name[4] < ThirdOrFourthMinNumberGroupName) || (name[4] > ThirdOrFourthMaxNumberGroupName);
        }

        private bool CheckAlreadyExists(string name)
        {
            bool flag = false; // Group not contains in list of group
            foreach (Group group in _groups)
            {
                if (group.GetName().Equals(name))
                {
                    flag = true; // Group contains in list of group
                }
            }

            return flag;
        }

        private void CorrectInput(string name)
        {
            if (InCorrectGroupName(name))
            {
                throw new IsuException("Incorrect Input Group Name");
            }

            if (CheckAlreadyExists(name))
            {
                throw new IsuException("Group with this name already exists");
            }
        }
    }
}
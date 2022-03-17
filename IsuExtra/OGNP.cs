using System;
using System.Collections.Generic;
using Isu;
using IsuExtra.Tools;

namespace IsuExtra
{
    public class OGNP
    {
        private static int _nextId = 0;
        private int _id;
        private MegaFaculty _megaFaculty;
        private List<Flow> _flows = new List<Flow>();
        private Dictionary<int, Student> _registeredStudents = new Dictionary<int, Student>();
        public OGNP(MegaFaculty megaFaculty)
        {
            _megaFaculty = megaFaculty;
            _id = ++_nextId;
        }

        public void AddStudent(Student student, Flow flow)
        {
            if (_megaFaculty.GetFirstLetter() == student.GetGroup()[0])
            {
                throw new IsuExtraException("You can't join to this OGNP");
            }

            flow.AddStudent(student);
            _registeredStudents.Add(student.GetID(), student);
        }

        public Flow AddFlow(int spots)
        {
            var newFlow = new Flow(spots);
            _flows.Add(new Flow(spots));
            return newFlow;
        }

        public List<Flow> GetFlows()
        {
            return _flows;
        }

        public bool ConsistStudent(Student student)
        {
            return _registeredStudents.ContainsKey(student.GetID());
        }

        public int GetId()
        {
            return _id;
        }
    }
}
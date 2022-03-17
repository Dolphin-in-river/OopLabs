using System.Collections.Generic;
using Reports.DAL.Entities;

namespace Reports.Server.Services
{
    public class Filter
    {
        public List<EmployeeType> Types
        {
            get;
            set;
        }

        public Filter()
        {
            Types = new List<EmployeeType>();
        }

        public void AddType(EmployeeType type)
        {
            Types.Add(type);
        }
    }
}
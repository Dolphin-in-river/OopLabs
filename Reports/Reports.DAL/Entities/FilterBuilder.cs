using Reports.DAL.Entities;

namespace Reports.Server.Services
{
    public class FilterBuilder
    {
        private Filter _filter;

        public FilterBuilder()
        {
            _filter = new Filter();
        }


        public FilterBuilder BuildTypeEmployee(EmployeeType type)
        {
            _filter.Types.Add(type);
            return this;
        }

        public Filter GetResult()
        {
            return _filter;
        }
    }
}
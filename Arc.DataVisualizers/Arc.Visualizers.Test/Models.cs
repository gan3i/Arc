using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Visualizers.Test
{
    public class Employee
    {
        public string Name { get; set; }
        public int Salary { get; set; }
        public int DepartmentId { get; set; }
        public Department Dept { get; set; }
    }

    public class Department
    {
        public Department()
        {
            Employees = new List<Employee>();
        }
        public int DepartmentId { get; set; }
        public string DName { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }
}

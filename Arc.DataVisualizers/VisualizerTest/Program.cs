using Arc.DataVisualizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizerTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<Department> deptList = new List<Department>(new[] {
                new Department {DepartmentId=1,DName="PHP" },
                new Department {DepartmentId=2,DName="Mobile" },
                new Department {DepartmentId=3,DName="Java" }
            });

            List<Employee> empList = new List<Employee>(new[] {
                new Employee { Name= "First", Salary=2000,DepartmentId=1,Dept = deptList.FirstOrDefault(d=>d.DepartmentId == 1)},
                new Employee {  Name="Second",Salary=1000,DepartmentId=2,Dept = deptList.FirstOrDefault(d=>d.DepartmentId == 2)},
                new Employee { Name= "Third",Salary=1000,DepartmentId=3,Dept = deptList.FirstOrDefault(d=>d.DepartmentId == 3)},
                new Employee { Name= "Fourth",Salary=4000,DepartmentId=3,Dept = deptList.FirstOrDefault(d=>d.DepartmentId == 3)},
            });

            foreach (var dept in deptList)
            {
                dept.Employees = empList.Where(e => e.DepartmentId == dept.DepartmentId).ToList();
            }

            ListVisualizer.TestShowVisualizer(empList);

            //Console.ReadLine();
        }

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
}

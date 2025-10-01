using System.Collections.Generic;
using System.Linq;

namespace InfrastructureCheckers
{
    
    /*
        Group employees by department and find the department with the highest average salary
        Given a list of employees with department and salary, 
        group employees by department and identify the department with the highest average salary.
    */
    
    public class EmployeeCheck     {
        public class Employee
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Department { get; set; }
            public decimal Salary { get; set; }
        }

        public static void Filter()
        {
            // Example data
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Alice", Department = "HR", Salary = 55000 },
                new Employee { Id = 2, Name = "Bob", Department = "IT", Salary = 75000 },
                new Employee { Id = 3, Name = "Charlie", Department = "IT", Salary = 80000 },
                new Employee { Id = 4, Name = "Diana", Department = "Finance", Salary = 65000 },
                new Employee { Id = 5, Name = "Evan", Department = "Finance", Salary = 70000 },
                new Employee { Id = 6, Name = "Fiona", Department = "HR", Salary = 60000 },
                new Employee { Id = 7, Name = "George", Department = "IT", Salary = 72000 },
                new Employee { Id = 8, Name = "Hannah", Department = "Marketing", Salary = 50000 },
                new Employee { Id = 9, Name = "Ian", Department = "Marketing", Salary = 52000 },
                new Employee { Id = 10, Name = "Jane", Department = "Finance", Salary = 68000 }
            };

            var res = employees.GroupBy(d => new {d.Department, d.Name})
                .Select(s=>new {s.Key.Name,s.Key.Department})
                .ToList();

            var maxDep = employees.GroupBy(d => d.Department)
                .Select(s => new {Dep = s.Key, Sal = s.Max(v => v.Salary)})
                .ToList();
            
            // max avg salary
            var maxAvgDep = employees.GroupBy(d => d.Department)
                .Select(s => new {Dep = s.Key, Sal = s.Average(v => v.Salary)})
                .OrderByDescending(o=>o.Sal)
                .FirstOrDefault();
            
            
            var emp = employees
                .GroupBy(g=>g.Department)
                .Select(s=>new {dep = s.Key, avg = s.Average(v=>v.Salary)})
                .OrderByDescending(o=>o.avg)
                .FirstOrDefault();
            
            
            
            // largest payed per dep
            var topEmployeesPerDept = employees
                .GroupBy(e => e.Department)
                .Select(g => g.OrderByDescending(e => e.Salary).First())
                .ToList();

            var largestSlPerDep = employees.GroupBy(g=>g.Department)
                .Select(s=>s.OrderByDescending(v=>v.Salary).First()).ToList();
            
            // Select the top 3 highest paid employees in each department. Mode: GroupBy + projection. Focus: Grouping, ordering, Take.
            var largest3PerDep = employees.GroupBy(g=>g.Department)
                .Select(s=>s.OrderByDescending(v=>v.Salary).Take(3)).ToList();
        }
    }
    
    
}

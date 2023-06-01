using AnimalTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalTest.Repository.Common
{
    public interface IEmployeeRepository
    {
        Employee GetEmployeeById(Guid id);
      /*  IEnumerable<Employee> GetEmployees();
        bool InsertEmployee(Employee employee);
        bool DeleteEmployee(int studentID);
        bool UpdateEmployee(Employee employee);
        bool SaveChanges(); */

    }
}

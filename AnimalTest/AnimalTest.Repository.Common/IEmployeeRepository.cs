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
        bool CreateEmployee(Employee employee);
        bool UpdateEmployee(Guid id, Employee employee);
        List<Employee> GetAllEmployees();
        bool DeleteEmployee(Guid id);

    }
}

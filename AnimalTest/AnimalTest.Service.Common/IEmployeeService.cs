using AnimalTest.Models;
using AnimalTest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalTest.Service.Common
{
    public interface IEmployeeService 
    {
        Employee GetEmployeeById(Guid id);    
        bool CreateEmployee(Employee employee);
        bool UpdateEmployee(Guid id, Employee employee);
        List<Employee> GetAllEmployees();
        bool DeleteEmployee(Guid id);   
    }
}

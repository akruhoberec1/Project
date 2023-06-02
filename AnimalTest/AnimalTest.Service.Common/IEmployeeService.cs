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
        Task<Employee> GetEmployeeByIdAsync(Guid id);    
        Task<bool> CreateEmployeeAsync(Employee employee);
        Task<bool> UpdateEmployeeAsync(Guid id, Employee employee);
        Task<List<Employee>> GetAllEmployeesFilteredAsync(Paging paging, Sorting sorting, Filtering filtering);
        Task<bool> DeleteEmployeeAsync(Guid id);
    }
}

using AnimalTest.Common;
using AnimalTest.Models;
using AnimalTest.Repository;
using AnimalTest.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalTest.Service
{
    public class EmployeeService : IEmployeeService
    {
        public async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            EmployeeRepository repository = new EmployeeRepository();   
            Employee employee = await  repository.GetEmployeeByIdAsync(id); 

            return employee;
      
        }

        public async Task<bool> CreateEmployeeAsync(Employee employee)
        {
            EmployeeRepository repository = new EmployeeRepository();   
            
            bool isCreated = await repository.CreateEmployeeAsync(employee);  

            return isCreated;
        }

        public async Task<bool> UpdateEmployeeAsync(Guid id, Employee employee) 
        {
            EmployeeRepository repository = new EmployeeRepository();
            bool isUpdated = await repository.UpdateEmployeeAsync(id, employee);  

            return isUpdated;
            
        }

        public async Task<PagedList<Employee>> GetAllEmployeesFilteredAsync(Paging paging, Sorting sorting, Filtering filtering)
        {
            EmployeeRepository repository = new EmployeeRepository();
            PagedList<Employee> employees = await repository.GetAllEmployeesFilteredAsync(paging, sorting, filtering);   
            
            return employees;
        }

        public async Task<bool> DeleteEmployeeAsync(Guid id) 
        {
            EmployeeRepository repository = new EmployeeRepository();
            bool isDeleted = await repository.DeleteEmployeeAsync(id); 
            return isDeleted;
        }

    }
}

using AnimalTest.Common;
using AnimalTest.Models;
using AnimalTest.Repository;
using AnimalTest.Repository.Common;
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
        private readonly IEmployeeRepository _employeeRepository;   
        public EmployeeService(IEmployeeRepository employeeRepository) 
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {  
            Employee employee = await  _employeeRepository.GetEmployeeByIdAsync(id); 

            return employee;
      
        }

        public async Task<bool> CreateEmployeeAsync(Employee employee)
        {  
            
            bool isCreated = await _employeeRepository.CreateEmployeeAsync(employee);  

            return isCreated;
        }

        public async Task<bool> UpdateEmployeeAsync(Guid id, Employee employee) 
        {
            bool isUpdated = await _employeeRepository.UpdateEmployeeAsync(id, employee);  

            return isUpdated;
            
        }

        public async Task<PagedList<Employee>> GetAllEmployeesFilteredAsync(Paging paging, Sorting sorting, Filtering filtering)
        {
            PagedList<Employee> employees = await _employeeRepository.GetAllEmployeesFilteredAsync(paging, sorting, filtering);   
            
            return employees;
        }

        public async Task<bool> DeleteEmployeeAsync(Guid id) 
        {
            bool isDeleted = await _employeeRepository.DeleteEmployeeAsync(id); 
            return isDeleted;
        }

    }
}

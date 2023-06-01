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
        public Employee GetEmployeeById(Guid id)
        {
            EmployeeRepository repository = new EmployeeRepository();   
            Employee employee = repository.GetEmployeeById(id); 

            return employee;
      
        }

        public bool CreateEmployee(Employee employee)
        {
            EmployeeRepository repository = new EmployeeRepository();   
            
            bool isCreated = repository.CreateEmployee(employee);  

            return isCreated;
        }

        public bool UpdateEmployee(Guid id, Employee employee) 
        {
            EmployeeRepository repository = new EmployeeRepository();
            bool isUpdated = repository.UpdateEmployee(id, employee);  

            return isUpdated;
            
        }

        public List<Employee> GetAllEmployees()
        {
            EmployeeRepository repository = new EmployeeRepository();
            List<Employee> employees = repository.GetAllEmployees();   
            
            return employees;
        }

        public bool DeleteEmployee(Guid id) 
        {
            EmployeeRepository repository = new EmployeeRepository();
            bool isDeleted = repository.DeleteEmployee(id); 
            return isDeleted;
        }

    }
}

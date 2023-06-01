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

    }
}

using AnimalTest.Models;
using AnimalTest.Repository;
using AnimalTest.Service;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;

namespace AnimalTest.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {

        
        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> Get()
        {
            EmployeeService service = new EmployeeService();
            List<Employee> employees = await service.GetAllEmployeesAsync();
            if (employees == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "We couldn't find any employees.");
            }

            //List<EmployeeRest> mappedEmployees = MapEmployeeToRest(employees);

            return Request.CreateResponse(HttpStatusCode.OK, employees);

        }

        //GET BY ID WORKS REPO PATTERN
        [HttpGet]
        [Route("{id}")]
        public async Task<HttpResponseMessage> Get(Guid id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please insert valid ID.");
            }

            Employee employee = await GetEmployeeByIdAsync(id);
            EmployeeRest employeeToShow = MapEmployeeToRest(employee);

            if (employeeToShow == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Did not find an employee.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, employeeToShow);

        }


        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> Post([FromBody]EmployeeRest employeeRest)
        {
            Employee employee = MapEmployeeFromRest(employeeRest);
            EmployeeService employeeService = new EmployeeService();
            bool isCreated = await employeeService.CreateEmployeeAsync(employee);

            if (isCreated == false)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, your request was pretty bad! Insert valid fields!");
            }

            return Request.CreateResponse(HttpStatusCode.OK, "You posted an employee successfully");
          
        }
        


        [HttpPut]
        [Route("{id}")]
        public async Task<HttpResponseMessage> Put(Guid id, [FromBody] EmployeeRest employeeRest)
        {
            if(employeeRest == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, the request is invalid.");
            }

            Employee employee = MapEmployeeFromRest(employeeRest);
            EmployeeService employeeService = new EmployeeService();
            bool isUpdated = await employeeService.UpdateEmployeeAsync(id, employee);

            if(isUpdated == false)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Couldn't update employee.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "The employee was updated successfully!");
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            EmployeeService service = new EmployeeService();
            bool isDeleted = await service.DeleteEmployeeAsync(id);    

            if( isDeleted == false)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong, couldn't delete.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "All good, employee deleted! You're free from his bad work.");
        }


        [HttpGet]
        [Route("{id}")]
        private async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {

            EmployeeService employeeService = new EmployeeService();
            Employee employee = await employeeService.GetEmployeeByIdAsync(id);
            //EmployeeRest mappedEmployee = MapEmployeeToRest(employee); // List<EmployeeRest> mappedEmployee = MapEmployeeToRest(new[] {employee});

            return employee;

        }

        private EmployeeRest MapEmployeeToRest(Employee employee)
        {


            EmployeeRest employeeRest = new EmployeeRest()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                OIB = employee.OIB,
                Salary = employee.Salary,
                Certified = employee.Certified
            };

            return employeeRest;
        }

        private Employee MapEmployeeFromRest(EmployeeRest employeeRest)
        {
            Employee employee = new Employee()
            {
                FirstName = employeeRest.FirstName,
                LastName = employeeRest.LastName,
                OIB = employeeRest.OIB, 
                Salary = employeeRest.Salary,
                Certified = employeeRest.Certified  
            };

            return employee;
        }

        //private List<EmployeeRest> MapEmployeeListToRest(List<Employee> employees)
        //{


        //    List<EmployeeRest> employeeRests = foreach (employees as e)
        //    {

        //    };

        //}






    }
}
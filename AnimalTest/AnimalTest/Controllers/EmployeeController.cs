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
        public HttpResponseMessage Get()
        {
            List<Employee> employees = new List<Employee>();

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
            

            using (connection)
            {
                try
                {
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand("SELECT a.FirstName, a.LastName, a.OIB, b.Salary, b.Certified FROM Employee as b INNER JOIN Person as a ON a.Id = b.Id", connection);
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //popunimo listu objektima
                            employees.Add(new Employee()
                            {
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                OIB = reader["OIB"].ToString(),
                                Salary = (decimal)reader["Salary"],
                                Certified = (bool)reader["Certified"]

                            });
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, employees);
                    }
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No rows found.");
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
                }
            }
        }

        //GET BY ID WORKS REPO PATTERN
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(Guid id)
        {
            EmployeeRest employeeToShow = GetEmployeeById(id);

            if (employeeToShow == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Did not find an employee.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, employeeToShow);

        }


        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Employee employee)
        {

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            using (connection)
            {
                try
                {
                    if (employee == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, your request was pretty bad! Insert valid fields!");
                    }

                    Guid id = Guid.NewGuid();
                    connection.Open();
                    NpgsqlTransaction transaction = connection.BeginTransaction();


                    NpgsqlCommand cmd = new NpgsqlCommand($"INSERT INTO Person (Id, FirstName, LastName, OIB) VALUES (@Id, @FirstName, @LastName, @OIB)", connection);

                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.Parameters.AddWithValue("FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("OIB", employee.OIB);

                    int affectedRowsPerson = cmd.ExecuteNonQuery();



                    NpgsqlCommand cmdEmployee = new NpgsqlCommand($"INSERT INTO Employee (Id, Salary, Certified) VALUES(@Id,@Salary,@Certified)", connection);

                    cmdEmployee.Parameters.AddWithValue("Id", id);
                    cmdEmployee.Parameters.AddWithValue("Salary", employee.Salary);
                    cmdEmployee.Parameters.AddWithValue("Certified", employee.Certified);

                    int affectedRowsEmployee = cmdEmployee.ExecuteNonQuery();

                    transaction.Commit();


                    if (affectedRowsPerson > 0 && affectedRowsEmployee > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, employee);
                    }

                    return Request.CreateResponse(HttpStatusCode.NotFound, "Sorry, insert didn't take.");

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
        }
        


        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(Guid id, [FromBody] Employee employee)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            EmployeeRest currentEmployee = GetEmployeeById(id);

            if (currentEmployee == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "This user does not exist");
            }
            try
            {
                using (connection)
                {
                    StringBuilder queryBuilderPerson = new StringBuilder();
                    NpgsqlCommand cmd = new NpgsqlCommand("",connection);
                    queryBuilderPerson.Append("UPDATE Person SET ");
                    connection.Open();
                    char commaToRemove = ',';

                    NpgsqlTransaction transaction = connection.BeginTransaction();
                    //person values
                    queryBuilderPerson.Append("FirstName = @firstName,");
                    cmd.Parameters.AddWithValue("@firstName", employee.FirstName);
                    queryBuilderPerson.Append(" LastName = @lastName,");
                    cmd.Parameters.AddWithValue("@lastName", employee.LastName);
                    queryBuilderPerson.Append(" OIB = @OIB,");
                    cmd.Parameters.AddWithValue("@OIB", employee.OIB);

                    string queryPerson = queryBuilderPerson.ToString().TrimEnd(commaToRemove);
                    StringBuilder finalPersonQuery = new StringBuilder();
                    finalPersonQuery.Append(queryPerson);

                    finalPersonQuery.Append(" WHERE Id = @id");
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandText = finalPersonQuery.ToString();
                    cmd.ExecuteNonQuery();

                    int affectedRowsPerson = cmd.ExecuteNonQuery();


                    //employee values

                    StringBuilder queryBuilderEmployee = new StringBuilder();
                    NpgsqlCommand cmdEmployee = new NpgsqlCommand("", connection);
                    queryBuilderEmployee.Append("UPDATE Employee SET ");
                    
                    queryBuilderEmployee.Append("Salary = @salary,");
                    cmdEmployee.Parameters.AddWithValue("@salary", employee.Salary);
                    queryBuilderEmployee.Append("Certified = @certified,");
                    cmdEmployee.Parameters.AddWithValue("@certified", employee.Certified);


                    string queryEmployee = queryBuilderEmployee.ToString().TrimEnd(commaToRemove);
                    StringBuilder finalEmployeeQuery = new StringBuilder();
                    finalEmployeeQuery.Append(queryEmployee);

                    finalEmployeeQuery.Append(" WHERE Id = @id");
                    cmdEmployee.Parameters.AddWithValue("@id", id);
                    cmdEmployee.CommandText = finalEmployeeQuery.ToString();
                    cmdEmployee.ExecuteNonQuery();

                    transaction.Commit();

                    int affectedRowsEmployee = cmdEmployee.ExecuteNonQuery();
                    if(affectedRowsEmployee > 0 && affectedRowsPerson > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Employee updated successfully!");
                    }
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You typed in something weird, please try again.");
                    
                }
            }
            catch (Exception ex)
            {
                
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }


        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            try
            {
                EmployeeRest employee = GetEmployeeById(id);

                if (employee == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The employee is already non-existant");
                }
                using (connection)
                {    
                    connection.Open();
                    NpgsqlTransaction transaction = connection.BeginTransaction();
                    NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Employee WHERE Id=@id", connection);
                    cmd.Transaction = transaction;  

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    NpgsqlCommand cmdPerson = new NpgsqlCommand("DELETE FROM Person WHERE Id=@id", connection);
                    cmdPerson.Transaction = transaction;
                    cmdPerson.Parameters.AddWithValue("@id", id);
                    cmdPerson.ExecuteNonQuery();

                    transaction.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, "Employee deleted successfuly!");

                }
            }
            catch (Exception ex)
            {
                
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        [HttpGet]
        [Route("{id}")]
        private EmployeeRest GetEmployeeById(Guid id)
        {
            EmployeeRest employee = MapEmployeeToRest(id);

            return employee;

        }

        private EmployeeRest MapEmployeeToRest(Guid id)
        {
            EmployeeService employeeService = new EmployeeService();
            Employee employee = employeeService.GetEmployeeById(id);

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






    }
}
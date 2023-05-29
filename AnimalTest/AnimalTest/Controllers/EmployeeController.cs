using AnimalTest.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AnimalTest.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]Employee employee)
        {

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
            connection.Open();

            Guid uuid = Guid.NewGuid();
            using (connection)
            {
                try
                {
                    if (employee == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, your request was pretty bad! Insert valid fields!");
                    }

                    Guid Id = Guid.NewGuid();

                    NpgsqlCommand cmd = new NpgsqlCommand($"INSERT INTO Person (Id, FirstName, LastName, OIB) VALUES (@Id, @FirstName, @LastName, @OIB)", connection);

                    cmd.Parameters.AddWithValue("Id", uuid);
                    cmd.Parameters.AddWithValue("FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("OIB", employee.OIB);

                    int affectedRowsPerson = cmd.ExecuteNonQuery();


                    NpgsqlCommand cmdEmployee = new NpgsqlCommand($"INSERT INTO Employee (Id, Salary, Certified) VALUES(@Id,@Salary,@Certified", connection);

                    cmdEmployee.Parameters.AddWithValue("Id", uuid);
                    cmdEmployee.Parameters.AddWithValue("Salary", employee.Salary);
                    cmdEmployee.Parameters.AddWithValue("Certified", employee.Certified);

                    int affectedRowsEmployee = cmdEmployee.ExecuteNonQuery();


                    if(affectedRowsPerson > 0 && affectedRowsEmployee > 0) 
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, employee);
                    }

                    return Request.CreateResponse(HttpStatusCode.NotFound, "Sorry, insert didn't take.");

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }



        //PUT koristiti string builder

    }
}
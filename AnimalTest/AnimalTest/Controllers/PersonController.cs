using AnimalTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Security.Cryptography;
using System.Data;
using Npgsql;

namespace AnimalTest.Controllers
{
    [RoutePrefix("api/person")]

    public class PersonController : ApiController
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;






        //POst method
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post(Person person)
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            Guid Uuid = Guid.NewGuid(); 
            using (connection)
            {
                try
                {
                    if (person == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, your request was pretty bad! Insert valid fields!");
                    }

                    Guid Id = Guid.NewGuid();

                    NpgsqlCommand cmd = new NpgsqlCommand ($"INSERT INTO \"Person\" (Uuid, FirstName, LastName, OIB) VALUES (@Id, @FirstName, @LastName, @OIB)");

         
                    
                    cmd.Parameters.AddWithValue("Uuid", person.Id);
                    cmd.Parameters.AddWithValue("FirstName", person.FirstName);
                    cmd.Parameters.AddWithValue("LastName", person.LastName);
                    cmd.Parameters.AddWithValue("OIB", person.OIB);
                    cmd.Parameters.AddWithValue("IsEmployee", person.IsEmployee);

                    if (person.IsEmployee)
                    {
                        NpgsqlCommand cmdEmployee = new NpgsqlCommand($"INSERT INTO \"Employee\" (Uuid, Salary, Certified) VALUES(@Id,@Salary,@Certified");
                    }


                    int NoOfAffetctedRows = cmd.ExecuteNonQuery();



                    
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
                }
                connection.Close();

            }
            
        }

    }
}
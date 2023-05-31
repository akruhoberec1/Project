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
using System.Runtime.Remoting.Messaging;

namespace AnimalTest.Controllers
{
    [RoutePrefix("api/person")]

    public class PersonController : ApiController
    {


        //get method
        [Route("")]
        public HttpResponseMessage Get()
        {
            List<Person> people = new List<Person>();

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
            connection.Open();

            using(connection) 
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM Person ORDER BY LastName, FirstName", connection);

                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //popunimo listu objektima
                            people.Add(new Person()
                            {
                                Id  = (Guid)reader["Id"],
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                OIB = reader["OIB"].ToString()
                            });
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, people);
                    }
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No rows found.");
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, ex);
                }
            }      
        }













       [HttpGet]
       [Route("{id}")]
       public HttpResponseMessage GetById(int id)
       {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
            connection.Open();
            int personId = id;
            using (connection)
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM Person WHERE Id = @Id", connection);
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while(reader.Read()) 
                        {
                            Person person = new Person()
                            {
                                Id = (Guid)reader["Id"],
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                OIB  = reader["OIB"].ToString()
                            };

                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No person by given Id");
                }
                catch(Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound , ex);
                }
            }
       }


    }
}
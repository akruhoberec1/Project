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


        //get method
        [Route("")]
        public HttpResponseMessage Get()
        {
            List<Person> people = new List<Person>();
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
            connection.Open();


            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM Person ORDER BY LastName, FirstName", connection);

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Person person = new Person()
                        {
                            Id = [Guid]Read(""),
                            Title = title,
                            Tags = new List<Tag>()
                        };

                        Books.Add(book);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "No rows found.");
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex);
            }
        }




        //POst method




    }
}
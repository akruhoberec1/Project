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
    [RoutePrefix("api/guest")]
    public class GuestController : PersonController
    {

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post(Guest guest)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;


            var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            Guid uuid = Guid.NewGuid();
            using (connection)
            {
                try
                {
                    if (guest == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, your request was pretty bad! Insert valid fields!");
                    }

                    Guid Id = Guid.NewGuid();

                    NpgsqlCommand cmd = new NpgsqlCommand($"INSERT INTO \"Person\" (Uuid, FirstName, LastName, OIB) VALUES (@Id, @FirstName, @LastName, @OIB)");
                    NpgsqlCommand cmdGuest = new NpgsqlCommand($"INSERT INTO \"Guest\" (Uuid, Email, Phone) VALUES(@Id,@Email,@Phone");


                    cmd.Parameters.AddWithValue("Id", uuid);
                    cmd.Parameters.AddWithValue("FirstName", guest.FirstName);
                    cmd.Parameters.AddWithValue("LastName", guest.LastName);
                    cmd.Parameters.AddWithValue("OIB", guest.OIB);
                    cmdGuest.Parameters.AddWithValue("Id", uuid);
                    cmdGuest.Parameters.AddWithValue("Salary", guest.Email);
                    cmdGuest.Parameters.AddWithValue("Certified", guest.Phone);


                    int affectedRowsPerson = cmd.ExecuteNonQuery();
                    int affectedRowsGuest = cmdGuest.ExecuteNonQuery();

                    if (affectedRowsPerson > 0 && affectedRowsGuest > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, guest);
                    }

                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, the request is bad.");

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }
    }
}
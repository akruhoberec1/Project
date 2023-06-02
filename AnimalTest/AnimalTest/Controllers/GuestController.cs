using AnimalTest.Models;
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

namespace AnimalTest.Controllers
{
    [RoutePrefix("api/guest")]
    public class GuestController : ApiController
    {

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            List<Guest> guests = new List<Guest>();

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
            connection.Open();

            using (connection)
            {
                try
                {

                    NpgsqlCommand cmd = new NpgsqlCommand("SELECT a.FirstName, a.LastName, a.OIB, b.Email, b.Phone FROM Guest as b INNER JOIN Person as a ON a.Id = b.Id", connection);
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //popunimo listu objektima
                            guests.Add(new Guest()
                            {
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                OIB = reader["OIB"].ToString(),
                                Email = (string)reader["Email"],
                                Phone = (int)reader["Phone"]

                            });
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, guests);
                    }
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Cannot find any guests!");
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, ex);
                }
            }
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(Guid id)
        {
            Guest guest = GetGuestById(id);

            if (guest == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Guest cannot be found, something went wrong.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, guest);

        }


        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody] Guest guest)
        {

            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            using (connection)
            {
                try
                {
                    if (guest == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, your request was bad! Insert valid fields!");
                    }

                    Guid id = Guid.NewGuid();
                    connection.Open();
                    NpgsqlTransaction transaction = connection.BeginTransaction();


                    NpgsqlCommand cmd = new NpgsqlCommand($"INSERT INTO Person (Id, FirstName, LastName, OIB) VALUES (@Id, @FirstName, @LastName, @OIB)", connection);

                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.Parameters.AddWithValue("FirstName", guest.FirstName);
                    cmd.Parameters.AddWithValue("LastName", guest.LastName);
                    cmd.Parameters.AddWithValue("OIB", guest.OIB);

                    int affectedRowsPerson = cmd.ExecuteNonQuery();



                    NpgsqlCommand cmdGuest = new NpgsqlCommand($"INSERT INTO Guest (Id, Email, Phone) VALUES(@Id,@Email,@Phone)", connection);

                    cmdGuest.Parameters.AddWithValue("Id", id);
                    cmdGuest.Parameters.AddWithValue("Email", guest.Email);
                    cmdGuest.Parameters.AddWithValue("Phone", guest.Phone);

                    int affectedRowsEmployee = cmdGuest.ExecuteNonQuery();

                    transaction.Commit();


                    if (affectedRowsPerson > 0 && affectedRowsEmployee > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, guest);
                    }

                    return Request.CreateResponse(HttpStatusCode.NotFound, "Sorry, insert didn't take.");

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }



        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Put(Guid id, [FromBody] Guest guest)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            Guest getGuest = GetGuestById(id);

            if (getGuest == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "This user does not exist");
            }
            try
            {
                using (connection)
                {
                    StringBuilder queryBuilderPerson = new StringBuilder();
                    NpgsqlCommand cmd = new NpgsqlCommand("", connection);
                    queryBuilderPerson.Append("UPDATE Person SET ");
                    connection.Open();
                    char commaToRemove = ',';

                    NpgsqlTransaction transaction = connection.BeginTransaction();
                    //person values
                    queryBuilderPerson.Append("FirstName = @firstName,");
                    cmd.Parameters.AddWithValue("@firstName", guest.FirstName);
                    queryBuilderPerson.Append(" LastName = @lastName,");
                    cmd.Parameters.AddWithValue("@lastName", guest.LastName);
                    queryBuilderPerson.Append(" OIB = @OIB,");
                    cmd.Parameters.AddWithValue("@OIB", guest.OIB);

                    string queryPerson = queryBuilderPerson.ToString().TrimEnd(commaToRemove);
                    StringBuilder finalPersonQuery = new StringBuilder();
                    finalPersonQuery.Append(queryPerson);

                    finalPersonQuery.Append(" WHERE Id = @id");
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandText = finalPersonQuery.ToString();
                    cmd.ExecuteNonQuery();

                    int affectedRowsPerson = cmd.ExecuteNonQuery();


                    //employee values

                    StringBuilder queryBuilderGuest = new StringBuilder();
                    NpgsqlCommand cmdGuest = new NpgsqlCommand("", connection);
                    queryBuilderGuest.Append("UPDATE Guest SET ");

                    queryBuilderGuest.Append("Email = @email,");
                    cmdGuest.Parameters.AddWithValue("@email", guest.Email);
                    queryBuilderGuest.Append("Phone = @phone,");
                    cmdGuest.Parameters.AddWithValue("@phone", guest.Phone);


                    string queryGuest = queryBuilderGuest.ToString().TrimEnd(commaToRemove);
                    StringBuilder finalGuestQuery = new StringBuilder();
                    finalGuestQuery.Append(queryGuest);

                    finalGuestQuery.Append(" WHERE Id = @id");
                    cmdGuest.Parameters.AddWithValue("@id", id);
                    cmdGuest.CommandText = finalGuestQuery.ToString();
                    cmdGuest.ExecuteNonQuery();

                    transaction.Commit();

                    int affectedRowsGuest = cmdGuest.ExecuteNonQuery();
                    if (affectedRowsGuest > 0 && affectedRowsPerson > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Guest updated successfully!");
                    }
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You typed in something weird, please try again.");

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }


        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            try
            {
                Guest guest = GetGuestById(id);

                if (guest == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Guest is already non-existant");
                }
                using (connection)
                {
                    connection.Open();
                    NpgsqlTransaction transaction = connection.BeginTransaction();
                    NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Guest WHERE Id=@id", connection);
                    cmd.Transaction = transaction;

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    NpgsqlCommand cmdPerson = new NpgsqlCommand("DELETE FROM Guest WHERE Id=@id", connection);
                    cmdPerson.Transaction = transaction;
                    cmdPerson.Parameters.AddWithValue("@id", id);
                    cmdPerson.ExecuteNonQuery();

                    transaction.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, "Guest deleted successfuly!");

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [HttpGet]
        [Route("{id}")]
        private Guest GetGuestById(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());

            using (connection)
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT a.FirstName, a.LastName, a.OIB, b.Email, b.Phone FROM Guest as b INNER JOIN Person as a ON a.Id = b.Id WHERE a.Id=@Id", connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@Id", id);


                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    Guest guest = new Guest();
                    guest.Id = id;
                    guest.FirstName = (string)reader["FirstName"];
                    guest.LastName = (string)reader["LastName"];
                    guest.OIB = (string)reader["OIB"];
                    guest.Email = (string)reader["Email"];
                    guest.Phone = (int)reader["Phone"];
                    return guest;
                }

                return null;
            }
        }



    }
}
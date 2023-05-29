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


        //get method
        //[Route("")]
        //public HttpResponseMessage Get()
        //{

        //    NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
        //    connection.Open();

        //    try
        //    {

        //    }
        //    catch 
        //    { 

        //    }
        //}




        //POst method




    }
}
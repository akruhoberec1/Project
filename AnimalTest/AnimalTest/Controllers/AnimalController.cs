﻿using AnimalTest.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;

namespace AnimalTest.Controllers
{
    public class AnimalController : ApiController
    {



        public static List<Animal> animals = new List<Animal>
            {
               new Animal { Id=1, Name="Dog", Sound="VauVau" },
               new Animal { Id=2, Name="Turtle", Sound="Hellooo" },
               new Animal { Id = 3, Name = "Fish", Sound = " " },
               new Animal { Id = 4, Name = "Cat", Sound = "Meow" },
               new Animal { Id = 5, Name = "Cow", Sound = "Mooo!" }
            };



public HttpResponseMessage Get()
        {
            if(animals.Count() == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound,"Sorry, we can't find any animals for you");
            }
            return Request.CreateResponse<List<Animal>>(HttpStatusCode.OK, animals);
        }


        public HttpResponseMessage Get(int id)
        {
            if(id < 0 || animals.All(m => m.Id != id))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"Sorry, we cannot find an animal using Id number {id}.");
            }
            Animal animal = animals.Find(m => m.Id == id);
            return Request.CreateResponse<Animal>(HttpStatusCode.OK, animal);
        }

        public HttpResponseMessage Post([FromBody] Animal ani)
        {

            Animal animal = new Animal();
            animal.Id = ani.Id;
            animal.Name = ani.Name;
            animal.Sound = ani.Sound;

            if (animal.Id == 0 || animal.Name == null || animal.Id < 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Sorry, we cannot post an animal");
            }

            animals.Add(animal);


            return Request.CreateResponse<List<Animal>>(HttpStatusCode.OK, animals);
        }


        public HttpResponseMessage Put(int id, [FromBody]  Animal animal)
        {
            if (id <= 0 || animals.All(m => m.Id != id))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"Sorry, we cannot find an animal using id number {id}.");
            }

            Animal animalToPut = animals.Find(u => u.Id == id);

            if (animalToPut == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Sorry, there is no animal to update.");
            }

            animalToPut.Id = id;
            animalToPut.Name = animal.Name;
            animalToPut.Sound = animal.Sound;
            
            return Request.CreateResponse<Animal>(HttpStatusCode.OK, animalToPut);

        }


        public HttpResponseMessage Delete(int id)
        {

            if(id < 0 || animals.All(m => m.Id != id))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Sorry, we cannot find an animal using id number {id}.");
            }


            Animal animalToRemove = animals.Find(r => r.Id == id);
            if (animalToRemove == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"There is no animal you're looking for.");
            }


            bool result = animals.Remove(animalToRemove);
            if(result != true)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, $"Sorry, but the animal {animalToRemove.Name} is still alive!");
            }

            return Request.CreateResponse(HttpStatusCode.OK, $"{animalToRemove.Name} just went extinct from the database!");
        }

    }

}

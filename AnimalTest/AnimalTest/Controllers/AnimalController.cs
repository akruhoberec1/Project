using AnimalTest.Models;
using System.Collections;
using System.Collections.Generic;
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
               new Animal { Id=3, Name="Fish", Sound=" " },
               new Animal { Id=4, Name="Cat", Sound="Meow" },
               new Animal { Id=5, Name="Cow", Sound="Mooo!" }
            };



        public IEnumerable<Animal> Get()
        {

            return animals; 
        }


        public Animal Get(int id)
        {
            return animals.Find(m => m.Id == id);
        }

        public List<Animal> Post([FromBody] Animal ani)
        {
            Animal animal = new Animal();
            animal.Id = ani.Id;
            animal.Name = ani.Name;
            animal.Sound = ani.Sound;

            animals.Add(animal);
            return animals;
        }


        public Animal Put(int id, [FromBody]  Animal ani)
        {

            Animal animalToPut = animals.Find(u => u.Id == id);

            animalToPut.Id = ani.Id;
            animalToPut.Name = ani.Name;
            animalToPut.Sound = ani.Sound;

            return animalToPut;

        }


        public List<Animal> Delete(int id)
        {
            Animal animalToRemove = animals.Find(r => r.Id == id);

            if(animalToRemove != null) { 
            animals.Remove(animalToRemove);
            }


            return animals;

        }
    }
}

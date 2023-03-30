using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CretaceousClient.Models
{
  public class Animal
  {
    public int AnimalId { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public int Age { get; set; }


    // GetAnimals() method handles calling a method that queries our API for all Animal objects and deserializing the API's response.
    public static List<Animal> GetAnimals()
    {
      // NOTE: we don't pass an API key as an argument to ApiHelper.GetAll();. Your personal API will not require a key unless you add Token-Based Authentication through your further exploration.
      var apiCallTask = ApiHelper.GetAll(); // ApiHelper class model will contain .GetAll() definition
      var result = apiCallTask.Result;

      JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(result);
      List<Animal> animalList = JsonConvert.DeserializeObject<List<Animal>>(jsonResponse.ToString());

      return animalList;
    }

    public static List<Animal> GetAnimalsWithPagination(int pageFromClient)
    {
      // NOTE: we don't pass an API key as an argument to ApiHelper.GetAll();. Your personal API will not require a key unless you add Token-Based Authentication through your further exploration.
      var apiCallTask = ApiHelper.GetAllWithPagination(pageFromClient); // ApiHelper class model will contain .GetAll() definition
      var result = apiCallTask.Result;

      JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(result);
      List<Animal> animalList = JsonConvert.DeserializeObject<List<Animal>>(jsonResponse.ToString());

      return animalList;
    }

    public static Animal GetDetails(int id)
    {
      var apiCallTask = ApiHelper.Get(id);
      var result = apiCallTask.Result;

      JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(result);
      Animal animal = JsonConvert.DeserializeObject<Animal>(jsonResponse.ToString());

      return animal;
    }

    public static void Post(Animal animal)
    {
      string jsonAnimal = JsonConvert.SerializeObject(animal);
      ApiHelper.Post(jsonAnimal);
    }

    public static void Put(Animal animal)
    {
      string jsonAnimal = JsonConvert.SerializeObject(animal);
      ApiHelper.Put(animal.AnimalId, jsonAnimal);
    }

    public static void Delete(int id)
    {
      ApiHelper.Delete(id);
    }
  }
}
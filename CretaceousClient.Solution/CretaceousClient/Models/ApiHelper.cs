using System.Threading.Tasks;
using RestSharp;
using System.Linq;

namespace CretaceousClient.Models
{
  public class ApiHelper
  {
    public static async Task<string> GetAll()
    {
      RestClient client = new RestClient("http://localhost:5133/");
      RestRequest request = new RestRequest($"api/animals", Method.Get);
      RestResponse response = await client.GetAsync(request);
      return response.Content;
    }

    public static async Task<string> GetAllWithPagination(int pageFromClient)
    {
      RestClient client = new RestClient("http://localhost:5133/");
      RestRequest request = new RestRequest($"api/Animals/api/tryjson?pagenumber={pageFromClient}", Method.Get); // pagination.next();
      // RestRequest request = new RestRequest($"api/Animals/api/tryjson", Method.Get);

      // RestResponse response = await client.GetAsync(request);
      // RestResponse headerString = response.Headers.ElementAt(3);
      // return response.Content;

      // Execute the request and get the response
      RestResponse response = await client.GetAsync(request);

      // Check if the request was successful
      // if (response.IsSuccessful)
      // {
        // Get the headers from the response
        // IEnumerable<object> headers = response.Headers;

        var headers = response.Headers.ElementAt(3).Value.ToString();

        // Loop through the headers and output them to the console
        // foreach (var header in headers)
        // {
        //     return $"{header.Name}: {header.Value}";
        // }
        // }
        // else
        // {
        //     return $"Error: {response.ErrorMessage}";
        // }
        return headers;
      }

      public static async Task<string> Get(int id)
      {
        RestClient client = new RestClient("http://localhost:5133/");
        RestRequest request = new RestRequest($"api/animals/{id}", Method.Get);
        RestResponse response = await client.GetAsync(request);
        return response.Content;
      }
      // CretaceousApi.Solution/Controllers/AnimalsController.cs
      public static async void Post(string newAnimal)
      {
        RestClient client = new RestClient("http://localhost:5133/");
        RestRequest request = new RestRequest($"api/animals", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        request.AddJsonBody(newAnimal);
        await client.PostAsync(request);
      }

      public static async void Put(int id, string newAnimal)
      {
        RestClient client = new RestClient("http://localhost:5133/");
        RestRequest request = new RestRequest($"api/animals/{id}", Method.Put);
        request.AddHeader("Content-Type", "application/json");
        request.AddJsonBody(newAnimal);
        await client.PutAsync(request);
      }

      public static async void Delete(int id)
      {
        RestClient client = new RestClient("http://localhost:5133/");
        RestRequest request = new RestRequest($"api/animals/{id}", Method.Delete);
        request.AddHeader("Content-Type", "application/json");
        await client.DeleteAsync(request);
      }
    }
  }
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DataAccess.Models;

namespace ConsoleApp
{
    class Program
    {
        static readonly HttpClient Client = new HttpClient();

        static async Task<IEnumerable<T>> GetProductAsync<T>(string path)
        {
            IEnumerable<T> entities = null;
            HttpResponseMessage response = await Client.GetAsync(path);

            if (response.IsSuccessStatusCode)
                entities = await response.Content.ReadAsAsync<IEnumerable<T>>();

            return entities;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            Client.BaseAddress = new Uri("https://localhost:44313/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            
            Console.WriteLine("Products:");
            (await GetProductAsync<Product>("api/product"))
                .Select(x => x.ProductName)
                .ToList()
                .ForEach(Console.WriteLine);

            Console.WriteLine($"\n{new string('=', 80)}\nCategories:");
            (await GetProductAsync<Category>("api/category"))
                .Select(x => x.CategoryName)
                .ToList()
                .ForEach(Console.WriteLine);

            Console.ReadLine();
        }
    }
}
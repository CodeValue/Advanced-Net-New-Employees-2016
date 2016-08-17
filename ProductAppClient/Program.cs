using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProductAppClient
{
    class Program
    {
        const string API_BASE_URL = "http://localhost:47503/";

        static void Main()
        {
            //dont dorget to install the package Microsoft.AspNet.WebApi.Client
            //write "Install-Package Microsoft.AspNet.WebApi.Client" in the Package Manager Console

            Console.WriteLine("Press the <enter> key to start");
            Console.ReadLine();
            RunAsync().Wait();

            Console.ReadLine();

        }

        static async Task RunAsync()
        {
            await GetSingleProduct();
            Console.WriteLine();
            await PostNewProduct();
            Console.WriteLine();
            await GetAllProducts();
            Console.WriteLine();
            await PutProduct();
            Console.WriteLine();
            await GetAllProducts();
            Console.WriteLine();
            await DeleteProduct();
            Console.WriteLine();
            await GetAllProducts();
        }

        private static async Task GetSingleProduct()
        {
            int id = 1;
            Console.WriteLine($"Getting product with id {id}");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"api/products/{id}");
                if (response.IsSuccessStatusCode)
                {
                    Product product = await response.Content.ReadAsAsync<Product>();
                    Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                }
            }
        }
        private static async Task GetAllProducts()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/products/");
                if (response.IsSuccessStatusCode)
                {
                    Product[] products = await response.Content.ReadAsAsync<Product[]>();
                    Console.WriteLine("Products:");
                    foreach (var product in products)
                    {
                        Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                    }
                }
            }
        }
        private static async Task PostNewProduct()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var newProduct = new Product() { Category = "cat1", Name = "NewProduct1", Price = 100 };
                HttpResponseMessage response = await client.PostAsJsonAsync("api/products/", newProduct);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Product created:");
                    Console.WriteLine("{0}\t${1}\t{2}", newProduct.Name, newProduct.Price, newProduct.Category);
                }
            }
        }

        private static async Task PutProduct()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                int id = 1;
                var newProduct = new Product() { Category = "UpdateCategory", Name = "UpdatedName", Price = 200 };
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/products/{id}", newProduct);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Product with id {id} updated:");
                    Console.WriteLine("{0}\t${1}\t{2}", newProduct.Name, newProduct.Price, newProduct.Category);
                }
            }
        }

        private static async Task DeleteProduct()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                int id = 1;
                HttpResponseMessage response = await client.DeleteAsync($"api/products/{id}");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Product with id {id} deleted successfully");
                }
            }
        }
    }
}

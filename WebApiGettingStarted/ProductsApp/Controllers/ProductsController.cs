using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;
using ProductsApp.Models.DTOs;

namespace ProductsApp.Controllers
{
    public class ProductsController : ApiController
    {
        static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1, LastUpdate = DateTime.UtcNow.AddDays(-2)},
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M, LastUpdate = DateTime.UtcNow.AddDays(-2)},
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M,LastUpdate = DateTime.UtcNow.AddDays(-2)}
        };
        private static int _id = _products.Count;


        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }

        public IHttpActionResult GetProduct(int id)
        {
            var product = _products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        public IHttpActionResult DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _products.Remove(product);
            return Ok(product);
        }

        public IHttpActionResult PostProduct(NewProductDTO product)
        {
            if (product == null) return this.BadRequest("no product details were sent");

            _products.Add(new Product()
            {
                Id = GenerateNewId(),
                Category = product.Category,
                Name = product.Name,
                Price = product.Price,
                LastUpdate = DateTime.UtcNow
            });

            return Ok();
        }
        public IHttpActionResult PutProduct(int id, NewProductDTO updatedProduct)
        {
            if (updatedProduct == null) return this.BadRequest("no product details were sent");
            var product = _products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = updatedProduct.Name;
            product.Category = updatedProduct.Category;
            product.Price = updatedProduct.Price;
            product.LastUpdate = DateTime.UtcNow;

            return Ok();
        }
        private int GenerateNewId()
        {
            return Interlocked.Increment(ref _id);
        }
    }
}

using System;

namespace ProductsApp.Models.DTOs
{
    /// <summary>
    /// The purpose of a DTO is not to expose the internal structure of your domain-model.
    /// it allows you to make changes to your model without affecting clients and to provide a better
    /// format for serialization and transportation
    /// </summary>
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class NewProductDTO
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}
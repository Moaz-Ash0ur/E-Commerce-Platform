using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingDAL.Domains
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public double Rate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }


        // FK
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Navigation
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }







}




using System;
using TextContext;
using Microsoft.EntityFrameworkCore;
namespace Models
{
    public partial class Product
    {
        public int Id { get; set; }

        public string? name { get; set; }

        public int price { get; set; }

        public string? imager { get; set; }

        public string? remarks { get; set; }

        public int number { get; set; }

        public int manufacturer_id { get; set; }

        public int product_class_id { get; set; }



        public Manufacturer? Manufacturer { get; set; }
        public ProductClass? ProductClass { get; set; }
        public ICollection<Item> Item { get; set; }
    }
}


using System;
namespace Models
{
    public partial class ProductClass
    {
        public int Id { get; set; }

        public string? class_name { get; set; }

        public ICollection<Product> Product { get; set; }
    }
}


using System;
namespace Models
{
    public partial class Manufacturer
    {
        public int Id { get; set; }

        public string? name { get; set; }

        public string password { get; set; }

        public ICollection<Product> Product { get; set; }
        public ICollection<Item> Item { get; set; }
    }
}


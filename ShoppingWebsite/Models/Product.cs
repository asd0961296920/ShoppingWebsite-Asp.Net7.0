using System;
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

    }
}


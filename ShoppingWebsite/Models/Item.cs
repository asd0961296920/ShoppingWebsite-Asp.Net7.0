using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public partial class Item
    {
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        public string? order_id { get; set; }

        public int product_id { get; set; }

        public string? product_name { get; set; }

        public int price { get; set; }

        public int user_id { get; set; }

        public int manufacturer_id { get; set; }


    }
}


using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public partial class Order
    {
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        public string? order_number { get; set; }

        public string? adress { get; set; }

        public string? phone { get; set; }

        public int price { get; set; }

        public int user_id { get; set; }

        public int manufacturer_id { get; set; }

        public bool payment { get; set; }
    }
}


using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public partial class Shop
    {
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        public int user_id { get; set; }

        public int manufacturer_id { get; set; }

        public int product_id { get; set; }

        public int number { get; set; }


    }
}


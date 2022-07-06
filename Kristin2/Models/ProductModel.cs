using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kristin2.Models
{
    public class ProductModel
    {
        [Key]
        public int Product_number { get; set; }
        public string Product_name { get; set; }
        public int Price { get; set; }
        public int Duration { get; set; }
        public string Color { get; set; }
    }
}
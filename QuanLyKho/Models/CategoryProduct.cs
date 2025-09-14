using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace QuanLyKho.Models
{
    public class CategoryProduct
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }


        public virtual ICollection<Product> Products { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;


namespace QuanLyKho.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }


        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;


        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
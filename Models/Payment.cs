using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace QuanLyKho.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }


        public virtual ICollection<Order> Orders { get; set; }
    }
}
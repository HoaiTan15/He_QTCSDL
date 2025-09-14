using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace QuanLyKho.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }


        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }


        [Required]
        public string Username { get; set; }


        [Required]
        public string Password { get; set; } // Demo: lưu plaintext; production: hash


        public DateTime Date { get; set; } = DateTime.Now;


        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
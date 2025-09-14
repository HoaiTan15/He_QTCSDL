using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace QuanLyKho.Models
{
    public class Staff
    {
        [Key]
        public int StaffID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }


        [Required]
        public string Username { get; set; }


        [Required]
        public string Password { get; set; }


        public string Position { get; set; } // ví dụ: Admin, Staff


        public virtual ICollection<ImportReceipt> ImportReceipts { get; set; }
    }
}
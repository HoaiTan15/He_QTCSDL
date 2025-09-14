using System.ComponentModel.DataAnnotations;

namespace QuanLyKho.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }

        [Required, StringLength(20)]
        public string Role { get; set; } = "User"; // mặc định là User
    }
}

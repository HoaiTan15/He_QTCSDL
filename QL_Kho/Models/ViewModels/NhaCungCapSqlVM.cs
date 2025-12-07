using System;

namespace QL_Kho.Models.ViewModels
{
    public class NhaCungCapSqlVM
    {
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public int SoLuongSP { get; set; }
    }
}
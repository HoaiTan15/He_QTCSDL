using System;

namespace QL_Kho.ViewModels
{
    public class SanPhamSqlVM
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public decimal DonGia { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuongTon { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public string MaDM { get; set; }
        public string MaNCC { get; set; }
        public string TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }


        public string TenDM { get; set; }
        public string TenNCC { get; set; }
    }
}

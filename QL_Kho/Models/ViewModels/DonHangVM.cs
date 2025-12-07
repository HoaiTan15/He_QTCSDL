using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_Kho.Models.ViewModels
{
    public class DonHangVM
    {
        public string MaDH { get; set; }
        public string MaUser { get; set; }
        public DateTime? NgayDat { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        public string DiaChiGiao { get; set; }
        public string SDT { get; set; }
        public string GhiChu { get; set; }
        public string PhuongThucTT { get; set; }

        public string TenNguoiDung { get; set; }
        public string Email { get; set; }

        public int SoLuongSanPham { get; set; }
    }
}
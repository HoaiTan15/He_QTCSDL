using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_Kho.Models.ViewModels
{
    public class NhapHangVM
    {
        public string MaNCC { get; set; }
        public string MaQL { get; set; } // Người quản lý đang đăng nhập
        public List<NhapHangItem> ChiTietNhap { get; set; }
    }

    public class NhapHangItem
    {
        public string MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGiaNhap { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_Kho.Models.ViewModels
{
    public class DanhMucSqlVM
    {
        public string MaDM { get; set; }
        public string TenDM { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public string TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public int SoLuongSP { get; set; }
    }
}
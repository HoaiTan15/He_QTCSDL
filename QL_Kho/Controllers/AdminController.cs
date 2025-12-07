using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_Kho.Models;
using QL_Kho.Models.ViewModels;
using QL_Kho.ViewModels;

namespace QL_Kho.Controllers
{
    public class AdminController : Controller
    {
        private Model1 db = new Model1();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserID"] == null || Session["UserRole"]?.ToString() != "admin")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này! ";
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            ViewBag.TongDonHang = db.DONHANGs.Count();
            ViewBag.DonHangMoi = db.DONHANGs.Count(d => d.NgayDat >= DateTime.Today);
            ViewBag.TongSanPham = db.SANPHAMs.Count();
            ViewBag.TongNguoiDung = db.NGUOIDUNGs.Count();

            return View();
        }

        // ========================= DASHBOARD =========================

        public ActionResult Dashboard()
        {
            var stats = new DashboardViewModel
            {
                TongDonHang = db.DONHANGs.Count(),
                DonHangChoXacNhan = db.DONHANGs.Count(d => d.TrangThai == "Chờ xác nhận"),
                DonHangDangGiao = db.DONHANGs.Count(d => d.TrangThai == "Đang giao"),
                DonHangHoanThanh = db.DONHANGs.Count(d => d.TrangThai == "Đã giao"),

                TongSanPham = db.SANPHAMs.Count(),
                SanPhamHoatDong = db.SANPHAMs.Count(sp => sp.TrangThai == "HoatDong"),
                SanPhamHetHang = db.SANPHAMs.Count(sp => sp.TrangThai == "HetHang"),

                TongNguoiDung = db.NGUOIDUNGs.Count(),
                NguoiDungMoi = db.NGUOIDUNGs.Count(u => u.NgayTao >= DateTime.Now.AddDays(-30)),

                DoanhThuHomNay = db.DONHANGs
                    .Where(d => d.NgayDat >= DateTime.Today && d.TrangThai == "Đã giao")
                    .Sum(d => (decimal?)d.TongTien) ?? 0
            };

            ViewBag.DonHangMoi = db.DONHANGs
                .OrderByDescending(d => d.NgayDat)
                .Take(5)
                .ToList();

            return View(stats);
        }

        // ========================= SẢN PHẨM (SQL-FIRST) =========================

        public ActionResult QuanLySanPham()
        {
            // LẤY DỮ LIỆU TỪ STORED PROCEDURE
            var sanPham = db.Database.SqlQuery<SanPhamSqlVM>(
                "EXEC SP_GetSanPham"
            ).ToList();

            ViewBag.DanhMucList = db.DANHMUCs.Where(dm => dm.TrangThai == "HoatDong").ToList();
            ViewBag.NhaCungCapList = db.NHACUNGCAPs.Where(ncc => ncc.TrangThai == "HoatDong").ToList();

            return View(sanPham);
        }

        [HttpPost]
        public JsonResult ThemSanPham(SANPHAM sanPham, HttpPostedFileBase HinhAnh)
        {
            try
            {
                string fileName = null;

                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    fileName = Path.GetFileName(HinhAnh.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    HinhAnh.SaveAs(path);
                }

                db.Database.ExecuteSqlCommand(
                    "EXEC SP_ThemSanPham @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8",
                    sanPham.TenSP,
                    sanPham.DonGia,
                    sanPham.DonViTinh,
                    sanPham.SoLuongTon,
                    fileName,
                    sanPham.MoTa,
                    sanPham.MaNCC,
                    sanPham.MaDM,
                    "HoatDong"
                );

                return Json(new { success = true, message = "Thêm sản phẩm thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult ChiTietSanPham(string id)
        {
            var result = db.Database.SqlQuery<SanPhamSqlVM>(
                "SELECT sp.*, dm.TenDM, ncc.TenNCC FROM SANPHAM sp " +
                "LEFT JOIN DANHMUC dm ON sp.MaDM = dm.MaDM " +
                "LEFT JOIN NHACUNGCAP ncc ON sp.MaNCC = ncc.MaNCC " +
                "WHERE sp.MaSP = @p0",
                id.Trim()
            ).FirstOrDefault();

            if (result == null)
                return Content("<div class='alert alert-danger'>Không tìm thấy sản phẩm</div>");

            return PartialView("_ChiTietSanPham", result);
        }


        public ActionResult SuaSanPham(string id)
        {
            var result = db.Database.SqlQuery<SanPhamSqlVM>(
                "SELECT sp.*, dm.TenDM, ncc.TenNCC FROM SANPHAM sp " +
                "LEFT JOIN DANHMUC dm ON sp.MaDM = dm.MaDM " +
                "LEFT JOIN NHACUNGCAP ncc ON sp.MaNCC = ncc.MaNCC " +
                "WHERE sp.MaSP = @p0",
                id.Trim()
            ).FirstOrDefault();

            if (result == null)
                return Content("<div class='alert alert-danger'>Không tìm thấy sản phẩm</div>");

            ViewBag.DanhMucList = db.DANHMUCs.Where(dm => dm.TrangThai == "HoatDong").ToList();
            ViewBag.NhaCungCapList = db.NHACUNGCAPs.Where(ncc => ncc.TrangThai == "HoatDong").ToList();

            return PartialView("_SuaSanPham", result);
        }


        [HttpPost]
        public JsonResult CapNhatSanPham(SanPhamSqlVM sp, HttpPostedFileBase HinhAnh)
        {
            try
            {
                string fileName = sp.HinhAnh;

                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    fileName = Path.GetFileName(HinhAnh.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    HinhAnh.SaveAs(path);
                }

                db.Database.ExecuteSqlCommand(
                    "EXEC SP_SuaSanPham @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9",
                    sp.MaSP.Trim(),
                    sp.TenSP,
                    sp.DonGia,
                    sp.DonViTinh,
                    sp.SoLuongTon,
                    fileName,
                    sp.MoTa,
                    sp.MaNCC,
                    sp.MaDM,
                    sp.TrangThai
                );

                return Json(new { success = true, message = "Cập nhật sản phẩm thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult XoaSanPham(string maSP)
        {
            try
            {
                db.Database.ExecuteSqlCommand(
                    "EXEC SP_XoaSanPham @p0",
                    maSP.Trim()
                );

                return Json(new { success = true, message = "Xóa sản phẩm thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ========================= DANH MỤC (SQL-FIRST) =========================

        public ActionResult QuanLyDanhMuc()
        {
            // LẤY DỮ LIỆU TỪ STORED PROCEDURE
            var danhMuc = db.Database.SqlQuery<DanhMucSqlVM>(
                "EXEC SP_GetDanhMuc"
            ).ToList();

            return View(danhMuc);
        }

        [HttpPost]
        public JsonResult ThemDanhMuc(string TenDM, string MoTa, HttpPostedFileBase HinhAnh)
        {
            try
            {
                string fileName = null;

                // Upload hình ảnh
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    fileName = Path.GetFileName(HinhAnh.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);

                    if (!Directory.Exists(Server.MapPath("~/Content/images")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/images"));
                    }

                    HinhAnh.SaveAs(path);
                }

                // Gọi stored procedure
                db.Database.ExecuteSqlCommand(
                    "EXEC SP_ThemDanhMuc @p0, @p1, @p2, @p3",
                    TenDM,
                    MoTa,
                    fileName,
                    "HoatDong"
                );

                return Json(new { success = true, message = "Thêm danh mục thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        public ActionResult ChiTietDanhMuc(string id)
        {
            var result = db.Database.SqlQuery<DanhMucSqlVM>(
                "EXEC SP_ChiTietDanhMuc @p0",
                id.Trim()
            ).FirstOrDefault();

            if (result == null)
                return Content("<div class='alert alert-danger'>Không tìm thấy danh mục</div>");

            return PartialView("_ChiTietDanhMuc", result);
        }

        public ActionResult SuaDanhMuc(string id)
        {
            var result = db.Database.SqlQuery<DanhMucSqlVM>(
                "EXEC SP_ChiTietDanhMuc @p0",
                id.Trim()
            ).FirstOrDefault();

            if (result == null)
                return Content("<div class='alert alert-danger'>Không tìm thấy danh mục</div>");

            return PartialView("_SuaDanhMuc", result);
        }

        [HttpPost]
        public JsonResult CapNhatDanhMuc(DanhMucSqlVM dm, HttpPostedFileBase HinhAnh)
        {
            try
            {
                string fileName = dm.HinhAnh;

                // Upload hình ảnh mới
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    fileName = Path.GetFileName(HinhAnh.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    HinhAnh.SaveAs(path);
                }

                // Gọi stored procedure
                db.Database.ExecuteSqlCommand(
                    "EXEC SP_SuaDanhMuc @p0, @p1, @p2, @p3, @p4",
                    dm.MaDM.Trim(),
                    dm.TenDM,
                    dm.MoTa,
                    fileName,
                    dm.TrangThai
                );

                return Json(new { success = true, message = "Cập nhật danh mục thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult XoaDanhMuc(string maDM)
        {
            try
            {
                db.Database.ExecuteSqlCommand(
                    "EXEC SP_XoaDanhMuc @p0",
                    maDM.Trim()
                );

                return Json(new { success = true, message = "Xóa danh mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        // ========================= NHÀ CUNG CẤP (SQL-FIRST) =========================

        public ActionResult NhaCungCap()
        {
            // LẤY DỮ LIỆU TỪ STORED PROCEDURE
            var nhaCungCap = db.Database.SqlQuery<NhaCungCapSqlVM>(
                "EXEC SP_GetNhaCungCap"
            ).ToList();

            return View(nhaCungCap);
        }

        [HttpPost]
        public JsonResult ThemNhaCungCap(string TenNCC, string DiaChi, string SDT, string Email)
        {
            try
            {
                // Gọi stored procedure
                db.Database.ExecuteSqlCommand(
                    "EXEC SP_ThemNhaCungCap @p0, @p1, @p2, @p3, @p4",
                    TenNCC,
                    DiaChi,
                    SDT,
                    Email,
                    "HoatDong"
                );

                return Json(new { success = true, message = "Thêm nhà cung cấp thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        public ActionResult ChiTietNhaCungCap(string id)
        {
            var result = db.Database.SqlQuery<NhaCungCapSqlVM>(
                "EXEC SP_ChiTietNhaCungCap @p0",
                id.Trim()
            ).FirstOrDefault();

            if (result == null)
                return Content("<div class='alert alert-danger'>Không tìm thấy nhà cung cấp</div>");

            return PartialView("_ChiTietNhaCungCap", result);
        }

        public ActionResult SuaNhaCungCap(string id)
        {
            var result = db.Database.SqlQuery<NhaCungCapSqlVM>(
                "EXEC SP_ChiTietNhaCungCap @p0",
                id.Trim()
            ).FirstOrDefault();

            if (result == null)
                return Content("<div class='alert alert-danger'>Không tìm thấy nhà cung cấp</div>");

            return PartialView("_SuaNhaCungCap", result);
        }

        [HttpPost]
        public JsonResult CapNhatNhaCungCap(NhaCungCapSqlVM ncc)
        {
            try
            {
                // Gọi stored procedure
                db.Database.ExecuteSqlCommand(
                    "EXEC SP_SuaNhaCungCap @p0, @p1, @p2, @p3, @p4, @p5",
                    ncc.MaNCC.Trim(),
                    ncc.TenNCC,
                    ncc.DiaChi,
                    ncc.SDT,
                    ncc.Email,
                    ncc.TrangThai
                );

                return Json(new { success = true, message = "Cập nhật nhà cung cấp thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult XoaNhaCungCap(string maNCC)
        {
            try
            {
                db.Database.ExecuteSqlCommand(
                    "EXEC SP_XoaNhaCungCap @p0",
                    maNCC.Trim()
                );

                return Json(new { success = true, message = "Xóa nhà cung cấp thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public ActionResult NhapHang()
        {

            ViewBag.NCC = db.NHACUNGCAPs.Where(n => n.TrangThai == "HoatDong").ToList();
            ViewBag.SanPham = db.SANPHAMs.Where(s => s.TrangThai == "HoatDong").ToList();

            return View();
        }

        [HttpPost]
        public JsonResult NhapHang(string MaNCC, string ChiTietJson)
        {
            try
            {
                string maQL = Session["UserID"].ToString();

                var result = db.Database.SqlQuery<NhapHangResult>(
                    "EXEC sp_NhapHang @p0, @p1, @p2",
                    MaNCC,
                    maQL,
                    ChiTietJson
                ).FirstOrDefault();

                return Json(new { success = true, message = "Nhập hàng thành công!", maHDN = result.MaHDN });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public class NhapHangResult
        {
            public string MaHDN { get; set; }
            public string Message { get; set; }
        }
        // ========================= ĐƠN HÀNG (SQL-FIRST) =========================

        public ActionResult DonHang()
        {
            var ds = db.Database.SqlQuery<DonHangVM>(
                "SELECT * FROM vw_DonHangDayDu ORDER BY NgayDat DESC"
            ).ToList();

            return View(ds);
        }

        public ActionResult ChiTietDonHang(string id)
        {
            var detail = db.Database.SqlQuery<ChiTietDonHangViewModel>(
                @"SELECT ctd.MaCTDH,
                 ctd.MaSP,
                 sp.TenSP,
                 sp.HinhAnh,
                 ctd.SoLuong,
                 ctd.DonGia,
                 (ctd.SoLuong * ctd.DonGia) AS ThanhTien
          FROM CHITIETDONHANG ctd
          JOIN SANPHAM sp ON sp.MaSP = ctd.MaSP
          WHERE ctd.MaDH = @p0",
                id.Trim()
            ).ToList();

            return PartialView("_ChiTietDonHang", detail);
        }



        [HttpPost]
        public JsonResult CapNhatTrangThaiDonHang(string maDH, string trangThai)
        {
            try
            {
                db.Database.ExecuteSqlCommand(
                    "UPDATE DONHANG SET TrangThai = @p0 WHERE MaDH = @p1",
                    trangThai,
                    maDH.Trim()
                );

                return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult HuyDonHang(string maDH)
        {
            try
            {
                // Kiểm tra đơn hàng tồn tại
                var donHang = db.DONHANGs.FirstOrDefault(d => d.MaDH.Trim() == maDH.Trim());
                if (donHang == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
                }

                // Chỉ cho phép hủy đơn "Chờ xác nhận"
                if (donHang.TrangThai != "Chờ xác nhận")
                {
                    return Json(new { success = false, message = "Chỉ có thể hủy đơn hàng đang chờ xác nhận!" });
                }

                // Hoàn trả số lượng tồn kho
                var chiTiet = db.CHITIETDONHANGs.Where(ct => ct.MaDH.Trim() == maDH.Trim()).ToList();
                foreach (var item in chiTiet)
                {
                    var sanPham = db.SANPHAMs.Find(item.MaSP.Trim());
                    if (sanPham != null)
                    {
                        sanPham.SoLuongTon += item.SoLuong;
                        if (sanPham.TrangThai == "HetHang" && sanPham.SoLuongTon > 0)
                        {
                            sanPham.TrangThai = "HoatDong";
                        }
                    }
                }

                // Cập nhật trạng thái đơn hàng
                donHang.TrangThai = "Đã hủy";
                donHang.NgayCapNhat = DateTime.Now;
                db.SaveChanges();

                return Json(new { success = true, message = "Đã hủy đơn hàng thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

    }
}
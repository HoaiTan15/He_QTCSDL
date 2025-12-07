# HƯỚNG DẪN SỬA LỖI THÊM SẢN PHẨM

## VẤN ĐỀ
Database của bạn thiếu các stored procedures cần thiết cho chức năng quản lý sản phẩm:
- `SP_GetSanPham` - Lấy danh sách sản phẩm
- `SP_ThemSanPham` - Thêm sản phẩm mới
- `SP_SuaSanPham` - Sửa sản phẩm
- `SP_XoaSanPham` - Xóa sản phẩm

## CÁCH KHẮC PHỤC

### Bước 1: Chạy Stored Procedures
1. Mở SQL Server Management Studio (SSMS)
2. Kết nối đến database `DT_DB`
3. Mở file: `ThemStoredProcedures_SanPham.sql`
4. Nhấn F5 hoặc Execute để chạy script

### Bước 2: Kiểm tra kết quả
Chạy các lệnh sau trong SSMS để kiểm tra:

```sql
-- Kiểm tra stored procedures đã được tạo
SELECT name 
FROM sys.procedures 
WHERE name IN ('SP_GetSanPham', 'SP_ThemSanPham', 'SP_SuaSanPham', 'SP_XoaSanPham')

-- Test thêm sản phẩm
EXEC SP_ThemSanPham 
    @TenSP = N'Sản phẩm test',
    @DonGia = 100000,
    @DonViTinh = N'Cái',
    @SoLuongTon = 10,
    @HinhAnh = NULL,
    @MoTa = N'Mô tả test',
    @MaNCC = 'NCC001',  -- Thay bằng mã NCC có trong database
    @MaDM = 'DM001',    -- Thay bằng mã DM có trong database
    @TrangThai = 'HoatDong'

-- Xem kết quả
EXEC SP_GetSanPham
```

### Bước 3: Test trên Web Application
1. Build lại project (Ctrl + Shift + B)
2. Chạy ứng dụng (F5)
3. Đăng nhập với tài khoản admin
4. Vào trang Quản lý sản phẩm
5. Thử thêm sản phẩm mới

## THAY ĐỔI ĐÃ THỰC HIỆN

### 1. Tạo file SQL: `ThemStoredProcedures_SanPham.sql`
Chứa 4 stored procedures:
- **SP_GetSanPham**: Lấy danh sách sản phẩm kèm tên danh mục và nhà cung cấp
- **SP_ThemSanPham**: Thêm sản phẩm mới (trigger tự tạo MaSP)
- **SP_SuaSanPham**: Cập nhật thông tin sản phẩm
- **SP_XoaSanPham**: Xóa sản phẩm (soft delete nếu đã có trong hóa đơn)

### 2. Sửa `AdminController.cs`
- Sửa thứ tự tham số trong `CapNhatSanPham` để khớp với stored procedure SP_SuaSanPham
- Thay đổi từ: `@p5,@p6,@p7,@p8,@p9` = `MoTa, MaDM, MaNCC, TrangThai, fileName`
- Thành: `@p5,@p6,@p7,@p8,@p9` = `fileName, MoTa, MaNCC, MaDM, TrangThai`

## LƯU Ý
- Stored procedures sử dụng trigger `trg_AutoID_SANPHAM` để tự động tạo MaSP
- MaSP có định dạng: SP001, SP002, SP003...
- Khi xóa sản phẩm đã có trong hóa đơn, hệ thống sẽ chuyển trạng thái thành "Ngung" thay vì xóa vật lý
- Khi sửa sản phẩm mà không upload ảnh mới, ảnh cũ sẽ được giữ nguyên

## KIỂM TRA LỖI THƯỜNG GẶP

### Lỗi: "Could not find stored procedure 'SP_ThemSanPham'"
➜ Bạn chưa chạy file SQL. Quay lại Bước 1.

### Lỗi: "Danh mục không tồn tại" hoặc "Nhà cung cấp không tồn tại"
➜ Kiểm tra database đã có dữ liệu trong bảng DANHMUC và NHACUNGCAP chưa

### Lỗi: "Cannot insert duplicate key"
➜ Không nên xảy ra vì trigger tự tạo MaSP, nhưng nếu gặp thì kiểm tra trigger `trg_AutoID_SANPHAM`

## Liên hệ
Nếu vẫn gặp lỗi, cung cấp thông tin:
- Thông báo lỗi chi tiết
- Screenshot màn hình lỗi
- Kết quả khi chạy: `SELECT * FROM sys.procedures WHERE name LIKE '%SanPham%'`

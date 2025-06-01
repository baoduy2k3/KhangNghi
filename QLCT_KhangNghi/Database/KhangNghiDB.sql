-- ================================================
-- CSDL QUẢN LÝ HOẠT ĐỘNG KINH DOANH - KHANG NGHỊ (BẢN RÚT GỌN ĐỊA CHỈ)
-- ================================================
CREATE DATABASE KhangNghiDB1;
GO
USE KhangNghiDB1;
GO

-- 1. CHỨC VỤ
CREATE TABLE ChucVu (
    MaChucVu CHAR(5) PRIMARY KEY,
    TenChucVu NVARCHAR(100)
);

-- 2. LOẠI KHÁCH HÀNG
CREATE TABLE LoaiKhachHang (
    MaLoaiKH CHAR(5) PRIMARY KEY,
    TenLoaiKH NVARCHAR(100)
);

-- 3. PHÒNG BAN
CREATE TABLE PhongBan (
    MaPB CHAR(5) PRIMARY KEY,
    TenPB NVARCHAR(100)
);

-- 4. NHÂN VIÊN
CREATE TABLE NhanVien (
    MaNV CHAR(5) PRIMARY KEY,
    HoTen NVARCHAR(100),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    Email NVARCHAR(100),
    SoDienThoai NVARCHAR(20),
    DiaChi NVARCHAR(255),
    MaChucVu CHAR(5) FOREIGN KEY REFERENCES ChucVu(MaChucVu),
    MaPB CHAR(5) FOREIGN KEY REFERENCES PhongBan(MaPB)
);

-- 5. KHÁCH HÀNG
CREATE TABLE KhachHang (
    MaKH CHAR(5) PRIMARY KEY,
    TenKH NVARCHAR(100),
    Email NVARCHAR(100),
    SoDienThoai NVARCHAR(20),
    DiaChi NVARCHAR(255),
    MaLoaiKH CHAR(5) FOREIGN KEY REFERENCES LoaiKhachHang(MaLoaiKH)
);

-- 6. NHÀ CUNG CẤP
CREATE TABLE NhaCungCap (
    MaNCC CHAR(5) PRIMARY KEY,
    TenNCC NVARCHAR(100),
    Email NVARCHAR(100),
    SoDienThoai NVARCHAR(20),
    DiaChi NVARCHAR(255)
);

-- 7. LOẠI & SẢN PHẨM - DỊCH VỤ
CREATE TABLE LoaiSanPham (
    MaLoai CHAR(5) PRIMARY KEY,
    TenLoai NVARCHAR(100)
);

CREATE TABLE SanPham (
    MaSP CHAR(5) PRIMARY KEY,
    TenSP NVARCHAR(100),
    DonViTinh NVARCHAR(20),
    MoTa NVARCHAR(255),
    GiaBan DECIMAL(18,2),
    MaLoai CHAR(5) FOREIGN KEY REFERENCES LoaiSanPham(MaLoai)
);

CREATE TABLE DichVu (
    MaDV CHAR(5) PRIMARY KEY,
    TenDV NVARCHAR(100),
    GiaDichVu DECIMAL(18,2),
    MoTa NVARCHAR(255)
);

-- 8. HỢP ĐỒNG
CREATE TABLE HopDong (
    MaHD CHAR(5) PRIMARY KEY,
    TenHopDong NVARCHAR(100),
    NgayKy DATE,
    NgayHetHan DATE,
    NgayHoanThanh DATE,
    MaKH CHAR(5) FOREIGN KEY REFERENCES KhachHang(MaKH),
    LoaiHopDong NVARCHAR(50),
    TrangThai NVARCHAR(50),
    TongTien DECIMAL(18,2),
    CongNo DECIMAL(18,2)
);

CREATE TABLE ChiTietHopDong (
    MaChiTietHD CHAR(5) PRIMARY KEY,
    MaHD CHAR(5) NOT NULL,
    MaSP CHAR(5) NULL,
    MaDV CHAR(5) NULL,
    SoLuong INT,
    DonGia DECIMAL(18,2),
    FOREIGN KEY (MaHD) REFERENCES HopDong(MaHD),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP),
    FOREIGN KEY (MaDV) REFERENCES DichVu(MaDV)
);

-- 9. HOÁ ĐƠN BÁN
CREATE TABLE HoaDon (
    MaHDon CHAR(5) PRIMARY KEY,
    NgayLap DATE,
    MaKH CHAR(5) FOREIGN KEY REFERENCES KhachHang(MaKH),
    MaHD CHAR(5) FOREIGN KEY REFERENCES HopDong(MaHD),
    TrangThai NVARCHAR(50),
    GhiChu NVARCHAR(255)
);

CREATE TABLE ChiTietHoaDon (
    MaHDon CHAR(5),
    MaSP CHAR(5),
    SoLuong INT,
    DonGia DECIMAL(18,2),
    PRIMARY KEY (MaHDon, MaSP),
    FOREIGN KEY (MaHDon) REFERENCES HoaDon(MaHDon),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- 10. NHẬP - XUẤT HÀNG
CREATE TABLE NhapHang (
    MaPhieuNhap CHAR(5) PRIMARY KEY,
    NgayNhap DATE,
    MaNCC CHAR(5) FOREIGN KEY REFERENCES NhaCungCap(MaNCC),
    MaNV CHAR(5) FOREIGN KEY REFERENCES NhanVien(MaNV),
    TongTien DECIMAL(18,2)
);

CREATE TABLE ChiTietNhapHang (
    MaPhieuNhap CHAR(5),
    MaSP CHAR(5),
    SoLuong INT,
    DonGia DECIMAL(18,2),
    PRIMARY KEY (MaPhieuNhap, MaSP),
    FOREIGN KEY (MaPhieuNhap) REFERENCES NhapHang(MaPhieuNhap),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

CREATE TABLE XuatHang (
    MaPhieuXuat CHAR(5) PRIMARY KEY,
    NgayXuat DATE,
    MaKH CHAR(5) FOREIGN KEY REFERENCES KhachHang(MaKH),
    MaNV CHAR(5) FOREIGN KEY REFERENCES NhanVien(MaNV),
    TongTien DECIMAL(18,2)
);

CREATE TABLE ChiTietXuatHang (
    MaPhieuXuat CHAR(5),
    MaSP CHAR(5),
    SoLuong INT,
    DonGia DECIMAL(18,2),
    PRIMARY KEY (MaPhieuXuat, MaSP),
    FOREIGN KEY (MaPhieuXuat) REFERENCES XuatHang(MaPhieuXuat),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- 11. PHÂN CÔNG
CREATE TABLE PhanCong (
    MaPhanCong CHAR(5) PRIMARY KEY,
    MaHD CHAR(5) FOREIGN KEY REFERENCES HopDong(MaHD),
    MaNV CHAR(5) FOREIGN KEY REFERENCES NhanVien(MaNV),
    NgayThucHien DATE,
    GhiChu NVARCHAR(255)
);

-- 12. PHIẾU BẢO HÀNH
CREATE TABLE PhieuBaoHanh (
    MaPhieu CHAR(5) PRIMARY KEY,
    MaHDon CHAR(5) FOREIGN KEY REFERENCES HoaDon(MaHDon),
    NgayLap DATE,
    ThoiHan INT,
    GhiChu NVARCHAR(255)
);

-- 13. LỊCH LÀM VIỆC
CREATE TABLE LichLamViec (
    MaLich CHAR(5) PRIMARY KEY,
    MaNV CHAR(5) FOREIGN KEY REFERENCES NhanVien(MaNV),
    Ngay DATE,
    CaLam NVARCHAR(20),
    DiaDiem NVARCHAR(255),
    GhiChu NVARCHAR(255)
);

-- 14. TỒN KHO
CREATE TABLE TonKho (
    MaTonKho CHAR(5) PRIMARY KEY,
    MaSP CHAR(5) FOREIGN KEY REFERENCES SanPham(MaSP),
    Ngay DATE,
    SoLuongTonDauKy INT,
    SoLuongTonCuoiKy INT
);

-- 15. THANH TOÁN
CREATE TABLE ThanhToan (
    MaTT CHAR(5) PRIMARY KEY,
    NgayTT DATE,
    SoTien DECIMAL(18,2),
    HinhThuc NVARCHAR(50),
    MaHDon CHAR(5) FOREIGN KEY REFERENCES HoaDon(MaHDon),
    MaKH CHAR(5) FOREIGN KEY REFERENCES KhachHang(MaKH)
);

--Ràng buộc toàn vẹn
--NHÂN VIÊN
ALTER TABLE NhanVien
ADD CONSTRAINT CK_NhanVien_GioiTinh CHECK (GioiTinh IN (N'Nam', N'Nữ'));

ALTER TABLE NhanVien
ADD CONSTRAINT CK_NhanVien_Tuoi CHECK (DATEDIFF(YEAR, NgaySinh, GETDATE()) >= 18);

ALTER TABLE NhanVien
ADD CONSTRAINT CK_NhanVien_Email CHECK (Email LIKE '_%@_%._%');

ALTER TABLE NhanVien
ADD CONSTRAINT CK_NhanVien_SDT CHECK (
    SoDienThoai LIKE '0%' AND
    LEN(SoDienThoai) BETWEEN 10 AND 12 AND
    SoDienThoai NOT LIKE '%[^0-9]%'
);

--KHÁCH HÀNG
ALTER TABLE KhachHang
ADD CONSTRAINT CK_KhachHang_Email CHECK (Email LIKE '_%@_%._%');

ALTER TABLE KhachHang
ADD CONSTRAINT CK_KhachHang_SDT CHECK (
    SoDienThoai LIKE '0%' AND
    LEN(SoDienThoai) BETWEEN 10 AND 12 AND
    SoDienThoai NOT LIKE '%[^0-9]%'
);

--NHÀ CUNG CẤP
ALTER TABLE NhaCungCap
ADD CONSTRAINT CK_NCC_Email CHECK (Email LIKE '_%@_%._%');

ALTER TABLE NhaCungCap
ADD CONSTRAINT CK_NCC_SDT CHECK (
    SoDienThoai LIKE '0%' AND
    LEN(SoDienThoai) BETWEEN 10 AND 12 AND
    SoDienThoai NOT LIKE '%[^0-9]%'
);

--SẢN PHẨM
ALTER TABLE SanPham
ADD CONSTRAINT CK_SanPham_Gia CHECK (GiaBan > 0);

--DỊCH VỤ
ALTER TABLE DichVu
ADD CONSTRAINT CK_DichVu_Gia CHECK (GiaDichVu > 0);


-- Thêm nhân viên
CREATE PROCEDURE sp_ThemNhanVien
    @MaNV CHAR(5), @HoTen NVARCHAR(100), @NgaySinh DATE, @GioiTinh NVARCHAR(10),
    @Email NVARCHAR(100), @SoDienThoai NVARCHAR(20), @DiaChi NVARCHAR(255),
    @MaChucVu CHAR(5), @MaPB CHAR(5)
AS
BEGIN
    INSERT INTO NhanVien VALUES (@MaNV, @HoTen, @NgaySinh, @GioiTinh,
                                 @Email, @SoDienThoai, @DiaChi, @MaChucVu, @MaPB)
END
GO

-- Xoá nhân viên
CREATE PROCEDURE sp_XoaNhanVien @MaNV CHAR(5)
AS
BEGIN
    DELETE FROM NhanVien WHERE MaNV = @MaNV
END
GO

-- Sửa nhân viên
CREATE PROCEDURE sp_SuaNhanVien
    @MaNV CHAR(5), @HoTen NVARCHAR(100), @NgaySinh DATE, @GioiTinh NVARCHAR(10),
    @Email NVARCHAR(100), @SoDienThoai NVARCHAR(20), @DiaChi NVARCHAR(255),
    @MaChucVu CHAR(5), @MaPB CHAR(5)
AS
BEGIN
    UPDATE NhanVien SET
        HoTen = @HoTen,
        NgaySinh = @NgaySinh,
        GioiTinh = @GioiTinh,
        Email = @Email,
        SoDienThoai = @SoDienThoai,
        DiaChi = @DiaChi,
        MaChucVu = @MaChucVu,
        MaPB = @MaPB
    WHERE MaNV = @MaNV
END
GO

-- Xem chi tiết nhân viên
CREATE PROCEDURE sp_LayNhanVien
AS
BEGIN
    SELECT nv.MaNV, nv.HoTen, nv.NgaySinh, nv.GioiTinh, nv.Email,
           nv.SoDienThoai, nv.DiaChi, cv.TenChucVu, pb.TenPB
    FROM NhanVien nv
    JOIN ChucVu cv ON nv.MaChucVu = cv.MaChucVu
    JOIN PhongBan pb ON nv.MaPB = pb.MaPB
END
GO

-- Chức vụ
INSERT INTO ChucVu VALUES
('CV001', N'Giám đốc'),
('CV002', N'Phó Giám đốc'),
('CV003', N'Kế toán trưởng'),
('CV004', N'Trưởng phòng'),
('CV005', N'Nhân viên');

-- Phòng ban
INSERT INTO PhongBan VALUES
('PB001', N'Phòng Kinh doanh'),
('PB002', N'Phòng Kế toán'),
('PB003', N'Phòng Nhân sự'),
('PB004', N'Phòng Kỹ thuật'),
('PB005', N'Phòng Marketing');

-- Nhân viên
INSERT INTO NhanVien VALUES
('NV001', N'Nguyễn Văn A', '1990-01-01', N'Nam', 'a@gmail.com', '0900000001', N'12B Hàng Trống, Hoàn Kiếm, Hà Nội', 'CV001', 'PB001'),
('NV002', N'Lê Thị B', '1992-02-02', N'Nữ', 'b@gmail.com', '0900000002', N'25 Nguyễn Trãi, Thanh Xuân, Hà Nội', 'CV005', 'PB002'),
('NV003', N'Trần Văn C', '1991-03-03', N'Nam', 'c@gmail.com', '0900000003', N'78 Lê Duẩn, Đà Nẵng', 'CV004', 'PB003'),
('NV004', N'Phạm Thị D', '1993-04-04', N'Nữ', 'd@gmail.com', '0900000004', N'10 Pasteur, Quận 1, TP.HCM', 'CV003', 'PB004'),
('NV005', N'Hoàng Văn E', '1994-05-05', N'Nam', 'e@gmail.com', '0900000005', N'99 Trần Phú, TP Nha Trang', 'CV002', 'PB005');

-- Thêm loại khách hàng
INSERT INTO LoaiKhachHang VALUES
('LKH01', N'Cá nhân'),
('LKH02', N'Công ty'),
('LKH03', N'Tổ chức')


-- Thêm khách hàng
INSERT INTO KhachHang VALUES
('KH001', N'Nguyễn Thị Hương', 'huong@gmail.com', '0911000001', N'123 Cầu Giấy, Hà Nội', 'LKH01'),
('KH002', N'Công ty ABC', 'contact@abc.com.vn', '0911000002', N'456 Trần Hưng Đạo, TP.HCM', 'LKH02'),
('KH003', N'Trường Đại học XYZ', 'info@xyz.edu.vn', '0911000003', N'789 Lê Lợi, Đà Nẵng', 'LKH03'),
('KH004', N'Ngô Văn Bình', 'binhngo@gmail.com', '0911000004', N'88 Nguyễn Văn Cừ, Hải Phòng', 'LKH01'),
('KH005', N'Phan Thị Lệ', 'le.phan@gmail.com', '0911000005', N'100 Trường Chinh, Cần Thơ', 'LKH01');

-- Thêm khách hàng
CREATE PROCEDURE sp_ThemKhachHang
    @MaKH CHAR(5), @TenKH NVARCHAR(100), @Email NVARCHAR(100),
    @SoDienThoai NVARCHAR(20), @DiaChi NVARCHAR(255), @MaLoaiKH CHAR(5)
AS
BEGIN
    INSERT INTO KhachHang(MaKH, TenKH, Email, SoDienThoai, DiaChi, MaLoaiKH)
    VALUES (@MaKH, @TenKH, @Email, @SoDienThoai, @DiaChi, @MaLoaiKH)
END
GO

-- Sửa khách hàng
CREATE PROCEDURE sp_SuaKhachHang
    @MaKH CHAR(5), @TenKH NVARCHAR(100), @Email NVARCHAR(100),
    @SoDienThoai NVARCHAR(20), @DiaChi NVARCHAR(255), @MaLoaiKH CHAR(5)
AS
BEGIN
    UPDATE KhachHang SET
        TenKH = @TenKH,
        Email = @Email,
        SoDienThoai = @SoDienThoai,
        DiaChi = @DiaChi,
        MaLoaiKH = @MaLoaiKH
    WHERE MaKH = @MaKH
END
GO

-- Xóa khách hàng
CREATE PROCEDURE sp_XoaKhachHang
    @MaKH CHAR(5)
AS
BEGIN
    DELETE FROM KhachHang WHERE MaKH = @MaKH
END
GO

-- Xem danh sách khách hàng kèm loại khách hàng
CREATE PROCEDURE sp_LayKhachHang
AS
BEGIN
    SELECT kh.MaKH, kh.TenKH, kh.Email, kh.SoDienThoai, kh.DiaChi, lkh.TenLoaiKH
    FROM KhachHang kh
    JOIN LoaiKhachHang lkh ON kh.MaLoaiKH = lkh.MaLoaiKH
END
GO

-- Thêm loại sản phẩm
INSERT INTO LoaiSanPham (MaLoai, TenLoai) VALUES
('L001', N'Thiết bị đóng cắt'),
('L002', N'Thiết bị điều khiển'),
('L003', N'Thiết bị tự động hóa'),
('L004', N'Thiết bị đo lường hiển thị'),
('L005', N'Thiết bị truyền tải điện'),
('L006', N'Vật tư phụ kiện'),
('L007', N'Biến tần');

--Lấy danh sách loạiloại sản phẩm
CREATE PROCEDURE sp_LayDanhSachLoaiSanPham
AS
BEGIN
    SELECT MaLoai, TenLoai
    FROM LoaiSanPham;
END


-- Thêm sản phẩm
INSERT INTO SanPham (MaSP, TenSP, DonViTinh, MoTa, GiaBan, MaLoai) VALUES
-- L001: Thiết bị đóng cắt
('SP001', N'Aptomat 2P 20A', N'Cái', N'Aptomat 2 pha, chịu dòng 20A', 75000, 'L001'),
('SP002', N'Cầu dao đảo chiều 3P 63A', N'Cái', N'Chuyển đổi nguồn điện 3 pha', 185000, 'L001'),
('SP003', N'CB tép MCB 1P 10A', N'Cái', N'CB bảo vệ quá tải, ngắn mạch', 45000, 'L001'),
('SP004', N'Khởi động từ 220V 18A', N'Cái', N'Điều khiển động cơ điện', 125000, 'L001'),
('SP005', N'Cầu chì ống 500V 10A', N'Cái', N'Bảo vệ dòng điện', 15000, 'L001'),

-- L002: Thiết bị điều khiển
('SP006', N'Relay trung gian 24VDC', N'Cái', N'Relay điều khiển tín hiệu trung gian', 35000, 'L002'),
('SP007', N'Nút nhấn ON/OFF', N'Cái', N'Nút nhấn điều khiển', 15000, 'L002'),
('SP008', N'Công tắc hành trình', N'Cái', N'Giới hạn vị trí thiết bị', 28000, 'L002'),
('SP009', N'Bộ điều khiển nhiệt độ', N'Cái', N'Điều khiển nhiệt độ tự động', 215000, 'L002'),
('SP010', N'Công tắc xoay 3 vị trí', N'Cái', N'Tùy chọn chế độ máy', 42000, 'L002'),

-- L003: Thiết bị tự động hóa
('SP011', N'Cảm biến tiệm cận', N'Cái', N'Phát hiện vật thể gần', 65000, 'L003'),
('SP012', N'Cảm biến quang', N'Cái', N'Phát hiện vật bằng ánh sáng', 72000, 'L003'),
('SP013', N'PLC Mitsubishi FX3U', N'Cái', N'Điều khiển lập trình', 1850000, 'L003'),
('SP014', N'Module mở rộng I/O', N'Cái', N'Mở rộng cổng vào ra cho PLC', 490000, 'L003'),
('SP015', N'Màn hình HMI 7 inch', N'Cái', N'Giao diện người máy', 1250000, 'L003'),

-- L004: Thiết bị đo lường hiển thị
('SP016', N'Đồng hồ Volt 3 pha', N'Cái', N'Đo điện áp 3 pha', 130000, 'L004'),
('SP017', N'Đồng hồ Ampe kỹ thuật số', N'Cái', N'Đo dòng điện', 150000, 'L004'),
('SP018', N'Đồng hồ đa năng PZEM', N'Cái', N'Đo điện áp, dòng, công suất', 290000, 'L004'),
('SP019', N'Cảm biến dòng Hall', N'Cái', N'Đo dòng điện không tiếp xúc', 85000, 'L004'),
('SP020', N'Màn hình LED 4 số', N'Cái', N'Hiển thị tín hiệu điện', 95000, 'L004'),

-- L005: Thiết bị truyền tải điện
('SP021', N'Dây điện đơn CV 2.5mm', N'Mét', N'Dẫn điện trong hệ thống dân dụng', 9800, 'L005'),
('SP022', N'Ống ruột gà lõi thép', N'Mét', N'Bảo vệ dây điện', 18000, 'L005'),
('SP023', N'Đầu cốt đồng 10mm2', N'Cái', N'Kết nối dây dẫn', 2000, 'L005'),
('SP024', N'Thanh cái đồng 20x3mm', N'Mét', N'Phân phối điện tủ điện', 125000, 'L005'),
('SP025', N'Đầu nối nhanh 2 dây', N'Cái', N'Nối dây không cần hàn', 3500, 'L005'),

-- L006: Vật tư phụ kiện
('SP026', N'Cầu đấu dây 2 tầng', N'Cái', N'Đấu nối dây trong tủ', 4200, 'L006'),
('SP027', N'Nẹp nhựa định hình dây', N'Mét', N'Gọn dây trong tủ', 1800, 'L006'),
('SP028', N'Ốc vít M4', N'Cái', N'Lắp đặt tủ điện', 200, 'L006'),
('SP029', N'Bản lề tủ điện', N'Cái', N'Mở/đóng cánh tủ', 8500, 'L006'),
('SP030', N'Quạt tủ điện 220V', N'Cái', N'Tản nhiệt bên trong tủ', 115000, 'L006'),

-- L007: Biến tần
('SP031', N'Biến tần Delta 1.5kW', N'Cái', N'Điều chỉnh tốc độ động cơ', 2200000, 'L007'),
('SP032', N'Biến tần LS 2.2kW', N'Cái', N'Điều khiển động cơ 3 pha', 2650000, 'L007'),
('SP033', N'Biến tần INVT GD20 0.75kW', N'Cái', N'Tiết kiệm điện cho motor nhỏ', 1750000, 'L007'),
('SP034', N'Biến tần Schneider ATV12', N'Cái', N'Ứng dụng trong điều hòa, băng tải', 2950000, 'L007'),
('SP035', N'Biến tần ABB ACS150 1.1kW', N'Cái', N'Chạy động cơ ổn định, bền bỉ', 3050000, 'L007');

CREATE PROCEDURE sp_ThemSanPham
    @MaSP CHAR(5),
    @TenSP NVARCHAR(100),
    @DonViTinh NVARCHAR(20),
    @MoTa NVARCHAR(255),
    @GiaBan DECIMAL(18,2),
    @MaLoai CHAR(5)
AS
BEGIN
    INSERT INTO SanPham (MaSP, TenSP, DonViTinh, MoTa, GiaBan, MaLoai)
    VALUES (@MaSP, @TenSP, @DonViTinh, @MoTa, @GiaBan, @MaLoai)
END
GO

CREATE PROCEDURE sp_SuaSanPham
    @MaSP CHAR(5),
    @TenSP NVARCHAR(100),
    @DonViTinh NVARCHAR(20),
    @MoTa NVARCHAR(255),
    @GiaBan DECIMAL(18,2),
    @MaLoai CHAR(5)
AS
BEGIN
    UPDATE SanPham SET
        TenSP = @TenSP,
        DonViTinh = @DonViTinh,
        MoTa = @MoTa,
        GiaBan = @GiaBan,
        MaLoai = @MaLoai
    WHERE MaSP = @MaSP
END
GO

CREATE PROCEDURE sp_XoaSanPham
    @MaSP CHAR(5)
AS
BEGIN
    DELETE FROM SanPham WHERE MaSP = @MaSP
END
GO

CREATE PROCEDURE sp_LaySanPham
AS
BEGIN
    SELECT sp.MaSP, sp.TenSP, sp.DonViTinh, sp.MoTa, sp.GiaBan, lsp.TenLoai AS TenLoaiSanPham
    FROM SanPham sp
    JOIN LoaiSanPham lsp ON sp.MaLoai = lsp.MaLoai
END
GO

INSERT INTO DichVu (MaDV, TenDV, GiaDichVu, MoTa) VALUES
('DV001', N'Sửa điện cho tất cả dòng máy điện công nghiệp nhẹ và nặng', 1000000,
 N'Chúng tôi cung cấp dịch vụ sửa chữa cho tất cả các dòng máy điện công nghiệp, từ nhẹ đến nặng, bảo đảm hiệu suất hoạt động tối ưu.'),
('DV002', N'Thi công lắp đặt hệ thống nhà xưởng', 1500000,
 N'Lắp đặt các hệ thống điện và thiết bị cho các nhà xưởng, đảm bảo an toàn và tối ưu hiệu suất.'),
('DV003', N'Thiết kế đấu tủ bù công suất phản kháng bù cosphi', 1200000,
 N'Cung cấp giải pháp đấu tủ bù công suất giúp tiết kiệm điện năng và bảo vệ thiết bị điện trong hệ thống.'),
('DV004', N'Cân bằng 3 pha cho các cơ sở sản xuất', 1300000,
 N'Giải pháp cân bằng 3 pha giúp đảm bảo ổn định điện năng, tránh tình trạng quá tải và nâng cao hiệu suất.'),
('DV005', N'Làm tủ điện theo yêu cầu khách hàng', 1100000,
 N'Tùy chỉnh tủ điện theo yêu cầu của khách hàng, đáp ứng mọi nhu cầu và mục đích sử dụng.'),
('DV006', N'Nâng cấp lên chương trình máy PLC, biến tần, servo...', 2000000,
 N'Cung cấp giải pháp nâng cấp các hệ thống máy PLC, biến tần và servo để tối ưu hóa quy trình sản xuất.'),
('DV007', N'Sửa chữa di dời máy sản xuất', 1800000,
 N'Chúng tôi hỗ trợ di dời và sửa chữa các máy móc sản xuất, đảm bảo hoạt động ổn định khi tái lắp đặt.'),
('DV008', N'Sửa điện máy ép thổi chai nhựa', 900000,
 N'Cung cấp dịch vụ sửa chữa cho máy ép thổi chai nhựa, giúp máy móc hoạt động trở lại nhanh chóng.'),
('DV009', N'Sửa điện máy ó keo nhựa', 950000,
 N'Sửa chữa và bảo trì các máy ó keo nhựa, đảm bảo hoạt động liên tục và ổn định trong sản xuất.'),
('DV010', N'Sửa điện máy dập', 1000000,
 N'Cung cấp dịch vụ sửa chữa máy dập, giúp khôi phục hiệu suất máy móc và giảm thiểu thời gian dừng máy.'),
('DV011', N'Đấu tụ bù cosphi', 1100000,
 N'Cung cấp dịch vụ đấu tụ bù cosphi giúp tối ưu hóa hiệu suất hệ thống điện và tiết kiệm chi phí năng lượng.'),
('DV012', N'Đấu tủ điều khiển cho các dòng máy công nghiệp', 1200000,
 N'Cung cấp giải pháp đấu tủ điều khiển phù hợp cho các dòng máy công nghiệp khác nhau.'),
('DV013', N'Xử lý hệ thống thủy lực cho ngành công nghiệp nặng', 1700000,
 N'Giải pháp xử lý hệ thống thủy lực giúp nâng cao hiệu quả vận hành các thiết bị công nghiệp nặng.'),
('DV014', N'Di dời đấu nối điện cho máy', 1400000,
 N'Dịch vụ di dời và đấu nối điện cho các loại máy móc sản xuất, đảm bảo tính chính xác và an toàn.'),
('DV015', N'Xử lý sự cố quá tải chập điện nhà máy xưởng sản xuất', 1600000,
 N'Chúng tôi cung cấp các giải pháp xử lý sự cố quá tải, ngắt mạch điện trong các nhà máy sản xuất.'),
('DV016', N'Xử lý hệ thống điện lò hơi áp suất', 1500000,
 N'Giải pháp xử lý hệ thống điện cho lò hơi áp suất, giúp tối ưu hóa hiệu suất và độ bền của lò hơi.');

CREATE PROCEDURE sp_ThemDichVu
    @MaDV CHAR(5),
    @TenDV NVARCHAR(100),
    @GiaDichVu DECIMAL(18,2),
    @MoTa NVARCHAR(255)
AS
BEGIN
    INSERT INTO DichVu(MaDV, TenDV, GiaDichVu, MoTa)
    VALUES (@MaDV, @TenDV, @GiaDichVu, @MoTa)
END
GO

CREATE PROCEDURE sp_SuaDichVu
    @MaDV CHAR(5),
    @TenDV NVARCHAR(100),
    @GiaDichVu DECIMAL(18,2),
    @MoTa NVARCHAR(255)
AS
BEGIN
    UPDATE DichVu
    SET TenDV = @TenDV,
        GiaDichVu = @GiaDichVu,
        MoTa = @MoTa
    WHERE MaDV = @MaDV
END
GO

CREATE PROCEDURE sp_XoaDichVu
    @MaDV CHAR(5)
AS
BEGIN
    DELETE FROM DichVu WHERE MaDV = @MaDV
END
GO

CREATE PROCEDURE sp_LayDanhSachDichVu
AS
BEGIN
    SELECT MaDV, TenDV, GiaDichVu, MoTa
    FROM DichVu
    ORDER BY MaDV
END
GO

--Nhap/Xuat hang 
INSERT INTO SoNhaTenDuong VALUES ('DC02', N'KCN VSIP Bắc Ninh, Từ Sơn, Bắc Ninh', 'P01');
INSERT INTO SoNhaTenDuong VALUES ('DC03', N'Lô CN-03, KCN Đồng Văn II, Duy Tiên, Hà Nam', 'P01');
INSERT INTO SoNhaTenDuong VALUES ('DC04', N'Số 1, Đường số 8, KCN Việt Nam-Singapore, Thuận An, Bình Dương', 'P01');

INSERT INTO NhaCungCap (MaNCC, TenNCC, Email, SoDienThoai, MaSoNha) VALUES
('NCC01', N'Công ty TNHH Schneider Electric Việt Nam', N'contact.vn@schneider-electric.com', '1800585858', 'DC02'),
('NCC02', N'Công ty TNHH Siemens Việt Nam', N'info.vn@siemens.com', '02838251900', 'DC03'),
('NCC03', N'Công ty Cổ phần Tập đoàn LS Việt Nam', N'lsvina@lsvina.com', '02223765052', 'DC02'), -- Giả sử cùng địa chỉ hoặc một địa chỉ khác
('NCC04', N'Công ty Cổ phần CADIVI', N'cadivi@cadivi.vn', '02838299443', 'DC04');

INSERT INTO LoaiSanPham (MaLoai, TenLoai) VALUES
('LSP01', N'Thiết bị đóng cắt hạ thế'),
('LSP02', N'Thiết bị tự động hóa'),
('LSP03', N'Dây và cáp điện'),
('LSP04', N'Tủ điện công nghiệp');

INSERT INTO SanPham (MaSP, TenSP, DonViTinh, MoTa, GiaBan, MaLoai) VALUES
('SP001', N'Aptomat MCCB EZC100H 3P 50A', N'Cái', N'Schneider Electric EasyPact EZC100H, 3 cực, 50A, 25kA', 950000.00, 'LSP01'),
('SP002', N'Contactor LC1D09M7 220VAC 9A', N'Cái', N'Schneider Electric TeSys D, 3 cực, 9A, cuộn coil 220VAC', 280000.00, 'LSP01'),
('SP003', N'PLC Siemens S7-1200 CPU 1214C DC/DC/DC', N'Bộ', N'Siemens SIMATIC S7-1200, CPU 1214C, 14DI/10DQ/2AI', 7500000.00, 'LSP02'),
('SP004', N'Cáp điện CADIVI CVV 4x2.5mm2', N'Mét', N'Cáp đồng, cách điện PVC, vỏ PVC, 4 lõi, tiết diện 2.5mm2', 45000.00, 'LSP03'),
('SP005', N'Biến tần LS SV004iG5A-2 0.4kW 220V', N'Cái', N'LS Industrial Systems, dòng iG5A, 0.4kW, 1 pha 220V', 2100000.00, 'LSP02'),
('SP006', N'Aptomat MCB Acti9 iK60N 1P 16A', N'Cái', N'Schneider Electric Acti9 iK60N, 1 cực, 16A, 6kA', 85000.00, 'LSP01'),
('SP007', N'Relay nhiệt LRD12 5.5-8A', N'Cái', N'Schneider Electric TeSys LRD, dải chỉnh 5.5-8A', 320000.00, 'LSP01');

DECLARE @TongTienPN1 DECIMAL(18,2);
SET @TongTienPN1 = (10 * 7000000) + (20 * 20000000) + (50 * 6000000); 

INSERT INTO NhapHang (MaPhieuNhap, NgayNhap, MaNCC, MaNV, TongTien) VALUES
('PN001', '2023-10-01', 'NCC01', 'NV01', @TongTienPN1);

INSERT INTO ChiTietNhapHang (MaPhieuNhap, MaSP, SoLuong, DonGia) VALUES
('PN001', 'SP001', 10, 70000000), 
('PN001', 'SP002', 20, 20000000), 
('PN001', 'SP006', 50, 6000000); 

DECLARE @TongTienPN2 DECIMAL(18,2);
SET @TongTienPN2 = (5 * 6000000.00) + (200 * 35000.00); -- Tính tổng tiền cho PN002

INSERT INTO NhapHang (MaPhieuNhap, NgayNhap, MaNCC, MaNV, TongTien) VALUES
('PN002', '2023-10-15', 'NCC02', 'NV02', @TongTienPN2);

INSERT INTO ChiTietNhapHang (MaPhieuNhap, MaSP, SoLuong, DonGia) VALUES
('PN002', 'SP003', 5, 6000000.00);

DECLARE @TongTienPN3 DECIMAL(18,2);
SET @TongTienPN3 = (200 * 35000.00); -- Cáp điện CADIVI CVV 4x2.5mm2

INSERT INTO NhapHang (MaPhieuNhap, NgayNhap, MaNCC, MaNV, TongTien) VALUES
('PN003', '2023-10-18', 'NCC04', 'NV02', @TongTienPN3);

INSERT INTO ChiTietNhapHang (MaPhieuNhap, MaSP, SoLuong, DonGia) VALUES
('PN003', 'SP004', 200, 35000.00);

DECLARE @TongTienPN4 DECIMAL(18,2);
SET @TongTienPN4 = (8 * 1500000.00) + (15 * 250000.00); -- Tính tổng tiền cho PN004

INSERT INTO NhapHang (MaPhieuNhap, NgayNhap, MaNCC, MaNV, TongTien) VALUES
('PN004', '2023-11-05', 'NCC03', 'NV01', @TongTienPN4); -- Phiếu này nhập hàng từ LS (NCC03)

INSERT INTO ChiTietNhapHang (MaPhieuNhap, MaSP, SoLuong, DonGia) VALUES
('PN004', 'SP005', 8, 1500000.00);

DECLARE @TongTienPN5 DECIMAL(18,2);
SET @TongTienPN5 = (25 * 250000.00);

INSERT INTO NhapHang (MaPhieuNhap, NgayNhap, MaNCC, MaNV, TongTien) VALUES
('PN005', '2023-11-06', 'NCC01', 'NV01', @TongTienPN5);

INSERT INTO ChiTietNhapHang (MaPhieuNhap, MaSP, SoLuong, DonGia) VALUES
('PN005', 'SP007', 25, 250000.00);

GO
CREATE PROCEDURE sp_LayDanhSachPhieuNhap
AS
BEGIN
    SELECT
        nh.MaPhieuNhap,
        nh.NgayNhap,
        nh.MaNCC,
        ncc.TenNCC, -- Lấy tên Nhà cung cấp
        nh.MaNV,
        nv.HoTen AS TenNhanVienLap, -- Lấy tên Nhân viên lập phiếu
        nh.TongTien
    FROM NhapHang nh
    INNER JOIN NhaCungCap ncc ON nh.MaNCC = ncc.MaNCC
    INNER JOIN NhanVien nv ON nh.MaNV = nv.MaNV
    ORDER BY nh.NgayNhap DESC, nh.MaPhieuNhap DESC; -- Sắp xếp theo ngày nhập mới nhất
END
GO

CREATE PROCEDURE sp_LayChiTietPhieuNhap
    @MaPhieuNhap CHAR(5)
AS
BEGIN
    SELECT
        ctnh.MaPhieuNhap,
        ctnh.MaSP,
        sp.TenSP, -- Lấy tên sản phẩm
        sp.DonViTinh,
        ctnh.SoLuong,
        ctnh.DonGia,
        (ctnh.SoLuong * ctnh.DonGia) AS ThanhTien
    FROM ChiTietNhapHang ctnh
    INNER JOIN SanPham sp ON ctnh.MaSP = sp.MaSP
    WHERE ctnh.MaPhieuNhap = @MaPhieuNhap;
END
GO

USE KhangNghiDB;
GO
EXEC sp_LayDanhSachPhieuNhap;

GO
CREATE PROCEDURE sp_ThemPhieuNhap
    @MaPhieuNhap CHAR(5),
    @NgayNhap DATE,
    @MaNCC CHAR(5),
    @MaNV CHAR(5),
    @TongTien DECIMAL(18,2)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM NhapHang WHERE MaPhieuNhap = @MaPhieuNhap)
    BEGIN
        RETURN
    END
    INSERT INTO NhapHang (MaPhieuNhap, NgayNhap, MaNCC, MaNV, TongTien)
    VALUES (@MaPhieuNhap, @NgayNhap, @MaNCC, @MaNV, @TongTien);
END
GO

CREATE PROCEDURE sp_XoaTatCaChiTietPhieuNhap
    @MaPhieuNhap CHAR(5)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM NhapHang WHERE MaPhieuNhap = @MaPhieuNhap)
    BEGIN
        RETURN 
    END

    DELETE FROM ChiTietNhapHang
    WHERE MaPhieuNhap = @MaPhieuNhap;
END
GO

-- Stored Procedure để xóa Phiếu Nhập chính
CREATE PROCEDURE sp_XoaPhieuNhap
    @MaPhieuNhap CHAR(5)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM NhapHang WHERE MaPhieuNhap = @MaPhieuNhap)
    BEGIN
        RETURN
    END
    DELETE FROM NhapHang
    WHERE MaPhieuNhap = @MaPhieuNhap;
END
GO

CREATE PROCEDURE sp_SuaPhieuNhap
    @MaPhieuNhap CHAR(5),
    @NgayNhap DATE,
    @MaNCC CHAR(5),
    @MaNV CHAR(5),
    @TongTien DECIMAL(18,2)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM NhapHang WHERE MaPhieuNhap = @MaPhieuNhap)
    BEGIN
        RETURN
    END

    UPDATE NhapHang
    SET NgayNhap = @NgayNhap,
        MaNCC = @MaNCC,
        MaNV = @MaNV,
        TongTien = @TongTien
    WHERE MaPhieuNhap = @MaPhieuNhap;
END
GO

USE KhangNghiDB; 
GO

IF OBJECT_ID('sp_ThemChiTietPhieuNhap', 'P') IS NOT NULL
    DROP PROCEDURE sp_ThemChiTietPhieuNhap;
GO

CREATE PROCEDURE sp_ThemChiTietPhieuNhap
    @MaPhieuNhap CHAR(5),
    @MaSP CHAR(5),
    @SoLuong INT,
    @DonGia DECIMAL(18,2)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM NhapHang WHERE MaPhieuNhap = @MaPhieuNhap)
    BEGIN
        RETURN -1; 
    END
    IF @SoLuong <= 0
    BEGIN
        RETURN -3;
    END
    IF @DonGia < 0
    BEGIN
        RETURN -4;
    END
    IF EXISTS (SELECT 1 FROM ChiTietNhapHang WHERE MaPhieuNhap = @MaPhieuNhap AND MaSP = @MaSP)
    BEGIN
        RETURN -5;
    END
    BEGIN TRY
        INSERT INTO ChiTietNhapHang (MaPhieuNhap, MaSP, SoLuong, DonGia)
        VALUES (@MaPhieuNhap, @MaSP, @SoLuong, @DonGia);

        DECLARE @TongTienMoi DECIMAL(18,2);
        SELECT @TongTienMoi = SUM(SoLuong * DonGia)
        FROM ChiTietNhapHang
        WHERE MaPhieuNhap = @MaPhieuNhap;

        UPDATE NhapHang
        SET TongTien = @TongTienMoi
        WHERE MaPhieuNhap = @MaPhieuNhap;
        
        
        RETURN 0;
    END TRY
    BEGIN CATCH
    END CATCH
END
GO

GO

CREATE PROCEDURE sp_XoaMotChiTietPhieuNhap
    @MaPhieuNhap CHAR(5),
    @MaSP CHAR(5)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM ChiTietNhapHang WHERE MaPhieuNhap = @MaPhieuNhap AND MaSP = @MaSP)
    BEGIN
        RAISERROR (N'Chi tiết sản phẩm [%s] không tồn tại trong Phiếu Nhập [%s] để xóa.', 16, 1, @MaSP, @MaPhieuNhap);
        RETURN -1; 
    END

    BEGIN TRY
        DELETE FROM ChiTietNhapHang
        WHERE MaPhieuNhap = @MaPhieuNhap AND MaSP = @MaSP;

        DECLARE @TongTienMoi DECIMAL(18,2);
        SELECT @TongTienMoi = ISNULL(SUM(ISNULL(SoLuong, 0) * ISNULL(DonGia, 0)), 0) 
        FROM ChiTietNhapHang
        WHERE MaPhieuNhap = @MaPhieuNhap;

        UPDATE NhapHang
        SET TongTien = @TongTienMoi
        WHERE MaPhieuNhap = @MaPhieuNhap;
        
        RETURN 0;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN -99; 
    END CATCH
END
GO

CREATE PROCEDURE sp_SuaMotChiTietPhieuNhap
    @MaPhieuNhap CHAR(5),
    @MaSP CHAR(5),
    @SoLuongMoi INT,
    @DonGiaMoi DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM ChiTietNhapHang WHERE MaPhieuNhap = @MaPhieuNhap AND MaSP = @MaSP)
    BEGIN
        RAISERROR (N'Chi tiết sản phẩm [%s] không tồn tại trong Phiếu Nhập [%s] để sửa.', 16, 1, @MaSP, @MaPhieuNhap);
        RETURN -1;
    END

    IF @SoLuongMoi <= 0
    BEGIN
        RAISERROR (N'Số lượng sản phẩm mới phải lớn hơn 0.', 16, 1);
        RETURN -2; 
    END

    IF @DonGiaMoi < 0
    BEGIN
        RAISERROR (N'Đơn giá sản phẩm mới không được âm.', 16, 1);
        RETURN -3;
    END

    BEGIN TRY
        UPDATE ChiTietNhapHang
        SET SoLuong = @SoLuongMoi,
            DonGia = @DonGiaMoi
        WHERE MaPhieuNhap = @MaPhieuNhap AND MaSP = @MaSP;

        DECLARE @TongTienMoiCuaPhieu DECIMAL(18,2);
        SELECT @TongTienMoiCuaPhieu = ISNULL(SUM(ISNULL(SoLuong, 0) * ISNULL(DonGia, 0)), 0)
        FROM ChiTietNhapHang
        WHERE MaPhieuNhap = @MaPhieuNhap;

        UPDATE NhapHang
        SET TongTien = @TongTienMoiCuaPhieu
        WHERE MaPhieuNhap = @MaPhieuNhap;
        
        RETURN 0;
    END TRY
    BEGIN CATCH
    END CATCH
END
GO



CREATE PROCEDURE sp_TimKiemPhieuNhap
    @TuKhoaChung NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @TuKhoaChung = ''
        SET @TuKhoaChung = NULL;

    SELECT
        pn.MaPhieuNhap,
        pn.NgayNhap,
        pn.MaNCC,
        ncc.TenNCC, 
        pn.MaNV,
        nv.HoTen AS TenNhanVien, 
        pn.TongTien
    FROM NhapHang pn
    INNER JOIN NhaCungCap ncc ON pn.MaNCC = ncc.MaNCC
    INNER JOIN NhanVien nv ON pn.MaNV = nv.MaNV
    WHERE
        (@TuKhoaChung IS NULL) OR
        (
            pn.MaPhieuNhap LIKE '%' + @TuKhoaChung + '%' OR
            pn.MaNCC LIKE '%' + @TuKhoaChung + '%' OR
            pn.MaNV LIKE '%' + @TuKhoaChung + '%'
        )
    ORDER BY pn.NgayNhap DESC, pn.MaPhieuNhap DESC;
END
GO

IF OBJECT_ID('sp_LayDanhSachPhieuXuat', 'P') IS NOT NULL
    DROP PROCEDURE sp_LayDanhSachPhieuXuat;
GO

CREATE PROCEDURE sp_LayDanhSachPhieuXuat
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        xh.MaPhieuXuat,
        xh.NgayXuat,
        xh.MaKH,
        xh.MaNV,
        xh.TongTien
    FROM XuatHang xh
    INNER JOIN KhachHang kh ON xh.MaKH = kh.MaKH
    INNER JOIN NhanVien nv ON xh.MaNV = nv.MaNV
    ORDER BY xh.NgayXuat DESC, xh.MaPhieuXuat DESC;
END
GO

use KhangNghiDB
INSERT INTO LoaiKhachHang (MaLoaiKH, TenLoaiKH) VALUES
('LKH01', N'Khách hàng doanh nghiệp'),
('LKH02', N'Khách hàng cá nhân'),
('LKH03', N'Đại lý');
GO
INSERT INTO KhachHang (MaKH, TenKH, Email, SoDienThoai, MaSoNha, MaLoaiKH) VALUES
('KH001', N'Công ty TNHH Điện ABC', N'contact@dienabc.com', '0912345678', 'DC01', 'LKH01'),
('KH002', N'Anh Trần Văn Nam', N'namtv@email.com', '0987654321', 'DC02', 'LKH02'),
('KH003', N'Đại lý Điện Lực XYZ', N'daily@dienlucxyz.vn', '0243999888', 'DC03', 'LKH03');
GO

DECLARE @TongTienPX1 DECIMAL(18,2);
-- Giả sử SP001 (Aptomat MCCB) giá bán 950000, SP002 (Contactor) giá bán 280000
SET @TongTienPX1 = (2 * 950000.00) + (5 * 280000.00);

INSERT INTO XuatHang (MaPhieuXuat, NgayXuat, MaKH, MaNV, TongTien) VALUES
('PX001', '2023-11-10', 'KH001', 'NV01', @TongTienPX1);

INSERT INTO ChiTietXuatHang (MaPhieuXuat, MaSP, SoLuong, DonGia) VALUES
('PX001', 'SP001', 2, 950000.00), -- Giá bán từ bảng SanPham
('PX001', 'SP002', 5, 280000.00); -- Giá bán từ bảng SanPham

-- Phiếu Xuất 2: Cho Anh Trần Văn Nam, nhân viên NV03 xuất
DECLARE @TongTienPX2 DECIMAL(18,2);
-- Giả sử SP006 (Aptomat MCB) giá bán 85000, SP004 (Cáp điện) giá bán 45000
SET @TongTienPX2 = (10 * 85000.00) + (50 * 45000.00);

INSERT INTO XuatHang (MaPhieuXuat, NgayXuat, MaKH, MaNV, TongTien) VALUES
('PX002', '2023-11-15', 'KH002', 'NV03', @TongTienPX2);

INSERT INTO ChiTietXuatHang (MaPhieuXuat, MaSP, SoLuong, DonGia) VALUES
('PX002', 'SP006', 10, 85000.00),
('PX002', 'SP004', 50, 45000.00);

-- Phiếu Xuất 3: Cho Đại lý Điện Lực XYZ, nhân viên NV02 xuất
DECLARE @TongTienPX3 DECIMAL(18,2);
-- Giả sử SP003 (PLC Siemens) giá bán 7500000, SP005 (Biến tần LS) giá bán 2100000
SET @TongTienPX3 = (1 * 7500000.00) + (2 * 2100000.00);

INSERT INTO XuatHang (MaPhieuXuat, NgayXuat, MaKH, MaNV, TongTien) VALUES
('PX003', '2023-11-20', 'KH003', 'NV02', @TongTienPX3);

INSERT INTO ChiTietXuatHang (MaPhieuXuat, MaSP, SoLuong, DonGia) VALUES
('PX003', 'SP003', 1, 7500000.00),
('PX003', 'SP005', 2, 2100000.00);

-- Phiếu Xuất 4: Cho Công ty Điện ABC, nhân viên NV01 xuất (thêm một phiếu khác)
DECLARE @TongTienPX4 DECIMAL(18,2);
-- Giả sử SP007 (Relay nhiệt) giá bán 320000
SET @TongTienPX4 = (3 * 320000.00);

INSERT INTO XuatHang (MaPhieuXuat, NgayXuat, MaKH, MaNV, TongTien) VALUES
('PX004', '2023-11-22', 'KH001', 'NV01', @TongTienPX4);

INSERT INTO ChiTietXuatHang (MaPhieuXuat, MaSP, SoLuong, DonGia) VALUES
('PX004', 'SP007', 3, 320000.00);
IF OBJECT_ID('sp_ThemPhieuXuat', 'P') IS NOT NULL
    DROP PROCEDURE sp_ThemPhieuXuat;
GO

CREATE PROCEDURE sp_ThemPhieuXuat
    @MaPhieuXuat CHAR(5),
    @NgayXuat DATE,
    @MaKH CHAR(5),
    @MaNV CHAR(5),
    @TongTien DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM XuatHang WHERE MaPhieuXuat = @MaPhieuXuat)
    BEGIN
        RAISERROR (N'Mã Phiếu Xuất [%s] đã tồn tại.', 16, 1, @MaPhieuXuat);
        RETURN -1; 
    END
    IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKH = @MaKH)
    BEGIN
        RAISERROR (N'Mã Khách Hàng [%s] không tồn tại.', 16, 1, @MaKH);
        RETURN -2;
    END
    IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = @MaNV)
    BEGIN
        RAISERROR (N'Mã Nhân Viên [%s] không tồn tại.', 16, 1, @MaNV);
        RETURN -3;
    END
    INSERT INTO XuatHang (MaPhieuXuat, NgayXuat, MaKH, MaNV, TongTien)
    VALUES (@MaPhieuXuat, @NgayXuat, @MaKH, @MaNV, @TongTien);
    IF @@ROWCOUNT > 0
        RETURN 0; 
    ELSE
        RETURN -4; 
END
GO

IF OBJECT_ID('sp_XoaTatCaChiTietPhieuXuat', 'P') IS NOT NULL
    DROP PROCEDURE sp_XoaTatCaChiTietPhieuXuat;
GO
--Xóa chi tiết của phiếu xuất
CREATE PROCEDURE sp_XoaTatCaChiTietPhieuXuat
    @MaPhieuXuat CHAR(5)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ChiTietXuatHang
    WHERE MaPhieuXuat = @MaPhieuXuat;
END
GO

-- Stored Procedure để xóa Phiếu Xuất chính
IF OBJECT_ID('sp_XoaPhieuXuat', 'P') IS NOT NULL
    DROP PROCEDURE sp_XoaPhieuXuat;
GO
CREATE PROCEDURE sp_XoaPhieuXuat
    @MaPhieuXuat CHAR(5)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM XuatHang WHERE MaPhieuXuat = @MaPhieuXuat)
    BEGIN
        RAISERROR (N'Mã Phiếu Xuất [%s] không tồn tại để xóa.', 16, 1, @MaPhieuXuat);
        RETURN -1;
    END
    DELETE FROM XuatHang
    WHERE MaPhieuXuat = @MaPhieuXuat;

    IF @@ROWCOUNT > 0
        RETURN 0; 
    ELSE
        RETURN -2;
END
GO

--Sửa phiếu xuất hàng
CREATE PROCEDURE sp_SuaPhieuXuat
    @MaPhieuXuat CHAR(5),
    @NgayXuat DATE,
    @MaKH CHAR(5),
    @MaNV CHAR(5),
    @TongTien DECIMAL(18,2) 
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM XuatHang WHERE MaPhieuXuat = @MaPhieuXuat)
    BEGIN
        RAISERROR (N'Mã Phiếu Xuất [%s] không tồn tại để sửa.', 16, 1, @MaPhieuXuat);
        RETURN -1;
    END
    IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKH = @MaKH)
    BEGIN
        RAISERROR (N'Mã Khách Hàng [%s] không tồn tại.', 16, 1, @MaKH);
        RETURN -2;
    END
    IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = @MaNV)
    BEGIN
        RAISERROR (N'Mã Nhân Viên [%s] không tồn tại.', 16, 1, @MaNV);
        RETURN -3;
    END

    UPDATE XuatHang
    SET NgayXuat = @NgayXuat,
        MaKH = @MaKH,
        MaNV = @MaNV,
        TongTien = @TongTien 
    WHERE MaPhieuXuat = @MaPhieuXuat;

    IF @@ROWCOUNT > 0
        RETURN 0;
    ELSE
        RETURN -4; 
END
GO

--Chi tiết phiếu xuất
IF OBJECT_ID('sp_LayChiTietPhieuXuat', 'P') IS NOT NULL
    DROP PROCEDURE sp_LayChiTietPhieuXuat;
GO
GO

CREATE PROCEDURE sp_LayChiTietPhieuXuat
    @MaPhieuXuat CHAR(5)
AS
BEGIN
    SELECT
        ctxh.MaPhieuXuat,
        ctxh.MaSP,
        sp.DonViTinh,
        ctxh.SoLuong,
        ctxh.DonGia,
		(ctxh.SoLuong * ctxh.DonGia) AS ThanhTien
    FROM ChiTietXuatHang ctxh
    INNER JOIN SanPham sp ON ctxh.MaSP = sp.MaSP
    WHERE ctxh.MaPhieuXuat = @MaPhieuXuat;
END
GO

--Thêm chi tiết phiếu xuất
GO

IF OBJECT_ID('sp_ThemChiTietPhieuXuat', 'P') IS NOT NULL 
    DROP PROCEDURE sp_ThemChiTietPhieuXuat
GO

CREATE PROCEDURE sp_ThemChiTietPhieuXuat
    @MaPhieuXuat CHAR(5),
    @MaSP CHAR(5),
    @SoLuong INT,
    @DonGia DECIMAL(18,2)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM XuatHang WHERE MaPhieuXuat = @MaPhieuXuat)
    BEGIN
        RETURN -1; 
    END
    IF @SoLuong <= 0
    BEGIN
        RETURN -3;
    END
    IF @DonGia < 0
    BEGIN
        RETURN -4;
    END
    IF EXISTS (SELECT 1 FROM ChiTietXuatHang WHERE MaPhieuXuat = @MaPhieuXuat AND MaSP = @MaSP)
    BEGIN
        RETURN -5;
    END
    BEGIN TRY
        INSERT INTO ChiTietXuatHang(MaPhieuXuat, MaSP, SoLuong, DonGia)
        VALUES (@MaPhieuXuat, @MaSP, @SoLuong, @DonGia);

        DECLARE @TongTienMoi DECIMAL(18,2);
        SELECT @TongTienMoi = SUM(SoLuong * DonGia)
        FROM ChiTietXuatHang
        WHERE MaPhieuXuat = @MaPhieuXuat;

        UPDATE XuatHang
        SET TongTien = @TongTienMoi
        WHERE MaPhieuXuat = @MaPhieuXuat;
        RETURN 0;
    END TRY
    BEGIN CATCH
    END CATCH
END
GO
IF OBJECT_ID('sp_XoaMotChiTietPhieuXuat', 'P') IS NOT NULL 
    DROP PROCEDURE sp_XoaMotChiTietPhieuXuat
GO

GO

CREATE PROCEDURE sp_XoaMotChiTietPhieuXuat
    @MaPhieuXuat CHAR(5),
    @MaSP CHAR(5)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM ChiTietXuatHang WHERE MaPhieuXuat = @MaPhieuXuat AND MaSP = @MaSP)
    BEGIN
        RAISERROR (N'Chi tiết sản phẩm [%s] không tồn tại trong Phiếu Xuất [%s] để xóa.', 16, 1, @MaSP, @MaPhieuXuat);
        RETURN -1; 
    END

    BEGIN TRY
        DELETE FROM ChiTietXuatHang
        WHERE MaPhieuXuat = @MaPhieuXuat AND MaSP = @MaSP;

        DECLARE @TongTienMoi DECIMAL(18,2);
        SELECT @TongTienMoi = ISNULL(SUM(ISNULL(SoLuong, 0) * ISNULL(DonGia, 0)), 0) 
        FROM ChiTietXuatHang
        WHERE MaPhieuXuat = @MaPhieuXuat;

        UPDATE XuatHang
        SET TongTien = @TongTienMoi
        WHERE MaPhieuXuat = @MaPhieuXuat;
        
        RETURN 0;
    END TRY
    BEGIN CATCH
        RETURN -99; 
    END CATCH
END
GO
IF OBJECT_ID('sp_SuaMotChiTietPhieuXuat', 'P') IS NOT NULL 
    DROP PROCEDURE sp_SuaMotChiTietPhieuXuat
GO
GO

CREATE PROCEDURE sp_SuaMotChiTietPhieuXuat
    @MaPhieuXuat CHAR(5),
    @MaSP CHAR(5),
    @SoLuongMoi INT,
    @DonGiaMoi DECIMAL(18,2)
AS
BEGIN

    IF NOT EXISTS (SELECT 1 FROM ChiTietXuatHang WHERE MaPhieuXuat = @MaPhieuXuat AND MaSP = @MaSP)
    BEGIN
        RAISERROR (N'Chi tiết sản phẩm [%s] không tồn tại trong Phiếu Xuất [%s] để sửa.', 16, 1, @MaSP, @MaPhieuXuat);
        RETURN -1;
    END

    IF @SoLuongMoi <= 0
    BEGIN
        RAISERROR (N'Số lượng sản phẩm mới phải lớn hơn 0.', 16, 1);
        RETURN -2; 
    END

    IF @DonGiaMoi < 0
    BEGIN
        RAISERROR (N'Đơn giá sản phẩm mới không được âm.', 16, 1);
        RETURN -3;
    END

    BEGIN TRY
        UPDATE ChiTietXuatHang
        SET SoLuong = @SoLuongMoi,
            DonGia = @DonGiaMoi
        WHERE MaPhieuXuat = @MaPhieuXuat AND MaSP = @MaSP;

        DECLARE @TongTienMoiCuaPhieu DECIMAL(18,2);
        SELECT @TongTienMoiCuaPhieu = ISNULL(SUM(ISNULL(SoLuong, 0) * ISNULL(DonGia, 0)), 0)
        FROM ChiTietXuatHang
        WHERE MaPhieuXuat = @MaPhieuXuat;

        UPDATE XuatHang
        SET TongTien = @TongTienMoiCuaPhieu
        WHERE MaPhieuXuat = @MaPhieuXuat;
        
        RETURN 0;
    END TRY
    BEGIN CATCH
    END CATCH
END
GO



CREATE PROCEDURE sp_TimKiemPhieuXuat
    @TuKhoaChung NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @TuKhoaChung = ''
        SET @TuKhoaChung = NULL;

    SELECT
        px.MaPhieuXuat,
        px.NgayXuat,
        px.MaKH,
        px.MaNV,
        nv.HoTen AS TenNhanVien, 
        px.TongTien
    FROM XuatHang px
    INNER JOIN KhachHang kh ON px.MaKH = kh.MaKH
    INNER JOIN NhanVien nv ON px.MaNV = nv.MaNV
    WHERE
        (@TuKhoaChung IS NULL) OR
        (
            px.MaPhieuXuat LIKE '%' + @TuKhoaChung + '%' OR
            px.MaKH LIKE '%' + @TuKhoaChung + '%' OR
            px.MaNV LIKE '%' + @TuKhoaChung + '%'
        )
    ORDER BY px.NgayXuat DESC, px.MaPhieuXuat DESC;
END
GO

--Ton kho
Use KhangNghiDB
GO
IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_TonKho_MaTonKho' AND parent_object_id = OBJECT_ID('dbo.TonKho'))
BEGIN
    ALTER TABLE dbo.TonKho DROP CONSTRAINT DF_TonKho_MaTonKho;
END
GO
Drop sequence TonKho_MaTonKho_Seq

IF NOT EXISTS (SELECT * FROM sys.sequences WHERE name = 'TonKho_MaTonKho_Seq')
BEGIN
    CREATE SEQUENCE dbo.TonKho_MaTonKho_Seq
        AS INT
        START WITH 1
        INCREMENT BY 1
        MINVALUE 1
        NO CYCLE;
END
GO


IF OBJECT_ID('dbo.sp_LayTonKhoTheoThang') IS NOT NULL
    DROP PROCEDURE dbo.sp_LayTonKhoTheoThang;
GO

CREATE PROCEDURE sp_LayTonKhoTheoThang
    @Thang INT,
    @Nam INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NgayDauThangCuaKy DATE = DATEFROMPARTS(@Nam, @Thang, 1);
    DECLARE @NgayCuoiThangCuaKy DATE = EOMONTH(@NgayDauThangCuaKy);

    DECLARE @CalculatedStock TABLE (
        MaSP CHAR(5) PRIMARY KEY,
        TenSP NVARCHAR(100),
        DonViTinh NVARCHAR(20),
        SoLuongTonDauKy INT,
        SoLuongTonCuoiKy INT,
        NgayGiaoDichCuoiCungTrongKy DATE
    );

    WITH TonDauThangCTE AS (
        SELECT
            sp_ref.MaSP,
            ISNULL(tk_prev.SoLuongTonCuoiKy, 0) AS SoLuongTonDauThang
        FROM
            SanPham sp_ref
        LEFT JOIN
            TonKho tk_prev ON sp_ref.MaSP = tk_prev.MaSP
                           AND tk_prev.Ngay = DATEADD(DAY, -1, @NgayDauThangCuaKy)
    ),
    NhapTrongThangCTE AS (
        SELECT
            ctnh.MaSP,
            SUM(ISNULL(ctnh.SoLuong, 0)) AS TongNhap,
            MAX(nh.NgayNhap) AS NgayNhapCuoiCung
        FROM ChiTietNhapHang ctnh
        JOIN NhapHang nh ON ctnh.MaPhieuNhap = nh.MaPhieuNhap
        WHERE nh.NgayNhap BETWEEN @NgayDauThangCuaKy AND @NgayCuoiThangCuaKy
        GROUP BY ctnh.MaSP
    ),
    XuatTrongThangCTE AS (
        SELECT
            ctxh.MaSP,
            SUM(ISNULL(ctxh.SoLuong, 0)) AS TongXuat,
            MAX(xh.NgayXuat) AS NgayXuatCuoiCung
        FROM ChiTietXuatHang ctxh
        JOIN XuatHang xh ON ctxh.MaPhieuXuat = xh.MaPhieuXuat
        WHERE xh.NgayXuat BETWEEN @NgayDauThangCuaKy AND @NgayCuoiThangCuaKy
        GROUP BY ctxh.MaSP
    ),
    GiaoDichTrongKy AS ( 
        SELECT
            sp.MaSP,
            sp.TenSP,
            sp.DonViTinh,
            tdt.SoLuongTonDauThang,
            ISNULL(ntt.TongNhap, 0) AS TongNhap,
            ISNULL(xtt.TongXuat, 0) AS TongXuat,
            ntt.NgayNhapCuoiCung,
            xtt.NgayXuatCuoiCung
        FROM SanPham sp
        INNER JOIN TonDauThangCTE tdt ON sp.MaSP = tdt.MaSP
        LEFT JOIN NhapTrongThangCTE ntt ON sp.MaSP = ntt.MaSP
        LEFT JOIN XuatTrongThangCTE xtt ON sp.MaSP = xtt.MaSP
        WHERE
            (tdt.SoLuongTonDauThang > 0 OR ntt.TongNhap IS NOT NULL OR xtt.TongXuat IS NOT NULL)
    )
    INSERT INTO @CalculatedStock (MaSP, TenSP, DonViTinh, SoLuongTonDauKy, SoLuongTonCuoiKy, NgayGiaoDichCuoiCungTrongKy)
    SELECT
        gdtk.MaSP,
        gdtk.TenSP,
        gdtk.DonViTinh,
        gdtk.SoLuongTonDauThang,
        (gdtk.SoLuongTonDauThang + gdtk.TongNhap - gdtk.TongXuat) AS SoLuongTonCuoiKy,
        CASE
            WHEN gdtk.NgayNhapCuoiCung IS NULL AND gdtk.NgayXuatCuoiCung IS NULL THEN @NgayCuoiThangCuaKy
            WHEN gdtk.NgayNhapCuoiCung IS NOT NULL AND gdtk.NgayXuatCuoiCung IS NULL THEN gdtk.NgayNhapCuoiCung
            WHEN gdtk.NgayNhapCuoiCung IS NULL AND gdtk.NgayXuatCuoiCung IS NOT NULL THEN gdtk.NgayXuatCuoiCung
            WHEN gdtk.NgayNhapCuoiCung >= gdtk.NgayXuatCuoiCung THEN gdtk.NgayNhapCuoiCung
            ELSE gdtk.NgayXuatCuoiCung
        END AS NgayGiaoDichCuoiCungTrongKy
    FROM GiaoDichTrongKy gdtk;
    BEGIN TRANSACTION;
    SAVE TRANSACTION BeforeDeleteTonKho;

    BEGIN TRY
        DELETE tk
        FROM TonKho tk
        INNER JOIN @CalculatedStock cs ON tk.MaSP = cs.MaSP
        WHERE tk.Ngay BETWEEN @NgayDauThangCuaKy AND @NgayCuoiThangCuaKy;
       

        INSERT INTO TonKho (MaSP, Ngay, SoLuongTonDauKy, SoLuongTonCuoiKy, SoLuongTon)
        SELECT
            cs.MaSP,
            cs.NgayGiaoDichCuoiCungTrongKy,
            cs.SoLuongTonDauKy,
            cs.SoLuongTonCuoiKy,
            cs.SoLuongTonCuoiKy
        FROM @CalculatedStock cs;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION BeforeDeleteTonKho; 

        THROW;
    END CATCH
    SELECT
        tk.MaTonKho,
        cs.MaSP,
        cs.TenSP,
        cs.DonViTinh,
        cs.NgayGiaoDichCuoiCungTrongKy AS Ngay, 
        cs.SoLuongTonDauKy,
        cs.SoLuongTonCuoiKy,
        cs.SoLuongTonCuoiKy AS SoLuongTon
    FROM @CalculatedStock cs
    JOIN TonKho tk ON cs.MaSP = tk.MaSP AND tk.Ngay = cs.NgayGiaoDichCuoiCungTrongKy
    ORDER BY cs.TenSP, cs.NgayGiaoDichCuoiCungTrongKy;

END
GO

INSERT INTO NhaCungCap VALUES
('NCC01', N'Công ty TNHH Thiết bị Kỹ thuật Minh Long', 'minhlong@tech.com', '0909000001', N'123 Nguyễn Văn Cừ, Hà Nội'),
('NCC02', N'Công ty Cổ phần Phát triển Phú Mỹ', 'phumy@corp.vn', '0909000002', N'88 Trường Chinh, TP.HCM'),
('NCC03', N'Công ty TNHH TM&DV Thành Đạt', 'thanhdat@supplies.vn', '0909000003', N'56 Pasteur, Đà Nẵng'),
('NCC04', N'Tổng Công ty Thiết bị Sài Gòn', 'sales@saigontek.vn', '0909000004', N'01 Lý Thường Kiệt, TP.HCM'),
('NCC05', N'Công ty TNHH Thiết bị Hoàng Gia', 'info@hoanggia.com.vn', '0909000005', N'789 Huỳnh Tấn Phát, Cần Thơ'),
('NCC06', N'Công ty CP Công nghệ Nam Việt', 'support@namviet.com', '0909000006', N'19 Nguyễn Văn Linh, Huế'),
('NCC07', N'Công ty TNHH Thiết bị An Bình', 'anbinh@sup.vn', '0909000007', N'42 Điện Biên Phủ, Hải Phòng'),
('NCC08', N'Công ty CP Cơ khí Đông Á', 'dongaco@mech.com', '0909000008', N'12 Phan Đình Phùng, Quảng Ninh'),
('NCC09', N'Công ty TNHH Dịch vụ và Thiết bị Bách Khoa', 'bkservice@tech.vn', '0909000009', N'234 Trần Hưng Đạo, Bình Dương'),
('NCC10', N'Công ty TNHH Vật tư và Thiết bị Y tế Phúc An', 'phucan@med.vn', '0909000010', N'45 Võ Thị Sáu, Đồng Nai');

CREATE PROCEDURE sp_ThemNhaCungCap
    @MaNCC CHAR(5),
    @TenNCC NVARCHAR(100),
    @Email NVARCHAR(100),
    @SoDienThoai NVARCHAR(20),
    @DiaChi NVARCHAR(255)
AS
BEGIN
    INSERT INTO NhaCungCap (MaNCC, TenNCC, Email, SoDienThoai, DiaChi)
    VALUES (@MaNCC, @TenNCC, @Email, @SoDienThoai, @DiaChi)
END
GO

CREATE PROCEDURE sp_XoaNhaCungCap
    @MaNCC CHAR(5)
AS
BEGIN
    DELETE FROM NhaCungCap WHERE MaNCC = @MaNCC
END
GO

CREATE PROCEDURE sp_SuaNhaCungCap
    @MaNCC CHAR(5),
    @TenNCC NVARCHAR(100),
    @Email NVARCHAR(100),
    @SoDienThoai NVARCHAR(20),
    @DiaChi NVARCHAR(255)
AS
BEGIN
    UPDATE NhaCungCap
    SET TenNCC = @TenNCC,
        Email = @Email,
        SoDienThoai = @SoDienThoai,
        DiaChi = @DiaChi
    WHERE MaNCC = @MaNCC
END
GO

CREATE PROCEDURE sp_LayNhaCungCap
AS
BEGIN
    SELECT MaNCC, TenNCC, Email, SoDienThoai, DiaChi
    FROM NhaCungCap
END
GO


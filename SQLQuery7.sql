-- =============================================
-- 1. TẠO DATABASE VÀ BẢNG (GIỮ NGUYÊN CẤU TRÚC CŨ)
-- =============================================
CREATE DATABASE QuanLyCuaHangTV;
GO

USE QuanLyCuaHangTV;
GO

-- FormNhanVien
CREATE TABLE FormNhanVien (
    MaNhanVien NVARCHAR(50) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    ChucVu NVARCHAR(100),
    Luong DECIMAL(18, 2)
);

-- FormKhachHang
CREATE TABLE FormKhachHang (
    MaKhachHang NVARCHAR(50) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(255),
    SoDienThoai VARCHAR(20)
);

-- FormQuanLyTV
CREATE TABLE FormQuanLyTV (
    MaTIVI NVARCHAR(50) PRIMARY KEY,
    TenTIVI NVARCHAR(100) NOT NULL,
    KichThuoc NVARCHAR(50),
    HangSanXuat NVARCHAR(100),
    SoLuong INT,
    Gia DECIMAL(18, 2),
    BaoHanh NVARCHAR(50)
);

-- FormTonKho
CREATE TABLE FormTonKho (
    MaTonKho NVARCHAR(50) PRIMARY KEY,
    MaTIVI NVARCHAR(50),
    NgayCapNhat DATE,
    SoLuongTon INT NOT NULL DEFAULT 0,
    GhiChu NVARCHAR(500),
    FOREIGN KEY (MaTIVI) REFERENCES FormQuanLyTV(MaTIVI)
);

-- FormHoaDon
CREATE TABLE FormHoaDon (
    MaHoaDon NVARCHAR(50) PRIMARY KEY,
    MaNhanVien NVARCHAR(50),
    MaKhachHang NVARCHAR(50),
    NgayLap DATETIME,
    TongTien DECIMAL(18, 2), -- Cột này sẽ được Trigger tự động cập nhật
    FOREIGN KEY (MaNhanVien) REFERENCES FormNhanVien(MaNhanVien),
    FOREIGN KEY (MaKhachHang) REFERENCES FormKhachHang(MaKhachHang)
);

-- FormChiTietHoaDon
CREATE TABLE FormChiTietHoaDon (
    MaChiTietHoaDon NVARCHAR(50) PRIMARY KEY,
    MaHoaDon NVARCHAR(50),
    MaTIVI NVARCHAR(50),
    SoLuongMua INT NOT NULL,
    DonGia DECIMAL(18, 2), -- Giá này sẽ dùng để tính Tổng tiền
    FOREIGN KEY (MaHoaDon) REFERENCES FormHoaDon(MaHoaDon),
    FOREIGN KEY (MaTIVI) REFERENCES FormQuanLyTV(MaTIVI)
);
GO

-- =============================================
-- 2. TRIGGER QUAN TRỌNG: LIÊN KẾT TỔNG TIỀN VÀ ĐƠN GIÁ CHI TIẾT
-- =============================================
-- Mục đích: Mỗi khi Thêm/Sửa/Xóa ở bảng Chi Tiết -> Tự động tính lại Tổng Tiền ở bảng Hóa Đơn
CREATE TRIGGER trg_CapNhatTongTien
ON FormChiTietHoaDon
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Tìm danh sách các Mã Hóa Đơn bị thay đổi dữ liệu
    DECLARE @DanhSachHD TABLE (MaHoaDon NVARCHAR(50));
    
    INSERT INTO @DanhSachHD
    SELECT MaHoaDon FROM inserted -- Hóa đơn vừa thêm/sửa dòng chi tiết
    UNION
    SELECT MaHoaDon FROM deleted; -- Hóa đơn vừa bị xóa dòng chi tiết

    -- 2. Tính toán lại tổng tiền và cập nhật vào bảng FormHoaDon
    UPDATE FormHoaDon
    SET TongTien = (
        SELECT ISNULL(SUM(SoLuongMua * DonGia), 0)
        FROM FormChiTietHoaDon
        WHERE FormChiTietHoaDon.MaHoaDon = FormHoaDon.MaHoaDon
    )
    WHERE MaHoaDon IN (SELECT MaHoaDon FROM @DanhSachHD);
END;
GO





USE QuanLyCuaHangTV;
GO

-- =============================================
-- 1. THÊM DỮ LIỆU NHÂN VIÊN (5 người)
-- =============================================
INSERT INTO FormNhanVien (MaNhanVien, HoTen, ChucVu, Luong) VALUES 
('NV001', N'Nguyễn Văn An', N'Quản Lý', 20000000),
('NV002', N'Trần Thị Bích', N'Thu Ngân', 8000000),
('NV003', N'Lê Hoàng Nam', N'Tư Vấn Bán Hàng', 7000000),
('NV004', N'Phạm Minh Tuấn', N'Kỹ Thuật Viên', 9000000),
('NV005', N'Hoàng Thị Lan', N'Tư Vấn Bán Hàng', 7000000);

-- =============================================
-- 2. THÊM DỮ LIỆU KHÁCH HÀNG (10 khách)
-- =============================================
INSERT INTO FormKhachHang (MaKhachHang, HoTen, DiaChi, SoDienThoai) VALUES 
('KH001', N'Võ Thanh Tâm', N'123 Lê Lợi, TP.HCM', '0901111222'),
('KH002', N'Ngô Văn Hùng', N'45 Nguyễn Huệ, Hà Nội', '0902222333'),
('KH003', N'Đặng Thu Thảo', N'78 Đường 3/2, Cần Thơ', '0903333444'),
('KH004', N'Bùi Tiến Dũng', N'12 Trần Phú, Đà Nẵng', '0904444555'),
('KH005', N'Phan Thị Mơ', N'99 Hùng Vương, Huế', '0905555666'),
('KH006', N'Lý Hải', N'Tân Bình, TP.HCM', '0906666777'),
('KH007', N'Trấn Thành', N'Quận 7, TP.HCM', '0907777888'),
('KH008', N'Trường Giang', N'Quận 10, TP.HCM', '0908888999'),
('KH009', N'Mỹ Tâm', N'Sơn Trà, Đà Nẵng', '0909999000'),
('KH010', N'Sơn Tùng', N'Thái Bình', '0910000111');

-- =============================================
-- 3. THÊM DỮ LIỆU TIVI (10 sản phẩm)
-- =============================================
INSERT INTO FormQuanLyTV (MaTIVI, TenTIVI, KichThuoc, HangSanXuat, SoLuong, Gia, BaoHanh) VALUES 
('TV001', N'Sony Bravia 4K', N'55 Inch', N'Sony', 50, 15500000, N'24 Tháng'),
('TV002', N'Samsung QLED 8K', N'65 Inch', N'Samsung', 30, 35000000, N'24 Tháng'),
('TV003', N'LG OLED C1', N'48 Inch', N'LG', 40, 22000000, N'24 Tháng'),
('TV004', N'TCL Android TV', N'43 Inch', N'TCL', 100, 7500000, N'12 Tháng'),
('TV005', N'Casper Linux TV', N'32 Inch', N'Casper', 80, 4500000, N'12 Tháng'),
('TV006', N'Sony OLED A80J', N'65 Inch', N'Sony', 20, 48000000, N'36 Tháng'),
('TV007', N'Samsung Crystal UHD', N'50 Inch', N'Samsung', 60, 11000000, N'24 Tháng'),
('TV008', N'LG NanoCell', N'55 Inch', N'LG', 45, 16500000, N'24 Tháng'),
('TV009', N'Xiaomi TV P1', N'43 Inch', N'Xiaomi', 70, 8900000, N'18 Tháng'),
('TV010', N'Sharp Aquos', N'60 Inch', N'Sharp', 25, 13000000, N'12 Tháng');

-- =============================================
-- 4. THÊM DỮ LIỆU TỒN KHO
-- =============================================
INSERT INTO FormTonKho (MaTonKho, MaTIVI, NgayCapNhat, SoLuongTon, GhiChu) VALUES 
('TK001', 'TV001', '2023-10-01', 50, N'Kho chính'),
('TK002', 'TV002', '2023-10-01', 30, N'Kho chính'),
('TK003', 'TV003', '2023-10-02', 40, N'Kho VIP'),
('TK004', 'TV004', '2023-10-02', 100, N'Kho phụ'),
('TK005', 'TV005', '2023-10-03', 80, N'Kho phụ'),
('TK006', 'TV006', '2023-10-03', 20, N'Hàng trưng bày'),
('TK007', 'TV007', '2023-10-04', 60, N'Kho chính'),
('TK008', 'TV008', '2023-10-04', 45, N'Kho chính'),
('TK009', 'TV009', '2023-10-05', 70, N'Mới nhập'),
('TK010', 'TV010', '2023-10-05', 25, N'Hàng tồn lâu');

-- =============================================
-- 5. THÊM HÓA ĐƠN (8 Hóa đơn)
-- Lưu ý: TongTien để là 0, Trigger sẽ tự cập nhật sau khi thêm chi tiết
-- =============================================
INSERT INTO FormHoaDon (MaHoaDon, MaNhanVien, MaKhachHang, NgayLap, TongTien) VALUES 
('HD001', 'NV002', 'KH001', '2023-10-10', 0),
('HD002', 'NV003', 'KH002', '2023-10-11', 0),
('HD003', 'NV002', 'KH003', '2023-10-11', 0),
('HD004', 'NV005', 'KH004', '2023-10-12', 0),
('HD005', 'NV005', 'KH005', '2023-10-13', 0),
('HD006', 'NV002', 'KH001', '2023-10-14', 0), -- Khách cũ quay lại mua
('HD007', 'NV003', 'KH006', '2023-10-15', 0),
('HD008', 'NV005', 'KH007', '2023-10-16', 0);

-- =============================================
-- 6. THÊM CHI TIẾT HÓA ĐƠN (Sẽ kích hoạt Trigger cập nhật Tổng Tiền)
-- =============================================

-- Hóa đơn 1: Mua 1 cái Sony 4K
INSERT INTO FormChiTietHoaDon (MaChiTietHoaDon, MaHoaDon, MaTIVI, SoLuongMua, DonGia) 
VALUES ('CT001', 'HD001', 'TV001', 1, 15500000);

-- Hóa đơn 2: Mua 2 cái TCL (Cho quán cafe)
INSERT INTO FormChiTietHoaDon (MaChiTietHoaDon, MaHoaDon, MaTIVI, SoLuongMua, DonGia) 
VALUES ('CT002', 'HD002', 'TV004', 2, 7500000);

-- Hóa đơn 3: Mua 1 Samsung QLED + 1 Loa (Giả sử bán kèm) -> Ở đây chỉ demo TV
INSERT INTO FormChiTietHoaDon (MaChiTietHoaDon, MaHoaDon, MaTIVI, SoLuongMua, DonGia) 
VALUES ('CT003', 'HD003', 'TV002', 1, 35000000);

-- Hóa đơn 4: Mua 3 cái Casper (Giá rẻ)
INSERT INTO FormChiTietHoaDon (MaChiTietHoaDon, MaHoaDon, MaTIVI, SoLuongMua, DonGia) 
VALUES ('CT004', 'HD004', 'TV005', 3, 4500000);

-- Hóa đơn 5: Mua hàng cao cấp Sony OLED
INSERT INTO FormChiTietHoaDon (MaChiTietHoaDon, MaHoaDon, MaTIVI, SoLuongMua, DonGia) 
VALUES ('CT005', 'HD005', 'TV006', 1, 48000000);

-- Hóa đơn 6: Khách quen mua nhiều loại (2 dòng chi tiết)
INSERT INTO FormChiTietHoaDon (MaChiTietHoaDon, MaHoaDon, MaTIVI, SoLuongMua, DonGia) VALUES 
('CT006', 'HD006', 'TV007', 1, 11000000), -- Dòng 1
('CT007', 'HD006', 'TV009', 1, 8900000);  -- Dòng 2

-- Hóa đơn 7: LG NanoCell
INSERT INTO FormChiTietHoaDon (MaChiTietHoaDon, MaHoaDon, MaTIVI, SoLuongMua, DonGia) 
VALUES ('CT008', 'HD007', 'TV008', 2, 16500000);

-- Hóa đơn 8: Sharp Aquos
INSERT INTO FormChiTietHoaDon (MaChiTietHoaDon, MaHoaDon, MaTIVI, SoLuongMua, DonGia) 
VALUES ('CT009', 'HD008', 'TV010', 1, 13000000);
GO

-- =============================================
-- KIỂM TRA LẠI KẾT QUẢ
-- =============================================
SELECT * FROM FormNhanVien;
SELECT * FROM FormKhachHang;
SELECT * FROM FormQuanLyTV;



-- Bước 4: Xem kết quả
-- Bạn sẽ thấy TongTien ở FormHoaDon tự động thành 20000000.00
SELECT * FROM FormHoaDon WHERE MaHoaDon = 'HD001';
SELECT * FROM FormChiTietHoaDon WHERE MaHoaDon = 'HD001';

DROP TABLE FormChiTietHoaDon;
DROP TABLE FormHoaDon;
DROP TABLE FormTonKho;
DROP TABLE FormQuanLyTV;
DROP TABLE FormKhachHang;
DROP TABLE FormNhanVien;
GO

-- Code xóa database
USE master;
GO
ALTER DATABASE QuanLyCuaHangTV SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE QuanLyCuaHangTV;
GO
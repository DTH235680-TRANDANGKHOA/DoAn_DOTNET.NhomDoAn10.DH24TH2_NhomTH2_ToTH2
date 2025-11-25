-- =============================================
-- TẠO DATABASE MỚI
-- =============================================
CREATE DATABASE QuanLyCuaHangTV;
GO

USE QuanLyCuaHangTV;
GO

-- 1. FormNhanVien
CREATE TABLE FormNhanVien (
    MaNhanVien NVARCHAR(50) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    ChucVu NVARCHAR(100),
    Luong DECIMAL(18, 2)
);

-- 2. FormKhachHang
CREATE TABLE FormKhachHang (
    MaKhachHang NVARCHAR(50) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(255),
    SoDienThoai VARCHAR(20) -- Tên cột trong SQL
);

-- 3. FormQuanLyTV
CREATE TABLE FormQuanLyTV (
    MaTIVI NVARCHAR(50) PRIMARY KEY,
    TenTIVI NVARCHAR(100) NOT NULL,
    KichThuoc NVARCHAR(50),
    HangSanXuat NVARCHAR(100),
    SoLuong INT,
    Gia DECIMAL(18, 2),
    BaoHanh NVARCHAR(50)
);

-- 4. FormTonKho (ĐÃ SỬA)
CREATE TABLE FormTonKho (
    MaTonKho NVARCHAR(50) PRIMARY KEY,
    MaTIVI NVARCHAR(50),
    NgayCapNhat DATE,
    -- ĐÃ XÓA HangSanXuat (vì đã có ở FormQuanLyTV)
    SoLuongTon INT NOT NULL DEFAULT 0,
    GhiChu NVARCHAR(500),
    FOREIGN KEY (MaTIVI) REFERENCES FormQuanLyTV(MaTIVI)
);

-- 5. FormHoaDon
CREATE TABLE FormHoaDon (
    MaHoaDon NVARCHAR(50) PRIMARY KEY,
    MaNhanVien NVARCHAR(50),
    MaKhachHang NVARCHAR(50),
    NgayLap DATETIME,
    TongTien DECIMAL(18, 2),
    FOREIGN KEY (MaNhanVien) REFERENCES FormNhanVien(MaNhanVien),
    FOREIGN KEY (MaKhachHang) REFERENCES FormKhachHang(MaKhachHang)
);

-- 6. FormChiTietHoaDon
CREATE TABLE FormChiTietHoaDon (
    MaChiTietHoaDon NVARCHAR(50) PRIMARY KEY,
    MaHoaDon NVARCHAR(50),
    MaTIVI NVARCHAR(50),
    SoLuongMua INT NOT NULL,
    DonGia DECIMAL(18, 2),
    FOREIGN KEY (MaHoaDon) REFERENCES FormHoaDon(MaHoaDon),
    FOREIGN KEY (MaTIVI) REFERENCES FormQuanLyTV(MaTIVI)
);
GO
GO
USE QuanLyCuaHangTV;
GO

SELECT * FROM FormNhanVien;
SELECT * FROM FormKhachHang;
SELECT * FROM FormQuanLyTV;
SELECT * FROM FormTonKho;
SELECT * FROM FormHoaDon;
SELECT * FROM FormChiTietHoaDon;
GO
-- Code xóa bảng
USE QuanLyCuaHangTV;
GO
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
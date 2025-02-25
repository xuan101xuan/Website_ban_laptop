USE [master]
GO

CREATE DATABASE [TTPShopDB]
GO
ALTER DATABASE [TTPShopDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TTPShopDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TTPShopDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TTPShopDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TTPShopDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TTPShopDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TTPShopDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [TTPShopDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TTPShopDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TTPShopDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TTPShopDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TTPShopDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TTPShopDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TTPShopDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TTPShopDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TTPShopDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TTPShopDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TTPShopDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TTPShopDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TTPShopDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TTPShopDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TTPShopDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TTPShopDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TTPShopDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TTPShopDB] SET RECOVERY FULL 
GO
ALTER DATABASE [TTPShopDB] SET  MULTI_USER 
GO
ALTER DATABASE [TTPShopDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TTPShopDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TTPShopDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TTPShopDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TTPShopDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TTPShopDB', N'ON'
GO
ALTER DATABASE [TTPShopDB] SET QUERY_STORE = OFF
GO
USE [TTPShopDB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[IDCart] [int] IDENTITY(1,1) NOT NULL,
	[MaTK] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[MaSP] [int] NOT NULL,
 CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED 
(
	[IDCart] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietDH](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[SoDH] [int] NOT NULL,
	[MaSP] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhMuc](
	[MaDM] [int] IDENTITY(1,1) NOT NULL,
	[TenDM] [nvarchar](80) NOT NULL,
	[AnhDM] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonHang](
	[SoDH] [int] IDENTITY(1,1) NOT NULL,
	[NgayDat] [datetime] NOT NULL,
	[DiaChiGH] [nvarchar](70) NOT NULL,
	[TongTien] [int] NOT NULL,
	[MaTK] [int] NULL,
	[TinhTrang] [int] NULL,
	[HinhThucTT] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[SoDH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LienHe](
	[MaLH] [int] IDENTITY(1,1) NOT NULL,
	[DiaChi] [nvarchar](100) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[DienThoai] [varchar](11) NOT NULL,
	[ThoiGianLamViec] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaLH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPham](
	[MaSP] [int] IDENTITY(1,1) NOT NULL,
	[MaDM] [int] NOT NULL,
	[TenSP] [nvarchar](100) NOT NULL,
	[GiaBan] [int] NOT NULL,
	[AnhSP] [nvarchar](100) NULL,
	[MoTa] [nvarchar](max) NULL,
	[PhanTramKM] [int] NULL,
	[NgayTao] [datetime] NOT NULL,
	[SoLuong] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[MaTK] [int] IDENTITY(1,1) NOT NULL,
	[DiaChiEmail] [nvarchar](50) NOT NULL,
	[MatKhau] [varchar](20) NOT NULL,
	[HoTen] [nvarchar](40) NOT NULL,
	[VaiTro] [varchar](30) NULL,
	[GioiTinh] [nvarchar](4) NULL,
	[NgaySinh] [datetime] NULL,
	[SoDienThoai] [nvarchar](20) NULL,
	[TrangThai] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaTK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Cart] ON 

INSERT [dbo].[Cart] ([IDCart], [MaTK], [quantity], [MaSP]) VALUES (29, 1, 1, 83)
SET IDENTITY_INSERT [dbo].[Cart] OFF
GO
SET IDENTITY_INSERT [dbo].[ChiTietDH] ON 

INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (10, 10, 81, 1)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (15, 12, 78, 1)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (21, 15, 75, 1)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (27, 18, 81, 2)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (28, 19, 78, 2)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (29, 19, 77, 2)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (33, 21, 71, 1)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (34, 22, 83, 1)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (35, 23, 76, 2)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (36, 23, 68, 1)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (37, 24, 83, 2)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (38, 24, 81, 1)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (43, 10, 62, 1)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (44, 28, 89, 2)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (45, 29, 81, 1)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (47, 31, 89, 1)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (48, 32, 83, 1)
INSERT [dbo].[ChiTietDH] ([id], [SoDH], [MaSP], [SoLuong]) VALUES (49, 33, 83, 1)
SET IDENTITY_INSERT [dbo].[ChiTietDH] OFF
GO
SET IDENTITY_INSERT [dbo].[DanhMuc] ON 

INSERT [dbo].[DanhMuc] ([MaDM], [TenDM], [AnhDM]) VALUES (6, N'Laptop MacBook', N'logo-macbook-149x40.png')
INSERT [dbo].[DanhMuc] ([MaDM], [TenDM], [AnhDM]) VALUES (7, N'Laptop Asus', N'logo-asus-149x40.png')
INSERT [dbo].[DanhMuc] ([MaDM], [TenDM], [AnhDM]) VALUES (8, N'Laptop HP', N'logo-hp-149x40-1.png')
INSERT [dbo].[DanhMuc] ([MaDM], [TenDM], [AnhDM]) VALUES (9, N'Laptop Lenovo', N'logo-lenovo-149x40.png')
INSERT [dbo].[DanhMuc] ([MaDM], [TenDM], [AnhDM]) VALUES (10, N'Laptop Acer', N'logo-acer-149x40.png')
INSERT [dbo].[DanhMuc] ([MaDM], [TenDM], [AnhDM]) VALUES (11, N'Laptop Dell', N'logo-dell-149x40.png')
INSERT [dbo].[DanhMuc] ([MaDM], [TenDM], [AnhDM]) VALUES (12, N'Laptop MSI', N'logo-msi-149x40.png')
INSERT [dbo].[DanhMuc] ([MaDM], [TenDM], [AnhDM]) VALUES (13, N'Laptop LG', N'logo-lg-149x40.png')
INSERT [dbo].[DanhMuc] ([MaDM], [TenDM], [AnhDM]) VALUES (14, N'Laptop GIGABYTE', N'logo-gigabyte-149x40.png')
INSERT [dbo].[DanhMuc] ([MaDM], [TenDM], [AnhDM]) VALUES (19, N'Laptop Huawei', N'huawei-logo.jpg')
INSERT [dbo].[DanhMuc] ([MaDM], [TenDM], [AnhDM]) VALUES (21, N'Laptop Microsoft', N'microsoft-logo.png')
SET IDENTITY_INSERT [dbo].[DanhMuc] OFF
GO
SET IDENTITY_INSERT [dbo].[DonHang] ON 

INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (10, CAST(N'2022-11-06T20:32:35.623' AS DateTime), N'15 Khóm 1, Thị Trấn Mỹ Long, huyện Cầu Ngang, Trà Vinh', 109980000, 1, 3, 1)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (12, CAST(N'2022-11-15T18:59:36.227' AS DateTime), N'19 Đ.Nguyễn Hữu Thọ, Tân Hưng, Quận 7, Thành phố Hồ Chí Minh', 28990000, 2, 0, 0)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (15, CAST(N'2022-11-15T19:02:47.800' AS DateTime), N'19 Đ.Nguyễn Hữu Thọ, Tân Hưng, Quận 7, Thành phố Hồ Chí Minh', 29571300, 3, 0, 0)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (18, CAST(N'2022-11-15T19:04:53.270' AS DateTime), N'19 Đ.Nguyễn Hữu Thọ, Tân Hưng, Quận 7, Thành phố Hồ Chí Minh', 181980000, 2, 0, 0)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (19, CAST(N'2022-11-15T19:07:52.703' AS DateTime), N'164 Đường Tân Mỹ, Tân Thuận Tây, Quận 7, Thành phố Hồ Chí Minh', 99273000, 3, 0, 1)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (21, CAST(N'2022-11-22T00:00:00.000' AS DateTime), N'Hồ Chí Minh', 8000000, 2, 0, 0)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (22, CAST(N'2022-12-06T20:34:23.557' AS DateTime), N'Trà Vinh', 13500000, 1, 1, 0)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (23, CAST(N'2022-12-01T21:41:29.537' AS DateTime), N'Mỹ Long Nam, Cầu Ngang, Trà Vinh', 115014000, 1, 0, 0)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (24, CAST(N'2022-12-02T19:10:49.683' AS DateTime), N'Xã Mỹ Long Nam, Cầu Ngang, Trà Vinh', 117990000, 1, 0, 0)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (28, CAST(N'2022-12-19T11:48:11.013' AS DateTime), N'Xã Mỹ Long Nam, Cầu Ngang, Trà Vinh', 32300000, 1, 0, 1)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (29, CAST(N'2022-12-19T11:51:06.263' AS DateTime), N'Xã Mỹ Long Nam, Cầu Ngang, Trà Vinh', 90990000, 1, 0, 0)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (31, CAST(N'2022-12-19T20:25:31.690' AS DateTime), N'Xã Mỹ Long Nam, Cầu Ngang, Trà Vinh', 16150000, 17, 0, 0)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (32, CAST(N'2022-12-19T22:36:23.300' AS DateTime), N'Quận 7', 13500000, 17, 0, 1)
INSERT [dbo].[DonHang] ([SoDH], [NgayDat], [DiaChiGH], [TongTien], [MaTK], [TinhTrang], [HinhThucTT]) VALUES (33, CAST(N'2022-12-21T10:09:05.743' AS DateTime), N'19 Đ.Nguyễn Hữu Thọ, Tân Hưng, Quận 7, Thành phố Hồ Chí Minh', 12490000, 1, 0, 1)
SET IDENTITY_INSERT [dbo].[DonHang] OFF
GO
SET IDENTITY_INSERT [dbo].[LienHe] ON 

INSERT [dbo].[LienHe] ([MaLH], [DiaChi], [Email], [DienThoai], [ThoiGianLamViec]) VALUES (2, N'475A Điện Biên Phủ, Phường 25, Quận Bình Thạnh, Thành phố Hồ Chí Minh', N'TTPShop@hotmail.com', N'0987654321', N'Thứ 2 đến thứ 7: 8h đến 22h')
SET IDENTITY_INSERT [dbo].[LienHe] OFF
GO
SET IDENTITY_INSERT [dbo].[SanPham] ON 

INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (55, 14, N'GIGABYTE Gaming G5 i5 11400H (51S1123SH)', 29990000, N'gigabyte_gaming_g5_2_i5.png', N'gigabyte-gaming-g5-i5', 10, CAST(N'2022-11-15T17:55:16.000' AS DateTime), 122)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (57, 14, N'GIGABYTE Gaming G5 i5 11400H (51S1121SH)', 26990000, N'gigabyte_gaming_g5_1_i5.png', N'gigabyte-g5-i5-51s1121sh', 10, CAST(N'2022-11-15T17:59:04.000' AS DateTime), 26)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (59, 13, N'LG Gram 17 2021 i7 1165G7 (17Z90P-G.AH76A5)', 52890000, N'lg_gram_17_2021_i7.png', N'lg-gram-17-i7', 0, CAST(N'2022-11-15T18:03:43.000' AS DateTime), 123)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (60, 13, N'LG Gram 16 2021 i7 1165G7 (16Z90P-G.AH75A5)', 50890000, N'lg_gram_16_2021_i7.png', N'lg-gram-16-i7-16z90pgah', 0, CAST(N'2022-11-15T18:04:45.000' AS DateTime), 122)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (61, 12, N'MSI Gaming GF65 Thin 10UE i5 10500H (286VN)', 29490000, N'msi_gaming_gf65_thin_10ue_i5.png', N'msi-gamin-gf65-thin-10ue-i5', 5, CAST(N'2022-11-15T18:06:15.000' AS DateTime), 123)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (62, 12, N'MSI Modern 15 A11MU i5 1155G7 (680VN)', 18990000, N'msi_modern_15_a11mu_i5.png', N'msi-modern-15-a11mu-i5', 0, CAST(N'2022-11-15T18:07:44.000' AS DateTime), 15)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (63, 12, N'MSI Modern 14 B11MOU i3 1115G4 (849VN)', 14190000, N'msi_modern_14_b11mou_i3.png', N'msi-modern-14-b11mou', 0, CAST(N'2022-11-15T18:08:48.000' AS DateTime), 10)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (64, 11, N'Dell Vostro 3400 i5 1135G7 (70253900)', 18890000, N'dell_vostro_3400_i5.png', N'dell-vostro-3400-i5', 0, CAST(N'2022-11-15T18:11:16.000' AS DateTime), 20)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (66, 11, N'Dell Gaming G3 15 i7 10750H (P89F002BWH)', 31990000, N'dell_gaming_g3_15_i7.png', N'dell-g3-15-i7', 0, CAST(N'2022-11-15T18:14:16.000' AS DateTime), 21)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (67, 10, N'Acer Nitro 5 Gaming AN515 57 727J i7 11800H', 29990000, N'acer_nitro_5_gaming_an515_57_727j_i7.png', N'acer-nitro-gaming-an515', 6, CAST(N'2022-11-15T18:16:18.000' AS DateTime), 20)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (68, 10, N'Acer Nitro 5 Gaming AN515 57 5831 i5 11400H', 32990000, N'acer_nitro_5_gaming_an515_57_5831_i5.png', N'acer-nitro-5-gaming-an515', 5, CAST(N'2022-11-15T18:17:18.000' AS DateTime), 25)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (89, 10, N'Acer Nitro AN515 43 R9FD R5 3550H/8GB/512GB/4GB GTX1650/Win10 (NH.Q6ZSV.003) ', 18990000, N'acer_nitro_an515_43_r9fd_r5_3550h.png', N'acer-nitro-an515-43', 5, CAST(N'2022-12-06T12:44:50.000' AS DateTime), 10)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (70, 9, N'Lenovo Ideapad 5 Pro 14ITL6 i5 1135G7 (82L30094VN)', 23090000, N'lenovo_ideapad_5_pro_14itl6_i5.png', N'', 0, CAST(N'2022-11-15T18:19:20.000' AS DateTime), 121)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (71, 9, N'Lenovo Yoga 9 14ITL5 i7/1185G7 (82BG006EVN)', 49990000, N'lenovo_yoga_9_14itl5_i7.png', N'lenovo-yoga-9-14itl5-i7', 10, CAST(N'2022-11-15T18:20:32.000' AS DateTime), 50)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (72, 9, N'Lenovo YOGA Slim 7 Carbon 13ITL5 i7 1165G7', 34990000, N'lenovo_yoga_slim_7_carbon_13itl5_i7.png', N'lenovo-yoga-slim-7-carbon', 5, CAST(N'2022-11-15T18:22:02.000' AS DateTime), 123)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (73, 8, N'HP Envy 13 ba1030TU i7 1165G7 (2K0B6PA)', 30390000, N'hp_elitebook_x360_830_g8_i7.png', N'hp-envy-13-ba1030tu-i7', 0, CAST(N'2022-11-15T18:23:05.000' AS DateTime), 15)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (74, 8, N'HP EliteBook X360 830 G8 i7 1165G7 (3G1A4PA)', 41590000, N'hp-elitebook-x360-830-g8-i7-3g1a4pa-19-600x600.jpg', N'hp-elitebook-x360-830', 7, CAST(N'2022-11-15T18:24:59.000' AS DateTime), 55)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (75, 8, N'HP Envy 13 ba1031TU i7 1165G7 (2K0B7PA)', 33690000, N'hp_envy_13_ba1031tu_i7.png', N'', 0, CAST(N'2022-11-15T18:26:25.000' AS DateTime), 5)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (76, 7, N'Asus ROG Zephyrus G14 Alan Walker GA401QEC R9 (K2064T)', 49990000, N'asus_rog_zephyrus_g14_r9.png', N'asus-rog-zephyrus-gaming-g14', 10, CAST(N'2022-11-15T18:27:45.000' AS DateTime), 15)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (77, 7, N'Asus ZenBook UX325EA i5 1135G7 (KG363T)', 23790000, N'asus_zenbook_ux325ea_i5.png', N'asus-zenbook-ux325ea-i5-kg363t', 0, CAST(N'2022-11-15T18:28:37.000' AS DateTime), 20)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (78, 7, N'Asus TUF Gaming FX516PC i7 11370H/8GB/512GB/4GB RTX3050/144Hz/Win10 (HN001T)', 28990000, N'asus_tuf_gaming_i7.png', N'asus-tuf-gaming-fx516pc-i7', 6, CAST(N'2022-11-15T18:29:47.000' AS DateTime), 10)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (79, 6, N'MacBook Air M1 2020 7-core GPU', 28990000, N'macbook_air_m1_2020_7core.png', N'macbook-air-m1-2020', 0, CAST(N'2022-11-15T18:31:21.000' AS DateTime), 10)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (80, 6, N'MacBook Pro M1 2020', 44990000, N'macbook_pro_m1_2020_8core.png', N'macbook-pro-m1', 0, CAST(N'2022-11-15T18:32:12.000' AS DateTime), 123)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (81, 6, N'MacBook Pro 16 M1 Max 2021/32 core-GPU', 90990000, N'macbook_pro_16_m1_max_2021_32core.png', N'apple-macbook-pro-16-m1', 0, CAST(N'2022-11-15T18:33:34.000' AS DateTime), 200)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (83, 19, N'Huawei MateBook D 15 R5 3500U 8GB/256GB+1TB/Win10 (Boh-WAQ9R)', 12490000, N'huawei_matebook_d15_r5_3500U.png', N'huawei-matebook', 0, CAST(N'2022-11-22T10:42:12.000' AS DateTime), 10)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (99, 19, N'Laptop Huawei MateBook 13 i5 10210U/16GB/512GB/2GB MX250/Win10', 29990000, N'huawei_matebook_13_i5.png', N'Laptop Huawei MateBook 13 i5', 5, CAST(N'2022-12-20T16:38:48.400' AS DateTime), 10)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (97, 21, N'Laptop Microsoft Surface Book i7', 58579000, N'microsoft_surface_book_i7.png', N'Laptop Microsoft Surface Book', 1, CAST(N'2022-12-20T16:30:17.167' AS DateTime), 15)
INSERT [dbo].[SanPham] ([MaSP], [MaDM], [TenSP], [GiaBan], [AnhSP], [MoTa], [PhanTramKM], [NgayTao], [SoLuong]) VALUES (98, 21, N'Surface Laptop Studio (2021) – Intel Core i7/16GB/512GB RTX-3050 Ti', 72000000, N'microsoft_surface_laptop_studio_2021_i7.png', N'Surface Laptop Studio (2021)', 8, CAST(N'2022-12-20T16:34:12.167' AS DateTime), 10)

SET IDENTITY_INSERT [dbo].[SanPham] OFF
GO
SET IDENTITY_INSERT [dbo].[TaiKhoan] ON 

INSERT [dbo].[TaiKhoan] ([MaTK], [DiaChiEmail], [MatKhau], [HoTen], [VaiTro], [GioiTinh], [NgaySinh], [SoDienThoai], [TrangThai]) VALUES (1, N'admin@gmail.com', N'MTIzNDU2', N'Admin', N'Administrator', N'Nam', CAST(N'2000-09-16T00:00:00.000' AS DateTime), N'079586345', 1)
INSERT [dbo].[TaiKhoan] ([MaTK], [DiaChiEmail], [MatKhau], [HoTen], [VaiTro], [GioiTinh], [NgaySinh], [SoDienThoai], [TrangThai]) VALUES (2, N'chiduc@gmail.com', N'MTIzNDU2', N'Chí Đức', N'User', N'Nam', CAST(N'2003-11-15T00:00:00.000' AS DateTime), N'097584236', 1)
INSERT [dbo].[TaiKhoan] ([MaTK], [DiaChiEmail], [MatKhau], [HoTen], [VaiTro], [GioiTinh], [NgaySinh], [SoDienThoai], [TrangThai]) VALUES (3, N'user2@gmail.com', N'MTIzNDU2', N'Nguyễn Thị Hoài Thương', N'User', N'Nữ', CAST(N'2002-08-28T00:00:00.000' AS DateTime), N'078965482', 0)
INSERT [dbo].[TaiKhoan] ([MaTK], [DiaChiEmail], [MatKhau], [HoTen], [VaiTro], [GioiTinh], [NgaySinh], [SoDienThoai], [TrangThai]) VALUES (10, N'tung@gmail.com', N'MTIzNDU2', N'Trần Tùng', N'User', N'Nam', CAST(N'2000-09-15T00:00:00.000' AS DateTime), N'095874263', 1)
INSERT [dbo].[TaiKhoan] ([MaTK], [DiaChiEmail], [MatKhau], [HoTen], [VaiTro], [GioiTinh], [NgaySinh], [SoDienThoai], [TrangThai]) VALUES (12, N'hongduc@gmail.com', N'MTIzNDU2', N'Hồng Đức', N'User', N'Nam', CAST(N'2000-01-18T00:00:00.000' AS DateTime), N'035874698', 1)
INSERT [dbo].[TaiKhoan] ([MaTK], [DiaChiEmail], [MatKhau], [HoTen], [VaiTro], [GioiTinh], [NgaySinh], [SoDienThoai], [TrangThai]) VALUES (17, N'thienvan@gmail.com', N'MTIzNDU2', N'Vân Thiên', N'Administrator', N'Nam', CAST(N'2021-12-05T00:00:00.000' AS DateTime), N'025478965', 1)
INSERT [dbo].[TaiKhoan] ([MaTK], [DiaChiEmail], [MatKhau], [HoTen], [VaiTro], [GioiTinh], [NgaySinh], [SoDienThoai], [TrangThai]) VALUES (18, N'phuongvi@gmail.com', N'MTIzNDU2', N'Mai Võ Phương Vi', N'User', N'Nữ', CAST(N'2000-01-17T00:00:00.000' AS DateTime), N'032579658', 1)
INSERT [dbo].[TaiKhoan] ([MaTK], [DiaChiEmail], [MatKhau], [HoTen], [VaiTro], [GioiTinh], [NgaySinh], [SoDienThoai], [TrangThai]) VALUES (20, N'hieuthanh@gmail.com', N'MTIzNDU2', N'Thanh Hiếu', N'User', N'Nam', CAST(N'2000-09-15T00:00:00.000' AS DateTime), N'0356987458', 1)
INSERT [dbo].[TaiKhoan] ([MaTK], [DiaChiEmail], [MatKhau], [HoTen], [VaiTro], [GioiTinh], [NgaySinh], [SoDienThoai], [TrangThai]) VALUES (21, N'van@gmail.com', N'MTIzNDU2', N'User 1', N'User', N'Nam', CAST(N'2021-12-21T00:00:00.000' AS DateTime), N'0325479658', 1)
INSERT [dbo].[TaiKhoan] ([MaTK], [DiaChiEmail], [MatKhau], [HoTen], [VaiTro], [GioiTinh], [NgaySinh], [SoDienThoai], [TrangThai]) VALUES (22, N'thien@gmail.com', N'MTIzNDU2', N'User 1', N'User', N'Nam', CAST(N'2021-12-21T00:00:00.000' AS DateTime), N'023654879', 1)
INSERT [dbo].[TaiKhoan] ([MaTK], [DiaChiEmail], [MatKhau], [HoTen], [VaiTro], [GioiTinh], [NgaySinh], [SoDienThoai], [TrangThai]) VALUES (23, N'minhtu@gmail.com', N'MTIzNDU2', N'User 2', N'User', N'Nam', CAST(N'2021-12-21T00:00:00.000' AS DateTime), N'095478654', 1)
SET IDENTITY_INSERT [dbo].[TaiKhoan] OFF
GO
SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[TaiKhoan] ADD UNIQUE NONCLUSTERED 
(
	[DiaChiEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[TaiKhoan] ADD UNIQUE NONCLUSTERED 
(
	[DiaChiEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_SanPham] FOREIGN KEY([MaSP])
REFERENCES [dbo].[SanPham] ([MaSP])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_SanPham]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_TaiKhoan] FOREIGN KEY([MaTK])
REFERENCES [dbo].[TaiKhoan] ([MaTK])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_TaiKhoan]
GO
ALTER TABLE [dbo].[ChiTietDH]  WITH CHECK ADD  CONSTRAINT [fkHoaDon1] FOREIGN KEY([SoDH])
REFERENCES [dbo].[DonHang] ([SoDH])
GO
ALTER TABLE [dbo].[ChiTietDH] CHECK CONSTRAINT [fkHoaDon1]
GO
ALTER TABLE [dbo].[ChiTietDH]  WITH CHECK ADD  CONSTRAINT [fkHoaDon2] FOREIGN KEY([MaSP])
REFERENCES [dbo].[SanPham] ([MaSP])
GO
ALTER TABLE [dbo].[ChiTietDH] CHECK CONSTRAINT [fkHoaDon2]
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD FOREIGN KEY([MaTK])
REFERENCES [dbo].[TaiKhoan] ([MaTK])
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD FOREIGN KEY([MaTK])
REFERENCES [dbo].[TaiKhoan] ([MaTK])
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD FOREIGN KEY([MaDM])
REFERENCES [dbo].[DanhMuc] ([MaDM])
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD FOREIGN KEY([MaDM])
REFERENCES [dbo].[DanhMuc] ([MaDM])
GO
USE [master]
GO
ALTER DATABASE [TTPShopDB] SET  READ_WRITE 
GO

USE [master]
GO
/****** Object:  Database [FreshChoice]    Script Date: 1/5/2021 1:37:31 PM ******/
CREATE DATABASE [FreshChoice]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FreshChoice', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\FreshChoice.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FreshChoice_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\FreshChoice_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [FreshChoice] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FreshChoice].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FreshChoice] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FreshChoice] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FreshChoice] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FreshChoice] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FreshChoice] SET ARITHABORT OFF 
GO
ALTER DATABASE [FreshChoice] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FreshChoice] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FreshChoice] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FreshChoice] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FreshChoice] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FreshChoice] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FreshChoice] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FreshChoice] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FreshChoice] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FreshChoice] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FreshChoice] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FreshChoice] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FreshChoice] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FreshChoice] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FreshChoice] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FreshChoice] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FreshChoice] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FreshChoice] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [FreshChoice] SET  MULTI_USER 
GO
ALTER DATABASE [FreshChoice] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FreshChoice] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FreshChoice] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FreshChoice] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FreshChoice] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FreshChoice] SET QUERY_STORE = OFF
GO
USE [FreshChoice]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[AddressName] [varchar](50) NOT NULL,
	[AddressDescription] [varchar](250) NOT NULL,
	[AddressLongitude] [float] NOT NULL,
	[AddressLatitude] [float] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Brand]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brand](
	[BrandId] [int] IDENTITY(1,1) NOT NULL,
	[BrandName] [varchar](100) NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_Brand] PRIMARY KEY CLUSTERED 
(
	[BrandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[CartId] [int] IDENTITY(1,1) NOT NULL,
	[CartIsActive] [bit] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED 
(
	[CartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartItem]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartItem](
	[CartItemId] [int] IDENTITY(1,1) NOT NULL,
	[CartItemQnt] [int] NOT NULL,
	[CartItemTotalPrice] [float] NOT NULL,
	[ItemId] [int] NOT NULL,
	[CartId] [int] NOT NULL,
 CONSTRAINT [PK_CartItem] PRIMARY KEY CLUSTERED 
(
	[CartItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Delivery]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Delivery](
	[DeliveryId] [int] IDENTITY(1,1) NOT NULL,
	[DeliveryType] [varchar](20) NOT NULL,
	[UserId] [int] NULL,
	[DeliveryFee] [float] NULL,
 CONSTRAINT [PK_Delivery] PRIMARY KEY CLUSTERED 
(
	[DeliveryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[ImageId] [int] IDENTITY(1,1) NOT NULL,
	[ImageBlob] [image] NULL,
	[ImageUrl] [varchar](200) NOT NULL,
	[ImageDescription] [varchar](150) NOT NULL,
	[ItemId] [int] NOT NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
(
	[ImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[ItemId] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [varchar](150) NOT NULL,
	[ItemDescription] [varchar](3300) NOT NULL,
	[ItemAvailableQnt] [int] NOT NULL,
	[ItemQnt] [int] NOT NULL,
	[ItemPrice] [float] NOT NULL,
	[BrandId] [int] NOT NULL,
	[ItemIsDeleted] [bit] NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[OrderDescription] [varchar](250) NOT NULL,
	[OrderBillNo] [varchar](80) NOT NULL,
	[OrderConfirmationNo] [varchar](40) NOT NULL,
	[OrderUpdatedDate] [datetime] NOT NULL,
	[OrderNextDeadline] [datetime] NOT NULL,
	[DeliveryId] [int] NULL,
	[OrderStatusId] [int] NOT NULL,
	[CartId] [int] NOT NULL,
	[OrderAmount] [float] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatus]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatus](
	[OrderStatusId] [int] IDENTITY(1,1) NOT NULL,
	[OrderStatusDescription] [varchar](50) NOT NULL,
	[OrderStatusOrder] [int] NOT NULL,
 CONSTRAINT [PK_OrderStatus] PRIMARY KEY CLUSTERED 
(
	[OrderStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[TransactionDescription] [varchar](200) NOT NULL,
	[TransactionAmount] [float] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[WalletId] [int] NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](100) NOT NULL,
	[UserEmail] [varchar](150) NOT NULL,
	[UserContact] [varchar](10) NOT NULL,
	[RoleId] [int] NOT NULL,
	[UserPassword] [varchar](300) NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[ActivationCode] [uniqueidentifier] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 1/5/2021 1:37:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallet](
	[WalletId] [int] IDENTITY(1,1) NOT NULL,
	[WalletDescription] [varchar](100) NOT NULL,
	[WalletTotal] [float] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Wallet] PRIMARY KEY CLUSTERED 
(
	[WalletId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Address] ON 

INSERT [dbo].[Address] ([AddressId], [AddressName], [AddressDescription], [AddressLongitude], [AddressLatitude], [UserId]) VALUES (4, N'My Home', N'457, Rathnapura Road, Munagama, Horana', 80.0762768, 6.7222113, 6)
SET IDENTITY_INSERT [dbo].[Address] OFF
SET IDENTITY_INSERT [dbo].[Brand] ON 

INSERT [dbo].[Brand] ([BrandId], [BrandName], [CategoryId]) VALUES (1, N'Unknown', 1)
INSERT [dbo].[Brand] ([BrandId], [BrandName], [CategoryId]) VALUES (2, N'Keels', 1)
INSERT [dbo].[Brand] ([BrandId], [BrandName], [CategoryId]) VALUES (3, N'Tomato Products', 2)
INSERT [dbo].[Brand] ([BrandId], [BrandName], [CategoryId]) VALUES (4, N'Apple', 3)
INSERT [dbo].[Brand] ([BrandId], [BrandName], [CategoryId]) VALUES (5, N'Samsung', 3)
INSERT [dbo].[Brand] ([BrandId], [BrandName], [CategoryId]) VALUES (6, N'JBS 2', 4)
SET IDENTITY_INSERT [dbo].[Brand] OFF
SET IDENTITY_INSERT [dbo].[Cart] ON 

INSERT [dbo].[Cart] ([CartId], [CartIsActive], [UserId]) VALUES (1, 1, 3)
INSERT [dbo].[Cart] ([CartId], [CartIsActive], [UserId]) VALUES (2, 1, 5)
INSERT [dbo].[Cart] ([CartId], [CartIsActive], [UserId]) VALUES (3, 0, 6)
INSERT [dbo].[Cart] ([CartId], [CartIsActive], [UserId]) VALUES (4, 0, 6)
INSERT [dbo].[Cart] ([CartId], [CartIsActive], [UserId]) VALUES (5, 0, 6)
INSERT [dbo].[Cart] ([CartId], [CartIsActive], [UserId]) VALUES (6, 0, 6)
INSERT [dbo].[Cart] ([CartId], [CartIsActive], [UserId]) VALUES (7, 0, 6)
INSERT [dbo].[Cart] ([CartId], [CartIsActive], [UserId]) VALUES (8, 0, 6)
SET IDENTITY_INSERT [dbo].[Cart] OFF
SET IDENTITY_INSERT [dbo].[CartItem] ON 

INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (1, 4, 800, 1, 1)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (2, 5, 2500, 2, 1)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (3, 3, 600, 4, 1)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (4, 1, 200, 3, 1)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (5, 1, 200, 3, 2)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (6, 1, 500, 2, 2)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (7, 2, 334000, 5, 2)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (8, 1, 200, 3, 3)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (9, 1, 200, 4, 4)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (10, 1, 200, 3, 4)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (11, 1, 500, 2, 4)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (12, 1, 200, 1, 4)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (13, 1, 200, 3, 5)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (14, 1, 200, 1, 6)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (15, 1, 200, 1, 7)
INSERT [dbo].[CartItem] ([CartItemId], [CartItemQnt], [CartItemTotalPrice], [ItemId], [CartId]) VALUES (16, 1, 200, 4, 8)
SET IDENTITY_INSERT [dbo].[CartItem] OFF
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (1, N'Vegetables')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (2, N'Fruits')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (3, N'Electronics')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (4, N'ELAO')
SET IDENTITY_INSERT [dbo].[Category] OFF
SET IDENTITY_INSERT [dbo].[Delivery] ON 

INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (1, N'DELIVERY', 4, NULL)
INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (2, N'PICKUP', NULL, NULL)
INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (3, N'PICKUP', NULL, NULL)
INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (4, N'PICKUP', NULL, NULL)
INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (5, N'PICKUP', NULL, NULL)
INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (6, N'DELIVERY', NULL, NULL)
INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (7, N'DELIVERY', NULL, NULL)
INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (8, N'DELIVERY', NULL, 1053.7886143568687)
INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (9, N'PICKUP', NULL, NULL)
INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (10, N'DELIVERY', NULL, 1053.7886143568687)
INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (11, N'DELIVERY', NULL, 1053.7886143568687)
INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (12, N'DELIVERY', NULL, 1053.7886143568687)
INSERT [dbo].[Delivery] ([DeliveryId], [DeliveryType], [UserId], [DeliveryFee]) VALUES (13, N'DELIVERY', NULL, 1053.7886143568687)
SET IDENTITY_INSERT [dbo].[Delivery] OFF
SET IDENTITY_INSERT [dbo].[Image] ON 

INSERT [dbo].[Image] ([ImageId], [ImageBlob], [ImageUrl], [ImageDescription], [ItemId]) VALUES (3, NULL, N'/Uploads/Carrot 500g_0.jpg', N'', 3)
INSERT [dbo].[Image] ([ImageId], [ImageBlob], [ImageUrl], [ImageDescription], [ItemId]) VALUES (4, NULL, N'/Uploads/Carrot 500g_0.jpg', N'', 3)
INSERT [dbo].[Image] ([ImageId], [ImageBlob], [ImageUrl], [ImageDescription], [ItemId]) VALUES (5, NULL, N'/Uploads/Carrot 500g_2.jpg', N'', 3)
INSERT [dbo].[Image] ([ImageId], [ImageBlob], [ImageUrl], [ImageDescription], [ItemId]) VALUES (6, NULL, N'/Uploads/Apple 500g_0.jpg', N'', 4)
INSERT [dbo].[Image] ([ImageId], [ImageBlob], [ImageUrl], [ImageDescription], [ItemId]) VALUES (7, NULL, N'/Uploads/Tomato 1kg_0.jpg', N'', 2)
INSERT [dbo].[Image] ([ImageId], [ImageBlob], [ImageUrl], [ImageDescription], [ItemId]) VALUES (8, NULL, N'/Uploads/Beat Root 500g_0.jpg', N'', 1)
INSERT [dbo].[Image] ([ImageId], [ImageBlob], [ImageUrl], [ImageDescription], [ItemId]) VALUES (9, NULL, N'/Uploads/Iphone XS_0.jpg', N'', 5)
SET IDENTITY_INSERT [dbo].[Image] OFF
SET IDENTITY_INSERT [dbo].[Item] ON 

INSERT [dbo].[Item] ([ItemId], [ItemName], [ItemDescription], [ItemAvailableQnt], [ItemQnt], [ItemPrice], [BrandId], [ItemIsDeleted]) VALUES (1, N'Beat Root 500g', N'Lorem ipsum is placeholder text commonly used in the graphic, print, and publishing industries for previewing layouts and visual mockups.', 0, 5, 200, 1, 0)
INSERT [dbo].[Item] ([ItemId], [ItemName], [ItemDescription], [ItemAvailableQnt], [ItemQnt], [ItemPrice], [BrandId], [ItemIsDeleted]) VALUES (2, N'Tomato 1kg', N'Lorem ipsum is placeholder text commonly used in the graphic, print, and publishing industries for previewing layouts and visual mockups.', 0, 6, 500, 1, NULL)
INSERT [dbo].[Item] ([ItemId], [ItemName], [ItemDescription], [ItemAvailableQnt], [ItemQnt], [ItemPrice], [BrandId], [ItemIsDeleted]) VALUES (3, N'Carrot 500g', N'Lorem ipsum is placeholder text commonly used in the graphic, print, and publishing industries for previewing layouts and visual mockups.', 0, 5, 200, 1, NULL)
INSERT [dbo].[Item] ([ItemId], [ItemName], [ItemDescription], [ItemAvailableQnt], [ItemQnt], [ItemPrice], [BrandId], [ItemIsDeleted]) VALUES (4, N'Apple 500g', N'Good Apple', 1, 3, 200, 3, NULL)
INSERT [dbo].[Item] ([ItemId], [ItemName], [ItemDescription], [ItemAvailableQnt], [ItemQnt], [ItemPrice], [BrandId], [ItemIsDeleted]) VALUES (5, N'Iphone XS', N'213', 0, 2, 167000, 4, 0)
SET IDENTITY_INSERT [dbo].[Item] OFF
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([OrderId], [OrderDescription], [OrderBillNo], [OrderConfirmationNo], [OrderUpdatedDate], [OrderNextDeadline], [DeliveryId], [OrderStatusId], [CartId], [OrderAmount], [UserId]) VALUES (4, N'New Order', N'BS_2312', N'JSDOI_3245', CAST(N'2020-12-16T12:19:22.137' AS DateTime), CAST(N'2021-01-02T12:19:00.000' AS DateTime), NULL, 6, 1, 1500, 3)
INSERT [dbo].[Order] ([OrderId], [OrderDescription], [OrderBillNo], [OrderConfirmationNo], [OrderUpdatedDate], [OrderNextDeadline], [DeliveryId], [OrderStatusId], [CartId], [OrderAmount], [UserId]) VALUES (6, N'New Order', N'SFS34323', N'SDG_345', CAST(N'2020-12-18T23:43:59.210' AS DateTime), CAST(N'2020-12-19T23:42:00.000' AS DateTime), 1, 5, 1, 1550, 3)
INSERT [dbo].[Order] ([OrderId], [OrderDescription], [OrderBillNo], [OrderConfirmationNo], [OrderUpdatedDate], [OrderNextDeadline], [DeliveryId], [OrderStatusId], [CartId], [OrderAmount], [UserId]) VALUES (7, N'New Order', N'PICKUP_U_6_27304895', N'PICKUP_273023', CAST(N'2021-01-05T12:15:22.687' AS DateTime), CAST(N'2021-01-06T12:15:00.000' AS DateTime), 3, 2, 3, 200, 6)
INSERT [dbo].[Order] ([OrderId], [OrderDescription], [OrderBillNo], [OrderConfirmationNo], [OrderUpdatedDate], [OrderNextDeadline], [DeliveryId], [OrderStatusId], [CartId], [OrderAmount], [UserId]) VALUES (8, N'New Order', N'PICKUP_U_6_20350969', N'PICKUP_203559', CAST(N'2021-01-05T12:04:59.403' AS DateTime), CAST(N'2021-01-05T20:04:59.403' AS DateTime), 4, 1, 3, 200, 6)
INSERT [dbo].[Order] ([OrderId], [OrderDescription], [OrderBillNo], [OrderConfirmationNo], [OrderUpdatedDate], [OrderNextDeadline], [DeliveryId], [OrderStatusId], [CartId], [OrderAmount], [UserId]) VALUES (9, N'New Order', N'PICKUP_U_6_17706773', N'PICKUP_17705', CAST(N'2021-01-05T12:16:05.740' AS DateTime), CAST(N'2021-01-05T20:16:05.740' AS DateTime), 5, 1, 4, 1100, 6)
INSERT [dbo].[Order] ([OrderId], [OrderDescription], [OrderBillNo], [OrderConfirmationNo], [OrderUpdatedDate], [OrderNextDeadline], [DeliveryId], [OrderStatusId], [CartId], [OrderAmount], [UserId]) VALUES (10, N'New Order', N'PICKUP_U_6_98240275', N'PICKUP_982432', CAST(N'2021-01-05T12:34:32.277' AS DateTime), CAST(N'2021-01-05T20:34:32.277' AS DateTime), 9, 1, 5, 200, 6)
INSERT [dbo].[Order] ([OrderId], [OrderDescription], [OrderBillNo], [OrderConfirmationNo], [OrderUpdatedDate], [OrderNextDeadline], [DeliveryId], [OrderStatusId], [CartId], [OrderAmount], [UserId]) VALUES (11, N'New Order', N'23432', N'234dsa', CAST(N'2021-01-05T12:38:18.910' AS DateTime), CAST(N'2021-01-05T20:38:18.910' AS DateTime), 11, 1, 6, 1253.7886143568687, 6)
INSERT [dbo].[Order] ([OrderId], [OrderDescription], [OrderBillNo], [OrderConfirmationNo], [OrderUpdatedDate], [OrderNextDeadline], [DeliveryId], [OrderStatusId], [CartId], [OrderAmount], [UserId]) VALUES (12, N'New Order', N'23432', N'DELIVERY_131520', CAST(N'2021-01-05T12:39:20.730' AS DateTime), CAST(N'2021-01-05T20:39:20.730' AS DateTime), 12, 1, 7, 1253.7886143568687, 6)
INSERT [dbo].[Order] ([OrderId], [OrderDescription], [OrderBillNo], [OrderConfirmationNo], [OrderUpdatedDate], [OrderNextDeadline], [DeliveryId], [OrderStatusId], [CartId], [OrderAmount], [UserId]) VALUES (13, N'New Order', N'DELIVERY_U_6_64930689', N'DELIVERY_649333', CAST(N'2021-01-05T12:41:33.220' AS DateTime), CAST(N'2021-01-05T20:41:33.220' AS DateTime), 13, 1, 8, 1253.7886143568687, 6)
SET IDENTITY_INSERT [dbo].[Order] OFF
SET IDENTITY_INSERT [dbo].[OrderStatus] ON 

INSERT [dbo].[OrderStatus] ([OrderStatusId], [OrderStatusDescription], [OrderStatusOrder]) VALUES (1, N'PLACED/CONFIRMED', 1)
INSERT [dbo].[OrderStatus] ([OrderStatusId], [OrderStatusDescription], [OrderStatusOrder]) VALUES (2, N'ON SALES', 2)
INSERT [dbo].[OrderStatus] ([OrderStatusId], [OrderStatusDescription], [OrderStatusOrder]) VALUES (4, N'READY', 3)
INSERT [dbo].[OrderStatus] ([OrderStatusId], [OrderStatusDescription], [OrderStatusOrder]) VALUES (5, N'COMPLETED', 4)
INSERT [dbo].[OrderStatus] ([OrderStatusId], [OrderStatusDescription], [OrderStatusOrder]) VALUES (6, N'DELIVERED', 5)
SET IDENTITY_INSERT [dbo].[OrderStatus] OFF
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (1, N'User')
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (2, N'Admin')
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (3, N'Sales')
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (4, N'Order Manager')
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (5, N'Delivery')
SET IDENTITY_INSERT [dbo].[Role] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserId], [UserName], [UserEmail], [UserContact], [RoleId], [UserPassword], [FirstName], [LastName], [IsEmailVerified], [ActivationCode]) VALUES (3, N'Pasan Manohara', N'pasanmanohara@gmail.com', N'0715634089', 2, N'827ccb0eea8a706c4c34a16891f84e7b', N'Pasan', N'Manohara', 1, NULL)
INSERT [dbo].[User] ([UserId], [UserName], [UserEmail], [UserContact], [RoleId], [UserPassword], [FirstName], [LastName], [IsEmailVerified], [ActivationCode]) VALUES (4, N'Delivery Boy', N'delivery@gmail.com', N'324325432', 5, N'rewqrr32', N'Delivery', N'Boy', 1, NULL)
INSERT [dbo].[User] ([UserId], [UserName], [UserEmail], [UserContact], [RoleId], [UserPassword], [FirstName], [LastName], [IsEmailVerified], [ActivationCode]) VALUES (5, N'Dm Ravi', N'dm@gmail.com', N'0715634089', 1, N'bsZrZZ+P16Bvv+lmEzz7gJsS0A0ZYwtDh1uKTIAbPEk=', N'Dm', N'Ravi', 1, N'aa641f40-7a68-4a96-a71e-1ff5752a5342')
INSERT [dbo].[User] ([UserId], [UserName], [UserEmail], [UserContact], [RoleId], [UserPassword], [FirstName], [LastName], [IsEmailVerified], [ActivationCode]) VALUES (6, N'Pasan Manohara', N'pasan@gmail.com', N'0715634089', 2, N'WZRHGrsBESr8wYFZ9sx0tPURuZgG2lmzyvWpwXPKz8U=', N'Pasan', N'Manohara', 1, N'a02c3e8a-3b5c-43c4-9467-7274d44ca0ef')
SET IDENTITY_INSERT [dbo].[User] OFF
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_User]
GO
ALTER TABLE [dbo].[Brand]  WITH CHECK ADD  CONSTRAINT [FK_Brand_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
GO
ALTER TABLE [dbo].[Brand] CHECK CONSTRAINT [FK_Brand_Category]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_User]
GO
ALTER TABLE [dbo].[CartItem]  WITH CHECK ADD  CONSTRAINT [FK_CartItem_Cart] FOREIGN KEY([CartId])
REFERENCES [dbo].[Cart] ([CartId])
GO
ALTER TABLE [dbo].[CartItem] CHECK CONSTRAINT [FK_CartItem_Cart]
GO
ALTER TABLE [dbo].[CartItem]  WITH CHECK ADD  CONSTRAINT [FK_CartItem_Item] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Item] ([ItemId])
GO
ALTER TABLE [dbo].[CartItem] CHECK CONSTRAINT [FK_CartItem_Item]
GO
ALTER TABLE [dbo].[Delivery]  WITH CHECK ADD  CONSTRAINT [FK_Delivery_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Delivery] CHECK CONSTRAINT [FK_Delivery_User]
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [FK_Image_Item] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Item] ([ItemId])
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [FK_Image_Item]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Brand] FOREIGN KEY([BrandId])
REFERENCES [dbo].[Brand] ([BrandId])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Brand]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Cart] FOREIGN KEY([CartId])
REFERENCES [dbo].[Cart] ([CartId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Cart]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Delivery] FOREIGN KEY([CartId])
REFERENCES [dbo].[Cart] ([CartId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Delivery]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_OrderStatus] FOREIGN KEY([OrderStatusId])
REFERENCES [dbo].[OrderStatus] ([OrderStatusId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_OrderStatus]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_User]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Wallet] FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([WalletId])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Wallet]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[Wallet]  WITH CHECK ADD  CONSTRAINT [FK_Wallet_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Wallet] CHECK CONSTRAINT [FK_Wallet_User]
GO
USE [master]
GO
ALTER DATABASE [FreshChoice] SET  READ_WRITE 
GO

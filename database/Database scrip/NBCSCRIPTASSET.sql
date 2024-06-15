USE [master]
GO
/****** Object:  Database [nbc_asset_management]    Script Date: 2024/06/11 3:20:38 PM ******/
CREATE DATABASE [nbc_asset_management]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'nbc_asset_management', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\nbc_asset_management.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'nbc_asset_management_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\nbc_asset_management_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [nbc_asset_management] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [nbc_asset_management].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [nbc_asset_management] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [nbc_asset_management] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [nbc_asset_management] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [nbc_asset_management] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [nbc_asset_management] SET ARITHABORT OFF 
GO
ALTER DATABASE [nbc_asset_management] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [nbc_asset_management] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [nbc_asset_management] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [nbc_asset_management] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [nbc_asset_management] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [nbc_asset_management] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [nbc_asset_management] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [nbc_asset_management] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [nbc_asset_management] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [nbc_asset_management] SET  ENABLE_BROKER 
GO
ALTER DATABASE [nbc_asset_management] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [nbc_asset_management] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [nbc_asset_management] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [nbc_asset_management] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [nbc_asset_management] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [nbc_asset_management] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [nbc_asset_management] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [nbc_asset_management] SET RECOVERY FULL 
GO
ALTER DATABASE [nbc_asset_management] SET  MULTI_USER 
GO
ALTER DATABASE [nbc_asset_management] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [nbc_asset_management] SET DB_CHAINING OFF 
GO
ALTER DATABASE [nbc_asset_management] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [nbc_asset_management] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [nbc_asset_management] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [nbc_asset_management] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [nbc_asset_management] SET QUERY_STORE = OFF
GO
USE [nbc_asset_management]
GO
/****** Object:  User [ITAssetManagement]    Script Date: 2024/06/11 3:20:42 PM ******/
CREATE USER [ITAssetManagement] FOR LOGIN [ITAssetManagement] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[assigned_desktops]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[assigned_desktops](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[desktop_monitor_id] [int] NOT NULL,
	[user_assigned_id] [int] NOT NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__assigned__3213E83FF18A11E6] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[desktop_monitor_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[assigned_laptops]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[assigned_laptops](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[laptop_id] [int] NOT NULL,
	[user_assigned_id] [int] NOT NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__assigned__3213E83FC9CE8C72] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[assigned_sdwans]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[assigned_sdwans](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sdwan_id] [int] NOT NULL,
	[user_assigned_id] [int] NOT NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__assigned__3213E83FE8F3CB21] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[desktop_cpus]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[desktop_cpus](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[brand_name] [varchar](255) NOT NULL,
	[model] [varchar](100) NOT NULL,
	[cpu_serial_number] [varchar](50) NOT NULL,
	[cpu_tag_number] [varchar](50) NOT NULL,
	[comments] [text] NULL,
	[attachment] [varbinary](1) NULL,
	[status_id] [int] NOT NULL,
	[user_assigned_id] [int] NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__desktop___3213E83F4E8C4DAC] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[desktop_monitors]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[desktop_monitors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[brand_name] [varchar](255) NOT NULL,
	[model] [varchar](100) NOT NULL,
	[monitor_serial_number] [varchar](50) NOT NULL,
	[monitor_tag_number] [varchar](50) NOT NULL,
	[comments] [text] NULL,
	[attachment] [varbinary](1) NULL,
	[status_id] [int] NOT NULL,
	[user_assigned_id] [int] NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__desktop___3213E83F08E9C775] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[device_status]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[device_status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[user_created] [int] NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__device_s__3213E83F9DB36AC3] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[firewalls]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[firewalls](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tag_number] [varchar](255) NOT NULL,
	[serial_number] [varchar](255) NOT NULL,
	[model] [varchar](255) NOT NULL,
	[comments] [varchar](255) NULL,
	[status_id] [int] NOT NULL,
	[attachment] [varbinary](max) NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__firewall__3213E83F39459F39] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[joined_desktops_monitors]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[joined_desktops_monitors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[desktop_cpu_id] [int] NULL,
	[desktop_monitor_id] [int] NULL,
	[user_assigned_id] [int] NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__joined_d__3213E83FEEF31C9C] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[laptops]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[laptops](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[brand_name] [varchar](255) NOT NULL,
	[serial_number] [varchar](50) NOT NULL,
	[tag_number] [varchar](50) NOT NULL,
	[model] [varchar](100) NOT NULL,
	[comments] [text] NULL,
	[attachment] [varbinary](1) NULL,
	[type] [char](1) NOT NULL,
	[device_status_id] [int] NOT NULL,
	[user_assigned_id] [int] NOT NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__laptops__3213E83F4FFA3A58] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[loaned_laptops]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[loaned_laptops](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[loaned_laptop_id] [int] NOT NULL,
	[user_loaned_id] [int] NOT NULL,
	[descriptions] [varchar](255) NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__loaned_l__3213E83FB38A0B23] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[loaned_sdwans]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[loaned_sdwans](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[loaned_sdwans_id] [int] NOT NULL,
	[user_loaned_id] [int] NOT NULL,
	[descriptions] [varchar](255) NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__loaned_s__3213E83F25F082FD] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[router_mtc]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[router_mtc](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tag_number] [varchar](255) NOT NULL,
	[serial_number] [varchar](255) NOT NULL,
	[model] [varchar](255) NOT NULL,
	[comments] [varchar](255) NULL,
	[status_id] [int] NOT NULL,
	[attachment] [varbinary](max) NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__router_m__3213E83FE7FB514F] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sdwan_laptops]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sdwan_laptops](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[brand_name] [varchar](255) NOT NULL,
	[serial_number] [varchar](255) NOT NULL,
	[tag_number] [varchar](255) NOT NULL,
	[model] [varchar](255) NOT NULL,
	[comments] [varchar](255) NULL,
	[attachment] [varbinary](max) NULL,
	[status_id] [int] NOT NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__sdwan_la__3213E83FF663A849] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sdwans]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sdwans](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[firewall_id] [int] NULL,
	[sdwanlaptop_id] [int] NULL,
	[router_id] [int] NULL,
	[type] [bit] NOT NULL,
	[description] [varchar](255) NULL,
	[user_assigned_id] [int] NOT NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__sdwans__3213E83FED85DFE1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[trailing_assigned_desktops]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trailing_assigned_desktops](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[desktop_monitor_id] [int] NOT NULL,
	[user_assigned_id] [int] NOT NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__trailing__3213E83F3FB96F2C] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[trailing_assigned_laptops]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trailing_assigned_laptops](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[laptop_id] [int] NOT NULL,
	[user_assigned_id] [int] NOT NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__trailing__3213E83FE469A82D] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[trailing_assigned_sdwans]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trailing_assigned_sdwans](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sdwan_id] [int] NOT NULL,
	[user_assigned_id] [int] NOT NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__trailing__3213E83F7C639F68] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[trailing_device_status]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trailing_device_status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[user_created] [int] NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__trailing__3213E83F6291863B] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[trailing_joined_desktops_monitors]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trailing_joined_desktops_monitors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[desktop_cpu_id] [int] NOT NULL,
	[desktop_monitor_id] [int] NOT NULL,
	[user_assigned_id] [int] NOT NULL,
	[user_created] [int] NOT NULL,
	[user_update] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[trailing_loaned_laptops]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trailing_loaned_laptops](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[loaned_laptop_id] [int] NOT NULL,
	[user_loaned_id] [int] NOT NULL,
	[descriptions] [varchar](255) NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[trailing_loaned_sdwans]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trailing_loaned_sdwans](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[loaned_sdwans_id] [int] NOT NULL,
	[user_loaned_id] [int] NOT NULL,
	[descriptions] [varchar](255) NULL,
	[user_created] [int] NOT NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NOT NULL,
	[date_updated] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[fullname] [varchar](250) NOT NULL,
	[username] [varchar](250) NOT NULL,
	[email] [varchar](255) NOT NULL,
	[user_created] [int] NULL,
	[user_updated] [int] NULL,
	[date_created] [datetime] NULL,
	[date_updated] [datetime] NULL,
 CONSTRAINT [PK__users__3213E83F20257F84] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[assigned_desktops] ADD  CONSTRAINT [DF__assigned___date___44FF419A]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[assigned_laptops] ADD  CONSTRAINT [DF__assigned___date___33D4B598]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[assigned_sdwans] ADD  CONSTRAINT [DF__assigned___date___60A75C0F]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[desktop_cpus] ADD  CONSTRAINT [DF__desktop_c__date___3C69FB99]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[desktop_monitors] ADD  CONSTRAINT [DF__desktop_m__date___38996AB5]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[device_status] ADD  CONSTRAINT [DF__device_st__date___276EDEB3]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[firewalls] ADD  CONSTRAINT [DF__firewalls__date___52593CB8]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[joined_desktops_monitors] ADD  CONSTRAINT [DF__joined_de__date___403A8C7D]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[laptops] ADD  CONSTRAINT [DF__laptops__date_cr__2A4B4B5E]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[loaned_laptops] ADD  CONSTRAINT [DF__loaned_la__date___2F10007B]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[loaned_sdwans] ADD  CONSTRAINT [DF__loaned_sd__date___5BE2A6F2]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[router_mtc] ADD  CONSTRAINT [DF__router_mt__date___571DF1D5]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[sdwan_laptops] ADD  CONSTRAINT [DF__sdwan_lap__date___4D94879B]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[sdwans] ADD  CONSTRAINT [DF__sdwans__date_cre__49C3F6B7]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[trailing_assigned_desktops] ADD  CONSTRAINT [DF__trailing___date___68536ACF]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[trailing_assigned_laptops] ADD  CONSTRAINT [DF__trailing___date___6F00685E]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[trailing_assigned_sdwans] ADD  CONSTRAINT [DF__trailing___date___7889D298]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[trailing_device_status] ADD  CONSTRAINT [DF__trailing___date___7F36D027]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[trailing_joined_desktops_monitors] ADD  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[trailing_loaned_laptops] ADD  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[trailing_loaned_sdwans] ADD  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[users] ADD  CONSTRAINT [DF__users__date_crea__24927208]  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[assigned_desktops]  WITH NOCHECK ADD  CONSTRAINT [FK__assigned___devic__45F365D3] FOREIGN KEY([desktop_monitor_id])
REFERENCES [dbo].[joined_desktops_monitors] ([id])
GO
ALTER TABLE [dbo].[assigned_desktops] CHECK CONSTRAINT [FK__assigned___devic__45F365D3]
GO
ALTER TABLE [dbo].[assigned_desktops]  WITH CHECK ADD  CONSTRAINT [FK__assigned___user___46E78A0C] FOREIGN KEY([user_assigned_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[assigned_desktops] CHECK CONSTRAINT [FK__assigned___user___46E78A0C]
GO
ALTER TABLE [dbo].[assigned_desktops]  WITH CHECK ADD  CONSTRAINT [FK_assigned_desktops_users] FOREIGN KEY([user_updated])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[assigned_desktops] CHECK CONSTRAINT [FK_assigned_desktops_users]
GO
ALTER TABLE [dbo].[assigned_desktops]  WITH CHECK ADD  CONSTRAINT [FK_assigned_desktops_users1] FOREIGN KEY([user_created])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[assigned_desktops] CHECK CONSTRAINT [FK_assigned_desktops_users1]
GO
ALTER TABLE [dbo].[assigned_laptops]  WITH CHECK ADD  CONSTRAINT [FK__assigned___devic__34C8D9D1] FOREIGN KEY([laptop_id])
REFERENCES [dbo].[laptops] ([id])
GO
ALTER TABLE [dbo].[assigned_laptops] CHECK CONSTRAINT [FK__assigned___devic__34C8D9D1]
GO
ALTER TABLE [dbo].[assigned_laptops]  WITH CHECK ADD  CONSTRAINT [FK__assigned___user___35BCFE0A] FOREIGN KEY([user_assigned_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[assigned_laptops] CHECK CONSTRAINT [FK__assigned___user___35BCFE0A]
GO
ALTER TABLE [dbo].[assigned_laptops]  WITH CHECK ADD  CONSTRAINT [FK_assigned_laptops_assigned_laptops] FOREIGN KEY([id])
REFERENCES [dbo].[assigned_laptops] ([id])
GO
ALTER TABLE [dbo].[assigned_laptops] CHECK CONSTRAINT [FK_assigned_laptops_assigned_laptops]
GO
ALTER TABLE [dbo].[assigned_laptops]  WITH CHECK ADD  CONSTRAINT [FK_assigned_laptops_users] FOREIGN KEY([user_created])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[assigned_laptops] CHECK CONSTRAINT [FK_assigned_laptops_users]
GO
ALTER TABLE [dbo].[assigned_laptops]  WITH CHECK ADD  CONSTRAINT [FK_assigned_laptops_users1] FOREIGN KEY([user_updated])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[assigned_laptops] CHECK CONSTRAINT [FK_assigned_laptops_users1]
GO
ALTER TABLE [dbo].[assigned_sdwans]  WITH CHECK ADD  CONSTRAINT [FK__assigned___date___619B8048] FOREIGN KEY([sdwan_id])
REFERENCES [dbo].[sdwans] ([id])
GO
ALTER TABLE [dbo].[assigned_sdwans] CHECK CONSTRAINT [FK__assigned___date___619B8048]
GO
ALTER TABLE [dbo].[assigned_sdwans]  WITH CHECK ADD  CONSTRAINT [FK__assigned___user___628FA481] FOREIGN KEY([user_assigned_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[assigned_sdwans] CHECK CONSTRAINT [FK__assigned___user___628FA481]
GO
ALTER TABLE [dbo].[assigned_sdwans]  WITH CHECK ADD  CONSTRAINT [FK_assigned_sdwans_users] FOREIGN KEY([user_created])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[assigned_sdwans] CHECK CONSTRAINT [FK_assigned_sdwans_users]
GO
ALTER TABLE [dbo].[assigned_sdwans]  WITH CHECK ADD  CONSTRAINT [FK_assigned_sdwans_users1] FOREIGN KEY([user_updated])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[assigned_sdwans] CHECK CONSTRAINT [FK_assigned_sdwans_users1]
GO
ALTER TABLE [dbo].[desktop_cpus]  WITH CHECK ADD  CONSTRAINT [FK__desktop_c__statu__3D5E1FD2] FOREIGN KEY([status_id])
REFERENCES [dbo].[device_status] ([id])
GO
ALTER TABLE [dbo].[desktop_cpus] CHECK CONSTRAINT [FK__desktop_c__statu__3D5E1FD2]
GO
ALTER TABLE [dbo].[desktop_cpus]  WITH CHECK ADD  CONSTRAINT [FK_desktop_cpus_desktop_cpus] FOREIGN KEY([user_updated])
REFERENCES [dbo].[desktop_cpus] ([id])
GO
ALTER TABLE [dbo].[desktop_cpus] CHECK CONSTRAINT [FK_desktop_cpus_desktop_cpus]
GO
ALTER TABLE [dbo].[desktop_cpus]  WITH CHECK ADD  CONSTRAINT [FK_desktop_cpus_desktop_cpus1] FOREIGN KEY([id])
REFERENCES [dbo].[desktop_cpus] ([id])
GO
ALTER TABLE [dbo].[desktop_cpus] CHECK CONSTRAINT [FK_desktop_cpus_desktop_cpus1]
GO
ALTER TABLE [dbo].[desktop_cpus]  WITH CHECK ADD  CONSTRAINT [FK_desktop_cpus_users] FOREIGN KEY([user_created])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[desktop_cpus] CHECK CONSTRAINT [FK_desktop_cpus_users]
GO
ALTER TABLE [dbo].[device_status]  WITH CHECK ADD  CONSTRAINT [FK_device_status_device_status] FOREIGN KEY([user_updated])
REFERENCES [dbo].[device_status] ([id])
GO
ALTER TABLE [dbo].[device_status] CHECK CONSTRAINT [FK_device_status_device_status]
GO
ALTER TABLE [dbo].[firewalls]  WITH CHECK ADD  CONSTRAINT [FK_firewalls_users] FOREIGN KEY([user_created])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[firewalls] CHECK CONSTRAINT [FK_firewalls_users]
GO
ALTER TABLE [dbo].[firewalls]  WITH CHECK ADD  CONSTRAINT [FK_firewalls_users1] FOREIGN KEY([user_updated])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[firewalls] CHECK CONSTRAINT [FK_firewalls_users1]
GO
ALTER TABLE [dbo].[joined_desktops_monitors]  WITH NOCHECK ADD  CONSTRAINT [FK__joined_de__deskt__412EB0B6] FOREIGN KEY([desktop_cpu_id])
REFERENCES [dbo].[desktop_cpus] ([id])
GO
ALTER TABLE [dbo].[joined_desktops_monitors] NOCHECK CONSTRAINT [FK__joined_de__deskt__412EB0B6]
GO
ALTER TABLE [dbo].[joined_desktops_monitors]  WITH NOCHECK ADD  CONSTRAINT [FK__joined_de__deskt__4222D4EF] FOREIGN KEY([desktop_monitor_id])
REFERENCES [dbo].[desktop_monitors] ([id])
GO
ALTER TABLE [dbo].[joined_desktops_monitors] NOCHECK CONSTRAINT [FK__joined_de__deskt__4222D4EF]
GO
ALTER TABLE [dbo].[joined_desktops_monitors]  WITH NOCHECK ADD  CONSTRAINT [FK_joined_desktops_monitors_users] FOREIGN KEY([user_created])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[joined_desktops_monitors] CHECK CONSTRAINT [FK_joined_desktops_monitors_users]
GO
ALTER TABLE [dbo].[joined_desktops_monitors]  WITH NOCHECK ADD  CONSTRAINT [FK_joined_desktops_monitors_users1] FOREIGN KEY([user_updated])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[joined_desktops_monitors] CHECK CONSTRAINT [FK_joined_desktops_monitors_users1]
GO
ALTER TABLE [dbo].[laptops]  WITH CHECK ADD  CONSTRAINT [FK__laptops__status___2B3F6F97] FOREIGN KEY([device_status_id])
REFERENCES [dbo].[device_status] ([id])
GO
ALTER TABLE [dbo].[laptops] CHECK CONSTRAINT [FK__laptops__status___2B3F6F97]
GO
ALTER TABLE [dbo].[laptops]  WITH CHECK ADD  CONSTRAINT [FK__laptops__user_id__2C3393D0] FOREIGN KEY([user_assigned_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[laptops] CHECK CONSTRAINT [FK__laptops__user_id__2C3393D0]
GO
ALTER TABLE [dbo].[laptops]  WITH CHECK ADD  CONSTRAINT [FK_laptops_users] FOREIGN KEY([user_created])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[laptops] CHECK CONSTRAINT [FK_laptops_users]
GO
ALTER TABLE [dbo].[laptops]  WITH CHECK ADD  CONSTRAINT [FK_laptops_users1] FOREIGN KEY([user_updated])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[laptops] CHECK CONSTRAINT [FK_laptops_users1]
GO
ALTER TABLE [dbo].[loaned_laptops]  WITH CHECK ADD  CONSTRAINT [FK__loaned_la__devic__300424B4] FOREIGN KEY([loaned_laptop_id])
REFERENCES [dbo].[laptops] ([id])
GO
ALTER TABLE [dbo].[loaned_laptops] CHECK CONSTRAINT [FK__loaned_la__devic__300424B4]
GO
ALTER TABLE [dbo].[loaned_laptops]  WITH CHECK ADD  CONSTRAINT [FK__loaned_la__user___30F848ED] FOREIGN KEY([user_loaned_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[loaned_laptops] CHECK CONSTRAINT [FK__loaned_la__user___30F848ED]
GO
ALTER TABLE [dbo].[loaned_laptops]  WITH CHECK ADD  CONSTRAINT [FK_loaned_laptops_users] FOREIGN KEY([user_created])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[loaned_laptops] CHECK CONSTRAINT [FK_loaned_laptops_users]
GO
ALTER TABLE [dbo].[loaned_laptops]  WITH CHECK ADD  CONSTRAINT [FK_loaned_laptops_users1] FOREIGN KEY([user_updated])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[loaned_laptops] CHECK CONSTRAINT [FK_loaned_laptops_users1]
GO
ALTER TABLE [dbo].[loaned_sdwans]  WITH CHECK ADD  CONSTRAINT [FK__loaned_sd__devic__5CD6CB2B] FOREIGN KEY([loaned_sdwans_id])
REFERENCES [dbo].[sdwans] ([id])
GO
ALTER TABLE [dbo].[loaned_sdwans] CHECK CONSTRAINT [FK__loaned_sd__devic__5CD6CB2B]
GO
ALTER TABLE [dbo].[loaned_sdwans]  WITH CHECK ADD  CONSTRAINT [FK__loaned_sd__user___5DCAEF64] FOREIGN KEY([user_loaned_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[loaned_sdwans] CHECK CONSTRAINT [FK__loaned_sd__user___5DCAEF64]
GO
ALTER TABLE [dbo].[loaned_sdwans]  WITH CHECK ADD  CONSTRAINT [FK_loaned_sdwans_users] FOREIGN KEY([user_created])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[loaned_sdwans] CHECK CONSTRAINT [FK_loaned_sdwans_users]
GO
ALTER TABLE [dbo].[loaned_sdwans]  WITH CHECK ADD  CONSTRAINT [FK_loaned_sdwans_users1] FOREIGN KEY([user_updated])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[loaned_sdwans] CHECK CONSTRAINT [FK_loaned_sdwans_users1]
GO
ALTER TABLE [dbo].[router_mtc]  WITH CHECK ADD  CONSTRAINT [FK__router_mt__statu__5812160E] FOREIGN KEY([status_id])
REFERENCES [dbo].[device_status] ([id])
GO
ALTER TABLE [dbo].[router_mtc] CHECK CONSTRAINT [FK__router_mt__statu__5812160E]
GO
ALTER TABLE [dbo].[router_mtc]  WITH CHECK ADD  CONSTRAINT [FK_router_mtc_users] FOREIGN KEY([user_created])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[router_mtc] CHECK CONSTRAINT [FK_router_mtc_users]
GO
ALTER TABLE [dbo].[router_mtc]  WITH CHECK ADD  CONSTRAINT [FK_router_mtc_users1] FOREIGN KEY([user_updated])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[router_mtc] CHECK CONSTRAINT [FK_router_mtc_users1]
GO
ALTER TABLE [dbo].[sdwan_laptops]  WITH CHECK ADD  CONSTRAINT [FK__sdwan_lap__statu__4E88ABD4] FOREIGN KEY([status_id])
REFERENCES [dbo].[device_status] ([id])
GO
ALTER TABLE [dbo].[sdwan_laptops] CHECK CONSTRAINT [FK__sdwan_lap__statu__4E88ABD4]
GO
ALTER TABLE [dbo].[sdwan_laptops]  WITH CHECK ADD  CONSTRAINT [FK_sdwan_laptops_users] FOREIGN KEY([user_created])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[sdwan_laptops] CHECK CONSTRAINT [FK_sdwan_laptops_users]
GO
ALTER TABLE [dbo].[sdwan_laptops]  WITH CHECK ADD  CONSTRAINT [FK_sdwan_laptops_users1] FOREIGN KEY([user_updated])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[sdwan_laptops] CHECK CONSTRAINT [FK_sdwan_laptops_users1]
GO
/****** Object:  Trigger [dbo].[trigger_assigned_desktops_DateCreated]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_assigned_desktops_DateCreated]
ON [dbo].[assigned_desktops]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE assigned_desktops
    SET date_created = GETDATE()
    FROM assigned_desktops
    INNER JOIN inserted ON assigned_desktops.id = inserted.id
    WHERE assigned_desktops.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[assigned_desktops] ENABLE TRIGGER [trigger_assigned_desktops_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_assigned_desktops_DateUpdated]    Script Date: 2024/06/11 3:20:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_assigned_desktops_DateUpdated]
ON [dbo].[assigned_desktops]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE assigned_desktops
    SET date_updated = GETDATE()
    FROM assigned_desktops
    INNER JOIN inserted ON assigned_desktops.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[assigned_desktops] ENABLE TRIGGER [trigger_assigned_desktops_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_delete_assigned_desktop]    Script Date: 2024/06/11 3:20:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--- ASSIGNED DESKTOP
CREATE TRIGGER [dbo].[trigger_delete_assigned_desktop]
ON [dbo].[assigned_desktops]
AFTER DELETE
AS
BEGIN
    -- Insert deleted records into trail_assigned_laptops
    INSERT INTO trailing_assigned_desktops (desktop_monitor_id, user_assigned_id, user_created, date_created, date_updated)
    SELECT desktop_monitor_id, user_assigned_id, user_created, date_created, date_updated
    FROM deleted;
END;
GO
ALTER TABLE [dbo].[assigned_desktops] ENABLE TRIGGER [trigger_delete_assigned_desktop]
GO
/****** Object:  Trigger [dbo].[trigger_assigned_laptops_DateCreated]    Script Date: 2024/06/11 3:20:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ASSIGNED SDWAN TRIGGERS START
CREATE TRIGGER [dbo].[trigger_assigned_laptops_DateCreated]
ON [dbo].[assigned_laptops]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE assigned_laptops
    SET date_created = GETDATE()
    FROM assigned_laptops
    INNER JOIN inserted ON assigned_laptops.id = inserted.id
    WHERE assigned_laptops.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[assigned_laptops] ENABLE TRIGGER [trigger_assigned_laptops_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_assigned_laptops_DateUpdated]    Script Date: 2024/06/11 3:20:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_assigned_laptops_DateUpdated]
ON [dbo].[assigned_laptops] 
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE assigned_laptops 
    SET date_updated = GETDATE()
    FROM assigned_laptops 
    INNER JOIN inserted ON assigned_laptops.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[assigned_laptops] ENABLE TRIGGER [trigger_assigned_laptops_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_delete_assigned_laptops]    Script Date: 2024/06/11 3:20:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--USE [nbc_asset_management]
--GO
--/****** Object:  Trigger [dbo].[trigger_delete_assigned_desktop]    Script Date: 5/16/2024 10:47:22 AM ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
----- ASSIGNED DESKTOP
--ALTER TRIGGER [dbo].[trigger_delete_assigned_desktop]
--ON [dbo].[assigned_desktops]
--AFTER DELETE
--AS
--BEGIN
--    -- Insert deleted records into trail_assigned_laptops
--    INSERT INTO trailing_assigned_desktops (desktop_monitor_id, user_assigned_id, user_created, date_created, date_updated)
--    SELECT desktop_monitor_id, user_assigned_id, user_created, date_created, date_updated
--    FROM deleted;
--END;


--- ASSIGNED LAPTOPS
CREATE TRIGGER [dbo].[trigger_delete_assigned_laptops]
ON [dbo].[assigned_laptops]
AFTER DELETE
AS
BEGIN
    -- Insert deleted records into trail_assigned_laptops
    INSERT INTO trailing_assigned_laptops (laptop_id, user_assigned_id, user_created, date_created, date_updated)
    SELECT laptop_id, user_assigned_id, user_created, date_created, date_updated
    FROM deleted;
END;
GO
ALTER TABLE [dbo].[assigned_laptops] ENABLE TRIGGER [trigger_delete_assigned_laptops]
GO
/****** Object:  Trigger [dbo].[trigger_assigned_sdwans_DateCreated]    Script Date: 2024/06/11 3:20:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ASSIGNED SDWAN TRIGGERS START
CREATE TRIGGER [dbo].[trigger_assigned_sdwans_DateCreated]
ON [dbo].[assigned_sdwans] 
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE assigned_sdwans 
    SET date_created = GETDATE()
    FROM assigned_sdwans 
    INNER JOIN inserted ON assigned_sdwans.id = inserted.id
    WHERE assigned_sdwans.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[assigned_sdwans] ENABLE TRIGGER [trigger_assigned_sdwans_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_assigned_sdwans_DateUpdated]    Script Date: 2024/06/11 3:20:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_assigned_sdwans_DateUpdated]
ON [dbo].[assigned_sdwans] 
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE assigned_sdwans 
    SET date_updated = GETDATE()
    FROM assigned_sdwans 
    INNER JOIN inserted ON assigned_sdwans.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[assigned_sdwans] ENABLE TRIGGER [trigger_assigned_sdwans_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_delete_assigned_sdwans]    Script Date: 2024/06/11 3:20:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ASSIGNED SDWAN START

CREATE TRIGGER [dbo].[trigger_delete_assigned_sdwans]
ON [dbo].[assigned_sdwans]
AFTER DELETE
AS
BEGIN
 
    INSERT INTO trailing_assigned_sdwans (sdwan_id, user_assigned_id, user_created,user_updated, date_created, date_updated)
    SELECT sdwan_id, user_assigned_id, user_created,user_updated, date_created, date_updated
    FROM deleted;
END;
GO
ALTER TABLE [dbo].[assigned_sdwans] ENABLE TRIGGER [trigger_delete_assigned_sdwans]
GO
/****** Object:  Trigger [dbo].[trigger_desktop_cpus_DateCreated]    Script Date: 2024/06/11 3:20:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  DESKTOP CPU  TRIGGERS START
CREATE TRIGGER [dbo].[trigger_desktop_cpus_DateCreated]
ON [dbo].[desktop_cpus]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE desktop_cpus
    SET date_created = GETDATE()
    FROM desktop_cpus
    INNER JOIN inserted ON desktop_cpus.id = inserted.id
    WHERE desktop_cpus.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[desktop_cpus] ENABLE TRIGGER [trigger_desktop_cpus_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_desktop_cpus_DateUpdated]    Script Date: 2024/06/11 3:20:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_desktop_cpus_DateUpdated]
ON [dbo].[desktop_cpus] 
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE desktop_cpus 
    SET date_updated = GETDATE()
    FROM desktop_cpus 
    INNER JOIN inserted ON desktop_cpus.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[desktop_cpus] ENABLE TRIGGER [trigger_desktop_cpus_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_desktop_monitors_DateCreated]    Script Date: 2024/06/11 3:20:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  DESKTOP MONITOR TRIGGERS START
CREATE TRIGGER [dbo].[trigger_desktop_monitors_DateCreated]
ON [dbo].[desktop_monitors]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE desktop_monitors
    SET date_created = GETDATE()
    FROM desktop_monitors
    INNER JOIN inserted ON desktop_monitors.id = inserted.id
    WHERE desktop_monitors.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[desktop_monitors] ENABLE TRIGGER [trigger_desktop_monitors_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_desktop_monitors_DateUpdated]    Script Date: 2024/06/11 3:20:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_desktop_monitors_DateUpdated]
ON [dbo].[desktop_monitors] 
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE desktop_monitors 
    SET date_updated = GETDATE()
    FROM desktop_monitors 
    INNER JOIN inserted ON desktop_monitors.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[desktop_monitors] ENABLE TRIGGER [trigger_desktop_monitors_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_delete_device]    Script Date: 2024/06/11 3:20:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--- DEVICE STATUS
CREATE TRIGGER [dbo].[trigger_delete_device]
ON [dbo].[device_status]
AFTER DELETE
AS
BEGIN
    -- Insert deleted records into trail_assigned_laptops
    INSERT INTO trailing_device_status (name, user_created, date_created, date_updated)
    SELECT name, user_created, date_created, date_updated
    FROM deleted;
END;
GO
ALTER TABLE [dbo].[device_status] ENABLE TRIGGER [trigger_delete_device]
GO
/****** Object:  Trigger [dbo].[trigger_device_status_DateCreated]    Script Date: 2024/06/11 3:20:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- TRAILING DEVICE STATUS TRIGGERS START

CREATE TRIGGER [dbo].[trigger_device_status_DateCreated]
ON [dbo].[device_status]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE device_status
    SET date_created = GETDATE()
    FROM device_status 
    INNER JOIN inserted ON device_status.id = inserted.id
    WHERE device_status.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[device_status] ENABLE TRIGGER [trigger_device_status_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_device_status_DateUpdated]    Script Date: 2024/06/11 3:20:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_device_status_DateUpdated]
ON [dbo].[device_status] 
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE device_status 
    SET date_updated = GETDATE()
    FROM device_status 
    INNER JOIN inserted ON device_status.id = inserted.id;
END;
-- TRAILING DEVICE STATUS TRIGGERS END
GO
ALTER TABLE [dbo].[device_status] ENABLE TRIGGER [trigger_device_status_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_firewalls_DateCreated]    Script Date: 2024/06/11 3:20:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_firewalls_DateCreated]
ON [dbo].[firewalls] 
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE firewalls 
    SET date_created = GETDATE()
    FROM firewalls 
    INNER JOIN inserted ON firewalls .id = inserted.id
    WHERE firewalls .date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[firewalls] ENABLE TRIGGER [trigger_firewalls_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_firewalls_DateUpdated]    Script Date: 2024/06/11 3:20:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- trigger



-- FIREWALL TRIGGERS START
CREATE TRIGGER [dbo].[trigger_firewalls_DateUpdated]
ON [dbo].[firewalls] 
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE firewalls 
    SET date_updated = GETDATE()
    FROM firewalls 
    INNER JOIN inserted ON firewalls.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[firewalls] ENABLE TRIGGER [trigger_firewalls_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_delete_joined_desktops_monitors]    Script Date: 2024/06/11 3:20:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--CREATE TABLE trailing_joined_desktops_monitors (
--  id INT IDENTITY(1,1)  PRIMARY KEY,
--  desktop_cpu_id int NOT NULL, 
--  desktop_monitor_id int NOT NULL,
-- user_assigned_id INT NOT NULL,
-- user_created INT NOT NULL,
-- user_update INT NULL,
-- date_created DATETIME DEFAULT GETDATE() NOT NULL,
--  date_updated DATETIME NULL,
--);


-- JOIN MONITOR AND DESKTOP START
CREATE TRIGGER [dbo].[trigger_delete_joined_desktops_monitors]
ON [dbo].[joined_desktops_monitors]
AFTER DELETE
AS
BEGIN
    
    INSERT INTO trailing_joined_desktops_monitors (desktop_cpu_id, desktop_monitor_id, user_assigned_id, user_created, date_created, date_updated)
    SELECT desktop_cpu_id, desktop_monitor_id, user_assigned_id, user_created, date_created, date_updated
    FROM deleted;
  
END;
GO
ALTER TABLE [dbo].[joined_desktops_monitors] ENABLE TRIGGER [trigger_delete_joined_desktops_monitors]
GO
/****** Object:  Trigger [dbo].[trigger_joined_desktops_monitors_DateCreated]    Script Date: 2024/06/11 3:20:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  JOINED DESKTOP AND MONITOR  TRIGGERS START
CREATE TRIGGER [dbo].[trigger_joined_desktops_monitors_DateCreated]
ON [dbo].[joined_desktops_monitors]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE joined_desktops_monitors
    SET date_created = GETDATE()
    FROM joined_desktops_monitors
    INNER JOIN inserted ON joined_desktops_monitors.id = inserted.id
    WHERE joined_desktops_monitors.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[joined_desktops_monitors] ENABLE TRIGGER [trigger_joined_desktops_monitors_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_joined_desktops_monitors_DateUpdated]    Script Date: 2024/06/11 3:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_joined_desktops_monitors_DateUpdated]
ON [dbo].[joined_desktops_monitors] 
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE joined_desktops_monitors 
    SET date_updated = GETDATE()
    FROM joined_desktops_monitors 
    INNER JOIN inserted ON joined_desktops_monitors.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[joined_desktops_monitors] ENABLE TRIGGER [trigger_joined_desktops_monitors_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_laptops_DateCreated]    Script Date: 2024/06/11 3:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  LAPTOPS TRIGGERS START
CREATE TRIGGER [dbo].[trigger_laptops_DateCreated]
ON [dbo].[laptops]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE laptops
    SET date_created = GETDATE()
    FROM laptops
    INNER JOIN inserted ON laptops.id = inserted.id
    WHERE laptops.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[laptops] ENABLE TRIGGER [trigger_laptops_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_laptops_DateUpdated]    Script Date: 2024/06/11 3:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_laptops_DateUpdated]
ON [dbo].[laptops] 
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE laptops 
    SET date_updated = GETDATE()
    FROM laptops 
    INNER JOIN inserted ON laptops.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[laptops] ENABLE TRIGGER [trigger_laptops_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_delete_loaned_laptops]    Script Date: 2024/06/11 3:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- LOANED LAPTOPS START
CREATE TRIGGER [dbo].[trigger_delete_loaned_laptops]
ON [dbo].[loaned_laptops]
AFTER DELETE
AS
BEGIN
 
    INSERT INTO trailing_loaned_laptops (loaned_laptop_id, user_loaned_id, descriptions, user_created, date_created, date_updated)
    SELECT loaned_laptop_id, user_loaned_id, descriptions, user_created, date_created, date_updated
    FROM deleted;
END;
GO
ALTER TABLE [dbo].[loaned_laptops] ENABLE TRIGGER [trigger_delete_loaned_laptops]
GO
/****** Object:  Trigger [dbo].[trigger_loaned_laptops_DateCreated]    Script Date: 2024/06/11 3:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  LOANED LAPTOPS TRIGGERS START
CREATE TRIGGER [dbo].[trigger_loaned_laptops_DateCreated]
ON [dbo].[loaned_laptops]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE loaned_laptops
    SET date_created = GETDATE()
    FROM loaned_laptops
    INNER JOIN inserted ON loaned_laptops.id = inserted.id
    WHERE loaned_laptops.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[loaned_laptops] ENABLE TRIGGER [trigger_loaned_laptops_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_loaned_laptops_DateUpdated]    Script Date: 2024/06/11 3:20:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_loaned_laptops_DateUpdated]
ON [dbo].[loaned_laptops] 
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE loaned_laptops 
    SET date_updated = GETDATE()
    FROM loaned_laptops 
    INNER JOIN inserted ON loaned_laptops.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[loaned_laptops] ENABLE TRIGGER [trigger_loaned_laptops_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_delete_loaned_sdwans]    Script Date: 2024/06/11 3:20:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- LOANED SDWAN START
CREATE TRIGGER [dbo].[trigger_delete_loaned_sdwans]
ON [dbo].[loaned_sdwans]
AFTER DELETE
AS
BEGIN
 
    INSERT INTO trailing_loaned_sdwans (loaned_sdwans_id, user_loaned_id, user_created,descriptions, date_created, date_updated)
    SELECT loaned_sdwans_id, user_loaned_id, user_created,descriptions, date_created, date_updated
    FROM deleted;
END;
GO
ALTER TABLE [dbo].[loaned_sdwans] ENABLE TRIGGER [trigger_delete_loaned_sdwans]
GO
/****** Object:  Trigger [dbo].[trigger_loaned_sdwans_DateCreated]    Script Date: 2024/06/11 3:20:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- trigger


-- lOANED SDWAN TRIGGERS START
CREATE TRIGGER [dbo].[trigger_loaned_sdwans_DateCreated]
ON [dbo].[loaned_sdwans] 
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE loaned_sdwans 
    SET date_created = GETDATE()
    FROM loaned_sdwans 
    INNER JOIN inserted ON loaned_sdwans.id = inserted.id
    WHERE loaned_sdwans.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[loaned_sdwans] ENABLE TRIGGER [trigger_loaned_sdwans_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_loaned_sdwans_DateUpdated]    Script Date: 2024/06/11 3:20:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_loaned_sdwans_DateUpdated]
ON [dbo].[loaned_sdwans] 
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE loaned_sdwans 
    SET date_updated = GETDATE()
    FROM loaned_sdwans 
    INNER JOIN inserted ON loaned_sdwans.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[loaned_sdwans] ENABLE TRIGGER [trigger_loaned_sdwans_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_router_mtc_DateCreated]    Script Date: 2024/06/11 3:20:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ROUTE MTC TRIGGERS START
CREATE TRIGGER [dbo].[trigger_router_mtc_DateCreated]
ON [dbo].[router_mtc]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE router_mtc
    SET date_created = GETDATE()
    FROM router_mtc
    INNER JOIN inserted ON router_mtc.id = inserted.id
    WHERE router_mtc.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[router_mtc] ENABLE TRIGGER [trigger_router_mtc_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_router_mtc_DateUpdated]    Script Date: 2024/06/11 3:20:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_router_mtc_DateUpdated]
ON [dbo].[router_mtc]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE router_mtc
    SET date_updated = GETDATE()
    FROM router_mtc
    INNER JOIN inserted ON router_mtc.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[router_mtc] ENABLE TRIGGER [trigger_router_mtc_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_sdwan_laptops_DateCreated]    Script Date: 2024/06/11 3:20:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- SDWAN LAPTOPS TRIGGERS START
CREATE TRIGGER [dbo].[trigger_sdwan_laptops_DateCreated]
ON [dbo].[sdwan_laptops]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE sdwan_laptops
    SET date_created = GETDATE()
    FROM sdwan_laptops
    INNER JOIN inserted ON sdwan_laptops.id = inserted.id
    WHERE sdwan_laptops.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[sdwan_laptops] ENABLE TRIGGER [trigger_sdwan_laptops_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_sdwan_laptops_DateUpdated]    Script Date: 2024/06/11 3:20:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_sdwan_laptops_DateUpdated]
ON [dbo].[sdwan_laptops]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE sdwan_laptops
    SET date_updated = GETDATE()
    FROM sdwan_laptops
    INNER JOIN inserted ON sdwan_laptops.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[sdwan_laptops] ENABLE TRIGGER [trigger_sdwan_laptops_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_sdwans_DateCreated]    Script Date: 2024/06/11 3:20:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- SDWAN TRIGGERS START
CREATE TRIGGER [dbo].[trigger_sdwans_DateCreated]
ON [dbo].[sdwans]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE sdwans
    SET date_created = GETDATE()
    FROM sdwans
    INNER JOIN inserted ON sdwans.id = inserted.id
    WHERE sdwans.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[sdwans] ENABLE TRIGGER [trigger_sdwans_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_sdwans_DateUpdated]    Script Date: 2024/06/11 3:20:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_sdwans_DateUpdated]
ON [dbo].[sdwans]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE sdwans
    SET date_updated = GETDATE()
    FROM sdwans
    INNER JOIN inserted ON sdwans.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[sdwans] ENABLE TRIGGER [trigger_sdwans_DateUpdated]
GO
/****** Object:  Trigger [dbo].[trigger_users_DateCreated]    Script Date: 2024/06/11 3:20:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  USERS TRIGGERS START
CREATE TRIGGER [dbo].[trigger_users_DateCreated]
ON [dbo].[users]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE users
    SET date_created = GETDATE()
    FROM users
    INNER JOIN inserted ON users.id = inserted.id
    WHERE users.date_created IS NULL;
END;
GO
ALTER TABLE [dbo].[users] ENABLE TRIGGER [trigger_users_DateCreated]
GO
/****** Object:  Trigger [dbo].[trigger_users_DateUpdated]    Script Date: 2024/06/11 3:20:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trigger_users_DateUpdated]
ON [dbo].[users] 
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE users 
    SET date_updated = GETDATE()
    FROM users 
    INNER JOIN inserted ON users.id = inserted.id;
END;
GO
ALTER TABLE [dbo].[users] ENABLE TRIGGER [trigger_users_DateUpdated]
GO
USE [master]
GO
ALTER DATABASE [nbc_asset_management] SET  READ_WRITE 
GO

--先建立一個DB叫DemoDB

USE [DemoDB]
GO
/****** Object:  Table [dbo].[apply_file]    Script Date: 2024/1/7 下午 07:05:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[apply_file](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[file_path] [varchar](255) NULL,
	[created_at] [datetimeoffset](7) NULL,
	[updated_at] [datetimeoffset](7) NULL,
 CONSTRAINT [PK__apply_fi__3213E83F8D8A1AB0] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[orgs]    Script Date: 2024/1/7 下午 07:05:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[orgs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](50) NULL,
	[org_no] [nvarchar](50) NULL,
	[created_at] [datetimeoffset](7) NULL,
	[updated_at] [datetimeoffset](7) NULL,
 CONSTRAINT [PK__orgs__3213E83F4E6FC1DE] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[syslog]    Script Date: 2024/1/7 下午 07:05:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[syslog](
	[seq_no] [int] IDENTITY(1,1) NOT NULL,
	[user_account] [varchar](50) NULL,
	[ipaddress] [varchar](60) NULL,
	[login_at] [datetime] NULL,
	[created_at] [datetimeoffset](7) NULL,
	[note] [nvarchar](255) NULL,
 CONSTRAINT [PK__syslog__4B660EB1E10B2721] PRIMARY KEY CLUSTERED 
(
	[seq_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 2024/1/7 下午 07:05:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_type] [int] NULL,
	[org_id] [int] NULL,
	[user_name] [nvarchar](50) NULL,
	[birthday] [date] NULL,
	[email] [varchar](255) NULL,
	[user_account] [varchar](50) NULL,
	[user_hash] [varchar](255) NULL,
	[user_salt] [varchar](16) NULL,
	[status] [int] NULL,
	[created_at] [datetimeoffset](7) NULL,
	[updated_at] [datetimeoffset](7) NULL,
 CONSTRAINT [PK__users__3213E83F714CF319] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[apply_file]  WITH CHECK ADD  CONSTRAINT [FK__apply_fil__user___32E0915F] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[apply_file] CHECK CONSTRAINT [FK__apply_fil__user___32E0915F]
GO
ALTER TABLE [dbo].[users]  WITH CHECK ADD  CONSTRAINT [FK__users__org_id__300424B4] FOREIGN KEY([org_id])
REFERENCES [dbo].[orgs] ([id])
GO
ALTER TABLE [dbo].[users] CHECK CONSTRAINT [FK__users__org_id__300424B4]
GO
USE [master]
GO
ALTER DATABASE [DemoDB] SET  READ_WRITE 
GO

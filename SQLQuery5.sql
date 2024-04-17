USE ADM_weighingconveyor_scada;
GO
DROP TABLE  [dbo].[customer]
GO
CREATE TABLE  [dbo].[customer](
	[id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[cust_code] [nvarchar](50) NULL,
	[cust_name] [nvarchar](128) NULL,
	[cust_company] [nvarchar](128) NULL,
	[cust_avatar] [nvarchar](250) NULL,
	[cust_add] [nvarchar](128) NULL,
	[payment_term] [nvarchar](50) NULL,
	[email_address] [nvarchar](50) NULL,
	[fax_no] [nvarchar](50) NULL,
	[tel_no] [nvarchar](50) NULL,
	[mobile_no] [nvarchar](50) NOT NULL,
	[created_date] [datetime] NULL,
	[created_by] [nvarchar](50) NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [nvarchar](50) NULL
) ON [PRIMARY]
GO
DROP TABLE  [dbo].[product];
GO
CREATE TABLE  [dbo].[product](
	[id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[prod_code] [nvarchar](20) NULL,
	[prod_fullname] [nvarchar](255) NULL,
	[hash_code] [nvarchar](55) NULL,
	[ingredient] [nvarchar](20) NULL,
	[exp] [decimal](18, 2) NULL,
	[market] [nvarchar](10) NULL,
	[prod_name] [nvarchar](128) NULL,
	[label_path] [nvarchar](255) NULL,
	[barcode] [nvarchar](20) NULL,
	[delay_m4] [nvarchar](20) NULL,
	[delay_m5] [nvarchar](20) NULL,
	[pack_size] [decimal](18, 2) NULL,
	[loose_uom] [nvarchar](10) NULL,
	[whole_uom] [nvarchar](50) NULL,
	[created_date] [datetime] NULL,
	[created_by] [nvarchar](50) NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [nvarchar](50) NULL
) ON [PRIMARY]
GO
DROP TABLE  [dbo].[prod_shift_data]
GO
CREATE TABLE  [dbo].[prod_shift_data](
	[id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[work_order_no] NVARCHAR(50) NULL,
	[prod_code] NVARCHAR(50) NULL,
	[lot_no] NVARCHAR(50) NULL,
	[production_date] DATETIME NULL,
	[expiry_date] DATETIME NULL,
	[user_name] NVARCHAR (50) NULL,
	[shift_no] NVARCHAR (10) NULL,
	[cust_code] NVARCHAR (50) NULL,
	[device_code] NVARCHAR(50) NULL,
	[qty_tare_weigh] DECIMAL (18, 2) NULL,
	[qty_order_weigh] DECIMAL(18,2),--//
	[loose_uom] [nvarchar](10) NULL,--//
	[created_by] NVARCHAR(50) NULL,
	[created_date] [datetime] NULL,
	[updated_by] NVARCHAR(50) NULL,
	[updated_date] [datetime] NULL
) ON [PRIMARY]
GO
DROP TABLE  [dbo].[weigh_session]
GO
CREATE TABLE  [dbo].[weigh_session] (
    [id] INT IDENTITY(1,1) PRIMARY KEY not null,
    [session_code] NVARCHAR(50),
    [start_time] DATETIME,
    [end_time] DATETIME Null,
    [cust_id] INT,
    [cust_name] NVARCHAR(128),
    [cust_address] NVARCHAR(128),
    [boat_id] NVARCHAR(50),
    [so_number] NVARCHAR(50),
    [qty_counted] INT,
	[qty_weighed] DECIMAL(18,2),
    [qty_tare_weigh] DECIMAL(18,2),
    [qty_good_weigh] DECIMAL(18,2), --//
	[qty_order_weigh] DECIMAL(18,2),
    [gap] DECIMAL(18,2),
    [document_no] NVARCHAR(50),
	[shift_data_id] [int] NULL,
	[user_id] [int] NULL,
    [device_code] NVARCHAR(50) NULL,
    [status_code] NVARCHAR(1) NULL,
    [created_date] DATETIME NULL,
    [created_by] NVARCHAR(50) NULL,
    [updated_date] DATETIME NULL,
    [updated_by] NVARCHAR(50) NULL
);
GO
DROP TABLE  [dbo].[weigh_session_d]
GO
CREATE TABLE  [dbo].[weigh_session_d](
	[id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[session_code] NVARCHAR(50),
	[current_weigh] DECIMAL(18,2),  -- from plc
	[barcode] [nvarchar](20) NULL,  -- from plc
	[prod_code] [nvarchar](50) NULL, -- from last [prod_shift_data] 
	[prod_fullname] [nvarchar](255) NULL,   -- from last [prod_shift_data] 
	[prod_d365_code] [nvarchar](50) NULL,  -- from [Product] that product_code = product_code of last [prod_shift_data] 
	[production_date] [datetime] NULL,-- from [Product] that product_code = product_code of last [prod_shift_data] 
	[start_time] [datetime] NULL, -- from [weigh_session] parent
	[end_time] [datetime] NULL, -- from [weigh_session] parent
	[qty_counted] INT, -- -- from [weigh_session] parent
    [qty_weighed] DECIMAL(18,2), -- from [weigh_session] parent
    [gap] DECIMAL(18,2),  -- from [weigh_session] parent
	[shift_data_id] [int] NULL,   -- [prod_shift_data] id
	[created_date] [datetime] NULL, -- now
	[created_by] [nvarchar](50) NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [nvarchar](50) NULL
) ON [PRIMARY]
GO
DROP TABLE  [dbo].[device]
GO
CREATE TABLE  [dbo].[device](
	[id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[device_name] [nvarchar](50) NULL,
	[ip_address] [nvarchar](50) NULL,
	[port] [nvarchar](50) NULL,
	[created_date] [datetime] NULL,
	[created_by] [nvarchar](50) NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [nvarchar](50) NULL
) ON [PRIMARY]
GO
DROP TABLE  [dbo].[user]
GO
CREATE TABLE  [dbo].[user](
	[id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[user_code] [nvarchar](50) NULL,
	[user_name] [nvarchar](50) NULL,
	[password][nvarchar](512) not null,
	[user_avatar] [nvarchar](250) NULL,
	[user_group] [int] NULL,
	[email_address] [nvarchar](50) NULL,
	[tel_no] [nvarchar](50) NULL,
	[mobile_no] [nvarchar](50) NULL,
	[created_date] [datetime] NULL,
	[created_by] [nvarchar](50) NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [nvarchar](50) NULL
) ON [PRIMARY]
GO
DROP TABLE  [dbo].[user_group]
GO
CREATE TABLE  [dbo].[user_group](
	[id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[group_description] [nvarchar](128) NULL,
	[created_date] [datetime] NULL,
	[created_by] [nvarchar](50) NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [nvarchar](50) NULL
) ON [PRIMARY]
GO
DROP TABLE  [dbo].[variable]
GO
CREATE TABLE [dbo].[variable] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [device_id] INT  NOT NULL,
    [type] INT NOT NULL,
    [area] INT NOT NULL,
	[bit_address] INT NOT NULL,
    [byte_address] INT NOT NULL,
    [name] NVARCHAR(255) NOT NULL,
    [module] NVARCHAR(50),
    [unit] NVARCHAR(50),
    [message] NVARCHAR(255),
    [value] NVARCHAR(255),
    [purpose] NVARCHAR(50),
	[created_date] [datetime] NULL,
	[created_by] [nvarchar](50) NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [nvarchar](50) NULL
) ON [PRIMARY]
go
DROP TABLE  [dbo].[tmp_printreport]
GO
CREATE TABLE [dbo].[tmp_printreport] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
	[session_code] NVARCHAR(50),
) ON [PRIMARY]
go
Use ADM_weighingconveyor_scada 
go
INSERT INTO [dbo].[customer] (cust_code, cust_name, cust_company, cust_avatar, cust_add, payment_term, email_address, fax_no, tel_no, mobile_no, created_date, created_by, updated_date, updated_by) 
VALUES 
('CUST001', 'Customer 1', 'Company A', 'avatar1.jpg', 'Address 1', 'Net 30 Days', 'customer1@example.com', '123456789', '987654321', '123-456-7890', GETDATE(), 'Admin', GETDATE(), 'Admin'),
('CUST002', 'Customer 2', 'Company B', 'avatar2.jpg', 'Address 2', 'Net 15 Days', 'customer2@example.com', '987654321', '123456789', '987-654-3210', GETDATE(), 'Admin', GETDATE(), 'Admin'),
('CUST003', 'Customer 3', 'Company C', 'avatar3.jpg', 'Address 3', 'Net 30 Days', 'customer3@example.com', '123456789', '987654321', '123-987-4560', GETDATE(), 'Admin', GETDATE(), 'Admin'),
('CUST004', 'Customer 4', 'Company D', 'avatar4.jpg', 'Address 4', 'Net 45 Days', 'customer4@example.com', '987654321', '123456789', '789-456-1230', GETDATE(), 'Admin', GETDATE(), 'Admin'),
('CUST005', 'Customer 5', 'Company E', 'avatar5.jpg', 'Address 5', 'Net 30 Days', 'customer5@example.com', '123456789', '987654321', '987-123-6540', GETDATE(), 'Admin', GETDATE(), 'Admin'),
('CUST006', 'Customer 6', 'Company F', 'avatar6.jpg', 'Address 6', 'Net 60 Days', 'customer6@example.com', '987654321', '123456789', '654-789-3210', GETDATE(), 'Admin', GETDATE(), 'Admin'),
('CUST007', 'Customer 7', 'Company G', 'avatar7.jpg', 'Address 7', 'Net 30 Days', 'customer7@example.com', '123456789', '987654321', '789-123-6540', GETDATE(), 'Admin', GETDATE(), 'Admin'),
('CUST008', 'Customer 8', 'Company H', 'avatar8.jpg', 'Address 8', 'Net 30 Days', 'customer8@example.com', '987654321', '123456789', '654-987-3210', GETDATE(), 'Admin', GETDATE(), 'Admin'),
('CUST009', 'Customer 9', 'Company I', 'avatar9.jpg', 'Address 9', 'Net 30 Days', 'customer9@example.com', '123456789', '987654321', '321-654-9870', GETDATE(), 'Admin', GETDATE(), 'Admin'),
('CUST010', 'Customer 10', 'Company J', 'avatar10.jpg', 'Address 10', 'Net 30 Days', 'customer10@example.com', '987654321', '123456789', '789-654-3210', GETDATE(), 'Admin', GETDATE(), 'Admin');

INSERT INTO [dbo].[product] (prod_code, prod_fullname, hash_code, ingredient, exp, market, prod_name, label_path, barcode, delay_m4, delay_m5, pack_size, loose_uom, whole_uom, created_date, created_by, updated_date, updated_by)
VALUES 
('01015M3000', 'Chicken feed - CF2020 Crumble - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'CF2020', NULL, '1005', NULL, NULL, 30.00, 'kg', 'box', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('01016W3000', 'Chicken feed - CF2040 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'CF2040', NULL, '1006', NULL, NULL, 30.00, 'kg', 'box', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('01018M3000', 'Chicken feed - C222 Crumble - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'C222', 'C222_2024C.bmp', '1007', NULL, NULL, 30.00, 'kg', 'box', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('01019W3000', 'Chicken feed - C224 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'C224', 'C224_2024C.bmp', '1008', NULL, NULL, 30.00, 'kg', 'box', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('01516M3000', 'Chicken feed - C242 Crumble - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'C242', NULL, '1015', NULL, NULL, 30.00, 'kg', 'box', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('01517V3000', 'Chicken feed - C262 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'C262', NULL, '1016', NULL, NULL, 30.00, 'kg', 'box', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('15521G3000', 'Duck feed - D777 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'D777', 'D777_2024C.bmp', '1032', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('15522G3000', 'Duck feed - D779 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'D779', 'D779_2024C.bmp', '1033', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('15524G3000', 'Chicken feed - C421 Granular - 30kg NEW', NULL, NULL, 3, 'Cambodia', 'C421', 'C421_2024C.bmp', '1034', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('15525W3000', 'Duck feed - D722 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'D722', NULL, '1035', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('15526V3000', 'Duck feed - D724 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'D724', NULL, '1036', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('15527G3000', 'Duck feed - D775 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'D775', NULL, '1037', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('26033W0200', 'Pig feed - Jolie1 Granular-02kg (in box 20kg)-NEW', NULL, NULL, 3, 'Cambodia', 'Jolie1', 'Jolie1-2kg_2024C.bmp', '1055', NULL, NULL, 2.00, 'kg', 'box', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('26034W3000', 'Pig feed - Jolie1 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'Jolie1', 'Jolie1-30kg_2024C.bmp', '1056', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('26035V0500', 'Pig feed - Jolie2 Granular - 5kg- NEW', NULL, NULL, 3, 'Cambodia', 'Jolie2', 'Jolie2-5kg_2024C.bmp', '1057', NULL, NULL, 5.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('26036V3000', 'Pig feed - Jolie2 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'Jolie2', 'Jolie2-30kg_2024C.bmp', '1058', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('26039V3000', 'Pig feed - Spidy2 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'Spidy2', NULL, '1059', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('28016F3000', 'Pig feed - B9999 Powder - 30kg- NEW', NULL, NULL, 4, 'Cambodia', 'B9999', 'B9999-30kg_2024C.bmp', '1074', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('28017F3000', 'Pig feed - CF900 Powder - 30kg- NEW', NULL, NULL, 4, 'Cambodia', 'CF900', NULL, '1075', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('28019F0500', 'Pig feed - B9999 Powder - 05kg- NEW', NULL, NULL, 4, 'Cambodia', 'B9999', 'B9999-5kg_2024C.bmp', '1076', NULL, NULL, 5.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('28020F0500', 'Pig feed - CF900 Powder - 05kg- NEW', NULL, NULL, 4, 'Cambodia', 'CF900', NULL, '1077', NULL, NULL, 5.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('28543G3000', 'Pig feed - B112 Mash - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'B112', NULL, '1086', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('28544G3000', 'Pig feed - B114 Mash - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'B114', NULL, '1087', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('28546G3000', 'Pig feed - CF3060 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'CF3060', NULL, '1088', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('28548G3000', 'Pig feed - B110 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'B110', 'B110_2024C.bmp', '1089', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('28549G3000', 'Pig feed - B112 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'B112', 'B112_2024C.bmp', '1090', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('28550G3000', 'Pig feed - B114 Granular - 30Kg- NEW', NULL, NULL, 3, 'Cambodia', 'B114', 'B114_2024C.bmp', '1091', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('32017G3000', 'Pig feed - M116 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'M116', NULL, '1100', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('32018G3000', 'Pig feed - M118 Granular - 30kg- NEW', NULL, NULL, 3, 'Cambodia', 'M118', 'M118_2024C.bmp', '1101', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('34513F0500', 'Pig feed - M16 Powder - 05kg- NEW', NULL, NULL, 4, 'Cambodia', 'M16', NULL, '1110', NULL, NULL, 5.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('34514F3000', 'Pig feed - M16 Powder - 30kg- NEW', NULL, NULL, 4, 'Cambodia', 'M16', NULL, '1111', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('34515F0500', 'Pig feed - M18 Powder - 05kg- NEW', NULL, NULL, 4, 'Cambodia', 'M18', 'M18-5kg_2024C.bmp', '1112', NULL, NULL, 5.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('34516F3000', 'Pig feed - M18 Powder - 30kg- NEW', NULL, NULL, 4, 'Cambodia', 'M18', 'M18-30kg_2024C.bmp', '1113', NULL, NULL, 30.00, 'kg', 'bag', '2024-04-05', 'Admin', '2024-04-05', 'Admin');
INSERT INTO [dbo].[prod_shift_data] (
    [work_order_no], 
    [prod_code], 
    [lot_no], 
    [production_date], 
    [expiry_date], 
    [user_name], 
    [shift_no], 
    [cust_code], 
    [device_code], 
    [qty_tare_weigh], 
    [qty_order_weigh], 
    [loose_uom], 
    [created_by], 
    [created_date], 
    [updated_by], 
    [updated_date]
) 
VALUES 
('WO12345', '01015M3000', 'LOT123', '2024-04-05', '2024-04-10', 'John Doe', 'Shift A', 'CUST001', 'DEV001', 100.00, 100.00, 'bag', 'Admin', '2024-04-05', 'Admin', '2024-04-05'),
('WO12346', '01016W3000', 'LOT124', '2024-04-05', '2024-04-10', 'Jane Smith', 'Shift B', 'CUST002', 'DEV002', 150.00, 150.00, 'bag', 'Admin', '2024-04-05', 'Admin', '2024-04-05'),
('WO12347', '15521G3000', 'LOT125', '2024-04-05', '2024-04-10', 'Michael Johnson', 'Shift C', 'CUST003', 'DEV003', 200.00, 200.00, 'bag', 'Admin', '2024-04-05', 'Admin', '2024-04-05'),
('WO12348', '26033W0200', 'LOT126', '2024-04-05', '2024-04-10', 'Emily Brown', 'Shift A', 'CUST004', 'DEV004', 120.00, 120.00, 'bag', 'Admin', '2024-04-05', 'Admin', '2024-04-05'),
('WO12349', '28016F3000', 'LOT127', '2024-04-05', '2024-04-10', 'David Wilson', 'Shift B', 'CUST005', 'DEV005', 180.00, 180.00, 'bag', 'Admin', '2024-04-05', 'Admin', '2024-04-05');
INSERT INTO [dbo].[device] ([device_name], [ip_address], [port], [created_date], [created_by], [updated_date], [updated_by]) 
VALUES 
('D1', '192.168.1.31', '9100', '2024-04-07', 'Admin', '2024-04-07', 'Admin'),
('D2', '127.0.0.1', '9100', '2024-04-07', 'Admin', '2024-04-07', 'Admin'),
('D3', '192.168.1.103', '8080', '2024-04-07', 'Admin', '2024-04-07', 'Admin'),
('D4', '192.168.1.104', '8080', '2024-04-07', 'Admin', '2024-04-07', 'Admin'),
('D5', '192.168.1.105', '8080', '2024-04-07', 'Admin', '2024-04-07', 'Admin');
INSERT INTO [dbo].[user] ([user_code], [user_name], [password], [user_avatar], [user_group], [email_address], [tel_no], [mobile_no], [created_date], [created_by], [updated_date], [updated_by]) 
VALUES 
('USR001', 'JohnDoe', 'password123', NULL, 1, 'john.doe@example.com', '123456789', '987654321', '2024-04-07', 'Admin', '2024-04-07', 'Admin'),
('USR002', 'JaneSmith', 'password456', NULL, 2, 'jane.smith@example.com', '123456780', '987654322', '2024-04-07', 'Admin', '2024-04-07', 'Admin'),
('USR003', 'MikeJohnson', 'password789', NULL, 1, 'mike.johnson@example.com', '123456781', '987654323', '2024-04-07', 'Admin', '2024-04-07', 'Admin'),
('USR004', 'EmilyBrown', 'passwordabc', NULL, 2, 'emily.brown@example.com', '123456782', '987654324', '2024-04-07', 'Admin', '2024-04-07', 'Admin'),
('USR005', 'DavidWilson', 'passwordxyz', NULL, 1, 'david.wilson@example.com', '123456783', '987654325', '2024-04-07', 'Admin', '2024-04-07', 'Admin');
INSERT INTO [dbo].[user_group] ([group_description], [created_date], [created_by], [updated_date], [updated_by])
VALUES 
('Admin Group', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('Managers Group', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('Employees Group', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('IT Team Group', '2024-04-05', 'Admin', '2024-04-05', 'Admin'),
('Sales Team Group', '2024-04-05', 'Admin','2024-04-05', 'Admin');

INSERT INTO [dbo].[weigh_session] (
    [session_code], 
    [start_time], 
    [end_time], 
    [cust_id], 
    [cust_name], 
    [cust_address], 
    [boat_id], 
    [so_number], 
    [qty_counted], 
    [qty_weighed], 
    [qty_tare_weigh], 
    [qty_good_weigh], 
    [qty_order_weigh], 
    [gap], 
    [document_no], 
    [shift_data_id], 
    [user_id], 
    [device_code], 
    [status_code], 
    [created_date], 
    [created_by], 
    [updated_date], 
    [updated_by]
) 
VALUES 
('ADMD10000001', '2024-04-05 08:00:00', '2024-04-05 10:00:00', 1, 'Customer A', 'Address A', 1, 'SO001', 100, 200, 5.00, 195.00, 190.00, 5.00, 'DOC001', 1, 1, 'DEV001', 'A', '2024-04-05 08:00:00', 'Admin', '2024-04-05 10:00:00', 'Admin'),
('ADMD10000002', '2024-04-05 09:00:00', '2024-04-05 11:00:00', 2, 'Customer B', 'Address B', 2, 'SO002', 150, 250, 6.00, 244.00, 240.00, 4.00, 'DOC002', 1, 2, 'DEV002', 'A', '2024-04-05 09:00:00', 'Admin', '2024-04-05 11:00:00', 'Admin'),
('ADMD10000003', '2024-04-05 10:00:00', '2024-04-05 12:00:00', 3, 'Customer C', 'Address C', 3, 'SO003', 200, 300, 7.00, 293.00, 290.00, 3.00, 'DOC003', 1, 3, 'DEV003', 'A', '2024-04-05 10:00:00', 'Admin', '2024-04-05 12:00:00', 'Admin'),
('ADMD10000004', '2024-04-05 11:00:00', '2024-04-05 13:00:00', 4, 'Customer D', 'Address D', 4, 'SO004', 250, 350, 8.00, 342.00, 340.00, 2.00, 'DOC004', 2, 1, 'DEV004', 'A', '2024-04-05 11:00:00', 'Admin', '2024-04-05 13:00:00', 'Admin'),
('ADMD10000005', '2024-04-05 12:00:00', '2024-04-05 14:00:00', 5, 'Customer E', 'Address E', 5, 'SO005', 300, 400, 9.00, 391.00, 390.00, 1.00, 'DOC005', 2, 2, 'DEV005', 'A', '2024-04-05 12:00:00', 'Admin', '2024-04-05 14:00:00', 'Admin');


DECLARE @ParentSessionID INT
SET @ParentSessionID = 2

-- Insert the first 10 records with some modifications
INSERT INTO [dbo].[weigh_session_d] (
    [session_code], 
    [current_weigh], 
    [barcode], 
    [prod_code], 
    [prod_fullname], 
    [prod_d365_code], 
    [production_date], 
    [start_time], 
    [end_time], 
    [qty_counted], 
    [qty_weighed], 
    [gap], 
    [shift_data_id], 
    [created_date], 
    [created_by], 
    [updated_date], 
    [updated_by]
)
SELECT TOP 10
    'ADMD10000001',  -- Sample session code
    w.[qty_tare_weigh],  -- Data from weigh_session parent table
    'BARCODE00',  -- Sample barcode from PLC
    s.[prod_code],  -- Product code from last prod_shift_data
    prod.[prod_fullname],  -- Product full name from last prod_shift_data
    prod.[hash_code],  -- Product D365 code from Product table where product_code matches with last prod_shift_data
    s.[production_date],  -- Production date from Product table where product_code matches with last prod_shift_data
    w.[start_time],  -- Start time from weigh_session parent table
    w.[end_time],  -- End time from weigh_session parent table
    w.[qty_counted],  -- Quantity counted from weigh_session parent table
    w.[qty_weighed],  -- Quantity weighed from weigh_session parent table
    w.[gap],  -- Gap from weigh_session parent table
    w.[shift_data_id],  -- Shift data id from last prod_shift_data
    GETDATE(),  -- Current date and time
    'Admin',  -- Created by
    GETDATE(),  -- Updated date (current date and time)
    'Admin'  -- Updated by
FROM 
    [dbo].[weigh_session] w
INNER JOIN
    [dbo].[prod_shift_data] s ON w.[shift_data_id] = s.[id]
INNER JOIN
    [dbo].[Product] prod ON s.[prod_code] = prod.[prod_code]
WHERE 
    w.[id] = @ParentSessionID

-- Insert the next 10 records with some modifications
INSERT INTO [dbo].[weigh_session_d] (
    [session_code], 
    [current_weigh], 
    [barcode], 
    [prod_code], 
    [prod_fullname], 
    [prod_d365_code], 
    [production_date], 
    [start_time], 
    [end_time], 
    [qty_counted], 
    [qty_weighed], 
    [gap], 
    [shift_data_id], 
    [created_date], 
    [created_by], 
    [updated_date], 
    [updated_by]
)
SELECT TOP 10
    'ADMD10000001',  -- Sample session code
    w.[qty_tare_weigh] - RAND(1),  -- Data from weigh_session parent table with modification
    'BARCODE00',  -- Sample barcode from PLC
    s.[prod_code],  -- Product code from last prod_shift_data
    prod.[prod_fullname],  -- Product full name from last prod_shift_data
    prod.[hash_code],  -- Product D365 code from Product table where product_code matches with last prod_shift_data
    s.[production_date],  -- Production date from Product table where product_code matches with last prod_shift_data
    w.[start_time],  -- Start time from weigh_session parent table
    w.[end_time],  -- End time from weigh_session parent table
    w.[qty_counted],  -- Quantity counted from weigh_session parent table
    w.[qty_weighed],  -- Quantity weighed from weigh_session parent table
    w.[gap],  -- Gap from weigh_session parent table
    w.[shift_data_id],  -- Shift data id from last prod_shift_data
    GETDATE(),  -- Current date and time
    'Admin',  -- Created by
    GETDATE(),  -- Updated date (current date and time)
    'Admin'  -- Updated by
FROM 
    [dbo].[weigh_session] w
INNER JOIN
    [dbo].[prod_shift_data] s ON w.[shift_data_id] = s.[id]
INNER JOIN
    [dbo].[Product] prod ON s.[prod_code] = prod.[prod_code]
WHERE 
    w.[id] = @ParentSessionID

INSERT INTO [dbo].[variable] 
([device_id], [type], [area], [bit_address], [byte_address], [name], [module], [unit], [message], [value], [purpose], [created_date], [created_by], [updated_date], [updated_by])
VALUES
(1, 0, 5, 0, 0, 'bJogNeg_Rewinder', 'General', 'Celsius', 'Temperature reading from sensor','0', 'Read', GETDATE(), 'Admin', GETDATE(), 'Admin'),
(1, 0, 5, 1, 0, 'bJogPos_Rewinder', 'General', 'PSI', 'Pressure reading from sensor', '0', 'Read', GETDATE(), 'Admin', GETDATE(), 'Admin'),
(1, 0, 5, 2, 0, 'bJogNeg_StampServo', 'Infeed', 'Liters/min', 'Flow rate measurement', '0', 'Read', GETDATE(), 'Admin', GETDATE(), 'Admin'),
(1, 0, 5, 3, 0, 'bJogPos_StampServo', 'Infeed', 'Meters', 'Liquid level measurement', '0', 'Read', GETDATE(), 'Admin', GETDATE(), 'Admin'),
(1, 0, 5, 4, 0, 'bEnable_StampServo', 'Infeed', 'RPM', 'Rotational speed measurement', '0', 'Read', GETDATE(), 'Admin', GETDATE(), 'Admin'),
(1, 0, 5, 5, 0, 'bHome_StampServo', 'Outfeed', 'Volts', 'Voltage measurement', '0', 'Read', GETDATE(), 'Admin', GETDATE(), 'Admin'),
(1, 6, 5, 0, 2, 'Servo_CurPos', 'Outfeed', 'Percentage', 'Humidity reading from sensor', 0, 'Read', GETDATE(), 'Admin', GETDATE(), 'Admin'),
(1, 4, 5, 0, 6, 'nCounter', 'Scale', 'Amps', 'Current measurement', '0', 'Read', GETDATE(), 'Admin', GETDATE(), 'Admin'),
(1, 6, 4, 0, 0, 'Distance Sensor', 'Scale', 'Meters', 'Distance measurement', '0', 'Read', GETDATE(), 'Admin', GETDATE(), 'Admin'),
(1, 6, 4, 0, 4, 'Force Sensor', 'Scale', 'Newtons', 'Force measurement', '0', 'Read', GETDATE(), 'Admin',  GETDATE(), 'Admin');

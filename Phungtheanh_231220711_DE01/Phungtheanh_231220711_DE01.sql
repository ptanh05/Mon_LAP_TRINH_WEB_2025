CREATE DATABASE PhungTheAnh_23220711_de01;
GO

USE PhungTheAnh_23220711_de01;
GO

CREATE TABLE PtaComputer (
    ptaComId INT IDENTITY(1,1) PRIMARY KEY,
    ptaComName NVARCHAR(100) NOT NULL,
    ptaComPrice DECIMAL(10,2) NOT NULL,
    ptaComImage NVARCHAR(200) NOT NULL,
    ptaComStatus BIT NOT NULL
);
GO

INSERT INTO PtaComputer (ptaComName, ptaComPrice, ptaComImage, ptaComStatus) VALUES
('Laptop Gaming ASUS ROG', 25000000, 'asus-rog.jpg', 1),
('PC Desktop Dell OptiPlex', 15000000, 'dell-optiplex.png', 1),
('Máy tính Phùng Thế Anh - MSV: 23220711', 12000000, 'phungtheanh.jpg', 0);
GO

SELECT * FROM PtaComputer;
GO

SELECT name FROM sys.databases WHERE name = 'PhungTheAnh_23220711_de01';

USE PhungTheAnh_23220711_de01;
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PtaComputer';

SELECT * FROM PtaComputer;
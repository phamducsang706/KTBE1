IF NOT EXISTS (SELECT 1 FROM Category WHERE CategoryName = N'Áo thun')
    INSERT INTO Category (CategoryName) VALUES (N'Áo thun');

IF NOT EXISTS (SELECT 1 FROM Category WHERE CategoryName = N'Quần jean')
    INSERT INTO Category (CategoryName) VALUES (N'Quần jean');

INSERT INTO Product
    (CategoryID, ProductName, ProductDecription, ProductPrice, ProductImage)
SELECT CategoryID, N'Áo thun đen', N'Áo thun cotton, dễ phối đồ', 149000, N'ao-thun-den.jpg'
FROM Category
WHERE CategoryName = N'Áo thun';

INSERT INTO Product
    (CategoryID, ProductName, ProductDecription, ProductPrice, ProductImage)
SELECT CategoryID, N'Áo thun trắng', N'Áo thun cotton màu trắng', 139000, N'ao-thun-trang.jpg'
FROM Category
WHERE CategoryName = N'Áo thun';

INSERT INTO Product
    (CategoryID, ProductName, ProductDecription, ProductPrice, ProductImage)
SELECT CategoryID, N'Quần jean xanh', N'Quần jean xanh dáng thường', 329000, N'quan-jean-xanh.jpg'
FROM Category
WHERE CategoryName = N'Quần jean';
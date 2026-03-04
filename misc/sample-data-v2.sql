-- Sample database v2: evolved schema for diffing/migration practice
-- Compatible with SQL Server (T-SQL)
--
-- Changes from v1 (sample-data.sql):
-- ┌─────────────────────────────────────────────────────────────────────────┐
-- │ TABLES                                                                  │
-- │  Customers    - ADDED: Phone, IsActive                                  │
-- │               - REMOVED: City (moved to Addresses table)                │
-- │  Products     - ADDED: SKU, IsActive, CategoryId (FK)                   │
-- │               - REMOVED: Category (string — now a FK to Categories)     │
-- │  Orders       - ADDED: ShippedDate, TrackingNumber                      │
-- │  OrderItems   - unchanged                                               │
-- │  Wishlists    - DELETED (dropped entirely)                              │
-- │  Categories   - NEW                                                     │
-- │  Addresses    - NEW (customer shipping addresses)                       │
-- │  Reviews      - NEW (product reviews)                                   │
-- │                                                                         │
-- │ STORED PROCEDURES                                                       │
-- │  GetOrdersByCustomer  - MODIFIED (reflects new Orders columns)          │
-- │  PlaceOrder           - MODIFIED (uses SKU lookup; sets TrackingNumber) │
-- │  UpdateOrderStatus    - MODIFIED (sets ShippedDate on Shipped status)   │
-- │  GetSalesSummary      - MODIFIED (joins Categories instead of string)   │
-- │  SearchCustomers      - DELETED                                         │
-- │  GetCustomerAddresses - NEW                                             │
-- │  AddProductReview     - NEW                                             │
-- │  GetProductsByCategory- NEW                                             │
-- └─────────────────────────────────────────────────────────────────────────┘

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SampleStore2')
    CREATE DATABASE SampleStore2;
GO

USE SampleStore2;
GO

-- ── Tables ────────────────────────────────────────────────────────────────────

-- Drop in FK-safe order
IF OBJECT_ID('dbo.Reviews',   'U') IS NOT NULL DROP TABLE dbo.Reviews;
IF OBJECT_ID('dbo.OrderItems','U') IS NOT NULL DROP TABLE dbo.OrderItems;
IF OBJECT_ID('dbo.Orders',    'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID('dbo.Addresses', 'U') IS NOT NULL DROP TABLE dbo.Addresses;
IF OBJECT_ID('dbo.Products',  'U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID('dbo.Categories','U') IS NOT NULL DROP TABLE dbo.Categories;
IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE dbo.Customers;

-- Customers: City removed, Phone + IsActive added
CREATE TABLE dbo.Customers (
    CustomerId   INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
    FirstName    NVARCHAR(50)  NOT NULL,
    LastName     NVARCHAR(50)  NOT NULL,
    Email        NVARCHAR(100) NOT NULL UNIQUE,
    Phone        NVARCHAR(20)  NULL,                          -- NEW
    IsActive     BIT           NOT NULL DEFAULT 1,            -- NEW
    CreatedDate  DATE          NOT NULL DEFAULT GETDATE()
    -- City removed — see Addresses table
);

-- Categories: new standalone lookup table (replaces Products.Category string)
CREATE TABLE dbo.Categories (
    CategoryId   INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Name         NVARCHAR(50)  NOT NULL UNIQUE,
    Description  NVARCHAR(200) NULL
);

-- Products: Category string removed; CategoryId FK + SKU + IsActive added
CREATE TABLE dbo.Products (
    ProductId    INT            NOT NULL IDENTITY(1,1) PRIMARY KEY,
    SKU          NVARCHAR(50)   NOT NULL UNIQUE,              -- NEW
    Name         NVARCHAR(100)  NOT NULL,
    CategoryId   INT            NOT NULL REFERENCES dbo.Categories(CategoryId), -- NEW (replaces Category string)
    Price        DECIMAL(10,2)  NOT NULL,
    StockQty     INT            NOT NULL DEFAULT 0,
    IsActive     BIT            NOT NULL DEFAULT 1            -- NEW
    -- Category NVARCHAR(50) removed
);

-- Addresses: new table for customer shipping addresses
CREATE TABLE dbo.Addresses (
    AddressId    INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
    CustomerId   INT           NOT NULL REFERENCES dbo.Customers(CustomerId),
    Label        NVARCHAR(50)  NOT NULL DEFAULT 'Home',       -- e.g. Home, Work
    Street       NVARCHAR(100) NOT NULL,
    City         NVARCHAR(50)  NOT NULL,
    State        NVARCHAR(50)  NULL,
    PostalCode   NVARCHAR(20)  NULL,
    IsDefault    BIT           NOT NULL DEFAULT 0
);

-- Orders: ShippedDate + TrackingNumber added
CREATE TABLE dbo.Orders (
    OrderId         INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
    CustomerId      INT           NOT NULL REFERENCES dbo.Customers(CustomerId),
    OrderDate       DATE          NOT NULL,
    ShippedDate     DATE          NULL,                       -- NEW
    TrackingNumber  NVARCHAR(50)  NULL,                       -- NEW
    Status          NVARCHAR(20)  NOT NULL DEFAULT 'Pending'
);

-- OrderItems: unchanged
CREATE TABLE dbo.OrderItems (
    OrderItemId  INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
    OrderId      INT           NOT NULL REFERENCES dbo.Orders(OrderId),
    ProductId    INT           NOT NULL REFERENCES dbo.Products(ProductId),
    Quantity     INT           NOT NULL,
    UnitPrice    DECIMAL(10,2) NOT NULL
);

-- Reviews: new table
CREATE TABLE dbo.Reviews (
    ReviewId     INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
    ProductId    INT           NOT NULL REFERENCES dbo.Products(ProductId),
    CustomerId   INT           NOT NULL REFERENCES dbo.Customers(CustomerId),
    Rating       TINYINT       NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Title        NVARCHAR(100) NULL,
    Body         NVARCHAR(2000) NULL,
    ReviewDate   DATE          NOT NULL DEFAULT GETDATE()
);
GO

-- ── Seed data ──────────────────────────────────────────────────────────────────

INSERT INTO dbo.Customers (FirstName, LastName, Email, Phone, IsActive, CreatedDate) VALUES
    ('Alice',   'Johnson',  'alice@example.com',   '617-555-0101', 1, '2024-01-15'),
    ('Bob',     'Smith',    'bob@example.com',     '312-555-0102', 1, '2024-02-03'),
    ('Carol',   'Williams', 'carol@example.com',   NULL,           1, '2024-03-20'),
    ('David',   'Brown',    'david@example.com',   '617-555-0104', 1, '2024-04-08'),
    ('Eve',     'Davis',    'eve@example.com',     '602-555-0105', 1, '2024-05-12'),
    ('Frank',   'Miller',   'frank@example.com',   '312-555-0106', 0, '2025-01-07');  -- IsActive=0 (deactivated)

INSERT INTO dbo.Categories (Name, Description) VALUES
    ('Electronics', 'Gadgets and tech accessories'),
    ('Office',      'Desk and workspace supplies'),
    ('Kitchen',     'Mugs, bottles, and kitchen gear');

INSERT INTO dbo.Products (SKU, Name, CategoryId, Price, StockQty, IsActive) VALUES
    ('EL-001', 'Wireless Mouse',        1,  29.99, 150, 1),
    ('EL-002', 'USB-C Hub',             1,  49.99,  80, 1),
    ('EL-003', 'Mechanical Keyboard',   1, 119.99,  45, 1),
    ('OF-001', 'Desk Lamp',             2,  34.99,  60, 1),
    ('OF-002', 'Notebook (3-pack)',     2,   8.99, 300, 1),
    ('OF-003', 'Standing Desk Mat',     2,  54.99,  35, 1),
    ('KT-001', 'Coffee Mug',            3,  14.99, 200, 1),
    ('KT-002', 'Reusable Water Bottle', 3,  22.99, 175, 1),
    ('EL-004', 'Webcam HD 1080p',       1,  79.99,  55, 1);   -- NEW product

INSERT INTO dbo.Addresses (CustomerId, Label, Street, City, State, PostalCode, IsDefault) VALUES
    (1, 'Home',  '10 Elm St',      'Boston',  'MA', '02101', 1),
    (2, 'Home',  '22 Oak Ave',     'Chicago', 'IL', '60601', 1),
    (2, 'Work',  '100 Wacker Dr',  'Chicago', 'IL', '60602', 0),
    (3, 'Home',  '5 Pine Rd',      'Denver',  'CO', '80201', 1),
    (4, 'Home',  '8 Maple Ln',     'Boston',  'MA', '02102', 1),
    (5, 'Home',  '77 Sun Blvd',    'Phoenix', 'AZ', '85001', 1),
    (6, 'Home',  '33 River Rd',    'Chicago', 'IL', '60603', 1);

INSERT INTO dbo.Orders (CustomerId, OrderDate, ShippedDate, TrackingNumber, Status) VALUES
    (1, '2025-01-10', '2025-01-12', 'TRK-10001', 'Delivered'),
    (2, '2025-01-14', '2025-01-16', 'TRK-10002', 'Delivered'),
    (1, '2025-02-03', '2025-02-05', 'TRK-10003', 'Shipped'),
    (3, '2025-02-18', '2025-02-20', 'TRK-10004', 'Delivered'),
    (4, '2025-03-01', NULL,         NULL,         'Pending'),
    (2, '2025-03-05', NULL,         NULL,         'Cancelled'),
    (5, '2025-03-10', '2025-03-12', 'TRK-10007', 'Shipped'),
    (6, '2025-03-11', NULL,         NULL,         'Pending');

INSERT INTO dbo.OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES
    (1, 1,  1,  29.99),
    (1, 7,  2,  14.99),
    (2, 3,  1, 119.99),
    (3, 2,  1,  49.99),
    (3, 4,  1,  34.99),
    (4, 5,  4,   8.99),
    (4, 6,  1,  54.99),
    (5, 1,  2,  29.99),
    (5, 2,  1,  49.99),
    (6, 3,  1, 119.99),
    (6, 4,  1,  34.99),
    (7, 8,  1,  22.99),
    (7, 7,  1,  14.99),
    (8, 5,  2,   8.99);

INSERT INTO dbo.Reviews (ProductId, CustomerId, Rating, Title, Body, ReviewDate) VALUES
    (1, 1, 5, 'Great mouse',       'Smooth tracking, comfortable grip.',      '2025-01-20'),
    (3, 2, 4, 'Solid keyboard',    'Nice feel but louder than expected.',      '2025-01-25'),
    (7, 1, 5, 'Love this mug',     'Keeps coffee hot for hours.',             '2025-01-22'),
    (6, 3, 3, 'Decent mat',        'A bit thin but does the job.',            '2025-03-01'),
    (8, 5, 5, 'Great bottle',      'Leakproof and keeps drinks cold all day.','2025-03-15');
GO

-- ── Stored Procedures ─────────────────────────────────────────────────────────

-- GetOrdersByCustomer: MODIFIED — includes ShippedDate and TrackingNumber
CREATE OR ALTER PROCEDURE dbo.GetOrdersByCustomer
    @CustomerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        o.OrderId,
        o.OrderDate,
        o.ShippedDate,
        o.TrackingNumber,
        o.Status,
        p.Name          AS Product,
        i.Quantity,
        i.UnitPrice,
        i.Quantity * i.UnitPrice  AS LineTotal
    FROM dbo.Orders     o
    JOIN dbo.OrderItems i ON i.OrderId   = o.OrderId
    JOIN dbo.Products   p ON p.ProductId = i.ProductId
    WHERE o.CustomerId = @CustomerId
    ORDER BY o.OrderDate DESC, o.OrderId, p.Name;
END;
GO

-- PlaceOrder: MODIFIED — looks up product by SKU instead of ProductId
CREATE OR ALTER PROCEDURE dbo.PlaceOrder
    @CustomerId  INT,
    @SKU         NVARCHAR(50),    -- changed: was @ProductId INT
    @Quantity    INT,
    @NewOrderId  INT OUTPUT,
    @ErrorMsg    NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @NewOrderId = -1;
    SET @ErrorMsg   = NULL;

    DECLARE @ProductId INT, @Stock INT, @Price DECIMAL(10,2);
    SELECT @ProductId = ProductId, @Stock = StockQty, @Price = Price
    FROM dbo.Products
    WHERE SKU = @SKU AND IsActive = 1;

    IF @ProductId IS NULL
    BEGIN
        SET @ErrorMsg = 'Active product with SKU ' + @SKU + ' not found.';
        RETURN;
    END

    IF @Stock < @Quantity
    BEGIN
        SET @ErrorMsg = 'Insufficient stock. Available: ' + CAST(@Stock AS NVARCHAR(10));
        RETURN;
    END

    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO dbo.Orders (CustomerId, OrderDate, Status)
        VALUES (@CustomerId, CAST(GETDATE() AS DATE), 'Pending');

        SET @NewOrderId = SCOPE_IDENTITY();

        INSERT INTO dbo.OrderItems (OrderId, ProductId, Quantity, UnitPrice)
        VALUES (@NewOrderId, @ProductId, @Quantity, @Price);

        UPDATE dbo.Products
        SET StockQty = StockQty - @Quantity
        WHERE ProductId = @ProductId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @NewOrderId = -1;
        SET @ErrorMsg   = ERROR_MESSAGE();
    END CATCH;
END;
GO

-- UpdateOrderStatus: MODIFIED — sets ShippedDate when transitioning to Shipped
CREATE OR ALTER PROCEDURE dbo.UpdateOrderStatus
    @OrderId        INT,
    @NewStatus      NVARCHAR(20),
    @TrackingNumber NVARCHAR(50)  = NULL,   -- NEW optional param
    @ErrorMsg       NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ErrorMsg = NULL;

    DECLARE @CurrentStatus NVARCHAR(20);
    SELECT @CurrentStatus = Status FROM dbo.Orders WHERE OrderId = @OrderId;

    IF @CurrentStatus IS NULL
    BEGIN
        SET @ErrorMsg = 'Order not found.';
        RETURN;
    END

    IF @CurrentStatus IN ('Delivered', 'Cancelled')
    BEGIN
        SET @ErrorMsg = 'Cannot change status of a ' + @CurrentStatus + ' order.';
        RETURN;
    END

    IF @NewStatus NOT IN ('Pending', 'Shipped', 'Delivered', 'Cancelled')
    BEGIN
        SET @ErrorMsg = 'Invalid status value: ' + @NewStatus;
        RETURN;
    END

    UPDATE dbo.Orders
    SET Status         = @NewStatus,
        ShippedDate    = CASE WHEN @NewStatus = 'Shipped'   THEN CAST(GETDATE() AS DATE) ELSE ShippedDate END,
        TrackingNumber = CASE WHEN @TrackingNumber IS NOT NULL THEN @TrackingNumber ELSE TrackingNumber END
    WHERE OrderId = @OrderId;
END;
GO

-- GetSalesSummary: MODIFIED — joins Categories table instead of using string column
CREATE OR ALTER PROCEDURE dbo.GetSalesSummary
    @StartDate DATE = NULL,
    @EndDate   DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @StartDate = ISNULL(@StartDate, DATEFROMPARTS(YEAR(GETDATE()), 1, 1));
    SET @EndDate   = ISNULL(@EndDate,   CAST(GETDATE() AS DATE));

    SELECT
        cat.Name                         AS Category,
        p.Name                           AS Product,
        p.SKU,
        SUM(i.Quantity)                  AS UnitsSold,
        SUM(i.Quantity * i.UnitPrice)    AS Revenue
    FROM dbo.Orders     o
    JOIN dbo.OrderItems i   ON i.OrderId   = o.OrderId
    JOIN dbo.Products   p   ON p.ProductId = i.ProductId
    JOIN dbo.Categories cat ON cat.CategoryId = p.CategoryId
    WHERE o.Status <> 'Cancelled'
      AND o.OrderDate BETWEEN @StartDate AND @EndDate
    GROUP BY cat.CategoryId, cat.Name, p.ProductId, p.Name, p.SKU
    ORDER BY cat.Name, Revenue DESC;
END;
GO

-- SearchCustomers: DELETED (removed in v2 — functionality moved to application layer)

-- GetCustomerAddresses: NEW
CREATE OR ALTER PROCEDURE dbo.GetCustomerAddresses
    @CustomerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.AddressId,
        a.Label,
        a.Street,
        a.City,
        a.State,
        a.PostalCode,
        a.IsDefault
    FROM dbo.Addresses a
    WHERE a.CustomerId = @CustomerId
    ORDER BY a.IsDefault DESC, a.Label;
END;
GO

-- AddProductReview: NEW
CREATE OR ALTER PROCEDURE dbo.AddProductReview
    @ProductId  INT,
    @CustomerId INT,
    @Rating     TINYINT,
    @Title      NVARCHAR(100)  = NULL,
    @Body       NVARCHAR(2000) = NULL,
    @ErrorMsg   NVARCHAR(200)  OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @ErrorMsg = NULL;

    IF @Rating NOT BETWEEN 1 AND 5
    BEGIN
        SET @ErrorMsg = 'Rating must be between 1 and 5.';
        RETURN;
    END

    -- One review per customer per product
    IF EXISTS (SELECT 1 FROM dbo.Reviews WHERE ProductId = @ProductId AND CustomerId = @CustomerId)
    BEGIN
        SET @ErrorMsg = 'Customer has already reviewed this product.';
        RETURN;
    END

    INSERT INTO dbo.Reviews (ProductId, CustomerId, Rating, Title, Body, ReviewDate)
    VALUES (@ProductId, @CustomerId, @Rating, @Title, @Body, CAST(GETDATE() AS DATE));
END;
GO

-- GetProductsByCategory: NEW
CREATE OR ALTER PROCEDURE dbo.GetProductsByCategory
    @CategoryName NVARCHAR(50),
    @ActiveOnly   BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.SKU,
        p.Name,
        p.Price,
        p.StockQty,
        ISNULL(AVG(CAST(r.Rating AS DECIMAL(3,1))), NULL) AS AvgRating,
        COUNT(r.ReviewId)                                  AS ReviewCount
    FROM dbo.Products   p
    JOIN dbo.Categories c ON c.CategoryId = p.CategoryId
    LEFT JOIN dbo.Reviews r ON r.ProductId = p.ProductId
    WHERE c.Name = @CategoryName
      AND (@ActiveOnly = 0 OR p.IsActive = 1)
    GROUP BY p.ProductId, p.SKU, p.Name, p.Price, p.StockQty
    ORDER BY p.Name;
END;
GO

-- ── Sample procedure calls ─────────────────────────────────────────────────────

-- Order history for Alice
EXEC dbo.GetOrdersByCustomer @CustomerId = 1;

-- Place a new order using SKU
DECLARE @NewId INT, @Err NVARCHAR(200);
EXEC dbo.PlaceOrder
    @CustomerId = 2,
    @SKU        = 'OF-001',
    @Quantity   = 1,
    @NewOrderId = @NewId OUTPUT,
    @ErrorMsg   = @Err  OUTPUT;
SELECT @NewId AS NewOrderId, @Err AS ErrorMessage;

-- Ship order 5 with a tracking number
DECLARE @Err2 NVARCHAR(200);
EXEC dbo.UpdateOrderStatus
    @OrderId        = 5,
    @NewStatus      = 'Shipped',
    @TrackingNumber = 'TRK-99001',
    @ErrorMsg       = @Err2 OUTPUT;
SELECT @Err2 AS ErrorMessage;

-- Sales summary for 2025
EXEC dbo.GetSalesSummary @StartDate = '2025-01-01', @EndDate = '2025-12-31';

-- Addresses for Bob
EXEC dbo.GetCustomerAddresses @CustomerId = 2;

-- Add a review
DECLARE @Err3 NVARCHAR(200);
EXEC dbo.AddProductReview
    @ProductId  = 2,
    @CustomerId = 4,
    @Rating     = 4,
    @Title      = 'Good hub',
    @Body       = 'Works great with my laptop.',
    @ErrorMsg   = @Err3 OUTPUT;
SELECT @Err3 AS ErrorMessage;

-- Electronics products with ratings
EXEC dbo.GetProductsByCategory @CategoryName = 'Electronics', @ActiveOnly = 1;

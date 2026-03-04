-- Sample database: simple e-commerce schema
-- Compatible with SQL Server (T-SQL)

-- Create database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SampleStore1')
    CREATE DATABASE SampleStore1;
GO

USE SampleStore1;
GO

-- Customers
IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE dbo.Customers;
CREATE TABLE dbo.Customers (
    CustomerId   INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
    FirstName    NVARCHAR(50)  NOT NULL,
    LastName     NVARCHAR(50)  NOT NULL,
    Email        NVARCHAR(100) NOT NULL UNIQUE,
    City         NVARCHAR(50)  NULL,
    CreatedDate  DATE          NOT NULL DEFAULT GETDATE()
);

-- Products
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL DROP TABLE dbo.Products;
CREATE TABLE dbo.Products (
    ProductId    INT            NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Name         NVARCHAR(100)  NOT NULL,
    Category     NVARCHAR(50)   NOT NULL,
    Price        DECIMAL(10,2)  NOT NULL,
    StockQty     INT            NOT NULL DEFAULT 0
);

-- Orders
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL DROP TABLE dbo.Orders;
CREATE TABLE dbo.Orders (
    OrderId      INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
    CustomerId   INT           NOT NULL REFERENCES dbo.Customers(CustomerId),
    OrderDate    DATE          NOT NULL,
    Status       NVARCHAR(20)  NOT NULL DEFAULT 'Pending'  -- Pending, Shipped, Delivered, Cancelled
);

-- Order line items
IF OBJECT_ID('dbo.OrderItems', 'U') IS NOT NULL DROP TABLE dbo.OrderItems;
CREATE TABLE dbo.OrderItems (
    OrderItemId  INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
    OrderId      INT           NOT NULL REFERENCES dbo.Orders(OrderId),
    ProductId    INT           NOT NULL REFERENCES dbo.Products(ProductId),
    Quantity     INT           NOT NULL,
    UnitPrice    DECIMAL(10,2) NOT NULL  -- price at time of purchase
);
GO

-- ── Seed data ──────────────────────────────────────────────────────────────────

INSERT INTO dbo.Customers (FirstName, LastName, Email, City, CreatedDate) VALUES
    ('Alice',   'Johnson',  'alice@example.com',   'Boston',      '2024-01-15'),
    ('Bob',     'Smith',    'bob@example.com',     'Chicago',     '2024-02-03'),
    ('Carol',   'Williams', 'carol@example.com',   'Denver',      '2024-03-20'),
    ('David',   'Brown',    'david@example.com',   'Boston',      '2024-04-08'),
    ('Eve',     'Davis',    'eve@example.com',     'Phoenix',     '2024-05-12'),
    ('Frank',   'Miller',   'frank@example.com',   'Chicago',     '2025-01-07');

INSERT INTO dbo.Products (Name, Category, Price, StockQty) VALUES
    ('Wireless Mouse',       'Electronics',  29.99,  150),
    ('USB-C Hub',            'Electronics',  49.99,   80),
    ('Mechanical Keyboard',  'Electronics', 119.99,   45),
    ('Desk Lamp',            'Office',       34.99,   60),
    ('Notebook (3-pack)',    'Office',        8.99,  300),
    ('Standing Desk Mat',    'Office',       54.99,   35),
    ('Coffee Mug',           'Kitchen',      14.99,  200),
    ('Reusable Water Bottle','Kitchen',      22.99,  175);

INSERT INTO dbo.Orders (CustomerId, OrderDate, Status) VALUES
    (1, '2025-01-10', 'Delivered'),   -- OrderId 1  Alice
    (2, '2025-01-14', 'Delivered'),   -- OrderId 2  Bob
    (1, '2025-02-03', 'Shipped'),     -- OrderId 3  Alice
    (3, '2025-02-18', 'Delivered'),   -- OrderId 4  Carol
    (4, '2025-03-01', 'Pending'),     -- OrderId 5  David
    (2, '2025-03-05', 'Cancelled'),   -- OrderId 6  Bob
    (5, '2025-03-10', 'Shipped'),     -- OrderId 7  Eve
    (6, '2025-03-11', 'Pending');     -- OrderId 8  Frank

INSERT INTO dbo.OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES
    -- Order 1: Alice bought a mouse and mug
    (1, 1,  1,  29.99),
    (1, 7,  2,  14.99),
    -- Order 2: Bob bought a keyboard
    (2, 3,  1, 119.99),
    -- Order 3: Alice bought a hub and lamp
    (3, 2,  1,  49.99),
    (3, 4,  1,  34.99),
    -- Order 4: Carol bought notebooks and a mat
    (4, 5,  4,   8.99),
    (4, 6,  1,  54.99),
    -- Order 5: David bought a mouse and hub
    (5, 1,  2,  29.99),
    (5, 2,  1,  49.99),
    -- Order 6: Bob's cancelled order (keyboard + lamp)
    (6, 3,  1, 119.99),
    (6, 4,  1,  34.99),
    -- Order 7: Eve bought a water bottle and mug
    (7, 8,  1,  22.99),
    (7, 7,  1,  14.99),
    -- Order 8: Frank bought notebooks
    (8, 5,  2,   8.99);
GO

-- ── Stored Procedures ─────────────────────────────────────────────────────────

-- GetOrdersByCustomer: full order history for one customer
CREATE OR ALTER PROCEDURE dbo.GetOrdersByCustomer
    @CustomerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        o.OrderId,
        o.OrderDate,
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

-- PlaceOrder: create an order and decrement stock in a transaction
-- Returns the new OrderId via OUTPUT parameter.
-- Returns -1 and an error message if any item is out of stock.
CREATE OR ALTER PROCEDURE dbo.PlaceOrder
    @CustomerId  INT,
    @ProductId   INT,
    @Quantity    INT,
    @NewOrderId  INT OUTPUT,
    @ErrorMsg    NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @NewOrderId = -1;
    SET @ErrorMsg   = NULL;

    -- Check stock
    DECLARE @Stock INT;
    SELECT @Stock = StockQty FROM dbo.Products WHERE ProductId = @ProductId;

    IF @Stock IS NULL
    BEGIN
        SET @ErrorMsg = 'Product not found.';
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

        DECLARE @Price DECIMAL(10,2);
        SELECT @Price = Price FROM dbo.Products WHERE ProductId = @ProductId;

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

-- UpdateOrderStatus: advance an order to a new status with validation
CREATE OR ALTER PROCEDURE dbo.UpdateOrderStatus
    @OrderId    INT,
    @NewStatus  NVARCHAR(20),   -- Pending | Shipped | Delivered | Cancelled
    @ErrorMsg   NVARCHAR(200) OUTPUT
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

    -- Simple state-machine: Delivered and Cancelled are terminal
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

    UPDATE dbo.Orders SET Status = @NewStatus WHERE OrderId = @OrderId;
END;
GO

-- GetSalesSummary: revenue by category and product for a date range
CREATE OR ALTER PROCEDURE dbo.GetSalesSummary
    @StartDate DATE = NULL,
    @EndDate   DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Default to current calendar year if not supplied
    SET @StartDate = ISNULL(@StartDate, DATEFROMPARTS(YEAR(GETDATE()), 1, 1));
    SET @EndDate   = ISNULL(@EndDate,   CAST(GETDATE() AS DATE));

    SELECT
        p.Category,
        p.Name                           AS Product,
        SUM(i.Quantity)                  AS UnitsSold,
        SUM(i.Quantity * i.UnitPrice)    AS Revenue
    FROM dbo.Orders     o
    JOIN dbo.OrderItems i ON i.OrderId   = o.OrderId
    JOIN dbo.Products   p ON p.ProductId = i.ProductId
    WHERE o.Status <> 'Cancelled'
      AND o.OrderDate BETWEEN @StartDate AND @EndDate
    GROUP BY p.Category, p.ProductId, p.Name
    ORDER BY p.Category, Revenue DESC;
END;
GO

-- SearchCustomers: find customers by name fragment or city
CREATE OR ALTER PROCEDURE dbo.SearchCustomers
    @SearchTerm NVARCHAR(100) = NULL,
    @City       NVARCHAR(50)  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.CustomerId,
        c.FirstName + ' ' + c.LastName  AS FullName,
        c.Email,
        c.City,
        c.CreatedDate,
        COUNT(o.OrderId)                AS TotalOrders,
        ISNULL(SUM(i.Quantity * i.UnitPrice), 0) AS LifetimeSpend
    FROM dbo.Customers  c
    LEFT JOIN dbo.Orders     o ON o.CustomerId = c.CustomerId AND o.Status <> 'Cancelled'
    LEFT JOIN dbo.OrderItems i ON i.OrderId    = o.OrderId
    WHERE (@SearchTerm IS NULL OR c.FirstName + ' ' + c.LastName LIKE '%' + @SearchTerm + '%')
      AND (@City       IS NULL OR c.City = @City)
    GROUP BY c.CustomerId, c.FirstName, c.LastName, c.Email, c.City, c.CreatedDate
    ORDER BY c.LastName, c.FirstName;
END;
GO

-- ── Sample procedure calls ─────────────────────────────────────────────────────

-- Order history for Alice (CustomerId 1)
EXEC dbo.GetOrdersByCustomer @CustomerId = 1;

-- Place a new order (Bob buys a desk lamp)
DECLARE @NewId INT, @Err NVARCHAR(200);
EXEC dbo.PlaceOrder
    @CustomerId = 2,
    @ProductId  = 4,
    @Quantity   = 1,
    @NewOrderId = @NewId OUTPUT,
    @ErrorMsg   = @Err  OUTPUT;
SELECT @NewId AS NewOrderId, @Err AS ErrorMessage;

-- Ship order 5
DECLARE @Err2 NVARCHAR(200);
EXEC dbo.UpdateOrderStatus @OrderId = 5, @NewStatus = 'Shipped', @ErrorMsg = @Err2 OUTPUT;
SELECT @Err2 AS ErrorMessage;

-- Sales summary for all of 2025
EXEC dbo.GetSalesSummary @StartDate = '2025-01-01', @EndDate = '2025-12-31';

-- Find customers in Chicago
EXEC dbo.SearchCustomers @City = 'Chicago';

-- ── Handy starter queries ──────────────────────────────────────────────────────

-- All orders with customer name and total
SELECT
    o.OrderId,
    c.FirstName + ' ' + c.LastName  AS Customer,
    o.OrderDate,
    o.Status,
    SUM(i.Quantity * i.UnitPrice)   AS OrderTotal
FROM dbo.Orders        o
JOIN dbo.Customers     c ON c.CustomerId = o.CustomerId
JOIN dbo.OrderItems    i ON i.OrderId    = o.OrderId
GROUP BY o.OrderId, c.FirstName, c.LastName, o.OrderDate, o.Status
ORDER BY o.OrderDate;

-- Revenue by customer (delivered orders only)
SELECT
    c.FirstName + ' ' + c.LastName  AS Customer,
    COUNT(DISTINCT o.OrderId)       AS Orders,
    SUM(i.Quantity * i.UnitPrice)   AS TotalSpent
FROM dbo.Customers  c
JOIN dbo.Orders     o ON o.CustomerId = c.CustomerId
JOIN dbo.OrderItems i ON i.OrderId    = o.OrderId
WHERE o.Status = 'Delivered'
GROUP BY c.CustomerId, c.FirstName, c.LastName
ORDER BY TotalSpent DESC;

-- Best-selling products by quantity
SELECT
    p.Name,
    p.Category,
    SUM(i.Quantity)              AS UnitsSold,
    SUM(i.Quantity * i.UnitPrice) AS Revenue
FROM dbo.Products   p
JOIN dbo.OrderItems i ON i.ProductId = p.ProductId
JOIN dbo.Orders     o ON o.OrderId   = i.OrderId
WHERE o.Status <> 'Cancelled'
GROUP BY p.ProductId, p.Name, p.Category
ORDER BY UnitsSold DESC;

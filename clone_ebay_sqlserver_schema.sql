-- ============================================================================
-- DB SCHEMA: CloneEbayDB (SQL Server)
-- Group 2 - PRN232 eBay Clone Seller Subsystem Project
-- Created Date: 2026-08-21
-- Tables: 17 core tables + 1 denormalized summary table
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CloneEbayDB')
BEGIN
    CREATE DATABASE CloneEbayDB;
END
GO

USE CloneEbayDB;
GO

-- ----------------------------------------------------------------------------
-- 1. Table: [User]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.User', 'U') IS NOT NULL DROP TABLE dbo.[User];
CREATE TABLE dbo.[User] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Username] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(150) NOT NULL,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [Role] NVARCHAR(20) NOT NULL CONSTRAINT DF_User_Role DEFAULT ('Buyer'), -- Buyer, Seller, Admin
    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_User_CreatedAt DEFAULT (GETUTCDATE()),
    CONSTRAINT PK_User PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT UQ_User_Email UNIQUE ([Email]),
    CONSTRAINT UQ_User_Username UNIQUE ([Username])
);

-- ----------------------------------------------------------------------------
-- 2. Table: [Store]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Store', 'U') IS NOT NULL DROP TABLE dbo.Store;
CREATE TABLE dbo.[Store] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [SellerId] INT NOT NULL,
    [StoreName] NVARCHAR(150) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [BannerImageURL] NVARCHAR(500) NULL,
    [SellerType] NVARCHAR(20) NOT NULL CONSTRAINT DF_Store_SellerType DEFAULT ('Individual'), -- Individual, Business
    [LegalName] NVARCHAR(150) NULL,
    [Phone] NVARCHAR(20) NULL,
    [VerificationStatus] NVARCHAR(20) NOT NULL CONSTRAINT DF_Store_VerificationStatus DEFAULT ('Pending'), -- Pending, Approved, Rejected
    [RejectionReason] NVARCHAR(500) NULL,
    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_Store_CreatedAt DEFAULT (GETUTCDATE()),
    [UpdatedAt] DATETIME2 NULL,
    CONSTRAINT PK_Store PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_Store_User FOREIGN KEY ([SellerId]) REFERENCES dbo.[User] ([Id]),
    CONSTRAINT UQ_Store_SellerId UNIQUE ([SellerId])
);

-- ----------------------------------------------------------------------------
-- 3. Table: [Address]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Address', 'U') IS NOT NULL DROP TABLE dbo.Address;
CREATE TABLE dbo.[Address] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL,
    [RecipientName] NVARCHAR(100) NOT NULL,
    [Phone] NVARCHAR(20) NOT NULL,
    [Street] NVARCHAR(255) NOT NULL,
    [City] NVARCHAR(100) NOT NULL,
    [State] NVARCHAR(100) NULL,
    [ZipCode] NVARCHAR(20) NULL,
    [Country] NVARCHAR(100) NOT NULL CONSTRAINT DF_Address_Country DEFAULT ('Vietnam'),
    [IsDefault] BIT NOT NULL CONSTRAINT DF_Address_IsDefault DEFAULT (0),
    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_Address_CreatedAt DEFAULT (GETUTCDATE()),
    CONSTRAINT PK_Address PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_Address_User FOREIGN KEY ([UserId]) REFERENCES dbo.[User] ([Id]) ON DELETE CASCADE
);

-- ----------------------------------------------------------------------------
-- 4. Table: [Category]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Category', 'U') IS NOT NULL DROP TABLE dbo.Category;
CREATE TABLE dbo.[Category] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [ParentId] INT NULL,
    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_Category_CreatedAt DEFAULT (GETUTCDATE()),
    CONSTRAINT PK_Category PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_Category_Parent FOREIGN KEY ([ParentId]) REFERENCES dbo.[Category] ([Id])
);

-- ----------------------------------------------------------------------------
-- 5. Table: [Product]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Product', 'U') IS NOT NULL DROP TABLE dbo.Product;
CREATE TABLE dbo.[Product] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [SellerId] INT NOT NULL,
    [CategoryId] INT NOT NULL,
    [Title] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [Price] DECIMAL(18,2) NOT NULL,
    [IsAuction] BIT NOT NULL CONSTRAINT DF_Product_IsAuction DEFAULT (0),
    [AuctionEndTime] DATETIME2 NULL,
    [Images] NVARCHAR(MAX) NULL, -- JSON String array of Image URLs
    [Status] NVARCHAR(20) NOT NULL CONSTRAINT DF_Product_Status DEFAULT ('Active'), -- Active, Hidden, OutOfStock, AuctionEnded
    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_Product_CreatedAt DEFAULT (GETUTCDATE()),
    [UpdatedAt] DATETIME2 NULL,
    CONSTRAINT PK_Product PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_Product_Seller FOREIGN KEY ([SellerId]) REFERENCES dbo.[User] ([Id]),
    CONSTRAINT FK_Product_Category FOREIGN KEY ([CategoryId]) REFERENCES dbo.[Category] ([Id])
);

-- ----------------------------------------------------------------------------
-- 6. Table: [Inventory]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Inventory', 'U') IS NOT NULL DROP TABLE dbo.Inventory;
CREATE TABLE dbo.[Inventory] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [ProductId] INT NOT NULL,
    [Quantity] INT NOT NULL CONSTRAINT DF_Inventory_Quantity DEFAULT (0),
    [LastUpdated] DATETIME2 NOT NULL CONSTRAINT DF_Inventory_LastUpdated DEFAULT (GETUTCDATE()),
    CONSTRAINT PK_Inventory PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_Inventory_Product FOREIGN KEY ([ProductId]) REFERENCES dbo.[Product] ([Id]) ON DELETE CASCADE,
    CONSTRAINT UQ_Inventory_ProductId UNIQUE ([ProductId])
);

-- ----------------------------------------------------------------------------
-- 7. Table: [Coupon]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Coupon', 'U') IS NOT NULL DROP TABLE dbo.Coupon;
CREATE TABLE dbo.[Coupon] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [ProductId] INT NOT NULL,
    [SellerId] INT NOT NULL,
    [Code] NVARCHAR(50) NOT NULL,
    [DiscountPercent] DECIMAL(5,2) NOT NULL,
    [StartDate] DATETIME2 NOT NULL,
    [EndDate] DATETIME2 NOT NULL,
    [MaxUsage] INT NOT NULL CONSTRAINT DF_Coupon_MaxUsage DEFAULT (1),
    [UsedCount] INT NOT NULL CONSTRAINT DF_Coupon_UsedCount DEFAULT (0),
    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_Coupon_CreatedAt DEFAULT (GETUTCDATE()),
    CONSTRAINT PK_Coupon PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_Coupon_Product FOREIGN KEY ([ProductId]) REFERENCES dbo.[Product] ([Id]),
    CONSTRAINT FK_Coupon_Seller FOREIGN KEY ([SellerId]) REFERENCES dbo.[User] ([Id]),
    CONSTRAINT UQ_Coupon_Code UNIQUE ([Code])
);

-- ----------------------------------------------------------------------------
-- 8. Table: [OrderTable]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.OrderTable', 'U') IS NOT NULL DROP TABLE dbo.OrderTable;
CREATE TABLE dbo.[OrderTable] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [BuyerId] INT NOT NULL,
    [OrderDate] DATETIME2 NOT NULL CONSTRAINT DF_OrderTable_OrderDate DEFAULT (GETUTCDATE()),
    [TotalPrice] DECIMAL(18,2) NOT NULL,
    [Status] NVARCHAR(20) NOT NULL CONSTRAINT DF_OrderTable_Status DEFAULT ('Pending'), -- Pending, Confirmed, Shipped, Delivered, Cancelled
    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_OrderTable_CreatedAt DEFAULT (GETUTCDATE()),
    [UpdatedAt] DATETIME2 NULL,
    CONSTRAINT PK_OrderTable PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_OrderTable_Buyer FOREIGN KEY ([BuyerId]) REFERENCES dbo.[User] ([Id])
);

-- ----------------------------------------------------------------------------
-- 9. Table: [OrderItem]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.OrderItem', 'U') IS NOT NULL DROP TABLE dbo.OrderItem;
CREATE TABLE dbo.[OrderItem] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [OrderId] INT NOT NULL,
    [ProductId] INT NOT NULL,
    [CouponId] INT NULL,
    [Quantity] INT NOT NULL CONSTRAINT DF_OrderItem_Quantity DEFAULT (1),
    [UnitPrice] DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_OrderItem PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_OrderItem_Order FOREIGN KEY ([OrderId]) REFERENCES dbo.[OrderTable] ([Id]) ON DELETE CASCADE,
    CONSTRAINT FK_OrderItem_Product FOREIGN KEY ([ProductId]) REFERENCES dbo.[Product] ([Id]),
    CONSTRAINT FK_OrderItem_Coupon FOREIGN KEY ([CouponId]) REFERENCES dbo.[Coupon] ([Id])
);

-- ----------------------------------------------------------------------------
-- 10. Table: [Payment]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Payment', 'U') IS NOT NULL DROP TABLE dbo.Payment;
CREATE TABLE dbo.[Payment] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [OrderId] INT NOT NULL,
    [Amount] DECIMAL(18,2) NOT NULL,
    [Method] NVARCHAR(50) NOT NULL, -- VNPay, CreditCard, PayPal, COD
    [Status] NVARCHAR(20) NOT NULL CONSTRAINT DF_Payment_Status DEFAULT ('Pending'), -- Pending, Completed, Failed, Refunded
    [PaidAt] DATETIME2 NULL,
    CONSTRAINT PK_Payment PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_Payment_Order FOREIGN KEY ([OrderId]) REFERENCES dbo.[OrderTable] ([Id]) ON DELETE CASCADE,
    CONSTRAINT UQ_Payment_OrderId UNIQUE ([OrderId])
);

-- ----------------------------------------------------------------------------
-- 11. Table: [ShippingInfo]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.ShippingInfo', 'U') IS NOT NULL DROP TABLE dbo.ShippingInfo;
CREATE TABLE dbo.[ShippingInfo] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [OrderId] INT NOT NULL,
    [Carrier] NVARCHAR(50) NULL, -- GHN, GHTK, ViettelPost
    [TrackingNumber] NVARCHAR(100) NULL,
    [Status] NVARCHAR(30) NOT NULL CONSTRAINT DF_ShippingInfo_Status DEFAULT ('Preparing'), -- Preparing, LabelCreated, HandedToCarrier, InTransit, Delivered
    [EstimatedArrival] DATETIME2 NULL,
    [ShippedAt] DATETIME2 NULL,
    [DeliveredAt] DATETIME2 NULL,
    CONSTRAINT PK_ShippingInfo PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_ShippingInfo_Order FOREIGN KEY ([OrderId]) REFERENCES dbo.[OrderTable] ([Id]) ON DELETE CASCADE,
    CONSTRAINT UQ_ShippingInfo_OrderId UNIQUE ([OrderId])
);

-- ----------------------------------------------------------------------------
-- 12. Table: [ReturnRequest]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.ReturnRequest', 'U') IS NOT NULL DROP TABLE dbo.ReturnRequest;
CREATE TABLE dbo.[ReturnRequest] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [OrderId] INT NOT NULL,
    [Reason] NVARCHAR(500) NOT NULL,
    [Status] NVARCHAR(30) NOT NULL CONSTRAINT DF_ReturnRequest_Status DEFAULT ('Requested'), -- Requested, Accepted, RefundOffered, Declined, RefundedByReturn, Closed
    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_ReturnRequest_CreatedAt DEFAULT (GETUTCDATE()),
    [UpdatedAt] DATETIME2 NULL,
    CONSTRAINT PK_ReturnRequest PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_ReturnRequest_Order FOREIGN KEY ([OrderId]) REFERENCES dbo.[OrderTable] ([Id])
);

-- ----------------------------------------------------------------------------
-- 13. Table: [Bid]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Bid', 'U') IS NOT NULL DROP TABLE dbo.Bid;
CREATE TABLE dbo.[Bid] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [ProductId] INT NOT NULL,
    [BidderId] INT NOT NULL,
    [Amount] DECIMAL(18,2) NOT NULL,
    [BidTime] DATETIME2 NOT NULL CONSTRAINT DF_Bid_BidTime DEFAULT (GETUTCDATE()),
    CONSTRAINT PK_Bid PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_Bid_Product FOREIGN KEY ([ProductId]) REFERENCES dbo.[Product] ([Id]),
    CONSTRAINT FK_Bid_Bidder FOREIGN KEY ([BidderId]) REFERENCES dbo.[User] ([Id])
);

-- ----------------------------------------------------------------------------
-- 14. Table: [Review]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Review', 'U') IS NOT NULL DROP TABLE dbo.Review;
CREATE TABLE dbo.[Review] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [ProductId] INT NOT NULL,
    [BuyerId] INT NOT NULL,
    [Rating] INT NOT NULL,
    [Comment] NVARCHAR(1000) NULL,
    [Response] NVARCHAR(1000) NULL, -- Seller reply (max 1 reply)
    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_Review_CreatedAt DEFAULT (GETUTCDATE()),
    CONSTRAINT PK_Review PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_Review_Product FOREIGN KEY ([ProductId]) REFERENCES dbo.[Product] ([Id]),
    CONSTRAINT FK_Review_Buyer FOREIGN KEY ([BuyerId]) REFERENCES dbo.[User] ([Id]),
    CONSTRAINT CK_Review_Rating CHECK (Rating >= 1 AND Rating <= 5)
);

-- ----------------------------------------------------------------------------
-- 15. Table: [Feedback]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Feedback', 'U') IS NOT NULL DROP TABLE dbo.Feedback;
CREATE TABLE dbo.[Feedback] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [SellerId] INT NOT NULL,
    [AverageRating] DECIMAL(3,2) NOT NULL CONSTRAINT DF_Feedback_AverageRating DEFAULT (0),
    [TotalReviews] INT NOT NULL CONSTRAINT DF_Feedback_TotalReviews DEFAULT (0),
    [PositiveRate] DECIMAL(5,2) NOT NULL CONSTRAINT DF_Feedback_PositiveRate DEFAULT (0),
    [LastUpdated] DATETIME2 NOT NULL CONSTRAINT DF_Feedback_LastUpdated DEFAULT (GETUTCDATE()),
    CONSTRAINT PK_Feedback PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_Feedback_Seller FOREIGN KEY ([SellerId]) REFERENCES dbo.[User] ([Id]),
    CONSTRAINT UQ_Feedback_SellerId UNIQUE ([SellerId])
);

-- ----------------------------------------------------------------------------
-- 16. Table: [Dispute]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Dispute', 'U') IS NOT NULL DROP TABLE dbo.Dispute;
CREATE TABLE dbo.[Dispute] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [OrderId] INT NOT NULL,
    [ReturnRequestId] INT NULL,
    [BuyerId] INT NOT NULL,
    [SellerId] INT NOT NULL,
    [Reason] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [Status] NVARCHAR(30) NOT NULL CONSTRAINT DF_Dispute_Status DEFAULT ('Open'), -- Open, UnderReview, Resolved
    [Resolution] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME2 NOT NULL CONSTRAINT DF_Dispute_CreatedAt DEFAULT (GETUTCDATE()),
    [UpdatedAt] DATETIME2 NULL,
    CONSTRAINT PK_Dispute PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_Dispute_Order FOREIGN KEY ([OrderId]) REFERENCES dbo.[OrderTable] ([Id]),
    CONSTRAINT FK_Dispute_ReturnRequest FOREIGN KEY ([ReturnRequestId]) REFERENCES dbo.[ReturnRequest] ([Id]),
    CONSTRAINT FK_Dispute_Buyer FOREIGN KEY ([BuyerId]) REFERENCES dbo.[User] ([Id]),
    CONSTRAINT FK_Dispute_Seller FOREIGN KEY ([SellerId]) REFERENCES dbo.[User] ([Id])
);

-- ----------------------------------------------------------------------------
-- 17. Table: [Message]
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Message', 'U') IS NOT NULL DROP TABLE dbo.Message;
CREATE TABLE dbo.[Message] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [SenderId] INT NOT NULL,
    [ReceiverId] INT NOT NULL,
    [Content] NVARCHAR(2000) NOT NULL,
    [IsRead] BIT NOT NULL CONSTRAINT DF_Message_IsRead DEFAULT (0),
    [Timestamp] DATETIME2 NOT NULL CONSTRAINT DF_Message_Timestamp DEFAULT (GETUTCDATE()),
    CONSTRAINT PK_Message PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_Message_Sender FOREIGN KEY ([SenderId]) REFERENCES dbo.[User] ([Id]),
    CONSTRAINT FK_Message_Receiver FOREIGN KEY ([ReceiverId]) REFERENCES dbo.[User] ([Id])
);

-- ----------------------------------------------------------------------------
-- 18. Table: [SalesSummary] (Denormalized Table for Module 6 Dashboard performance)
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.SalesSummary', 'U') IS NOT NULL DROP TABLE dbo.SalesSummary;
CREATE TABLE dbo.[SalesSummary] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [SellerId] INT NOT NULL,
    [Period] NVARCHAR(10) NOT NULL, -- 'week', 'month'
    [TotalRevenue] DECIMAL(18,2) NOT NULL CONSTRAINT DF_SalesSummary_TotalRevenue DEFAULT (0),
    [TotalOrders] INT NOT NULL CONSTRAINT DF_SalesSummary_TotalOrders DEFAULT (0),
    [AverageOrderValue] DECIMAL(18,2) NOT NULL CONSTRAINT DF_SalesSummary_AverageOrderValue DEFAULT (0),
    [LastUpdated] DATETIME2 NOT NULL CONSTRAINT DF_SalesSummary_LastUpdated DEFAULT (GETUTCDATE()),
    CONSTRAINT PK_SalesSummary PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_SalesSummary_Seller FOREIGN KEY ([SellerId]) REFERENCES dbo.[User] ([Id])
);
GO

-- ============================================================================
-- INDEXES FOR PERFORMANCE OPTIMIZATION
-- ============================================================================
CREATE INDEX IX_Product_SellerId ON dbo.Product([SellerId]);
CREATE INDEX IX_Product_CategoryId ON dbo.Product([CategoryId]);
CREATE INDEX IX_Coupon_ProductId ON dbo.Coupon([ProductId]);
CREATE INDEX IX_OrderItem_OrderId ON dbo.OrderItem([OrderId]);
CREATE INDEX IX_OrderItem_ProductId ON dbo.OrderItem([ProductId]);
CREATE INDEX IX_OrderTable_BuyerId ON dbo.OrderTable([BuyerId]);
CREATE INDEX IX_Bid_ProductId ON dbo.Bid([ProductId]);
CREATE INDEX IX_Review_ProductId ON dbo.Review([ProductId]);
CREATE INDEX IX_Message_Sender_Receiver ON dbo.Message([SenderId], [ReceiverId]);
GO

-- ============================================================================
-- INITIAL SEED DATA FOR TESTING
-- ============================================================================
-- Insert Admin, Seller, Buyer accounts
-- Password for all test users: Passw0rd! (hashed string mock placeholder)
INSERT INTO dbo.[User] ([Username], [Email], [PasswordHash], [Role]) VALUES 
('admin', 'admin@ebayclone.com', '$2a$11$e.f1GvQ1sZ4d/j0N8l4W7eYw8E9Jz8M9c0e1f2g3h4i5j6k7l8m9', 'Admin'),
('seller1', 'seller01@example.com', '$2a$11$e.f1GvQ1sZ4d/j0N8l4W7eYw8E9Jz8M9c0e1f2g3h4i5j6k7l8m9', 'Seller'),
('buyer1', 'buyer01@example.com', '$2a$11$e.f1GvQ1sZ4d/j0N8l4W7eYw8E9Jz8M9c0e1f2g3h4i5j6k7l8m9', 'Buyer');

-- Insert Approved Store for seller1 (UserId = 2)
INSERT INTO dbo.[Store] ([SellerId], [StoreName], [Description], [SellerType], [LegalName], [Phone], [VerificationStatus]) VALUES 
(2, 'Hieu''s Tech Store', 'Chuyên cung cấp phụ kiện công nghệ và điện thoại chính hãng', 'Individual', 'Pham Trung Hieu', '0901234567', 'Approved');

-- Insert Categories
INSERT INTO dbo.[Category] ([Name], [Description], [ParentId]) VALUES 
('Electronics', 'Thiết bị điện tử', NULL),
('Smartphones', 'Điện thoại thông minh', 1),
('Laptops', 'Máy tính xách tay', 1),
('Fashion', 'Thời trang & Phụ kiện', NULL);

GO

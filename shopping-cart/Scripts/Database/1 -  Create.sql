-- Shopping Cart Database Creation
USE master
GO

IF DB_ID('ShoppingCartDb') IS NULL
    BEGIN
        CREATE DATABASE [ShoppingCartDb]
    END

GO
USE [ShoppingCartDb]
GO

IF OBJECT_ID('dbo.ShoppingCart') IS NULL
    BEGIN
        CREATE TABLE [dbo].[ShoppingCart]
        (
            Id      INT IDENTITY (1, 1) PRIMARY KEY,
            UserId  BIGINT NOT NULL,
            CONSTRAINT ShoppingCartUnique UNIQUE (Id, UserId)
        )

        CREATE INDEX ShoppingCard_UserId
        ON [dbo].[ShoppingCart] (UserId)
    END

IF OBJECT_ID('dbo.ShoppingCartItem') IS NULL
    BEGIN
        CREATE TABLE [dbo].[ShoppingCartItem]
        (
            Id                  INT IDENTITY (1, 1) PRIMARY KEY,
            ShoppingCartId      INT NOT NULL,
            ProductCatalogId    BIGINT NOT NULL,
            ProductName         NVARCHAR(100) NOT NULL,
            ProductDescription  NVARCHAR(500) NULL,
            Amount              INT NOT NULL,
            Currency            NVARCHAR(5) NOT NULL
        )

        ALTER TABLE dbo.ShoppingCartItem WITH CHECK ADD CONSTRAINT [FK_ShoppingCart]
            FOREIGN KEY (ShoppingCartId) REFERENCES dbo.ShoppingCart (Id)

        ALTER TABLE dbo.ShoppingCartItem CHECK CONSTRAINT [FK_ShoppingCart]

        CREATE INDEX ShoppingCartItem_ShoppingCardId
        ON dbo.ShoppingCartItem (ShoppingCartId)
    END
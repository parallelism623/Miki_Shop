IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [ExpireAt] datetime2 NULL,
    [RefreshTk] nvarchar(max) NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Category] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(20) NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ProductSize] (
    [Id] int NOT NULL IDENTITY,
    [Size] int NULL,
    CONSTRAINT [PK_ProductSize] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AppRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AppRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AppRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AppUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AppUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AppUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AppUserLogins] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(max) NULL,
    [ProviderKey] nvarchar(max) NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    CONSTRAINT [PK_AppUserLogins] PRIMARY KEY ([UserId]),
    CONSTRAINT [FK_AppUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AppUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AppUserRoles] PRIMARY KEY ([RoleId], [UserId]),
    CONSTRAINT [FK_AppUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AppUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AppUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AppUserTokens] PRIMARY KEY ([UserId]),
    CONSTRAINT [FK_AppUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Orders] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [CreateAt] varchar(20) NULL,
    [Address] varchar(50) NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__ORDERS__USER_ID__412EB0B6] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Product] (
    [ID] varchar(20) NOT NULL,
    [Name] nvarchar(40) NULL,
    [Sale] int NULL,
    [Description] text NULL,
    [CategoryId] int NULL,
    [CreateAt] datetime NULL,
    [MinPrice] int NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY ([ID]),
    CONSTRAINT [FK__PRODUCT__CATEGOR__2CF2ADDF] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id])
);
GO

CREATE TABLE [Order_detail] (
    [OrderId] int NOT NULL,
    [ProductId] varchar(20) NOT NULL,
    [Quantity] int NULL,
    CONSTRAINT [PK_Order_detail] PRIMARY KEY ([ProductId], [OrderId]),
    CONSTRAINT [FK__ORDER_DET__ORDER__32AB8735] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK__ORDER_DET__PRODU__3587F3E0] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ID]) ON DELETE CASCADE
);
GO

CREATE TABLE [ProductImage] (
    [Url] varchar(200) NOT NULL,
    [ProductId] varchar(20) NULL,
    [Index] int NULL,
    CONSTRAINT [PK__PRODUCT___C5B100083B40F9CD] PRIMARY KEY ([Url]),
    CONSTRAINT [FK__PRODUCT_I__PRODU__395884C4] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ID])
);
GO

CREATE TABLE [ProductSizeDetail] (
    [ProductId] varchar(20) NOT NULL,
    [SizeId] int NOT NULL,
    [Quantity] int NULL,
    [Price] int NULL,
    CONSTRAINT [PK__PRODUCT___9420F6C487A5BC55] PRIMARY KEY ([ProductId], [SizeId]),
    CONSTRAINT [FK__PRODUCT_S__PRODU__3D2915A8] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ID]),
    CONSTRAINT [FK__PRODUCT_S__SIZE___3E1D39E1] FOREIGN KEY ([SizeId]) REFERENCES [ProductSize] ([Id])
);
GO

CREATE INDEX [IX_AppRoleClaims_RoleId] ON [AppRoleClaims] ([RoleId]);
GO

CREATE INDEX [IX_AppUserClaims_UserId] ON [AppUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AppUserRoles_UserId] ON [AppUserRoles] ([UserId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_Order_detail_OrderId] ON [Order_detail] ([OrderId]);
GO

CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);
GO

CREATE INDEX [IX_Product_CategoryId] ON [Product] ([CategoryId]);
GO

CREATE INDEX [IX_ProductImage_ProductId] ON [ProductImage] ([ProductId]);
GO

CREATE INDEX [IX_ProductSizeDetail_SizeId] ON [ProductSizeDetail] ([SizeId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240314185727_init', N'6.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [AspNetUsers] ADD [FullName] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240316084302_init1', N'6.0.11');
GO

COMMIT;
GO


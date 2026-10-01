IF DB_ID(N'GestionClientesDB') IS NULL
    CREATE DATABASE GestionClientesDB;
GO
USE GestionClientesDB;
GO

IF OBJECT_ID(N'dbo.Bitacora', N'U') IS NOT NULL DROP TABLE dbo.Bitacora;
IF OBJECT_ID(N'dbo.Clientes', N'U') IS NOT NULL DROP TABLE dbo.Clientes;
IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NOT NULL DROP TABLE dbo.Usuarios;
GO

CREATE TABLE dbo.Usuarios (
    Id              INT IDENTITY(1,1) CONSTRAINT PK_Usuarios PRIMARY KEY,
    NombreUsuario   NVARCHAR(50)  NOT NULL CONSTRAINT UQ_Usuarios_NombreUsuario UNIQUE,
    NombreCompleto  NVARCHAR(100) NOT NULL,
    PasswordHash    VARBINARY(32) NOT NULL,
    PasswordSalt    VARBINARY(16) NOT NULL,
    Activo          BIT           NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT (1),
    FechaCreacion   DATETIME2(0)  NOT NULL CONSTRAINT DF_Usuarios_Fecha DEFAULT (SYSDATETIME())
);
GO

CREATE TABLE dbo.Clientes (
    Id                 INT IDENTITY(1,1) CONSTRAINT PK_Clientes PRIMARY KEY,
    Nombres            NVARCHAR(100) NOT NULL,
    Apellidos          NVARCHAR(100) NOT NULL,
    DUI                CHAR(10)      NOT NULL CONSTRAINT UQ_Clientes_DUI UNIQUE,
    Email              NVARCHAR(150) NULL,
    Telefono           NVARCHAR(20)  NULL,
    Direccion          NVARCHAR(250) NULL,
    FechaCreacion      DATETIME2(0)  NOT NULL CONSTRAINT DF_Clientes_Fecha DEFAULT (SYSDATETIME()),
    FechaModificacion  DATETIME2(0)  NULL
);
GO

-- Sin FK hacia Clientes a propósito: el registro de bitácora debe sobrevivir al ELIMINAR.
CREATE TABLE dbo.Bitacora (
    Id         BIGINT IDENTITY(1,1) CONSTRAINT PK_Bitacora PRIMARY KEY,
    Accion     NVARCHAR(10)  NOT NULL CONSTRAINT CK_Bitacora_Accion CHECK (Accion IN (N'AGREGAR', N'EDITAR', N'ELIMINAR')),
    ClienteId  INT           NOT NULL,
    FechaHora  DATETIME2(0)  NOT NULL CONSTRAINT DF_Bitacora_Fecha DEFAULT (SYSDATETIME()),
    Usuario    NVARCHAR(50)  NOT NULL,
    Detalle    NVARCHAR(MAX) NULL
);
CREATE INDEX IX_Bitacora_FechaHora ON dbo.Bitacora (FechaHora DESC);
GO

-- Usuario inicial: admin / Admin123!
INSERT INTO dbo.Usuarios (NombreUsuario, NombreCompleto, PasswordHash, PasswordSalt)
VALUES (N'admin', N'Administrador',
        0x2E202F1400CD3F40780CDAE6ECD702303573C1543CBAE3B02459FA46F527D573,
        0x8F3A1C5E7B9D2F4A6C8E0B1D3F5A7C9E);

-- Datos de ejemplo (pocos, solo para demostrar)
INSERT INTO dbo.Clientes (Nombres, Apellidos, DUI, Email, Telefono, Direccion) VALUES
 (N'María José', N'López Hernández', '01234567-8', N'maria.lopez@correo.com', N'7000-1111', N'San Salvador'),
 (N'Carlos Alberto', N'Martínez Ruiz', '02345678-9', N'carlos.martinez@correo.com', N'7000-2222', N'Santa Tecla'),
 (N'Ana Sofía', N'Ramírez Castro', '03456789-0', NULL, N'7000-3333', N'Soyapango');
GO

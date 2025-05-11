-- Veritabanını oluştur
CREATE DATABASE BirSiberDB;
GO

USE BirSiberDB;
GO

-- Users tablosu
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    LastLoginAt DATETIME NULL
);
GO

-- Randevular tablosu
CREATE TABLE Appointments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    ServiceType NVARCHAR(50) NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO

-- Hizmetler tablosu
CREATE TABLE Services (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Price DECIMAL(18,2) NOT NULL,
    Duration INT NOT NULL, -- dakika cinsinden
    IsActive BIT NOT NULL DEFAULT 1
);
GO

-- Çalışma Saatleri tablosu
CREATE TABLE WorkingHours (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DayOfWeek INT NOT NULL, -- 0=Pazar, 1=Pazartesi, ...
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1
);
GO 
CREATE TABLE LibraryResources (
    Id INT IDENTITY(1,1) PRIMARY KEY,   -- Auto-incremented ID
    Title NVARCHAR(255) NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    PublicationYear INT NOT NULL,        -- Matches your C# property name
    Category NVARCHAR(100),
    IsAvailable BIT NOT NULL,
    DueDate DATETIME NULL
);

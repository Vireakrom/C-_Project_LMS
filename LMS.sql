USE LibraryA4;

CREATE TABLE Books (
    BookID INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(100),
    Author NVARCHAR(100),
    ISBN NVARCHAR(20),
    Quantity INT,
    Category NVARCHAR(50),
    Status NVARCHAR(20)
);
GO
CREATE TABLE Readers (
    ReaderID INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    RegisterDate DATE
);
GO
CREATE TABLE Transactions (
    TransactionID INT PRIMARY KEY IDENTITY,
    ReaderID INT FOREIGN KEY REFERENCES Readers(ReaderID),
    BookID INT FOREIGN KEY REFERENCES Books(BookID),
    BorrowDate DATE,
    ReturnDate DATE,
    Status NVARCHAR(20)
);

ALTER TABLE Readers
ADD CONSTRAINT DF_RegisterDate
DEFAULT GETDATE() FOR RegisterDate;

ALTER Table Books DROP COLUMN Category;
ALTER TABLE Books DROP COLUMN Status;

ALTER TABLE Books
ADD AddedDate DATE DEFAULT GETDATE(),
    LastRestockedDate DATE DEFAULT GETDATE();

ALTER TABLE Transactions
DROP COLUMN Status;

ALTER TABLE Transactions
ADD IsReturn BIT;	

USE LibraryA4;
EXEC sp_rename 'Books.Quantity', 'TotalQuantity', 'COLUMN';

ALTER TABLE Transactions
ADD DueDate DATE NULL;

ALTER TABLE Readers
ADD IsActive BIT NOT NULL DEFAULT 1;

ALTER TABLE Books
ADD AvailableQuantity INT NULL;

use LibraryA4;
-- Insert a book with all other details. BookID will be auto-generated.
INSERT INTO dbo.Books (Title, Author, ISBN, TotalQuantity, AddedDate, LastRestockedDate, AvailableQuantity)
VALUES ('The Great Gatsby', 'F. Scott Fitzgerald', '978-0743273565', 10, '2023-01-15', '2024-03-10', 8);

-- Insert another book, letting some nullable fields be NULL.
INSERT INTO dbo.Books (Title, Author, ISBN, TotalQuantity, AddedDate, AvailableQuantity)
VALUES ('To Kill a Mockingbird', 'Harper Lee', '978-0061120084', 15, '2022-11-01', 12);

-- Another example, omitting LastRestockedDate
INSERT INTO dbo.Books (Title, Author, ISBN, TotalQuantity, AddedDate, AvailableQuantity)
VALUES ('1984', 'George Orwell', '978-0451524935', 20, '2023-05-20', 18);

-- One more example
INSERT INTO dbo.Books (Title, Author, ISBN, TotalQuantity, AddedDate, LastRestockedDate, AvailableQuantity)
VALUES ('Pride and Prejudice', 'Jane Austen', '978-0141439518', 8, '2021-09-10', '2023-11-25', 5);

INSERT INTO dbo.Books (Title, Author, ISBN, TotalQuantity, AddedDate, LastRestockedDate, AvailableQuantity)
VALUES ('Sense and Sensibility', 'Jane Austen', '978-0141439587', 12, '2022-03-20', '2024-02-15', 10);


INSERT INTO dbo.Readers (FullName, Email, Phone)
VALUES ('Alice Wonderland', 'alice.w@example.com', '555-123-4567');

INSERT INTO dbo.Readers (FullName, Email, Phone)
VALUES ('Bob Thebuilder', 'bob.t@example.com', '555-987-6543');

INSERT INTO dbo.Readers (FullName, Email, Phone)
VALUES ('Charlie Chaplin', 'charlie.c@example.com', '555-555-1111');

use LibraryA4;
select * from Transactions where IsReturn = 1;

USE LibraryA4;
DELETE FROM Books;
DELETE FROM Transactions;
DELETE FROM Readers;
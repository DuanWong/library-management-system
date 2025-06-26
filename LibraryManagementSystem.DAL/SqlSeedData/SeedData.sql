-- Delete rows from tables in the correct order (dependencies first)
 DELETE FROM LoanDetails;
 DELETE FROM Loans;
 DELETE FROM Ratings;
 DELETE FROM BookAuthors;
 DELETE FROM Editions;
 DELETE FROM Books;
 DELETE FROM Authors;
 DELETE FROM Editorials;
 DELETE FROM Subjects;


-- Insert Subjects
IF NOT EXISTS (SELECT 1 FROM Subjects WHERE Name = 'Fiction')
BEGIN
    INSERT INTO Subjects (Name) VALUES ('Fiction');
END

IF NOT EXISTS (SELECT 1 FROM Subjects WHERE Name = 'Science Fiction')
BEGIN
    INSERT INTO Subjects (Name) VALUES ('Science Fiction');
END

IF NOT EXISTS (SELECT 1 FROM Subjects WHERE Name = 'Biography')
BEGIN
    INSERT INTO Subjects (Name) VALUES ('Biography');
END

-- Insert Authors
IF NOT EXISTS (SELECT 1 FROM Authors WHERE FirstName = 'John' AND LastName = 'Doe')
BEGIN
    INSERT INTO Authors (FirstName, LastName, BirthDate, Biography)
    VALUES ('John', 'Doe', '1980-04-01', 'John Doe is a fictional character.');
END

IF NOT EXISTS (SELECT 1 FROM Authors WHERE FirstName = 'Jane' AND LastName = 'Smith')
BEGIN
    INSERT INTO Authors (FirstName, LastName, BirthDate, Biography)
    VALUES ('Jane', 'Smith', '1975-02-15', 'Jane Smith writes about science and history.');
END

-- Insert Editorials
IF NOT EXISTS (SELECT 1 FROM Editorials WHERE Name = 'Penguin Books')
BEGIN
    INSERT INTO Editorials (Name) VALUES ('Penguin Books');
END

IF NOT EXISTS (SELECT 1 FROM Editorials WHERE Name = 'HarperCollins')
BEGIN
    INSERT INTO Editorials (Name) VALUES ('HarperCollins');
END

-- Insert Books
IF NOT EXISTS (SELECT 1 FROM Books WHERE Title = 'The Great Adventure')
BEGIN
    INSERT INTO Books (Title, SubjectId, Synopsis, Photo)
    VALUES ('The Great Adventure', (SELECT SubjectId FROM Subjects WHERE Name = 'Fiction'), 'An epic tale of adventure and discovery.', 'photo1.jpg');
END

IF NOT EXISTS (SELECT 1 FROM Books WHERE Title = 'Journey Through Time')
BEGIN
    INSERT INTO Books (Title, SubjectId, Synopsis, Photo)
    VALUES ('Journey Through Time', (SELECT SubjectId FROM Subjects WHERE Name = 'Science Fiction'), 'A thrilling story about time travel and its consequences.', 'photo2.jpg');
END

IF NOT EXISTS (SELECT 1 FROM Books WHERE Title = 'Life of Einstein')
BEGIN
    INSERT INTO Books (Title, SubjectId, Synopsis, Photo)
    VALUES ('Life of Einstein', (SELECT SubjectId FROM Subjects WHERE Name = 'Biography'), 'The life story of Albert Einstein and his contributions to science.', 'photo3.jpg');
END

-- Insert Editions
IF NOT EXISTS (SELECT 1 FROM Editions WHERE ISBN = '9781234567890')
BEGIN
    INSERT INTO Editions (BookId, ISBN, EditorialId, EditionDate)
    VALUES ((SELECT BookId FROM Books WHERE Title = 'The Great Adventure'), '9781234567890', (SELECT EditorialId FROM Editorials WHERE Name = 'Penguin Books'), '2025-01-01');
END

IF NOT EXISTS (SELECT 1 FROM Editions WHERE ISBN = '9789876543210')
BEGIN
    INSERT INTO Editions (BookId, ISBN, EditorialId, EditionDate)
    VALUES ((SELECT BookId FROM Books WHERE Title = 'Journey Through Time'), '9789876543210', (SELECT EditorialId FROM Editorials WHERE Name = 'HarperCollins'), '2025-02-01');
END

IF NOT EXISTS (SELECT 1 FROM Editions WHERE ISBN = '9781928374650')
BEGIN
    INSERT INTO Editions (BookId, ISBN, EditorialId, EditionDate)
    VALUES ((SELECT BookId FROM Books WHERE Title = 'Life of Einstein'), '9781928374650', (SELECT EditorialId FROM Editorials WHERE Name = 'Penguin Books'), '2025-03-01');
END

-- Insert Readers
IF NOT EXISTS (SELECT 1 FROM Readers WHERE CoreId = 'reader001')
BEGIN
    INSERT INTO Readers (CoreId, FirstName, LastName, Email, Phone, BirthDate)
    VALUES ('reader001', 'Alice', 'Johnson', 'alice@example.com', '(204) 555-1234', '1990-01-15');
END

IF NOT EXISTS (SELECT 1 FROM Readers WHERE CoreId = 'reader002')
BEGIN
    INSERT INTO Readers (CoreId, FirstName, LastName, Email, Phone, BirthDate)
    VALUES ('reader002', 'Bob', 'Williams', 'bob@example.com', '(204) 555-5678', '1985-07-20');
END

-- Insert Loans
IF NOT EXISTS (SELECT 1 FROM Loans WHERE ReaderId = (SELECT ReaderId FROM Readers WHERE CoreId = 'reader001'))
BEGIN
    INSERT INTO Loans (ReaderId, InitialDate, FinalDate)
    VALUES ((SELECT ReaderId FROM Readers WHERE CoreId = 'reader001'), '2025-03-15', '2025-05-15');
END

IF NOT EXISTS (SELECT 1 FROM Loans WHERE ReaderId = (SELECT ReaderId FROM Readers WHERE CoreId = 'reader002'))
BEGIN
    INSERT INTO Loans (ReaderId, InitialDate, FinalDate)
    VALUES ((SELECT ReaderId FROM Readers WHERE CoreId = 'reader002'), '2025-03-16', '2025-05-16');
END

-- Insert LoanDetails
IF NOT EXISTS (SELECT 1 FROM LoanDetails WHERE LoanId = (SELECT LoanId FROM Loans WHERE ReaderId = (SELECT ReaderId FROM Readers WHERE CoreId = 'reader001')) AND BookId = (SELECT BookId FROM Books WHERE Title = 'The Great Adventure'))
BEGIN
    INSERT INTO LoanDetails (LoanId, BookId)
    VALUES ((SELECT LoanId FROM Loans WHERE ReaderId = (SELECT ReaderId FROM Readers WHERE CoreId = 'reader001')), (SELECT BookId FROM Books WHERE Title = 'The Great Adventure'));
END

IF NOT EXISTS (SELECT 1 FROM LoanDetails WHERE LoanId = (SELECT LoanId FROM Loans WHERE ReaderId = (SELECT ReaderId FROM Readers WHERE CoreId = 'reader002')) AND BookId = (SELECT BookId FROM Books WHERE Title = 'Journey Through Time'))
BEGIN
    INSERT INTO LoanDetails (LoanId, BookId)
    VALUES ((SELECT LoanId FROM Loans WHERE ReaderId = (SELECT ReaderId FROM Readers WHERE CoreId = 'reader002')), (SELECT BookId FROM Books WHERE Title = 'Journey Through Time'));
END

-- Insert Ratings
IF NOT EXISTS (SELECT 1 FROM Ratings WHERE ReaderId = (SELECT ReaderId FROM Readers WHERE CoreId = 'reader001') AND BookId = (SELECT BookId FROM Books WHERE Title = 'The Great Adventure'))
BEGIN
    INSERT INTO Ratings (ReaderId, BookId, Rate, Comment)
    VALUES ((SELECT ReaderId FROM Readers WHERE CoreId = 'reader001'), (SELECT BookId FROM Books WHERE Title = 'The Great Adventure'), 5, 'An amazing adventure story!');
END

IF NOT EXISTS (SELECT 1 FROM Ratings WHERE ReaderId = (SELECT ReaderId FROM Readers WHERE CoreId = 'reader002') AND BookId = (SELECT BookId FROM Books WHERE Title = 'Journey Through Time'))
BEGIN
    INSERT INTO Ratings (ReaderId, BookId, Rate, Comment)
    VALUES ((SELECT ReaderId FROM Readers WHERE CoreId = 'reader002'), (SELECT BookId FROM Books WHERE Title = 'Journey Through Time'), 4, 'A thrilling time travel novel!');
END

-- Insert BookAuthors
IF NOT EXISTS (SELECT 1 FROM BookAuthors WHERE BookId = (SELECT BookId FROM Books WHERE Title = 'The Great Adventure') AND AuthorId = (SELECT AuthorId FROM Authors WHERE FirstName = 'John' AND LastName = 'Doe'))
BEGIN
    INSERT INTO BookAuthors (BookId, AuthorId)
    VALUES ((SELECT BookId FROM Books WHERE Title = 'The Great Adventure'), (SELECT AuthorId FROM Authors WHERE FirstName = 'John' AND LastName = 'Doe'));
END

IF NOT EXISTS (SELECT 1 FROM BookAuthors WHERE BookId = (SELECT BookId FROM Books WHERE Title = 'Journey Through Time') AND AuthorId = (SELECT AuthorId FROM Authors WHERE FirstName = 'Jane' AND LastName = 'Smith'))
BEGIN
    INSERT INTO BookAuthors (BookId, AuthorId)
    VALUES ((SELECT BookId FROM Books WHERE Title = 'Journey Through Time'), (SELECT AuthorId FROM Authors WHERE FirstName = 'Jane' AND LastName = 'Smith'));
END

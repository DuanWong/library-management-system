# Library Management System (LMS)

## Overview
The **Library Management System (LMS)** is a web-based application designed to help libraries efficiently manage books, users, borrowing records, and overdue book reports. The system provides functionalities for librarians to oversee inventory and for users (readers) to search, borrow, return, and review books.

## Development Team
Sukhjinder, Fabian, Samuel, Duan.

## Key Features
- **User Authentication & Role Management**: Secure login with roles for **Librarians** (manage inventory, users) and **Readers** (borrow, review books).
- **Book Management**: CRUD operations (Add, Edit, Delete, Search) with categorization by subject, author, availability, and rating.
- **Borrow & Return Books**: Track loan durations and automate due date calculations.
- **Fine Calculation**: Automated fines for late returns.
- **Book Reviews & Ratings**: Users can rate and review books; system displays popular books based on ratings.
- **Statistics & Reports**: Generate reports for most borrowed books, overdue books, and user activity.

## Database Schema
The system uses the following tables:
- **Author**: Stores author details.
- **Subject**: Categorizes books by subject.
- **Book**: Contains book information (ISBN, name, synopsis, etc.).
- **BookAuthor**: Maps books to authors (many-to-many relationship).
- **User**: Stores user details (readers and librarians).
- **Editorial**: Tracks publishing companies.
- **Edition**: Manages book editions.
- **Loan**: Records book loans.
- **LoanDetail**: Tracks individual books within a loan.
- **Rating**: Stores user ratings and reviews for books.

## Technology Stack
- **Frontend**: ASP.NET Core MVC, Bootstrap
- **Backend**: ASP.NET Core, C#
- **Database**: SQLite
- **Authentication**: ASP.NET Core Identity

## Default Accounts:
   - Librarian: `johndoe@library.com` (password: Admin123!)
   - Readers: Register an account using the registration function.

## Usage
- **Librarians**: Log in to manage books, users, and view reports.
- **Readers**: Log in to search, borrow, return, and review books.

## What can Librarians do!
- Main Page
![Main Page](./ReadmeImages/MainPage.png)

- View and Edit all Loans
![Loan Edit Page](./ReadmeImages/Loans.png)

- View and Edit all Readers
![Reader Edit Page](./ReadmeImages/Readers-dashboard.png)

- Serach the Database for any Loan
![Loan Search Page](./ReadmeImages/SearhLoans.png)


## What can Readers Do!
- Use the My Accont Page
![My Account Page](./ReadmeImages/MyAccountView.png)

- Search for any Book In the Database
![Book Search Page](./ReadmeImages/Search.png)

- Have a specific page to look at their Loans
![Reader Loans Page](./ReadmeImages/MyLoans%20Page.png)

## ER Diagram
![ER Diagram](./ReadmeImages/erDiagram.png)

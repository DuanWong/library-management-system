using LibraryManagementSystem.Common;
using LibraryManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.DAL
{
    public class BookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await _context.Books
                .Include(b => b.Subject)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.Editions)
                    .ThenInclude(e => e.Editorial)
                .Include(b => b.Ratings)
                .Include(b => b.LoanDetails)
                .ToListAsync();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _context.Books
                .Include(b => b.Subject)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.Editions)
                    .ThenInclude(e => e.Editorial)
                .Include(b => b.Ratings)
                .Include(b => b.LoanDetails)
                .SingleOrDefaultAsync(r => r.BookId == id);
        }

        public async Task<Book> AddBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }


        public async Task UpdateBook(Book book)
        {
            Book? existingBook = await _context.Books
                                        .Include(e => e.Editions)
                                        .FirstOrDefaultAsync(r => r.BookId == book.BookId);

            if (existingBook == null)
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingBook.Title = book.Title;
            existingBook.SubjectId = book.SubjectId;
            existingBook.Synopsis = book.Synopsis;
            existingBook.Photo = book.Photo;

            if (existingBook.Editions != null) 
                _context.Editions.RemoveRange(existingBook.Editions);
            existingBook.Editions = book.Editions;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBook(Book? book)
        {
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}

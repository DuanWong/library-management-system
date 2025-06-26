using System.Net;
using System.Reflection.PortableExecutable;
using LibraryManagementSystem.Common;
using LibraryManagementSystem.Model;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryManagementSystem.DAL
{
    public class ReaderRepository
    {
        private readonly LibraryDbContext _context;

        public ReaderRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reader>> GetAllReaders()
        {
            return await _context.Readers
                .Include(r => r.Loans)
                .Include(r => r.Ratings)
                .ToListAsync();
        }

        public async Task<Reader> GetReaderById(int id)
        {
            return await _context.Readers
                .Include(r => r.Loans)
                .Include(r => r.Ratings)
                .SingleOrDefaultAsync(r => r.ReaderId == id);
        }

        public async Task<Reader> GetReaderByEmail(string? email)
        {
            return await _context.Readers
                .Include(r => r.Loans)
                .Include(r => r.Ratings)
                .FirstOrDefaultAsync(r => r.Email == email);
        }

        public async Task<Reader> AddReader(Reader reader)
        {
            _context.Readers.Add(reader);
            await _context.SaveChangesAsync();
            return reader;
        }


        public async Task UpdateReader(Reader reader)
        {
            Reader? existingReader = await _context.Readers
                                            .Include(r => r.Loans)
                                            .Include(r => r.Ratings)
                                            .FirstOrDefaultAsync(r => r.ReaderId == reader.ReaderId);

            if (existingReader == null)
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingReader.FirstName = reader.FirstName;
            existingReader.LastName = reader.LastName;
            existingReader.Email = reader.Email;
            existingReader.Phone = reader.Phone;
            existingReader.BirthDate = reader.BirthDate;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteReader(Reader? reader)
        {
            if (reader != null)
            {
                _context.Readers.Remove(reader);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(short, string)> GetCurrentReaction(int readerId, int bookId)
        {
            Rating rating = await _context.Ratings.FirstOrDefaultAsync(r => r.ReaderId == readerId && r.BookId == bookId);

            return (rating?.Rate??0, rating?.Comment??"");
        }

        public async Task UpdateRating(Reader reader, int bookId, short rate, string comment)
        {
            Rating? rating = await _context.Ratings
                                .FirstOrDefaultAsync(r => r.BookId == bookId && r.ReaderId == reader.ReaderId);

            if (rating is null)
            {
                // New rating.
                rating = new Rating
                {
                    BookId = bookId,
                    ReaderId = reader.ReaderId,
                    Rate = rate,
                    Comment = comment
                };

                _context.Ratings.Add(rating);
            }
            else
            {
                // Update existing rating.
                rating.Rate = rate;
                rating.Comment = comment;

                _context.Ratings.Update(rating);
            }

            await _context.SaveChangesAsync();
        }
    }
}

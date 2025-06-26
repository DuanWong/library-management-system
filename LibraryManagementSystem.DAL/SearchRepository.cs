using LibraryManagementSystem.Common.DTOs;
using LibraryManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.DAL
{
    public class SearchRepository
    {
        private readonly LibraryDbContext _context;

        public SearchRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<SearchResultDTO>> SearchBooks(string value, string option)
        {
            value = value.ToLower();

            IQueryable<Book> query = _context.Books
                .Include(b => b.Subject)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.Editions)
                    .ThenInclude(e => e.Editorial);

            switch (option)
            {
                case "Title":
                    query = query.Where(b => b.Title.ToLower().Contains(value));
                    break;

                case "ISBN":
                    query = query.Where(b => b.Editions.Any(e => e.ISBN.ToLower().Contains(value)));
                    break;

                case "Author":
                    query = query.Where(b => b.BookAuthors.Any(ba =>
                        (ba.Author.FirstName + " " + ba.Author.LastName).ToLower().Contains(value)));
                    break;

                case "Editorial":
                    query = query.Where(b => b.Editions.Any(e => e.Editorial.Name.ToLower().Contains(value)));
                    break;

                case "Edition Number":
                    query = query.Where(b => b.Editions.Any(e => e.EditionId.ToString().Contains(value)));
                    break;

                case "Edition Year":
                    query = query.Where(b => b.Editions.Any(e => e.EditionDate.Year.ToString().Contains(value)));
                    break;
            }

            List<Book> books = await query.ToListAsync();

            List<SearchResultDTO> results = books.Select(b => new SearchResultDTO
            {
                BookId = b.BookId,
                Title = b.Title,
                SubjectId = b.SubjectId,
                Subject = b.Subject?.Name,
                Synopsis = b.Synopsis,
                Authors = b.Authors,
                Available = b.LoanDetails == null || b.LoanDetails.All(ld => ld.Loan.FinalDate <= DateTime.Now)
            }).ToList();

            return results;
        }
    }
}

using LibraryManagementSystem.Common.DTOs;
using LibraryManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.DAL
{
    public class ReportsRepository
    {
        private readonly LibraryDbContext _context;

        public ReportsRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<LoansPerReaderResultDTO>> LoansPerReader(string readerFullName, string email)
        {
            IQueryable<Reader> query = _context.Readers
                .Include(r => r.Loans)
                    .ThenInclude(l => l.LoanDetails)
                        .ThenInclude(ld => ld.Book);

            if (!string.IsNullOrEmpty(readerFullName))
            {
                query = query.Where(r => (r.FirstName + " " + r.LastName).Contains(readerFullName));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(r => r.Email.Contains(email));
            }

            List<Reader> filteredReaders = await query.ToListAsync();

            List<LoansPerReaderResultDTO> results = filteredReaders.Select(r => new LoansPerReaderResultDTO
            {
                ReaderId = r.ReaderId,
                ReaderFullName = $"{r.FirstName} {r.LastName}",
                Email = r.Email,
                Age = r.BirthDate.HasValue ? DateTime.Now.Year - r.BirthDate.Value.Year : 0,
                Loans = r.Loans?
                            .OrderByDescending(l => l.InitialDate)
                            .ToList() ?? new List<Loan>()
            }).ToList();

            return results;
        }
    }
}

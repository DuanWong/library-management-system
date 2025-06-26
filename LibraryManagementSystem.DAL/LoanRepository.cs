using LibraryManagementSystem.Common;
using LibraryManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.DAL
{
    public class LoanRepository
    {
        private readonly LibraryDbContext _context;

        public LoanRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Loan>> GetAllLoans()
        {
            return await _context.Loans
                .Include(l => l.Reader)
                .Include(ld => ld.LoanDetails)
                    .ThenInclude(b => b.Book)
                .ToListAsync();
        }

        public async Task<Loan> GetLoanById(int id)
        {
            return await _context.Loans
                .Include(l => l.Reader)
                .Include(ld => ld.LoanDetails)
                    .ThenInclude(b => b.Book)
                .SingleOrDefaultAsync(r => r.LoanId == id);
        }

        public async Task<Loan> AddLoan(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return loan;
        }


        public async Task UpdateLoan(Loan loan)
        {
            Loan? existingLoan = await _context.Loans
                                            .Include(l => l.Reader)
                                            .Include(ld => ld.LoanDetails)
                                            .FirstOrDefaultAsync(l => l.LoanId == loan.LoanId);

            if (existingLoan == null)
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingLoan.ReaderId = loan.ReaderId;
            existingLoan.InitialDate = loan.InitialDate;
            existingLoan.FinalDate = loan.FinalDate;

            if (existingLoan.LoanDetails != null)
                _context.LoanDetails.RemoveRange(existingLoan.LoanDetails);
            existingLoan.LoanDetails = loan.LoanDetails;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteLoan(Loan? loan)
        {
            if (loan != null)
            {
                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();
            }
        }
    }
}
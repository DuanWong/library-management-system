using LibraryManagementSystem.Common;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.BLL
{
    public class LoanService
    {
        private readonly LoanRepository _loanRepository;

        public LoanService(LoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<List<Loan>> GetAllLoans()
        {
            return await _loanRepository.GetAllLoans();
        }

        public async Task<Loan?> GetLoanById(int id)
        {
            Loan loan = await _loanRepository.GetLoanById(id);

            if (loan == null)
            {
                return null;
            }

            return loan;
        }

        public async Task<ServiceResult> AddLoan(Loan loan)
        {
            try
            {
                Loan createdLoan = await _loanRepository.AddLoan(loan);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<ServiceResult> UpdateLoan(Loan loan)
        {
            try
            {
                await _loanRepository.UpdateLoan(loan);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Loan {Constants.NOT_FOUND_ERROR}");
                }

                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<ServiceResult> DeleteLoan(Loan? loan)
        {
            try
            {
                await _loanRepository.DeleteLoan(loan);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }
    }
}
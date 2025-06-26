using LibraryManagementSystem.Common.DTOs;
using LibraryManagementSystem.DAL;

namespace LibraryManagementSystem.BLL
{
    public class ReportsService
    {
        private readonly ReportsRepository _reportsRepository;

        public ReportsService(ReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        public async Task<List<LoansPerReaderResultDTO>> LoansPerReader(string readerFullName, string email)
        {
            return await _reportsRepository.LoansPerReader(readerFullName, email);
        }
    }
}

using LibraryManagementSystem.Common.DTOs;
using LibraryManagementSystem.DAL;

namespace LibraryManagementSystem.BLL
{
    public class SearchService
    {
        private readonly SearchRepository _searchRepository;

        public SearchService(SearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public async Task<List<SearchResultDTO>> SearchBooks(string value, string option)
        {
            return await _searchRepository.SearchBooks(value, option);
        }
    }
}

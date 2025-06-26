using LibraryManagementSystem.Common;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.BLL
{
    public class AuthorService
    {
        private readonly AuthorRepository _authorRepository;

        public AuthorService(AuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<List<Author>> GetAllAuthors()
        {
            return await _authorRepository.GetAllAuthors();
        }

        public async Task<Author?> GetAuthorById(int id)
        {
            Author author = await _authorRepository.GetAuthorById(id);

            if (author == null)
            {
                return null;
            }

            return author;
        }

        public async Task<ServiceResult> AddAuthor(Author author)
        {
            try
            {
                Author createdAuthor = await _authorRepository.AddAuthor(author);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<ServiceResult> UpdateAuthor(Author author)
        {
            try
            {
                await _authorRepository.UpdateAuthor(author);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Author {Constants.NOT_FOUND_ERROR}");
                }

                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<ServiceResult> DeleteAuthor(Author? author)
        {
            try
            {
                await _authorRepository.DeleteAuthor(author);

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
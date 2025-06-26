using LibraryManagementSystem.Common;
using LibraryManagementSystem.Common.DTOs;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.BLL
{
    public class BookService
    {
        private readonly BookRepository _bookRepository;

        public BookService(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await _bookRepository.GetAllBooks();
        }

        public async Task<Book?> GetBookById(int id)
        {
            Book book = await _bookRepository.GetBookById(id);

            if (book == null)
            {
                return null;
            }

            return book;
        }

        public async Task<ServiceResult> AddBook(Book book)
        {
            try
            {
                Book createdBook = await _bookRepository.AddBook(book);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<ServiceResult> UpdateBook(Book book)
        {
            try
            {
                await _bookRepository.UpdateBook(book);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Book {Constants.NOT_FOUND_ERROR}");
                }

                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<ServiceResult> DeleteBook(Book? book)
        {
            try
            {
                await _bookRepository.DeleteBook(book);

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
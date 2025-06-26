using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using LibraryManagementSystem.Common;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.Model;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryManagementSystem.BLL
{
    public class ReaderService
    {
        private readonly ReaderRepository _readerRepository;

        public ReaderService(ReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public async Task<List<Reader>> GetAllReaders()
        {
            return await _readerRepository.GetAllReaders();
        }

        public async Task<Reader?> GetReaderById(int id)
        {
            Reader reader = await _readerRepository.GetReaderById(id);

            if (reader == null)
            {
                return null;
            }

            return reader;
        }

        public async Task<Reader?> GetReaderByEmail(string? email)
        {
            return await _readerRepository.GetReaderByEmail(email);
        }

        public async Task<ServiceResult> AddReader(Reader reader)
        {
            try
            {
                Reader createdReader = await _readerRepository.AddReader(reader);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<ServiceResult> UpdateReader(Reader reader)
        {
            try
            {
                await _readerRepository.UpdateReader(reader);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Reader {Constants.NOT_FOUND_ERROR}");
                }

                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<ServiceResult> DeleteReader(Reader? reader)
        {
            try
            {
                await _readerRepository.DeleteReader(reader);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<(short, string)> GetCurrentReaction(int readerId, int bookId)
        {
            return await _readerRepository.GetCurrentReaction(readerId, bookId);
        }

        public async Task UpdateRating(Reader reader, int bookId, short rate, string comment)
        {
            await _readerRepository.UpdateRating(reader, bookId, rate, comment);
        }
    }
}
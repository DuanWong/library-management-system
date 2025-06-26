using LibraryManagementSystem.Common;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.BLL {
    public class EditorialService {
        private readonly EditorialRepository _editorialRepository;

        public EditorialService(EditorialRepository editorialRepository) {
            _editorialRepository = editorialRepository;
        }

        public async Task<List<Editorial>> GetAllEditorials() {
            return await _editorialRepository.GetAllEditorials();
        }

        public async Task<Editorial?> GetEditorialById(int id) {
            Editorial editorial = await _editorialRepository.GetEditorialById(id);

            if (editorial == null) {
                return null;
            }

            return editorial;
        }

        public async Task<ServiceResult> AddEditorial(Editorial editorial) {
            try {
                Editorial createdEditorial = await _editorialRepository.AddEditorial(editorial);

                return ServiceResult.Ok();
            }
            catch (Exception ex) {
                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<ServiceResult> UpdateEditorial(Editorial editorial) {
            try {
                await _editorialRepository.UpdateEditorial(editorial);

                return ServiceResult.Ok();
            }
            catch (Exception ex) {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR)) {
                    return ServiceResult.Fail($"Editorial {Constants.NOT_FOUND_ERROR}");
                }

                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<ServiceResult> DeleteEditorial(Editorial? editorial) {
            try {
                await _editorialRepository.DeleteEditorial(editorial);

                return ServiceResult.Ok();
            }
            catch (Exception ex) {
                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }
    }
}
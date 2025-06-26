using LibraryManagementSystem.Common;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.BLL
{
    public class SubjectService
    {
        private readonly SubjectRepository _subjectRepository;

        public SubjectService(SubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<List<Subject>> GetAllSubjects()
        {
            return await _subjectRepository.GetAllSubjects();
        }

        public async Task<Subject?> GetSubjectById(int id)
        {
            Subject subject = await _subjectRepository.GetSubjectById(id);

            if (subject == null)
            {
                return null;
            }

            return subject;
        }

        public async Task<ServiceResult> AddSubject(Subject subject)
        {
            try
            {
                Subject createdSubject = await _subjectRepository.AddSubject(subject);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<ServiceResult> UpdateSubject(Subject subject)
        {
            try
            {
                await _subjectRepository.UpdateSubject(subject);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Subject {Constants.NOT_FOUND_ERROR}");
                }

                (string message, string field) = Helper.GetMessage(this.GetType().Name, ex.Message, ex.InnerException);

                return ServiceResult.Fail($"{message}", field);
            }
        }

        public async Task<ServiceResult> DeleteSubject(Subject? subject)
        {
            try
            {
                await _subjectRepository.DeleteSubject(subject);

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
using LibraryManagementSystem.Common;
using LibraryManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.DAL
{
    public class SubjectRepository
    {
        private readonly LibraryDbContext _context;

        public SubjectRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Subject>> GetAllSubjects()
        {
            return await _context.Subjects
                            .Include(b => b.Books)
                            .ToListAsync();
        }

        public async Task<Subject> GetSubjectById(int id)
        {
            return await _context.Subjects
                            .Include(b => b.Books)
                            .SingleOrDefaultAsync(r => r.SubjectId == id);
        }

        public async Task<Subject> AddSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return subject;
        }


        public async Task UpdateSubject(Subject subject)
        {
            Subject existingSubject = await _context.Subjects.FirstOrDefaultAsync(r => r.SubjectId == subject.SubjectId);

            if (existingSubject == null)
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingSubject.Name = subject.Name;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubject(Subject? subject)
        {
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
        }
    }
}
using LibraryManagementSystem.Common;
using LibraryManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.DAL {
    public class EditorialRepository {
        private readonly LibraryDbContext _context;

        public EditorialRepository(LibraryDbContext context) {
            _context = context;
        }

        public async Task<List<Editorial>> GetAllEditorials() {
            return await _context.Editorials
                            .Include(b => b.Editions)
                            .ToListAsync();
        }

        public async Task<Editorial> GetEditorialById(int id) {
            return await _context.Editorials
                            .Include(b => b.Editions)
                            .SingleOrDefaultAsync(r => r.EditorialId == id);
        }

        public async Task<Editorial> AddEditorial(Editorial editorial) {
            _context.Editorials.Add(editorial);
            await _context.SaveChangesAsync();
            return editorial;
        }

        public async Task UpdateEditorial(Editorial editorial) {
            Editorial existingEditorial = await _context.Editorials
                .Include(e => e.Editions)
                    .ThenInclude(e => e.Book)
                .FirstOrDefaultAsync(r => r.EditorialId == editorial.EditorialId);

            if (existingEditorial == null) {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingEditorial.Name = editorial.Name;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteEditorial(Editorial? editorial) {
            if (editorial != null) {
                _context.Editorials.Remove(editorial);
                await _context.SaveChangesAsync();
            }
        }
    }
}
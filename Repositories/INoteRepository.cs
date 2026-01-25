using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;

namespace IsTakipWpf.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllAsync();
        Task<Note> GetByIdAsync(int id);
        Task<int> AddAsync(Note note);
        Task<bool> UpdateAsync(Note note);
        Task<bool> DeleteAsync(int id);
    }
}

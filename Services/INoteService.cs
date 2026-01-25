using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;

namespace IsTakipWpf.Services
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetAllNotesAsync();
        Task<Note> GetNoteByIdAsync(int id);
        Task<int> AddNoteAsync(Note note);
        Task<bool> UpdateNoteAsync(Note note);
        Task<bool> DeleteNoteAsync(int id);
    }
}

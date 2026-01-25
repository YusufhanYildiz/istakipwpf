using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;
using IsTakipWpf.Repositories;

namespace IsTakipWpf.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            return await _noteRepository.GetAllAsync();
        }

        public async Task<Note> GetNoteByIdAsync(int id)
        {
            return await _noteRepository.GetByIdAsync(id);
        }

        public async Task<int> AddNoteAsync(Note note)
        {
            return await _noteRepository.AddAsync(note);
        }

        public async Task<bool> UpdateNoteAsync(Note note)
        {
            return await _noteRepository.UpdateAsync(note);
        }

        public async Task<bool> DeleteNoteAsync(int id)
        {
            return await _noteRepository.DeleteAsync(id);
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IsTakipWpf.Models;
using IsTakipWpf.Services;

namespace IsTakipWpf.ViewModels
{
    public class NotesViewModel : ViewModelBase, IRefreshable
    {
        private readonly INoteService _noteService;
        private readonly MaterialDesignThemes.Wpf.ISnackbarMessageQueue _messageQueue;
        
        private string _newNoteTitle;
        private string _newNoteContent;
        private bool _isAdding;

        public ObservableCollection<Note> Notes { get; } = new ObservableCollection<Note>();

        public string NewNoteTitle
        {
            get => _newNoteTitle;
            set => SetProperty(ref _newNoteTitle, value);
        }

        public string NewNoteContent
        {
            get => _newNoteContent;
            set => SetProperty(ref _newNoteContent, value);
        }

        public bool IsAdding
        {
            get => _isAdding;
            set => SetProperty(ref _isAdding, value);
        }

        public ICommand AddNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }
        public ICommand PinNoteCommand { get; }
        public ICommand ToggleAddModeCommand { get; }
        public ICommand SaveNoteCommand { get; }
        public ICommand CancelAddCommand { get; }

        public NotesViewModel(INoteService noteService, MaterialDesignThemes.Wpf.ISnackbarMessageQueue messageQueue)
        {
            _noteService = noteService;
            _messageQueue = messageQueue;

            AddNoteCommand = new RelayCommand(_ => { IsAdding = true; });
            ToggleAddModeCommand = new RelayCommand(_ => { IsAdding = !IsAdding; });
            CancelAddCommand = new RelayCommand(_ => { IsAdding = false; NewNoteTitle = ""; NewNoteContent = ""; });
            
            SaveNoteCommand = new RelayCommand(async _ => await SaveNewNoteAsync());
            DeleteNoteCommand = new RelayCommand(async id => await DeleteNoteAsync((int)id));
            PinNoteCommand = new RelayCommand(async note => await TogglePinAsync(note as Note));
        }

        public async Task RefreshAsync()
        {
            await LoadNotesAsync();
        }

        private async Task LoadNotesAsync()
        {
            var notes = await _noteService.GetAllNotesAsync();
            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
        }

        private async Task SaveNewNoteAsync()
        {
            if (string.IsNullOrWhiteSpace(NewNoteContent))
            {
                _messageQueue.Enqueue("Not içeriği boş olamaz.");
                return;
            }

            var note = new Note
            {
                Title = NewNoteTitle,
                Content = NewNoteContent,
                IsPinned = false,
                Color = "#FFFFFF" // Can be expanded later
            };

            await _noteService.AddNoteAsync(note);
            NewNoteTitle = "";
            NewNoteContent = "";
            IsAdding = false;
            await LoadNotesAsync();
            _messageQueue.Enqueue("Not eklendi.");
        }

        private async Task DeleteNoteAsync(int id)
        {
            if (await _noteService.DeleteNoteAsync(id))
            {
                var note = Notes.FirstOrDefault(n => n.Id == id);
                if (note != null) Notes.Remove(note);
                _messageQueue.Enqueue("Not silindi.");
            }
        }

        private async Task TogglePinAsync(Note note)
        {
            if (note == null) return;
            note.IsPinned = !note.IsPinned;
            await _noteService.UpdateNoteAsync(note);
            await LoadNotesAsync(); // Re-sort
        }
    }
}

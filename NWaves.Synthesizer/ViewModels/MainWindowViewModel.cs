using Caliburn.Micro;
using NWaves.Synthesizer.Interfaces;
using System.Windows.Input;

namespace NWaves.Synthesizer.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly ISynthesizerService _synthesizer;
        private readonly IKeyNoteMapperService _keyNoteMapper;

        private string _note = string.Empty;
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                NotifyOfPropertyChange(() => Note);
            }
        }


        public MainWindowViewModel(ISynthesizerService synthesizer, IKeyNoteMapperService keyNoteMapper)
        {
            _synthesizer = synthesizer;
            _keyNoteMapper = keyNoteMapper;
        }

        public void OnKeyDown(KeyEventArgs e)
        {
            try
            {
                var (note, octave) = _keyNoteMapper.Map(e.Key.ToString());

                _synthesizer.PlayNote(note, octave);

                Note = Note.Replace($"{note}{octave}", "").Trim();
                Note += $" {note}{octave}";
            }
            catch { }
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            try
            {
                var (note, octave) = _keyNoteMapper.Map(e.Key.ToString());

                _synthesizer.StopNote(note, octave);

                Note = Note.Replace($"{note}{octave}", "");
            }
            catch { }
        }
    }
}

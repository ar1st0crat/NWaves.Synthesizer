using Caliburn.Micro;
using NWaves.Synthesizer.Interfaces;
using System;
using System.Windows.Input;

namespace NWaves.Synthesizer.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly ISynthesizerService _synthesizer;
        private readonly IKeyNoteMapperService _keyNoteMapper;

        private string _keys = string.Empty;
        public string Keys
        {
            get => _keys;
            set
            {
                _keys = value;
                NotifyOfPropertyChange(() => Keys);
            }
        }

        private int _octave = 3;
        public int Octave
        {
            get => _octave;
            set
            {
                _octave = value;
                NotifyOfPropertyChange(() => Octave);
            }
        }

        private float _volume = 0.5f;
        public float Volume
        {
            get => (float)Math.Round(_volume, 1);
            set
            {
                _volume = value;
                _synthesizer.ChangeVolume(_volume);
                NotifyOfPropertyChange(() => Volume);
            }
        }

        private Instrument _sound;
        public Instrument Sound
        {
            get => _sound;
            set
            {
                _sound = value;
                NotifyOfPropertyChange(() => Sound);
                NotifyOfPropertyChange(() => SelectedInstrument);
            }
        }

        public string SelectedInstrument => $"pack://application:,,,/Assets/{_sound}.png";


        public MainWindowViewModel(ISynthesizerService synthesizer, IKeyNoteMapperService keyNoteMapper)
        {
            _synthesizer = synthesizer;
            _keyNoteMapper = keyNoteMapper;
        }

        public void OnKeyDown(KeyEventArgs e)
        {
            try
            {
                var key = e.Key.ToString();
                if (key.Length > 1) key = key.Substring(1);

                var (note, octaveDelta) = _keyNoteMapper.Map(key);

                var octave = Octave + octaveDelta;

                _synthesizer.PlayNote(note, octave);

                Keys = Keys.Replace(key, "");
                Keys += key;
            }
            catch { }
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            try
            {
                var key = e.Key.ToString();
                if (key.Length > 1) key = key.Substring(1);

                var (note, octaveDelta) = _keyNoteMapper.Map(key);

                var octave = Octave + octaveDelta;

                _synthesizer.StopNote(note, octave);

                Keys = Keys.Replace(key, "");
            }
            catch { }
        }

        public void Organ()
        {
            Sound = Instrument.Organ;
            _synthesizer.ChangeInstrument(Sound);
        }

        public void Guitar()
        {
            Sound = Instrument.Guitar;
            _synthesizer.ChangeInstrument(Sound);
        }

        public void OctaveDown()
        {
            if (Octave > 0) Octave--;
        }

        public void OctaveUp()
        {
            if (Octave < 8) Octave++;
        }

        public void VolumeDown()
        {
            if (Volume >= 0.1f) Volume -= 0.1f;
        }

        public void VolumeUp()
        {
            Volume += 0.1f;
        }
    }
}

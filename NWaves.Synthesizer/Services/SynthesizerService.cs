using NWaves.Signals.Builders;
using NWaves.Synthesizer.Interfaces;
using NWaves.Synthesizer.Utils;
using NWaves.Utils;

namespace NWaves.Synthesizer.Services
{
    public class SynthesizerService : ISynthesizerService
    {
        private readonly AudioService _audioService;

        public int SampleRate { get; set; } = 8000;
        public int OctaveDelta { get; set; } = +1;


        public SynthesizerService()
        {
            _audioService = new AudioService(SampleRate);
            _audioService.Play();
        }

        public void PlayNote(string note, int octave)
        {
            octave += OctaveDelta;

            if (_audioService.Notes.Contains(note + octave))
            {
                return;
            }

            var freq = Scale.NoteToFreq(note, octave);

            //var sound = new FadeInOutBuilder(
            //                new KarplusStrongBuilder()
            //                    .SetParameter("freq", freq)
            //                    .SetParameter("stretch", 2 * freq / 80)
            //                    .OfLength(SampleRate * 2)
            //                    .SampledAt(SampleRate));

            var sound = new FadeInOutBuilder(
                            new PadSynthBuilder()
                                .SetParameter("fftsize", 1024 * 16)
                                .SetParameter("freq", freq)
                                .SetAmplitudes(new[] { 25, 70, 10, 20, 5, 80, 0, 55, 1, 3, 0f })
                                .OfLength(SampleRate * 3)
                                .SampledAt(SampleRate));

            _audioService.Volume = 1f;
            _audioService.AddNote(note + octave, sound);
        }

        public void StopNote(string note, int octave)
        {
            octave += OctaveDelta;

            _audioService.RemoveNote(note + octave);
        }
    }
}

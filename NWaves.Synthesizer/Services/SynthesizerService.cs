﻿using NWaves.Signals.Builders;
using NWaves.Synthesizer.Config;
using NWaves.Synthesizer.Interfaces;
using NWaves.Utils;

namespace NWaves.Synthesizer.Services
{
    public class SynthesizerService : ISynthesizerService
    {
        private readonly IAudioService _audioService;
        private readonly SynthesizerConfig _config;
        private readonly int _sampleRate;

        private Instrument _instrument = Instrument.Organ;

        public float SecondsFadeIn { get; set; } = 0.1f;
        public float SecondsFadeOut { get; set; } = 0.9f;

        public SynthesizerService(IAudioService audioService, SynthesizerConfig config)
        {
            _config = config;
            _audioService = audioService;
            _sampleRate = audioService.WaveFormat.SampleRate;
            _audioService.Play();
        }

        public void PlayNote(string note, int octave)
        {
            var freq = Scale.NoteToFreq(note, octave);

            // if FadeOutSeconds == 0, then slightly increase this parameter for making AudioService logic simpler
            // (see comments in AudioService.cs):

            var secondsFadeOut = SecondsFadeOut > 0 ? SecondsFadeOut : 0.001f;

            FadeInOutBuilder sound;

            switch (_instrument)
            {
                case Instrument.Guitar:
                    sound = new FadeInOutBuilder(
                                    new KarplusStrongBuilder()
                                        .SetParameter("freq", freq)
                                        .SetParameter("stretch", freq / _config.StretchFactor)
                                        .OfLength(_sampleRate * _config.Seconds)
                                        .SampledAt(_sampleRate))
                                    .In(SecondsFadeIn)
                                    .Out(secondsFadeOut);
                    break;
                default:
                    sound = new FadeInOutBuilder(
                                    new PadSynthBuilder()
                                        .SetAmplitudes(_config.Amplitudes)
                                        .SetParameter("fftsize", _config.FftSize)
                                        .SetParameter("freq", freq)
                                        .OfLength(_sampleRate * _config.Seconds)
                                        .SampledAt(_sampleRate))
                                    .In(SecondsFadeIn)
                                    .Out(secondsFadeOut);
                    break;
            }

            _audioService.AddSound(note + octave, sound);
        }

        public void StopNote(string note, int octave)
        {
            _audioService.RemoveSound(note + octave);
        }

        public void ChangeInstrument(Instrument instrument)
        {
            _instrument = instrument;
        }

        public void ChangeVolume(float volume)
        {
            _audioService.Volume = volume;
        }
    }
}

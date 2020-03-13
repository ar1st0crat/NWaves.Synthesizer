using NAudio.Wave;
using NWaves.Synthesizer.Interfaces;
using NWaves.Synthesizer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NWaves.Synthesizer.Services
{
    class AudioService : IAudioService
    {
        private readonly WaveOut _waveOut;

        private readonly List<FadeInOutBuilder> _synthesizers = new List<FadeInOutBuilder>();

        public WaveFormat WaveFormat { get; private set; }

        public float Volume { get; set; } = 0.5f;

        public List<string> Notes { get; set; } = new List<string>();


        public AudioService(int sampleRate = 16000, int channels = 1)
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);

            _waveOut = new WaveOut { DesiredLatency = 70 };
            _waveOut.Init(this);
        }

        public void AddNote(string note, FadeInOutBuilder synthesizer)
        {
            Notes.Add(note);
            _synthesizers.Add(synthesizer);
        }

        public void RemoveNote(string note)
        {
            var index = Notes.IndexOf(note);

            if (index < 0) return;

            _synthesizers[index].FadeOut();
        }

        public int Read(float[] buffer, int offset, int sampleCount)
        {
            var channels = WaveFormat.Channels;

            if (!_synthesizers.Any())
            {
                Array.Clear(buffer, 0, buffer.Length);
                return sampleCount;
            }

            for (var n = 0; n < sampleCount; n += channels)
            {
                int index = _synthesizers.FindIndex(s => s.Finished);   // finished fading...

                if (index >= 0)
                {
                    _synthesizers.RemoveAt(index);
                    Notes.RemoveAt(index);
                }

                var sample = _synthesizers.Any() ? _synthesizers.Select(s => s.NextSample()).Sum() * Volume : 0;

                for (var i = 0; i < channels; i++)
                {
                    buffer[offset + n + i] = sample;
                }
            }

            return sampleCount;
        }

        public void Play() => _waveOut?.Play();

        public void Pause() => _waveOut.Pause();

        public void Resume() => _waveOut.Resume();

        public void Stop() => _waveOut?.Stop();

        public void Dispose() => _waveOut?.Dispose();
    }
}

using NAudio.Wave;
using NWaves.Synthesizer.Config;
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

        private readonly List<FadeInOutBuilder> _sounds = new List<FadeInOutBuilder>();
        private readonly List<string> _notes = new List<string>();

        public WaveFormat WaveFormat { get; private set; }

        public float Volume { get; set; }


        public AudioService(AudioConfig config)
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(config.SampleRate, config.Channels);
            
            Volume = config.Volume;

            _waveOut = new WaveOut { DesiredLatency = 70 };
            _waveOut.Init(this);
        }

        public void AddSound(string note, FadeInOutBuilder sound)
        {
            _notes.Add(note);
            _sounds.Add(sound);
        }

        public void RemoveSound(string note)
        {
            var index = _notes.IndexOf(note);

            if (index < 0) return;

            _sounds[index].FadeOut();
        }

        public int Read(float[] buffer, int offset, int sampleCount)
        {
            var channels = WaveFormat.Channels;

            if (!_sounds.Any())
            {
                Array.Clear(buffer, 0, buffer.Length);
                return sampleCount;
            }

            for (var n = 0; n < sampleCount; n += channels)
            {
                int index = _sounds.FindIndex(s => s.Finished);   // finished fading...

                if (index >= 0)
                {
                    _sounds.RemoveAt(index);
                    _notes.RemoveAt(index);
                }

                var sample = _sounds.Any() ? _sounds.Select(s => s.NextSample()).Sum() * Volume : 0;

                for (var i = 0; i < channels; i++)
                {
                    buffer[offset + n + i] = sample;
                }
            }

            return sampleCount;
        }

        public bool IsPlayingNote(string note) => _notes.Contains(note);

        public void Play() => _waveOut?.Play();

        public void Stop() => _waveOut?.Stop();

        public void Dispose() => _waveOut?.Dispose();
    }
}

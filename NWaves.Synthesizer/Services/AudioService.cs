using NAudio.Wave;
using NWaves.Signals.Builders;
using NWaves.Synthesizer.Config;
using NWaves.Synthesizer.Interfaces;
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

            while (_sounds[index].FadeStarted)  // skip sounds that have already started fading
            {
                index = _notes.IndexOf(note, index + 1);
            }

            _sounds[index].FadeOut();
        }

        public int Read(float[] buffer, int offset, int sampleCount)
        {
            if (!_sounds.Any())
            {
                Array.Clear(buffer, 0, buffer.Length);
                return sampleCount;
            }

            var channels = WaveFormat.Channels;

            for (var n = offset; n < sampleCount; n += channels)
            {
                int index = _sounds.FindIndex(s => s.FadeFinished);   // first note that finished fading...
                                                                      // (if we allowed fadeOutSeconds to be 0 then the logic here
                                                                      // would depend on smth different than FadeFinished property)
                while (index >= 0)
                {
                    _sounds.RemoveAt(index);
                    _notes.RemoveAt(index);
                    
                    index = _sounds.FindIndex(s => s.FadeFinished);   // other notes (perhaps) that finished fading
                }

                var sample = _sounds.Any() ? _sounds.Select(s => s.NextSample()).Sum() * Volume : 0;

                for (var i = 0; i < channels; i++)
                {
                    buffer[n + i] = sample;
                }
            }

            return sampleCount;
        }

        public void Play() => _waveOut?.Play();

        public void Stop() => _waveOut?.Stop();

        public void Dispose() => _waveOut?.Dispose();
    }
}

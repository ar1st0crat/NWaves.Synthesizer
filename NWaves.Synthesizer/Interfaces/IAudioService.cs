﻿using NAudio.Wave;
using NWaves.Signals.Builders;
using System;

namespace NWaves.Synthesizer.Interfaces
{
    public interface IAudioService : ISampleProvider, IDisposable
    {
        float Volume { get; set; }

        void Play();
        void Stop();
        
        void AddSound(string note, FadeInOutBuilder sound);
        void RemoveSound(string note);
    }
}

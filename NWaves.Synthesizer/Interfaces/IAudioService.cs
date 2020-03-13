using NAudio.Wave;
using NWaves.Synthesizer.Utils;
using System;

namespace NWaves.Synthesizer.Interfaces
{
    public interface IAudioService : ISampleProvider, IDisposable
    {
        void Play();
        void Pause();
        void Resume();
        void Stop();

        void AddNote(string note, FadeInOutBuilder synthesizer);
        void RemoveNote(string note);
    }
}

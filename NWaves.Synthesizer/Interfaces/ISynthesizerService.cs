namespace NWaves.Synthesizer.Interfaces
{
    public interface ISynthesizerService
    {
        void PlayNote(string note, int octave);
        void StopNote(string note, int octave);
        
        void ChangeInstrument(Instrument instrument);
        void ChangeVolume(float volume);
    }
}

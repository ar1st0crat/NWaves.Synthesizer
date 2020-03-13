namespace NWaves.Synthesizer.Interfaces
{
    public interface IKeyNoteMapperService
    {
        (string, int) Map(string key);
    }
}
using NWaves.Synthesizer.Interfaces;
using System.Collections.Generic;

namespace NWaves.Synthesizer.Services
{
    public class KeyNoteMapperService : IKeyNoteMapperService
    {
        private readonly Dictionary<string, (string, int)> _keyNoteDictionary =
            new Dictionary<string, (string, int)>()
            {
                { "Q", ("C", 3) },
                { "W", ("D", 3) },
                { "E", ("E", 3) },
                { "R", ("F", 3) },
                { "T", ("G", 3) },
                { "Y", ("A", 3) },
                { "U", ("B", 3) },
                { "D2", ("C#", 3) },
                { "D3", ("D#", 3) },
                { "D5", ("F#", 3) },
                { "D6", ("G#", 3) },
                { "D7", ("A#", 3) },
                { "Z", ("C", 4) },
                { "X", ("D", 4) },
                { "C", ("E", 4) },
                { "V", ("F", 4) },
                { "B", ("G", 4) },
                { "N", ("A", 4) },
                { "M", ("B", 4) },
                { "S", ("C#", 4) },
                { "D", ("D#", 4) },
                { "G", ("F#", 4) },
                { "H", ("G#", 4) },
                { "J", ("A#", 4) }
            };

        public (string, int) Map(string key) => _keyNoteDictionary[key];
    }
}

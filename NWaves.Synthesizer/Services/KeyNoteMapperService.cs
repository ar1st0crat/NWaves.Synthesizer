using NWaves.Synthesizer.Interfaces;
using System.Collections.Generic;

namespace NWaves.Synthesizer.Services
{
    public class KeyNoteMapperService : IKeyNoteMapperService
    {
        /// <summary>
        /// Key:
        ///     Key (on the keyboard)
        /// Value:
        ///     Note
        ///     Octave shift (relative to current base octave)
        /// </summary>
        private readonly Dictionary<string, (string, int)> _keyNoteDictionary =
            new Dictionary<string, (string, int)>()
            {
                { "Q", ("C", 0) },
                { "W", ("D", 0) },
                { "E", ("E", 0) },
                { "R", ("F", 0) },
                { "T", ("G", 0) },
                { "Y", ("A", 0) },
                { "U", ("B", 0) },
                { "2", ("C#", 0) },
                { "3", ("D#", 0) },
                { "5", ("F#", 0) },
                { "6", ("G#", 0) },
                { "7", ("A#", 0) },
                { "Z", ("C", 1) },
                { "X", ("D", 1) },
                { "C", ("E", 1) },
                { "V", ("F", 1) },
                { "B", ("G", 1) },
                { "N", ("A", 1) },
                { "M", ("B", 1) },
                { "S", ("C#", 1) },
                { "D", ("D#", 1) },
                { "G", ("F#", 1) },
                { "H", ("G#", 1) },
                { "J", ("A#", 1) }
            };

        public (string, int) Map(string key) => _keyNoteDictionary[key];
    }
}

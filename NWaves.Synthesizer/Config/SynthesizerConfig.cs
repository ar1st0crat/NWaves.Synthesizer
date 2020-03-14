namespace NWaves.Synthesizer.Config
{
    public class SynthesizerConfig
    {
        public int FftSize { get; set; } = 1024 * 16;
        public float[] Amplitudes { get; set; } = new[] { 25, 70, 10, 20, 5, 80, 0, 55, 1, 3, 0f };
        public float StretchFactor { get; set; } = 40f;
        public int Seconds { get; set; } = 3;
    }
}

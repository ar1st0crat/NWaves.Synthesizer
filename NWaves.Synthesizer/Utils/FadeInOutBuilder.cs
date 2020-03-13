using NWaves.Signals.Builders;

namespace NWaves.Synthesizer.Utils
{
    /// <summary>
    /// Fade in/out decorator for any signal generator
    /// </summary>
    public class FadeInOutBuilder : SignalBuilder
    {
        private readonly SignalBuilder _builder;
        private int _first, _last;

        public int FadeSamples { get; set; } = 200;
        

        public FadeInOutBuilder(SignalBuilder builder)
        {
            _builder = builder;
            _last = -1;
        }

        public override float NextSample()
        {
            var sample = _builder.NextSample();

            if (_last > 0)
            {
                sample *= _last-- / (float)FadeSamples;
            }
            else if (_first < FadeSamples)
            {
                sample *= _first++ / (float)FadeSamples;
            }
            
            return sample;
        }

        public void FadeOut()
        {
            _last = FadeSamples - 1;
        }

        public bool Finished => _last == 0;
    }
}

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NWaves.Synthesizer.Converters
{
    public class WhiteKeyColorConverter : IMultiValueConverter
    {
        private static readonly Brush _whiteBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        private static readonly Brush _pinkBrush = new SolidColorBrush(Color.FromRgb(255, 200, 200));

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var note = (char)values[0];
            var notes = values[1] as string;

            return notes != null && notes.Contains(note) ? _pinkBrush : _whiteBrush;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

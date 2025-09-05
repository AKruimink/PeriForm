using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace PeriForm.Infrastructure.Converters;

/// <summary>
/// Defines a class that converts decimal millisecond values to and from "hh:mm:ss.fff" formatted strings.
/// </summary>
public class DecimalMsToTimeTextConverter : MarkupExtension, IValueConverter
{
    /// <summary>
    /// Singleton instance of the <see cref="DecimalMsToTimeTextConverter"/>.
    /// </summary>
    private static DecimalMsToTimeTextConverter? _instance;

    /// <summary>
    /// Provides an instance of the <see cref="DecimalMsToTimeTextConverter"/>.
    /// </summary>
    public override object ProvideValue(IServiceProvider serviceProvider) =>
        _instance ??= new DecimalMsToTimeTextConverter();

    private const string Format = @"hh\:mm\:ss\.fff";

    /// <summary>
    /// Converts a decimal millisecond value into a formatted time string.
    /// </summary>
    /// <param name="value">The value in milliseconds (decimal).</param>
    /// <param name="targetType">The type of the bound target property.</param>
    /// <param name="parameter">Optional parameter (unused).</param>
    /// <param name="culture">The culture used during conversion.</param>
    /// <returns>A formatted string "hh:mm:ss.fff".</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return "00:00:00.000";
        }

        var ms = System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
        return TimeSpan.FromMilliseconds(ms).ToString(Format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts a formatted time string back into a decimal millisecond value.
    /// Accepts strict "hh:mm:ss.fff" format, with fallback to TimeSpan.Parse.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="targetType">The type of the bound target property.</param>
    /// <param name="parameter">Optional parameter (unused).</param>
    /// <param name="culture">The culture used during conversion.</param>
    /// <returns>The number of milliseconds as <see cref="decimal"/>.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var s = (value ?? "").ToString();
        if (TimeSpan.TryParseExact(s, Format, CultureInfo.InvariantCulture, out var ts))
        {
            return (decimal)ts.TotalMilliseconds;
        }

        if (TimeSpan.TryParse(s, CultureInfo.InvariantCulture, out ts))
        {
            return (decimal)ts.TotalMilliseconds;
        }

        return Binding.DoNothing;
    }
}

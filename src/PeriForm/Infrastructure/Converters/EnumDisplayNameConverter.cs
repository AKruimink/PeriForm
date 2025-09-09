using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace PeriForm.Infrastructure.Converters;

/// <summary>
/// Converts an enum value to a human‑friendly display name using the
/// <see cref="DescriptionAttribute"/>.  If no description is present, the
/// enum's name is used.  This converter can be used in XAML bindings to
/// display enums in ComboBoxes with user‑friendly labels.
/// </summary>
[ValueConversion(typeof(Enum), typeof(string))]
public class EnumDisplayNameConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum e)
        {
            var memInfo = e.GetType().GetMember(e.ToString());
            if (memInfo.Length > 0)
            {
                var attr = memInfo[0].GetCustomAttribute<DescriptionAttribute>();
                if (attr != null)
                {
                    return attr.Description;
                }
            }
            return e.ToString();
        }
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

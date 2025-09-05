using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;

namespace PeriForm.Infrastructure.Converters;

/// <summary>
/// Converts enum values to human-friendly display names.
/// Prefers <see cref="DisplayAttribute"/> (respects resource-based names via <see cref="DisplayAttribute.GetName()"/>),
/// falls back to <see cref="DescriptionAttribute"/> if present, otherwise returns the enum member name.
/// </summary>
public class EnumDisplayNameConverter : MarkupExtension, IValueConverter
{
    /// <summary>
    /// Singleton instance of the <see cref="EnumDisplayNameConverter"/>.
    /// </summary>
    private static EnumDisplayNameConverter? _instance;

    /// <summary>
    /// Provides an instance of the <see cref="EnumDisplayNameConverter"/>.
    /// </summary>
    public override object ProvideValue(IServiceProvider serviceProvider) =>
        _instance ??= new EnumDisplayNameConverter();

    /// <summary>
    /// Converts an enum value to a display string.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <param name="targetType">The type of the bound target property.</param>
    /// <param name="parameter">Unused.</param>
    /// <param name="culture">The culture used during conversion.</param>
    /// <returns>A display string for the enum value.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return string.Empty;
        }

        var type = value.GetType();
        if (!type.IsEnum)
        {
            return value.ToString() ?? string.Empty;
        }

        var name = Enum.GetName(type, value);
        if (name is null)
        {
            return value.ToString() ?? string.Empty;
        }

        var field = type.GetField(name, BindingFlags.Public | BindingFlags.Static);
        if (field is null)
        {
            return name;
        }

        // Prefer [Display]
        var display = field.GetCustomAttribute<DisplayAttribute>(inherit: false);
        var displayName = display?.GetName();
        if (!string.IsNullOrWhiteSpace(displayName))
        {
            return displayName!;
        }

        // Fallback to [Description]
        var description = field.GetCustomAttribute<DescriptionAttribute>(inherit: false)?.Description;
        if (!string.IsNullOrWhiteSpace(description))
        {
            return description!;
        }

        // Fallback to raw member name
        return name;
    }

    /// <summary>
    /// Not implemented. Enum display names are one-way only.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        Binding.DoNothing;
}

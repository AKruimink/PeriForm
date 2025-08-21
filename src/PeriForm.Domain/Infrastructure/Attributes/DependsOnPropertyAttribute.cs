namespace PeriForm.Domain.Infrastructure.Attributes;

/// <summary>
/// Defines a class that acts as an attributes that defines the property name that is relied on for the <see cref="INotifyPropertyChanged"/> invocation
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class DependsOnPropertyAttribute(string dependencyProperty) : Attribute
{
    /// <summary>
    /// Gets the name of the property that should be relied on
    /// </summary>
    public string DependencyProperty { get; } = dependencyProperty;
}

using System.ComponentModel;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace PeriForm.Infrastructure.ViewModel;

/// <summary>
/// Provides an attached property to wire a view's <see cref="FrameworkElement.DataContext"/>
/// to a ViewModel resolved from Microsoft.Extensions.DependencyInjection.
/// A dedicated DI scope is created per view and disposed when the view unloads.
/// </summary>
public static class ViewModelLocator
{
    /// <summary>
    /// <see cref="IServiceScopeFactory"/> used to create a per-view DI scope.
    /// </summary>
    private static IServiceScopeFactory? s_scopeFactory;

    /// <summary>
    /// Attached property that stores the DI scope created for a given view. This enables proper disposal when the view unloads or when the ViewModel type changes.
    /// </summary>
    private static readonly DependencyProperty s_viewModelScopeProperty =
        DependencyProperty.RegisterAttached(
            "ViewModelScope",
            typeof(IServiceScope),
            typeof(ViewModelLocator),
            new PropertyMetadata(null));

    /// <summary>
    /// The attached property that specifies the <see cref="Type"/> of the ViewModel to resolve and assign as the <see cref="FrameworkElement.DataContext"/>.
    /// </summary>
    public static readonly DependencyProperty WireViewModelProperty =
        DependencyProperty.RegisterAttached(
            "WireViewModel",
            typeof(Type),
            typeof(ViewModelLocator),
            new PropertyMetadata(null, WireViewModelChanged));

    /// <summary>
    /// Configures the locator with an <see cref="IServiceScopeFactory"/>. This must be called once during application startup after the host is built.
    /// </summary>
    /// <param name="scopeFactory">The <see cref="IServiceScopeFactory"/> used to create per-view scopes.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="scopeFactory"/> is <c>null</c>.</exception>
    public static void SetServiceScopeFactory(IServiceScopeFactory scopeFactory)
    {
        s_scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    /// <summary>
    /// Gets the <see cref="Type"/> currently set on <see cref="WireViewModelProperty"/>.
    /// </summary>
    /// <param name="obj">The target element.</param>
    /// <returns>The configured <see cref="Type"/>.</returns>
    public static Type GetWireViewModel(DependencyObject obj) => (Type)obj.GetValue(WireViewModelProperty);

    /// <summary>
    /// Sets the <see cref="Type"/> on <see cref="WireViewModelProperty"/> indicating
    /// the ViewModel to resolve for the target element.
    /// </summary>
    /// <param name="obj">The target element.</param>
    /// <param name="value">The ViewModel <see cref="Type"/> to resolve.</param>
    public static void SetWireViewModel(DependencyObject obj, Type value) => obj.SetValue(WireViewModelProperty, value);

    /// <summary>
    /// Handles changes to <see cref="WireViewModelProperty"/>. Creates a new DI scope for the view,
    /// resolves the requested ViewModel type, assigns it as DataContext, and wires disposal on unload.
    /// Disposes any previous scope held by the element.
    /// </summary>
    /// <param name="d">The dependency object (view) whose property changed.</param>
    /// <param name="e">Change args containing old and new values.</param>
    private static void WireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Do nothing in the designer to avoid DI initialization in design-time.
        if (DesignerProperties.GetIsInDesignMode(d))
        {
            return;
        }

        // Ignore if the type hasn't actually changed.
        if (Equals(e.NewValue, e.OldValue))
        {
            return;
        }

        if (s_scopeFactory is null)
        {
            throw new InvalidOperationException("ViewModelLocator was not initialized. Call SetServiceScopeFactory(...) at startup.");
        }

        // Dispose any prior scope for this element.
        if (d.GetValue(s_viewModelScopeProperty) is IServiceScope oldScope)
        {
            DetachUnloadHandlerAndDisposeScope(d, oldScope);
            d.ClearValue(s_viewModelScopeProperty);
        }

        // If the new value is null, clear DataContext and exit.
        if (e.NewValue is not Type vmType)
        {
            Bind(d, null);
            return;
        }

        // Create a fresh scope for this view and store it on the element.
        var scope = s_scopeFactory.CreateScope();
        d.SetValue(s_viewModelScopeProperty, scope);

        // Resolve the ViewModel and bind.
        var vm = scope.ServiceProvider.GetRequiredService(vmType);
        Bind(d, vm);

        // Ensure scope is disposed when the element unloads.
        if (d is FrameworkElement fe)
        {
            fe.Unloaded -= FrameworkElement_UnloadedDisposeScope; // avoid duplicates
            fe.Unloaded += FrameworkElement_UnloadedDisposeScope;
        }
    }

    /// <summary>
    /// Unload handler that disposes the per-view DI scope stored on the element.
    /// </summary>
    private static void FrameworkElement_UnloadedDisposeScope(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement frameworkElement && frameworkElement.GetValue(s_viewModelScopeProperty) is IServiceScope scope)
        {
            DetachUnloadHandlerAndDisposeScope(frameworkElement, scope);
            frameworkElement.ClearValue(s_viewModelScopeProperty);
        }
    }

    /// <summary>
    /// Assigns the <paramref name="viewModel"/> as <see cref="FrameworkElement.DataContext"/> if possible.
    /// </summary>
    /// <param name="view">The target view.</param>
    /// <param name="viewModel">The ViewModel instance (or <c>null</c> to clear).</param>
    private static void Bind(object view, object? viewModel)
    {
        if (view is FrameworkElement element)
        {
            element.DataContext = viewModel;
        }
    }

    /// <summary>
    /// Detaches the unload handler from <paramref name="element"/> and disposes <paramref name="scope"/>.
    /// </summary>
    private static void DetachUnloadHandlerAndDisposeScope(DependencyObject element, IServiceScope scope)
    {
        if (element is FrameworkElement fe)
        {
            fe.Unloaded -= FrameworkElement_UnloadedDisposeScope;
        }

        scope.Dispose();
    }
}

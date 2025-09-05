using System.Windows;
using System.Windows.Controls;

namespace PeriForm.Infrastructure.Controls;

public class TimeSpanPicker : Control
{
    private bool _isInternalUpdate;

    static TimeSpanPicker()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(TimeSpanPicker),
            new FrameworkPropertyMetadata(typeof(TimeSpanPicker)));
    }

    public TimeSpanPicker()
    {
        // keep DPs in sync from the start
        UpdatePartsFromSelected();
    }

    #region SelectedTimeSpan

    public TimeSpan SelectedTimeSpan
    {
        get => (TimeSpan)GetValue(SelectedTimeSpanProperty);
        set => SetValue(SelectedTimeSpanProperty, value);
    }

    public static readonly DependencyProperty SelectedTimeSpanProperty =
        DependencyProperty.Register(
            nameof(SelectedTimeSpan),
            typeof(TimeSpan),
            typeof(TimeSpanPicker),
            new FrameworkPropertyMetadata(
                TimeSpan.Zero,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedTimeSpanChanged));

    private static void OnSelectedTimeSpanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TimeSpanPicker)d;
        if (control._isInternalUpdate) return;
        control.UpdatePartsFromSelected();
        control.RaiseEvent(new RoutedEventArgs(SelectedTimeSpanChangedEvent));
    }

    public static readonly RoutedEvent SelectedTimeSpanChangedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(SelectedTimeSpanChanged),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(TimeSpanPicker));

    public event RoutedEventHandler SelectedTimeSpanChanged
    {
        add => AddHandler(SelectedTimeSpanChangedEvent, value);
        remove => RemoveHandler(SelectedTimeSpanChangedEvent, value);
    }

    #endregion SelectedTimeSpan

    #region MaxHours

    public int MaxHours
    {
        get => (int)GetValue(MaxHoursProperty);
        set => SetValue(MaxHoursProperty, value);
    }

    public static readonly DependencyProperty MaxHoursProperty =
        DependencyProperty.Register(
            nameof(MaxHours),
            typeof(int),
            typeof(TimeSpanPicker),
            new PropertyMetadata(23, OnMaxHoursChanged, CoerceMaxHours));

    private static object CoerceMaxHours(DependencyObject d, object baseValue)
    {
        var v = (int)baseValue;
        if (v < 0) v = 0;
        if (v > 999_999) v = 999_999; // arbitrary safety cap
        return v;
    }

    private static void OnMaxHoursChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var c = (TimeSpanPicker)d;
        if (c.Hours > c.MaxHours)
        {
            c.Hours = c.MaxHours;
        }
    }

    #endregion MaxHours

    #region Part properties (Hours/Minutes/Seconds/Milliseconds)

    public int Hours
    {
        get => (int)GetValue(HoursProperty);
        set => SetValue(HoursProperty, value);
    }

    public static readonly DependencyProperty HoursProperty =
        DependencyProperty.Register(
            nameof(Hours),
            typeof(int),
            typeof(TimeSpanPicker),
            new FrameworkPropertyMetadata(
                0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnPartChanged,
                CoerceHours));

    private static object CoerceHours(DependencyObject d, object baseValue)
    {
        var c = (TimeSpanPicker)d;
        var v = (int)baseValue;
        if (v < 0) v = 0;
        if (v > c.MaxHours) v = c.MaxHours;
        return v;
    }

    public int Minutes
    {
        get => (int)GetValue(MinutesProperty);
        set => SetValue(MinutesProperty, value);
    }

    public static readonly DependencyProperty MinutesProperty =
        DependencyProperty.Register(
            nameof(Minutes),
            typeof(int),
            typeof(TimeSpanPicker),
            new FrameworkPropertyMetadata(
                0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnPartChanged,
                CoerceMinuteSecond));

    public int Seconds
    {
        get => (int)GetValue(SecondsProperty);
        set => SetValue(SecondsProperty, value);
    }

    public static readonly DependencyProperty SecondsProperty =
        DependencyProperty.Register(
            nameof(Seconds),
            typeof(int),
            typeof(TimeSpanPicker),
            new FrameworkPropertyMetadata(
                0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnPartChanged,
                CoerceMinuteSecond));

    public int Milliseconds
    {
        get => (int)GetValue(MillisecondsProperty);
        set => SetValue(MillisecondsProperty, value);
    }

    public static readonly DependencyProperty MillisecondsProperty =
        DependencyProperty.Register(
            nameof(Milliseconds),
            typeof(int),
            typeof(TimeSpanPicker),
            new FrameworkPropertyMetadata(
                0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnPartChanged,
                CoerceMilliseconds));

    private static object CoerceMinuteSecond(DependencyObject d, object baseValue)
    {
        var v = (int)baseValue;
        if (v < 0) v = 0;
        if (v > 59) v = 59;
        return v;
    }

    private static object CoerceMilliseconds(DependencyObject d, object baseValue)
    {
        var v = (int)baseValue;
        if (v < 0) v = 0;
        if (v > 999) v = 999;
        return v;
    }

    private static void OnPartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var c = (TimeSpanPicker)d;
        c.UpdateSelectedFromParts();
    }

    #endregion Part properties (Hours/Minutes/Seconds/Milliseconds)

    private void UpdateSelectedFromParts()
    {
        if (_isInternalUpdate) return;

        // Coerce everything before composing
        CoerceValue(HoursProperty);
        CoerceValue(MinutesProperty);
        CoerceValue(SecondsProperty);
        CoerceValue(MillisecondsProperty);

        try
        {
            _isInternalUpdate = true;
            SelectedTimeSpan = new TimeSpan(0, Hours, Minutes, Seconds, Milliseconds);
        }
        finally
        {
            _isInternalUpdate = false;
        }
    }

    private void UpdatePartsFromSelected()
    {
        try
        {
            _isInternalUpdate = true;

            // clamp hours to MaxHours view; if SelectedTimeSpan is longer, we truncate the visual
            var total = SelectedTimeSpan;

            var hours = (int)total.TotalHours;
            if (hours > MaxHours) hours = MaxHours;
            if (hours < 0) hours = 0;

            Hours = hours;
            Minutes = total.Minutes;
            Seconds = total.Seconds;
            Milliseconds = total.Milliseconds;
        }
        finally
        {
            _isInternalUpdate = false;
        }
    }
}

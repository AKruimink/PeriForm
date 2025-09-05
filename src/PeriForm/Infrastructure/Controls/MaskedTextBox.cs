using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PeriForm.Infrastructure.Controls;

public class MaskedTextBox : TextBox
{
    private MaskedTextProvider _provider;
    private bool _updatingText;

    static MaskedTextBox()
    {
        // So bindings to Text update on every keystroke by default
        TextProperty.OverrideMetadata(typeof(MaskedTextBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBaseTextChanged));
    }

    public MaskedTextBox()
    {
        DataObject.AddPastingHandler(this, OnPaste);
        Loaded += (_, __) => EnsureProvider();
    }

    #region Dependency Properties

    public static readonly DependencyProperty MaskProperty =
        DependencyProperty.Register(nameof(Mask), typeof(string), typeof(MaskedTextBox),
            new PropertyMetadata(string.Empty, OnMaskChanged));

    public string Mask
    {
        get => (string)GetValue(MaskProperty);
        set => SetValue(MaskProperty, value);
    }

    public static readonly DependencyProperty CultureProperty =
        DependencyProperty.Register(nameof(Culture), typeof(CultureInfo), typeof(MaskedTextBox),
            new PropertyMetadata(CultureInfo.CurrentCulture, OnCultureChanged));

    public CultureInfo Culture
    {
        get => (CultureInfo)GetValue(CultureProperty);
        set => SetValue(CultureProperty, value);
    }

    public static readonly DependencyProperty PromptCharProperty =
        DependencyProperty.Register(nameof(PromptChar), typeof(char), typeof(MaskedTextBox),
            new PropertyMetadata('_', OnPromptCharChanged));

    public char PromptChar
    {
        get => (char)GetValue(PromptCharProperty);
        set => SetValue(PromptCharProperty, value);
    }

    public static readonly DependencyProperty IncludePromptProperty =
        DependencyProperty.Register(nameof(IncludePrompt), typeof(bool), typeof(MaskedTextBox),
            new PropertyMetadata(false, OnIncludeChanged));

    public bool IncludePrompt
    {
        get => (bool)GetValue(IncludePromptProperty);
        set => SetValue(IncludePromptProperty, value);
    }

    public static readonly DependencyProperty IncludeLiteralsProperty =
        DependencyProperty.Register(nameof(IncludeLiterals), typeof(bool), typeof(MaskedTextBox),
            new PropertyMetadata(true, OnIncludeChanged));

    public bool IncludeLiterals
    {
        get => (bool)GetValue(IncludeLiteralsProperty);
        set => SetValue(IncludeLiteralsProperty, value);
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(string), typeof(MaskedTextBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

    /// <summary>
    /// Unmasked value (no prompts/literals).
    /// </summary>
    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private static readonly DependencyPropertyKey IsMaskCompletedPropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(IsMaskCompleted), typeof(bool), typeof(MaskedTextBox),
            new PropertyMetadata(false));

    public static readonly DependencyProperty IsMaskCompletedProperty =
        IsMaskCompletedPropertyKey.DependencyProperty;

    public bool IsMaskCompleted
    {
        get => (bool)GetValue(IsMaskCompletedProperty);
        private set => SetValue(IsMaskCompletedPropertyKey, value);
    }

    #endregion Dependency Properties

    #region Initialization

    private static void OnMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var tb = (MaskedTextBox)d;
        tb.EnsureProvider();
        tb.SyncTextFromProvider();
    }

    private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var tb = (MaskedTextBox)d;
        tb.EnsureProvider();
        tb.SyncTextFromProvider();
    }

    private static void OnPromptCharChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var tb = (MaskedTextBox)d;
        if (tb._provider != null)
        {
            tb._provider.PromptChar = (char)e.NewValue;
            tb.SyncTextFromProvider();
        }
    }

    private static void OnIncludeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var tb = (MaskedTextBox)d;
        tb.SyncTextFromProvider();
    }

    private void EnsureProvider()
    {
        if (string.IsNullOrEmpty(Mask))
        {
            _provider = null;
            return;
        }

        var prompt = PromptChar == default ? '_' : PromptChar;

        _provider = new MaskedTextProvider(
            mask: Mask,
            culture: Culture ?? CultureInfo.CurrentCulture,
            allowPromptAsInput: false,
            promptChar: prompt,
            passwordChar: '\0',
            restrictToAscii: false);

        _provider.SkipLiterals = true;
        _provider.ResetOnPrompt = true;
        _provider.ResetOnSpace = true;

        // Seed from Value/Text as before
        if (!string.IsNullOrEmpty(Value))
        {
            _provider.Set(string.Empty);
            _provider.Set(Value);
        }
        else if (!string.IsNullOrEmpty(Text))
        {
            _provider.Set(string.Empty);
            _provider.Set(Text);
        }

        UpdateCompleted();
    }

    #endregion Initialization

    #region Text / Value syncing

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var tb = (MaskedTextBox)d;
        if (tb._provider == null)
        {
            tb.EnsureProvider();
        }

        if (tb._provider == null) // still no mask
        {
            tb._updatingText = true;
            tb.Text = e.NewValue?.ToString() ?? string.Empty;
            tb._updatingText = false;
            return;
        }

        tb._provider.Set(string.Empty);
        var newVal = e.NewValue?.ToString() ?? string.Empty;
        tb._provider.Set(newVal);
        tb.SyncTextFromProvider();
    }

    private static void OnBaseTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var tb = (MaskedTextBox)d;
        if (tb._updatingText) return;
        if (tb._provider == null) return;

        // When someone sets Text directly, try to parse it into the provider
        tb._provider.Set(string.Empty);
        tb._provider.Set(e.NewValue?.ToString() ?? string.Empty);
        tb.SyncValueFromProvider();
        tb.UpdateCompleted();
    }

    private void SyncTextFromProvider()
    {
        if (_provider == null)
            return;

        _updatingText = true;
        Text = GetProviderText();
        _updatingText = false;

        SyncValueFromProvider();
        UpdateCompleted();
        CoerceCaretToEditablePosition(CaretIndex);
    }

    private void SyncValueFromProvider()
    {
        if (_provider == null) return;

        var saved = _provider.ToString(false, false); // raw
        if (!string.Equals(Value, saved, StringComparison.Ordinal))
        {
            SetCurrentValue(ValueProperty, saved);
        }
    }

    private string GetProviderText()
    {
        if (_provider == null) return Text;

        return _provider.ToString(
            IncludePrompt,
            IncludeLiterals
        );
    }

    private void UpdateCompleted()
    {
        if (_provider == null)
        {
            IsMaskCompleted = true;
            return;
        }
        IsMaskCompleted = _provider.MaskCompleted;
    }

    #endregion Text / Value syncing

    #region Input handling

    protected override void OnPreviewTextInput(TextCompositionEventArgs e)
    {
        if (_provider == null || string.IsNullOrEmpty(Mask))
        {
            base.OnPreviewTextInput(e);
            return;
        }

        e.Handled = true;

        var selectionStart = SelectionStart;
        var selectionLength = SelectionLength;

        if (selectionLength > 0)
        {
            // Delete current selection first
            RemoveAt(selectionStart, selectionLength);
        }

        var pos = FindEditPositionFrom(selectionStart, forward: true);
        if (pos == -1) return;

        if (_provider.Replace(e.Text, pos))
        {
            SyncTextFromProvider();
            var next = FindEditPositionFrom(pos + 1, forward: true);
            CaretIndex = next == -1 ? pos + 1 : next;
        }
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        if (_provider == null || string.IsNullOrEmpty(Mask))
        {
            base.OnPreviewKeyDown(e);
            return;
        }

        // Navigation
        if (e.Key == Key.Left)
        {
            e.Handled = true;
            CaretIndex = FindEditPositionFrom(CaretIndex - 1, forward: false) switch
            {
                -1 => CaretIndex,
                int p => p
            };
            return;
        }

        if (e.Key == Key.Right)
        {
            e.Handled = true;
            CaretIndex = FindEditPositionFrom(CaretIndex + 1, forward: true) switch
            {
                -1 => CaretIndex,
                int p => p
            };
            return;
        }

        // Delete
        if (e.Key == Key.Delete)
        {
            e.Handled = true;

            if (SelectionLength > 0)
            {
                RemoveAt(SelectionStart, SelectionLength);
                SyncTextFromProvider();
                CaretIndex = FindEditPositionFrom(SelectionStart, forward: true) is int p1 && p1 != -1 ? p1 : SelectionStart;
                return;
            }

            var pos = FindEditPositionFrom(CaretIndex, forward: true);
            if (pos != -1)
            {
                _provider.RemoveAt(pos);
                SyncTextFromProvider();
                CaretIndex = pos;
            }
            return;
        }

        // Backspace
        if (e.Key == Key.Back)
        {
            e.Handled = true;

            if (SelectionLength > 0)
            {
                RemoveAt(SelectionStart, SelectionLength);
                SyncTextFromProvider();
                CaretIndex = FindEditPositionFrom(SelectionStart, forward: false) is int p2 && p2 != -1 ? p2 : SelectionStart;
                return;
            }

            var pos = FindEditPositionFrom(CaretIndex - 1, forward: false);
            if (pos != -1)
            {
                _provider.RemoveAt(pos);
                SyncTextFromProvider();
                CaretIndex = pos;
            }
            return;
        }

        base.OnPreviewKeyDown(e);
    }

    private void OnPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (_provider == null || string.IsNullOrEmpty(Mask))
            return;

        if (!e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true))
            return;

        var pasteText = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string ?? string.Empty;
        e.CancelCommand(); // we’ll handle it

        var start = SelectionStart;
        var len = SelectionLength;

        if (len > 0)
            RemoveAt(start, len);

        var pos = FindEditPositionFrom(start, forward: true);
        if (pos == -1) return;

        // Insert char by char; stop on first failure.
        foreach (var ch in pasteText)
        {
            if (!_provider.Replace(ch, pos))
                break;

            pos = FindEditPositionFrom(pos + 1, forward: true);
            if (pos == -1) break;
        }

        SyncTextFromProvider();
        CaretIndex = pos != -1 ? pos : Text.Length;
    }

    #endregion Input handling

    #region Helpers

    private void RemoveAt(int start, int length)
    {
        // Remove only editable positions
        var end = start + length - 1;
        for (int i = start; i <= end; i++)
        {
            int p = FindEditPositionFrom(i, forward: true);
            if (p == -1 || p > end) break;
            _provider.RemoveAt(p);
        }
    }

    private int FindEditPositionFrom(int start, bool forward)
    {
        if (_provider == null) return -1;

        if (forward)
        {
            for (int i = Math.Max(0, start); i < _provider.Length; i++)
            {
                if (_provider.IsEditPosition(i)) return i;
            }
        }
        else
        {
            for (int i = Math.Min(start, _provider.Length - 1); i >= 0; i--)
            {
                if (_provider.IsEditPosition(i)) return i;
            }
        }
        return -1;
    }

    private void CoerceCaretToEditablePosition(int desired)
    {
        var pos = FindEditPositionFrom(desired, forward: true);
        if (pos == -1)
            pos = FindEditPositionFrom(desired, forward: false);
        if (pos != -1)
            CaretIndex = pos;
    }

    #endregion Helpers
}

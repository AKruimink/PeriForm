namespace PeriForm.Domain.Inputs.Enums;

/// <summary>
/// Represents a keyboard key using its virtual key code value.
/// The values correspond to the VK_ constants defined in WinUser.h.
/// </summary>
public enum KeyCode : ushort
{
    /// <summary>
    /// Left mouse button (not typically used for keyboard input).
    /// </summary>
    LButton = 0x01,

    /// <summary>
    /// Right mouse button (not typically used for keyboard input).
    /// </summary>
    RButton = 0x02,

    /// <summary>
    /// Control-break processing.
    /// </summary>
    Cancel = 0x03,

    /// <summary>
    /// Middle mouse button.
    /// </summary>
    MButton = 0x04,

    /// <summary>
    /// Mouse X1 button.
    /// </summary>
    XButton1 = 0x05,

    /// <summary>
    /// Mouse X2 button.
    /// </summary>
    XButton2 = 0x06,

    /// <summary>
    /// Backspace key.
    /// </summary>
    Back = 0x08,

    /// <summary>
    /// Tab key.
    /// </summary>
    Tab = 0x09,

    /// <summary>
    /// Enter key.
    /// </summary>
    Enter = 0x0D,

    /// <summary>
    /// Left shift key.
    /// </summary>
    Shift = 0x10,

    /// <summary>
    /// Left control key.
    /// </summary>
    Control = 0x11,

    /// <summary>
    /// Left menu key (Alt).
    /// </summary>
    Alt = 0x12,

    /// <summary>
    /// Pause key.
    /// </summary>
    Pause = 0x13,

    /// <summary>
    /// Caps Lock key.
    /// </summary>
    CapsLock = 0x14,

    /// <summary>
    /// Esc key.
    /// </summary>
    Escape = 0x1B,

    /// <summary>
    /// Spacebar.
    /// </summary>
    Space = 0x20,

    /// <summary>
    /// Page up key.
    /// </summary>
    PageUp = 0x21,

    /// <summary>
    /// Page down key.
    /// </summary>
    PageDown = 0x22,

    /// <summary>
    /// End key.
    /// </summary>
    End = 0x23,

    /// <summary>
    /// Home key.
    /// </summary>
    Home = 0x24,

    /// <summary>
    /// Left arrow key.
    /// </summary>
    Left = 0x25,

    /// <summary>
    /// Up arrow key.
    /// </summary>
    Up = 0x26,

    /// <summary>
    /// Right arrow key.
    /// </summary>
    Right = 0x27,

    /// <summary>
    /// Down arrow key.
    /// </summary>
    Down = 0x28,

    /// <summary>
    /// Insert key.
    /// </summary>
    Insert = 0x2D,

    /// <summary>
    /// Delete key.
    /// </summary>
    Delete = 0x2E,

    /// <summary>
    /// The '0' key.
    /// </summary>
    D0 = 0x30,

    /// <summary>
    /// The '1' key.
    /// </summary>
    D1 = 0x31,

    /// <summary>
    /// The '2' key.
    /// </summary>
    D2 = 0x32,

    /// <summary>
    /// The '3' key.
    /// </summary>
    D3 = 0x33,

    /// <summary>
    /// The '4' key.
    /// </summary>
    D4 = 0x34,

    /// <summary>
    /// The '5' key.
    /// </summary>
    D5 = 0x35,

    /// <summary>
    /// The '6' key.
    /// </summary>
    D6 = 0x36,

    /// <summary>
    /// The '7' key.
    /// </summary>
    D7 = 0x37,

    /// <summary>
    /// The '8' key.
    /// </summary>
    D8 = 0x38,

    /// <summary>
    /// The '9' key.
    /// </summary>
    D9 = 0x39,

    /// <summary>
    /// The 'A' key.
    /// </summary>
    A = 0x41,

    /// <summary>
    /// The 'B' key.
    /// </summary>
    B = 0x42,

    /// <summary>
    /// The 'C' key.
    /// </summary>
    C = 0x43,

    /// <summary>
    /// The 'D' key.
    /// </summary>
    D = 0x44,

    /// <summary>
    /// The 'E' key.
    /// </summary>
    E = 0x45,

    /// <summary>
    /// The 'F' key.
    /// </summary>
    F = 0x46,

    /// <summary>
    /// The 'G' key.
    /// </summary>
    G = 0x47,

    /// <summary>
    /// The 'H' key.
    /// </summary>
    H = 0x48,

    /// <summary>
    /// The 'I' key.
    /// </summary>
    I = 0x49,

    /// <summary>
    /// The 'J' key.
    /// </summary>
    J = 0x4A,

    /// <summary>
    /// The 'K' key.
    /// </summary>
    K = 0x4B,

    /// <summary>
    /// The 'L' key.
    /// </summary>
    L = 0x4C,

    /// <summary>
    /// The 'M' key.
    /// </summary>
    M = 0x4D,

    /// <summary>
    /// The 'N' key.
    /// </summary>
    N = 0x4E,

    /// <summary>
    /// The 'O' key.
    /// </summary>
    O = 0x4F,

    /// <summary>
    /// The 'P' key.
    /// </summary>
    P = 0x50,

    /// <summary>
    /// The 'Q' key.
    /// </summary>
    Q = 0x51,

    /// <summary>
    /// The 'R' key.
    /// </summary>
    R = 0x52,

    /// <summary>
    /// The 'S' key.
    /// </summary>
    S = 0x53,

    /// <summary>
    /// The 'T' key.
    /// </summary>
    T = 0x54,

    /// <summary>
    /// The 'U' key.
    /// </summary>
    U = 0x55,

    /// <summary>
    /// The 'V' key.
    /// </summary>
    V = 0x56,

    /// <summary>
    /// The 'W' key.
    /// </summary>
    W = 0x57,

    /// <summary>
    /// The 'X' key.
    /// </summary>
    X = 0x58,

    /// <summary>
    /// The 'Y' key.
    /// </summary>
    Y = 0x59,

    /// <summary>
    /// The 'Z' key.
    /// </summary>
    Z = 0x5A,

    /// <summary>
    /// Left Windows key.
    /// </summary>
    LWin = 0x5B,

    /// <summary>
    /// Right Windows key.
    /// </summary>
    RWin = 0x5C,

    /// <summary>
    /// Applications key (context menu).
    /// </summary>
    Apps = 0x5D,

    /// <summary>
    /// Numeric keypad '0'.
    /// </summary>
    NumPad0 = 0x60,

    /// <summary>
    /// Numeric keypad '1'.
    /// </summary>
    NumPad1 = 0x61,

    /// <summary>
    /// Numeric keypad '2'.
    /// </summary>
    NumPad2 = 0x62,

    /// <summary>
    /// Numeric keypad '3'.
    /// </summary>
    NumPad3 = 0x63,

    /// <summary>
    /// Numeric keypad '4'.
    /// </summary>
    NumPad4 = 0x64,

    /// <summary>
    /// Numeric keypad '5'.
    /// </summary>
    NumPad5 = 0x65,

    /// <summary>
    /// Numeric keypad '6'.
    /// </summary>
    NumPad6 = 0x66,

    /// <summary>
    /// Numeric keypad '7'.
    /// </summary>
    NumPad7 = 0x67,

    /// <summary>
    /// Numeric keypad '8'.
    /// </summary>
    NumPad8 = 0x68,

    /// <summary>
    /// Numeric keypad '9'.
    /// </summary>
    NumPad9 = 0x69,

    /// <summary>
    /// Multiply key on numeric keypad.
    /// </summary>
    Multiply = 0x6A,

    /// <summary>
    /// Add key on numeric keypad.
    /// </summary>
    Add = 0x6B,

    /// <summary>
    /// Subtract key on numeric keypad.
    /// </summary>
    Subtract = 0x6D,

    /// <summary>
    /// Decimal key on numeric keypad.
    /// </summary>
    Decimal = 0x6E,

    /// <summary>
    /// Divide key on numeric keypad.
    /// </summary>
    Divide = 0x6F,

    /// <summary>
    /// F1 function key.
    /// </summary>
    F1 = 0x70,

    /// <summary>
    /// F2 function key.
    /// </summary>
    F2 = 0x71,

    /// <summary>
    /// F3 function key.
    /// </summary>
    F3 = 0x72,

    /// <summary>
    /// F4 function key.
    /// </summary>
    F4 = 0x73,

    /// <summary>
    /// F5 function key.
    /// </summary>
    F5 = 0x74,

    /// <summary>
    /// F6 function key.
    /// </summary>
    F6 = 0x75,

    /// <summary>
    /// F7 function key.
    /// </summary>
    F7 = 0x76,

    /// <summary>
    /// F8 function key.
    /// </summary>
    F8 = 0x77,

    /// <summary>
    /// F9 function key.
    /// </summary>
    F9 = 0x78,

    /// <summary>
    /// F10 function key.
    /// </summary>
    F10 = 0x79,

    /// <summary>
    /// F11 function key.
    /// </summary>
    F11 = 0x7A,

    /// <summary>
    /// F12 function key.
    /// </summary>
    F12 = 0x7B
}

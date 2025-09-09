namespace PeriForm.Domain.Inputs.Native.Enums;

/// <summary>
/// Specifies the type of low‑level hook to install using SetWindowsHookEx.
/// Values correspond to the WH_* constants defined in WinUser.h.
/// </summary>
internal enum HookType
{
    /// <summary>
    /// Installs a low‑level keyboard hook that monitors keyboard input events.
    /// </summary>
    LowLevelKeyboard = 13,

    /// <summary>
    /// Installs a low‑level mouse hook that monitors mouse input events.
    /// </summary>
    LowLevelMouse = 14
}

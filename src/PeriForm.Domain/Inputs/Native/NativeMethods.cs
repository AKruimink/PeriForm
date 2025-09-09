using System.Runtime.InteropServices;
using PeriForm.Domain.Inputs.Native.Enums;
using PeriForm.Domain.Inputs.Native.Structs;

namespace PeriForm.Domain.Inputs.Native;

/// <summary>
/// Contains Platform Invocation (P/Invoke) declarations for interacting with the Windows user32.dll.
/// These methods form the basis for simulating keyboard and mouse input as well as registering and handling global hooks and hotkeys.
/// </summary>
internal static partial class NativeMethods
{
    /// <summary>
    /// Sends a sequence of input events (keyboard, mouse or hardware) to the system.
    /// Each call to <see cref="SendInput"/> is atomic, either all events are injected or none are.
    /// The calling thread must be running in a context that allows user input (e.g. not a service session).
    /// </summary>
    /// <param name="nInputs">The number of events contained in the <paramref name="pInputs"/> array.</param>
    /// <param name="pInputs">An array of <see cref="Input"/> structures describing each event.</param>
    /// <param name="cbSize">The size of the <see cref="Input"/> structure in bytes.</param>
    /// <returns>The number of events successfully inserted, typically equals <paramref name="nInputs"/> or zero on failure.</returns>
    [LibraryImport("user32.dll", EntryPoint = "SendInput", SetLastError = true)]
    internal static partial uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

    /// <summary>
    /// Retrieves the current cursor position, in screen coordinates.
    /// </summary>
    /// <param name="lpPoint">On success, receives the screen coordinates of the cursor.</param>
    /// <returns><c>true</c> if successful, otherwise, <c>false</c> with details available via <see cref="Marshal.GetLastWin32Error"/>.</returns>
    [LibraryImport("user32.dll", EntryPoint = "GetCursorPos", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool GetCursorPos(out Point lpPoint);

    /// <summary>
    /// Moves the cursor to the specified screen coordinates.
    /// </summary>
    /// <param name="X">The x‑coordinate of the cursor, in pixels.</param>
    /// <param name="Y">The y‑coordinate of the cursor, in pixels.</param>
    /// <returns><c>true</c> if successful, otherwise, <c>false</c>.</returns>
    [LibraryImport("user32.dll", EntryPoint = "SetCursorPos", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool SetCursorPos(int x, int y);

    /// <summary>
    /// Installs an application‑defined hook procedure into a hook chain.
    /// Low‑level keyboard and mouse hooks use this function to intercept input globally.
    /// </summary>
    /// <param name="idHook">The type of hook to install (see <see cref="HookType"/>).</param>
    /// <param name="lpfn">A delegate to the hook procedure.</param>
    /// <param name="hMod">A handle to the DLL containing the hook procedure or <see cref="IntPtr.Zero"/> if the procedure is in the calling process.</param>
    /// <param name="dwThreadId">The identifier of the thread to install the hook for, zero installs the hook globally.</param>
    /// <returns>A handle to the hook procedure, use <see cref="UnhookWindowsHookEx"/> to remove it.</returns>
    [LibraryImport("user32.dll", EntryPoint = "SetWindowsHookEx", SetLastError = true)]
    internal static partial IntPtr SetWindowsHookEx(HookType idHook, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);

    /// <summary>
    /// Removes a hook installed by <see cref="SetWindowsHookEx"/>.
    /// </summary>
    /// <param name="hhk">A handle to the hook to remove.</param>
    /// <returns><c>true</c> if successful, otherwise, <c>false</c>.</returns>
    [LibraryImport("user32.dll", EntryPoint = "UnhookWindowsHookEx", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool UnhookWindowsHookEx(IntPtr hhk);

    /// <summary>
    /// Passes the hook information to the next hook procedure in the chain.
    /// </summary>
    /// <param name="hhk">A handle to the current hook.</param>
    /// <param name="nCode">The hook code passed to the current hook.</param>
    /// <param name="wParam">Additional message information.</param>
    /// <param name="lParam">Additional message information.</param>
    /// <returns>The value returned by the next hook in the chain.</returns>
    [LibraryImport("user32.dll", EntryPoint = "CallNextHookEx", SetLastError = true)]
    internal static partial IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Registers a system‑wide hotkey that will generate a <see cref="WindowsMessage.HotKey"/> message when pressed.
    /// </summary>
    /// <param name="hWnd">Handle to the window that will receive the hotkey messages, can be <see cref="IntPtr.Zero"/> for a message‑only window.</param>
    /// <param name="id">An application‑defined identifier for the hotkey.</param>
    /// <param name="fsModifiers">Modifier keys (see <see cref="ModifierKeys"/>).</param>
    /// <param name="vk">Virtual key code of the hotkey.</param>
    /// <returns><c>true</c> if successful, otherwise, <c>false</c>.</returns>
    [LibraryImport("user32.dll", EntryPoint = "RegisterHotKey", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool RegisterHotKey(IntPtr hWnd, int id, ModifierKeys fsModifiers, ushort vk);

    /// <summary>
    /// Unregisters a system‑wide hotkey registered by <see cref="RegisterHotKey"/>.
    /// </summary>
    /// <param name="hWnd">Handle to the window associated with the hotkey.</param>
    /// <param name="id">Identifier of the hotkey to unregister.</param>
    /// <returns><c>true</c> if successful, otherwise, <c>false</c>.</returns>
    [LibraryImport("user32.dll", EntryPoint = "UnregisterHotKey", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool UnregisterHotKey(IntPtr hWnd, int id);

    /// <summary>
    /// Delegate type for low‑level keyboard and mouse hook procedures.
    /// </summary>
    /// <param name="nCode">The hook code.</param>
    /// <param name="wParam">Message-specific information (see <see cref="WindowsMessage"/>).</param>
    /// <param name="lParam">Pointer to a structure that contains details about the message.</param>
    /// <returns>An <see cref="IntPtr"/> value indicating how to proceed in the hook chain.</returns>
    internal delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);
}

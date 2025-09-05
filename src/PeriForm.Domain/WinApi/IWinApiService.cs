using PeriForm.Domain.WinApi.Types;

namespace PeriForm.Domain.WinApi;

/// <summary>
/// Abstraction over the WinApi functions used by the hot‑key service.
/// </summary>
public interface IWinApiService
{
    bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    bool UnregisterHotKey(IntPtr hWnd, int id);

    sbyte VkKeyScan(char ch);

    bool GetMessage(out NativeTypes.MSG msg, IntPtr hWnd, uint filterMin, uint filterMax);

    bool TranslateMessage(ref NativeTypes.MSG msg);

    IntPtr DispatchMessage(ref NativeTypes.MSG msg);

    void PostQuitMessage(int exitCode);

    IntPtr CreateMessageOnlyWindow(string className);

    uint ModAlt { get; }

    uint ModControl { get; }

    uint ModShift { get; }

    uint ModWin { get; }
}

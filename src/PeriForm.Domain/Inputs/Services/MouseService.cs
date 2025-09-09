using System.Runtime.InteropServices;
using PeriForm.Domain.Inputs.Enums;
using PeriForm.Domain.Inputs.Interfaces;
using PeriForm.Domain.Inputs.Native;
using PeriForm.Domain.Inputs.Native.Enums;
using PeriForm.Domain.Inputs.Native.Structs;

namespace PeriForm.Domain.Inputs.Services;

/// <summary>
/// Default implementation of <see cref="IMouseService"/> using
/// <see cref="NativeMethods.SetCursorPos"/> and <see cref="NativeMethods.SendInput"/>
/// to simulate mouse movement and clicks.
/// </summary>
public class MouseService : IMouseService
{
    /// <inheritdoc />
    public Task MoveTo(int x, int y)
    {
        if (!NativeMethods.SetCursorPos(x, y))
        {
            throw new InvalidOperationException($"SetCursorPos failed: {Marshal.GetLastWin32Error()}");
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task MoveBy(int deltaX, int deltaY)
    {
        if (!NativeMethods.GetCursorPos(out var point))
        {
            throw new InvalidOperationException($"GetCursorPos failed: {Marshal.GetLastWin32Error()}");
        }

        var newX = point.X + deltaX;
        var newY = point.Y + deltaY;
        if (!NativeMethods.SetCursorPos(newX, newY))
        {
            throw new InvalidOperationException($"SetCursorPos failed: {Marshal.GetLastWin32Error()}");
        }

        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task Click(MouseButton button, TimeSpan holdDuration)
    {
        // Determine the flags and additional data based on the button
        MouseEventFlags downFlag;
        MouseEventFlags upFlag;
        uint mouseData = 0;

        switch (button)
        {
            case MouseButton.Left:
                downFlag = MouseEventFlags.LeftDown;
                upFlag = MouseEventFlags.LeftUp;
                break;

            case MouseButton.Right:
                downFlag = MouseEventFlags.RightDown;
                upFlag = MouseEventFlags.RightUp;
                break;

            case MouseButton.Middle:
                downFlag = MouseEventFlags.MiddleDown;
                upFlag = MouseEventFlags.MiddleUp;
                break;

            case MouseButton.XButton1:
                downFlag = MouseEventFlags.XDown;
                upFlag = MouseEventFlags.XUp;
                mouseData = 1; // XButton1
                break;

            case MouseButton.XButton2:
                downFlag = MouseEventFlags.XDown;
                upFlag = MouseEventFlags.XUp;
                mouseData = 2; // XButton2
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(button), button, null);
        }

        // Build mouse down
        var down = new Input
        {
            Type = InputType.Mouse,
            Data = new InputUnion
            {
                Mouse = new MouseInput
                {
                    Dx = 0,
                    Dy = 0,
                    MouseData = mouseData,
                    Flags = downFlag,
                    Time = 0,
                    ExtraInfo = IntPtr.Zero
                }
            }
        };

        // Build mouse up
        var up = new Input
        {
            Type = InputType.Mouse,
            Data = new InputUnion
            {
                Mouse = new MouseInput
                {
                    Dx = 0,
                    Dy = 0,
                    MouseData = mouseData,
                    Flags = upFlag,
                    Time = 0,
                    ExtraInfo = IntPtr.Zero
                }
            }
        };

        // Send down
        var downResult = NativeMethods.SendInput(1, [down], Marshal.SizeOf<Input>());
        if (downResult == 0)
        {
            throw new InvalidOperationException($"SendInput failed to send mouse down for {button}: {Marshal.GetLastWin32Error()}");
        }

        // Hold if needed
        if (holdDuration > TimeSpan.Zero)
        {
            await Task.Delay(holdDuration);
        }

        // Send up
        var upResult = NativeMethods.SendInput(1, [up], Marshal.SizeOf<Input>());
        if (upResult == 0)
        {
            throw new InvalidOperationException($"SendInput failed to send mouse up for {button}: {Marshal.GetLastWin32Error()}");
        }
    }

    /// <inheritdoc />
    public async Task ClickAt(int x, int y, MouseButton button, TimeSpan holdDuration)
    {
        await MoveTo(x, y);
        await Click(button, holdDuration);
    }

    /// <inheritdoc />
    public Task<(int X, int Y)> GetPosition()
    {
        if (!NativeMethods.GetCursorPos(out var point))
        {
            throw new InvalidOperationException($"GetCursorPos failed: {Marshal.GetLastWin32Error()}");
        }

        return Task.FromResult((point.X, point.Y));
    }
}

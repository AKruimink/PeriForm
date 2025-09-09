using PeriForm.Domain.Inputs.Enums;

namespace PeriForm.Domain.Inputs.Interfaces;

/// <summary>
/// Provides methods for moving the mouse cursor and performing click events.
/// </summary>
public interface IMouseService
{
    /// <summary>
    /// Moves the mouse to the specified absolute screen coordinates.
    /// </summary>
    /// <param name="x">The target x‑coordinate in pixels.</param>
    /// <param name="y">The target y‑coordinate in pixels.</param>
    Task MoveTo(int x, int y);

    /// <summary>
    /// Moves the mouse relative to its current position.
    /// </summary>
    /// <param name="deltaX">The horizontal distance to move.</param>
    /// <param name="deltaY">The vertical distance to move.</param>
    Task MoveBy(int deltaX, int deltaY);

    /// <summary>
    /// Clicks a mouse button.
    /// Optionally holds the button down for <paramref name="holdDuration"/> before releasing it.
    /// </summary>
    /// <param name="button">The button to click.</param>
    /// <param name="holdDuration">How long to hold the button down.</param>
    Task Click(MouseButton button, TimeSpan holdDuration);

    /// <summary>
    /// Moves the mouse to a coordinate and performs a click.
    /// </summary>
    /// <param name="x">The target x‑coordinate in pixels.</param>
    /// <param name="y">The target y‑coordinate in pixels.</param>
    /// <param name="button">The button to click.</param>
    /// <param name="holdDuration">How long to hold the button down.</param>
    Task ClickAt(int x, int y, MouseButton button, TimeSpan holdDuration);

    /// <summary>
    /// Retrieves the current cursor position in screen coordinates.
    /// </summary>
    /// <returns>A tuple containing the x and y coordinates.</returns>
    Task<(int X, int Y)> GetPosition();
}

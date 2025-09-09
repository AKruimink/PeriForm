using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using PeriForm.Domain.Infrastructure.Enums;
using PeriForm.Domain.Inputs.Enums;
using PeriForm.Domain.Inputs.Models;
using PeriForm.Infrastructure.ViewModel;

namespace PeriForm.Clickers
{
    public class ClickerItemViewModel : ViewModelBase
    {
        private Guid _id;
        private string _name = string.Empty;
        private TimeSpan _interval = TimeSpan.FromMilliseconds(100);
        private TimeSpan _holdDuration = TimeSpan.Zero;
        private MouseButton _button = MouseButton.Left;
        private bool _useFixedPosition;
        private int _positionX;
        private int _positionY;
        private int? _maxClicks;
        private TimeSpan? _maxDuration;
        private RunMode _runMode = Domain.Infrastructure.Enums.RunMode.Indefinite;
        private Hotkey? _startHotkey;
        private Hotkey? _stopHotkey;

        /// <summary>
        /// Unique identifier for this clicker.  This is typically set by the
        /// domain and should not change once created.
        /// </summary>
        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        /// <summary>
        /// Human‑readable name shown in the list and summary.  Optional, but if
        /// empty the summary will display "(unnamed)".
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Interval between clicks.  A zero or negative interval causes the
        /// clicker to click continuously without delay.
        /// </summary>
        public TimeSpan Interval
        {
            get => _interval;
            set => SetProperty(ref _interval, value);
        }

        /// <summary>
        /// Duration to hold the mouse button down for each click.
        /// </summary>
        public TimeSpan HoldDuration
        {
            get => _holdDuration;
            set => SetProperty(ref _holdDuration, value);
        }

        /// <summary>
        /// The mouse button used for clicking.
        /// </summary>
        public MouseButton Button
        {
            get => _button;
            set => SetProperty(ref _button, value);
        }

        /// <summary>
        /// If true, the clicker always clicks at <see cref="PositionX"/> and
        /// <see cref="PositionY"/>.  If false, the current cursor location is used.
        /// </summary>
        public bool UseFixedPosition
        {
            get => _useFixedPosition;
            set => SetProperty(ref _useFixedPosition, value);
        }

        /// <summary>
        /// X coordinate for fixed click positions.
        /// </summary>
        public int PositionX
        {
            get => _positionX;
            set => SetProperty(ref _positionX, value);
        }

        /// <summary>
        /// Y coordinate for fixed click positions.
        /// </summary>
        public int PositionY
        {
            get => _positionY;
            set => SetProperty(ref _positionY, value);
        }

        /// <summary>
        /// Maximum number of clicks to perform.  Only used when
        /// <see cref="RunMode"/> is <see cref="ClickerRunMode.ExecutionCount"/>.
        /// </summary>
        public int? MaxClicks
        {
            get => _maxClicks;
            set => SetProperty(ref _maxClicks, value);
        }

        /// <summary>
        /// Maximum duration to run.  Only used when <see cref="RunMode"/> is
        /// <see cref="ClickerRunMode.Duration"/>.
        /// </summary>
        public TimeSpan? MaxDuration
        {
            get => _maxDuration;
            set => SetProperty(ref _maxDuration, value);
        }

        /// <summary>
        /// Determines how this clicker ends: indefinitely, by count, or by
        /// duration.  Changing this value resets the unused properties.
        /// </summary>
        public RunMode RunMode
        {
            get => _runMode;
            set
            {
                if (SetProperty(ref _runMode, value))
                {
                    // Reset the unused fields when switching modes
                    if (value == RunMode.ExecutionCount)
                    {
                        _maxDuration = null;
                    }
                    else if (value == RunMode.Duration)
                    {
                        _maxClicks = null;
                    }
                    else
                    {
                        _maxClicks = null;
                        _maxDuration = null;
                    }

                    //// Notify for dependent properties
                    //OnPropertyChanged(nameof(MaxClicks));
                    //OnPropertyChanged(nameof(MaxDuration));
                }
            }
        }

        /// <summary>
        /// Hotkey that starts this clicker.  If null, the clicker must be started
        /// programmatically.
        /// </summary>
        public Hotkey? StartHotkey
        {
            get => _startHotkey;
            set => SetProperty(ref _startHotkey, value);
        }

        /// <summary>
        /// Hotkey that stops this clicker.  When equal to the start hotkey it
        /// toggles based on the current state.  If null, the clicker must be
        /// stopped programmatically.
        /// </summary>
        public Hotkey? StopHotkey
        {
            get => _stopHotkey;
            set => SetProperty(ref _stopHotkey, value);
        }
    }
}

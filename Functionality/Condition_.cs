using System;

public enum Mode
{
    None, DrawRectMode, DrawLineMode,
    DrawPolyline,
    DrawPolylineProcess,
    MoveMarker,
    DrawLineProcess,
    DrawRectProcess
}

namespace GraphicEditor.Functionality
{
    /// <summary>
    /// Этот класс создан как Альтернатива WorkplaceCondition.xaml.cs
    /// </summary>
    public class Condition_
    {
        public Mode Mode { get; set; }

        public void Reset()
        {
            Mode = Mode.None;
        }
    }
}

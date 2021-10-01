using System;

public enum Mode { None, DrawRectMode, DrawLineMode,
    DrawPolyline,
    DrawPolylineProcess,
    MoveMarker
}
public enum MouseAction { LeftButtonDown, LeftButtonUp, RightButtonDown, Move}

namespace GraphicEditor.Functionality
{
    /// <summary>
    /// Этот класс создан как Альтернатива WorkplaceCondition.xaml.cs
    /// </summary>
    public class Condition_
    {
        public Mode Mode { get; set; }
        public MouseAction MouseAction { get; set; }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        internal void Click(MouseAction leftButtonDown)
        {
            throw new NotImplementedException();
        }
    }
}

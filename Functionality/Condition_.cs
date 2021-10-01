using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Mode { None, DrawRectMode, DrawLineMode }
public enum Action { DrawRect, DrawLine, DrawPolyline, SelectFigure, MoveMarker }
public enum MouseAction { LMB_Down, LMB_Up, RMB_Down, Move}

namespace GraphicEditor.Functionality
{
    public class Condition_
    {
        public Mode Mode { get; set; }
        public Action Action { get; set; }
        public MouseAction MouseAction { get; set; }


    }
}

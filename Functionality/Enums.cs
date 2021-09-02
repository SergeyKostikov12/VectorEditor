using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum FigureType { Rectangle, Line }
public enum ClickCondition { Click, Drag, DoubleClick}
public enum Actions { DrawRect, DrawLine, DrawPolyline, MoveRect, RotateRect, ScaleRect, MovePoint, None }
public enum ButtonPressed { None, Load, Save, Rect, Line, Width, Color, Fill }
public enum CursorType { Crosshair, Arrow, Hand }


namespace GraphicEditor.Functionality
{
}

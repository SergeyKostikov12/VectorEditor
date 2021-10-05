using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphicEditor.Functionality
{
    public class ClickDispatcher
    {
        private int timeDown;
        private int timeUp;

        private bool isLMB_Down;
        private bool isLMB_Up;
        private bool isDrag;
        private bool isRMB_Down;
        private bool isClick;

        private WorkplaceCondition Condition;

        public ClickDispatcher(WorkplaceCondition condition)
        {
            Condition = condition;
        }
        public void LMB_Down(int time)
        {
            isLMB_Down = true;
            isLMB_Up = false;
            isClick = false;
            isDrag = false;
            isRMB_Down = false;
            timeDown = time;
            CalculateResult();
        }
        public void LMB_UP(int time)
        {
            isLMB_Up = true;
            isLMB_Down = false;
            isClick = false;
            isDrag = false;
            isRMB_Down = false;
            timeUp = time;
            CalculateTime();
            CalculateResult();
        }

        private void CalculateTime()
        {
            if (timeDown - timeUp <= 500)
            {
                isClick = true;
                isLMB_Down = false;
                isLMB_Up = false;
            }
        }

        public void Move()
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
            {
                isDrag = true;
            }
        }
        public void RMB_Down()
        {
            isRMB_Down = true;
            isLMB_Down = false;
            isLMB_Up = false;
            isClick = false;
            isDrag = false;
            CalculateResult();
        }

        public void CalculateResult()
        {
            //Начало рисования фигур
            if (Condition.DrawingMode == DrawingMode.RectangleMode)
            {
                if (isLMB_Down)
                {
                    Condition.DrawingMode = DrawingMode.StartDrawRectangle;
                }
            }
            else if (Condition.DrawingMode == DrawingMode.LineMode)
            {
                if (isLMB_Down)
                {
                    Condition.DrawingMode = DrawingMode.StartDrawLine;
                }
            }

            //Процесс рисования фигур
            if (Condition.DrawingMode == DrawingMode.StartDrawRectangle && isDrag)
            {
                Condition.DrawingMode = DrawingMode.DrawRectangleProcess;
            }
            else if (Condition.DrawingMode == DrawingMode.StartDrawLine && isDrag)
            {
                Condition.DrawingMode = DrawingMode.DrawSingleLineProcess;
            }
            else if (Condition.DrawingMode == DrawingMode.StartDrawLine && isClick)
            {
                Condition.DrawingMode = DrawingMode.DrawPolylineProcess;
            }

            //Конец рисования фигур
            if (Condition.DrawingMode == DrawingMode.DrawRectangleProcess && isLMB_Up)
            {
                Condition.DrawingMode = DrawingMode.EndDrawRectangle;
            }
            else if (Condition.DrawingMode == DrawingMode.DrawSingleLineProcess && isLMB_Up)
            {
                Condition.DrawingMode = DrawingMode.EndDrawSingleLine;
            }
            else if (Condition.DrawingMode == DrawingMode.DrawPolylineProcess && isRMB_Down)
            {
                Condition.DrawingMode = DrawingMode.EndDrawPolyline;
            }

            //Прочие действия во время процесса рисования фигур
            if (Condition.DrawingMode == DrawingMode.DrawPolylineProcess && isClick)
            {
                Condition.Action = Action.AddPoint;
            }

            //Клик вне режима рисования
            if (Condition.DrawingMode == DrawingMode.None && isClick)
            {
                Condition.Action = Action.SelectFigure;
            }

            //Перетаскивание
            if (Condition.DrawingMode == DrawingMode.None && isLMB_Down && isDrag)
            {
                Condition.Action = Action.MoveMarker;
            }
            else if (Condition.DrawingMode == DrawingMode.None && isRMB_Down && isDrag)
            {
                Condition.Action = Action.MoveWorkplace;
            }
        }
    }
}

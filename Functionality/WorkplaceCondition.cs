namespace GraphicEditor.Functionality
{
    public class WorkplaceCondition
    {

        public Mode Mode;
        public Actions Action;
        public DrawingMode DrawingMode;
        public bool MouseDown = false;
        public bool MouseDrag = false;
        public bool MouseUp = false;

        public void ResetCondition()
        {
            Action = Actions.None;
            DrawingMode = DrawingMode.None;
        }
        public bool IsDrawingLine()
        {
            return MouseDown && MouseDrag; 
        }
        public void ResetMouseState()
        {
            MouseDown = false;
            MouseDrag = false;
            MouseUp = false;
        }
    }
}

namespace GraphicEditor.Functionality
{
    public class WorkplaceCondition
    {
        public Actions Action;
        public DrawingMode DrawingMode;
        public bool MouseDown = false;
        public bool MouseDrag = false;
        public bool MouseUp = false;

        internal void ResetCondition()
        {
            Action = Actions.None;
            DrawingMode = DrawingMode.None;
        }
        public bool IsDrawLine()
        {
            return MouseDown & MouseDrag; 
        }
        public void ResetMouseState()
        {
            MouseDown = false;
            MouseDrag = false;
            MouseUp = false;
        }
    }
}

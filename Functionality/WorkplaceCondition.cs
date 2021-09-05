namespace GraphicEditor.Functionality
{
    public class WorkplaceCondition
    {
        public Actions Action;
        public ButtonPressed ButtonPressed;

        public bool MouseDown = false;
        public bool MouseDrag = false;
        public bool MouseUp = false;

        public bool IsDrawLine()
        {
            if (MouseDown & MouseDrag & MouseDrag) return true;
            else if (MouseDown & MouseUp) return false;
            else return false;
        }
        public void ResetMouseState()
        {
            MouseDown = false;
            MouseDrag = false;
            MouseUp = false;
        }
        internal void ResetCondition()
        {
            Action = Actions.None;
            ButtonPressed = ButtonPressed.None;
        }
    }
}

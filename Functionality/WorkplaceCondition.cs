namespace GraphicEditor.Functionality
{
    public class WorkplaceCondition
    {

        public Action Action {  get; set; }
        public DrawingMode DrawingMode {  get; set; }
        public bool IsFigureSelected { get; set; }

        public void ResetCondition()
        {
            Action = Action.None;
            DrawingMode = DrawingMode.None;
            IsFigureSelected = false;
        }
    }
}

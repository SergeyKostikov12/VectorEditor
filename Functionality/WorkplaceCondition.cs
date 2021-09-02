using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicEditor.Functionality
{
    public class WorkplaceCondition
    {
        private bool IsDraw = false;
        private bool IsMoving = false;
        private bool IsRotating = false;
        private bool IsScaling = false;
        private bool IsFirstPoint = true;
        private bool IsLineSelect = false;
        private bool IsPointMove = false;
        private bool IsWorkplaceMoved = false;

        public Actions Action;
        public ButtonPressed ButtonPressed;
        public ClickCondition ClickCondition;

        internal void ResetCondition()
        {
            throw new NotImplementedException();
        }
    }
}

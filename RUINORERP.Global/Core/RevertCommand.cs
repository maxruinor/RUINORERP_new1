using System;
using System.Collections.Generic;
using System.Text;

namespace RUINOR.Core
{
    public class RevertCommand : IRevertCommand
    {
        private ActionHandler doOperation;

        private ActionHandler undoOperation;

        #region ICommand Members

        public ActionHandler DoOperation
        {
            get
            {
                return doOperation;
            }
            set
            {
                doOperation = value;
            }
        }

        public ActionHandler UndoOperation
        {
            get
            {
                return undoOperation;
            }
            set
            {
                undoOperation = value;
            }
        }

        public void Do()
        {
            doOperation();
        }

        public void Undo()
        {
            undoOperation();
        }

        #endregion
    }
}

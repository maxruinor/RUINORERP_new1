using System;
using System.Collections.Generic;
using System.Text;

namespace RUINOR.Core
{
    public delegate void ActionHandler();
    public interface IRevertCommand
    {
        ActionHandler DoOperation { get; set; }
        ActionHandler UndoOperation { get; set; }

        void Do();
        void Undo();
    }
}
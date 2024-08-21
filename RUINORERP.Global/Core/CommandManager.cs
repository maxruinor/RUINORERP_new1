using System;
using System.Collections.Generic;
using System.Text;

namespace RUINOR.Core
{
    public static class CommandManager
    {

        //public bool CanUnDo { get { return undoMementos.Count != 0; } }
        //public bool CanReDo { get { return redoMementos.Count != 0; } }

        private static Stack<ICommand> redoMementos;

        private static Stack<ICommand> undoMementos;

        static CommandManager()
        {
            redoMementos = new Stack<ICommand>();
            undoMementos = new Stack<ICommand>();
        }

        /// <summary>
        /// 先清空Redo栈，将Command对象压入栈
        /// </summary>
        /// <param name="command">Command对象</param>
        public static void AddNewCommand(ICommand command)
        {
            redoMementos.Clear();
            undoMementos.Push(command);
        }

        /// <summary>
        /// 从Redo栈中弹出一个Command对象，执行Do操作，并且将该Command对象压入Undo栈
        /// </summary>
        public static void Redo()
        {
            try
            {
                if (redoMementos.Count > 0)
                {
                    ICommand command = redoMementos.Pop();
                    if (command != null)
                    {
                        undoMementos.Push(command);
                        command.Do();
                    }
                }
            }
            catch (InvalidOperationException invalidOperationException)
            {
                Console.WriteLine(invalidOperationException.Message);
            }
        }

        /// <summary>
        /// 从Undo栈中弹出一个Command对象，执行Undo操作，并且将该Command对象压入Redo栈
        /// </summary>
        public static void Undo()
        {
            try
            {
                if (undoMementos.Count > 0)
                {
                    ICommand command = undoMementos.Pop();
                    if (command != null)
                    {
                        command.Undo();
                        redoMementos.Push(command);
                    }
                }
            }
            catch (InvalidOperationException invalidOperationException)
            {
                Console.WriteLine(invalidOperationException.Message);
            }
        }

        /// <summary>
        /// 清空Redo栈与Undo栈
        /// </summary>
        public static void ClearAll()
        {
            ClearRedoStack();
            ClearUndoStack();
        }

        /// <summary>
        /// 清空Redo栈
        /// </summary>
        public static void ClearRedoStack()
        {
            redoMementos.Clear();
        }

        /// <summary>
        /// 清空Undo栈
        /// </summary>
        public static void ClearUndoStack()
        {
            undoMementos.Clear();
        }

        /// <summary>
        /// 当前在Redo栈中能重做的步骤数
        /// </summary>
        public static int RedoStepsCount
        {
            get
            {
                return redoMementos.Count;
            }
        }

        /// <summary>
        /// 当前在Undo栈中能撤消的步骤数
        /// </summary>
        public static int UndoStepsCount
        {
            get
            {
                return undoMementos.Count;
            }
        }
    }
}

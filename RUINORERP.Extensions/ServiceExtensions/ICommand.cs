using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Extensions.ServiceExtensions
{
    /// <summary>
    /// 撤销重做命令模式
    /// </summary>
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    public class ConfigFileReceiver
    {
        private readonly string _filePath;

        public ConfigFileReceiver(string filePath)
        {
            _filePath = filePath;
        }

        public JObject LoadConfig()
        {
            return File.Exists(_filePath) ? JObject.Parse(File.ReadAllText(_filePath)) : new JObject();
        }

        public void SaveConfig(JObject config)
        {
            File.WriteAllText(_filePath, config.ToString());
        }
    }

    public class EditConfigCommand : ICommand
    {
        private readonly ConfigFileReceiver _receiver;
        private readonly JObject _originalState;
        private readonly JObject _newState;

        public EditConfigCommand(ConfigFileReceiver receiver, JObject newState)
        {
            _receiver = receiver;
            _originalState = receiver.LoadConfig();
            _newState = newState;
        }

        public void Execute()
        {
            _receiver.SaveConfig(_newState);
        }

        public void Undo()
        {
            _receiver.SaveConfig(_originalState);
        }

        public class CommandManager
        {
            private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
            private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();

            public void ExecuteCommand(ICommand command)
            {
                command.Execute();
                _undoStack.Push(command);
                _redoStack.Clear();
            }

            public void UndoCommand()
            {
                if (_undoStack.Count > 0)
                {
                    var command = _undoStack.Pop();
                    command.Undo();
                    _redoStack.Push(command);
                }
            }

            public void RedoCommand()
            {
                if (_redoStack.Count > 0)
                {
                    var command = _redoStack.Pop();
                    command.Execute();
                    _undoStack.Push(command);
                }
            }
        }
    }
}

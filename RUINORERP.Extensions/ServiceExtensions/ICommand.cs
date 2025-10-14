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
        private readonly string _operationDescription;

        public EditConfigCommand(ConfigFileReceiver receiver, JObject newState, string operationDescription = "配置编辑")
        {
            _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
            _originalState = receiver.LoadConfig();
            _newState = newState ?? throw new ArgumentNullException(nameof(newState));
            _operationDescription = operationDescription;
        }

        public string Description => _operationDescription;

        public void Execute()
        {
            try
            {
                ValidateConfiguration(_newState);
                _receiver.SaveConfig(_newState);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"执行 '{_operationDescription}' 失败: {ex.Message}", ex);
            }
        }

        public void Undo()
        {
            try
            {
                _receiver.SaveConfig(_originalState);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"撤销 '{_operationDescription}' 失败: {ex.Message}", ex);
            }
        }

        private void ValidateConfiguration(JObject config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config), "配置对象不能为空");

            // 验证服务器配置
            if (config["SystemGlobalconfig"] != null)
            {
                var serverConfig = config["SystemGlobalconfig"];
                
                // 验证服务器端口
                if (serverConfig["ServerPort"] != null)
                {
                    var port = serverConfig["ServerPort"].Value<int>();
                    if (port < 1 || port > 65535)
                        throw new ArgumentException("服务器端口必须在1-65535范围内");
                }

                // 验证最大连接数
                if (serverConfig["MaxConnections"] != null)
                {
                    var maxConnections = serverConfig["MaxConnections"].Value<int>();
                    if (maxConnections < 1 || maxConnections > 10000)
                        throw new ArgumentException("最大连接数必须在1-10000范围内");
                }

                // 验证心跳间隔
                if (serverConfig["HeartbeatInterval"] != null)
                {
                    var heartbeatInterval = serverConfig["HeartbeatInterval"].Value<int>();
                    if (heartbeatInterval < 1000 || heartbeatInterval > 60000)
                        throw new ArgumentException("心跳间隔必须在1000-60000毫秒范围内");
                }
            }
        }

        public class CommandManager
        {
            private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
            private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();

            public bool CanUndo => _undoStack.Count > 0;
            public bool CanRedo => _redoStack.Count > 0;
            public int UndoCount => _undoStack.Count;
            public int RedoCount => _redoStack.Count;

            public event EventHandler CommandExecuted;
            public event EventHandler CommandUndone;
            public event EventHandler CommandRedone;

            public void ExecuteCommand(ICommand command)
            {
                try
                {
                    command.Execute();
                    _undoStack.Push(command);
                    _redoStack.Clear();
                    
                    CommandExecuted?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"命令执行失败: {ex.Message}", ex);
                }
            }

            public void UndoCommand()
            {
                if (CanUndo)
                {
                    var command = _undoStack.Pop();
                    command.Undo();
                    _redoStack.Push(command);
                    
                    CommandUndone?.Invoke(this, EventArgs.Empty);
                }
            }

            public void RedoCommand()
            {
                if (CanRedo)
                {
                    var command = _redoStack.Pop();
                    command.Execute();
                    _undoStack.Push(command);
                    
                    CommandRedone?.Invoke(this, EventArgs.Empty);
                }
            }

            public void Clear()
            {
                _undoStack.Clear();
                _redoStack.Clear();
            }
        }
    }
}

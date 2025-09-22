namespace RUINORERP.PacketSpec
{
    /// <summary>
    /// 错误码常量定义类
    /// 用于集中管理应用程序中的所有错误码
    /// </summary>
    public static class ErrorCodes
    {
        // 命令相关错误码
        public const string NullCommand = "NULL_COMMAND";
        public const string InvalidCommandId = "INVALID_COMMAND_ID";
        public const string InvalidCommandIdentifier = "INVALID_COMMAND_IDENTIFIER";
        public const string InvalidTimeout = "INVALID_TIMEOUT";
        public const string SessionRequired = "SESSION_REQUIRED";
        public const string DataRequired = "DATA_REQUIRED";
        public const string CommandExecutionError = "EXECUTION_ERROR";
        public const string CommandCancelled = "CANCELLED";
        public const string CommandValidationFailed = "COMMAND_VALIDATION_FAILED";
        public const string ProcessCancelled = "PROCESS_CANCELLED";
        public const string ProcessError = "PROCESS_ERROR";
		 public const string UnsupportedCommand = "UNSUPPORTED_COMMAND";
        public const string CommandNotFound = "COMMAND_NOT_FOUND"; // 命令未找到

        // 调度器相关错误码
        public const string DispatcherNotInitialized = "DISPATCHER_NOT_INITIALIZED";
        public const string DispatcherBusy = "DISPATCHER_BUSY";
        public const string NoHandlerFound = "NO_HANDLER_FOUND";
        public const string HandlerSelectionFailed = "HANDLER_SELECTION_FAILED";
        public const string NullResult = "NULL_RESULT";
        public const string DispatchCancelled = "DISPATCH_CANCELLED";
        public const string DispatchError = "DISPATCH_ERROR";

        // 消息处理相关错误码
        public const string InvalidPopupData = "INVALID_POPUP_DATA";
        public const string InvalidForwardData = "INVALID_FORWARD_DATA";
        public const string InvalidResponseData = "INVALID_RESPONSE_DATA";
        public const string InvalidSystemMessageData = "INVALID_SYSTEM_MESSAGE_DATA";
        public const string InvalidNotificationData = "INVALID_NOTIFICATION_DATA";
        public const string MissingTargetSession = "MISSING_TARGET_SESSION";
        public const string PopupSendError = "POPUP_SEND_ERROR";
        public const string PopupForwardError = "POPUP_FORWARD_ERROR";
        public const string MessageResponseError = "MESSAGE_RESPONSE_ERROR";
        public const string SystemMessageError = "SYSTEM_MESSAGE_ERROR";
        public const string NotificationError = "NOTIFICATION_ERROR";
        public const string UnsupportedMessageCommand = "UNSUPPORTED_MESSAGE_COMMAND";
        public const string MessageHandlerError = "MESSAGE_HANDLER_ERROR";
        
        // 新增的消息处理相关错误码
        public const string InvalidUserData = "INVALID_USER_DATA";
        public const string UserMessageSendError = "USER_MESSAGE_SEND_ERROR";
        public const string InvalidDepartmentData = "INVALID_DEPARTMENT_DATA";
        public const string DepartmentMessageSendError = "DEPARTMENT_MESSAGE_SEND_ERROR";
        public const string InvalidBroadcastData = "INVALID_BROADCAST_DATA";
        public const string BroadcastSendError = "BROADCAST_SEND_ERROR";
        public const string NotificationSendError = "NOTIFICATION_SEND_ERROR";

        // 处理器相关错误码
        public const string HandlerDisposed = "HANDLER_DISPOSED";
        public const string HandlerInitializeFailed = "HANDLER_INIT_FAILED";
        public const string HandlerNotInitialized = "HANDLER_NOT_INITIALIZED";
        public const string HandlerNotRunning = "HANDLER_NOT_RUNNING";
        public const string HandlerBusy = "HANDLER_BUSY";
        public const string UnhandledException = "UNHANDLED_EXCEPTION"; // 未处理的异常
        
 

        // 通用错误码
        public const string UnknownError = "UNKNOWN_ERROR"; // 未知错误
    }
}

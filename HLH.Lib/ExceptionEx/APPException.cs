// ----------------------------------------------------------------
// Description : 异常处理
// Author      : watson
// Create date : 创建日期 格式：2009-03-16
// Modify date : 		
// Modify desc : 
// ----------------------------------------------------------------
using HLH.Lib.Helper;
using System;
using System.Runtime.Serialization;

namespace HLH.Lib.ExceptionEx
{
    /// <summary>
    /// 异常处理类,记录异常信息
    /// </summary>
    /// <remarks>
    /// Exceptions that occur in the database layer are left as native exceptions.
    /// </remarks>
    [Serializable]
    public class APPException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="APPException"/> class.
        /// </summary>
        public APPException()
            : base("An exception occurred in the Business layer.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APPException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public APPException(string message)
            : base(message)
        {
            log4netHelper.warn(message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APPException"/> class.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the innerException parameter 
        /// is not a null reference, the current exception is raised in a catch block that handles 
        /// the inner exception.
        /// </param>
        public APPException(Exception ex)
            : base(ex.Message, ex)
        {
            if (ex != null)
            {
                log4netHelper.error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APPException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the innerException parameter 
        /// is not a null reference, the current exception is raised in a catch block that handles 
        /// the inner exception.
        /// </param>
        public APPException(string message, Exception ex)
            : base(message, ex)
        {
            if (ex != null)
            {
                log4netHelper.error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APPException"/> class 
        /// with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object 
        /// data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        protected APPException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
using System;
using System.Security.Principal;

namespace RUINORERP.Model.Context
{
    /// <summary>
    /// Defines the interface for an application 
    /// context manager type.
    /// </summary>
    public interface IContextManager
    {

        /// <summary>
        /// Gets a value indicating whether this
        /// context manager is valid for use in
        /// the current environment.
        /// </summary>
        bool IsValid { get; }
        /// <summary>
        /// Gets the current principal.
        /// </summary>
        IPrincipal GetUser();
        /// <summary>
        /// Sets the current principal.
        /// </summary>
        /// <param name="principal">Principal object.</param>
        void SetUser(IPrincipal principal);

        /// <summary>
        /// Sets the local context.
        /// </summary>
        /// <param name="localContext">Local context.</param>
        void SetLocalContext(ContextDictionary localContext);

        /// <summary>
        /// Gets or sets a reference to the current ApplicationContext.
        /// </summary>
       // ApplicationContext ApplicationContext { get; set; }
    }
}
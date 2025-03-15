using System;
using System.Security.Principal;
using System.Security.Claims;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Model;
using SqlSugar;
using Autofac;
using System.Collections.Generic;
using RUINORERP.Model.CommonModel;
using System.Collections.Concurrent;
using WorkflowCore.Interface;
using RUINORERP.Global;

namespace RUINORERP.Model.Context
{
    /// <summary>
    /// Provides consistent context information between the client
    /// and server DataPortal objects. 
    /// </summary>
    public class ApplicationContext
    {

        //BillConverterFactory �л��õ������UI����л��档����ģʽû�����õ�����ʱû�д����á��������������ֵ��
        public List<tb_MenuInfo> UserMenuList;

        #region ȫ�ֱ���
        public Dictionary<string, string> _DynamicConfigCache { get; set; }

        /// <summary>
        /// ���ﱣ���˿���ʹ�õĹ���ģ��
        /// </summary>
        public List<GlobalFunctionModule> CanUsefunctionModules { get; set; } = new List<GlobalFunctionModule>();

        /// <summary>
        /// web�����������֤
        /// </summary>
        public string SessionId { get; set; }



        #endregion

        #region ������

        /// <summary>
        /// ���ڱ���ע��Ĺ�������Ŀǰ��ʱ���ڿͻ��ˣ��������ڷ����
        /// </summary>
        public ConcurrentDictionary<string, string> RegistedWorkflowList = new ConcurrentDictionary<string, string>();


        /// <summary>
        /// ��������������
        /// </summary>
        public IWorkflowHost workflowHost;

        /// <summary>
        /// ������JSON������
        /// </summary>
        public WorkflowCore.Interface.IDefinitionLoader definitionLoader;

        #endregion


        /// <summary>
        /// �Ƿ�Ϊ����״̬ ��������õ����ߺ��һ�� ��־��¼��
        /// </summary>
        //public bool IsDebug { get; set; } = false;

        //��context.SysConfig.IsDebug�滻��
        //

        /// <summary>
        /// �Ƿ�Ϊ�������û�
        /// </summary>
        public bool IsSuperUser { get; set; } = false;

        // �쳣�������Ϣ ˭��ʲôģ�� ����ʲô���� ip��ַ ʱ��

        /// <summary>
        /// sugarClient
        /// </summary>
        public ISqlSugarClient Db { get; set; }

        /// <summary>
        /// �ڴ��е�ҵ�񼶵��û���Ϣ
        /// </summary>
        public UserInfo CurrentUser { get; set; } = new UserInfo();


        /// <summary>
        /// ��˾����Ϣ
        /// </summary>
        public tb_Company CompanyInfo { get; set; }


        /// <summary>
        /// ���ݿ⼶���û���Ϣ
        /// </summary>
        public ICurrentUserInfo CurUserInfo { get; set; }
        public string Status { get; set; }

        /// <summary>
        /// ϵͳ�������ã����Ȩ���������ˡ���ʹ��Ȩ���е�
        /// </summary>
        public tb_SystemConfig SysConfig { get; set; }

        /// <summary>
        /// ��ɫ�����������ã������ڵ�һ����ɫ��,����ǰ��ɫ
        /// </summary>
        public tb_RolePropertyConfig rolePropertyConfig { set; get; } = new tb_RolePropertyConfig();

        /// <summary>
        /// һ���˿����ж����ɫ
        /// </summary>
        public List<tb_RoleInfo> Roles { get; set; }

        /// <summary>
        /// ��ǰ��ɫ
        /// </summary>
        public tb_RoleInfo CurrentRole { get; set; }

        /// <summary>
        /// ��ǰ�û���ɫ��һ���˿��Զ�Ӧ�����ɫ�����ò�һ������
        /// </summary>
        public tb_User_Role CurrentUser_Role { get; set; }


        /// <summary>
        /// ��ǰ�û���ɫ�µĸ��Ի�����
        /// </summary>
        public tb_UserPersonalized CurrentUser_Role_Personalized { get; set; }


        /// <summary>
        /// ��ǰ��ɫ
        /// </summary>
        public List<tb_WorkCenterConfig> WorkCenterConfigList { get; set; } = new List<tb_WorkCenterConfig>();


        /// <summary>
        /// ʵ��ֻʹ���˲����ֶΣ�ʡ�¡�������Ե�����һ��������Ϣʵ���滻��TODO
        ///  ����ֻ��Ϊ���쳣��־���֡����������һ��������������Ϊ���� ����˼·���ټ���ע��
        /// </summary>
        public Logs log { get; set; }


        /// <summary>
        /// Creates a new instance of the type
        /// </summary>
        /// <param name="applicationContextAccessor"></param>
        public ApplicationContext(ApplicationContextAccessor applicationContextAccessor)
        {
            ApplicationContextAccessor = applicationContextAccessor;
            //ApplicationContextAccessor.GetContextManager().ApplicationContext = this;
        }

        internal ApplicationContextAccessor ApplicationContextAccessor { get; set; }

        /// <summary>
        /// Gets the context manager responsible
        /// for storing user and context information for
        /// the application.
        /// </summary>
        public IContextManager ContextManager => ApplicationContextAccessor.GetContextManager();

        /// <summary>
        /// Get or set the current ClaimsPrincipal
        /// ��ʾ�û���ݵĶ���
        /// </summary>
        public ClaimsPrincipal Principal
        {
            get { return (ClaimsPrincipal)ContextManager.GetUser(); }
            set { ContextManager.SetUser(value); }
        }

        /// <summary>
        /// Get or set the current <see cref="IPrincipal" />
        /// object representing the user's identity.
        /// </summary>
        /// <remarks>
        /// This is discussed in Chapter 5. When running
        /// under IIS the HttpContext.Current.User value
        /// is used, otherwise the current Thread.CurrentPrincipal
        /// value is used.
        /// </remarks>
        public IPrincipal User
        {
            get { return ContextManager.GetUser(); }
            set { ContextManager.SetUser(value); }
        }



        private readonly object _syncContext = new object();

        internal void SetContext(ContextDictionary clientContext)
        {
            lock (_syncContext)
                ContextManager.SetLocalContext(clientContext);
        }

        /// <summary>
        /// Clears all context collections.
        /// </summary>
        public void Clear()
        {

            SetContext(null);
            ContextManager.SetLocalContext(null);

        }

        #region Settings

        /// <summary>
        /// �Ƿ���ͬ�û����Ѿ���½�ˡ�Ĭ�Ϸ�
        /// </summary>
        public bool AlreadyLogged { get; set; } = false;

        /// <summary>
        /// �Ƿ�Ϊ����״̬
        /// </summary>
        public bool IsOnline { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether CSLA
        /// should fallback to using reflection instead of
        /// System.Linq.Expressions (true, default).
        /// </summary>
        public static bool UseReflectionFallback { get; set; } = false;

        /// <summary>
        /// Gets a value representing the application version
        /// for use in server-side data portal routing.
        /// </summary>
        public static string VersionRoutingTag { get; internal set; }



        /// <summary>
        /// Gets the authentication type being used by the
        /// CSLA .NET framework.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public static string AuthenticationType { get; set; } = "Csla";

        /// <summary>
        /// Get whether we are to flow User Principal to the server
        /// </summary>
        /// <remarks>
        /// This should generally be left at the default of false. Values on 
        /// the client can be manipulated, and therefore allowing the principal 
        /// to flow from client to server could result in an exploitable security 
        /// weakness, including impersonation or elevation of privileges.
        /// </remarks>
        public static bool FlowSecurityPrincipalFromClient { get; internal set; } = false;

        /// <summary>
        /// Gets a value indicating whether objects should be
        /// automatically cloned by the data portal Update()
        /// method when using a local data portal configuration.
        /// </summary>
        public static bool AutoCloneOnUpdate { get; internal set; } = true;

        /// <summary>
        /// Gets a value indicating whether the
        /// server-side business object should be returned to
        /// the client as part of the DataPortalException
        /// (default is false).
        /// </summary>
        public static bool DataPortalReturnObjectOnException { get; internal set; }




        /// <summary>
        /// Gets or sets the default transaction timeout in seconds.
        /// </summary>
        /// <value>
        /// The default transaction timeout in seconds.
        /// </value>
        public static int DefaultTransactionTimeoutInSeconds { get; internal set; } = 30;


        #endregion

        #region ע��������ֵ �����ӽ���ֻ��һ������������

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            CurrentServiceProvider = serviceProvider;
        }

        public void SetAutofacContainerScope(ILifetimeScope _autofacContainerScope)
        {
            AutofacContainerScope = _autofacContainerScope;
        }

        /// <summary>
        /// Gets the service provider scope for this application context.
        /// </summary>
        private IServiceProvider CurrentServiceProvider { get; set; }

        /// <summary>
        ///  Autofac������
        /// </summary>
        public static ILifetimeScope AutofacContainerScope { get; set; }


        /// <summary>
        /// Attempts to get service via DI using ServiceProviderServiceExtensions.GetRequiredService. 
        /// Throws exception if service not properly registered with DI.
        /// </summary>
        /// <typeparam name="T">Type of service/object to create.</typeparam>
        public T GetRequiredService<T>()
        {
            if (CurrentServiceProvider == null)
                throw new NullReferenceException(nameof(CurrentServiceProvider));

            var result = CurrentServiceProvider.GetRequiredService<T>();
            return result;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public T GetRequiredServiceByName<T>(string className)
        {
            if (AutofacContainerScope == null)
                throw new NullReferenceException(nameof(AutofacContainerScope));

            return AutofacContainerScope.ResolveNamed<T>(className);
        }

        #endregion



        /// <summary>
        /// Creates an object using 'Activator.CreateInstance' using
        /// service provider (if one is available) to populate any parameters 
        /// in CTOR that are not manually passed in.
        /// </summary>
        /// <typeparam name="T">Type of object to create.</typeparam>
        /// <param name="parameters">Parameters for constructor</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public T CreateInstanceDI<T>(params object[] parameters)
        {
            return (T)CreateInstanceDI(typeof(T), parameters);
        }




        /// <summary>
        /// Attempts to get service via DI using ServiceProviderServiceExtensions.GetRequiredService. 
        /// Throws exception if service not properly registered with DI.
        /// </summary>
        /// <param name="serviceType">Type of service/object to create.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public object GetRequiredService(Type serviceType)
        {
            if (CurrentServiceProvider == null)
                throw new NullReferenceException(nameof(CurrentServiceProvider));

            return CurrentServiceProvider.GetRequiredService(serviceType);
        }

        /// <summary>
        /// Creates an object using 'Activator.CreateInstance' using
        /// service provider (if one is available) to populate any parameters 
        /// in CTOR that are not manually passed in.
        /// </summary>
        /// <param name="objectType">Type of object to create</param>
        /// <param name="parameters">Manually passed in parameters for constructor</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public object CreateInstanceDI(Type objectType, params object[] parameters)
        {
            object result;
            if (CurrentServiceProvider != null)
                result = ActivatorUtilities.CreateInstance(CurrentServiceProvider, objectType, parameters);
            else
                result = Activator.CreateInstance(objectType, parameters);
            if (result is IUseApplicationContext tmp)
            {
                tmp.ApplicationContext = this;
            }
            return result;
        }

        /// <summary>
        /// Creates an instance of a generic type
        /// using its default constructor.
        /// </summary>
        /// <param name="type">Generic type to create</param>
        /// <param name="paramTypes">Type parameters</param>
        /// <returns></returns>
        internal object CreateGenericInstanceDI(Type type, params Type[] paramTypes)
        {
            var genericType = type.GetGenericTypeDefinition();
            var gt = genericType.MakeGenericType(paramTypes);
            return CreateInstanceDI(gt);
        }

        /// <summary>
        /// Creates an object using Activator.
        /// </summary>
        /// <typeparam name="T">Type of object to create.</typeparam>
        /// <param name="parameters">Parameters for constructor</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public T CreateInstance<T>(params object[] parameters)
        {
            return (T)CreateInstance(typeof(T), parameters);
        }

        /// <summary>
        /// Creates an object using Activator.
        /// </summary>
        /// <param name="objectType">Type of object to create</param>
        /// <param name="parameters">Parameters for constructor</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public object CreateInstance(Type objectType, params object[] parameters)
        {
            object result;
            result = Activator.CreateInstance(objectType, parameters);
            if (result is IUseApplicationContext tmp)
            {
                tmp.ApplicationContext = this;
            }
            return result;
        }

        /// <summary>
        /// Creates an instance of a generic type using Activator.
        /// </summary>
        /// <param name="type">Generic type to create</param>
        /// <param name="paramTypes">Type parameters</param>
        /// <returns></returns>
        internal object CreateGenericInstance(Type type, params Type[] paramTypes)
        {
            var genericType = type.GetGenericTypeDefinition();
            var gt = genericType.MakeGenericType(paramTypes);
            return CreateInstance(gt);
        }



    }
}

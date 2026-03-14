using Krypton.Toolkit;
using RUINORERP.Business.Cache;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RUINORERP.UI.FormProperty
{
    /// <summary>
    /// 单据信息显示窗体
    /// 用于显示创建时间、创建人、修改时间、修改人、审核时间、审核人
    /// </summary>
    public partial class frmBillInfo : frmBase
    {
        /// <summary>
        /// 当前绑定的实体对象
        /// </summary>
        private BaseEntity _entity;
        private static IEntityCacheManager _cacheManager;
        private static IEntityCacheManager CacheManager => _cacheManager ?? (_cacheManager = Startup.GetFromFac<IEntityCacheManager>());
        /// <summary>
        /// 构造函数
        /// </summary>
        public frmBillInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数 - 带实体参数
        /// </summary>
        /// <param name="entity">要显示信息的实体对象</param>
        public frmBillInfo(BaseEntity entity) : this()
        {
            _entity = entity;
        }

        /// <summary>
        /// 设置要显示的实体对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        public void SetEntity(BaseEntity entity)
        {
            _entity = entity;
            LoadBillInfo();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void frmBillInfo_Load(object sender, EventArgs e)
        {
            LoadBillInfo();
        }

        /// <summary>
        /// 加载并显示单据信息
        /// </summary>
        private void LoadBillInfo()
        {
            if (_entity == null)
            {
                return;
            }

            try
            {
                Type entityType = _entity.GetType();

                txtCreated_at.Text = GetPropertyValue(entityType, "Created_at");
                txtCreated_by.Text = GetEmployeeName(GetPropertyValue(entityType, "Created_by"));
                txtUpdated_at.Text = GetPropertyValue(entityType, "Updated_at");
                txtUpdated_by.Text = GetEmployeeName(GetPropertyValue(entityType, "Updated_by"));
                txtApproved_at.Text = GetPropertyValue(entityType, "Approved_at");
                txtApproved_by.Text = GetEmployeeName(GetPropertyValue(entityType, "Approved_by"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载单据信息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 通过反射获取属性值
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值的字符串表示</returns>
        private string GetPropertyValue(Type entityType, string propertyName)
        {
            try
            {
                PropertyInfo property = entityType.GetProperty(propertyName);
                if (property == null)
                {
                    return string.Empty;
                }

                object value = property.GetValue(_entity);
                if (value == null)
                {
                    return string.Empty;
                }

                if (value is DateTime dateTime)
                {
                    return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }

                return value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据员工ID获取员工名称
        /// </summary>
        /// <param name="employeeIdStr">员工ID字符串</param>
        /// <returns>员工名称</returns>
        private string GetEmployeeName(string employeeIdStr)
        {
            if (string.IsNullOrEmpty(employeeIdStr))
            {
                return string.Empty;
            }
            try
            {
                if (long.TryParse(employeeIdStr, out long employeeId))
                {
                    var employee = CacheManager.GetEntity<tb_Employee>(employeeId);
                    if (employee != null)
                    {
                        return employee.Employee_Name;
                    }
                }
            }
            catch
            {
            }

            return employeeIdStr;
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

using FastReport.DevComponents.DotNetBar.Controls;
using Pipelines.Sockets.Unofficial.Arenas;
using RUINORERP.Business;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.Model;
using RUINORERP.Model;
using RUINORERP.UI.SysConfig;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{

    /// <summary>
    /// 公共方法
    /// </summary>
    public class CommonHelper
    {
        private static CommonHelper m_instance;

        public static CommonHelper Instance
        {
            get
            {
                if (m_instance == null)
                {
                    Initialize();
                }
                return m_instance;
            }
            set
            {
                m_instance = value;
            }
        }


        /// <summary>
        /// 对象实例化
        /// </summary>
        public static void Initialize()
        {
            m_instance = new CommonHelper();
        }

        /// <summary>
        /// 获取枚举描述，描述保存在枚举值中的特性中
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<KeyValuePair<object, string>> GetEnumDescription(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("提供的类型必须是枚举类型。", nameof(enumType));

            List<KeyValuePair<object, string>> kvlistPayStatus = new List<KeyValuePair<object, string>>();
            Array enumValues = Enum.GetValues(enumType);
            foreach (var value in enumValues)
            {
                int currentValue = (int)value;
                string currentDescription = EnumExtensions.GetDescription(value as Enum);
                kvlistPayStatus.Add(new KeyValuePair<object, string>(currentValue, currentDescription));
            }
            return kvlistPayStatus;
        }


        /// <summary>
        /// 获取枚举值和名称
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public List<KeyValuePair<object, string>> GetKeyValuePairs(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("提供的类型必须是枚举类型。", nameof(enumType));

            // 获取枚举的基础类型
            Type underlyingType = Enum.GetUnderlyingType(enumType);

            List<KeyValuePair<object, string>> kvlistPayStatus = new List<KeyValuePair<object, string>>();
            Array enumValues = Enum.GetValues(enumType);
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValueInt;
            long currentValueLong;

            string currentName;
            while (e.MoveNext())
            {
                currentName = e.Current.ToString();
                if (underlyingType == typeof(int))
                {
                    currentValueInt = (int)e.Current;
                    kvlistPayStatus.Add(new KeyValuePair<object, string>(currentValueInt, currentName));
                }
                if (underlyingType == typeof(long))
                {
                    currentValueLong = (long)e.Current;
                    kvlistPayStatus.Add(new KeyValuePair<object, string>(currentValueLong, currentName));
                }
            }
            return kvlistPayStatus;
        }


        /// <summary>
        /// 返回真实类型的值
        /// </summary>
        /// <param name="DataType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object GetRealValueByDataType(Type DataType, object value)
        {
            Type newcolType = GetRealType(DataType);
            if (value == null || string.IsNullOrEmpty(value.ToString()) || value == DBNull.Value || value.ToString() == "")
            {
                return null;
            }
            // We need to convert the value
            return Convert.ChangeType(value, newcolType);
        }


        /// <summary>
        /// 得到真实类型
        /// </summary>
        /// <param name="DataType"></param>
        /// <returns></returns>
        public Type GetRealType(Type DataType)
        {
            Type newcolType;
            // We need to check whether the property is NULLABLE
            if (DataType.IsGenericType && DataType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                newcolType = DataType.GetGenericArguments()[0];
                //newcolType = DataType.GetUnderlyingType(DataType);//这个也可以
                //如果类型是例如此代码可为空，返回int部分(底层类型)。如果只需要将对象转换为特定类型，则可以使用System.Convert.ChangeType方法。
            }
            else
            {
                newcolType = DataType;
            }
            return newcolType;
        }









        #region ComboBox 静默初始化

        private const int WM_SETREDRAW = 0x000B;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 静默初始化ComboBox内部编辑控件，使用WM_SETREDRAW避免闪烁
        /// </summary>
        public static void InitializeComboBoxEditControl(ComboBox cmb)
        {
            if (cmb == null) return;

            // 只处理DropDown样式的ComboBox
            if (cmb.DropDownStyle != ComboBoxStyle.DropDown) return;

            // 确保控件句柄已创建
            if (!cmb.IsHandleCreated)
            {
                cmb.CreateControl();
            }

            if (cmb.Handle == IntPtr.Zero) return;

            // 禁用重绘
            SendMessage(cmb.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);

            try
            {
                // 执行DropDownStyle切换，触发内部编辑控件创建
                var originalStyle = cmb.DropDownStyle;
                cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                cmb.DropDownStyle = originalStyle;
            }
            finally
            {
                // 启用重绘
                SendMessage(cmb.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
                // 强制重绘
                cmb.Invalidate();
            }
        }

        #endregion
    }
}

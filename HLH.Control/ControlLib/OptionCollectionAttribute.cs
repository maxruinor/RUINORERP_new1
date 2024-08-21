using System;
using System.Collections.Generic;

namespace HLH.WinControl.ControlLib
{
    public class OptionCollectionAttribute : Attribute
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public OptionCollectionAttribute(params object[] objArr)
        {
            if (objArr != null)
            {
                foreach (var obj in objArr)
                {
                    OptionCollection.Add(obj);
                }
            }
        }

        /// <summary>
        /// 获取可供选择的项目集合
        /// </summary>
        public List<object> OptionCollection { get; private set; } = new List<object>();
    }
}

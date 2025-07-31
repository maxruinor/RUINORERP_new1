using System;

namespace RUINORERP.Model.Utilities
{
    /// <summary>
    /// 像提醒规则配置实体中，用List<int>保存了枚举值，但显示用另一个属性，需要用这个特性标记。
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DisplayTextAttribute : Attribute
    {

        public DisplayTextAttribute(bool displayColumnToGridView = false)
        {
            DisplayColumnToGridView = displayColumnToGridView;
        }


        /// <summary>
        /// 控制是否显示的表格列
        /// </summary>
        public bool DisplayColumnToGridView { get; set; } = false;
        /// <summary>
        /// 中文描述
        /// </summary>
        public string Description { get; set; }

    }
}

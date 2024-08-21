using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UControls
{

    /// <summary>
    /// 列显示控制器
    /// </summary>
    [Serializable]
    public class ColumnDisplayController
    {
        private string colDisplayText = string.Empty;
        private bool isFixed = false;
        private int colDisplayIndex = 0;
        private int colWith = 50;
        private string colEncryptedName = string.Empty;

        /// <summary>
        /// 显示的文字
        /// </summary>
        public string ColDisplayText { get => colDisplayText; set => colDisplayText = value; }

        /// <summary>
        /// 是否固定
        /// </summary>
        public bool IsFixed { get => isFixed; set => isFixed = value; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int ColDisplayIndex { get => colDisplayIndex; set => colDisplayIndex = value; }

        /// <summary>
        /// 宽度
        /// </summary>
        public int ColWith { get => colWith; set => colWith = value; }

        /// <summary>
        /// 加密后的列名，暂时不用
        /// </summary>
        public string ColEncryptedName { get => colEncryptedName; set => colEncryptedName = value; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColName { get; set; }

        public string DataPropertyName { get; set; }

        /// <summary>
        /// 显示
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 如果为真，则不参与控制，并且不显示
        /// </summary>
        public bool Disable { get; set; }

    }
}

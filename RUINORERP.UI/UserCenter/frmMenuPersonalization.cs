using HLH.WinControl.MyTypeConverter;
using Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.Model.Models;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UserCenter
{
    /// <summary>
    /// QueryConditionSettings  查询条件设置，可以设置显示行数。条件排序，条件的默认值。条件显示情况
    /// </summary>
    public partial class frmMenuPersonalization : KryptonForm
    {
        public frmMenuPersonalization()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            tb_UIMenuPersonalization menuSetting = MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations.FirstOrDefault(c => c.MenuID == CurMenuInfo.MenuID);
            if (menuSetting != null)
            {
                QueryShowColQty.Value= menuSetting.QueryConditionCols ;
            }
            else
            {
                QueryShowColQty.Value = 5;
                menuSetting=new tb_UIMenuPersonalization
                menuSetting.QueryConditionShowColsQty = QueryShowColQty.Value;
            }

        
            UserGlobalConfig.Instance.MenuPersonalizationlist[MenuPathKey] = mp;
            //保存
            UserGlobalConfig.Instance.Serialize();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 关联的菜单信息 实际是可以从点击时传入
        /// </summary>
        public tb_MenuInfo CurMenuInfo { get; set; }

        private void frmMenuPersonalization_Load(object sender, EventArgs e)
        {
            bool rs = await UIBizSrvice.SetQueryConditionsAsync(CurMenuInfo, QueryConditionFilter, QueryDtoProxy);
            if (rs)
            {
                QueryDtoProxy = LoadQueryConditionToUI();
            }
            1
            tb_UIMenuPersonalization menuSetting = MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations.FirstOrDefault(c => c.MenuID == CurMenuInfo.MenuID);
            if (menuSetting != null)
            {
                if (menuSetting.tb_UIQueryConditions != null && menuSetting.tb_UIQueryConditions.Count > 0)
                {
                    QueryShowColQty.Value = menuSetting.QueryConditionCols;
                }
                else
                {
                    QueryShowColQty.Value = 5;
                }
            }


        }
    }
}

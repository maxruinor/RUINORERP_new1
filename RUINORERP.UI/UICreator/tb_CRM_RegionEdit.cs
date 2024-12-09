
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 15:51:25
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using FluentValidation;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.UI.Common;

namespace RUINORERP.UI
{
    /// <summary>
    /// 销售分区表-大中华区数据编辑
    /// </summary>
    [MenuAttrAssemblyInfo("库位编辑", true, UIType.单表数据)]
    public partial class tb_CRM_RegionEdit : UserControl
    {
        public tb_CRM_RegionEdit()
        {

            InitializeComponent();









        }
        /*

                tb_CRM_Region UIToEntity()
                {
                tb_CRM_Region entity = new tb_CRM_Region();
                             entity.Region_Name = txtRegion_Name.Text ;
                               entity.Region_code = txtRegion_code.Text ;
                               entity.Parent_region_id = Int64.Parse(txtParent_region_id.Text);
                                entity.Sort = Int32.Parse(txtSort.Text);
                                entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                                entity.Notes = txtNotes.Text ;
                                       return entity;
        }
                */


        private tb_CRM_Region _EditEntity;
        public tb_CRM_Region EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CRM_Region entity)
        {
            _EditEntity = entity;
            DataBindingHelper.BindData4TextBox<tb_CRM_Region>(entity, t => t.Region_Name, txtRegion_Name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Region>(entity, t => t.Region_code, txtRegion_code, BindDataType4TextBox.Text, false);
             DataBindingHelper.BindData4TextBox<tb_CRM_Region>(entity, t => t.Parent_region_id, txtParent_region_id, BindDataType4TextBox.Qty, false);
             DataBindingHelper.BindData4TextBox<tb_CRM_Region>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4CheckBox<tb_CRM_Region>(entity, t => t.Is_enabled, chkIs_enabled, false);
            //有默认值
            DataBindingHelper.BindData4TextBox<tb_CRM_Region>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
        }




        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }




    }
}




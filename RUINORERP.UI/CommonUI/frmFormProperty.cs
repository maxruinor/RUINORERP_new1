using HLH.Lib.Helper;
using Netron.GraphLib;
using Netron.Xeon;
using Newtonsoft.Json;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Dto;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.UControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RUINORERP.UI.CommonUI
{
    public partial class frmFormProperty : frmBase
    {
        /// <summary>
        /// 菜单信息 以菜单为标准
        /// </summary>
        public tb_MenuInfo MenuInfo { get; set; }

        public object Entity { get; set; }

        public delegate void SaveToXmlDelegate(frmFormProperty frm, object Obj);
        public event SaveToXmlDelegate OnSaveToXml;

        public delegate void FromToXmlDelegate(frmFormProperty frm);
        public event FromToXmlDelegate OnFromToXml;


        public frmFormProperty()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_entity.CloseCaseOpinions.IsNullOrEmpty())
            {
                MessageBox.Show("请填写手动结案原因");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmApproval_Load(object sender, EventArgs e)
        {

        }

        private ApprovalEntity _entity;
        public void BindData(ApprovalEntity entity)
        {
            _entity = entity;
            //DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.BillNo, txtBillNO, BindDataType4TextBox.Text, false);
            ////这个只是显示给用户看。不会修改
            //DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.bizName, txtBillType, BindDataType4TextBox.Text, false);
            //txtBillType.ReadOnly = true;
            //entity.ApprovalResults = true;
            //DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.CloseCaseOpinions, txtOpinion, BindDataType4TextBox.Text, false);
            //errorProviderForAllInput.DataSource = entity;
        }

        RUINORERP.Common.Helper.XmlHelper manager = new RUINORERP.Common.Helper.XmlHelper();
        private void btnSaveFormData_Click(object sender, EventArgs e)
        {
            if (OnSaveToXml != null)
            {
                OnSaveToXml(this, Entity);

            }
        }


        public void Serialize<T>(T entity)
        {
            string PathwithFileName = System.IO.Path.Combine(Application.StartupPath + "\\FormProperty\\Data_", MenuInfo.CaptionCN);
            System.IO.FileInfo fi = new System.IO.FileInfo(PathwithFileName);
            //判断目录是否存在
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }
            //SerializationHelper.Serialize(entity, PathwithFileName, false);
            string json = JsonConvert.SerializeObject(entity);
            File.WriteAllText(PathwithFileName, json);
        }

        public T Deserialize<T>() where T : class
        {
            string PathwithFileName = System.IO.Path.Combine(Application.StartupPath + "\\FormProperty\\Data_", MenuInfo.CaptionCN);
            System.IO.FileInfo fi = new System.IO.FileInfo(PathwithFileName);
            //判断目录是否存在
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }
            if (System.IO.File.Exists(PathwithFileName))
            {
              //  Entity = SerializationHelper.Deserialize(PathwithFileName, false) as T;
                string json = File.ReadAllText(PathwithFileName);
                Entity= JsonConvert.DeserializeObject<T>(json) as T;
            }
            return Entity as T;
        }


        private void btnLoadFormData_Click(object sender, EventArgs e)
        {
            if (OnFromToXml != null)
            {
                OnFromToXml(this);
            }

        }
    }
}

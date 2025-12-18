﻿﻿﻿﻿﻿using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.Global;
using RUINORERP.Business;
using RUINORERP.Business.BNR; // 添加BNR命名空间引用

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("编号规则编辑", true, UIType.单表数据)]
    public partial class UCsysBillNoRuleEdit : BaseEditGeneric<tb_sys_BillNoRule>
    {
        public UCsysBillNoRuleEdit()
        {
            InitializeComponent();
        }

        tb_sys_BillNoRule entity;

        public override void BindData(BaseEntity baseEntity)
        {
            entity = baseEntity as tb_sys_BillNoRule;
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.RuleName, txtRuleName, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.BizType, typeof(BizType), cmbBizType, false);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.RuleType, typeof(RuleType), cmbRuleType, false);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.DateFormat, typeof(DateFormat), cmbDateFormat, false);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.ResetMode, typeof(ResetMode), cmbResetMode, false);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.StorageType, typeof(StorageType), cmbStorageType, false);
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.Prefix, txtPrefix, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.SequenceLength, txtSequenceLength, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.UseCheckDigit, chkUseCheckDigit, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.IsActive, chkIsActive, false);
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text, false);
            // 添加缺失的字段绑定
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.Priority, typeof(Priority), cmbPriority, false);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.EncryptionMethod, typeof(EncryptionMethod), cmbEncryptionMethod, false);
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.RulePattern, txtRulePattern, BindDataType4TextBox.Text, false);

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if (entity.ActionStatus == ActionStatus.修改 || entity.ActionStatus == ActionStatus.新增)
                {
                    //如果是采购入库引入变化则加载明细及相关数据
                    if (s2.PropertyName == entity.GetPropertyName<tb_sys_BillNoRule>(c => c.BizType))
                    {
                        entity.RuleName = $"{(BizType)entity.BizType}编号规则";
                    }

                    // 当相关字段发生变化时，自动生成RulePattern
                    if (s2.PropertyName == entity.GetPropertyName<tb_sys_BillNoRule>(c => c.Prefix) ||
                        s2.PropertyName == entity.GetPropertyName<tb_sys_BillNoRule>(c => c.DateFormat) ||
                        s2.PropertyName == entity.GetPropertyName<tb_sys_BillNoRule>(c => c.SequenceLength) ||
                        s2.PropertyName == entity.GetPropertyName<tb_sys_BillNoRule>(c => c.ResetMode))
                    {
                        GenerateRulePattern();
                    }
                }
            };

            base.BindData(entity);
        }

        /// <summary>
        /// 根据用户选择的各个条件自动生成RulePattern
        /// </summary>
        private void GenerateRulePattern()
        {
            if (entity == null) return;

            try
            {
                StringBuilder pattern = new StringBuilder();

                // 添加前缀部分
                if (!string.IsNullOrEmpty(entity.Prefix))
                {
                    pattern.Append($"{{S:{entity.Prefix}:upper}}");
                }

                // 添加日期部分
                string datePart = "";
                switch ((DateFormat)entity.DateFormat)
                {
                    case DateFormat.YearMonthDay:
                        datePart = "yyMMdd";
                        break;
                    case DateFormat.YearMonth:
                        datePart = "yyMM";
                        break;
                    case DateFormat.YearWeek:
                        datePart = "yyWW";
                        break;
                }

                if (!string.IsNullOrEmpty(datePart))
                {
                    pattern.Append($"{{D:{datePart}}}");
                }

                // 添加流水号部分
                string resetMode = "";
                switch ((ResetMode)entity.ResetMode)
                {
                    case ResetMode.None:
                        resetMode = "";
                        break;
                    case ResetMode.Day:
                        resetMode = "/daily";
                        break;
                    case ResetMode.Month:
                        resetMode = "/monthly";
                        break;
                    case ResetMode.Year:
                        resetMode = "/yearly";
                        break;
                }

                // 格式化流水号长度
                string seqFormat = new string('0', entity.SequenceLength > 0 ? entity.SequenceLength : 3);
                pattern.Append($"{{DB:{{S:{entity.Prefix ?? "NO"}}}{datePart}/{seqFormat}{resetMode}}}");

                entity.RulePattern = pattern.ToString();
            }
            catch (Exception ex)
            {
                // 如果自动生成失败，不抛出异常，让用户手动填写
                System.Diagnostics.Debug.WriteLine($"自动生成RulePattern失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 生成测试编号
        /// </summary>
        private void GenerateTestNumber()
        {
            if (entity == null || string.IsNullOrEmpty(entity.RulePattern))
            {
                txtTestResult.Text = "请先生成编码规则或手动输入编码规则";
                return;
            }

            try
            {
                // 使用轻量级工厂生成测试编号
                var factory = new LightweightBNRFactory();
                string testNumber = factory.Create(entity.RulePattern);
                
                // 显示测试结果到文本框
                txtTestResult.Text = testNumber;
            }
            catch (Exception ex)
            {
                txtTestResult.Text = $"生成测试编号时出错：{ex.Message}";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // 在保存前确保RulePattern已生成
            if (string.IsNullOrEmpty(entity.RulePattern))
            {
                GenerateRulePattern();
            }

            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnTestGenerate_Click(object sender, EventArgs e)
        {
            // 如果RulePattern为空，先尝试生成
            if (string.IsNullOrEmpty(entity.RulePattern))
            {
                GenerateRulePattern();
            }
            
            GenerateTestNumber();
        }

        private void UCSystemConfigEdit_Load(object sender, EventArgs e)
        {

        }
    }
}
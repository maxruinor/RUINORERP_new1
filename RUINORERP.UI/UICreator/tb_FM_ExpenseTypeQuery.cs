
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:04
// **************************************
using System;
﻿using SqlSugar;
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
    /// 业务类型 报销，员工借支还款，运费数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_ExpenseTypeQuery), "业务类型 报销，员工借支还款，运费数据查询", true)]
    public partial class tb_FM_ExpenseTypeQuery:UserControl
    {
     public tb_FM_ExpenseTypeQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_ExpenseType => tb_FM_ExpenseType.ExpenseType_id).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          subject_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_Subject>(k => k.Subject_id, v=>v.XXNAME, cmbSubject_id);
        }
        


    }
}



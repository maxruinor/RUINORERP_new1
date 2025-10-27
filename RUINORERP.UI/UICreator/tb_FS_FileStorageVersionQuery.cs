
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/27/2025 17:49:30
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
    /// 文件版本表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FS_FileStorageVersionQuery), "文件版本表数据查询", true)]
    public partial class tb_FS_FileStorageVersionQuery:UserControl
    {
     public tb_FS_FileStorageVersionQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FS_FileStorageVersion => tb_FS_FileStorageVersion.VersionId).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_FS_FileStorageInfo>(k => k.FileId, v=>v.XXNAME, cmbFileId);
        }
        


    }
}



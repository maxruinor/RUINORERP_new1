
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:33
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{

    public partial class tb_FM_Subject
    {
        /// <summary>
        /// 代码没有生成。手工添加的上级代码
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(tb_FM_Subject.Parent_subject_id))]
        public virtual tb_FM_Subject tb_FM_SubjectParent { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_Subject.Parent_subject_id))]
        public virtual List<tb_FM_Subject> tb_FM_Subjects { get; set; }
        //tb_FM_Subject.Subject_id)
        //Subject_id.FK_ACCOUNTINGSUBJECTS_ACCOUNTINGSUBJECTS)
        //tb_FM_Subject.Parent_subject_id)

    }
}


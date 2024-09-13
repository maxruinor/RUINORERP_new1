
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:01
// **************************************
using AutoMapper;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Services.BASE;
using System.Threading.Tasks;
using System;
﻿using SqlSugar;
using System.Collections.Generic;


namespace RUINORERP.Services
{
    /// <summary>
    /// 交易方式设定，后面扩展有关账期 账龄分析的字段,暂时保存一个主子关系方便后面扩展
    /// </summary>
    public partial class tb_PaymentMethodDetailServices : BaseServices<tb_PaymentMethodDetail>, Itb_PaymentMethodDetailServices
    {
        IMapper _mapper;
        public tb_PaymentMethodDetailServices(IMapper mapper, IBaseRepository<tb_PaymentMethodDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}
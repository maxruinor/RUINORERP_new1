using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using RUINORERP.UI.ATechnologyStack;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Business.Security;

namespace RUINORERP.UI.UserCenter.DataParts
{
    // ConditionGroup类定义
    public class ConditionGroup
    {
        public string PKFieldName { get; set; }
        public string StatusName { get; set; }
        public List<IConditionalModel> Conditions { get; set; }
        public string Identifier { get; set; }
    }
    public class ConditionBuilderFactory
    {
        public List<ConditionGroup> GetNeedBackConditionGroups(BizType bizType)
        {

            var grouplist = new List<ConditionGroup>
        {
            new ConditionGroup
            {
                StatusName = "待返回",
                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel
                    {
                        FieldName = "ApprovalStatus",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = "1",
                        CSharpTypeName = "int"
                    },

                    new ConditionalModel
                    {
                        FieldName = "DataStatus",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)DataStatus.确认).ToString(),
                        CSharpTypeName = "int"
                    },

                    new ConditionalModel
                    {
                        FieldName = "isdeleted",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = "False",
                        CSharpTypeName = "bool"
                    }
                }
            }
        };

            if (bizType == BizType.采购退货单)
            {
                AddNeedBackFromPurCondition(grouplist);
            }
            return grouplist;
        }

        public List<ConditionGroup> GetCommonConditionGroups(BizType bizType)
        {

            var grouplist = new List<ConditionGroup>
        {
            new ConditionGroup
            {

                StatusName = "未提交",
                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel{FieldName = "DataStatus",ConditionalType = ConditionalType.Equal,FieldValue =((int)DataStatus.草稿).ToString()  ,CSharpTypeName = "int"},
                    new ConditionalModel{FieldName = "isdeleted",ConditionalType = ConditionalType.Equal,FieldValue = "False",CSharpTypeName = "bool"}
                }
            },
            new ConditionGroup
            {
                StatusName = "未审核",
                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel
                    {
                        FieldName = "ApprovalStatus",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = "0",
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "DataStatus",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)DataStatus.新建).ToString(),
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "isdeleted",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = "False",
                        CSharpTypeName = "bool"
                    }
                }
            }
        };
            switch (bizType)
            {
                case BizType.销售订单:
                case BizType.销售出库单:
                case BizType.销售退回单:
                    // 添加销售限制条件（如果有权限限制）
                    if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
                    {
                        AddEmployeeIdCondition(grouplist);
                    }
                    break;
                case BizType.采购订单:
                    // 添加销售限制条件（如果有权限限制）
                    if (AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext))
                    {
                        AddEmployeeIdCondition(grouplist);
                    }
                    break;
                case BizType.采购退货单:
                    //// 添加销售限制条件（如果有权限限制）
                    //if (AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext))
                    //{
                    //    AddEmployeeIdCondition(grouplist);
                    //}
                    break;
                case BizType.借出单:
                case BizType.归还单:
                    // 添加销售限制条件（如果有权限限制）
                    if (AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext))
                    {
                        AddEmployeeIdCondition(grouplist);
                    }

                    break;

            }



            return grouplist;
        }

        private void AddEmployeeIdCondition(List<ConditionGroup> grouplist)
        {
            var employeeIdCondition = new ConditionalModel
            {
                FieldName = "Employee_ID",
                ConditionalType = ConditionalType.Equal,
                FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(),
                CSharpTypeName = "long"
            };

            foreach (var group in grouplist)
            {
                group.Conditions.Add(employeeIdCondition);
            }
        }

        /// <summary>
        /// 采购退款单 如果是处理方式是需要返回时提示
        /// </summary>
        /// <param name="grouplist"></param>
        private void AddNeedBackFromPurCondition(List<ConditionGroup> grouplist)
        {
            var employeeIdCondition = new ConditionalModel
            {
                FieldName = "ProcessWay",
                ConditionalType = ConditionalType.Equal,
                FieldValue = ((int)PurReProcessWay.需要返回).ToString(),
                CSharpTypeName = "int"
            };

            foreach (var group in grouplist)
            {
                group.Conditions.Add(employeeIdCondition);
            }
        }

        public List<ConditionGroup> GetPrePaymentConditionGroups(ReceivePaymentType paymentType)
        {

            string StatusNameDescription = string.Empty;
            string identifier = string.Empty;
            if (paymentType == ReceivePaymentType.付款)
            {
                StatusNameDescription = "待付款";
                identifier = SharedFlag.Flag2.ToString();
            }
            else
            {
                StatusNameDescription = "待回款";
                //硬编码的
                identifier = SharedFlag.Flag1.ToString();
            }

            return new List<ConditionGroup>
        {
           new ConditionGroup
            {
                          Identifier =identifier,
                StatusName = "待提交",

                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel
                    {
                        FieldName = "ReceivePaymentType",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)paymentType).ToString(),
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "PrePaymentStatus",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)PrePaymentStatus.草稿).ToString(),
                        CSharpTypeName = "int"
                    },
                     new ConditionalModel
                    {
                        FieldName = "isdeleted",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = "False",
                        CSharpTypeName = "bool"
                    }
                }
            }
            ,
                new ConditionGroup
            {
                          Identifier =identifier,
                StatusName = "待审核",

                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel
                    {
                        FieldName = "ReceivePaymentType",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)paymentType).ToString(),
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "PrePaymentStatus",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)PrePaymentStatus.待审核).ToString(),
                        CSharpTypeName = "int"
                    },
                     new ConditionalModel
                    {
                        FieldName = "isdeleted",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = "False",
                        CSharpTypeName = "bool"
                    }
                }
            }
                ,
                new ConditionGroup
            {
                          Identifier =identifier,
                StatusName = "待核销",

                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel
                    {
                        FieldName = "ReceivePaymentType",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)paymentType).ToString(),
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "PrePaymentStatus",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)PrePaymentStatus.待核销).ToString(),
                        CSharpTypeName = "int"
                    },
                     new ConditionalModel
                    {
                        FieldName = "isdeleted",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = "False",
                        CSharpTypeName = "bool"
                    }
                }
            }
        };
        }

        public List<ConditionGroup> GetARAPConditionGroups(ReceivePaymentType paymentType)
        {
            string StatusNameDescription = string.Empty;
            string identifier = string.Empty;
            if (paymentType == ReceivePaymentType.付款)
            {
                StatusNameDescription = "待付款";
                identifier = SharedFlag.Flag2.ToString();
            }
            else
            {
                StatusNameDescription = "待回款";
                //硬编码的
                identifier = SharedFlag.Flag1.ToString();
            }

            var arapGroupList = new List<ConditionGroup>
        {
            new ConditionGroup
            {
                 Identifier =identifier,
                StatusName = "待提交",
                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel
                    {
                        FieldName = "ReceivePaymentType",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)paymentType).ToString(),
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "ARAPStatus",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)ARAPStatus.草稿).ToString(),
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "isdeleted",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = "False",
                        CSharpTypeName = "bool"
                    }
                }
            },
              new ConditionGroup
            {
                 Identifier =identifier,
                StatusName = "待审核",
                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel
                    {
                        FieldName = "ReceivePaymentType",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)paymentType).ToString(),
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "ARAPStatus",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)ARAPStatus.待审核).ToString(),
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "isdeleted",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = "False",
                        CSharpTypeName = "bool"
                    }
                }
            },
               new ConditionGroup
            {
                 Identifier =identifier,
                StatusName = StatusNameDescription,
                Conditions = new List<IConditionalModel>
                {
                new ConditionalModel
                    {
                        FieldName = "ARAPStatus",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)ARAPStatus.待支付).ToString(),
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "ReceivePaymentType",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)paymentType).ToString(),
                        CSharpTypeName = "int"
                    },


                    new ConditionalModel
                    {
                        FieldName = "isdeleted",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = "False",
                        CSharpTypeName = "bool"
                    }
                }
            }
        };

            List<IConditionalModel> conModels = arapGroupList.FirstOrDefault(c => c.StatusName == StatusNameDescription).Conditions;
            conModels.Clear();
            conModels.Add(new ConditionalCollections()
            {
                ConditionalList = new List<KeyValuePair<WhereType, SqlSugar.ConditionalModel>>()
                              {
                               new KeyValuePair<WhereType, ConditionalModel>(
                               WhereType.And,
                               new ConditionalModel(){FieldName ="ARAPStatus",ConditionalType=ConditionalType.Equal,FieldValue=((int)ARAPStatus.部分支付).ToString(), CSharpTypeName = "int"}),

                               new KeyValuePair<WhereType, ConditionalModel> (
                               WhereType.Or,
                               new ConditionalModel() {FieldName ="ARAPStatus",ConditionalType=ConditionalType.Equal,FieldValue=((int)ARAPStatus.待支付).ToString(), CSharpTypeName = "int"}),


                               new KeyValuePair<WhereType, ConditionalModel> (
                               WhereType.
                               And,new ConditionalModel() {FieldName="ReceivePaymentType",ConditionalType=ConditionalType.Equal,FieldValue= ((int)paymentType).ToString(), CSharpTypeName = "int"}),

                               new KeyValuePair<WhereType, ConditionalModel> (
                               WhereType.
                               And,new ConditionalModel() {FieldName="isdeleted",ConditionalType=ConditionalType.Equal,FieldValue="False",CSharpTypeName = "bool"})
                              }
            });

            return arapGroupList;
        }

        public List<ConditionGroup> GetPaymentConditionGroups(ReceivePaymentType paymentType)
        {
            string StatusNameDescription = string.Empty;
            string identifier = string.Empty;
            if (paymentType == ReceivePaymentType.付款)
            {
                StatusNameDescription = "待支付";
                identifier = SharedFlag.Flag2.ToString();
            }
            else
            {
                StatusNameDescription = "待支付";
                //硬编码的
                identifier = SharedFlag.Flag1.ToString();
            }

            var grouplist = new List<ConditionGroup>
        {
                new ConditionGroup
            {
                 Identifier =identifier,
                StatusName = "待提交",
                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel
                    {
                        FieldName = "ReceivePaymentType",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)paymentType).ToString(),
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "PaymentStatus",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)PaymentStatus.草稿).ToString(),
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "isdeleted",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = "False",
                        CSharpTypeName = "bool"
                    }
                }
            },

            new ConditionGroup
            {

                Identifier= identifier,
                StatusName = StatusNameDescription,
                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel
                    {
                        FieldName = "ReceivePaymentType",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)paymentType).ToString(),
                        CSharpTypeName = "int"
                    },
                    new ConditionalModel
                    {
                        FieldName = "PaymentStatus",
                        ConditionalType = ConditionalType.Equal,
                        FieldValue = ((int)PaymentStatus.待审核).ToString(),
                        CSharpTypeName = "int"
                    }
                }
            }
        };



            return grouplist;
        }

        public List<ConditionGroup> GetPurchaseOrderSpecialConditions()
        {
            return new List<ConditionGroup>
        {
            new ConditionGroup
            {
                Identifier="1",
                StatusName = "待入库",
                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel { FieldName = "ApprovalStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" },
                    new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue =((int)DataStatus.确认).ToString(), CSharpTypeName = "int" },
                    new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
                }
            }
        };
        }





        public List<ConditionGroup> GetNeedReturnSpecialConditions(string _StatusName)
        {
            var list = new List<ConditionGroup>
        {
            new ConditionGroup
            {
                StatusName = _StatusName,//"待归还",
                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel { FieldName = "ApprovalStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" },
                    new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue =((int)DataStatus.确认).ToString(), CSharpTypeName = "int" },
                    new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
                }
            }
            };

            // 添加销售限制条件（如果有权限限制）
            if (AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext))
            {
                var employeeIdCondition = new ConditionalModel
                {
                    FieldName = "Employee_ID",
                    ConditionalType = ConditionalType.Equal,
                    FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(),
                    CSharpTypeName = "long"
                };

                // 将员工ID条件添加到所有条件组
                foreach (var group in list)
                {
                    group.Conditions.Add(employeeIdCondition);
                }
            }
            return list;
        }


        public List<ConditionGroup> GetSalesOrderSpecialConditions()
        {
            // 添加销售限制条件

            var list = new List<ConditionGroup>
        {
            new ConditionGroup
            {
                StatusName = "待出库",
                Conditions = new List<IConditionalModel>
                {
                    new ConditionalModel { FieldName = "ApprovalStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1", CSharpTypeName = "int" },
                    new ConditionalModel { FieldName = "DataStatus", ConditionalType = ConditionalType.Equal, FieldValue =((int)DataStatus.确认).ToString(), CSharpTypeName = "int" },
                    new ConditionalModel { FieldName = "isdeleted", ConditionalType = ConditionalType.Equal, FieldValue = "False", CSharpTypeName = "bool" }
                }
            }
        };

            // 添加销售限制条件（如果有权限限制）
            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
            {
                var employeeIdCondition = new ConditionalModel
                {
                    FieldName = "Employee_ID",
                    ConditionalType = ConditionalType.Equal,
                    FieldValue = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString(),
                    CSharpTypeName = "long"
                };

                // 将员工ID条件添加到所有条件组
                foreach (var group in list)
                {
                    group.Conditions.Add(employeeIdCondition);
                }
            }

            return list;
        }
    }

}

using Microsoft.Extensions.Logging;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Processor
{

    public partial class tb_EmployeeProcessor : BaseProcessor
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            //内部有代码指定，不用这里指定
            //queryFilter.QueryEntityType=typeof(tb_Employee);

            //内部的公共部分，外部是特殊情况
            var lambda = Expressionable.Create<tb_Employee>()
                            .And(t => t.Is_enabled == true)
                          .ToExpression();//注意 这一句 不能少
            //这个因为供应商和客户混在一起。限制条件在外面 调用时确定 
            //2024-4-11思路升级  条件可以合并，这里也可以限制。合并时要注意怎么联接
            queryFilter.FilterLimitExpressions.Add(lambda);

            queryFilter.SetQueryField<tb_Employee>(c => c.Employee_NO);
            queryFilter.SetQueryField<tb_Employee>(c => c.Employee_Name);
            queryFilter.SetQueryField<tb_Employee>(c => c.DepartmentID);
            queryFilter.SetQueryField<tb_Employee>(c => c.Email);
            return queryFilter;
        }

        /*
        /// <summary>
        /// 通过表达式，获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public override List<T> GetListByLimitExp<T>(List<T> list)
        {
            //如果限制条件没有。直接返回本身

            List<T> lastList = new List<T>();
            foreach (var item in list)
            {
                lastList.Add(item);
            }
            List<T> listInstances = new List<T>();
            var conditions = new List<IConditionalModel>();

            //List<tb_Employee> listInstances = new List<tb_Employee>();
            //List<tb_Employee> tb_Employees = new List<tb_Employee>();
            //for (int i = 0; i < list.Count; i++)
            //{
            //    tb_Employees.Add(list[i] as tb_Employee);
            //}

            //conditions.Add(new ConditionalModel()
            //{
            //    FieldName = "Is_available",
            //    ConditionalType = ConditionalType.Equal,
            //    FieldValue = "True"
            //    // FieldValue = ((ConstantExpression)binaryExpressionLeft.Right).Value.ToString() == "True" ? "true" : "false"
            //});

            // SqlFunc.

            QueryFilter queryFilter = this.GetQueryFilter();
            if (queryFilter.FilterLimitExpressions != null && queryFilter.FilterLimitExpressions.Count > 0)
            {
                if (queryFilter.FilterLimitExpressions.Count == 0)
                {
                    return list;
                }
                //多个条件？
                if (queryFilter.FilterLimitExpressions.Count > 1)
                {

                }

                // 将 LambdaExpression 转换为 BinaryExpression
                //var binaryExpression = queryFilter.FilterLimitExpressions[0].Body as BinaryExpression;//这样转换不行。只会第得左子树
                var binaryExpression = (BinaryExpression)queryFilter.FilterLimitExpressions[0].Body;

                // 逐层转换表达式
                while (binaryExpression != null)
                {


                    if (binaryExpression.Left != null && binaryExpression.NodeType == ExpressionType.AndAlso)
                    {
                        var binaryExpressionLeft = (BinaryExpression)binaryExpression.Left;
                        //(t.Is_enabled == True)
                        //左边是变量，右边是常量
                        if (binaryExpressionLeft.NodeType == ExpressionType.Equal)
                        {
                            var binaryExpressionright = (ConstantExpression)binaryExpressionLeft.Right;
                            string FieldName = binaryExpressionLeft.Left.GetMemberInfo().Name;
                            string FiledValue = (binaryExpressionright).Value.ToString();
                            for (int i = 0; i < lastList.Count; i++)
                            {
                                if (lastList[i].GetPropertyValue(FieldName).ToString() != FiledValue)
                                {
                                    //listInstances.Add(item);
                                    lastList.Remove(lastList[i]);
                                }
                            }
                        }
                    }

                    // 如果右子树也是 BinaryExpression，则继续转换
                    if (binaryExpression.NodeType == ExpressionType.AndAlso &&
                        binaryExpression.Right is BinaryExpression right)
                    {
                        binaryExpression = right;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return lastList;
        }
        */
    }
}
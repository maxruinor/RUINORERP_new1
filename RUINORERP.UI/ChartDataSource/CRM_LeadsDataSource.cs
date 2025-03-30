using Castle.Core.Resource;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Security;
using RUINORERP.UI.ChartAnalyzer;

namespace RUINORERP.UI.ChartDataSource
{
    /// <summary>
    /// 客户资料数据源（单表结构）
    /// </summary>
    public class CRM_LeadsDataSource : ChartDataSourceBase
    {

        #region  
        public async override Task<ChartDataSet> GetDataAsync(ChartRequest request)
        {
            var (startTime, endTime) = request.GetTimeRange();

            var query = MainForm.Instance.AppContext.Db.Queryable<tb_CRM_Leads>()
                  .WhereIF(request.Employee_ID.HasValue, c => c.Employee_ID == request.Employee_ID.Value)
                 .WhereIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                .Where(c => c.Created_at >= startTime && c.Created_at <= endTime);


            // 构建分组字段（时间维度 + 其他维度）
            var groupFields = new List<string>(); //{ request.GetTimeGroupExpression() };
            groupFields.Add(request.GetGroupByTimeField(request.TimeField));
            groupFields.Add("Employee_ID");
            //groupFields.AddRange(request.Dimensions);


            // 构建 GroupByModel 列表
            var groupByModels = new List<GroupByModel>();
            //var columnName = request.GetGroupByTimeField(request.TimeField);
            //var groupByModel = new GroupByModel
            //{
            //    FieldName = columnName
            //};
            //groupByModels.Add(groupByModel);

            var groupByModel2 = new GroupByModel
            {
                FieldName = "Employee_ID"
            };

            groupByModels.Add(groupByModel2);
            var result = await query
                  .GroupBy(request.GetGroupByTimeField(request.TimeField))
                  .GroupBy(groupByModels)
               .Select(g => (dynamic)new
               {
                   TimeGroup = SqlFunc.DateValue(g.Created_at.Value, DateType.Month), // 时间分组键
                   业务员 = g.Employee_ID,
                   Count = SqlFunc.AggregateCount(1)
               })

               .ToListAsync();

            // 转换为图表数据
            return base.TransformToChartDataSet(result, request);
        }

       
        #endregion



        private void ApplyFilters(ref IQueryable<tb_CRM_Customer> query, IEnumerable<QueryFilter> filters)
        {
            //foreach (var filter in filters)
            //{
            //    query = filter.Field switch
            //    {
            //        "SalesmanID" => query.Where(c => c.SalesmanID == filter.Value.ToString()),
            //        "Region" => query.Where(c => c.Region_ID == filter.Value.ToLong()),
            //        "CreateTime" => ApplyDateFilter(query, filter),
            //        _ => query
            //    };
            //}
        }

        
    }
}

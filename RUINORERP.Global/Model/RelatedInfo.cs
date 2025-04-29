using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.Model
{



    /// <summary>
    /// 一个表示关联关系的类，用于指向性，比方    base.SetRelatedBillCols<tb_SaleOrder>(c => c.SOrderNo, r => r.SaleOrderNo);
    /// 销售出库单关联销售订单，也显示了订单号但是订单号字段名不一样
    /// </summary>
    public class RelatedInfo<T1, T2>
    {

        public Expression<Func<T1, object>> ExpRelatedPKField { get; set; }
        public Expression<Func<T1, object>> ExpRelatedDisplayField { get; set; }

        public Expression<Func<T2, object>> ExpSourceDisplayField { get; set; }


        public RelatedInfo(Expression<Func<T1, object>> _ExpRelatedPKField, Expression<Func<T1, object>> _ExpRelatedDisplayField, Expression<Func<T2, object>> _ExpSourceDisplayField)
        {
            ExpRelatedPKField = _ExpRelatedPKField;
            ExpRelatedDisplayField = _ExpRelatedDisplayField;
            ExpSourceDisplayField = _ExpSourceDisplayField;
        }
        public RelatedInfo(Expression<Func<T1, object>> _ExpRelatedPKField,  Expression<Func<T2, object>> _ExpSourceDisplayField)
        {
            ExpRelatedPKField = _ExpRelatedPKField;
            ExpSourceDisplayField = _ExpSourceDisplayField;
        }
        public RelatedInfo()
        {

        }
    }

 
    //在销售出库单列表中双击订单号，显示销售订单
    //通过出库单中的订单ID或者订单号关联到销售订单

    /// <summary>
    /// 关联信息
    /// </summary>
    public class RelatedInfo
    {
        public RelatedInfo()
        {

        }


        public string SourceTableName { get; set; }

        /// <summary>
        /// 目标的实体表名 可以手动指定。也可能是来自于源表中的某字段的内容。所以将目标表名设置为一个key value ,如果指向表时value可以为空
        /// </summary>
        public KeyNamePair TargetTableName { get; set; }

        /// <summary>
        /// 不是真正的主键 。但是也是唯一性的查询条件，比方单号
        /// </summary>
        public string SourceUniqueField { get; set; }

        /// <summary>
        /// 最终就是为了显示友好的字段
        /// </summary>
        public string TargetDisplayField { get; set; }
    }


}

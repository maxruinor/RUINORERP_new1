     using System;
    using SqlSugar;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model.BillDetailDto
    {
        /// <summary>
        /// 销售订单明细
        /// </summary>
        [Serializable()]
        [SugarTable("tb_SalesOrderDetail")]
        public partial class sales_order_detail : BaseEntity, ICloneable
        {
            public sales_order_detail()
            {
                base.FieldNameList = fieldNameList;
                if (!PK_FK_ID_Check())
                {
                    throw new Exception("tb_SalesOrderDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
                }
            }



            private int _SaleOrderDetail_ID;
            /// <summary>
            /// 
            /// </summary>
            [SugarColumn(ColumnName = "SaleOrderDetail_ID", IsNullable = false, ColumnDescription = "明细ID", IsPrimaryKey = true, IsIdentity = true)]
        [Visible(false)]
        public int SaleOrderDetail_ID
            {
                get { return _SaleOrderDetail_ID; }
                set
                {
                    SetProperty(ref _SaleOrderDetail_ID, value);
                }
            }


            private int? _Order_ID;
            /// <summary>
            /// 订单ID
            /// </summary>
            [SugarColumn(ColumnName = "Order_ID", IsNullable = true, ColumnDescription = "订单")]
            [Visible(false)]
            public int? Order_ID
            {
                get { return _Order_ID; }
                set
                {
                    SetProperty(ref _Order_ID, value);
                }
            }


            private int? _Product_ID;
            /// <summary>
            /// 产品ID
            /// </summary>
            [SugarColumn(ColumnName = "Product_ID", IsNullable = true, ColumnDescription = "产品")]
            [Visible(false)]
            public int? Product_ID
            {
                get { return _Product_ID; }
                set
                {
                    SetProperty(ref _Product_ID, value);
                }
            }


            private int? _quantity;
            /// <summary>
            /// 数量
            /// </summary>
            [SugarColumn(ColumnName = "quantity", IsNullable = true, ColumnDescription = "数量")]
            [Summary]
            [Subtotal]
            [ToolTip("数量不能为空")]
            public int? quantity
            {
                get { return _quantity; }
                set
                {
                    SetProperty(ref _quantity, value);
                }
            }


            private decimal? _price;
            /// <summary>
            /// 单价
            /// </summary>
            [SugarColumn(ColumnName = "price", IsNullable = true, ColumnDescription = "单价")]
            [Subtotal]
            public decimal? price
            {
                get { return _price; }
                set
                {
                    SetProperty(ref _price, value);
                }
            }


        private decimal? _SubTotal;
        /// <summary>
        /// 单价
        /// </summary>
        [SugarColumn(ColumnName = "SubTotal", IsNullable = true, ColumnDescription = "金额小计")]
        [SubtotalResultAttribute]
        [Summary]
        [ReadOnly(true)]
        public decimal? SubTotal
        {
            get { return _SubTotal; }
            set
            {
                SetProperty(ref _SubTotal, value);
            }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [SugarColumn(ColumnName = "Summary", Length = 255, IsNullable = true, ColumnDescription = "摘要")]
        public string Summary
        {
            get { return _Summary; }
            set
            {
                SetProperty(ref _Summary, value);
            }
        }


        //如果为false,则不可以。
        private bool PK_FK_ID_Check()
            {
                bool rs = true;
                return rs;
            }


            #region 字段描述对应列表
            private ConcurrentDictionary<string, string> fieldNameList;


            /// <summary>
            /// 表列名的中文描述集合
            /// </summary>
            [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
            [SugarColumn(IsIgnore = true)]
            public override ConcurrentDictionary<string, string> FieldNameList
            {
                get
                {
                    if (fieldNameList == null)
                    {
                        fieldNameList = new ConcurrentDictionary<string, string>();
                        SugarColumn entityAttr;
                        Type type = typeof(tb_SalesOrderDetail);

                        foreach (PropertyInfo field in type.GetProperties())
                        {
                            foreach (Attribute attr in field.GetCustomAttributes(true))
                            {
                                entityAttr = attr as SugarColumn;
                                if (null != entityAttr)
                                {
                                    if (entityAttr.ColumnDescription == null)
                                    {
                                        continue;
                                    }
                                    if (entityAttr.IsIdentity)
                                    {
                                        continue;
                                    }
                                    if (entityAttr.IsPrimaryKey)
                                    {
                                        continue;
                                    }
                                    if (entityAttr.ColumnDescription.Trim().Length > 0)
                                    {
                                        fieldNameList.TryAdd(field.Name, entityAttr.ColumnDescription);
                                    }
                                }
                            }
                        }
                    }

                    return fieldNameList;
                }
                set
                {
                    fieldNameList = value;
                }

            }
            #endregion


            public object Clone()
            {
                tb_SalesOrderDetail loctype = (tb_SalesOrderDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
                return loctype;
            }
        }
    }



using FastReport.DevComponents.DotNetBar;
using FastReport.Editor.Dialogs;
using Microsoft.Extensions.Logging;
using NPOI.POIFS.Crypt.Dsig;
using RUINORERP.Model;
using RUINORERP.UI.UCSourceGrid;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.BusinessService.CalculationService
{

    /// <summary>
    /// 单据主子表双向计算协调器
    /// 如：销售出库单中，运费成本会与运费成本分摊相互计算
    /// </summary>
    /// <typeparam name="TMaster"></typeparam>
    /// <typeparam name="TDetail"></typeparam>
    public abstract class SourceGridCoordinator<TMaster, TDetail>
        where TMaster : BaseEntity
        where TDetail : BaseEntity
    {
        public readonly TMaster _master;
        public readonly List<TDetail> _details;
        public readonly SourceGridHelper _gridHelper;
        private readonly System.Threading.Timer _calculationTimer;
        private readonly ConcurrentQueue<GridChangeEvent> _pendingEvents = new();
        private volatile bool _isProcessing;
        private const int CalculationDelay = 300; // ms

        public Dictionary<string, string> MapFields { get; private set; } = new Dictionary<string, string>();

        protected SourceGridCoordinator(
            TMaster master,
            List<TDetail> details,
            SourceGridHelper gridHelper)
        {
            _master = master;
            _details = details;
            _gridHelper = gridHelper;

            // 注册主表变更监听
            _master.PropertyChanged += (s, e) =>
            {
                if (MapFields.ContainsKey(e.PropertyName))
                {
                    EnqueueEvent(GridChangeType.MasterProperty, e.PropertyName);
                }
            };

            // 注册明细变更监听
            _gridHelper.OnCalculateColumnValue += (rowObj, gridDefine, position) =>
            {
                if (rowObj is TDetail detail)
                {
                    EnqueueEvent(GridChangeType.DetailCell, detail);
                }
            };

            // 初始化延迟计算定时器
            _calculationTimer = new System.Threading.Timer(_ => ProcessQueue(), null, Timeout.Infinite, Timeout.Infinite);
        }

        public void EnqueueEvent(GridChangeType changeType, string masterPropertyName)
        {
            if (MapFields.ContainsKey(masterPropertyName))
            {
                _pendingEvents.Enqueue(new GridChangeEvent(changeType, masterPropertyName));
                ResetCalculationTimer();
            }
        }

        public void EnqueueEvent(GridChangeType changeType, TDetail detail)
        {
            _pendingEvents.Enqueue(new GridChangeEvent(changeType, _master, detail));
            ResetCalculationTimer();
        }

        private void ResetCalculationTimer()
        {
            _calculationTimer.Change(CalculationDelay, Timeout.Infinite);
        }

        private void ProcessQueue()
        {
            if (_isProcessing) return;
            _isProcessing = true;
            try
            {
                while (_pendingEvents.TryDequeue(out var changeEvent))
                {
                    try
                    {
                        switch (changeEvent.ChangeType)
                        {
                            case GridChangeType.MasterProperty:
                                HandleMasterPropertyChange(changeEvent.MasterEntity.ToString());
                                break;
                            case GridChangeType.DetailCell:
                                HandleDetailChange(_master, (TDetail)changeEvent.DetailEntities);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        // 记录错误但不中断处理
                    }
                }
            }
            finally
            {
                _isProcessing = false;
            }
        }

        protected abstract void HandleMasterPropertyChange(string propertyName);
        protected abstract void HandleDetailChange(TMaster master, TDetail detail);
        protected abstract void SetMapFields();
        public void SetMapFields(Expression<Func<TMaster, object>> fromExp, Expression<Func<TDetail, object>> toExp)
        {
            string fromColName = fromExp.GetMemberInfo().Name;
            string toColName = toExp.GetMemberInfo().Name;
            MapFields[fromColName] = toColName;
        }

        protected void SafeExecute(Action action)
        {
            try
            {
                _isProcessing = true;
                action();
            }
            finally
            {
                _isProcessing = false;
            }
        }
    }

    public enum GridChangeType { MasterProperty, DetailCell }

    public class GridChangeEvent
    {
        public GridChangeEvent(GridChangeType changeType, object MasterPropertyName)
        {
            ChangeType = changeType;
            MasterEntity = MasterPropertyName;
        }

        public GridChangeEvent(GridChangeType changeType, object masterEntity, object detailEntities)
        {
            ChangeType = changeType;
            MasterEntity = masterEntity;
            DetailEntities = detailEntities;
        }

        public GridChangeType ChangeType { get; private set; }
        public object MasterEntity { get; private set; }
        public object DetailEntities { get; private set; }

        // 重写 Equals 方法 - 同时比较 MasterEntity 和 DetailEntities
        public override bool Equals(object obj)
        {
            return obj is GridChangeEvent other &&
                   ChangeType == other.ChangeType &&
                   Equals(MasterEntity, other.MasterEntity) &&
                   Equals(DetailEntities, other.DetailEntities);
        }

        // 重写 GetHashCode 方法 - 包含所有关键属性
        public override int GetHashCode()
        {
            return Tuple.Create(ChangeType, MasterEntity, DetailEntities).GetHashCode();
        }

        // 重写 ToString 方法 - 包含所有属性
        public override string ToString()
        {
            return $"GridChangeEvent {{ ChangeType = {ChangeType}, MasterEntity = {MasterEntity}, DetailEntities = {DetailEntities} }}";
        }

        // 实现解构方法 - 包含所有属性
        public void Deconstruct(out GridChangeType changeType, out object masterEntity, out object detailEntities)
        {
            changeType = ChangeType;
            masterEntity = MasterEntity;
            detailEntities = DetailEntities;
        }
    }


}

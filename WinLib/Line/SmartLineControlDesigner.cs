
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
namespace CSharpWin.Line
{
    /// <summary>
    /// 直线描述
    /// </summary>
    public class SmartLineControlDesigner : ParentControlDesigner
    {


        DesignerVerbCollection verbs;
        IComponentChangeService componentChangeService;
        /// <summary>
        /// 重写右键菜单
        /// </summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (verbs == null)
                {
                    verbs = new DesignerVerbCollection();
                    DesignerVerb enableAutoScrollScrollVerb = new DesignerVerb("水平放置直线", new EventHandler(OnOrientationChange));
                    DesignerVerb disableAutoScrollVerb = new DesignerVerb("垂直放置直线", new EventHandler(OnOrientationChange));

                    verbs.Add(enableAutoScrollScrollVerb);
                    verbs.Add(disableAutoScrollVerb);
                }
                UpdateVerbsStatus();
                return verbs;
            }
        }
        /// <summary>
        /// 获取控件
        /// </summary>
        public new SmartLine Component
        {
            get { return (SmartLine)base.Component; }
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        protected void UpdateVerbsStatus()
        {
            verbs[0].Enabled = Component.Orientation != Orientation.Horizontal;
            verbs[1].Enabled = !verbs[0].Enabled;
        }
        /// <summary>
        /// 控件改变服务
        /// </summary>
        public IComponentChangeService ComponentChangeService
        {
            get
            {
                if (componentChangeService == null)
                {
                    componentChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
                }
                return componentChangeService;
            }
        }

        /// <summary>
        /// 界面布局改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnOrientationChange(object sender, EventArgs e)
        {
            try
            {
                Component.Orientation = (Component.Orientation == Orientation.Horizontal) ? Orientation.Vertical : Orientation.Horizontal;
                ComponentChangeService.OnComponentChanged(Component, null, null, null);
            }
            finally
            {
                UpdateVerbsStatus();
            }
        }
    }
}


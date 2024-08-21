using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Permissions;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-10-08
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public abstract class SystemMenuRenderer
    {
        #region Fields

        private EventHandlerList _events;
        private int _menuBarWidth = 24;

        private static readonly object EventMeasureSystemMenuItem = new object();
        private static readonly object EventRenderSystemMenuNC = new object();
        private static readonly object EventRenderSystemMenuItem = new object();

        #endregion

        protected SystemMenuRenderer() { }

        #region Properties

        public virtual int MenuBarWidth 
        {
            get { return _menuBarWidth; }
            protected set { _menuBarWidth = value; }
        }

        protected EventHandlerList Events
        {
            get
            {
                if (_events == null)
                {
                    _events = new EventHandlerList();
                }
                return _events;
            }
        }

        #endregion

        #region Events

        public event MeasureItemExEventHandler MeasureSystemMenuItem
        {
            add { AddHandler(EventMeasureSystemMenuItem, value); }
            remove { RemoveHandler(EventMeasureSystemMenuItem, value); }
        }

        public event DrawItemExEventHandler RenderSystemMenuItem
        {
            add { AddHandler(EventRenderSystemMenuItem, value); }
            remove { RemoveHandler(EventRenderSystemMenuItem, value); }
        }

        public event SystemMenuNCRenderEventHandler RenderSystemMenuNC
        {
            add { AddHandler(EventRenderSystemMenuNC, value); }
            remove { RemoveHandler(EventRenderSystemMenuNC, value); }
        }

        #endregion

        #region Public Methods

        public void CalcSystemMenuItem(
            MeasureItemExEventArgs e)
        {
            OnMeasureSystemMenuItem(e);
            MeasureItemExEventHandler handle =
                Events[EventMeasureSystemMenuItem] as MeasureItemExEventHandler;
            if (handle != null)
            {
                handle(this, e);
            }
        }

        public void DrawSystemMenuItem(
            DrawItemExEventArgs e)
        {
            OnRenderSystemMenuItem(e);
            DrawItemExEventHandler handle =
                Events[EventRenderSystemMenuItem] as DrawItemExEventHandler;
            if (handle != null)
            {
                handle(this, e);
            }
        }

        public void DrawSystemMenuNC(
            SystemMenuNCRenderEventArgs e)
        {
            OnRenderSystemMenuNC(e);
            SystemMenuNCRenderEventHandler handle =
                Events[EventRenderSystemMenuNC] as SystemMenuNCRenderEventHandler;
            if (handle != null)
            {
                handle(this, e);
            }
        }

        #endregion

        #region Protected Render/Measure Methods

        protected virtual void OnMeasureSystemMenuItem(
            MeasureItemExEventArgs e)
        {
            Graphics g = e.Graphics;
            string text = e.Text;
            Font font = e.Font;

            if (text != "-" && !string.IsNullOrEmpty(text))
            {
                Size size = TextRenderer.MeasureText(
                    g,
                    text,
                    font);
                e.ItemWidth =
                    size.Width + SystemInformation.MenuCheckSize.Width + 2;
            }
        }

        protected abstract void OnRenderSystemMenuItem(
           DrawItemExEventArgs e);

        protected abstract void OnRenderSystemMenuNC(
           SystemMenuNCRenderEventArgs e);

        #endregion

        #region Protected Methods

        [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
        protected void AddHandler(object key, Delegate value)
        {
            Events.AddHandler(key, value);
        }

        [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
        protected void RemoveHandler(object key, Delegate value)
        {
            Events.RemoveHandler(key, value);
        }

        #endregion
    }
}

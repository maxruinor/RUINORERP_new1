namespace SHControls.DataGrid
{

    using System;
    /// <summary>
    /// ButtonArray 的摘要说明。
    /// </summary>
    public class ButtonArray : System.Collections.CollectionBase
    {

        // 容器的一个引用,引用该控件数组所在的窗体的一个引用
        private readonly System.Windows.Forms.Form HostForm;

        // 添加一个新的按钮
        public System.Windows.Forms.Button AddNewButton()
        {
            // 创建一个新的按钮实例
            System.Windows.Forms.Button aButton = new System.Windows.Forms.Button();
            // 将该新的控件添加到(this)列表中
            this.List.Add(aButton);
            // 将控件添加到父窗体的控件集合中
            HostForm.Controls.Add(aButton);
            // 设置该控件的属性
            aButton.Top = Count * 25;
            aButton.Left = 100;
            // 用Button的Tag表示控件数组中的索引
            aButton.Tag = this.Count;
            aButton.Text = "Button " +
                this.Count.ToString();
            // 添加一个事件响应
            aButton.Click += new
                System.EventHandler(ClickHandler);
            // 返回该新的控件
            return aButton;
        }

        // 删除控件数组中的最后一个元素
        public void Remove()
        {
            // 检测该控件数组中是否有控件
            if (this.Count > 0)
            {
                // 如果有则先从父容器中删除最后一个元素
                // 注意:此处是通过索引访问

                this.HostForm.Controls.Remove(this[this.Count - 1]);
                // 删除控件数组中的最后一个控件
                this.List.RemoveAt(this.Count - 1);
            }
        }

        // 添加一个事件出来函数
        public void ClickHandler(Object sender,
            System.EventArgs e)
        {
            // 用MessageBox返回一条消息
            System.Windows.Forms.MessageBox.Show("你点击的是 " +

                ((System.Windows.Forms.Button)
                sender).Tag.ToString());
        }

        // 带父窗体引用的构造函数
        public ButtonArray(System.Windows.Forms.Form host)
        {
            // 为父窗体的引用赋值
            this.HostForm = host;
            // 添加一个默认的按钮
            this.AddNewButton();
        }

        // 控件数组的索引可以利用索引访问(只能得到)
        public System.Windows.Forms.Button this[int index]
        {
            get
            {
                return
                    (System.Windows.Forms.Button)this.List[index];
            }
        }
    }
}
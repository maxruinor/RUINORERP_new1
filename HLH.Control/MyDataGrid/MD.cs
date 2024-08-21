namespace SHControls.DataGrid
{

    using System;
    /// <summary>
    /// ButtonArray ��ժҪ˵����
    /// </summary>
    public class ButtonArray : System.Collections.CollectionBase
    {

        // ������һ������,���øÿؼ��������ڵĴ����һ������
        private readonly System.Windows.Forms.Form HostForm;

        // ���һ���µİ�ť
        public System.Windows.Forms.Button AddNewButton()
        {
            // ����һ���µİ�ťʵ��
            System.Windows.Forms.Button aButton = new System.Windows.Forms.Button();
            // �����µĿؼ���ӵ�(this)�б���
            this.List.Add(aButton);
            // ���ؼ���ӵ�������Ŀؼ�������
            HostForm.Controls.Add(aButton);
            // ���øÿؼ�������
            aButton.Top = Count * 25;
            aButton.Left = 100;
            // ��Button��Tag��ʾ�ؼ������е�����
            aButton.Tag = this.Count;
            aButton.Text = "Button " +
                this.Count.ToString();
            // ���һ���¼���Ӧ
            aButton.Click += new
                System.EventHandler(ClickHandler);
            // ���ظ��µĿؼ�
            return aButton;
        }

        // ɾ���ؼ������е����һ��Ԫ��
        public void Remove()
        {
            // ���ÿؼ��������Ƿ��пؼ�
            if (this.Count > 0)
            {
                // ��������ȴӸ�������ɾ�����һ��Ԫ��
                // ע��:�˴���ͨ����������

                this.HostForm.Controls.Remove(this[this.Count - 1]);
                // ɾ���ؼ������е����һ���ؼ�
                this.List.RemoveAt(this.Count - 1);
            }
        }

        // ���һ���¼���������
        public void ClickHandler(Object sender,
            System.EventArgs e)
        {
            // ��MessageBox����һ����Ϣ
            System.Windows.Forms.MessageBox.Show("�������� " +

                ((System.Windows.Forms.Button)
                sender).Tag.ToString());
        }

        // �����������õĹ��캯��
        public ButtonArray(System.Windows.Forms.Form host)
        {
            // Ϊ����������ø�ֵ
            this.HostForm = host;
            // ���һ��Ĭ�ϵİ�ť
            this.AddNewButton();
        }

        // �ؼ��������������������������(ֻ�ܵõ�)
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
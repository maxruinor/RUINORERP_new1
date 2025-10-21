using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Plugin.AlibabaStoreManager
{
    public partial class FavoritesForm : Form
    {
        private List<string> favorites;
        private Action<string> onUrlSelected;

        public FavoritesForm(List<string> favorites, Action<string> onUrlSelected)
        {
            InitializeComponent();
            this.favorites = favorites ?? new List<string>();
            this.onUrlSelected = onUrlSelected;
            LoadFavorites();
        }

        private void LoadFavorites()
        {
            listBoxFavorites.Items.Clear();
            foreach (string favorite in favorites)
            {
                listBoxFavorites.Items.Add(favorite);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (listBoxFavorites.SelectedItem != null)
            {
                string selectedUrl = listBoxFavorites.SelectedItem.ToString();
                onUrlSelected?.Invoke(selectedUrl);
                this.Close();
            }
            else
            {
                MessageBox.Show("请先选择一个收藏的网址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listBoxFavorites.SelectedItem != null)
            {
                string selectedUrl = listBoxFavorites.SelectedItem.ToString();
                if (MessageBox.Show($"确定要删除收藏的网址 '{selectedUrl}' 吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    favorites.Remove(selectedUrl);
                    LoadFavorites();
                }
            }
            else
            {
                MessageBox.Show("请先选择一个要删除的收藏网址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void listBoxFavorites_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxFavorites.SelectedItem != null)
            {
                string selectedUrl = listBoxFavorites.SelectedItem.ToString();
                onUrlSelected?.Invoke(selectedUrl);
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
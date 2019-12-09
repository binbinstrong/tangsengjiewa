using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 唐僧解瓦.机电.ToolUIs
{
    /// <summary>
    /// ValueSettingUI.xaml 的交互逻辑
    /// </summary>
    public partial class ValueSettingUI : Window
    {
        public ValueSettingUI()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
        }

        private void Okbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}

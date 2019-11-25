using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace 唐僧解瓦.机电.About
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
             Process.Start("Explorer", "https://bimdp.ke.qq.com");
        }
    }
}

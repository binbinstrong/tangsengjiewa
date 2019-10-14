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

namespace 唐僧解瓦.通用.UIs
{
    /// <summary>
    /// FloorSelector.xaml 的交互逻辑
    /// </summary>
    public partial class FloorSelector : Window
    {
        private static FloorSelector instance;

        public static FloorSelector Instance
        {
            get
            {
                if (instance == null)
                {
                    instance =new FloorSelector();
                }
                return instance;
            }
        }
        private FloorSelector()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            //base.OnClosing(e);
        }
    }
}

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
    /// FloorTypeSelector.xaml 的交互逻辑
    /// </summary>
    public partial class FloorTypeSelector : Window
    {
        public FloorTypeSelector()
        {
            InitializeComponent();
        }

        private static FloorTypeSelector instance;

        public static FloorTypeSelector Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FloorTypeSelector();
                }
                return instance;
            }
        }
       


        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            //base.OnClosing(e);
        }
    }
}

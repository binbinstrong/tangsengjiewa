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
    /// ViewSemutaneousSelector.xaml 的交互逻辑
    /// </summary>
    public partial class ViewSemutaneousSelector : Window
    {
        private static ViewSemutaneousSelector instance;

        public static ViewSemutaneousSelector Instance
        {
            get
            {
                if (instance == null)
                {
                    instance= new ViewSemutaneousSelector();
                }

                return instance;
            }
        }

        public ViewSemutaneousSelector()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            //base.OnClosing(e);
        }

        private void Clicked(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}

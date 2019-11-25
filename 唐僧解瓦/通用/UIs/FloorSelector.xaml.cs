using System.ComponentModel;
using System.Windows;

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

using System.ComponentModel;
using System.Windows;

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

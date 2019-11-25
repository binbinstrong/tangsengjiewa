using System.ComponentModel;
using System.Windows;

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

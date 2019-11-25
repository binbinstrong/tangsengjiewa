using System.Windows;

namespace 唐僧解瓦.机电.ToolUIs
{
    /// <summary>
    /// MepcurveRank.xaml 的交互逻辑
    /// </summary>
    public partial class MepcurveRank : Window
    {
        private static MepcurveRank instance;

        public static MepcurveRank Instance
        {
            get
            {
                if(instance==null)
                {
                    instance= new MepcurveRank();
                }
                return instance;
            }
        }
        public MepcurveRank()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
             
        }
    }
}

using System;
using System.Collections.Generic;
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

namespace 红瓦功能揭秘.机电.ToolUIs
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

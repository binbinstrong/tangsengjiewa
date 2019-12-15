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
using Autodesk.Revit.DB;
using 唐僧解瓦.BinLibrary.Extensions;

namespace 唐僧解瓦.注释.UIs
{
    /// <summary>
    /// ResultShow.xaml 的交互逻辑
    /// </summary>
    public partial class ResultShow : Window
    {
        private static ResultShow instance;

        ResultShow()
        {
            InitializeComponent();
        }

        public static ResultShow Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResultShow();
                }
                return instance;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            instance = null;
        }

        private void OnClicked(object sender, RoutedEventArgs e)
        {
            var ids = Cmd_LengthAccumulate.addedIds;
            var doc = Cmd_LengthAccumulate._doc;

            var modellines = ids.Select(m => m.GetElement(doc) as ModelLine);

            var totallen = default(double);
            foreach (ModelLine mline in modellines)
            {
                var para = mline.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                var lenth = para.AsDouble().FeetToMetric();
                totallen += lenth;
            }

            this.totalLen.Text = totallen.ToString();
            //MessageBox.Show(ids.Count.ToString());
        }
    }
}

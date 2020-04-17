using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using 唐僧解瓦.BinLibrary.Extensions;
using Form = System.Windows.Forms.Form;

namespace 唐僧解瓦.机电.ToolUIs
{
    public partial class FilterWindow : Form
    {
        private Document _doc;


        public FilterWindow()
        {
            InitializeComponent();
        }

        public FilterWindow(Document doc)
        {
            _doc = doc;
            InitializeComponent();
        }

        private void FilterWindow_Load(object sender, EventArgs e)
        {
            var uidoc = new UIDocument(_doc);

            var sel = uidoc.Selection;

            var elementids = sel.GetElementIds();

            var elements = elementids.Select(m =>
            {
                return m.GetElement(_doc);

            });

            //MessageBox.Show("nums :" + elements.Count().ToString());


            List<Category> catelist = new List<Category>();

            //listView_CateList.Clear();

            foreach (Element ele in elements)
            {
                var thiscate = ele.Category;
                if (!catelist.Contains(thiscate))
                {
                    catelist.Add(thiscate);
                    //MessageBox.Show(thiscate.Name);
                    listView_CateList.Items.Add(thiscate.Name);
                }
            }

            //MessageBox.Show(listView_CateList.Items.Count.ToString());
             
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            var uidoc = new UIDocument(_doc);

            var sel = uidoc.Selection;

            var elementids = sel.GetElementIds();
             
            var newElementIds = new List<ElementId>();

            var catesChecks = listView_CateList.Items.Cast<ListViewItem>().Where(m => m.Checked);
            //MessageBox.Show("checks count:"+catesChecks.Count().ToString());

            if (catesChecks.Count() > 0)
            {
                var cateNames = catesChecks.Select(m => m.Text);

                var names = string.Join("\n", cateNames);
                //MessageBox.Show(names);


                foreach (var id in elementids)
                {
                    var ele = id.GetElement(_doc);
                    var cate = ele.Category;
                    //MessageBox.Show(cate.Name);

                    if (cateNames.Contains(cate.Name))
                    {
                        newElementIds.Add(id);
                    }
                }

            }

            //MessageBox.Show(elementids.Count.ToString());


            if (newElementIds.Count > 0)
            {
                sel.SetElementIds(newElementIds);
                //MessageBox.Show("executed!");

            }

            this.Close();
        }
    }
}

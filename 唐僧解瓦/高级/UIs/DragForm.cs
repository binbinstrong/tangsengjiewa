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
using Form = System.Windows.Forms.Form;

namespace 唐僧解瓦.高级.UIs
{
    public partial class DragForm : Form
    {
        private static DragForm instance;
        private UIDocument _uidoc;
        public static DragForm Instance
        {
            get {
                if (instance == null)
                {
                    instance= new DragForm();
                }

                return instance;
            }
        }

        public static DragForm GetInstance(UIDocument uidoc)
        {
            return new DragForm(uidoc);
        }

        public DragForm()
        {
            InitializeComponent();
        }

        public DragForm(UIDocument uidoc)
        {
            InitializeComponent();
            _uidoc = uidoc;

        }

        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            UIApplication.DoDragDrop( null,new handler());
            
        }



    }

    public class handler : IDropHandler
    {
        public void Execute(UIDocument document, object data)
        {
            
            
        }
    }
}

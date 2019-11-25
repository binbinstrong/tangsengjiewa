using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Form = System.Windows.Forms.Form;

namespace 唐僧解瓦.Test.UIs
{
    public partial class ColumnTypesForm : Form
    {
        private static ColumnTypesForm instance;

        //public static ColumnTypesForm GetInstance()
        //{

        //}

        public static ColumnTypesForm Getinstance(List<Element> elements)
        {
             if (instance == null)
             {
                 instance = new ColumnTypesForm(elements);
             }
             return instance;
        }

        private List<FamilySymbol> symbols;
        public ColumnTypesForm()
        {
            InitializeComponent();
        }

        ColumnTypesForm(List<Element> elements)
        {
            symbols = elements.Cast<FamilySymbol>().ToList();
            InitializeComponent();
        }

        private void ColumnTypesForm_Load(object sender, EventArgs e)
        {
            this.symbolCombo.Items.Clear();
            this.symbolCombo.DataSource = symbols;
            this.symbolCombo.DisplayMember = "Name";
            symbolCombo.SelectedIndex = 0;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            //base.OnClosing(e);
        }
    }
}

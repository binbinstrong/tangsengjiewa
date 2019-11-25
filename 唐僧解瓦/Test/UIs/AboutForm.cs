using System;
using System.Windows.Forms;

namespace 唐僧解瓦.Test.UIs
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            this.Text = "唐僧工具箱";
            this.label1.Text = $"本工具为**内部工具\n旨在提高bim建模效率\n提升数字化建模水平。";
        }
    }
}

namespace 唐僧解瓦.Test.UIs
{
    partial class ColumnTypesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.symbolCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // symbolCombo
            // 
            this.symbolCombo.FormattingEnabled = true;
            this.symbolCombo.Location = new System.Drawing.Point(65, 40);
            this.symbolCombo.Name = "symbolCombo";
            this.symbolCombo.Size = new System.Drawing.Size(258, 20);
            this.symbolCombo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "柱子族类型";
            // 
            // ColumnTypesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 129);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.symbolCombo);
            this.Name = "ColumnTypesForm";
            this.Text = "ColumnTypesForm";
            this.Load += new System.EventHandler(this.ColumnTypesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox symbolCombo;
    }
}
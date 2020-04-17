namespace 唐僧解瓦.机电.ToolUIs
{
    partial class FilterWindow
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
            this.label_cate = new System.Windows.Forms.Label();
            this.label_sum = new System.Windows.Forms.Label();
            this.listView_CateList = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.label_totalNum = new System.Windows.Forms.Label();
            this.button_SelectAll = new System.Windows.Forms.Button();
            this.button_DropAll = new System.Windows.Forms.Button();
            this.button_Ok = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Apply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_cate
            // 
            this.label_cate.AutoSize = true;
            this.label_cate.Location = new System.Drawing.Point(21, 14);
            this.label_cate.Name = "label_cate";
            this.label_cate.Size = new System.Drawing.Size(29, 12);
            this.label_cate.TabIndex = 0;
            this.label_cate.Text = "类别";
            // 
            // label_sum
            // 
            this.label_sum.AutoSize = true;
            this.label_sum.Location = new System.Drawing.Point(177, 14);
            this.label_sum.Name = "label_sum";
            this.label_sum.Size = new System.Drawing.Size(29, 12);
            this.label_sum.TabIndex = 1;
            this.label_sum.Text = "合计";
            // 
            // listView_CateList
            // 
            this.listView_CateList.CheckBoxes = true;
            this.listView_CateList.HideSelection = false;
            this.listView_CateList.Location = new System.Drawing.Point(14, 37);
            this.listView_CateList.Name = "listView_CateList";
            this.listView_CateList.Size = new System.Drawing.Size(238, 248);
            this.listView_CateList.TabIndex = 2;
            this.listView_CateList.UseCompatibleStateImageBehavior = false;
            this.listView_CateList.View = System.Windows.Forms.View.List;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 297);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "选定的总数:";
            // 
            // label_totalNum
            // 
            this.label_totalNum.AutoSize = true;
            this.label_totalNum.Location = new System.Drawing.Point(211, 297);
            this.label_totalNum.Name = "label_totalNum";
            this.label_totalNum.Size = new System.Drawing.Size(23, 12);
            this.label_totalNum.TabIndex = 5;
            this.label_totalNum.Text = "num";
            // 
            // button_SelectAll
            // 
            this.button_SelectAll.Location = new System.Drawing.Point(271, 45);
            this.button_SelectAll.Name = "button_SelectAll";
            this.button_SelectAll.Size = new System.Drawing.Size(71, 26);
            this.button_SelectAll.TabIndex = 6;
            this.button_SelectAll.Text = "选择全部";
            this.button_SelectAll.UseVisualStyleBackColor = true;
            // 
            // button_DropAll
            // 
            this.button_DropAll.Location = new System.Drawing.Point(273, 80);
            this.button_DropAll.Name = "button_DropAll";
            this.button_DropAll.Size = new System.Drawing.Size(68, 25);
            this.button_DropAll.TabIndex = 7;
            this.button_DropAll.Text = "放弃全部";
            this.button_DropAll.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.Location = new System.Drawing.Point(105, 328);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(62, 24);
            this.button_Ok.TabIndex = 8;
            this.button_Ok.Text = "确定";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(190, 329);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(61, 22);
            this.button_Cancel.TabIndex = 9;
            this.button_Cancel.Text = "取消";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Apply
            // 
            this.button_Apply.Location = new System.Drawing.Point(278, 329);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(62, 22);
            this.button_Apply.TabIndex = 10;
            this.button_Apply.Text = "应用";
            this.button_Apply.UseVisualStyleBackColor = true;
            // 
            // FilterWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 356);
            this.Controls.Add(this.button_Apply);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.button_DropAll);
            this.Controls.Add(this.button_SelectAll);
            this.Controls.Add(this.label_totalNum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listView_CateList);
            this.Controls.Add(this.label_sum);
            this.Controls.Add(this.label_cate);
            this.Name = "FilterWindow";
            this.Text = "过滤器";
            this.Load += new System.EventHandler(this.FilterWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_cate;
        private System.Windows.Forms.Label label_sum;
        private System.Windows.Forms.ListView listView_CateList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_totalNum;
        private System.Windows.Forms.Button button_SelectAll;
        private System.Windows.Forms.Button button_DropAll;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Apply;
    }
}
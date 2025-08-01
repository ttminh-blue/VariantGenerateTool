namespace Tool
{
    partial class GenerateVariants
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtUrl = new TextBox();
            btnClick = new Button();
            label1 = new Label();
            txtContent = new TextBox();
            txtCopy = new Button();
            SuspendLayout();
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(103, 43);
            txtUrl.Margin = new Padding(4, 5, 4, 5);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(1136, 31);
            txtUrl.TabIndex = 0;
            // 
            // btnClick
            // 
            btnClick.Location = new Point(1283, 36);
            btnClick.Margin = new Padding(4, 5, 4, 5);
            btnClick.Name = "btnClick";
            btnClick.Size = new Size(107, 38);
            btnClick.TabIndex = 1;
            btnClick.Text = "Click";
            btnClick.UseVisualStyleBackColor = true;
            btnClick.Click += btnClick_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(37, 48);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(47, 25);
            label1.TabIndex = 2;
            label1.Text = "URL:";
            // 
            // txtContent
            // 
            txtContent.Location = new Point(33, 103);
            txtContent.Margin = new Padding(4, 5, 4, 5);
            txtContent.Multiline = true;
            txtContent.Name = "txtContent";
            txtContent.ReadOnly = true;
            txtContent.ScrollBars = ScrollBars.Both;
            txtContent.Size = new Size(1551, 812);
            txtContent.TabIndex = 3;
            // 
            // txtCopy
            // 
            txtCopy.Location = new Point(1425, 35);
            txtCopy.Margin = new Padding(4, 5, 4, 5);
            txtCopy.Name = "txtCopy";
            txtCopy.Size = new Size(107, 38);
            txtCopy.TabIndex = 4;
            txtCopy.Text = "Copy";
            txtCopy.UseVisualStyleBackColor = true;
            txtCopy.Click += txtCopy_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1623, 938);
            Controls.Add(txtCopy);
            Controls.Add(txtContent);
            Controls.Add(label1);
            Controls.Add(btnClick);
            Controls.Add(txtUrl);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "Generate Variants";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtUrl;
        private Button btnClick;
        private Label label1;
        private TextBox txtContent;
        private Button txtCopy;
    }
}
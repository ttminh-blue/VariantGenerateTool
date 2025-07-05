namespace Tool
{
    partial class Form1
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
            txtEmail = new TextBox();
            label2 = new Label();
            label3 = new Label();
            txtToken = new TextBox();
            SuspendLayout();
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(72, 26);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(313, 23);
            txtUrl.TabIndex = 0;
            // 
            // btnClick
            // 
            btnClick.Location = new Point(1035, 25);
            btnClick.Name = "btnClick";
            btnClick.Size = new Size(75, 23);
            btnClick.TabIndex = 1;
            btnClick.Text = "Click";
            btnClick.UseVisualStyleBackColor = true;
            btnClick.Click += btnClick_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 29);
            label1.Name = "label1";
            label1.Size = new Size(31, 15);
            label1.TabIndex = 2;
            label1.Text = "URL:";
            // 
            // txtContent
            // 
            txtContent.Location = new Point(23, 62);
            txtContent.Multiline = true;
            txtContent.Name = "txtContent";
            txtContent.Size = new Size(1087, 489);
            txtContent.TabIndex = 3;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(450, 27);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(234, 23);
            txtEmail.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(405, 30);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 5;
            label2.Text = "Email:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(699, 30);
            label3.Name = "label3";
            label3.Size = new Size(41, 15);
            label3.TabIndex = 6;
            label3.Text = "Token:";
            // 
            // txtToken
            // 
            txtToken.Location = new Point(746, 26);
            txtToken.Name = "txtToken";
            txtToken.Size = new Size(283, 23);
            txtToken.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1136, 563);
            Controls.Add(txtToken);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtEmail);
            Controls.Add(txtContent);
            Controls.Add(label1);
            Controls.Add(btnClick);
            Controls.Add(txtUrl);
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
        private TextBox txtEmail;
        private Label label2;
        private Label label3;
        private TextBox txtToken;
    }
}
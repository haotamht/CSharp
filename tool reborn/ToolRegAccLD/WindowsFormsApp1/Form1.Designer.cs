
namespace WindowsFormsApp1
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.keyproxytm = new System.Windows.Forms.TextBox();
            this.accfb = new System.Windows.Forms.TextBox();
            this.otpsim = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.loaisim = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(597, 335);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 69);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // keyproxytm
            // 
            this.keyproxytm.Location = new System.Drawing.Point(33, 61);
            this.keyproxytm.Multiline = true;
            this.keyproxytm.Name = "keyproxytm";
            this.keyproxytm.Size = new System.Drawing.Size(168, 234);
            this.keyproxytm.TabIndex = 1;
            // 
            // accfb
            // 
            this.accfb.Location = new System.Drawing.Point(251, 61);
            this.accfb.Multiline = true;
            this.accfb.Name = "accfb";
            this.accfb.Size = new System.Drawing.Size(206, 234);
            this.accfb.TabIndex = 2;
            // 
            // otpsim
            // 
            this.otpsim.Location = new System.Drawing.Point(33, 354);
            this.otpsim.Name = "otpsim";
            this.otpsim.Size = new System.Drawing.Size(235, 20);
            this.otpsim.TabIndex = 3;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(519, 61);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(218, 234);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // loaisim
            // 
            this.loaisim.Location = new System.Drawing.Point(312, 354);
            this.loaisim.Name = "loaisim";
            this.loaisim.Size = new System.Drawing.Size(100, 20);
            this.loaisim.TabIndex = 5;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(456, 354);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(100, 20);
            this.password.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.password);
            this.Controls.Add(this.loaisim);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.otpsim);
            this.Controls.Add(this.accfb);
            this.Controls.Add(this.keyproxytm);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox keyproxytm;
        private System.Windows.Forms.TextBox accfb;
        private System.Windows.Forms.TextBox otpsim;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox loaisim;
        private System.Windows.Forms.TextBox password;
    }
}


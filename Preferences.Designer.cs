namespace CFVG_Card_Creator
{
    partial class Preferences
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Preferences));
            this.ImageBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.DataBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.label_Images = new System.Windows.Forms.Label();
            this.textbox_Images = new System.Windows.Forms.TextBox();
            this.button_Images = new System.Windows.Forms.Button();
            this.button_Data = new System.Windows.Forms.Button();
            this.textbox_Data = new System.Windows.Forms.TextBox();
            this.label_Data = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Reset = new System.Windows.Forms.Button();
            this.button_CardArt = new System.Windows.Forms.Button();
            this.textbox_CardArt = new System.Windows.Forms.TextBox();
            this.label_CardArt = new System.Windows.Forms.Label();
            this.checkbox_Respace = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label_Images
            // 
            this.label_Images.AutoSize = true;
            this.label_Images.Location = new System.Drawing.Point(7, 37);
            this.label_Images.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Images.Name = "label_Images";
            this.label_Images.Size = new System.Drawing.Size(71, 13);
            this.label_Images.TabIndex = 3;
            this.label_Images.Text = "Image Folder:";
            // 
            // textbox_Images
            // 
            this.textbox_Images.Enabled = false;
            this.textbox_Images.Location = new System.Drawing.Point(80, 35);
            this.textbox_Images.Margin = new System.Windows.Forms.Padding(2);
            this.textbox_Images.Name = "textbox_Images";
            this.textbox_Images.Size = new System.Drawing.Size(186, 20);
            this.textbox_Images.TabIndex = 4;
            // 
            // button_Images
            // 
            this.button_Images.Location = new System.Drawing.Point(268, 35);
            this.button_Images.Margin = new System.Windows.Forms.Padding(0);
            this.button_Images.Name = "button_Images";
            this.button_Images.Size = new System.Drawing.Size(19, 19);
            this.button_Images.TabIndex = 5;
            this.button_Images.Text = "...";
            this.button_Images.UseVisualStyleBackColor = true;
            this.button_Images.Click += new System.EventHandler(this.button_Images_Click);
            // 
            // button_Data
            // 
            this.button_Data.Location = new System.Drawing.Point(268, 58);
            this.button_Data.Margin = new System.Windows.Forms.Padding(0);
            this.button_Data.Name = "button_Data";
            this.button_Data.Size = new System.Drawing.Size(19, 19);
            this.button_Data.TabIndex = 8;
            this.button_Data.Text = "...";
            this.button_Data.UseVisualStyleBackColor = true;
            this.button_Data.Click += new System.EventHandler(this.button_Data_Click);
            // 
            // textbox_Data
            // 
            this.textbox_Data.Enabled = false;
            this.textbox_Data.Location = new System.Drawing.Point(80, 58);
            this.textbox_Data.Margin = new System.Windows.Forms.Padding(2);
            this.textbox_Data.Name = "textbox_Data";
            this.textbox_Data.Size = new System.Drawing.Size(186, 20);
            this.textbox_Data.TabIndex = 7;
            // 
            // label_Data
            // 
            this.label_Data.AutoSize = true;
            this.label_Data.Location = new System.Drawing.Point(14, 60);
            this.label_Data.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Data.Name = "label_Data";
            this.label_Data.Size = new System.Drawing.Size(65, 13);
            this.label_Data.TabIndex = 6;
            this.label_Data.Text = "Data Folder:";
            // 
            // button_OK
            // 
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(127, 105);
            this.button_OK.Margin = new System.Windows.Forms.Padding(2);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(56, 19);
            this.button_OK.TabIndex = 11;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(187, 105);
            this.button_Cancel.Margin = new System.Windows.Forms.Padding(2);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(56, 19);
            this.button_Cancel.TabIndex = 12;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Reset
            // 
            this.button_Reset.Location = new System.Drawing.Point(67, 105);
            this.button_Reset.Margin = new System.Windows.Forms.Padding(2);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(56, 19);
            this.button_Reset.TabIndex = 10;
            this.button_Reset.Text = "Reset";
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // button_CardArt
            // 
            this.button_CardArt.Location = new System.Drawing.Point(268, 11);
            this.button_CardArt.Margin = new System.Windows.Forms.Padding(0);
            this.button_CardArt.Name = "button_CardArt";
            this.button_CardArt.Size = new System.Drawing.Size(19, 19);
            this.button_CardArt.TabIndex = 2;
            this.button_CardArt.Text = "...";
            this.button_CardArt.UseVisualStyleBackColor = true;
            this.button_CardArt.Click += new System.EventHandler(this.button_CardArt_Click);
            // 
            // textbox_CardArt
            // 
            this.textbox_CardArt.Enabled = false;
            this.textbox_CardArt.Location = new System.Drawing.Point(80, 11);
            this.textbox_CardArt.Margin = new System.Windows.Forms.Padding(2);
            this.textbox_CardArt.Name = "textbox_CardArt";
            this.textbox_CardArt.Size = new System.Drawing.Size(186, 20);
            this.textbox_CardArt.TabIndex = 1;
            // 
            // label_CardArt
            // 
            this.label_CardArt.AutoSize = true;
            this.label_CardArt.Location = new System.Drawing.Point(28, 14);
            this.label_CardArt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CardArt.Name = "label_CardArt";
            this.label_CardArt.Size = new System.Drawing.Size(48, 13);
            this.label_CardArt.TabIndex = 0;
            this.label_CardArt.Text = "Card Art:";
            // 
            // checkbox_Respace
            // 
            this.checkbox_Respace.AutoSize = true;
            this.checkbox_Respace.Location = new System.Drawing.Point(80, 83);
            this.checkbox_Respace.Name = "checkbox_Respace";
            this.checkbox_Respace.Size = new System.Drawing.Size(162, 17);
            this.checkbox_Respace.TabIndex = 9;
            this.checkbox_Respace.Text = "Invert Effect Text Respacing";
            this.checkbox_Respace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkbox_Respace.UseVisualStyleBackColor = true;
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 129);
            this.Controls.Add(this.checkbox_Respace);
            this.Controls.Add(this.button_CardArt);
            this.Controls.Add(this.textbox_CardArt);
            this.Controls.Add(this.label_CardArt);
            this.Controls.Add(this.button_Reset);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.button_Data);
            this.Controls.Add(this.textbox_Data);
            this.Controls.Add(this.label_Data);
            this.Controls.Add(this.button_Images);
            this.Controls.Add(this.textbox_Images);
            this.Controls.Add(this.label_Images);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Preferences";
            this.Text = "Preferences";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog ImageBrowser;
        private System.Windows.Forms.FolderBrowserDialog DataBrowser;
        private System.Windows.Forms.Label label_Images;
        public System.Windows.Forms.TextBox textbox_Images;
        private System.Windows.Forms.Button button_Images;
        private System.Windows.Forms.Button button_Data;
        public System.Windows.Forms.TextBox textbox_Data;
        private System.Windows.Forms.Label label_Data;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Reset;
        private System.Windows.Forms.Button button_CardArt;
        public System.Windows.Forms.TextBox textbox_CardArt;
        private System.Windows.Forms.Label label_CardArt;
        public System.Windows.Forms.CheckBox checkbox_Respace;
    }
}
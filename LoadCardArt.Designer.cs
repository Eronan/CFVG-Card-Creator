namespace CFVG_Card_Creator
{
    partial class LoadCardArt
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
            this.group_Selection = new System.Windows.Forms.GroupBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.checkbox_Ratio = new System.Windows.Forms.CheckBox();
            this.label_Zoom = new System.Windows.Forms.Label();
            this.numeric_Height = new System.Windows.Forms.NumericUpDown();
            this.label_Height = new System.Windows.Forms.Label();
            this.numeric_Width = new System.Windows.Forms.NumericUpDown();
            this.label_Width = new System.Windows.Forms.Label();
            this.numeric_PosY = new System.Windows.Forms.NumericUpDown();
            this.numeric_PosX = new System.Windows.Forms.NumericUpDown();
            this.label_PosY = new System.Windows.Forms.Label();
            this.label_PosX = new System.Windows.Forms.Label();
            this.openImageFile = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox_View = new System.Windows.Forms.PictureBox();
            this.picBox_CardImage = new System.Windows.Forms.PictureBox();
            this.group_Selection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_Height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_PosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_PosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_CardImage)).BeginInit();
            this.SuspendLayout();
            // 
            // group_Selection
            // 
            this.group_Selection.Controls.Add(this.button_OK);
            this.group_Selection.Controls.Add(this.checkbox_Ratio);
            this.group_Selection.Controls.Add(this.label_Zoom);
            this.group_Selection.Controls.Add(this.numeric_Height);
            this.group_Selection.Controls.Add(this.label_Height);
            this.group_Selection.Controls.Add(this.numeric_Width);
            this.group_Selection.Controls.Add(this.label_Width);
            this.group_Selection.Controls.Add(this.numeric_PosY);
            this.group_Selection.Controls.Add(this.numeric_PosX);
            this.group_Selection.Controls.Add(this.label_PosY);
            this.group_Selection.Controls.Add(this.label_PosX);
            this.group_Selection.Location = new System.Drawing.Point(367, 12);
            this.group_Selection.Name = "group_Selection";
            this.group_Selection.Size = new System.Drawing.Size(467, 79);
            this.group_Selection.TabIndex = 0;
            this.group_Selection.TabStop = false;
            this.group_Selection.Text = "Selection";
            // 
            // button_OK
            // 
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(374, 45);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 10;
            this.button_OK.Text = "Finished";
            this.button_OK.UseVisualStyleBackColor = true;
            // 
            // checkbox_Ratio
            // 
            this.checkbox_Ratio.AutoSize = true;
            this.checkbox_Ratio.Checked = true;
            this.checkbox_Ratio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkbox_Ratio.Location = new System.Drawing.Point(240, 45);
            this.checkbox_Ratio.Name = "checkbox_Ratio";
            this.checkbox_Ratio.Size = new System.Drawing.Size(79, 17);
            this.checkbox_Ratio.TabIndex = 9;
            this.checkbox_Ratio.Text = "Fixed Ratio";
            this.checkbox_Ratio.UseVisualStyleBackColor = true;
            this.checkbox_Ratio.CheckedChanged += new System.EventHandler(this.checkbox_Ratio_CheckedChanged);
            // 
            // label_Zoom
            // 
            this.label_Zoom.AutoSize = true;
            this.label_Zoom.Location = new System.Drawing.Point(162, 46);
            this.label_Zoom.Name = "label_Zoom";
            this.label_Zoom.Size = new System.Drawing.Size(66, 13);
            this.label_Zoom.TabIndex = 8;
            this.label_Zoom.Text = "Zoom: 100%";
            // 
            // numeric_Height
            // 
            this.numeric_Height.Location = new System.Drawing.Point(397, 19);
            this.numeric_Height.Maximum = new decimal(new int[] {
            441,
            0,
            0,
            0});
            this.numeric_Height.Name = "numeric_Height";
            this.numeric_Height.Size = new System.Drawing.Size(52, 20);
            this.numeric_Height.TabIndex = 7;
            this.numeric_Height.Value = new decimal(new int[] {
            441,
            0,
            0,
            0});
            this.numeric_Height.ValueChanged += new System.EventHandler(this.numeric_Height_ValueChanged);
            // 
            // label_Height
            // 
            this.label_Height.AutoSize = true;
            this.label_Height.Location = new System.Drawing.Point(350, 21);
            this.label_Height.Name = "label_Height";
            this.label_Height.Size = new System.Drawing.Size(41, 13);
            this.label_Height.TabIndex = 6;
            this.label_Height.Text = "Height:";
            // 
            // numeric_Width
            // 
            this.numeric_Width.Location = new System.Drawing.Point(292, 19);
            this.numeric_Width.Maximum = new decimal(new int[] {
            349,
            0,
            0,
            0});
            this.numeric_Width.Name = "numeric_Width";
            this.numeric_Width.Size = new System.Drawing.Size(52, 20);
            this.numeric_Width.TabIndex = 5;
            this.numeric_Width.Value = new decimal(new int[] {
            349,
            0,
            0,
            0});
            this.numeric_Width.ValueChanged += new System.EventHandler(this.numeric_Width_ValueChanged);
            // 
            // label_Width
            // 
            this.label_Width.AutoSize = true;
            this.label_Width.Location = new System.Drawing.Point(248, 21);
            this.label_Width.Name = "label_Width";
            this.label_Width.Size = new System.Drawing.Size(38, 13);
            this.label_Width.TabIndex = 4;
            this.label_Width.Text = "Width:";
            // 
            // numeric_PosY
            // 
            this.numeric_PosY.Location = new System.Drawing.Point(190, 19);
            this.numeric_PosY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numeric_PosY.Name = "numeric_PosY";
            this.numeric_PosY.Size = new System.Drawing.Size(52, 20);
            this.numeric_PosY.TabIndex = 3;
            this.numeric_PosY.ValueChanged += new System.EventHandler(this.numeric_Pos_ValueChanged);
            // 
            // numeric_PosX
            // 
            this.numeric_PosX.Location = new System.Drawing.Point(69, 19);
            this.numeric_PosX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numeric_PosX.Name = "numeric_PosX";
            this.numeric_PosX.Size = new System.Drawing.Size(52, 20);
            this.numeric_PosX.TabIndex = 2;
            this.numeric_PosX.ValueChanged += new System.EventHandler(this.numeric_Pos_ValueChanged);
            // 
            // label_PosY
            // 
            this.label_PosY.AutoSize = true;
            this.label_PosY.Location = new System.Drawing.Point(127, 21);
            this.label_PosY.Name = "label_PosY";
            this.label_PosY.Size = new System.Drawing.Size(57, 13);
            this.label_PosY.TabIndex = 1;
            this.label_PosY.Text = "Position Y:";
            // 
            // label_PosX
            // 
            this.label_PosX.AutoSize = true;
            this.label_PosX.Location = new System.Drawing.Point(6, 21);
            this.label_PosX.Name = "label_PosX";
            this.label_PosX.Size = new System.Drawing.Size(57, 13);
            this.label_PosX.TabIndex = 0;
            this.label_PosX.Text = "Position X:";
            // 
            // openImageFile
            // 
            this.openImageFile.FileName = "openImageFile";
            this.openImageFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.gif, *.tiff, *.bmp) | *.jpg; " +
    "*.jpeg; *.jpe; *.jfif; *.png; *.gif; *.tiff; *.bmp";
            // 
            // pictureBox_View
            // 
            this.pictureBox_View.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox_View.Location = new System.Drawing.Point(367, 97);
            this.pictureBox_View.Name = "pictureBox_View";
            this.pictureBox_View.Size = new System.Drawing.Size(467, 355);
            this.pictureBox_View.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_View.TabIndex = 2;
            this.pictureBox_View.TabStop = false;
            this.pictureBox_View.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_View_MouseDown);
            this.pictureBox_View.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_View_MouseMove);
            this.pictureBox_View.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_View_MouseUp);
            // 
            // picBox_CardImage
            // 
            this.picBox_CardImage.BackColor = System.Drawing.Color.Turquoise;
            this.picBox_CardImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBox_CardImage.Location = new System.Drawing.Point(12, 12);
            this.picBox_CardImage.Name = "picBox_CardImage";
            this.picBox_CardImage.Size = new System.Drawing.Size(349, 441);
            this.picBox_CardImage.TabIndex = 1;
            this.picBox_CardImage.TabStop = false;
            // 
            // LoadCardArt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 465);
            this.Controls.Add(this.pictureBox_View);
            this.Controls.Add(this.picBox_CardImage);
            this.Controls.Add(this.group_Selection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LoadCardArt";
            this.Text = "LoadCardArt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadCardArt_FormClosing);
            this.Load += new System.EventHandler(this.LoadCardArt_Load);
            this.group_Selection.ResumeLayout(false);
            this.group_Selection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_Height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_PosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_PosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_CardImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox group_Selection;
        private System.Windows.Forms.PictureBox picBox_CardImage;
        private System.Windows.Forms.PictureBox pictureBox_View;
        private System.Windows.Forms.Label label_PosX;
        private System.Windows.Forms.NumericUpDown numeric_PosY;
        private System.Windows.Forms.NumericUpDown numeric_PosX;
        private System.Windows.Forms.Label label_PosY;
        private System.Windows.Forms.NumericUpDown numeric_Width;
        private System.Windows.Forms.Label label_Width;
        private System.Windows.Forms.NumericUpDown numeric_Height;
        private System.Windows.Forms.Label label_Height;
        private System.Windows.Forms.CheckBox checkbox_Ratio;
        private System.Windows.Forms.Label label_Zoom;
        private System.Windows.Forms.OpenFileDialog openImageFile;
        private System.Windows.Forms.Button button_OK;
    }
}
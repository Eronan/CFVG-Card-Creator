using System;
using System.Drawing;
using System.Windows.Forms;


namespace CFVG_Card_Creator
{
    public partial class LoadCardArt : Form
    {
        //Bitmaps
        Bitmap originBitmap;
        Bitmap viewBitmap;
        public Bitmap saveBitmap = new Bitmap(349, 441);

        //Resolution of the CardArt
        public static double NormResolution = 349.0 / 441.0;

        //MouseEvents
        bool actOnEvent = true;
        bool mouseDrag = false;
        Point startPoint = new Point(0, 0);

        public LoadCardArt(string initialPath)
        {
            InitializeComponent();
            //Make the Bitmap Transparent
            saveBitmap.MakeTransparent();

            openImageFile.InitialDirectory = initialPath;

            //Open Image on Load
            if (openImageFile.ShowDialog(this) != DialogResult.Cancel)
            {
                //Load up File from Path
                originBitmap = new Bitmap(openImageFile.FileName);
                viewBitmap = (Bitmap)originBitmap.Clone();
                pictureBox_View.Image = viewBitmap;

                //Check which is smaller width or Height.
                if (originBitmap.Height * NormResolution > originBitmap.Width)
                {
                    //If width is smaller the maximum is based on width
                    numeric_Width.Maximum = originBitmap.Width;
                    numeric_Height.Maximum = (int)(originBitmap.Width / NormResolution);
                    numeric_Width.Value = originBitmap.Width;
                }
                else
                {
                    //If width is height the maximum is based on height
                    numeric_Height.Maximum = originBitmap.Height;
                    numeric_Width.Maximum = (int)(originBitmap.Height * NormResolution);
                    numeric_Height.Value = originBitmap.Height;
                }
            }
            else
            {
                //Close Form if no Image is loaded
                saveBitmap.Dispose();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        public LoadCardArt(Bitmap bmp)
        {
            InitializeComponent();
            //Make Bitmap Transparent
            saveBitmap.MakeTransparent();

            originBitmap = bmp;
            viewBitmap = (Bitmap)originBitmap.Clone();
            pictureBox_View.Image = viewBitmap;

            //Check which is smaller width or Height.
            if (originBitmap.Width / 441 < originBitmap.Height / 349)
            {
                //If width is smaller the maximum is based on width
                numeric_Width.Maximum = originBitmap.Width;
                numeric_Height.Maximum = (int)(originBitmap.Width / NormResolution);
                numeric_Width.Value = originBitmap.Width;
            }
            else
            {
                //If width is height the maximum is based on height
                numeric_Height.Maximum = originBitmap.Height;
                numeric_Width.Maximum = (int)(originBitmap.Height * NormResolution);
                numeric_Height.Value = originBitmap.Height;
            }
        }

        private void LoadCardArt_Load(object sender, EventArgs e)
        {
            
        }

        private void UpdatePicture()
        {
            //Get the Destination Rectangle
            RectangleF destinationRect = new RectangleF(0, 0, 349, 441);

            //Get the Rectangle from the Numerics
            RectangleF sourceRect = new RectangleF((float)numeric_PosX.Value, (float)numeric_PosY.Value, (float)numeric_Width.Value, (float)numeric_Height.Value);

            saveBitmap.Dispose();
            saveBitmap = new Bitmap(349, 441);

            //Load Graphics of Bitmap
            using (Graphics g = Graphics.FromImage(saveBitmap))
            {
                //Draw the Part of the Image onto the saveBitmap
                g.DrawImage(originBitmap, destinationRect,
                    sourceRect, 
                    GraphicsUnit.Pixel);
                g.Dispose();
            }

            //SaveBitmap into PictureBox
            picBox_CardImage.Image = saveBitmap;

            //ViewBitmap is a new instance of OriginBitmap
            viewBitmap = (Bitmap) originBitmap.Clone();

            //Load Graphics of Bitmap
            using (Graphics g = Graphics.FromImage(viewBitmap))
            {
                g.DrawRectangle(new Pen(Color.Red, 10), Rectangle.Round(sourceRect));
                g.Dispose();
            }

            //viewBitmap into PictureBox
            pictureBox_View.Image = viewBitmap;
        }

        private void numeric_Width_ValueChanged(object sender, EventArgs e)
        {
            if (actOnEvent)
            {
                //Make sure other Numerics don't fire
                actOnEvent = false;
                //Is Ratio Checked
                if (checkbox_Ratio.Checked)
                {
                    //Alter Height numeric
                    numeric_Height.Value = (int)((double)numeric_Width.Value / NormResolution);
                    //Zoom Ratio
                    label_Zoom.Text = "Zoom: " + (int)((331.00m / numeric_Width.Value) * 100);
                }
                else
                {
                    //Zoom X and Zoom Y
                    label_Zoom.Text = "Zoom X: " + (int)((331.00m / numeric_Width.Value) * 100) + Environment.NewLine
                        + "Zoom Y: " + (int)((441.0m / numeric_Height.Value) * 100);
                }
                UpdatePicture();
            }
            else actOnEvent = true;
            //Alter Maximums
            numeric_PosX.Maximum = originBitmap.Width - numeric_Width.Value;
        }

        private void numeric_Height_ValueChanged(object sender, EventArgs e)
        {
            if (actOnEvent)
            {
                //Make sure other Numerics don't fire
                actOnEvent = false;
                if (checkbox_Ratio.Checked)
                {
                    //Alter Width numeric
                    numeric_Width.Value = (int)((double)numeric_Height.Value * NormResolution);
                    //Zoom Ratio
                    label_Zoom.Text = "Zoom: " + (int)((441.0m / numeric_Height.Value) * 100);
                }
                else
                {
                    //Zoom X and Zoom Y
                    label_Zoom.Text = "Zoom X: " + (int)((331.00m / numeric_Width.Value) * 100) + Environment.NewLine
                        + "Zoom Y: " + (int)((441.0m / numeric_Height.Value) * 100);
                }
                UpdatePicture();
            }
            else actOnEvent = true;
            //Alter Maximums
            numeric_PosY.Maximum = originBitmap.Height - numeric_Height.Value;
        }

        private void numeric_Pos_ValueChanged(object sender, EventArgs e)
        {
            //Update the Picture only if MouseDrag is not on
            if (!mouseDrag) UpdatePicture();
        }

        private void pictureBox_View_MouseDown(object sender, MouseEventArgs e)
        {
            //Enable MouseDrag
            mouseDrag = true;
            Cursor.Current = Cursors.NoMove2D;
        }

        private void pictureBox_View_MouseUp(object sender, MouseEventArgs e)
        {
            //Disable MouseDrag
            mouseDrag = false;
            Cursor.Current = Cursors.Default;
            UpdatePicture();
        }

        private void pictureBox_View_MouseMove(object sender, MouseEventArgs e)
        {
            //Only work if mouseDrag is true
            if (mouseDrag)
            {
                //Determine Mouse Movement
                int mouseX = e.Location.X - startPoint.X;
                int mouseY = e.Location.Y - startPoint.Y;

                //Positive means move to the right
                //Negative means move to the left
                if (mouseX > 0)
                {
                    //Change Numeric Values
                    if (mouseX > numeric_PosX.Maximum - numeric_PosX.Value) numeric_PosX.Value = numeric_PosX.Maximum;
                    else numeric_PosX.Value += mouseX;
                }
                else
                {
                    //Change Numeric Values
                    if ((-1) * mouseX > numeric_PosX.Value) numeric_PosX.Value = 0;
                    else numeric_PosX.Value += mouseX;
                }

                //Positive means move down
                //Negative means move up
                if (mouseY > 0)
                {
                    //Change Numeric Values
                    if (mouseY > numeric_PosY.Maximum - numeric_PosY.Value) numeric_PosY.Value = numeric_PosY.Maximum;
                    else numeric_PosY.Value += mouseY;
                }
                else
                {
                    //Change Numeric Values
                    if ((-1) * mouseY > numeric_PosY.Value) numeric_PosY.Value = 0;
                    else numeric_PosY.Value += mouseY;
                }
            }
            startPoint = e.Location;
        }

        private void checkbox_Ratio_CheckedChanged(object sender, EventArgs e)
        {
            //Does user want Ratio?
            if (checkbox_Ratio.Checked)
            {
                //Maximums based on smaller value
                if (originBitmap.Width < originBitmap.Height)
                {
                    numeric_Width.Maximum = originBitmap.Width;
                    numeric_Height.Maximum = (int)(originBitmap.Width / NormResolution);
                }
                else
                {
                    numeric_Height.Maximum = originBitmap.Height;
                    numeric_Width.Maximum = (int)(originBitmap.Height * NormResolution);
                }
            }
            else
            {
                //Zoom X and Zoom y
                label_Zoom.Text = "Zoom X: " + (int)((331.00m / numeric_Width.Value) * 100) + Environment.NewLine
                            + "Zoom Y: " + (int)((441.0m / numeric_Height.Value) * 100);

                //Reset Maximums
                numeric_Width.Maximum = originBitmap.Width;
                numeric_Height.Maximum = originBitmap.Height;
            }
        }

        private void LoadCardArt_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Dispose of Bitmaps
            if (originBitmap != null) originBitmap.Dispose();
            else if (viewBitmap != null) viewBitmap.Dispose();
        }
    }
}

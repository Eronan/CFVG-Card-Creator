using System;
using System.Drawing;
using System.Windows.Forms;


namespace CFVG_Card_Creator
{
    public partial class LoadCardArt : Form
    {
        //Bitmaps
        Bitmap originBitmap = null;
        Bitmap viewBitmap;
        public Bitmap saveBitmap = new Bitmap(349, 441);
        UserRect rect;

        //Resolution of the CardArt
        public static double NormResolution = 349.0 / 441.0;
        decimal resizeValue = 0;

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
            if (originBitmap == null)
            {
                //Open Image on Load
                if (openImageFile.ShowDialog(this) == DialogResult.OK)
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
                    this.Close();
                }
            }
            if (originBitmap.Width / 467 > originBitmap.Height / 355)
            {
                resizeValue = (decimal)pictureBox_View.Width / originBitmap.Width;
                int initialHeight = pictureBox_View.Height;
                pictureBox_View.Size = new Size(pictureBox_View.Width, (int)(originBitmap.Height * resizeValue));
                int heightDifference = pictureBox_View.Size.Height - initialHeight;
                int moveToY = pictureBox_View.Location.Y - (int)(heightDifference / 2 * 1f);
                pictureBox_View.Location = new Point(pictureBox_View.Location.X, moveToY);
            }
            else
            {
                resizeValue = (decimal)pictureBox_View.Height / originBitmap.Height;
                int initialWidth = pictureBox_View.Width;
                pictureBox_View.Size = new Size((int)(originBitmap.Width * resizeValue), pictureBox_View.Height);
                int widthDifference = pictureBox_View.Size.Width - initialWidth;
                int moveToX = pictureBox_View.Location.X - (int)(widthDifference / 2 * 1f);
                pictureBox_View.Location = new Point(moveToX, pictureBox_View.Location.Y);
            }

            /*Size sizestep1 = Size.Subtract(new Size(PictureBox1.Image.Size.Width / 2, PictureBox1.Image.Size.Height / 2), PictureBox1.Size);
            Size finalsize = Size.Add(sizestep1, PictureBox1.Image.Size);*/

            rect = new UserRect(new Rectangle((int)numeric_PosX.Value, (int)numeric_PosY.Value, (int)(numeric_Width.Value * resizeValue), (int)(numeric_Height.Value * resizeValue)));
            rect.SetPictureBox(pictureBox_View, resizeValue);
            rect.SetMaximums(numeric_Width.Minimum, numeric_Width.Maximum, numeric_Height.Minimum, numeric_Height.Maximum);
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
            /*
            //Load Graphics of Bitmap
            using (Graphics g = Graphics.FromImage(viewBitmap))
            {
                g.DrawRectangle(new Pen(Color.Red, 10), Rectangle.Round(sourceRect));
                g.Dispose();
            }*/

            //viewBitmap into PictureBox
            pictureBox_View.Image = viewBitmap;
        }

        private void numeric_Width_ValueChanged(object sender, EventArgs e)
        {
            if (actOnEvent)
            {
                //Is Ratio Checked
                if (checkbox_Ratio.Checked)
                {
                    //Make sure other Numerics don't fire
                    actOnEvent = false;

                    //Alter Height numeric
                    numeric_Height.Value = (int)((double)numeric_Width.Value / NormResolution);
                    //Zoom Ratio
                    label_Zoom.Text = "Zoom: " + (int)((349.00m / numeric_Width.Value) * 100);
                }
                else
                {
                    //Zoom X and Zoom Y
                    label_Zoom.Text = "Zoom X: " + (int)((349.00m / numeric_Width.Value) * 100) + Environment.NewLine
                        + "Zoom Y: " + (int)((441.0m / numeric_Height.Value) * 100);

                    if (rect != null) rect.SetMaximums(numeric_Width.Minimum, numeric_Width.Maximum, numeric_Height.Minimum, numeric_Height.Maximum);
                }

                UpdatePicture();
            }
            else
            {
                actOnEvent = true;

                if (!mouseDrag && rect != null)
                {
                    rect.rect = new Rectangle((int)numeric_PosX.Value, (int)numeric_PosY.Value, (int)(numeric_Width.Value * resizeValue), (int)(numeric_Height.Value * resizeValue));

                    rect.SetMaximums(numeric_Width.Minimum, numeric_Width.Maximum, numeric_Height.Minimum, numeric_Height.Maximum);
                    pictureBox_View.Invalidate();
                }
            }
            //Alter Maximums
            numeric_PosX.Maximum = originBitmap.Width - numeric_Width.Value;
        }

        private void numeric_Height_ValueChanged(object sender, EventArgs e)
        {
            if (actOnEvent)
            {
                if (checkbox_Ratio.Checked)
                {
                    //Make sure other Numerics don't fire
                    actOnEvent = false;
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

                    if (rect != null) rect.SetMaximums(numeric_Width.Minimum, numeric_Width.Maximum, numeric_Height.Minimum, numeric_Height.Maximum);
                }

                UpdatePicture();
            }
            else
            {
                actOnEvent = true;

                if (!mouseDrag && rect != null)
                {
                    rect.rect = new Rectangle((int)numeric_PosX.Value, (int)numeric_PosY.Value, (int)(numeric_Width.Value * resizeValue), (int)(numeric_Height.Value * resizeValue));

                    rect.SetMaximums(numeric_Width.Minimum, numeric_Width.Maximum, numeric_Height.Minimum, numeric_Height.Maximum);
                    pictureBox_View.Invalidate();
                }
            }
            //Alter Maximums
            numeric_PosY.Maximum = originBitmap.Height - numeric_Height.Value;
        }

        private void numeric_Pos_ValueChanged(object sender, EventArgs e)
        {
            if (!mouseDrag && rect != null)
            {
                rect.rect = new Rectangle((int) numeric_PosX.Value, (int) numeric_PosY.Value, (int) (numeric_Width.Value * resizeValue), (int)(numeric_Height.Value * resizeValue));

                pictureBox_View.Invalidate();
            }
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
            rect.mPictureBox_MouseMove(sender, e);

            if (mouseDrag)
            {
                decimal getWidth = rect.currentWidth / resizeValue;
                decimal getHeight = rect.currentHeight / resizeValue;
                numeric_Width.Value = getHeight > numeric_Width.Maximum ? numeric_Width.Maximum : getWidth;
                numeric_Height.Value = getHeight < numeric_Height.Maximum ? numeric_Height.Maximum : getHeight;

                numeric_PosX.Maximum = originBitmap.Width - numeric_Width.Value;
                numeric_PosY.Maximum = originBitmap.Height - numeric_Height.Value;

                decimal getPosX = rect.currentX / resizeValue;
                decimal getPosY = rect.currentY / resizeValue;
                numeric_PosX.Value = getPosX > numeric_PosX.Maximum ? numeric_PosX.Maximum : getPosX;
                numeric_PosY.Value = getPosY > numeric_PosY.Maximum ? numeric_PosY.Maximum : getPosY;

                //UpdatePicture();
            }
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

            if (rect != null)
            {
                rect.SetMaximums(numeric_Width.Minimum, numeric_Width.Maximum, numeric_Height.Minimum, numeric_Height.Maximum);
                rect.ratio = checkbox_Ratio.Checked;
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

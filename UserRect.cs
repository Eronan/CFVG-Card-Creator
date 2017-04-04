using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CFVG_Card_Creator
{
    public class UserRect
    {
        //Code from zebulon75018: Download from https://www.codeproject.com/kb/graphics/rectangleresizable.aspx

        private PictureBox mPictureBox;
        public Rectangle rect;
        public bool allowDeformingDuringMovement = false;
        private bool mIsClick = false;
        private bool mMove = false;
        private int oldX;
        private int oldY;
        private int sizeNodeRect = 5;
        private Bitmap mBmp = null;
        private PosSizableRect nodeSelected = PosSizableRect.None;
        private int angle = 30;

        //Original
        private bool widthChanged = false;
        private bool heightChanged = false;

        private decimal resize = 1;

        public int minimumWidth = 0;
        public int maximumWidth = 0;
        public int minimumHeight = 0;
        public int maximumHeight = 0;
        public bool ratio = true;

        private enum PosSizableRect
        {
            UpMiddle,
            LeftMiddle,
            LeftBottom,
            LeftUp,
            RightUp,
            RightMiddle,
            RightBottom,
            BottomMiddle,
            None
        };

        public UserRect(Rectangle r)
        {
            rect = r;
            mIsClick = false;
        }

        public void Draw(Graphics g)
        {
            g.DrawRectangle(new Pen(Color.Red), rect);

            foreach (PosSizableRect pos in Enum.GetValues(typeof(PosSizableRect)))
            {
                g.DrawRectangle(new Pen(Color.Red), GetRect(pos));
            }
        }

        public void SetBitmapFile(string filename)
        {
            this.mBmp = new Bitmap(filename);
        }

        public void SetBitmap(Bitmap bmp)
        {
            this.mBmp = bmp;
        }

        public void SetPictureBox(PictureBox p, decimal resizeValue)
        {
            this.mPictureBox = p;
            mPictureBox.MouseDown += new MouseEventHandler(mPictureBox_MouseDown);
            mPictureBox.MouseUp += new MouseEventHandler(mPictureBox_MouseUp);
            //mPictureBox.MouseMove += new MouseEventHandler(mPictureBox_MouseMove);
            mPictureBox.Paint += new PaintEventHandler(mPictureBox_Paint);

            resize = resizeValue;
        }

        public void mPictureBox_Paint(object sender, PaintEventArgs e)
        {

            try
            {
                Draw(e.Graphics);
            }
            catch (Exception exp)
            {
                System.Console.WriteLine(exp.Message);
            }

        }

        private void mPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            mIsClick = true;

            nodeSelected = PosSizableRect.None;
            nodeSelected = GetNodeSelectable(e.Location);

            if (rect.Contains(new Point(e.X, e.Y)))
            {
                mMove = true;
            }
            oldX = e.X;
            oldY = e.Y;
        }

        private void mPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            mIsClick = false;
            mMove = false;
        }

        public void mPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            ChangeCursor(e.Location);
            if (mIsClick == false)
            {
                return;
            }

            Rectangle backupRect = rect;

            switch (nodeSelected)
            {
                case PosSizableRect.LeftUp:
                    //Position X Positive
                    rect.X += e.X - oldX;
                    //Width Negative
                    rect.Width -= e.X - oldX;
                    //Position Y
                    rect.Y += e.Y - oldY;
                    //Height Negative
                    rect.Height -= e.Y - oldY;

                    widthChanged = true;
                    heightChanged = true;
                    //Finish
                    break;
                case PosSizableRect.LeftMiddle:
                    if (ratio) break;
                    //Position X Positive
                    rect.X += e.X - oldX;
                    //Width Negative
                    rect.Width -= e.X - oldX;

                    widthChanged = true;
                    //Finish
                    break;
                case PosSizableRect.LeftBottom:
                    //Width Negative
                    rect.Width -= e.X - oldX;
                    //Position X
                    rect.X += e.X - oldX;
                    //Height Positive
                    rect.Height += e.Y - oldY;

                    widthChanged = true;
                    heightChanged = true;
                    //Finish
                    break;
                case PosSizableRect.BottomMiddle:
                    if (ratio) break;
                    //Height Positive
                    rect.Height += e.Y - oldY;

                    heightChanged = true;
                    //Finish
                    break;
                case PosSizableRect.RightUp:
                    //Width Positive
                    rect.Width += e.X - oldX;
                    //Position Y
                    rect.Y += e.Y - oldY;
                    //Height Negative
                    rect.Height -= e.Y - oldY;

                    widthChanged = true;
                    heightChanged = true;
                    //Finish
                    break;
                case PosSizableRect.RightBottom:
                    //Width Positive
                    rect.Width += e.X - oldX;
                    //Height Positive
                    rect.Height += e.Y - oldY;

                    widthChanged = true;
                    heightChanged = true;
                    //Finish
                    break;
                case PosSizableRect.RightMiddle:
                    if (ratio) break;
                    //Width Positive
                    rect.Width += e.X - oldX;

                    widthChanged = true;
                    //Finish
                    break;
                case PosSizableRect.UpMiddle:
                    if (ratio) break;
                    //Position Y Positive
                    rect.Y += e.Y - oldY;
                    //Height Negative
                    rect.Height -= e.Y - oldY;

                    heightChanged = true;
                    //Finish
                    break;

                default:
                    if (mMove)
                    {
                        //Position X Positive
                        rect.X += e.X - oldX;
                        //Position Y Positive
                        rect.Y += e.Y - oldY;
                    }
                    //Finish
                    break;
            }
            oldX = e.X;
            oldY = e.Y;

            if (rect.Width < 5 || rect.Height < 5)
            {
                rect = backupRect;
            }

            TestIfRectInsideArea();

            mPictureBox.Invalidate();
        }

        private void TestIfRectInsideArea()
        {
            // Test if rectangle is allowed
            //X Position
            if (rect.X < 0) rect.X = 0;
            //Y Position
            if (rect.Y < 0) rect.Y = 0;
            //Width
            if (rect.Width < minimumWidth) rect.Width = minimumWidth;
            else if (rect.Width > maximumWidth) rect.Width = maximumWidth;
            //Height
            if (rect.Height < minimumHeight) rect.Height = minimumHeight;
            else if (rect.Height > maximumHeight) rect.Height = maximumHeight;

            if (ratio)
            {
                if (widthChanged) rect.Height = (int)(rect.Width / LoadCardArt.NormResolution);
                else if (heightChanged) rect.Width = (int)(rect.Height * LoadCardArt.NormResolution);

                widthChanged = false;
                heightChanged = false;
            }

            //Rect Maximum
            if (rect.X + rect.Width > mPictureBox.Width)
            {
                rect.Width = mPictureBox.Width - rect.X - 1; // -1 to be still show 
                if (allowDeformingDuringMovement == false)
                {
                    mIsClick = false;
                }
            }
            if (rect.Y + rect.Height > mPictureBox.Height)
            {
                rect.Height = mPictureBox.Height - rect.Y - 1;// -1 to be still show 
                if (allowDeformingDuringMovement == false)
                {
                    mIsClick = false;
                }
            }
        }

        public void SetMaximums(decimal minWidth, decimal maxWidth, decimal minHeight, decimal maxHeight)
        {
            minimumWidth = (int)(minWidth * resize);
            maximumWidth = (int)(maxWidth * resize);
            minimumHeight = (int)(minHeight * resize);
            maximumHeight = (int)(maxHeight * resize);
        }

        private Rectangle CreateRectSizableNode(int x, int y)
        {
            return new Rectangle(x - sizeNodeRect / 2, y - sizeNodeRect / 2, sizeNodeRect, sizeNodeRect);
        }

        private Rectangle GetRect(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.LeftUp:
                    return CreateRectSizableNode(rect.X, rect.Y);

                case PosSizableRect.LeftMiddle:
                    return CreateRectSizableNode(rect.X, rect.Y + +rect.Height / 2);

                case PosSizableRect.LeftBottom:
                    return CreateRectSizableNode(rect.X, rect.Y + rect.Height);

                case PosSizableRect.BottomMiddle:
                    return CreateRectSizableNode(rect.X + rect.Width / 2, rect.Y + rect.Height);

                case PosSizableRect.RightUp:
                    return CreateRectSizableNode(rect.X + rect.Width, rect.Y);

                case PosSizableRect.RightBottom:
                    return CreateRectSizableNode(rect.X + rect.Width, rect.Y + rect.Height);

                case PosSizableRect.RightMiddle:
                    return CreateRectSizableNode(rect.X + rect.Width, rect.Y + rect.Height / 2);

                case PosSizableRect.UpMiddle:
                    return CreateRectSizableNode(rect.X + rect.Width / 2, rect.Y);
                default:
                    return new Rectangle();
            }
        }

        public RectangleF getBounds()
        {
            return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }

        private PosSizableRect GetNodeSelectable(Point p)
        {
            foreach (PosSizableRect r in Enum.GetValues(typeof(PosSizableRect)))
            {
                if (GetRect(r).Contains(p))
                {
                    return r;
                }
            }
            return PosSizableRect.None;
        }

        private void ChangeCursor(Point p)
        {
            mPictureBox.Cursor = GetCursor(GetNodeSelectable(p));
        }

        public int currentX
        {
            get { return rect.X; }
        }

        public int currentY
        {
            get { return rect.Y; }
        }

        public int currentWidth
        {
            get { return rect.Width; }
        }

        public int currentHeight
        {
            get { return rect.Height; }
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Cursor GetCursor(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.LeftUp:
                    return Cursors.SizeNWSE;

                case PosSizableRect.LeftMiddle:
                    return Cursors.SizeWE;

                case PosSizableRect.LeftBottom:
                    return Cursors.SizeNESW;

                case PosSizableRect.BottomMiddle:
                    return Cursors.SizeNS;

                case PosSizableRect.RightUp:
                    return Cursors.SizeNESW;

                case PosSizableRect.RightBottom:
                    return Cursors.SizeNWSE;

                case PosSizableRect.RightMiddle:
                    return Cursors.SizeWE;

                case PosSizableRect.UpMiddle:
                    return Cursors.SizeNS;
                default:
                    return Cursors.Default;
            }
        }

    }
}

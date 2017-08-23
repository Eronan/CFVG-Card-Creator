using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFVG_Card_Creator
{
    //Code from zebulon75018: Download from https://www.codeproject.com/kb/graphics/rectangleresizable.aspx

    class ResizeRect
    {
        //Rectangle Values
        int _maxWidth = 0;
        int _maxHeight = 0;
        Rectangle _rect;
        bool _useRatio = true;
        public decimal resize = 0;
        public bool Validated = true;

        //Private Values
        private int sizeNodeRect = 5;
        private PosSizeableRect nodeSelected = PosSizeableRect.None;
        private bool mIsClick = false;
        private bool mMove = false;
        private int oldX;
        private int oldY;

        //Reference Values
        Size _bmp;

        private enum PosSizeableRect
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

        public ResizeRect(Bitmap loadBmp, PictureBox picBox, decimal resize = 1)
        {
            //Set Bitmap
            _bmp = loadBmp.Size;
            //Set Maximums
            _rect = new Rectangle(new Point(0,0), loadBmp.Size);
            changeRatio(true);
            this.resize = resize;

            //PictureBox
            picBox.MouseDown += new MouseEventHandler(mPictureBox_MouseDown);
            picBox.MouseUp += new MouseEventHandler(mPictureBox_MouseUp);
            picBox.MouseMove += new MouseEventHandler(mPictureBox_MouseMove);
        }

        //Code from zebulon
        private void mPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            //Determine Values for Movement
            mIsClick = true;

            nodeSelected = PosSizeableRect.None;
            nodeSelected = GetNodeSelectable(e.Location);

            if (_rect.Contains(new Point(e.X, e.Y)))
            {
                mMove = true;
            }
            oldX = e.X;
            oldY = e.Y;
        }

        //Code from zebulon
        private void mPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            //Reset Values
            mIsClick = false;
            mMove = false;
        }

        private void mPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Validated = false;

            //Get PictureBox
            PictureBox mPictureBox = sender as PictureBox;
            if (mPictureBox == null) return;

            mPictureBox.Cursor = GetCursor(GetNodeSelectable(e.Location));

            //Return if not Clicked
            if (mIsClick == false) return;

            //Values
            Rectangle backupRect = _rect;
            int moveX = (e.X - oldX);//Multiply by Resize (Maybe)
            int moveY = (e.Y - oldY);//Multiply by Resize (Maybe)

            //Set NodeSelected
            switch (nodeSelected)
            {
                case PosSizeableRect.LeftUp:
                    //Width Negative
                    //Height Negative
                    _rect.X += moveX;
                    _rect.Width -= moveY;
                    _rect.Y += moveY;
                    _rect.Height -= moveY;
                    break;
                case PosSizeableRect.LeftMiddle:
                    if (_useRatio) break;
                    //Width Negative
                    _rect.X += moveX;
                    _rect.Width -= moveX;
                    break;
                case PosSizeableRect.LeftBottom:
                    //Width Negative
                    //Height Positive
                    _rect.Width -= moveX;
                    _rect.X += moveX;
                    _rect.Height += moveY;
                    break;
                case PosSizeableRect.BottomMiddle:
                    if (_useRatio) break;
                    //Height Positive
                    _rect.Height += moveY;
                    break;
                case PosSizeableRect.RightUp:
                    //Width Positive
                    //Height Negative
                    _rect.Width += moveX;
                    _rect.Y += moveY;
                    _rect.Height -= moveY;
                    break;
                case PosSizeableRect.RightBottom:
                    //Width Positive
                    //Height Positive
                    _rect.Width += moveX;
                    _rect.Height += moveY;
                    break;
                case PosSizeableRect.RightMiddle:
                    if (_useRatio) break;
                    //Width Positive
                    _rect.Width += moveX;
                    break;
                case PosSizeableRect.UpMiddle:
                    if (_useRatio) break;
                    //Height Negative
                    _rect.Y += moveY;
                    _rect.Height -= moveY;
                    break;
                default:
                    if (mMove)
                    {
                        _rect.X += moveX;
                        _rect.Y += moveY;
                    }
                    break;
            }

            //Set new Old Positions
            oldX = e.X;
            oldY = e.Y;

            //Get Check Minimums
            if (_rect.Width < 5 || _rect.Height < 5)
            {
                _rect = backupRect;
            }

            //Test Areas
            TestIfRectInsideArea();
        }

        private void TestIfRectInsideArea()
        {
            // Test if rectangle is allowed
            if (_rect.X < 0) _rect.X = 0;
            if (_rect.Y < 0) _rect.Y = 0;
            if (_rect.Width > _maxWidth) _rect.Width = _maxWidth;
            if (_rect.Height > _maxHeight) _rect.Height = _maxHeight;

            //Fix Ratio
            if (_useRatio)
            {
                //useRatio is True
                //Check Smaller Value
                switch (nodeSelected)
                {
                    case PosSizeableRect.UpMiddle:
                    case PosSizeableRect.BottomMiddle:
                        //Height-only changed
                        _rect.Width = (int)(_rect.Height * LoadCardArt.NormResolution);
                        break;
                    default:
                        //Width or Height changed
                        _rect.Height = (int)(_rect.Width / LoadCardArt.NormResolution);
                        break;
                }
            }

            //Rect Maximum
            if (_rect.X + _rect.Width > _bmp.Width)
            {
                _rect.Width = _bmp.Width - _rect.X - 1; // -1 to be still show 
                mIsClick = false;
            }
            if (_rect.Y + _rect.Height > _bmp.Height)
            {
                _rect.Height = _bmp.Height - _rect.Y - 1;// -1 to be still show 
                mIsClick = false;
            }

            Validated = true;
        }

        //Code from zebulon
        private PosSizeableRect GetNodeSelectable(Point p)
        {
            foreach (PosSizeableRect r in Enum.GetValues(typeof(PosSizeableRect)))
            {
                if (nodePosition(r).Contains(p)) return r;
            }
            return PosSizeableRect.None;
        }

        //Code from zebulon
        private Rectangle CreateRectSizeableNode(int x, int y)
        {
            //Return Position
            return new Rectangle(x - sizeNodeRect / 2, y - sizeNodeRect / 2, sizeNodeRect, sizeNodeRect);
        }

        //Code from zebulon
        private Rectangle nodePosition(PosSizeableRect p)
        {
            //Get Node Positions
            switch (p)
            {
                case PosSizeableRect.LeftUp:
                    return CreateRectSizeableNode(_rect.X, _rect.Y);
                case PosSizeableRect.LeftMiddle:
                    return CreateRectSizeableNode(_rect.X, _rect.Y + +_rect.Height / 2);
                case PosSizeableRect.LeftBottom:
                    return CreateRectSizeableNode(_rect.X, _rect.Y + _rect.Height);
                case PosSizeableRect.BottomMiddle:
                    return CreateRectSizeableNode(_rect.X + _rect.Width / 2, _rect.Y + _rect.Height);
                case PosSizeableRect.RightUp:
                    return CreateRectSizeableNode(_rect.X + _rect.Width, _rect.Y);
                case PosSizeableRect.RightBottom:
                    return CreateRectSizeableNode(_rect.X + _rect.Width, _rect.Y + _rect.Height);
                case PosSizeableRect.RightMiddle:
                    return CreateRectSizeableNode(_rect.X + _rect.Width, _rect.Y + _rect.Height / 2);
                case PosSizeableRect.UpMiddle:
                    return CreateRectSizeableNode(_rect.X + _rect.Width / 2, _rect.Y);
                default:
                    return new Rectangle();
            }
        }

        //Code from zebulon
        private Cursor GetCursor(PosSizeableRect p)
        {
            switch (p)
            {
                case PosSizeableRect.LeftUp:
                    return Cursors.SizeNWSE;
                case PosSizeableRect.LeftMiddle:
                    return Cursors.SizeWE;
                case PosSizeableRect.LeftBottom:
                    return Cursors.SizeNESW;
                case PosSizeableRect.BottomMiddle:
                    return Cursors.SizeNS;
                case PosSizeableRect.RightUp:
                    return Cursors.SizeNESW;
                case PosSizeableRect.RightBottom:
                    return Cursors.SizeNWSE;
                case PosSizeableRect.RightMiddle:
                    return Cursors.SizeWE;
                case PosSizeableRect.UpMiddle:
                    return Cursors.SizeNS;
                default:
                    return Cursors.Default;
            }
        }

        //Code from zebulon
        public void DrawRect(Bitmap bmp)
        {
            //Draw Rectangle on Image
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawRectangle(Pens.Red, _rect);
                //Draw Smaller Nodes on Rectangle
                foreach (PosSizeableRect pos in Enum.GetValues(typeof(PosSizeableRect)))
                {
                    g.DrawRectangle(new Pen(Color.Red), nodePosition(pos));
                }
                g.Dispose();
            }
        }

        public void DrawRect(Graphics g)
        {
            //Draw Rectangle on Image
            g.DrawRectangle(new Pen(Color.Red), _rect);
            //Draw Smaller Nodes on Rectangle
            foreach (PosSizeableRect pos in Enum.GetValues(typeof(PosSizeableRect)))
            {
                g.DrawRectangle(new Pen(Color.Red), nodePosition(pos));
            }
            g.Dispose();
        }

        public void changeRatio(bool useRatio)
        {
            //Change useRatio and check useRatio is True
            this._useRatio = useRatio;
            if (this._useRatio)
            {
                //useRatio is True
                //Check Smaller Value
                if (_bmp.Width / 441 < _bmp.Height / 349)
                {
                    //Set Maximums
                    _maxWidth = _bmp.Width;
                    _maxHeight = (int)(_bmp.Width / LoadCardArt.NormResolution);
                }
                else
                {
                    //Set Maximums
                    _maxHeight = _bmp.Height;
                    _maxWidth = (int)(_bmp.Height / LoadCardArt.NormResolution);
                }
            }
            else
            {
                _maxHeight = _bmp.Height;
                _maxWidth = _bmp.Width;
            }
        }

        public Rectangle SizeableRectangle
        {
            get { return new Rectangle(this.X, this.Y, this.Width, this.Height); }
            set { _rect = value; }
        }

        public int X
        {
            get { return (int)(_rect.X * resize); }
        }

        public int Y
        {
            get { return (int)(_rect.Y * resize); }
        }

        public int Width
        {
            get { return (int)(_rect.Width / resize); }
        }
        
        public int Height
        {
            get { return (int)(_rect.Width / resize); }
        }
    }
}

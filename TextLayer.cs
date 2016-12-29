using System.Drawing;
using System.Drawing.Drawing2D;

namespace CFVG_Card_Creator
{
    //Inherit from ImageLayer class
    class TextLayer : ImageLayer
    {
        //Other Variables
        string text = "";
        Font font;
        Color fontColour = Color.Black;
        public Pen outline = null;
        float scale = 0;

        public TextLayer(string newText, Font newFont, Color newfontColour)
        {
            //Font Styles and Text
            text = newText;
            font = newFont;
            fontColour = newfontColour;

            Size newSize = new Size(349, 507);

            //Get Size of Image
            using (GraphicsPath measureG = new GraphicsPath())
            {
                measureG.AddString(text, font.FontFamily, (int)font.Style, font.Size, new Point(0, 0), new StringFormat());
                RectangleF dim = measureG.GetBounds();
                newSize = Size.Round(dim.Size);

                base.image = new Bitmap(newSize.Width, newSize.Height);

                measureG.Dispose();
            }
        }

        public TextLayer(string newText, Font newFont, Color newFontColour, float posX, int posY, StringAlignment textAlign)
        {
            //Font Styles and Text
            text = newText;
            font = newFont;
            fontColour = newFontColour;

            Size newSize = new Size(349, 507);
            //Get Size of Image
            using (GraphicsPath measureG = new GraphicsPath())
            {
                measureG.AddString(text, font.FontFamily, (int)font.Style, font.Size, new Point(0, 0), new StringFormat());
                RectangleF dim = measureG.GetBounds();
                newSize = Size.Round(dim.Size);

                base.image = new Bitmap(newSize.Width, newSize.Height);

                measureG.Dispose();
            }

            //Position
            base.posX = (int)posX;
            base.posY = posY;

            if (textAlign == StringAlignment.Center)
            {
                int check = (int)(posX - (float)newSize.Width / 2f);
                base.posX = check >= 0 ? check : 0;
            }
            else if (textAlign == StringAlignment.Far)
            {
                base.posX = (int)(posX - newSize.Width);
            }
        }

        public TextLayer(string newText, Font newFont, Color newFontColour, float posX, int posY, StringAlignment textAlign, Pen isOutline)
        {
            //Font Styles and Text
            text = newText;
            font = newFont;
            fontColour = newFontColour;

            Size newSize = new Size(349, 507);
            //Get Size of Image
            using (GraphicsPath measureG = new GraphicsPath())
            {
                measureG.AddString(text, font.FontFamily, (int)font.Style, font.Size, new Point(0, 0), new StringFormat());
                RectangleF dim = measureG.GetBounds();
                newSize = Size.Round(dim.Size);

                base.image = new Bitmap(newSize.Width, newSize.Height);

                measureG.Dispose();
            }

            //Position
            base.posX = (int)posX;
            base.posY = posY;

            if (textAlign == StringAlignment.Center)
            {
                int check = (int)(posX - (float)newSize.Width / 2f);
                base.posX = check >= 0 ? check : 0;
            }
            else if (textAlign == StringAlignment.Far)
            {
                base.posX = (int)(posX - newSize.Width);
            }

            outline = isOutline;
        }
        
        public TextLayer(string newText, Font newFont, Color newFontColour, float posX, int posY, int maxWidth, StringAlignment textAlign)
        {
            //Font Styles and Text
            text = newText;
            font = newFont;
            fontColour = newFontColour;

            Size newSize = new Size(349, 507);
            //Get Size of Image
            using (GraphicsPath measureG = new GraphicsPath())
            {
                measureG.AddString(text, font.FontFamily, (int)font.Style, font.Size, new Point(0, 0), new StringFormat());
                RectangleF dim = measureG.GetBounds();
                newSize = Size.Round(dim.Size);

                if (newSize.Width > maxWidth)
                {
                    scale = (float) maxWidth / newSize.Width;
                    newSize.Width = maxWidth;
                }

                base.image = new Bitmap(newSize.Width, newSize.Height);


                measureG.Dispose();
            }

            //Position
            base.posX = (int)posX;
            base.posY = posY;

            if (textAlign == StringAlignment.Center)
            {
                int check = (int)(posX - (float)newSize.Width / 2f);
                base.posX = check >= 0 ? check : 0;
            }
            else if (textAlign == StringAlignment.Far)
            {
                base.posX = (int)(posX - newSize.Width);
            }
        }


        public TextLayer(string newText, Font newFont, Color newFontColour, float posX, int posY, int maxWidth, StringAlignment textAlign, Pen isOutline)
        {
            //Font Styles and Text
            text = newText;
            font = newFont;
            fontColour = newFontColour;

            Size newSize = new Size(349, 507);
            //Get Size of Image
            using (GraphicsPath measureG = new GraphicsPath())
            {
                measureG.AddString(text, font.FontFamily, (int)font.Style, font.Size, new Point(0, 0), new StringFormat());
                RectangleF dim = measureG.GetBounds();
                newSize = Size.Round(dim.Size);

                if (newSize.Width > maxWidth)
                {
                    scale = (float)maxWidth / newSize.Width;
                    newSize.Width = maxWidth;
                }

                base.image = new Bitmap(newSize.Width, newSize.Height);

                measureG.Dispose();
            }

            //Position
            base.posX = (int)posX;
            base.posY = posY;

            if (textAlign == StringAlignment.Center)
            {
                int check = (int)(posX - (float)newSize.Width / 2f);
                base.posX = check >= 0 ? check : 0;
            }
            else if (textAlign == StringAlignment.Far)
            {
                base.posX = (int)(posX - newSize.Width);
            }

            outline = isOutline;
        }

        
        //New DrawImage Class
        public new void DrawImage(Graphics g)
        {
            //Create Path
            GraphicsPath path = new GraphicsPath();
            path.AddString(text, font.FontFamily, (int)font.Style, font.Size, new Point(0, 0), new StringFormat());

            if (scale != 0)
            {
                Matrix scaleM = new Matrix();
                scaleM.Scale(scale, 1, MatrixOrder.Append);
                float translateX = (posX + 3.93565f) * (1 - scale);
                path.Transform(scaleM);
                scaleM.Dispose();
            }

            Matrix transM = new Matrix();
            transM.Translate(posX, posY);
            path.Transform(transM);
            transM.Dispose();

            //Only create outline if there is a Pen loaded
            if (outline != null)
            {
                //Create Outline
                Pen pen = outline;
                pen.LineJoin = LineJoin.Round;
                g.DrawPath(pen, path);

                pen.Dispose();
            }

            //Draw
            SolidBrush brush = new SolidBrush(fontColour);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillPath(brush, path);

            path.Dispose();
            brush.Dispose();
        }

        public string Text
        {
            get { return text; }
        }

        public void DisposeFont()
        {
            font.Dispose();
        }

        public float fontSize
        {
            get { return font.Size; }
        }

        public FontFamily fontFamily
        {
            get { return font.FontFamily; }
        }

        public Font FullFont
        {
            get { return font; }
        }

        public Color TextColour
        {
            get { return fontColour; }
        }

        public Point Position
        {
            get { return new Point(posX, posY); }
        }
    }
}

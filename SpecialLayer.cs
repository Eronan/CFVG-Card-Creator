using System.Drawing;
using System.Drawing.Drawing2D;

namespace CFVG_Card_Creator
{
    //Used for Counter Blasts, Counter Charges and etc.
    class SpecialLayer : ImageLayer
    {
        string value;
        Font valueFont;
        Color fontColour;
        Pen outline = null;

        public SpecialLayer(Bitmap img, string text, Font textFont, Color textColour, int posX, int posY)
        {
            //Load Image and Text
            base.image = img;
            value = text;
            valueFont = textFont;
            fontColour = textColour;
            base.posX = posX;
            base.posY = posY;
        }

        public SpecialLayer(Bitmap img, string text, Font textFont, Color textColour, int posX, int posY, Pen isOutline)
        {
            //Load Image and Text
            base.image = img;
            value = text;
            valueFont = textFont;
            fontColour = textColour;
            base.posX = posX;
            base.posY = posY;

            outline = isOutline;
        }

        public new void DrawImage(Graphics g)
        {
            g.DrawImageUnscaled(image, base.posX, base.posY);
            //Create String
            GraphicsPath path = new GraphicsPath();
            path.AddString(value, valueFont.FontFamily, (int)valueFont.Style, 10.9f, new Point(base.posX + 15 + (outline == null ? 0 : 3), base.posY + (outline == null ? 0 : 3)), new StringFormat());

            if (outline != null)
            {
                outline.LineJoin = LineJoin.Round;
                g.DrawPath(outline, path);
            }

            //Draw
            SolidBrush brush = new SolidBrush(fontColour);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillPath(brush, path);

            path.Dispose();
            brush.Dispose();
        }
    }
}

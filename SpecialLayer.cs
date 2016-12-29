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

        public new void DrawImage(Graphics g)
        {
            g.DrawImageUnscaled(image, base.posX, base.posY);
            //Create String
            GraphicsPath path = new GraphicsPath();
            path.AddString(value, valueFont.FontFamily, (int)valueFont.Style, 10.9f, new Point(base.posX + 15, base.posY), new StringFormat());

            //Create Outline
            /*Pen pen = new Pen(Color.White);
            pen.LineJoin = LineJoin.Round;
            g.DrawPath(pen, path);*/

            //pen.Dispose();

            //Draw
            SolidBrush brush = new SolidBrush(fontColour);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillPath(brush, path);

            path.Dispose();
            brush.Dispose();
        }
    }
}

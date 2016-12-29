using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CFVG_Card_Creator
{
    class ImageLayer
    {
        public int posX;
        public int posY;
        public Bitmap image;

        public ImageLayer()
        {
            //Load Blank Image at Point(0,0)
            image = new Bitmap(349, 507);
            image.MakeTransparent();
            posX = 0;
            posY = 0;
        }

        public ImageLayer(Bitmap newImage)
        {
            //Load Image at Point(0,0)
            image = newImage;
            posX = 0;
            posY = 0;
        }

        public ImageLayer(Bitmap newImage, int newPosX, int newPosY)
        {
            //Load Image at Point(posX,posY)
            image = newImage;
            posX = newPosX;
            posY = newPosY;
        }

        public void ChangeImageColour(Color newColour)
        {
            //Change the colour of the image to specified colour
            ///Go through each pixel
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    //Change the pixel colour if it is not empty
                    Color gotColor = image.GetPixel(x, y);
                    if (gotColor.A > 0)
                    {
                        gotColor = Color.FromArgb(newColour.R, newColour.G, newColour.B);
                        image.SetPixel(x, y, gotColor);
                    }
                }
            }
        }

        public void DrawImage(Graphics g)
        {
            //Draw Image onto Graphics class
            g.DrawImageUnscaled(image, posX, posY);
        }

        public int width
        {
            //Return Width no Set
            get { return image.Width; }
        }

        public int height
        {
            //Return Height no Set
            get { return image.Height; }
        }

        public Rectangle toRect
        {
            //Return Rectangle no Set
            get { return new Rectangle(posX, posY, this.width, this.height); }
        }
    }
}

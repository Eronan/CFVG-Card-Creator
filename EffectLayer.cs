using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CFVG_Card_Creator
{
    class EffectLayer
    {
        const double TEXTHEIGHT = 17;
        const int MAXWIDTH = 325;
        List<TextLayer> effect_Text = new List<TextLayer>();
        List<ImageLayer> effect_Images = new List<ImageLayer>();
        List<SpecialLayer> effect_Special = new List<SpecialLayer>();

        public EffectLayer()
        {

        }

        public EffectLayer(string text, FontFamily textFont, int startY, Dictionary<string, Bitmap> effectIcons, Dictionary<string, Bitmap> specialIcons)
        {
            bool red = false;
            bool italics = false;
            bool bold = false;

            text = text.Replace("<<", "«").Replace(">>", "»");

            double currentY = startY;

            //Find respace requirement
            foreach (string line in text.Split('\n'))
            {
                //Find Respace Value
                int width = 0;
                foreach (string word in line.Split(' '))
                {
                    foreach (string input in word.Split('_'))
                    {
                        //Initialize Variables
                        Bitmap getBitmap = null;

                        //Create editable string from initial
                        string newInput = input;

                        //Remove Apostrophes
                        if (input.StartsWith("'''"))
                        {
                            //Remove three apostrophes
                            newInput = input.Remove(0, 3);
                        }
                        //Remove the end of three apostrophes
                        if (newInput.EndsWith("'''")) newInput = newInput.Remove(newInput.Length - 3, 3);

                        if (newInput.StartsWith("{") && newInput.EndsWith("}"))
                        {
                            //Get string without Brackets
                            string key = newInput.Substring(1, newInput.Length - 2);

                            if (effectIcons.TryGetValue(key, out getBitmap))
                            {
                                width += getBitmap.Width + 2;
                            }
                            else
                            {
                                int startText = key.IndexOf("(");
                                int endText = key.IndexOf(")");

                                //Check it exists
                                if (startText != -1 && endText != -1)
                                {
                                    string mainKey = key.Remove(startText);

                                    if (specialIcons.TryGetValue(mainKey, out getBitmap))
                                    {
                                        width += getBitmap.Width + 2;
                                    }
                                }
                            }
                        }
                        else if (newInput.Length != 0)
                        {
                            TextLayer temporary = new TextLayer(newInput, new Font(textFont, 11.5f, FontStyle.Regular), Color.Black, 0, 0, StringAlignment.Near);
                            width += temporary.width + 3;

                            //Fix Spacing
                            switch (newInput.Last())
                            {
                                case 'l':
                                case '1':
                                case 'd':
                                case 'n':
                                    width += 1;
                                    break;
                            }
                        }
                        else if (newInput.Length == 0)
                        {
                            width += 3;
                        }
                    }
                }

                int respace = (MAXWIDTH - width) > 0 ? (MAXWIDTH - width) / line.Split(' ').Length : 0;

                int currentX = 12;

                foreach (string word in line.Split(' '))
                {
                    foreach (string input in word.Split('_'))
                    {
                        //Initialize Variables
                        Bitmap getBitmap = null;
                        bool redChanged = false;

                        //Create editable string from initial
                        string newInput = input;

                        //Enable Change Colour to Red, Enable Italics, Enable Bold
                        if (!italics && !red && input.StartsWith("\""))
                        {
                            red = true;
                            redChanged = true;
                        }
                        else if (!italics && input.StartsWith("(")) italics = true;
                        else if (input.StartsWith("'''"))
                        {
                            //Remove three apostrophes
                            newInput = input.Remove(0, 3);
                            bold = true;
                        }

                        //Remove the end of three apostrophes
                        if (newInput.EndsWith("'''")) newInput = newInput.Remove(newInput.Length - 3, 3);

                        //Set colour of the text
                        Color textColour = red ? Color.Red : Color.Black;

                        //Check if image fetch is required
                        if (newInput.StartsWith("{") && newInput.EndsWith("}"))
                        {
                            //Get string without Brackets
                            string key = newInput.Substring(1, newInput.Length - 2);

                            //Check if exists and load into getBitmap
                            if (effectIcons.TryGetValue(key, out getBitmap))
                            {
                                //Load image in the centre of height
                                effect_Images.Add(new ImageLayer(getBitmap, currentX + 2, (int)(currentY + (TEXTHEIGHT - getBitmap.Height) / 2)));
                                currentX += getBitmap.Width;
                            }
                            else
                            {
                                //Find the value for the Special Replacement
                                int startText = key.IndexOf("(");
                                int endText = key.IndexOf(")");

                                //Check that it exists
                                if (startText != -1 && endText != -1)
                                {
                                    string mainKey = key.Remove(startText) + (red ? "R" : "");

                                    if (specialIcons.TryGetValue(mainKey, out getBitmap))
                                    {
                                        //Mix of Image and Text Layer
                                        effect_Special.Add(new SpecialLayer(getBitmap, key.Substring(startText + 1, endText - startText - 1), new Font(textFont, 11.09f, FontStyle.Regular), textColour, currentX + 1, (int)(currentY + (TEXTHEIGHT - getBitmap.Height) / 2)));
                                        currentX += getBitmap.Width;
                                    }
                                }
                            }
                        }
                        else if (newInput.StartsWith("{") && !newInput.StartsWith("}")) //Make sure 
                        {
                            MessageBox.Show("Make sure there is only 1 word for your text replacement.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (newInput.Length != 0)
                        {
                            //SEt Font Style
                            FontStyle style = FontStyle.Regular;

                            if (bold && italics) style = FontStyle.Bold | FontStyle.Italic;
                            else if (italics) style = FontStyle.Italic;
                            else if (bold) style = FontStyle.Bold;

                            effect_Text.Add(new TextLayer(newInput, new Font(textFont, 11.5f, style), textColour, currentX, (int)(currentY + 2.5), StringAlignment.Near));

                            currentX += effect_Text.Last().width + 3 - (italics ? 1 : 0);

                            //Fix Spacing
                            switch (newInput.Last())
                            {
                                case 'l':
                                case '1':
                                case 'd':
                                case 'n':
                                    currentX += 1;
                                    break;
                            }
                        }
                        else if (newInput.Length == 0)
                        {
                            currentX += 2;
                        }

                        //End styles
                        if (red && input.EndsWith("\"") && (!redChanged || input.Length > 1)) red = false;
                        else if (italics && input.EndsWith(")")) italics = false;
                        else if (input.EndsWith("'''"))
                        {
                            bold = false;
                        }
                    }

                    currentX += respace;
                }

                currentY += TEXTHEIGHT;
            }

            int integer = effect_Text.Count;
            integer = effect_Images.Count;
            integer = effect_Special.Count;
        }

        public void DrawImage(Graphics g)
        {
            foreach (ImageLayer img in effect_Images)
            {
                img.DrawImage(g);
            }
            foreach (TextLayer txt in effect_Text)
            {
                txt.DrawImage(g);
            }
            foreach (SpecialLayer spec in effect_Special)
            {
                spec.DrawImage(g);
            }
        }
    }
}

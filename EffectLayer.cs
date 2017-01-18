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

        public EffectLayer(string text, FontFamily textFont, int startY, Dictionary<string, Bitmap> effectIcons, Dictionary<string, Bitmap> specialIcons, char[] wordSplit)
        {
            bool red = false;
            bool italics = false;
            bool bold = false;

            bool measureBold = false;
            bool measureItalics = false;

            text = text.Replace("<<", "«").Replace(">>", "»");

            double currentY = startY;

            //Find respace requirement
            foreach (string line in text.Split('\n'))
            {
                //Find Respace Value
                int width = 0;
                foreach (string word in line.Split(wordSplit[0]))
                {
                    foreach (string input in word.Split(wordSplit[1]))
                    {
                        //Initialize Variables
                        Bitmap getBitmap = null;

                        //Create editable string from initial
                        string newInput = input;

                        //Notify Program to set value false
                        bool endBold = false;
                        bool endItalics = false;

                        //Remove and Enable Colour Change, Italics and Bold
                        while (newInput.StartsWith("<r>") || newInput.StartsWith("<i>") || newInput.StartsWith("<b>"))
                        {
                            if (newInput.StartsWith("<r>")) newInput = newInput.Remove(0, 3);
                            else if (newInput.StartsWith("<i>"))
                            {
                                newInput = newInput.Remove(0, 3);
                                measureItalics = true;
                            }
                            else if (newInput.StartsWith("<b>"))
                            {
                                newInput = newInput.Remove(0, 3);
                                measureBold = true;
                            }
                        }

                        //Remove End Tags
                        while (newInput.EndsWith("</b>") || newInput.EndsWith("</r>") || newInput.EndsWith("</i>"))
                        {
                            if (newInput.EndsWith("</r>")) newInput = newInput.Remove(newInput.Length - 4, 4);
                            else if (newInput.EndsWith("</b>"))
                            {
                                newInput = newInput.Remove(newInput.Length - 4, 4);
                                endBold = true;
                            }
                            else if (newInput.EndsWith("</i>"))
                            {
                                newInput = newInput.Remove(newInput.Length - 4, 4);
                                endItalics = true;
                            }
                        }

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
                            FontStyle style = FontStyle.Regular;

                            if (measureBold && measureItalics) style = FontStyle.Bold | FontStyle.Italic;
                            else if (measureItalics) style = FontStyle.Italic;
                            else if (measureBold) style = FontStyle.Bold;

                            TextLayer temporary = new TextLayer(newInput, new Font(textFont, 11.5f, style), Color.Black, 0, 0, StringAlignment.Near);
                            width += temporary.width + 3;

                            if (!measureItalics && !measureBold)
                            {
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
                        }
                        else if (newInput.Length == 0)
                        {
                            width += 3;
                        }

                        //End styles
                        if (measureItalics && endItalics) measureItalics = false;
                        if (measureBold && endBold) measureBold = false;
                    }
                }

                int respace = (MAXWIDTH - width) > 0 ? (MAXWIDTH - width) / line.Split(wordSplit[0]).Length : 0;

                int currentX = 12;

                foreach (string word in line.Split(wordSplit[0]))
                {
                    foreach (string input in word.Split(wordSplit[1]))
                    {
                        //Initialize Variables
                        Bitmap getBitmap = null;

                        //Create editable string from initial
                        string newInput = input;

                        //Notify Program to set value false
                        bool endBold = false;
                        bool endItalics = false;
                        bool endRed = false;

                        //Remove and Enable Colour Change, Italics and Bold
                        while (newInput.StartsWith("<r>") || newInput.StartsWith("<i>") || newInput.StartsWith("<b>"))
                        {
                            if (newInput.StartsWith("<r>"))
                            {
                                newInput = newInput.Remove(0, 3);
                                red = true;
                            }
                            else if (newInput.StartsWith("<i>"))
                            {
                                newInput = newInput.Remove(0, 3);
                                italics = true;
                            }
                            else if (newInput.StartsWith("<b>"))
                            {
                                newInput = newInput.Remove(0, 3);
                                bold = true;
                            }
                        }

                        //Remove End Tags
                        while (newInput.EndsWith("</b>") || newInput.EndsWith("</r>") || newInput.EndsWith("</i>"))
                        {
                            if (newInput.EndsWith("</b>"))
                            {
                                newInput = newInput.Remove(newInput.Length - 4, 4);
                                endBold = true;
                            }
                            else if (newInput.EndsWith("</r>"))
                            {
                                newInput = newInput.Remove(newInput.Length - 4, 4);
                                endRed = true;
                            }
                            else if (newInput.EndsWith("</i>"))
                            {
                                newInput = newInput.Remove(newInput.Length - 4, 4);
                                endItalics = true;
                            }
                        }



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
                                        effect_Special.Add(new SpecialLayer(getBitmap, key.Substring(startText + 1, endText - startText - 1), new Font(textFont, 11.09f, FontStyle.Regular), textColour, currentX + 2, (int)(currentY + (TEXTHEIGHT - getBitmap.Height) / 2)));
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

                            currentX += effect_Text.Last().width + 3 - (italics ? 1 : 0) - (endBold || endItalics ? 2 : 0);

                            if (!italics)
                            {
                                //Fix Spacing
                                switch (newInput.Last())
                                {
                                    case 'w':
                                    case 'Y':
                                        currentX--;
                                        break;
                                    case 'l':
                                    case 'i':
                                    case '1':
                                    case '.':
                                    case 'r':
                                    case '!':
                                    case 'E':
                                    case 'I':
                                    case 'U':
                                    case '|':
                                    case 'D':
                                    case 'F':
                                    case 'G':
                                    case 'H':
                                    case 'J':
                                    case 'K':
                                    case 'L':
                                    case '0':
                                    case 'd':
                                    case 'N':
                                    case 'B':
                                        currentX++;
                                        break;
                                }
                            }
                            else
                            {
                                switch (newInput.Last())
                                {
                                    case '2':
                                    case '5':
                                    case '7':
                                    case 'y':
                                    case 'F':
                                    case 'X':
                                    case 'd':
                                    case 'g':
                                    case 'j':
                                        currentX--;
                                        break;
                                    case '`':
                                    case '1':
                                    case 'q':
                                    case 'e':
                                    case 'u':
                                    case '\\':
                                    case 'h':
                                    case 'c':
                                    case 'v':
                                    case 'b':
                                    case ',':
                                    case '.':
                                    case '~':
                                    case '%':
                                    case '^':
                                    case '*':
                                    case 'W':
                                    case 'T':
                                    case 'U':
                                    case '|':
                                    case 'A':
                                    case 'L':
                                    case 'V':
                                    case '>':
                                        currentX++;
                                        break;
                                }
                            }
                        }
                        else if (newInput.Length == 0)
                        {
                            currentX += 2;
                        }

                        //End styles
                        if (red && endRed) red = false;
                        if (italics && endItalics) italics = false;
                        if (bold && endBold) bold = false;
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

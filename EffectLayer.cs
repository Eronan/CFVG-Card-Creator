using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CFVG_Card_Creator
{
    class EffectLayer
    {
        public static double TEXTHEIGHT = 15;
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

                        //Get Tag Values: Bold, Red, Italics, endBold, endRed, endItalics
                        Tuple<bool, bool, bool, bool, bool, bool> EndTags = RemoveTags(newInput, measureBold, false, measureItalics, out newInput);

                        measureBold = EndTags.Item1;
                        measureItalics = EndTags.Item3;
                        bool endBold = EndTags.Item4;
                        bool endItalics = EndTags.Item6;

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

                            width += LastCharacter(newInput.Last(), measureItalics);
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

                        //Get Tag Values: Bold, Red, Italics, endBold, endRed, endItalics
                        Tuple<bool, bool, bool, bool, bool, bool> EndTags = RemoveTags(newInput, bold, red, italics, out newInput);

                        bold = EndTags.Item1;
                        red = EndTags.Item2;
                        italics = EndTags.Item3;
                        bool endBold = EndTags.Item4;
                        bool endRed = EndTags.Item5;
                        bool endItalics = EndTags.Item6;

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
                                        effect_Special.Add(new SpecialLayer(getBitmap, key.Substring(startText + 1, endText - startText - 1), new Font(textFont, 11.09f, FontStyle.Regular), textColour, currentX + 2, (int)((double)currentY + (TEXTHEIGHT - getBitmap.Height) / 2)));
                                        currentX += getBitmap.Width;
                                    }
                                }
                            }
                        }
                        else if (newInput.StartsWith("{") && !newInput.StartsWith("}")) //Double Check
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

                            //Change currentX based on last character
                            currentX += LastCharacter(newInput.Last(), italics);

                        }
                        else if (newInput.Length == 0)  currentX += 2;

                        //End styles
                        if (red && endRed) red = false;
                        if (italics && endItalics) italics = false;
                        if (bold && endBold) bold = false;
                    }

                    currentX += respace;
                }

                currentY += TEXTHEIGHT;
            }
        }

        public EffectLayer(string text, FontFamily textFont, int startY, Dictionary<string, Bitmap> effectIcons, Dictionary<string, Bitmap> specialIcons, char[] wordSplit, int outline)
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
                Pen outlinePen = new Pen(Color.White, outline + 0.5f);

                foreach (string word in line.Split(wordSplit[0]))
                {
                    foreach (string input in word.Split(wordSplit[1]))
                    {
                        //Initialize Variables
                        Bitmap getBitmap = null;

                        //Create editable string from initial
                        string newInput = input;

                        //Get Tag Values: Bold, Red, Italics, endBold, endRed, endItalics
                        Tuple<bool, bool, bool, bool, bool, bool> EndTags = RemoveTags(newInput, measureBold, false, measureItalics, out newInput);

                        measureBold = EndTags.Item1;
                        measureItalics = EndTags.Item3;
                        bool endBold = EndTags.Item4;
                        bool endItalics = EndTags.Item6;

                        if (newInput.StartsWith("{") && newInput.EndsWith("}"))
                        {
                            //Get string without Brackets
                            string key = newInput.Substring(1, newInput.Length - 2);

                            if (effectIcons.TryGetValue(key, out getBitmap))
                            {
                                width += getBitmap.Width + 1 + outline;
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
                                        width += getBitmap.Width + 1 + outline;
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
                            width += temporary.width + 2 + outline;

                            width += LastCharacter(newInput.Last(), measureItalics);
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

                        //Get Tag Values: Bold, Red, Italics, endBold, endRed, endItalics
                        Tuple<bool, bool, bool, bool, bool, bool> EndTags = RemoveTags(newInput, bold, red, italics, out newInput);

                        bold = EndTags.Item1;
                        red = EndTags.Item2;
                        italics = EndTags.Item3;
                        bool endBold = EndTags.Item4;
                        bool endRed = EndTags.Item5;
                        bool endItalics = EndTags.Item6;

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
                                getBitmap = outlineImage(getBitmap, outline);
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
                                        getBitmap = outlineImage(getBitmap, outline);
                                        //Mix of Image and Text Layer
                                        effect_Special.Add(new SpecialLayer(getBitmap, key.Substring(startText + 1, endText - startText - 1), new Font(textFont, 11.09f, FontStyle.Regular), textColour, currentX + 2, (int)((double)currentY + (TEXTHEIGHT - getBitmap.Height) / 2), outlinePen));
                                        currentX += getBitmap.Width;
                                    }
                                }
                            }
                        }
                        else if (newInput.StartsWith("{") && !newInput.StartsWith("}")) //Double Check
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

                            effect_Text.Add(new TextLayer(newInput, new Font(textFont, 11.5f, style), textColour, currentX, (int)(currentY + 2.5), StringAlignment.Near, outlinePen));

                            currentX += effect_Text.Last().width + 3 - (italics ? 1 : 0) - (endBold || endItalics ? 2 : 0);

                            //Change currentX based on last character
                            currentX += LastCharacter(newInput.Last(), italics);
                        }
                        else if (newInput.Length == 0) currentX += 2;

                        //End styles
                        if (red && endRed) red = false;
                        if (italics && endItalics) italics = false;
                        if (bold && endBold) bold = false;
                    }

                    currentX += respace;
                }

                currentY += TEXTHEIGHT;
            }
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

        public static Bitmap outlineImage(Bitmap original, int outline)
        {
            Bitmap returnBitmap = new Bitmap(original.Width + outline * 2, original.Height + outline * 2);
            Bitmap mask = new Bitmap(original.Width + outline * 2, original.Height + outline * 2);

            //Move original Bitmap 1 across and 1 down
            Graphics g = Graphics.FromImage(returnBitmap);
            g.DrawImage(original, new Point(outline, outline));

            for (int i = 0; i < returnBitmap.Width; i++)
            {
                for (int j = 0; j < returnBitmap.Height; j++)
                {
                    //Check if Pixel is Transparent
                    if (returnBitmap.GetPixel(i, j).A < 201)
                    {
                        if (j + 1 < returnBitmap.Height && returnBitmap.GetPixel(i, j + 1).A != Color.Transparent.A)
                        {
                            //If Pixel Underneath is not Transparent
                            mask.SetPixel(i, j, Color.FromArgb(201, 255, 255, 255));
                            for (int k = 0; k < outline - 1; k++)
                            {
                                Color outlineColour = Color.White;
                                if (k == outline - 2) Color.FromArgb(201, 255, 255, 255);
                                mask.SetPixel(i, j - k, outlineColour);
                            }
                        }
                        if (j - 1 > 0 && returnBitmap.GetPixel(i, j - 1).A != Color.Transparent.A)
                        {
                            //If Pixel Above is not Transparent
                            mask.SetPixel(i, j, Color.FromArgb(201, 255, 255, 255));
                            for (int k = 0; k < outline - 1; k++)
                            {
                                Color outlineColour = Color.White;
                                if (k == outline - 2) Color.FromArgb(201, 255, 255, 255);
                                mask.SetPixel(i, j + k, outlineColour);
                            }
                        }
                        if (i - 1 > 0 && returnBitmap.GetPixel(i - 1, j).A != Color.Transparent.A)
                        {
                            //If Pixel to the Left is not Transparent
                            mask.SetPixel(i, j, Color.FromArgb(201, 255, 255, 255));
                            for (int k = 0; k < outline - 1; k++)
                            {
                                Color outlineColour = Color.White;
                                if (k == outline - 2) Color.FromArgb(201, 255, 255, 255);
                                mask.SetPixel(i + k, j, outlineColour);
                            }
                        }
                        if (i + 1 < returnBitmap.Width && returnBitmap.GetPixel(i + 1, j).A != Color.Transparent.A)
                        {
                            //If Pixel to the Right is not Transparent
                            mask.SetPixel(i, j, Color.FromArgb(201, 255, 255, 255));
                            for (int k = 0; k < outline - 1; k++)
                            {
                                Color outlineColour = Color.White;
                                if (k == outline - 2) Color.FromArgb(201, 255, 255, 255);
                                mask.SetPixel(i - k, j, outlineColour);
                            }
                        }
                    }
                }
            }

            g.DrawImage(mask, new Point(1, 1));
            g.Dispose();

            mask.Dispose();

            return returnBitmap;
        }

        private Tuple<bool, bool, bool, bool, bool, bool> RemoveTags(string text, bool bold, bool red, bool italics, out string newString)
        {
            bool endBold = false;
            bool endRed = false;
            bool endItalics = false;

            //Remove and Enable Colour Change, Italics and Bold
            while (text.StartsWith("<r>") || text.StartsWith("<i>") || text.StartsWith("<b>"))
            {
                if (text.StartsWith("<b>"))
                {
                    text = text.Remove(0, 3);
                    bold = true;
                }
                else if (text.StartsWith("<r>"))
                {
                    text = text.Remove(0, 3);
                    red = true;
                }
                else if (text.StartsWith("<i>"))
                {
                    text = text.Remove(0, 3);
                    italics = true;
                }
            }

            //Remove End Tags
            while (text.EndsWith("</b>") || text.EndsWith("</r>") || text.EndsWith("</i>"))
            {
                if (text.EndsWith("</b>"))
                {
                    text = text.Remove(text.Length - 4, 4);
                    endBold = true;
                }
                else if (text.EndsWith("</r>"))
                {
                    text = text.Remove(text.Length - 4, 4);
                    endRed = true;
                }
                else if (text.EndsWith("</i>"))
                {
                    text = text.Remove(text.Length - 4, 4);
                    endItalics = true;
                }
            }

            newString = text;

            return new Tuple<bool, bool, bool, bool, bool, bool>(bold, red, italics, endBold, endRed, endItalics);
        }

        private int LastCharacter(char lastChar, bool italics)
        {
            if (!italics)
            {
                //Fix Spacing
                switch (lastChar)
                {
                    case 'w': case 'Y':
						return -1;
                    case 'l': case 'i': case '1': case '.': case 'r': case '!': case 'E': case 'I': case 'U': case '|': case 'D': case 'F': case 'G': case 'H': case 'J': case 'K':
                    case 'L': case '0': case 'd': case 'N': case 'B':
						return 1;
                }
            }
            else
            {
                switch (lastChar)
                {
                    case '2': case '5': case '7': case 'y': case 'F': case 'X': case 'g': case 'j':
						return -1;
                    case '`': case '1': case 'q': case 'e': case 'u': case '\\': case 'h': case 'c': case 'v': case 'b': case ',': case '.': case '~': case '%': case '^': case '*':
                    case 'W': case 'T': case 'U': case '|': case 'A': case 'L': case 'V': case '>':
						return 1;
                }
            }
            return 0;
        }


    }
}

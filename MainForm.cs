using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;


namespace CFVG_Card_Creator
{
    public partial class MainForm : Form
    {
        //Card to be exported
        Bitmap mainBitmap = new Bitmap(349, 508);

        //Background Art
        Bitmap CardArt = new Bitmap(349, 508);

        //FontList
        FontFamily Impact;
        FontFamily MatrixBook;
        FontFamily Optima;
        FontFamily Optima_Thin;

        //Replacemens and Nation Colours
        Dictionary<string, Tuple<Bitmap, Color, Color, Color>> NationList = new Dictionary<string, Tuple<Bitmap, Color, Color, Color>>();
        Dictionary<string, Bitmap> effectReplacements = new Dictionary<string, Bitmap>();
        Dictionary<string, Bitmap> specialReplacements = new Dictionary<string, Bitmap>();

        //Layers
        List<ImageLayer> Layers_Images = new List<ImageLayer>();
        List<TextLayer> Layers_Text = new List<TextLayer>();
        EffectLayer Layers_Effect = new EffectLayer();


        //Other
        string filePath = null;

        //Preference Information
        string cardArtPath = "";
        string imagePath = "";
        string dataPath = "";
        bool invertRespace = false;
        string appPath = Application.StartupPath;

        private void SetUp()
        {
            tooltip.SetToolTip(button_Bold, "Bold");
            tooltip.SetToolTip(button_Italics, "Italics");
            tooltip.SetToolTip(button_Red, "Red");

            //Get Nation Colours
            using (StreamReader nationsReader = new StreamReader(appPath + "/Images/Nation/nations.txt"))
            {
                string text = nationsReader.ReadToEnd();

                string[] allNations = text.Replace("\r\n", "").Split(';');
                foreach (string line in allNations)
                {
                    string[] splitLine = line.Split('|');
                    if (splitLine.Length > 1)
                    {
                        //Load Bitmap
                        Bitmap flagBitmap = null;
                        if (splitLine[1].Trim().Length > 4) flagBitmap = new Bitmap(appPath + "/Images/Nation/" + splitLine[1].Trim());

                        Color tempOutline = ColorTranslator.FromHtml(splitLine[4]);
                        tempOutline = Color.FromArgb(125, tempOutline);

                        NationList.Add(splitLine[0].Trim(), new Tuple<Bitmap, Color, Color, Color>(flagBitmap, ColorTranslator.FromHtml(splitLine[2].Trim()), ColorTranslator.FromHtml(splitLine[3].Trim()), tempOutline));
                        combobox_Nation.Items.Add(splitLine[0].Trim());
                    }
                }

                nationsReader.Close();
            }

            //Read Effect replacements
            using (StreamReader effectsReader = new StreamReader(appPath + "/Images/Effect/replacements.txt"))
            {
                string completeText = "";
                string line = "";
                while ((line = effectsReader.ReadLine()) != null)
                {
                    if (!line.StartsWith("#")) completeText += line;
                }

                string[] allReplaces = completeText.Split(';');
                foreach (string replaceLines in allReplaces)
                {
                    string[] seperate = replaceLines.Trim().Split('=');
                    if (seperate.Length > 1) effectReplacements.Add(seperate[0].Trim(), new Bitmap(appPath + "/Images/Effect/" + seperate[1].Trim()));
                }

                effectsReader.Close();
            }

            //Add CB, CC, SB and SC into Dictionary
            specialReplacements.Add("CB", Properties.Resources.Icon_CB);
            specialReplacements.Add("CC", Properties.Resources.Icon_CC);
            specialReplacements.Add("SB", Properties.Resources.Icon_SB);
            specialReplacements.Add("SC", Properties.Resources.Icon_SC);
            specialReplacements.Add("CBR", Properties.Resources.RedIcon_CB);
            specialReplacements.Add("CCR", Properties.Resources.RedIcon_CC);
            specialReplacements.Add("SBR", Properties.Resources.RedIcon_SB);
            specialReplacements.Add("SCR", Properties.Resources.RedIcon_SC);

            //Load Settings
            using (XmlReader reader = XmlReader.Create(appPath + "/Settings.xml"))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "CARDARTPATH":
                                //Read path for card Art
                                reader.Read();
                                cardArtPath = reader.Value;
                                break;
                            case "IMAGEPATH":
                                //Read Path for Images
                                reader.Read();
                                imagePath = reader.Value;
                                break;
                            case "DATAPATH":
                                //Read Path for Save Files
                                reader.Read();
                                dataPath = reader.Value;
                                break;
                            case "RESPACE":
                                //Read BooleaN for Inverting
                                reader.Read();
                                invertRespace = bool.Parse(reader.Value);
                                break;
                        }
                    }
                }
            }

            saveImage.InitialDirectory = imagePath;
            openData.InitialDirectory = dataPath;
            saveData.InitialDirectory = dataPath;

            //Load Fonts

            PrivateFontCollection myFonts = new PrivateFontCollection();

            //Impact
            myFonts.AddFontFile(appPath + "/Fonts\\impact.ttf");
            //MatrixBook
            myFonts.AddFontFile(appPath + "/Fonts\\MatrixBook.ttf");
            //Optima
            myFonts.AddFontFile(appPath + "/Fonts\\Optima_Regular.ttf");
            //myFonts.AddFontFile(appPath + "/Fonts\\Optima_Bold.ttf");
            myFonts.AddFontFile(appPath + "/Fonts\\Optima_Italic.ttf");
            //OptimaThin
            myFonts.AddFontFile(appPath + "/Fonts\\Optima_Thin.ttf");


            //Load into FontFamily
            Impact = new FontFamily("impact", myFonts);
            MatrixBook = new FontFamily("MatrixBook", myFonts);
            Optima = new FontFamily("Optima", myFonts);
            Optima_Thin = new FontFamily("optima-Thin", myFonts);

            mainBitmap.MakeTransparent();
            CardArt.MakeTransparent();

            //Set all ComboBox Defaults to 0
            combobox_Border.SelectedIndex = 0;
            combobox_Trigger.SelectedIndex = 0;
            combobox_Shield.SelectedIndex = 0;
            combobox_Nation.SelectedIndex = 0;
        }

        public MainForm()
        {
            InitializeComponent();
            SetUp();
        }

        public MainForm(string path)
        {
            InitializeComponent();

            SetUp();

            if (path.EndsWith(".crd"))
            {
                filePath = path;
                openFromFile();
            }
            else
            {
                LoadCardArt cardartForm = new LoadCardArt(new Bitmap(path));
                if (cardartForm.ShowDialog(this) == DialogResult.OK)
                {
                    CardArt = (Bitmap)cardartForm.saveBitmap.Clone();

                    //Disable
                    group_Edit.Enabled = false;
                    Cursor = Cursors.WaitCursor;
                    //Update
                    UpdateLayers();
                    UpdateImage();
                    //Reset Cursor
                    Cursor = Cursors.Default;
                    group_Edit.Enabled = true;

                    button_CardArt.Focus();
                }
                cardartForm.saveBitmap.Dispose();
                cardartForm.Dispose();
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            UpdateLayers();
            UpdateImage();

            //Load up Dictionary of Effect Replacements into ToolStrip Menu
            foreach (KeyValuePair<string, Bitmap> item in effectReplacements)
            {
                //EditMenu Tool
                ToolStripMenuItem MenuTool = new ToolStripMenuItem(item.Key, item.Value, SymbolMenu_Item_Click, "SymbolMenu_" + item.Key);
                MenuTool.ImageScaling = ToolStripItemImageScaling.None;
                EditMenu_Symbols.DropDownItems.Add(MenuTool);

                //InsertSymbol Tool
                ToolStripMenuItem ContextTool = new ToolStripMenuItem(item.Key, item.Value, SymbolMenu_Item_Click, "InsertSymbol_" + item.Key);
                ContextTool.ImageScaling = ToolStripItemImageScaling.None;
                TextboxMenu_Insert.DropDownItems.Add(ContextTool);
            }

            EditMenu_Symbols.DropDown.MaximumSize = new Size(EditMenu_Symbols.DropDown.Width, 600);
            TextboxMenu_Insert.DropDown.MaximumSize = new Size(TextboxMenu_Insert.DropDown.Width, 300);

            //Disable SP Setting
            //checkbox_SP.Enabled = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.N))
            {
                //New File
                FileMenu_New_Click(FileMenu_New, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.O))
            {
                //Open File
                FileMenu_Open_Click(FileMenu_Open, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.S))
            {
                //Save File
                button_Save_Click(FileMenu_Save, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.S))
            {
                //Save File As
                FileMenu_SaveAs_Click(FileMenu_SaveAs, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.E))
            {
                //Export Image
                button_SaveImage_Click(FileMenu_SaveImage, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.L))
            {
                //Load Card Art
                button_CardArt_Click(FileMenu_LoadImage, null);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FileMenu_New_Click(object sender, EventArgs e)
        {
            //Reset Fields to Defaults
            textbox_Code.Text = "";
            textbox_Name.Text = "";
            combobox_Border.SelectedIndex = 0;
            combobox_Trigger.SelectedIndex = 0;
            numeric_Grade.Value = 0;
            numeric_Power.Value = 4000;
            combobox_Shield.SelectedIndex = 0;
            textbox_Clan.Text = "";
            combobox_Nation.SelectedIndex = 0;
            textbox_Race.Text = "";
            checkbox_LegLeader.Checked = false;
            checkbox_LegMate.Checked = false;
            textbox_Illust.Text = "";
            textbox_Design.Text = "";
            richtextbox_Effect.Text = "";
            checkbox_Effect.Checked = true;
            richtextbox_Flavour.Text = "";
            CardArt = new Bitmap(349, 508);

            filePath = null;

            UpdateLayers();
            UpdateImage();
        }

        private void FileMenu_Open_Click(object sender, EventArgs e)
        {
            DialogResult result = openData.ShowDialog(this);
            openData.InitialDirectory = dataPath;

            if (result == DialogResult.OK)
            {
                //Load into filePath string
                filePath = openData.FileName;
                openFromFile();


                //Disable
                button_Preview.Enabled = false;
                button_Reload.Enabled = false;
                FileMenu_Open.Enabled = false;
                Cursor = Cursors.WaitCursor;
                //Update
                UpdateLayers();
                UpdateImage();
                //Reset Cursor
                Cursor = Cursors.Default;
                FileMenu_Open.Enabled = true;
                button_Preview.Enabled = true;
                button_Reload.Enabled = true;
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (filePath != null)
            {
                //Save to the original filePath
                saveToFile(filePath);
            }
            else
            {
                //Save to new File
                saveData.FileName = textbox_Name.Text;
                DialogResult result = saveData.ShowDialog(this);
                saveData.InitialDirectory = dataPath;

                if (result == DialogResult.OK) saveToFile(saveData.FileName);
            }
        }

        private void FileMenu_SaveAs_Click(object sender, EventArgs e)
        {
            //Save to new File
            saveData.FileName = textbox_Name.Text;
            DialogResult result = saveData.ShowDialog(this);

            if (result == DialogResult.OK) saveToFile(saveData.FileName);
        }

        private void button_SaveImage_Click(object sender, EventArgs e)
        {
            saveImage.FileName = textbox_Name.Text;

            DialogResult result = saveImage.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                ImageFormat format = ImageFormat.Bmp;

                switch (saveImage.FileName.Split('.').Last())
                {
                    case "png":
                        format = ImageFormat.Png;
                        break;
                    case "jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case "gif":
                        format = ImageFormat.Gif;
                        break;
                    case "bmp":
                        format = ImageFormat.Bmp;
                        break;
                }

                //Save the mainBItmap object
                mainBitmap.Save(saveImage.FileName, format);
            }
        }

        private void FileMenu_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_CardArt_Click(object sender, EventArgs e)
        {
            LoadCardArt cardartForm = new LoadCardArt(cardArtPath);
            try
            {
                if (cardartForm.ShowDialog(this) == DialogResult.OK)
                {
                    CardArt = (Bitmap)cardartForm.saveBitmap.Clone();

                    //Disable
                    group_Edit.Enabled = false;
                    Cursor = Cursors.WaitCursor;
                    //Update
                    UpdateLayers();
                    UpdateImage();
                    //Reset Cursor
                    Cursor = Cursors.Default;
                    group_Edit.Enabled = true;

                    button_CardArt.Focus();
                }
                cardartForm.saveBitmap.Dispose();
                cardartForm.Dispose();
            }
            catch (ObjectDisposedException) { }
        }

        private void EditMenu_Preferences_Click(object sender, EventArgs e)
        {
            //Load Preferences
            Preferences prefForm = new Preferences(cardArtPath, imagePath, dataPath, invertRespace);

            if (prefForm.ShowDialog(this) == DialogResult.OK)
            {
                cardArtPath = prefForm.textbox_CardArt.Text;
                imagePath = prefForm.textbox_Images.Text;
                dataPath = prefForm.textbox_Data.Text;
                invertRespace = prefForm.checkbox_Respace.Checked;

                saveImage.InitialDirectory = imagePath;
                openData.InitialDirectory = dataPath;
                saveData.InitialDirectory = dataPath;

                using (XmlWriter writer = XmlWriter.Create("Settings.xml"))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Settings");


                    //Path for Card Art
                    writer.WriteStartElement("CARDARTPATH");
                    writer.WriteString(cardArtPath);
                    writer.WriteFullEndElement();

                    //Path for Image Path
                    writer.WriteStartElement("IMAGEPATH");
                    writer.WriteString(imagePath);
                    writer.WriteFullEndElement();

                    //Path for Data Path
                    writer.WriteStartElement("DATAPATH");
                    writer.WriteString(dataPath);
                    writer.WriteFullEndElement();

                    //Boolean Value for Respacing
                    writer.WriteElementString("RESPACE", invertRespace.ToString());

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }

            prefForm.Dispose();
        }

        private void HelpMenu_About_Click(object sender, EventArgs e)
        {
            ShowAboutDialog();
        }


        private void HelpMenu_Help_Click(object sender, EventArgs e)
        {
            //Start Word Document
            Process.Start(appPath + "/Help.pdf");
        }

        public static Bitmap BorderFromResources(string border)
        {
            //Retrive the Correct Border from Resources
            switch (border)
            {
                case "Normal":
                    return Properties.Resources.Border_Normal;
                case "Trigger":
                    return Properties.Resources.Border_Trigger;
                case "Stride":
                    return Properties.Resources.Border_Stride;
                case "G-Guardian":
                    return Properties.Resources.Border_G_Guardian;
                default:
                    return null;
            }
        }

        private void button_Preview_Click(object sender, EventArgs e)
        {
            //Disable
            group_Edit.Enabled = false;
            Cursor = Cursors.WaitCursor;
            //Update
            UpdateLayers();
            UpdateImage();
            //Reset Cursor
            Cursor = Cursors.Default;
            group_Edit.Enabled = true;

            button_Preview.Focus();
        }

        private void combobox_Border_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Edit Usability based on Border
            if (combobox_Border.Text == "Trigger")
            {
                combobox_Trigger.Enabled = true;
                numeric_Power.Enabled = true;
            }
            else if (combobox_Border.Text == "G-Guardian")
            {
                combobox_Trigger.Enabled = false;
                numeric_Power.Enabled = false;
            }
            else
            {
                combobox_Trigger.Enabled = false;
                numeric_Power.Enabled = true;
            }
        }

        private void SymbolMenu_BrackLeft_Click(object sender, EventArgs e)
        {
            //Add in Special Bracket
            if (richtextbox_Effect.ContainsFocus) richtextbox_Effect.SelectedText = "【";
        }

        private void SymbolMenu_BrackRight_Click(object sender, EventArgs e)
        {
            //Add in Special Bracket
            if (richtextbox_Effect.ContainsFocus) richtextbox_Effect.SelectedText = "】";
        }
        private void SymbolMenu_TextIcon_Clicked(object sender, EventArgs e)
        {
            //Add in CB, CC, SB or SC Texts
            if (!richtextbox_Effect.ContainsFocus) return;
            ToolStripMenuItem itemSender = sender as ToolStripMenuItem;
            switch (itemSender.Text)
            {
                case "Counter Blast":
                    richtextbox_Effect.SelectedText = "_{CB()}_";
                    richtextbox_Effect.SelectionStart -= 3;
                    break;
                case "Counter Charge":
                    richtextbox_Effect.SelectedText = "_{CC()}_";
                    richtextbox_Effect.SelectionStart -= 3;
                    break;
                case "Soul Blast":
                    richtextbox_Effect.SelectedText = "_{SB()}_";
                    richtextbox_Effect.SelectionStart -= 3;
                    break;
                case "Soul Charge":
                    richtextbox_Effect.SelectedText = "_{SC()}_";
                    richtextbox_Effect.SelectionStart -= 3;
                    break;
            }
        }

        private void SymbolMenu_Item_Click(object sender, EventArgs e)
        {
            //Add in Custom Texts
            if (!richtextbox_Effect.ContainsFocus) return;
            ToolStripMenuItem itemSender = sender as ToolStripMenuItem;
            richtextbox_Effect.SelectedText = "_{" + itemSender.Text + "}_";
        }

        private void ExportMenu_FanonTable_Click(object sender, EventArgs e)
        {
            DialogResult nameResult = MessageBox.Show("Is your card's name already in usage?", "Card Name", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            //Start CardTable Template
            string cardTableExport = "{{CardTable" + Environment.NewLine;

            //Add in Trigger Parameter
            if (combobox_Border.Text == "Stride" || combobox_Border.Text == "G-Guardian") cardTableExport += "|trig = G" + Environment.NewLine;
            else if (combobox_Trigger.Text != "None" && combobox_Trigger.Enabled) cardTableExport += "|trig = " + combobox_Trigger.Text.Replace(" Trigger", "") + Environment.NewLine;

            //Choose Nation
            if (combobox_Nation.Text != "Cray Elemental" || combobox_Nation.Text != "Touken Ranbu" || combobox_Nation.Text != "None")
            {
                cardTableExport += "|nation = " + combobox_Nation.Text + Environment.NewLine;
            }

            cardTableExport += "|clan = " + textbox_Clan.Text + Environment.NewLine;

            //Only Write Name if there is already a card with the same name
            if (nameResult == DialogResult.Yes) cardTableExport += "|name = " + textbox_Name.Text + Environment.NewLine;

            //Image default uses .png and card name
            cardTableExport += "|image = " + textbox_Name.Text + ".png <!--You may need to change this to whatever the image is called on the website-->" + Environment.NewLine;

            //Grade
            cardTableExport += "|grade = " + numeric_Grade.Value + Environment.NewLine;

            //Skillicon
            if (combobox_Border.Text == "Stride") cardTableExport += "|skillicon = triple" + Environment.NewLine;
            else if (combobox_Border.Text == "G-Guardian") cardTableExport += "|skillicon = none" + Environment.NewLine;

            //G-Guardian determines Power && Crit
            if (combobox_Border.Text != "G-Guardian") cardTableExport += "|power = " + numeric_Power.Value + Environment.NewLine;
            else cardTableExport += "|critical = nil" + Environment.NewLine;

            //Shield
            if (combobox_Shield.Text != "None") cardTableExport += "|shield = " + combobox_Shield.Text.Replace(" Shield", "") + Environment.NewLine;

            //Race(s): If there are multiple races signified by '/' put in race and race2 parameters
            string[] races = textbox_Race.Text.Split('/');
            cardTableExport += "|race = " + races[0] + Environment.NewLine;
            if (races.Length > 1) cardTableExport += "|race2 = " + races[1] + Environment.NewLine;

            //Only Input if valid
            if (textbox_Illust.TextLength > 0) cardTableExport += "|illust = " + textbox_Illust.Text + Environment.NewLine;
            if (textbox_Design.TextLength > 0) cardTableExport += "|design = " + textbox_Design.Text + Environment.NewLine;
            if (textbox_Code.TextLength > 0) cardTableExport += "|set1 = " + textbox_Code.Text + Environment.NewLine;
            if (richtextbox_Flavour.TextLength > 0) cardTableExport += "|flavor = " + richtextbox_Flavour.Text + Environment.NewLine;
            if (checkbox_Effect.Checked) cardTableExport += "|effect = [Enter Effect Here]" + Environment.NewLine;

            //End Template
            cardTableExport += "}}";

            //Create Label
            LinkLabel visitHomePage = new LinkLabel();
            visitHomePage.AutoSize = true;
            visitHomePage.Location = new System.Drawing.Point(5, 5);
            visitHomePage.Name = "linkLabel1";
            visitHomePage.Size = new System.Drawing.Size(238, 13);
            visitHomePage.TabIndex = 0;
            visitHomePage.TabStop = true;
            visitHomePage.Text = "Visit the home page to learn how to create pages";
            visitHomePage.Click += new EventHandler(homepageLink_Clicked);

            DialogResult copy = ShowExportText("Card Table", visitHomePage, cardTableExport);
            if (copy == DialogResult.OK)
            {
                Clipboard.SetText(cardTableExport);
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    Process.Start("http://cardfightvanguardfanon.wikia.com/wiki/" + textbox_Name.Text);
                }
            }
        }

        private void ExportMenu_ReadableText_Click(object sender, EventArgs e)
        {

            //Name & Grade
            string text = textbox_Name.Text + Environment.NewLine +
                "Grade " + numeric_Grade.Value;

            //Trigger
            if (combobox_Trigger.Text != "None") text += "/" + combobox_Trigger.Text;
            text += Environment.NewLine;

            //Power
            if (combobox_Border.Text != "G-Guardian") text += "G-Guardian";
            else text += numeric_Power.Value.ToString();

            //Shield
            if (combobox_Shield.Text != "None") text += "/" + combobox_Shield.Text.Replace("None", "");
            text += Environment.NewLine;

            //Clan & Race
            text += textbox_Clan.Text + "/" + textbox_Race.Text + Environment.NewLine;

            //Effect
            text += "[Insert Your Effect Here]";

            Label label_Text = new Label();
            label_Text.Text = "Developers:";
            label_Text.Font = SystemFonts.DialogFont;
            label_Text.AutoSize = true;
            label_Text.Location = new System.Drawing.Point(5, 5);
            label_Text.Name = "label_Text";
            label_Text.Size = new System.Drawing.Size(238, 13);
            label_Text.TabIndex = 0;
            label_Text.TabStop = true;
            label_Text.Text = "Share your card's information in text format";

            DialogResult copy = ShowExportText("Readable Text", label_Text, text);
            if (copy==DialogResult.OK) Clipboard.SetText(text);
        }

        public static void homepageLink_Clicked(object sender, EventArgs e)
        {
            Process.Start("http://cardfightvanguardfanon.wikia.com/wiki/Cardfight!!_Vanguard_Fanon_Wiki");
        }

        private void button_Bold_Click(object sender, EventArgs e)
        {
            richtextbox_Effect.Focus();
            if (richtextbox_Effect.ContainsFocus)
            {
                if (richtextbox_Effect.SelectedText.Length == 0)
                {
                    int selectindex = richtextbox_Effect.SelectionStart;
                    richtextbox_Effect.SelectedText = "<b></b>";
                    richtextbox_Effect.SelectionStart = selectindex + 3;
                }
                else richtextbox_Effect.SelectedText = "<b>" + richtextbox_Effect.SelectedText + "</b>";
            }
        }

        private void button_Italics_Click(object sender, EventArgs e)
        {
            richtextbox_Effect.Focus();
            if (richtextbox_Effect.ContainsFocus)
            {
                if (richtextbox_Effect.SelectedText.Length == 0)
                {
                    int selectindex = richtextbox_Effect.SelectionStart;
                    richtextbox_Effect.SelectedText = "<i></i>";
                    richtextbox_Effect.SelectionStart = selectindex + 3;
                }
                else richtextbox_Effect.SelectedText = "<i>" + richtextbox_Effect.SelectedText + "</i>";
            }
        }

        private void button_Red_Click(object sender, EventArgs e)
        {
            richtextbox_Effect.Focus();
            if (richtextbox_Effect.ContainsFocus)
            {
                if (richtextbox_Effect.SelectedText.Length == 0)
                {
                    int selectindex = richtextbox_Effect.SelectionStart;
                    richtextbox_Effect.SelectedText = "<r></r>";
                    richtextbox_Effect.SelectionStart = selectindex + 3;
                }
                else richtextbox_Effect.SelectedText = "<r>" + richtextbox_Effect.SelectedText + "</r>";
            }
        }

        private void checkbox_Effect_CheckedChanged(object sender, EventArgs e)
        {
            //Disable/Enable Effect TextBox
            richtextbox_Effect.Enabled = checkbox_Effect.Checked;
        }

        private void richtextbox_Effect_TextChanged(object sender, EventArgs e)
        {
            //Replace all [[ and ]] with Special Brackets
            int selectedIndex = richtextbox_Effect.SelectionStart;
            int originalLength = richtextbox_Effect.TextLength;
            richtextbox_Effect.Text = richtextbox_Effect.Text.Replace("[[", "【");
            richtextbox_Effect.Text = richtextbox_Effect.Text.Replace("]]", "】");
            richtextbox_Effect.SelectionStart = selectedIndex - originalLength + richtextbox_Effect.TextLength;
        }

        private void TextboxMenu_Spacing_Click(object sender, EventArgs e)
        {
            if (!richtextbox_Effect.Focused) return;
            int sIndex = richtextbox_Effect.SelectionStart;
            int sLength = richtextbox_Effect.SelectionLength;

            char[] text = richtextbox_Effect.SelectedText.ToCharArray();

            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case ' ':
                        text[i] = '_';
                        break;
                    case '_':
                        text[i] = ' ';
                        break;
                }
            }

            richtextbox_Effect.SelectedText = new string(text);

            richtextbox_Effect.SelectionStart = sIndex;
            richtextbox_Effect.SelectionLength = sLength;
        }

        private void checkbox_SP_CheckedChanged(object sender, EventArgs e)
        {
            //Enable/Disable Flavour TextBox
            richtextbox_Flavour.Enabled = !checkbox_SP.Checked;
        }

        private void button_Reload_Click(object sender, EventArgs e)
        {
            //Re-open File from FilePath
            if (filePath == null)
            {
                MessageBox.Show("You haven't loaded a file.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            openFromFile();

            //Disable
            group_Edit.Enabled = false;
            Cursor = Cursors.WaitCursor;
            //Update
            UpdateLayers();
            UpdateImage();
            //Reset Cursor
            Cursor = Cursors.Default;
            group_Edit.Enabled = true;

            button_Reload.Focus();
        }

        public void openFromFile()
        {
            try
            {
                checkbox_SP.Checked = false;
                //Load File from filePath variable
                using (StreamReader reader = new StreamReader(filePath))
                {
                    //Text File into String
                    string textFile = reader.ReadToEnd();

                    //Each '|' represents a new parameter
                    string[] parameters = textFile.Split('|');
                    foreach (string parameter in parameters)
                    {
                        //'=' is the seperator between the key and value
                        string[] values = parameter.Split('=');
                        //Load based on Key
                        switch (values[0])
                        {
                            case "Code":
                                textbox_Code.Text = values[1];
                                break;
                            case "Name":
                                textbox_Name.Text = values[1];
                                break;
                            case "Border":
                                combobox_Border.Text = values[1];
                                break;
                            case "Trigger":
                                combobox_Trigger.Text = values[1];
                                break;
                            case "Grade":
                                //Convert to Decimal
                                numeric_Grade.Value = decimal.Parse(values[1]);
                                break;
                            case "Power":
                                //Convert to Decimal
                                numeric_Power.Value = decimal.Parse(values[1]);
                                break;
                            case "Shield":
                                combobox_Shield.Text = values[1];
                                break;
                            case "Clan":
                                textbox_Clan.Text = values[1];
                                break;
                            case "Nation":
                                combobox_Nation.Text = values[1];
                                break;
                            case "Race":
                                textbox_Race.Text = values[1];
                                break;
                            case "LegLead":
                                //If the value equals 1 then it's true. If otherwise, it's false.
                                checkbox_LegLeader.Checked = values[1] == "1";
                                break;
                            case "LegMate":
                                //If the value equals 1 then it's true. If otherwise, it's false.
                                checkbox_LegMate.Checked = values[1] == "1";
                                break;
                            case "Illust":
                                textbox_Illust.Text = values[1];
                                break;
                            case "Design":
                                textbox_Design.Text = values[1];
                                break;
                            case "Effect":
                                //Load complete text into textbox
                                richtextbox_Effect.Text = values[1];
                                checkbox_Effect.Checked = richtextbox_Effect.TextLength > 0;
                                break;
                            case "Flavour":
                                //Load complete flavour into textbox
                                richtextbox_Flavour.Text = values[1];
                                break;
                            case "Image":
                                //Convert string to Bitmap
                                ///Code by caesay, Omar Mahili & Peters
                                Image img = null;
                                byte[] bitmapBytes = Convert.FromBase64String(values[1]);
                                using (MemoryStream memoryStream = new MemoryStream(bitmapBytes))
                                {
                                    img = Image.FromStream(memoryStream);
                                }

                                //Load into CardArt Bitmap
                                CardArt = new Bitmap(img);
                                break;
                            case "SP":
                                //Set SP to True
                                if (values[1].ToCharArray()[0] == '1') checkbox_SP.Checked = true;
                                else checkbox_SP.Checked = false;
                                break;
                        }
                    }

                    //End Reading
                    reader.Close();
                }
            }
            catch { MessageBox.Show("There was an issue loading the file.", "", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void saveToFile(string file)
        {
            //Convert Bitmap to String
            ///Code by casesay, Omar Mahili & Peters
            string bitmapString = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                CardArt.Save(memoryStream, ImageFormat.Bmp);
                byte[] bitmapBytes = memoryStream.GetBuffer();
                bitmapString = Convert.ToBase64String(bitmapBytes, Base64FormattingOptions.InsertLineBreaks);

                memoryStream.Close();
            }

            //Save to text-like File
            using (StreamWriter writer = new StreamWriter(file, false))
            {
                //'|' represents a separate Parameter
                //'=' separates the Key and Value
                writer.Write("|Code=" + textbox_Code.Text);
                writer.Write("|Name=" + textbox_Name.Text);
                writer.Write("|Border=" + combobox_Border.Text);
                writer.Write("|Trigger=" + combobox_Trigger.Text);
                writer.Write("|Grade=" + numeric_Grade.Value);
                writer.Write("|Power=" + numeric_Power.Value);
                writer.Write("|Shield=" + combobox_Shield.Text);
                writer.Write("|Clan=" + textbox_Clan.Text);
                writer.Write("|Nation=" + combobox_Nation.Text);
                writer.Write("|Race=" + textbox_Race.Text);
                //Convert Boolean to Integer
                writer.Write("|LegLead=" + (checkbox_LegLeader.Checked ? 1 : 0));
                //Convert Boolean to Integer
                writer.Write("|LegMate=" + (checkbox_LegMate.Checked ? 1 : 0));
                writer.Write("|Illust=" + textbox_Illust.Text);
                writer.Write("|Design=" + textbox_Design.Text);
                writer.Write("|SP=" + (checkbox_SP.Checked ? 1 : 0));
                writer.Write("|Effect=" + (checkbox_Effect.Checked ? richtextbox_Effect.Text : ""));
                writer.Write("|Flavour=" + richtextbox_Flavour.Text);
                writer.Write("|Image=" + bitmapString);
                writer.Close();
            }

            filePath = file;
        }

        private void UpdateLayers()
        {
            //Clear old Layers
            Layers_Images.Clear();
            Layers_Text.Clear();
            //Add Card Art
            Layers_Images.Add(new ImageLayer(CardArt));

            //Add Border
            //Get Bitmap using BorderFromResources()
            Bitmap borderBitmap = BorderFromResources(combobox_Border.Text);
            if (borderBitmap != null)
            {
                Layers_Images.Add(new ImageLayer(borderBitmap, 0, 425));
            }

            //Legions
            if (checkbox_LegLeader.Checked) Layers_Images.Add(new ImageLayer(Properties.Resources.Legion_Leader, 0, 424));
            if (checkbox_LegMate.Checked) Layers_Images.Add(new ImageLayer(Properties.Resources.Legion_Mate, 321, 424));

            //Nation Images
            Tuple<Bitmap, Color, Color, Color> value = new Tuple<Bitmap, Color, Color, Color>(null, Color.White, Color.Black, Color.White);
            if (combobox_Nation.Text != "Touken Ranbu" && NationList.TryGetValue(combobox_Nation.Text, out value) && value.Item1 != null)
            {
                Layers_Images.Insert(1, new ImageLayer(Properties.Resources.Nation_Colour, 195, 459));
                Layers_Images[1].ChangeImageColour(value.Item2);
                Layers_Images.Add(new ImageLayer(value.Item1, 304, 460));
                Layers_Images.Add(new ImageLayer(Properties.Resources.Nation_Shader, 195, 459));

                //Load Gold Border only if there is a Nation
                if ((combobox_Border.Text == "G-Guardian" || combobox_Border.Text == "Stride"))
                {
                    Layers_Images.Add(new ImageLayer(Properties.Resources.G_Nation, 298, 455));
                }
            }
            else if (combobox_Nation.Text == "Touken Ranbu" && NationList.TryGetValue(combobox_Nation.Text, out value) && value.Item1 != null)
            {
                Layers_Images.Insert(1, new ImageLayer(Properties.Resources.Nation_TR, 195, 459));
                Layers_Images.Add(new ImageLayer(value.Item1, 304, 460));
            }
            else if (combobox_Nation.Text == "Cray Elemental")
            {
                Layers_Images.Insert(1, new ImageLayer(Properties.Resources.Nation_CE, 195, 459));
                Layers_Images.Add(new ImageLayer(Properties.Resources.Nation_Shader, 195, 459));
            }

            //Grade
            Bitmap gradeBitmap;
            switch ((int)numeric_Grade.Value)
            {
                case 1:
                    gradeBitmap = Properties.Resources.Grade_1;
                    break;
                case 2:
                    gradeBitmap = Properties.Resources.Grade_2;
                    break;
                case 3:
                    gradeBitmap = Properties.Resources.Grade_3;
                    break;
                case 4:
                    //Grade 4 depends on Border
                    if (combobox_Border.Text == "Stride") gradeBitmap = Properties.Resources.Grade_4_Stride;
                    else if (combobox_Border.Text == "G-Guardian") gradeBitmap = Properties.Resources.Grade_4_G_Guardian;
                    else gradeBitmap = Properties.Resources.Grade_4;
                    break;
                default:
                    gradeBitmap = Properties.Resources.Grade_0;
                    break;
            }

            Layers_Images.Add(new ImageLayer(gradeBitmap, 5, 8));


            //Trigger Icon
            if (combobox_Border.Text == "Trigger")
            {
                //Only accessible if Trigger is the Border
                switch (combobox_Trigger.Text)
                {
                    case "Critical Trigger":
                        Layers_Images.Add(new ImageLayer(Properties.Resources.Trigger_Critical, 280, 11));
                        break;
                    case "Draw Trigger":
                        Layers_Images.Add(new ImageLayer(Properties.Resources.Trigger_Draw, 280, 7));
                        break;
                    case "Stand Trigger":
                        Layers_Images.Add(new ImageLayer(Properties.Resources.Trigger_Stand, 280, 8));
                        break;
                    case "Heal Trigger":
                        Layers_Images.Add(new ImageLayer(Properties.Resources.Trigger_Heal, 280, 9));
                        break;
                }
            }

            //Shield Icon
            switch (combobox_Shield.Text)
            {
                case "0 Shield":
                    Layers_Images.Add(new ImageLayer(Properties.Resources.Shield_0, 0, 113));
                    break;
                case "5000 Shield":
                    Layers_Images.Add(new ImageLayer(Properties.Resources.Shield_5000, 0, 119));
                    break;
                case "10000 Shield":
                    Layers_Images.Add(new ImageLayer(Properties.Resources.Shield_10000, 0, 119));
                    break;
                case "15000 Shield":
                    Layers_Images.Add(new ImageLayer(Properties.Resources.Shield_15000, 0, 115));
                    break;
            }

            //Name of Card
            if (textbox_Name.Text.Length != 0)
            {
                if (combobox_Border.Text == "Stride" || combobox_Border.Text == "G-Guardian") Layers_Text.Add(new TextLayer(textbox_Name.Text, new Font(Impact, 17.91f, FontStyle.Italic), Color.White, 192, 431, 219, StringAlignment.Center));
                else Layers_Text.Add(new TextLayer(textbox_Name.Text, new Font(Impact, 17.91f, FontStyle.Italic), Color.Black, 192, 431, 219, StringAlignment.Center));
            }

            //Power of Card
            ///There is no value for a G-Guardian
            if (combobox_Border.Text == "Stride")
            {
                //Add in a '+' sign
                Layers_Text.Add(new TextLayer(numeric_Power.Value + "+", new Font(Impact, 25.3f, FontStyle.Italic),
                    Color.FromArgb(201, 187, 55), 90, 456, StringAlignment.Center, new Pen(Color.Black, 4)));
            }
            else if (combobox_Border.Text != "G-Guardian")
            {
                //As long as it's not a G-Guardian Border
                Layers_Text.Add(new TextLayer(numeric_Power.Value.ToString(), new Font(Impact, 25.3f, FontStyle.Italic),
                    Color.FromArgb(201, 187, 55), 90, 455, StringAlignment.Center, new Pen(Color.Black, 4)));
            }

            //Clan of Card
            ///Clan only works if Nation exists
            if (textbox_Clan.Text.Length != 0 && combobox_Nation.Text != "None")
            {
                Tuple<Bitmap, Color, Color, Color> outTuple = new Tuple<Bitmap, Color, Color, Color>(Properties.Resources.Nation_Colour, Color.White, Color.Black, Color.White);
                NationList.TryGetValue(combobox_Nation.Text, out outTuple);

                Color outlineColour = outTuple.Item4;

                Layers_Text.Add(new TextLayer(textbox_Clan.Text, new Font(Optima, 10.44f, FontStyle.Regular), outTuple.Item3, 244, 460, 78, StringAlignment.Center, new Pen(outlineColour, 2f)));
            }

            //Race of Card
            if (textbox_Race.Text.Length != 0)
            {
                Layers_Text.Add(new TextLayer(textbox_Race.Text, new Font(Optima_Thin, 10.85f, FontStyle.Regular), Color.White, 245, 475, 78, StringAlignment.Center, new Pen(Color.FromArgb(125, 0, 0, 0), 1f)));
            }

            //Booster Set Value + Illustrator/Design Colour and Pen
            Color bottomColour = Color.White;
            Pen bottomOutline = new Pen(Color.FromArgb(125, 255, 255, 255));

            //if it's trigger make it black if not stays white.
            if (combobox_Border.Text == "Trigger")
            {
                bottomColour = Color.Black;
                bottomOutline = new Pen(Color.FromArgb(125, 0, 0, 0));
            }

            //Illustrator + Designer
            ///Combine both Illustrator TextBox and Designer Textbox into 1 string
            string illustratText = "";
            if (textbox_Design.Text.Length > 0)
            {
                illustratText += "DESIGN / " + textbox_Design.Text;
            }
            if (textbox_Illust.Text.Length > 0)
            {
                illustratText += " ILLUST / " + textbox_Illust.Text;
            }

            if (illustratText.Length > 0)
            {
                Layers_Text.Add(new TextLayer(illustratText, new Font(Optima_Thin, 9.15f, FontStyle.Regular), bottomColour, 318, 492, 150, StringAlignment.Far, (Pen)bottomOutline.Clone()));
            }

            if (textbox_Code.Text.Length > 0)
            {
                Layers_Text.Add(new TextLayer(textbox_Code.Text, new Font(Optima_Thin, 9.15f, FontStyle.Bold), bottomColour, 22, 492, 150, StringAlignment.Near, (Pen)bottomOutline));
            }

            //Effect of Card
            if (checkbox_Effect.Checked && richtextbox_Effect.Text.Length > 0)
            {
                //Determne Word Splitter
                char[] splitChars = invertRespace ? new char[2] { '_', ' ' } : new char[2] { ' ', '_' };


                if (checkbox_SP.Checked && richtextbox_Effect.Lines.Length > 0)
                {
                    Layers_Effect = new EffectLayer(richtextbox_Effect.Text, Optima, 419 - 15 * richtextbox_Effect.Lines.Length, effectReplacements, specialReplacements, splitChars, 1.5f);
                }
                else
                {
                    //Create Effect Box
                    Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_Bottom, 8, 405));
                    //Create based on number of lines
                    ///All Images = Height: 17
                    ///Text = 10 Pixels down from the top
                    switch (richtextbox_Effect.Lines.Length)
                    {
                        case 0:
                            //No Images
                            break;
                        case 1:
                            //Only Top and Bottom Images
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_Top, 8, 388));
                            //Insert Effect Text
                            Layers_Effect = new EffectLayer(richtextbox_Effect.Text, Optima, 398, effectReplacements, specialReplacements, splitChars);
                            break;
                        case 2:
                            //Top, LineCut and Bottom Images
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_LineCut, 8, 390));
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_Top, 8, 373));
                            //Insert Effect Text
                            Layers_Effect = new EffectLayer(richtextbox_Effect.Text, Optima, 383, effectReplacements, specialReplacements, splitChars);
                            break;
                        case 3:
                            //Top, 2 LineCut and Bottom Images
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_LineCut, 8, 390));
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_LineCut, 8, 375));
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_Top, 8, 358));
                            //Insert Effect Text
                            Layers_Effect = new EffectLayer(richtextbox_Effect.Text, Optima, 368, effectReplacements, specialReplacements, splitChars);
                            break;
                        case 4:
                            //Top, 2 LineCut, 1 Lines and Bottom Images
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_LineCut, 8, 390));
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_Lines, 8, 375));
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_LineCut, 8, 360));
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_Top, 8, 343));
                            //Insert Effect Text
                            Layers_Effect = new EffectLayer(richtextbox_Effect.Text, Optima, 353, effectReplacements, specialReplacements, splitChars);
                            break;
                        default:
                            //Top, 2 LineCut, X Lines and Bottom Images
                            double extralines = richtextbox_Effect.Lines.Length - 3;
                            //Spawns at the bottom
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_LineCut, 8, 390));
                            //"Stretch" Lines by making multiple images
                            for (int i = 0; i < Math.Ceiling(extralines / 2); i++)
                            {
                                Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_Lines, 8, 390 - (int)((Math.Ceiling(extralines / 2) - i) * EffectLayer.TEXTHEIGHT)));
                            }
                            //Middle of Effect Box
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_LineCut, 8, 375 - (int)(Math.Ceiling(extralines / 2) * EffectLayer.TEXTHEIGHT)));
                            //"Stretch" Lines by Making multiple Images
                            for (int i = 0; i < Math.Floor(extralines / 2); i++)
                            {
                                Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_Lines, 8, 375 - (int)((extralines - i) * EffectLayer.TEXTHEIGHT)));
                            }
                            //Top of Effect Box
                            Layers_Images.Add(new ImageLayer(Properties.Resources.EffectBox_Top, 8, 358 - (int)(extralines * EffectLayer.TEXTHEIGHT)));
                            //Insert Effect Text
                            Layers_Effect = new EffectLayer(richtextbox_Effect.Text, Optima, 368 - (int)(extralines * EffectLayer.TEXTHEIGHT), effectReplacements, specialReplacements, splitChars);
                            break;
                    }
                }
            }
            else Layers_Effect = new EffectLayer();//EffectLayer is empty

            //Flavour Text
            if (!checkbox_SP.Checked && richtextbox_Flavour.TextLength > 0)
            {
                //Get Number of Lines
                int flavourCount = richtextbox_Flavour.Lines.Length;
                int effectCount = checkbox_Effect.Checked ? richtextbox_Effect.Lines.Length : 0;

                //Width of the Text
                float width = 0;
                for (int i = 0; i < flavourCount; i++)
                {
                    //Get Line
                    string line = richtextbox_Flavour.Lines[i];
                    //Make sure Line isn't Empty
                    if (line.Length > 0)
                    {
                        int y = 0;
                        //Get Y Position based on effectCount & flavourCount
                        if (effectCount == 0) y = 412 - 16 * flavourCount;
                        else y = 397 - 16 * flavourCount - (int)EffectLayer.TEXTHEIGHT * effectCount + (int)(EffectLayer.TEXTHEIGHT - 17) * (effectCount < 2 ? 1 : 2);

                        //Initialize TextLayer
                        TextLayer text = new TextLayer(line, new Font(MatrixBook, 13), Color.White, 174f, y + 15 * i, StringAlignment.Center, new Pen(Color.FromArgb(0, 0, 0), 1f));

                        //Add to List
                        Layers_Text.Add(text);

                        if (text.image.Width > width) width = text.image.Width;
                    }
                }

                width += 20;

                //Determine Y Position based on effectCount
                if (effectCount == 0)
                {
                    Layers_Images.Add(new ImageLayer(new Bitmap(Properties.Resources.Flavour_Box, new Size((int)width, 4 + 16 * flavourCount)), (int)(174.5 - width / 2), 410 - 16 * flavourCount));
                }
                else
                {
                    Layers_Images.Add(new ImageLayer(new Bitmap(Properties.Resources.Flavour_Box, new Size((int)width, 4 + 16 * flavourCount)), (int)(174.5 - width / 2), 395 - 16 * flavourCount - 15 * effectCount - 2 * (effectCount < 2 ? 1 : 2)));
                }
            }
        }

        private void UpdateImage()
        {
            //Draw other images onto the main Bitmap
            mainBitmap = new Bitmap(349, 508);
            mainBitmap.MakeTransparent();
            //Take Graphics from mainBitmap
            using (Graphics g = Graphics.FromImage(mainBitmap))
            {
                //Draw onto main Bitmap based on the class' .DrawImage function
                ///Text Layer is always on top of Image Layer
                ///Effect Layer is always the last Layer on top.
                foreach (ImageLayer imgLay in Layers_Images)
                {
                    imgLay.DrawImage(g);
                }
                foreach (TextLayer imgLay in Layers_Text)
                {
                    imgLay.DrawImage(g);
                }
                Layers_Effect.DrawImage(g);
                //Make sure Art doesn't Cover Corners
                g.DrawImageUnscaled(Properties.Resources.Corners, 0, 0);
                g.Dispose();
            }
            //

            //update the PictureBox image
            picBox_CardImage.Image = mainBitmap;
        }


        //About Dialog
        private static DialogResult ShowAboutDialog()
        {
            Size size = new Size(500, 180);
            Form About = new Form();

            About.FormBorderStyle = FormBorderStyle.FixedDialog;
            About.ClientSize = size;
            About.Text = "About";

            Label DeveloperLabel = new Label();
            DeveloperLabel.Text = "Developers:";
            DeveloperLabel.Font = SystemFonts.DialogFont;
            DeveloperLabel.Location = new Point((size.Width / 2) - (DeveloperLabel.Size.Width / 3), 5);
            About.Controls.Add(DeveloperLabel);

            RichTextBox Developers = new RichTextBox();
            Developers.ReadOnly = true;
            Developers.BackColor = Color.White;
            Developers.Size = new Size(size.Width - 10, 140 - DeveloperLabel.Size.Height);
            Developers.Location = new Point(5, 5 + DeveloperLabel.Size.Height);
            Developers.Text = "Original Author" + Environment.NewLine + "Eronan"
                + Environment.NewLine + Environment.NewLine + "Images"
                + Environment.NewLine + "Eronan" + Environment.NewLine + "Mooshra12"
                + Environment.NewLine + Environment.NewLine + "Card Game"
                + Environment.NewLine + "Bushiroad";

            Developers.SelectAll();
            Developers.SelectionAlignment = HorizontalAlignment.Center;
            About.Controls.Add(Developers);

            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new Point((size.Width / 2) - (okButton.Size.Width / 2), 147);
            About.Controls.Add(okButton);

            About.AcceptButton = okButton;

            DialogResult result = About.ShowDialog();
            return result;
        }

        //About Dialog
        private static DialogResult ShowExportText(string name, LinkLabel label, string text)
        {
            Size size = new Size(500, 400);
            Form About = new Form();

            About.FormBorderStyle = FormBorderStyle.FixedDialog;
            About.ClientSize = size;
            About.Text = name;

            About.Controls.Add(label);

            RichTextBox TextBox = new RichTextBox();
            TextBox.ReadOnly = true;
            TextBox.BackColor = Color.White;
            TextBox.Size = new Size(size.Width - 10, 342);
            TextBox.Location = new Point(5, 23);
            TextBox.Text = text;
            TextBox.WordWrap = false;

            About.Controls.Add(TextBox);

            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.Text = "Copy";
            okButton.Location = new Point((size.Width / 2) - (okButton.Size.Width / 2), 367);
            About.Controls.Add(okButton);

            About.AcceptButton = okButton;

            DialogResult result = About.ShowDialog();
            return result;
        }

        private static DialogResult ShowExportText(string name, Label label, string text)
        {
            Size size = new Size(500, 400);
            Form About = new Form();

            About.FormBorderStyle = FormBorderStyle.FixedDialog;
            About.ClientSize = size;
            About.Text = name;

            //Add Label
            About.Controls.Add(label);

            RichTextBox TextBox = new RichTextBox();
            TextBox.ReadOnly = true;
            TextBox.BackColor = Color.White;
            TextBox.Size = new Size(size.Width - 10, 342);
            TextBox.Location = new Point(5, 23);
            TextBox.Text = text;
            TextBox.WordWrap = false;

            About.Controls.Add(TextBox);

            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.Text = "Copy";
            okButton.Location = new Point((size.Width / 2) - (okButton.Size.Width / 2), 367);
            About.Controls.Add(okButton);

            About.AcceptButton = okButton;

            DialogResult result = About.ShowDialog();
            return result;
        }
    }
}
using System;
using System.Windows.Forms;

namespace CFVG_Card_Creator
{
    //Set Preferences
    public partial class Preferences : Form
    {

        //Initializer
        public Preferences(string cardArtPath, string imagePath, string dataPath, bool invertSpace)
        {
            InitializeComponent();

            //Initialize Settings
            textbox_CardArt.Text = cardArtPath;
            textbox_Images.Text = imagePath;
            textbox_Data.Text = dataPath;
            checkbox_Respace.Checked = invertSpace;
        }

        //Load Card Art Locations
        private void button_CardArt_Click(object sender, EventArgs e)
        {
            DialogResult result = ImageBrowser.ShowDialog(this);

            if (result != DialogResult.Cancel)
            {
                textbox_CardArt.Text = ImageBrowser.SelectedPath;
            }
        }

        //Save Images Location
        private void button_Images_Click(object sender, EventArgs e)
        {
            DialogResult result = ImageBrowser.ShowDialog(this);

            if (result != DialogResult.Cancel)
            {
                textbox_Images.Text = ImageBrowser.SelectedPath;
            }
        }

        //Save Data Locations
        private void button_Data_Click(object sender, EventArgs e)
        {
            DialogResult result = DataBrowser.ShowDialog(this);

            if (result != DialogResult.Cancel)
            {
                textbox_Data.Text = DataBrowser.SelectedPath;
            }
        }

        //Set to Default
        private void button_Reset_Click(object sender, EventArgs e)
        {
            textbox_CardArt.Text = "";
            textbox_Data.Text = "";
            textbox_Images.Text = "";
            checkbox_Respace.Checked = false;
        }

    }
}

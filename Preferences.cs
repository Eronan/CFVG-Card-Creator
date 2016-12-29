using System;
using System.Windows.Forms;

namespace CFVG_Card_Creator
{
    //Set Preferences
    public partial class Preferences : Form
    {
        public string cardArtLocation;
        public string imageLocation;
        public string dataLocation;

        //Initializer
        public Preferences(string cardArtPath, string imagePath, string dataPath)
        {
            InitializeComponent();

            textbox_Images.Text = imagePath;
            textbox_Data.Text = dataPath;

            imageLocation = imagePath;
            dataLocation = dataPath;
        }

        //Load Card Art Locations
        private void button_CardArt_Click(object sender, EventArgs e)
        {
            DialogResult result = ImageBrowser.ShowDialog(this);

            if (result != DialogResult.Cancel)
            {
                textbox_CardArt.Text = ImageBrowser.SelectedPath;
                cardArtLocation = ImageBrowser.SelectedPath;
            }
        }

        //Save Images Location
        private void button_Images_Click(object sender, EventArgs e)
        {
            DialogResult result = ImageBrowser.ShowDialog(this);

            if (result != DialogResult.Cancel)
            {
                textbox_Images.Text = ImageBrowser.SelectedPath;
                imageLocation = ImageBrowser.SelectedPath;
            }
        }

        //Save Data Locations
        private void button_Data_Click(object sender, EventArgs e)
        {
            DialogResult result = DataBrowser.ShowDialog(this);

            if (result != DialogResult.Cancel)
            {
                textbox_Data.Text = DataBrowser.SelectedPath;
                dataLocation = DataBrowser.SelectedPath;
            }
        }

        //Set to Default
        private void button_Reset_Click(object sender, EventArgs e)
        {
            textbox_CardArt.Text = "";
            textbox_Data.Text = "";
            textbox_Images.Text = "";
        }

    }
}

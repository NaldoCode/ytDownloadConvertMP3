using System;
using System.IO;
using VideoLibrary;
using MediaToolkit;
using MediaToolkit.Model;
using System.Windows.Forms;

namespace ytDownloadConvertMP3
{
    public partial class FrmDownloadYouTube : Form
    {
        public FrmDownloadYouTube()
        {
            InitializeComponent();
        }

        int x;
        int y;
        bool mDown;
        private void btnClose_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        #region Move Panel :)
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            mDown = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mDown)
                SetDesktopLocation(MousePosition.X - x, MousePosition.Y - y);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mDown = false;
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Opacity > 0)
            {
                Opacity -= 0.090;
            }
            else
            {
                Close();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (txtURL.Text.Length > 10)
            {
                lblLoading.Text = "Loading...wait!";

                string userName = Environment.UserName;
                var path = $@"C:\Users\{userName}\Videos\"; // -.-
                var youTube = YouTube.Default;

                var video = youTube.GetVideo(txtURL.Text);
                File.WriteAllBytes(path + video.FullName, video.GetBytes());

                var inputFile = new MediaFile { Filename = path + video.FullName };
                var outputFile = new MediaFile { Filename = $"{path + video.FullName}.mp3" };

                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);
                    engine.Convert(inputFile, outputFile);
                }

                lblLoading.Text = "Download Complete.";

                if (cBoxDelete.Checked == true)
                {
                    File.Delete(path + video.FullName);
                    lblLoading.Text = "Download Complete. | [Deleted Vídeo]";
                }
            }
            else
            {
                lblLoading.Text = "Enter the URL!";
            }
        }
        private void FrmDownloadYouTube_Load(object sender, EventArgs e)
        {
            txtURL.Select();
        }
    }
}

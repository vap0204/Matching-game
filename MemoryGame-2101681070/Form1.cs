using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryGame_2101681070
{
    public partial class Form1 : Form
    {
        bool allowClick = false;
        PictureBox firstGuess;
        Random rnd = new Random();
        Timer clickTimer = new Timer();
        int time = 60;
        Timer timer = new Timer { Interval = 1000 };
        public Form1()
        {
            InitializeComponent();
            
        }
        private PictureBox[] pictureBoxes
        {
            get { return Controls.OfType<PictureBox>().ToArray(); }
        }
        private static IEnumerable<Image> images
        {
            get
            {
                return new Image[]
                {
                     Properties.Resources.img1,
                     Properties.Resources.img2,
                     Properties.Resources.img3,
                     Properties.Resources.img4,
                     Properties.Resources.img5,
                     Properties.Resources.img6,
                     Properties.Resources.img7,
                      Properties.Resources.img8
                };
            }

        }
        private void startGameTimer()
        {
            timer.Start();
            timer.Tick += delegate
            {
                time--;
                if (time<0)
                {
                    timer.Stop();
                    MessageBox.Show("Out of time");
                    ResetImages();
                }
                var ssTime = TimeSpan.FromSeconds(time);
                label1.Text = "00:" + time.ToString();
            };
        }
        private void ResetImages()
        {
            foreach(var pic in pictureBoxes)
            {
                pic.Tag = null;
                pic.Visible = true;
            }
            HideImages();
            setRandomImages();
            time = 60;
            timer.Start();
        }
        private void HideImages()
        {
            foreach(var pic in pictureBoxes)
            {
                pic.Tag = Properties.Resources.back;
            }
        }
        private PictureBox getFreeSlot()
        {
            int num;
            do
            {
                num = rnd.Next(0, pictureBoxes.Count());
            }
            while (pictureBoxes[num].Tag != null);
            return pictureBoxes[num];
        }
        private void setRandomImages()
        {
            foreach(var image in images)
            {
                getFreeSlot().Tag = image;
                getFreeSlot().Tag = image;

            }
        }
        private void CLICKTIMER_TIMER(object sender,EventArgs e)
        {
            HideImages();
            allowClick=true;
            clickTimer.Stop();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            allowClick = true;
            setRandomImages();
            HideImages();
            startGameTimer();
            clickTimer.Interval = 1000;
            clickTimer.Tick += CLICKTIMER_TIMER;
            button1.Enabled = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!allowClick) return;
            var pic = (PictureBox)sender;
            if (firstGuess==null)
            {
                firstGuess = pic;
                pic.Image = (Image)pic.Tag;
                return;
            }
            pic.Image = (Image)pic.Tag;
            if (pic.Image==firstGuess.Image && pic!=firstGuess)
            {
                pic.Visible = firstGuess.Visible = false;
                {
                    firstGuess = pic;
                }
                HideImages();
            }
            else
            {
                allowClick = false;
                clickTimer.Start();
            }
            firstGuess = null;
            if (pictureBoxes.Any(p => p.Visible)) return;
            MessageBox.Show("You win now try again");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

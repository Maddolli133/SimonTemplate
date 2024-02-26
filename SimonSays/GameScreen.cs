using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Drawing.Drawing2D;
using System.Threading;
using System.IO;

namespace SimonSays
{
    public partial class GameScreen : UserControl
    {
        public static int currentPatternIndex = 0;
        private List<int> availableColors = new List<int>() { 0, 1, 2, 3 }; // List of available colors
        public static List<int> currentPattern = new List<int>(); // List to store the current pattern
        private int currentLevel = 1;
        SoundPlayer blue = new SoundPlayer(Properties.Resources.blue);
        SoundPlayer green = new SoundPlayer(Properties.Resources.green);
        SoundPlayer red = new SoundPlayer(Properties.Resources.red);
        SoundPlayer yellow = new SoundPlayer(Properties.Resources.yellow);
        SoundPlayer oops = new SoundPlayer(Properties.Resources.mistake);
        System.Windows.Media.MediaPlayer backMedia = new System.Windows.Media.MediaPlayer();
       
        public GameScreen()
        {
            InitializeComponent();
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {

            GraphicsPath circlePath = new GraphicsPath();

            circlePath.AddArc(5, 5, 200, 200, 180, 90);
            circlePath.AddArc(70, 70, 70, 70, 270, -90);

            greenButton.Region = new Region(circlePath);
            Region buttonRegion = new Region(circlePath);

            greenButton.Region = buttonRegion;


            //rotate the orientation of the screen by 90 degrees 

            Matrix transformMatrix = new Matrix();

            transformMatrix.RotateAt(90, new PointF(55, 55));
            buttonRegion.Transform(transformMatrix);
            redButton.Region = buttonRegion;

            transformMatrix.RotateAt(360, new PointF(55, 55));
            buttonRegion.Transform(transformMatrix);
            blueButton.Region = buttonRegion;

            transformMatrix.RotateAt(360, new PointF(55, 55));
            buttonRegion.Transform(transformMatrix);
            yellowButton.Region = buttonRegion;

            backMedia.Open(new Uri(Application.StartupPath + "/Resources/gaga.mp3"));
            backMedia.Play();

            StartGame();
        }
        private void StartGame()
        {
            backMedia.Play();

            currentPatternIndex = 0;
            currentPattern.Clear();
            currentLevel = 1;
            availableColors = new List<int>() { 0, 1, 2, 3 }; // Reset available colors
            GeneratePattern(currentLevel);
            HighlightPattern(0);
        }

        private void GeneratePattern(int level)
        {
            Random random = new Random();

            // Select one random color from the available colors list
            int randGen = random.Next(0, availableColors.Count);
            currentPattern.Add(availableColors[randGen]);

            // Adjust the level if it exceeds the number of available colors
            if (level > availableColors.Count)
            {
                level = availableColors.Count;
            }
        }

        private void HighlightPattern(int index)
        {
            if (index >= currentPattern.Count)
            {
                currentPatternIndex = 0;
                return;
            }

            switch (currentPattern[index])
            {
                case 0:
                    greenButton.BackColor = Color.LawnGreen;
                    green.Play();
                    break;
                case 1:
                    redButton.BackColor = Color.Red;
                    red.Play();
                    break;
                case 2:
                    blueButton.BackColor = Color.Blue;
                    blue.Play();
                    break;
                case 3:
                    yellowButton.BackColor = Color.Yellow;
                    yellow.Play();
                    break;
            }

            Refresh();
            Thread.Sleep(1000);                      //this is being weird
            ResetButtonColors();

            HighlightPattern(index + 1);
        }
        //Add sound to each highlighted color
        private void greenButton_Click(object sender, EventArgs e)
        {
            CheckButton(0);
        }

        private void redButton_Click(object sender, EventArgs e)
        {
            CheckButton(1);
        }

        private void blueButton_Click(object sender, EventArgs e)
        {
            CheckButton(2);
        }

        private void yellowButton_Click(object sender, EventArgs e)
        {
            CheckButton(3);
        }

        private void CheckButton(int buttonIndex)
        {
            if (currentPattern[currentPatternIndex] == buttonIndex)
            {
                HighlightButton(buttonIndex); // Highlight the button

                currentPatternIndex++;

                if (currentPatternIndex == currentPattern.Count)
                {
                    currentLevel++;
                    GeneratePattern(currentLevel); // Generate next pattern with one more color
                    HighlightPattern(0);
                }
            }
            else
            {
                GameOver();
                oops.Play();
            }
        }

        private void HighlightButton(int buttonIndex)
        {
            //Add sound to each click
            switch (buttonIndex)
            {
                case 0:
                    greenButton.BackColor = Color.PapayaWhip;
                    break;
                case 1:
                    redButton.BackColor = Color.PapayaWhip;
                    break;
                case 2:
                    blueButton.BackColor = Color.PapayaWhip;
                    break;
                case 3:
                    yellowButton.BackColor = Color.PapayaWhip;
                    break;
            }
            Refresh();
            Thread.Sleep(100);
            ResetButtonColors();
        }

        private void ResetButtonColors()
        {
            greenButton.BackColor = Color.ForestGreen;
            redButton.BackColor = Color.DarkRed;
            blueButton.BackColor = Color.DarkBlue;
            yellowButton.BackColor = Color.Goldenrod;
        }
        public void GameOver()
        {
            //TODO: Play a game over sound
            backMedia.Stop();
            //TODO: close this screen and open the GameOverScreen
            Form1.ChangeScreen(this, new GameOverScreen());
        }

   
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Rex_Endless_Runner
{
    public partial class Form1 : Form
    {
        bool jumping = false; // boolean to check if player is jumping or not
        int jumpSpeed; // integer to set jump speed
        int force = 12; // force of the jump as an integer
        int score = 0; // default score integer set to 0
        int obstacleSpeed = 10; // the default speed for the obstacles
        Random rand = new Random(); /* create a new random class.
                                     This random number generator called rand will be used to
                                     calculate a random location for the obstacles to spawn once the game starts
                                     and when the reach the far left of the screen.*/
        int position;
        bool isGameOver = false;
        
        public Form1()
        {
            InitializeComponent();

            GameReset(); // run the reset game function
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            // linking the jumpspeed integer with the player picture boxes to location
            trex.Top += jumpSpeed;

            // show the score on the score text label
            txtScore.Text = "score: " + score;

            // if jumping is true and force is less than 0
            // then change jumping to false
            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            // if jumping is true
            // then change jump speed to -12 
            // reduce force by 1
            if (jumping == true)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                // else change the jump speed to 12
                jumpSpeed = 12;
            }

            if (trex.Top > 366 & jumping == false)
            {
                force = 12;
                trex.Top = 367;
                jumpSpeed = 0;
            }


            foreach (Control x in this.Controls)
            {
                // is X is a picture box and it has a tag of obstacle
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left -= obstacleSpeed; // move the obstacles towards the left

                    if (x.Left < -100)
                    {
                        // if the obstacles have gone off the screen
                        // then we respawn it off the far right
                        // in this case we are calculating the form width and a random number between 200 and 800
                        x.Left = this.ClientSize.Width + rand.Next(200, 500) + (x.Width * 15);
                        // we will add one to the score
                        score++;
                    }

                    // if the t rex collides with the obstacles
                    if (trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        // we stop the timer
                        gameTimer.Stop();
                        // change the t rex image to the dead one
                        trex.Image = Properties.Resources.dead;
                        // show press r to restart on the score text label
                        txtScore.Text += " Press R to restart the game!";
                        isGameOver = true;
                    }
                }
            }

            // if score is equal or greater than 10
            if (score > 10)
            {

                // the obstacle speed change to 15
                obstacleSpeed = 15;
            }

        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            //if the player pressed the space key and jumping boolean is false
            // then we set jumping to true
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            //when the keys are released we check if jumping is true
            // if so we need to set it back to false so the player can jump again
            if (jumping == true)
            {
                jumping = false;
            }

            // if the R key is pressed and released then we run the reset function
            if (e.KeyCode == Keys.R && isGameOver == true)
            {
                // run the reset game function
                GameReset();
            }
        }

        private void GameReset()
        {
            // This is the reset function
            force = 12; // set the force to 12
            jumpSpeed = 0; // set the jump speed to 0
            jumping = false;// change jumping to false
            score = 0; // set score to 0
            obstacleSpeed = 10; // set obstacle speed back to 10
            txtScore.Text = "Score: " + score; // change the score text to just show the score
            trex.Image = Properties.Resources.running; // change the t rex image to running
            isGameOver = false;
            trex.Top = 367;

            foreach (Control x in this.Controls)
            {
                // if X is a picture box and it has a tag of obstacle
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    // generate a random number in the position integer between 500 and 800
                    position = this.ClientSize.Width + rand.Next(500, 800) + (x.Width * 10);

                    // change the obstacles position to left for begining of the game
                    x.Left = position;
                }
            }

            gameTimer.Start(); // start the timer
        }
    }
}

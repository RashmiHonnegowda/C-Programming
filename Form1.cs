using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            new Settings();

            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += new EventHandler (UpdateScreen);
            gameTimer.Start();

            StartGame();

        }


        private void UpdateScreen(Object sender, EventArgs e)
        {
            if(Settings.GameOver == true)
            {
                if(Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }
            pbCanvas.Invalidate();
        }

        private void MovePlayer()
        {
            for(int i=snake.Count-1; i>=0;i--)
            {
                if(i==0)
                {
                    switch(Settings.direction)
                    {
                        case Direction.Right:
                            snake[i].X++;
                            break;
                        case Direction.Left:
                            snake[i].X--;
                            break;
                        case Direction.Up:
                            snake[i].Y--;
                            break;
                        case Direction.Down:
                            snake[i].Y++;
                            break;
                    }

                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYpos = pbCanvas.Size.Height / Settings.Height;

                    if(snake[i].X <0 || snake[i].Y < 0 || snake[i].X >= maxXpos || snake[i].Y >= maxYpos)
                    {
                        Die();
                    }

                    for(int j=1;j<snake.Count;j++)
                    {
                        if(snake[i].X == snake[j].X && snake[i].Y == snake[j].Y)
                        {
                            Die();
                        }
                    }

                    if(snake[0].X == food.X && snake[0].Y == food.Y)
                    {
                        Eat();
                    }
                }
                else
                {
                    snake[i].X = snake[i - 1].X;
                    snake[i].Y = snake[i - 1].Y;

                }

        
            }
        }

        private void Eat()
        {
            Circle food = new Circle();
            food.X = snake[snake.Count - 1].X;
            food.Y = snake[snake.Count - 1].Y;

            snake.Add(food);

            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void StartGame()
        {
            new Settings();
            lblGameOver.Visible = false;
            snake.Clear();
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            snake.Add(head);

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);

        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Brush snakecolour;
            if(!Settings.GameOver)
            {
                for(int i=0;i<snake.Count;i++)
                {
                    if(i==0)
                    {
                        snakecolour = Brushes.Black;
                    }
                    else
                    {
                        snakecolour = Brushes.Green;
                    }

                    canvas.FillEllipse(snakecolour,
                          new Rectangle(snake[i].X * Settings.Width,
                                        snake[i].Y * Settings.Height,
                                        Settings.Width, Settings.Height));
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                                      food.Y * Settings.Height, Settings.Width, Settings.Height));


                }
            }
            else
            {
                string gameOver = "Game Over \n Your final score is : " + Settings.Score + "\n Press Enter Key to try again !";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }
    }
}

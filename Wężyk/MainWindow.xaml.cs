using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Wężyk
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitBoard();
            InitSnake();
            InitTimer();
            InitFood();
        }

        private MySnake snake;
        private static readonly int size = 20; //Wymiary

        void InitBoard()
        {
            for (int i = 0; i < grid.Width / size; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(size);
                grid.ColumnDefinitions.Add(columnDefinition);
            }
            for (int j = 0; j < grid.Height / size; j++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(size);
                grid.RowDefinitions.Add(rowDefinition);
            }
            snake = new MySnake();
        }
        // Rysowanie węża
        void InitSnake()
        {
            grid.Children.Add(snake.Head.Rect);
            foreach (SnakePart snakePart in snake.Parts)
                grid.Children.Add(snakePart.Rect);
            snake.RedrawSnake();
        }
        // Poruszanie wężąa po planszy

        private int directionX = 1;
        private int directionY = 0;

        private void MoveSnake()
        {
            int snakePartCount = snake.Parts.Count;
            if(partsToAdd > 0)
            {
                SnakePart newPart = new SnakePart(snake.Parts[snake.Parts.Count - 1].X, snake.Parts[snake.Parts.Count - 1].Y);
                grid.Children.Add(newPart.Rect);
                snake.Parts.Add(newPart);
                partsToAdd--;
            }
            for (int i = snakePartCount - 1; i >=1; i-- )
            {
                snake.Parts[i].X = snake.Parts[i - 1].X;
                snake.Parts[i].Y = snake.Parts[i - 1].Y;
            }
            snake.Parts[0].X = snake.Head.X;
            snake.Parts[0].Y = snake.Head.Y;
            snake.Head.X += directionX;
            snake.Head.Y += directionY;

            if (CheckCollision())
                EndGame();
            else
            {
                if (CheckFood())
                    RedrawFood();
                snake.RedrawSnake();
            }
        }
        // Poruszanie wężąa
        private DispatcherTimer timer;

        void InitTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Start();
        }

        void TimerTick(object sender, EventArgs e)
        {
            MoveSnake();
        }
        //Sterowanie wężęm

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                directionX = -1;
                directionY = 0;
            }
            if (e.Key == Key.Right)
            {
                directionX = 1;
                directionY = 0;
            }
            if (e.Key == Key.Up)
            {
                directionX = 0;
                directionY = -1;
            }
            if (e.Key == Key.Down)
            {
                directionX = 0;
                directionY = 1;
            }
        }

        //Pojawianie się jedzenia na planszy
        private SnakePart food;

        void InitFood()
        {
            food = new SnakePart(0, 10);
            food.Rect.Width = food.Rect.Height = 20;
            food.Rect.Fill = Brushes.Crimson;
            grid.Children.Add(food.Rect);
            Grid.SetColumn(food.Rect, food.X);
            Grid.SetRow(food.Rect, food.Y);
        }
        // Zwiększanie węża
        private int partsToAdd;

        private bool CheckFood()
        {
            Random rand = new Random();
            if (snake.Head.X == food.X && snake.Head.Y == food.Y)
            {
                partsToAdd ++;
                // Losowanie następnego miejsca pojawienia się jedzenia
                for (int i = 0; i <20; i++)
                {
                    int x = rand.Next(0, (int)(grid.Width / size));
                    int y = rand.Next(0, (int)(grid.Height / size));
                    if (IsFieldFree(x,y))
                    {
                        food.X = x;
                        food.Y = y;
                        return true;
                    }
                }
                for (int i = 0; i <grid.Width / size; i++)
                for (int j = 0; j < grid.Height / size; j++)
                    {
                        if(IsFieldFree(i, j))
                        {
                            food.X = i;
                            food.Y = j;
                            return true;
                        }
                    }
                EndGame();
            }
            return false;
        }

        //Sprawdzanie czy pole jest puste (do losowania miejsca)
        private bool IsFieldFree(int x, int y)
        {
            if (snake.Head.X == x && snake.Head.Y == y)
                return false;
            foreach (SnakePart snakePart in snake.Parts)
            {
                if (snakePart.X == x && snakePart.Y == y)
                    return false;
            }
            return true;
        }
        //Koniec gry

        void EndGame()
        {
            timer.Stop();
            MessageBox.Show("KONIEC GRY");
            InitTimer();
        }
        //Rysowanie na planszy jedzenia

        private void RedrawFood()
        {
            Grid.SetColumn(food.Rect, food.X);
            Grid.SetRow(food.Rect, food.Y);
        }

        bool CheckCollision()
        {
            if (CheckBoardCollision())
                return true;
            if (CheckItselfCollision())
                return true;

            return false;
        }

        bool CheckBoardCollision()
        {
            if (snake.Head.X < 0 || snake.Head.X > grid.Width / size)
                return true;
            if (snake.Head.Y < 0 || snake.Head.Y > grid.Height / size)
                return true;
            return false;
        }

        bool CheckItselfCollision()
        {
            foreach (SnakePart snakePart in snake.Parts)
            {
                if (snake.Head.X == snakePart.X && snake.Head.Y == snakePart.Y)
                    return true;
            }
            return false;
        }

    }

}

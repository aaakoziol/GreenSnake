using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Wężyk
{
    class MySnake
    {
        public SnakePart Head { get; set; }
        public List<SnakePart> Parts { get; set; }

        public MySnake()
        {
            Head = new SnakePart(10, 0);
            Head.Rect.Width = Head.Rect.Height = 20;
            Head.Rect.Fill = System.Windows.Media.Brushes.DarkGreen;
            Parts = new List<SnakePart>();

            Parts.Add(new SnakePart(9, 0));
            Parts.Add(new SnakePart(8, 0));
            Parts.Add(new SnakePart(7, 0));
            Parts.Add(new SnakePart(6, 0));
            Parts.Add(new SnakePart(5, 0));
            Parts.Add(new SnakePart(4, 0));
            Parts.Add(new SnakePart(3, 0));
            Parts.Add(new SnakePart(2, 0));
            Parts.Add(new SnakePart(1, 0));
            Parts.Add(new SnakePart(0, 0));
        }
        public void RedrawSnake()
        {
            Grid.SetColumn(Head.Rect, Head.X);
            Grid.SetRow(Head.Rect, Head.Y);
            foreach(SnakePart snakePart in Parts)
            {
                Grid.SetColumn(snakePart.Rect, snakePart.X);
                Grid.SetRow(snakePart.Rect, snakePart.Y);
            }
        }
    }
}

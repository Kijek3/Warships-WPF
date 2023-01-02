using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Warships
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateBoard();
        }

        private void CreateBoard()
        {
            var rectangleSize = 80;
            var globalId = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Rectangle newRectangle = new Rectangle
                    {
                        Width = rectangleSize,
                        Height = rectangleSize,
                        Fill = Brushes.DarkGray,
                        StrokeThickness = 1,
                        Stroke = Brushes.Azure,
                    };
                    newRectangle.MouseEnter += OnMouseEnter;
                    newRectangle.MouseLeave += OnMouseLeave;
                    var id = globalId;
                    newRectangle.MouseLeftButtonDown += (sender, e) => OnMouseLeftButtonDown(sender, e, id);
                    Canvas.SetTop(newRectangle, i * rectangleSize);
                    Canvas.SetLeft(newRectangle, j * rectangleSize);
                    
                    Canvas.Children.Add(newRectangle);
                    globalId += 1;
                }
            }
        }
        
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource is Rectangle activeRectangle)
            {
                Rectangle newRectangle = activeRectangle;
                newRectangle.Fill = Brushes.SlateGray;
                Canvas.Children.Remove(activeRectangle);
                Canvas.Children.Add(newRectangle);
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource is Rectangle activeRectangle)
            {
                Rectangle newRectangle = activeRectangle;
                newRectangle.Fill = Brushes.DarkGray;
                Canvas.Children.Remove(activeRectangle);
                Canvas.Children.Add(newRectangle);
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseEventArgs e, int id)
        {
            Debug.WriteLine(id);
        }
    }
}
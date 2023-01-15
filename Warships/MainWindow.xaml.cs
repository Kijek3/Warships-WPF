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
        private Board _boardPlayer1;
        private Board _boardPlayer2;
        private const int RectangleSize = 80;
        
        public MainWindow()
        {
            InitializeComponent();
            GameManager.GetInstance().GameStateChanged += OnGameStateChanged;
            _boardPlayer1 = CreateBoard();
            GameManager.GetInstance().GameState = EGameState.SecondPlayerBoardGeneration;
            _boardPlayer2 = CreateBoard();
            GameManager.GetInstance().GameState = EGameState.FirstPlayerPlacingShips;
        }

        private Board CreateBoard()
        {
            var height = 10;
            var width = 10;
            var board = new Board(height, width);
            RedrawBoard(board);
            return board;
        }

        private void OnGameStateChanged()
        {
            if (GameManager.GetInstance().GameState == EGameState.SecondPlayerPlacingNext)
            {
                OverlayText.Text = "Player2 turn";
                Player1.Visibility = Visibility.Hidden;
                Player2.Visibility = Visibility.Hidden;
                Overlay.Visibility = Visibility.Visible;
            } else if (GameManager.GetInstance().GameState == EGameState.FirstPlayerTurnNext)
            {
                OverlayText.Text = "Player1 turn";
                Player1.Visibility = Visibility.Hidden;
                Player2.Visibility = Visibility.Hidden;
                Overlay.Visibility = Visibility.Visible;
            }
        }
        
        private void RedrawBoard(Board board)
        {
            var canvasBoard = GameManager.GetInstance().GameState.IsFirstPlayer() ? Player1 : Player2;
            for (int i = 0; i < board.RedrawBuffer.Count; i++)
            {
                var y = board.RedrawBuffer[i].Item1;
                var x = board.RedrawBuffer[i].Item2;
                
                var id = y * board.BoardProperties.Width + x;

                SolidColorBrush colorBrush = board.DrawnRectAt(y, x).RectColor();
                BoardRectangle boardRectangle = new BoardRectangle(board, RectangleSize, colorBrush, id, RedrawBoard);
                
                Canvas.SetTop(boardRectangle.Rectangle, y * RectangleSize);
                Canvas.SetLeft(boardRectangle.Rectangle, x * RectangleSize);

                foreach (UIElement rect in canvasBoard.Children)
                {
                    if (rect.Uid == id.ToString())
                    {
                        canvasBoard.Children.Remove(rect);
                        break;
                    }
                }
                
                canvasBoard.Children.Add(boardRectangle.Rectangle);
            }
            board.RedrawBuffer.Clear();
        }

        private void Overlay_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GameManager.GetInstance().GameState == EGameState.SecondPlayerPlacingNext)
            {
                GameManager.GetInstance().GameState = EGameState.SecondPlayerPlacingShips;
                Player1.Visibility = Visibility.Hidden;
                Player2.Visibility = Visibility.Visible;
                Overlay.Visibility = Visibility.Hidden;
            }
        }
    }
}
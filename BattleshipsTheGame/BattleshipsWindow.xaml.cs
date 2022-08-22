using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BattleshipsTheGame
{
    public partial class BattleshipsWindow : Window
    {
        private const int _battlefeldSize = 10;
        private int _pointsCounter;
        private int _movesCounter;
        private bool[,] _battlefieldArray;
        private readonly List<int> _shipsList;

        public BattleshipsWindow()
        {
            _shipsList = new List<int>() { 5, 4, 4 };
            InitializeComponent();
            SetupGame();
        }

        private void SetupGame()
        {
            _pointsCounter = 0;
            _movesCounter = 0;
            _battlefieldArray = new bool[_battlefeldSize, _battlefeldSize];

            var random = new Random();
            int rotate, column, row;
            bool isEnoughSpace;
            var tmpShipPosition = new List<KeyValuePair<int, int>>();
            foreach (var ship in _shipsList)
            {
                isEnoughSpace = false;
                while (!isEnoughSpace)
                {
                    tmpShipPosition.Clear();

                    rotate = random.Next(0, 2);

                    if (rotate == ((int)Rotate.VERTICAL))
                    {
                        row = random.Next(1, _battlefeldSize - ship);
                        column = random.Next(1, _battlefeldSize);
                    }
                    else
                    {
                        column = random.Next(1, _battlefeldSize - ship);
                        row = random.Next(1, _battlefeldSize);
                    }

                    //prepare list of potential fields occupied by the ship
                    tmpShipPosition.Add(new KeyValuePair<int, int>(row, column));
                    for (var i = 1; i < ship; i++)
                    {
                        switch (rotate)
                        {
                            case ((int)Rotate.VERTICAL):
                                tmpShipPosition.Add(new KeyValuePair<int, int>(row + i, column));
                                break;

                            case ((int)Rotate.HORIZONTAL):
                                tmpShipPosition.Add(new KeyValuePair<int, int>(row, column + i));
                                break;

                            default:
                                throw new ArgumentOutOfRangeException("rotate value is out of range.");
                        }
                    }

                    //check if any of fields are occupied already
                    isEnoughSpace = true;
                    foreach (var kvp in tmpShipPosition)
                        if (_battlefieldArray[kvp.Key, kvp.Value])
                            isEnoughSpace = false;

                    //put the ship on the battlefield
                    if (isEnoughSpace)
                        foreach (var kvp in tmpShipPosition)
                            _battlefieldArray[kvp.Key, kvp.Value] = true;
                }
            }
        }

        private void TextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "◦")
            {
                _movesCounter++;
                Coordinate coordinate = new Coordinate(textBox.Name
                    .Substring(textBox.Name.IndexOf("_") + 1)
                    .Split('_')
                    .Select(x => Int32.Parse(x)));
                if (_battlefieldArray[coordinate.Row, coordinate.Column])
                {
                    textBox.Text = "💥";
                    _pointsCounter++;
                    CheckIfWinner();
                }
                else
                    textBox.Text = "✕";
            }
        }

        private void CheckIfWinner()
        {
            var necessaryPoints = _shipsList.Sum();
            if (_pointsCounter == necessaryPoints)
            {
                var playAgain = MessageBox.Show($"You won!\nMoves counter: {_movesCounter}" +
                    $"\nDo you want to play once again?"
                    , "End of the game", MessageBoxButton.YesNo);
                if (playAgain == MessageBoxResult.Yes)
                    CreateNewGame();
                else
                    this.Close();
            }
        }

        private void CreateNewGame()
        {
            foreach (TextBox textBox in mainGrid.Children.OfType<TextBox>())
            {
                textBox.Text = "◦";
            }
            SetupGame();
        }

        private enum Rotate
        {
            VERTICAL,
            HORIZONTAL
        }
    }

    internal record Coordinate
    {
        public int Row, Column;

        public Coordinate(IEnumerable<int> enumarable)
        {
            Row = enumarable.ElementAt(0);
            Column = enumarable.ElementAt(1);
        }
    }
}
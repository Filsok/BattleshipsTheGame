using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BattleshipsTheGame
{
    public partial class BattleshipsWindow : Window
    {
        private int _battlefeldSize = 10;
        private int _pointsCounter = 0;
        private int _movesCounter = 0;
        private bool[,]? _battlefieldArray;
        private List<int>? _shipsList;

        public BattleshipsWindow()
        {
            InitializeComponent();
            SetupGame();
        }

        private void SetupGame()
        {
            _shipsList?.Clear();
            _shipsList = new List<int>() { 5, 4, 4 };
            _battlefieldArray = new bool[_battlefeldSize, _battlefeldSize];

            var random = new Random();
            int direction, column, row;
            bool isEnoughSpace;
            List<KeyValuePair<int, int>> tmpShipPosition = new List<KeyValuePair<int, int>>();
            foreach (int ship in _shipsList)
            {
                isEnoughSpace = false;
                while (!isEnoughSpace)
                {
                    tmpShipPosition.Clear();
                    //draw a rotate of this ship
                    direction = random.Next(0, 2);               //directions: 0-vertical(down), 1-horizontal(right)

                    //draw a start point on the battlefield
                    if (direction == 0)
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
                    for (int i = 1; i < ship; i++)
                    {
                        switch (direction)
                        {
                            case 0:             //vertical
                                tmpShipPosition.Add(new KeyValuePair<int, int>(row + i, column));
                                break;

                            case 1:             //horizontal
                                tmpShipPosition.Add(new KeyValuePair<int, int>(row, column + i));
                                break;

                            default:
                                throw new ArgumentOutOfRangeException("direction value is out of range.");
                        }
                    }

                    //check if any of fields are occupied already
                    isEnoughSpace = true;
                    foreach (var kvp in tmpShipPosition)
                        if (_battlefieldArray[kvp.Key, kvp.Value] == true)
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
            _movesCounter++;
            TextBox textBox = sender as TextBox;
            string tmpString = textBox?.Name.Substring(12);
            int[] coordinates = new int[2];
            string[] tmpArray = tmpString.Split('_');
            for (int i = 0; i < coordinates.Length; i++)
                coordinates[i] = Convert.ToInt32(tmpArray[i]);
            if (_battlefieldArray?[coordinates[0], coordinates[1]] == true)
            {
                textBox.Text = "💥";
                _pointsCounter++;
            }
            else
                textBox.Text = "✕";

            IsWinner();
        }

        private async void IsWinner()
        {
            int? necessaryPoints = _shipsList?.Sum();
            if (_pointsCounter == necessaryPoints)
            {
                var playAgain = MessageBox.Show($"You won!\nMoves counter: {_movesCounter} \nDo you want to play once again?", "End of the game", MessageBoxButton.YesNo);
                if (playAgain == MessageBoxResult.Yes)
                {
                    this.Hide();
                    await Task.Run(() => CreateNewGame());
                    Task.WaitAll();
                    this.Close();
                }
                else if (playAgain == MessageBoxResult.No)
                    this.Close();
            }
        }

        private Task CreateNewGame()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                BattleshipsWindow newGame = new BattleshipsWindow();
                newGame.ShowDialog();
            });
            return Task.CompletedTask;
        }
    }
}
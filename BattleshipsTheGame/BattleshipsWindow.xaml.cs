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
    }
}

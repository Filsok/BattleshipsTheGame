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


        public BattleshipsWindow()
        {
            InitializeComponent();
            SetUpGame();
        }


        private void SetUpGame()
        {
            List<Ship> shipsList = new List<Ship>()
            {
                new Ship("Battleship",5),
                new Ship("Destroyer",4),
                new Ship("Destroyer",4)
            };

            bool[,] battlefieldArray = new bool[_battlefeldSize, _battlefeldSize];

            //"💥"
            //⭕❌＞︿＜(˘･_･˘)(ㆆ_ㆆ)✕💢💥

            var random = new Random();
            int direction, column, row;
            bool isEnoughSpace;
            List<KeyValuePair<int,int>> tmpShipPosition = new List<KeyValuePair<int,int>>();
            foreach (Ship ship in shipsList)
            {
                isEnoughSpace = false;
                while (!isEnoughSpace)
                {
                    tmpShipPosition.Clear();
                    //draw a rotate of this ship
                    direction = random.Next(0, 3);               //directions: 0-up, 1-down, 2-right, 3-left

                    //draw a start point on the battlefield
                    if (direction < 2)
                    {
                        row = direction == 1 ? random.Next(1 + ship.Length, _battlefeldSize) : random.Next(1, _battlefeldSize - ship.Length);
                        column = random.Next(1, _battlefeldSize);
                    }
                    else
                    {
                        column = direction == 3 ? random.Next(1 + ship.Length, _battlefeldSize) : random.Next(1, _battlefeldSize - ship.Length);
                        row = random.Next(1, _battlefeldSize);
                    }

                    //prepare list of potential fields occupied by the ship
                    tmpShipPosition.Add(new KeyValuePair<int,int>(row, column));
                    for (int i = 1; i < ship.Length; i++)
                    {
                        switch (direction)
                        {
                            case 0:             //up
                                tmpShipPosition.Add(new KeyValuePair<int, int>(row + i, column));
                                break;
                            case 1:             //down
                                tmpShipPosition.Add(new KeyValuePair<int, int>(row - i, column));
                                break;
                            case 2:             //right
                                tmpShipPosition.Add(new KeyValuePair<int, int>(row, column+i));
                                break;
                            case 3:             //left
                                tmpShipPosition.Add(new KeyValuePair<int, int>(row, column-i));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("direction value is out of range.");
                        }
                    }

                    //check if any of fields are occupied already
                    isEnoughSpace = true;
                    foreach (var kvp in tmpShipPosition)
                    {
                        if (battlefieldArray[kvp.Key,kvp.Value]==true)
                            isEnoughSpace=false;
                    }

                    //put the ship on the battlefield
                    if (isEnoughSpace)
                    {
                        foreach (var kvp in tmpShipPosition)
                        {
                            battlefieldArray[kvp.Key, kvp.Value] = true;
                        }
                    }
                }
            }
        }
    }
}

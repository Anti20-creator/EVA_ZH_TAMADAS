using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ZH
{
    struct Pos
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public class Model
    {
        int size;
        private int[,] map;
        public int OnTurn { get; set; }
        private Pos[,] players;
        public int Player1Next;
        public int Player2Next;

        public int Size { get { return size; } set { size = value; } }
        public EventHandler<int> generateTable;
        public EventHandler<int> GameOver;
        public Model() {
            Size = 0;
            map = new int[0, 0];
            OnTurn = 0;
            Player1Next = 0;
            Player2Next = 0;
            players = new Pos[0, 0];
        }

        public void StartGame(int e)
        {
            map = new int[e, e];
            Size = e;

            OnTurn = 0;
            Player1Next = 0;
            Player2Next = 0;


            players = new Pos[2, 4];

            players[0, 0] = new Pos(e-2, 0);
            players[0, 1] = new Pos(e-2, 1);
            players[0, 2] = new Pos(e-1, 1);
            players[0, 3] = new Pos(e-1, 0);

            players[1, 0] = new Pos(1, e-1);
            players[1, 1] = new Pos(1, e-2);
            players[1, 2] = new Pos(0, e-2);
            players[1, 3] = new Pos(0, e-1);
            generateMap();
            OnGenerateTable(e);
        }

        private void OnGenerateTable(int e)
        {
            if (generateTable != null)
            {
                generateTable(this, e);
            }
        }

        private void generateMap()
        {
            for(int i = 0; i < Size; ++i)
            {
                for(int j = 0; j < Size; ++j)
                {
                    map[i, j] = 0;
                }
            }

            for(int i = 0; i < 2; ++i)
            {
                for(int j = 0; j < 4; ++j) 
                {
                    if (players[i, j].X == -1) continue;
                    map[players[i, j].X, players[i, j].Y] = i * 4 + j + 1;
                }
            }
        }

        public int getMapElem(int x, int y)
        {
            return map[x, y];
        }

        public void StepGame(int x, int y)
        {
            int pnumber = OnTurn == 1 ? Player1Next : Player2Next;
            int trys = 1;
            while(players[OnTurn, pnumber].X == -1 && players[OnTurn, pnumber].Y == -1 && trys < 5)
            {
                pnumber++;
                pnumber %= 4;
                trys++;
            }
            if(trys >= 5)
            {
                OnTurn = OnTurn == 1 ? 0 : 1;
            }
            Pos from = players[OnTurn, pnumber];
            if(Math.Abs(x - from.X) < 2 && Math.Abs(y- from.Y) < 2 && (from.X != x || from.Y != y))
            {
                bool canmove = true;
                for(int i = 0; i < 4; ++i)
                {
                    if (i == pnumber) continue;
                    if(players[OnTurn, i].X == x && players[OnTurn, i].Y == y)
                    {
                        canmove = false;
                    }
                }

                if((Math.Abs(x - from.X) == 1 && Math.Abs(y - from.Y) == 0) ||
                    Math.Abs(x - from.X) == 0 && Math.Abs(y- from.Y) == 1)
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        int opponent = OnTurn == 1 ? 0 : 1;
                        if (players[opponent, i].X == x && players[opponent, i].Y == y)
                        {
                            canmove = false;
                        }
                    }
                }

                bool isKilling = false;
                int killpos = -1;

                if(Math.Abs(x - from.X) == 1 && Math.Abs(y - from.Y) == 1)
                {
                    for(int i = 0; i < 4; ++i)
                    {
                        int opponent = OnTurn == 1 ? 0 : 1;
                        if (players[opponent, i].X == x && players[opponent, i].Y == y)
                        {
                            isKilling = true;
                            killpos = i;
                        }
                    }
                }
                if (canmove)
                {
                    players[OnTurn, pnumber] = new Pos(x, y);
                    if (isKilling)
                    {
                        int opponent = OnTurn == 1 ? 0 : 1;
                        players[opponent, killpos] = new Pos(-1, -1);
                    }

                    if(OnTurn == 1)
                    {
                        if((x == Size - 1 || x == Size -2) &&
                            (y == 0 || y == 1))
                        {
                            OnGameOver(1);
                        }
                    }
                    else
                    {
                        if ((y == Size - 1 || y == Size - 2) &&
                            (x == 0 || x == 1))
                        {
                            OnGameOver(0);
                        }
                    }

                    if(OnTurn == 1)
                    {
                        Player1Next++;
                        Player1Next %= 4;
                    }
                    else
                    {
                        Player2Next++;
                        Player2Next %= 4;
                    }
                    
                    OnTurn = OnTurn == 1 ? 0 : 1;

                    
                }
                else
                {
                    MessageBox.Show("Invalid Move!");
                }

            }
            generateMap();
        }

        private void OnGameOver(int winner)
        {
            if(GameOver != null)
            {
                GameOver(this, winner);
            }
        }
    }
}

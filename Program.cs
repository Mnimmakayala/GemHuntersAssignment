using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemHuntersAssignment
{
    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Player
    {
        public string Name { get; }
        public Position Position { get; set; }
        public int GemCount { get; set; }

        public Player(string name, Position position)
        {
            Name = name;
            Position = position;
            GemCount = 0;
        }
        //Decides on the player direction
        public void Move(char direction)
        {
            switch (direction)
            {
                case 'U':
                    Position.Y--;
                    break;
                case 'D':
                    Position.Y++;
                    break;
                case 'L':
                    Position.X--;
                    break;
                case 'R':
                    Position.X++;
                    break;
                default:
                    Console.WriteLine("Invalid direction.");
                    break;
            }
        }
    }

    // Check for the cells in the matrix
    class Cell
    {
        public string occupied { get; set; }

        public Cell(string occupied)
        {
            occupied = occupied;
        }
    }

    // Designing the game board 
    class Board
    {
        private const int Size = 6;
        private Cell[,] Grid = new Cell[Size, Size];
        public Player Player1;
        public Player Player2;

        public Board()
        {
            InitializeGrid();
            InitializePlayers();
        }

        private void InitializeGrid()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j] = new Cell("-");
                }
            }
            PlaceObstacles();
            PlaceGems();
        }

        private void InitializePlayers()
        {
            Player1 = new Player("P1", new Position(0, 0));
            Player2 = new Player("P2", new Position(Size - 1, Size - 1));
            Grid[Player1.Position.Y, Player1.Position.X].occupied = Player1.Name;
            Grid[Player2.Position.Y, Player2.Position.X].occupied = Player2.Name;
        }

        // Updating the obstacle in the board randomly
        private void PlaceObstacles()
        {
            Random rand = new Random();
            int obstaclesCount = rand.Next(6, 12);
            for (int i = 0; i < obstaclesCount; i++)
            {
                int x = rand.Next(0, Size);
                int y = rand.Next(0, Size);
                if (Grid[y, x].occupied == "-")
                {
                    Grid[y, x].occupied = "O";
                }
            }
        }

        //Place gems randomly in the board
        private void PlaceGems()
        {
            Random rand = new Random();
            int gemsCollected = rand.Next(6, 12);
            for (int i = 0; i < gemsCollected; i++)
            {
                int x = rand.Next(0, Size);
                int y = rand.Next(0, Size);
                if (Grid[y, x].occupied == "-")
                {
                    Grid[y, x].occupied = "G";
                }
            }
        }

        //Display the board
        public void Display()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Console.Write(Grid[i, j].occupied + " ");
                }
                Console.WriteLine();
            }
        }

        //Move of the player
        public bool IsValidMove(Player player, char direction)
        {
            int x = player.Position.X;
            int y = player.Position.Y;

            switch (direction)
            {
                case 'U':
                    y--;
                    break;
                case 'D':
                    y++;
                    break;
                case 'L':
                    x--;
                    break;
                case 'R':
                    x++;
                    break;
                default:
                    Console.WriteLine("Invalid direction.");
                    return false;
            }

            if (x < 0 || y < 0 || x >= Size || y >= Size)
            {
                Console.WriteLine("Out of bounds.");
                return false;
            }

            if (Grid[y, x].occupied == "O")
            {
                Console.WriteLine("Obstacle in the way.");
                return false;
            }

            return true;
        }

        // Collect the gems
        public void GemsCollection(Player player)
        {
            int x = player.Position.X;
            int y = player.Position.Y;

            if (Grid[y, x].occupied == "G")
            {
                player.GemCount++;
                Grid[y, x].occupied = "-";
                Console.WriteLine(player.Name + " collected a gem!");
            }
        }
    }


    class Game
    {
        public Board Board;
        public Player Player1;
        public Player Player2;
        public Player CurrentTurn;
        public int TotalTurns;

        public Game()
        {
            Board = new Board();
            Player1 = Board.Player1;
            Player2 = Board.Player2;
            CurrentTurn = Player1;
            TotalTurns = 0;
        }

        // Initiating the start of game
        public void Start()
        {
            while (!IsGameOver())
            {
                Console.WriteLine("Current Turn: " + CurrentTurn.Name);
                Board.Display();
                Console.Write("Enter move (U/D/L/R): ");
                char direction = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();

                if (Board.IsValidMove(CurrentTurn, direction))
                {
                    CurrentTurn.Move(direction);
                    Board.GemsCollection(CurrentTurn);
                    SwitchTurn();
                    TotalTurns++;
                }
            }
            Board.Display();
            AnnounceWinner();
        }

        //Move to next player
        private void SwitchTurn()
        {
            CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
        }

        private bool IsGameOver()
        {
            return TotalTurns >= 30;
        }
        //Announcing the winner
        private void AnnounceWinner()
        {
            if (Player1.GemCount > Player2.GemCount)
            {
                Console.WriteLine("Player 1 wins!");
            }
            else if (Player2.GemCount > Player1.GemCount)
            {
                Console.WriteLine("Player 2 wins!");
            }
            else
            {
                Console.WriteLine("It's a tie!");
            }
        }
    }

    // Main class initiating the run

    class Program
    {
        public static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }


}

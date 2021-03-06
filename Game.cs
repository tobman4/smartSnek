using System;
using System.Collections.Generic;
using System.Linq;

namespace smartSnek {

    public class Point {
        public int X { get; set; }
        public int Y { get; set; }

        public Point Clone() {
            return new Point { X = X, Y = Y };
        }

        public bool isZero() {
            return X == 0 && Y == 0;
        }

        public void add(Point p) {
            X += p.X;
            Y += p.Y;
        }
    }

    public enum CellState {
        None,
        Snake,
        Food,
        Wall
    }

    public class Game : IgameController {

        private Point snakeSpeed = new Point { X = 0, Y = 0 };
        private Point head => snake[0];
        private List<Point> snake;
        private int toAdd = 2;
        private CellState[,] board;
        private int foodScore = 0;
        private int timeScore = 0;
        private Random rng;

        public bool isDead { get; private set; } = false;
        public int score => foodScore + timeScore;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Game(int w, int h, int seed = 0) {

            Width = w;
            Height = h;

            if(seed == 0) {
                rng = new();
            } else {
                rng = new(seed);
            }

            board = new CellState[w,h];

            Console.WriteLine($"Width: {board.GetLength(0)} Height: {board.GetLength(1)}");

            snake = new List<Point>();
            snake.Add(new Point { X=w/2, Y=h/2 });
            resetMap();
        }

        public void step() {
            if(isDead || snakeSpeed.isZero()) return;
            timeScore++;

            Point hold = snake[0].Clone();
            
            head.add(snakeSpeed);
            if (head.X < 0 || head.Y < 0 || head.X >= board.GetLength(0) || head.Y >= board.GetLength(1)) {
                //crash w wall
                isDead = true;
                return;
            } else if (board[head.X, head.Y] == CellState.Snake) {
                //crash w self
                isDead = true;
                return;
            } else if(board[head.X, head.Y] == CellState.Food) {
                toAdd++;
                placeFood();
                foodScore += 500;
            }

            board[head.X, head.Y] = CellState.Snake;

            for (int i = 1; i < snake.Count(); i++) {
                Point h = snake[i];
                snake[i] = hold;
                hold = h;
            }

            if(toAdd > 0) {
                snake.Add(hold);
                toAdd--;
            } else {
                board[hold.X, hold.Y] = CellState.None;
            }
        }

        public void reset() {
            isDead = false;

            board = new CellState[Width,Height];

            snake = new List<Point>();
            snake.Add(new Point { X = Width / 2, Y = Height / 2 });

            snakeSpeed = new Point { X = 0, Y = 0 };

            foodScore = 0;
            timeScore = 0;

            toAdd = 2;
            resetMap();
        }

        private void resetMap() {
            board = new CellState[board.GetLength(0), board.GetLength(1)];
            foreach (Point p in snake) {
                board[p.X, p.Y] = CellState.Snake;
            }
            placeFood();
        }

        private void placeFood() {
            Point newFood;
            do {
                newFood = new Point { X = rng.Next(0, board.GetLength(0)), Y = rng.Next(0, board.GetLength(1)) };
            } while (board[newFood.X, newFood.Y] != CellState.None);
            board[newFood.X, newFood.Y] = CellState.Food;
        }

        #region Controller
        public Point getHead() {
            return snake[0];
        }

        public CellState[,] getMap() {
            return board.Clone() as CellState[,];
        }

        public Point[] getSnake() {
            Point[] arr = new Point[snake.Count];
            snake.CopyTo(arr);
            return arr;
        }

        public void Left() {
            if(snakeSpeed.X == 0) {
                snakeSpeed = new Point { X = -1, Y = 0 };
            }
        }

        public void Right() {
            if (snakeSpeed.X == 0) {
                snakeSpeed = new Point { X = 1, Y = 0 };
            }
        }

        public void Up() {
            if (snakeSpeed.Y == 0) {
                snakeSpeed = new Point { X = 0, Y = -1 };
            }
        }

        public void Down() {
            if (snakeSpeed.Y == 0) {
                snakeSpeed = new Point { X = 0, Y = 1 };
            }
        }
        #endregion

        public override string ToString() {
            string o = $"Score: {score}\n";
            for(int i = 0; i < board.GetLength(1); i++) {
                for (int j = 0; j < board.GetLength(0); j++) {
                    switch(board[j,i]) {
                        case CellState.Food:
                            o += "F";
                            break;
                        case CellState.Snake:
                            o += "S";
                            break;
                        case CellState.None:
                            o += " ";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                o += "\n";
            }
            return o;
        }

    }
}

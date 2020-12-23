using System;
using System.Collections.Generic;

namespace Tic_Tac_Toe_Ideal_Game
{
    class Program
    {
        static int[,] gameBoard = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        static int count = 0;
        static int isComputerFirst = 0;
        static int UNKNOWN = 0;
        static int USER = 1;
        static int COMPUTER = 2;
        static int[,] DIAGONAL_A = { { 0, 0}, { 1, 1 }, { 2, 2} };
        static int[,] DIAGONAL_B = { { 0, 2 }, { 1, 1 }, { 2, 0 } };
        static int[] rowWinSum = { 0, 0, 0 };
        static int[] columnWinSum = { 0, 0, 0 };
        static int diagonalASum = 0;
        static int diagonalBSum = 0;

        public static void Main(String[] args)
        {
            int winner = UNKNOWN;
            while(winner == UNKNOWN && count != 9)
            {
                int[] newCoordinate = new int[2];
                if (count % 2 == isComputerFirst)
                {
                    PrintBoard();
                    newCoordinate = UserMove();
                }
                else
                {
                    newCoordinate = ProgramMove();
                }
                winner = CheckWinner(newCoordinate);
                Console.WriteLine(count);

            }
            PrintBoard();
            if ( winner== USER)
            {
                Console.WriteLine("You Win!");
            }
            else if (winner == COMPUTER)
            {
                Console.WriteLine("The Computer Wins!");
            }
            else
            {
                Console.WriteLine("It's a tie");
            }
        }

        public static void PrintBoard()
        {
            Console.WriteLine();
            Console.WriteLine("    0   1   2");
            Console.WriteLine("  +-----------+");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(i + " |");
                for (int j = 0; j < 3; j++)
                {
                    if (gameBoard[i, j] == UNKNOWN)
                    {
                        Console.Write("   |");
                    }
                    else if (gameBoard[i, j] == USER)
                    {
                        Console.Write(" X |");
                    }
                    else
                    {
                        Console.Write(" O |");
                    }

                }
                Console.WriteLine();
            }
            Console.WriteLine("  +-----------+");
        }
        public static int[] ProgramMove()
        {
            int[] win = PotentialWinCheck(COMPUTER);
            if(!( win is null))
            {
                PutMark(COMPUTER, win);
                return win;
            }

            int[] block = PotentialWinCheck(USER);
            if (!(block is null))
            {
                PutMark(COMPUTER, block);
                return block;
            }

            int[] programFork = Fork(COMPUTER);
            if(!(programFork is null))
            {
                PutMark(COMPUTER, programFork);
                return programFork;
            }

            int[] userFork = Fork(USER);
            if (!(userFork is null))
            {
                int[] tryBlock = TwoInARow(userFork);
                if(!(tryBlock is null))
                {
                    PutMark(COMPUTER, tryBlock);
                    return tryBlock;
                }
                else
                {
                    PutMark(COMPUTER, userFork);
                    return userFork;
                } 
            }


            if (Center())
            {
                PutMark(COMPUTER, new int[2] { 1,1});
                return new int[2] { 1, 1 };
            }

            int[] programOppoCorner = OppositeCorner();
            if (!(programOppoCorner is null))
            {
                PutMark(COMPUTER, programOppoCorner);
                return programOppoCorner;
            }

            int[] programCorner = GetCorner();
            if (!(programCorner is null))
            {
                PutMark(COMPUTER, programCorner);
                return programCorner;
            }

            int[] programSide = GetSide();
            if (!(programSide is null))
            {
                PutMark(COMPUTER, programSide);
                return programSide;
            }

            return null;

        }





        public static int[] ReadCoordinate()
        {
            int[] coordinate = new int[2];
            Console.WriteLine("Enter Row");
            coordinate[0] = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter Column");
            coordinate[1] = Int32.Parse(Console.ReadLine());
            if (CheckCoordinateRange(coordinate))
            {
                return coordinate;
            }
            else
            {
                return ReadCoordinate();
            }
        }
        public static bool CheckCoordinateRange(int[] coordinate)
        {
            return coordinate[0] < 3 && coordinate[1] < 3 && coordinate[0] >= 0 && coordinate[1] >= 0 && gameBoard[coordinate[0], coordinate[1]] == UNKNOWN;
        }

        public static int[] UserMove()
        {
            int[] coordinate = ReadCoordinate();
            PutMark(USER, coordinate);
            return coordinate;
        }
        public static void PutMark(int activePlayer, int[] coordinate)
        {
            count += 1;
            int score = 0;

            gameBoard[coordinate[0], coordinate[1]] = activePlayer;
            if (activePlayer == USER)
            {
                score = 1;
            }
            else
            {
                score = 4;
            }
            rowWinSum[coordinate[0]] += score;
            columnWinSum[coordinate[1]] += score;
            Console.WriteLine(coordinate[0] + "   " + coordinate[1]);

            if ( (coordinate[0]==0 && coordinate[1]==0) || (coordinate[0] == 1 && coordinate[1] == 1) || (coordinate[0] == 2 && coordinate[1] == 2))
            {
                diagonalASum += score;
            }
            if ((coordinate[0] == 0 && coordinate[1] == 2) || (coordinate[0] == 1 && coordinate[1] == 1) || (coordinate[0] == 2 && coordinate[1] == 0))
            {
                diagonalBSum += score;
            }
        }
        public static int CheckWinner(int[] newCoordinate)
        {
            if (rowWinSum[newCoordinate[0]] == 3)
            {
                return USER;
            }
            else if (rowWinSum[newCoordinate[0]] == 12)
            {
                return COMPUTER;
            }


            if (columnWinSum[newCoordinate[1]] == 3)
            {
                return USER;
            }
            else if (columnWinSum[newCoordinate[1]] == 12)
            {
                return COMPUTER;
            }


            if (diagonalASum == 3)
            {
                return USER;
            }
            else if (diagonalASum == 12)
            {
                return COMPUTER;
            }


            if (diagonalBSum == 3)
            {
                return USER;
            }

            else if (diagonalBSum == 12)
            {
                return COMPUTER;
            }
            return 0;
        }

        public static int GameBoardAt(int[] coordinate)
        {
            return gameBoard[coordinate[0], coordinate[1]];
        }

        public static int[] PotentialWinCheck(int activePlayer)
        {
            int test = 0;
            if (activePlayer == USER)
            {
                test = 1;
            }
            else
            {
                test = 4;
            }


            for (int i = 0; i < 3; i++)
            {
                if (rowWinSum[i] == 2 * test)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (gameBoard[i, j] == UNKNOWN)
                        {
                            return new int[2] { i, j };
                        }
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (columnWinSum[i] == 2 * test)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (gameBoard[i, j] == UNKNOWN)
                        {
                            return new int[2] { i, j };
                        }
                    }
                }
            }

            if (diagonalASum == 2 * test)
            {
                if (GameBoardAt(new int[2] { 0, 0 }) == UNKNOWN)
                {
                    return new int[2] { 0, 0 };
                }
                else if (GameBoardAt(new int[2] { 1, 1 }) == UNKNOWN)
                {
                    return new int[2] { 1, 1 };
                }
                else if (GameBoardAt(new int[2] { 2, 2 }) == UNKNOWN)
                {
                    return new int[2] { 2, 2 };
                }
            }

            if (diagonalBSum == 2 * test)
            {
                if (GameBoardAt(new int[2] { 2, 0 }) == UNKNOWN)
                {
                    return new int[2] { 2, 0 };
                }
                else if (GameBoardAt(new int[2] { 1, 1 }) == UNKNOWN)
                {
                    return new int[2] { 1, 1 };
                }
                else if (GameBoardAt(new int[2] { 0, 2 }) == UNKNOWN)
                {
                    return new int[2] { 0, 2 };
                }
            }
            return null;
        }

        public static int[] Fork(int activePlayer)
        {
            List<int[]> threat = new List<int[]>();

            int test = 0;
            if (activePlayer == USER)
            {
                test = 1;
            }
            else
            {
                test = 4;
            }
            for (int i = 0; i < 3; i++)
            {
                if (rowWinSum[i] == test)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (gameBoard[i, j] == UNKNOWN)
                        {
                            threat.Add(new int[2] { i, j });
                        }
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (columnWinSum[i] == test)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (gameBoard[i, j] == UNKNOWN)
                        {
                            threat.Add(new int[2] { i, j });
                        }
                    }
                }
            }

            if (diagonalASum == test)
            {
                if (GameBoardAt(new int[2] { 0, 0 }) == UNKNOWN)
                {
                    threat.Add(new int[2] { 0, 0 });
                }
                else if (GameBoardAt(new int[2] { 1, 1 }) == UNKNOWN)
                {
                    threat.Add(new int[2] { 1, 1 });
                }
                else if (GameBoardAt(new int[2] { 2, 2 }) == UNKNOWN)
                {
                    threat.Add(new int[2] { 2, 2 });
                }
            }

            if (diagonalBSum == test)
            {
                if (GameBoardAt(new int[2] { 2, 0 }) == UNKNOWN)
                {
                    threat.Add(new int[2] { 2, 0 });
                }
                else if (GameBoardAt(new int[2] { 1, 1 }) == UNKNOWN)
                {
                    threat.Add(new int[2] { 1, 1 });
                }
                else if (GameBoardAt(new int[2] { 0, 2 }) == UNKNOWN)
                {
                    threat.Add(new int[2] { 0, 2 });
                }
            }
            for (int i = 0; i < threat.Count - 1; i++)
            {
                for (int j = i + 1; j < threat.Count; j++)
                {
                    if (threat[i] == threat[j])
                    {
                        return threat[i];
                    }
                }
            }
            return null;

        }
        public static int[] TwoInARow(int[] blockFork)
        {
            List<int[]> potentialMove = new List<int[]>();
            for (int i = 0; i < 3; i++)
            {
                if (rowWinSum[i] == 4)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (gameBoard[i, j] == UNKNOWN)
                        {
                            potentialMove.Add(new int[2] { i, j });
                        }
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (columnWinSum[i] == 4)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (gameBoard[i, j] == UNKNOWN)
                        {
                            potentialMove.Add(new int[2] { i, j });
                        }
                    }
                }
            }


            if (diagonalASum == 4)
            {
                if (GameBoardAt(new int[2] { 0, 0 }) == UNKNOWN)
                {
                    if (blockFork == new int[2] { 0, 0 })
                    {
                        return blockFork;
                    }
                    else
                    {
                        potentialMove.Add(blockFork);
                    }
                }
                else if (GameBoardAt(new int[2] { 1, 1 }) == UNKNOWN)
                {
                    if (blockFork == new int[2] { 1, 1})
                    {
                        return blockFork;
                    }
                    else
                    {
                        potentialMove.Add(blockFork);
                    }
                }
                else if (GameBoardAt(new int[2] { 2, 2 }) == UNKNOWN)
                {
                    if (blockFork == new int[2] { 2, 2 })
                    {
                        return blockFork;
                    }
                    else
                    {
                        potentialMove.Add(blockFork);
                    }
                }
            }


            if (diagonalBSum == 4)
            {
                if (GameBoardAt(new int[2] { 2, 0 }) == UNKNOWN)
                {
                    if (blockFork == new int[2] { 2, 0 })
                    {
                        return blockFork;
                    }
                    else
                    {
                        potentialMove.Add(blockFork);
                    }
                }
                else if (GameBoardAt(new int[2] { 1, 1 }) == UNKNOWN)
                {
                    if (blockFork == new int[2] { 1, 1 })
                    {
                        return blockFork;
                    }
                    else
                    {
                        potentialMove.Add(blockFork);
                    }
                }
                else if (GameBoardAt(new int[2] { 0, 2 }) == UNKNOWN)
                {
                    if (blockFork == new int[2] { 0, 2 })
                    {
                        return blockFork;
                    }
                    else
                    {
                        potentialMove.Add(blockFork);
                    }
                }
            }

            if (potentialMove.Count > 0)
            {
                return potentialMove[0];
            }
            else
            {
                return null;
            }
        }


        public static bool Center()
        {
            return gameBoard[1,1] == UNKNOWN;
        }

        public static int[] OppositeCorner()
        {
            if(gameBoard[0,0] == USER && gameBoard[2,2] == UNKNOWN)
            {
                return new int[2] { 2, 2 };
            }
            else if(gameBoard[2, 2] == USER && gameBoard[0, 0] == UNKNOWN)
            {
                return new int[2] { 0, 0 };
            }
            else if (gameBoard[0, 2] == USER && gameBoard[2, 0] == UNKNOWN)
            {
                return new int[2] { 2, 0 };
            }
            else if (gameBoard[2, 0] == USER && gameBoard[0, 2] == UNKNOWN)
            {
                return new int[2] { 0, 2 };
            }
            else
            {
                return null;
            }
        }
        public static int[] GetCorner()
        {
            if (gameBoard[2, 2] == UNKNOWN)
            {
                return new int[2] { 2, 2 };
            }
            else if(gameBoard[0,0] == UNKNOWN)
            {
                return new int[2] { 0, 0 };
            }
            else if (gameBoard[2, 0] == UNKNOWN)
            {
                return new int[2] { 2, 0 };
            }
            else if (gameBoard[0, 2] == UNKNOWN)
            {
                return new int[2] { 0, 2 };
            }
            else
            {
                return null;
            }
        }
        public static int[] GetSide()
        {
            if (gameBoard[1, 0] == UNKNOWN)
            {
                return new int[2] { 1, 0 };
            }
            else if (gameBoard[2, 1] == UNKNOWN)
            {
                return new int[2] { 2, 1 };
            }
            else if (gameBoard[1, 2] == UNKNOWN)
            {
                return new int[2] { 1, 2 };
            }
            else if (gameBoard[0, 1] == UNKNOWN)
            {
                return new int[2] { 0, 1 };
            }
            else
            {
                return null;
            }
        }
    }
}

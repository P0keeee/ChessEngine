using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ChessEngine
{
    class AI : Player
    {
        private bool revMaxPlayer;
        private int nodeMinValue = -999;
        private int nodeMaxValue = 999;
        private bool canBook = true;
        private List<string> bookMoves = new List<string>();
        public AI(bool initColour, Board initBoard) : base(initColour, initBoard)
        {

        }
        public override void GetNextMove()
        {
            string str = string.Empty;
            StreamReader sr = new StreamReader("openingBook.txt");
            if (canBook == true)
            {
                bookMoves.Clear();
                while ((str = sr.ReadLine()) != null)
                {
                    if (str.Contains(board.GetFen()))
                    {
                        var nextLine = sr.ReadLine();
                        bookMoves.Add(nextLine);
                        //board.LoadBoardWithFen(nextLine);
                        //bookFound = true;
                    }
                }
                if(bookMoves.Count > 0)
                {
                    Random rnd = new Random();
                    int num = rnd.Next(bookMoves.Count);
                    //Console.WriteLine(bookMoves[num]);
                    board.LoadBoardWithFen(bookMoves[num]);
                }
                else
                {
                    canBook = false;
                }
            }
            if (canBook == false)
            {
                Init();
            }
        }

        /*public void RandomMove()
        {
            Dictionary<string, List<string>> allPossibleMoves = board.GetAllMove(colour);
            List<string> keyList = new List<string>(allPossibleMoves.Keys);

            Random rand = new Random();

            if (allPossibleMoves.Count > 0)
            {
                int nbPiece = rand.Next(allPossibleMoves.Count);
                string coordPiece = keyList[nbPiece];

                int nbMove = rand.Next(allPossibleMoves[coordPiece].Count);

                Console.WriteLine("{0} to {1}", coordPiece, allPossibleMoves[coordPiece][nbMove]);

                board.SetPieceCoord(coordPiece, allPossibleMoves[coordPiece][nbMove]);
            }
        }*/

        public void Init()
        {
            int depth = 3;

            Tree<int> rootNode = new Tree<int>(-99, board.GetFen());

            if(colour == false)
            {
                revMaxPlayer = true;
            }
            else
            {
                revMaxPlayer = false;
            }

            int rootNodeValue = Minimax(rootNode, depth, nodeMinValue,  nodeMaxValue , true);

            foreach (Tree<int> childNode in rootNode.Children)
            {
                
                if(childNode.Value == rootNodeValue)
                {
                    board.LoadBoardWithFen(childNode.Edge);
                    rootNode.DeleteSubtree(); //delete all child nodes
                    break;
                }
            }
        }
        public int Minimax(Tree<int> node, int depth, int alpha, int beta, bool maximizingPlayer)
        {
            AddChildren(node, maximizingPlayer);
            node.Children.Sort((n1, n2) => n2.Value.CompareTo(n1.Value));
            // Check for terminal node or maximum depth
            if (depth == 0 || node.Children.Count == 0)
            {
                return node.Value;
            }

            // Recursively evaluate the children of the current node
            if (maximizingPlayer)
            {
                int minValue = nodeMinValue;
                int bestValue = nodeMaxValue;
                foreach (Tree<int> childNode in node.Children)
                {
                    // Evaluate child node and update alpha value
                    int childValue = Minimax(childNode, depth - 1, alpha, beta, false);
                    minValue = Math.Max(minValue, childValue);
                    alpha = Math.Max(alpha, minValue);
                    beta = Math.Min(beta, bestValue);

                    // Prune search if beta <= alpha
                    if (beta <= alpha)
                    {
                        break;
                        
                    }
                }
                node.Value = minValue;
            }
            else
            {
                int bestValue = nodeMaxValue;
                int minValue = nodeMinValue;
                foreach (Tree<int> childNode in node.Children)
                {
                    // Evaluate child node and update beta value
                    int childValue = Minimax(childNode, depth - 1, alpha, beta, true);
                    bestValue = Math.Min(bestValue, childValue);
                    beta = Math.Min(beta, bestValue);
                    alpha = Math.Max(alpha, minValue);

                    // Prune search if beta <= alpha
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                node.Value = bestValue;
            }
            return node.Value;
        }

        public void AddChildren(Tree<int> node, bool maximizingPlayer)
        {
            bool colourToMove = maximizingPlayer;
            int heuristicVal;

            if (revMaxPlayer)
            {
                colourToMove = !colourToMove;
            }

            foreach (KeyValuePair<string, List<string>> pieceMoves in board.GetAllMove(colourToMove))
            {
                foreach(string move in pieceMoves.Value)
                {
                    board.LoadBoardWithFen(node.Edge);

                    board.SetPieceCoord(pieceMoves.Key, move);

                    board.UpdateFen();

                    if (revMaxPlayer)
                    {
                        //Console.WriteLine(board.GetHeuristicValue());
                        heuristicVal = (int)(board.GetHeuristicValue() * (-1));
                    }
                    else
                    {
                        heuristicVal = (int)board.GetHeuristicValue();
                    }

                    node.AddChild(heuristicVal, board.GetFen());
                }
            }
        }
    }
}

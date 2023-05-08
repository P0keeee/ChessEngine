using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Piece : ICloneable
    {
        protected string pos;
        
        // true=white, false=black
        protected bool colour;

        protected List<string> possibleMoves;

        protected Board board;

        protected int value;

        public Piece(string initPos, bool initColour, Board initBoard)
        {
            pos = initPos;
            colour = initColour;
            board = initBoard;
        }

        public string GetPos()
        {
            return pos;
        }

        public virtual void SetPos(string newPos)
        {
            pos = newPos;
        }

        public bool GetColour()
        {
            return colour;
        }

        public int GetValue()
        {
            //Console.WriteLine(value);
            return value;
        }

        public virtual List<string> GetPossibleMove() 
        {
            if(board.GetColourTurn() == colour)
            {
                possibleMoves = board.FilterMove(possibleMoves, pos, colour);
            }
            
            return possibleMoves;
        }

        protected List<string> GetLineMove(int vertInc = 0, int horInc = 0)
        {
            List<int> IjCoord = board.CoordToIj(pos);

            List<string> lineMove = new List<string>();

            int i, j;

            for (int n = 1; n < board.GetBoardEdgeLen(); n++)
            {
                i = IjCoord[0] + n * vertInc;
                j = IjCoord[1] + n * horInc;

                if (board.InBoard(i, j))
                {
                    if (board.IsVoid(i, j))
                    {
                        lineMove.Add(board.IjToCoord(i, j));
                    }
                    else
                    {
                        if (board.IsKillable(board.IjToCoord(i, j), colour))
                        {
                            lineMove.Add(board.IjToCoord(i, j));
                        }

                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return lineMove;
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}

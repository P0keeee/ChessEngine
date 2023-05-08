using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    class King : Piece
    {
        private bool isMoved;
        public King(string initPos, bool initColour, Board initBoard) : base(initPos, initColour, initBoard) 
        {
            value = 999;
            isMoved = false;
        }

        public override List<string> GetPossibleMove()
        {
            possibleMoves = new List<string>();

            List<int> IjCoord = board.CoordToIj(pos);

            for(int i = IjCoord[0]-1; i<=IjCoord[0]+1; i++)
            {
                for(int j = IjCoord[1]-1; j<=IjCoord[1]+1; j++)
                {
                    if (board.InBoard(i,j))
                    {
                        if (board.IsVoid(i, j) || board.IsKillable(board.IjToCoord(i, j), colour))
                        {
                            possibleMoves.Add(board.IjToCoord(i, j));
                        }
                    }  
                }
            }

            possibleMoves = base.GetPossibleMove();

            return possibleMoves;
        }

        public override void SetPos(string newPos)
        {
            if (!isMoved) { isMoved = true; }
            base.SetPos(newPos);
        }
    }
}

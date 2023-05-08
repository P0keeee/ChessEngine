using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    class Pawn : Piece
    {
        private bool firstMove;
        
        public Pawn(string initPos, bool initColour, Board initBoard) : base(initPos, initColour, initBoard) 
        {
            value = 1;
            firstMove = true;
        }

        public override void SetPos(string newPos)
        {
            pos = newPos;

            if(firstMove)
            {
                firstMove = false;
            }
        }

        public override List<string> GetPossibleMove()
        {
            if(colour)
            {
                possibleMoves = CheckPath(-1);
            }
            else
            {
                possibleMoves = CheckPath(1);

            }

            possibleMoves = base.GetPossibleMove();

            return possibleMoves;
        }

        public List<string> CheckPath(int direction)
        {
            List<string> pathMoves = new List<string>();

            char[,] boardArr = board.GetBoardTab();

            List<int> ijCoord = board.CoordToIj(pos);

            char nextElem = boardArr[ijCoord[0] + direction, ijCoord[1]];

            if (nextElem == ' ')
            {
                pathMoves.Add(board.IjToCoord(ijCoord[0] + direction, ijCoord[1]));

                if (firstMove && boardArr[ijCoord[0] + direction*2, ijCoord[1]] == ' ')
                {
                    pathMoves.Add(board.IjToCoord(ijCoord[0] + direction*2, ijCoord[1]));
                }
            }

            if (ijCoord[1]-1 >= 0 && board.IsKillable(board.IjToCoord(ijCoord[0]+direction, ijCoord[1]-1),colour) == true)
            {
                pathMoves.Add(board.IjToCoord(ijCoord[0] + direction, ijCoord[1] - 1));
            }
            if (ijCoord[1]+1 < board.GetBoardEdgeLen() && board.IsKillable(board.IjToCoord(ijCoord[0]+direction, ijCoord[1]+1), colour) == true)
            {
                pathMoves.Add(board.IjToCoord(ijCoord[0] + direction, ijCoord[1] + 1));
            }

            return pathMoves;
        }

        public void IsMoved()
        {
            firstMove = false;
        }
    }
}

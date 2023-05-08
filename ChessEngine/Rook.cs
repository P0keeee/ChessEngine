using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    class Rook : Piece
    {
        private bool isMoved;

        public Rook(string initPos, bool initColour, Board initBoard) : base(initPos, initColour, initBoard) 
        {
            value = 5;
            isMoved = false;
        }

        public override List<string> GetPossibleMove()
        {
            possibleMoves = new List<string>();

            possibleMoves.AddRange(GetLineMove(vertInc : -1));
            possibleMoves.AddRange(GetLineMove(vertInc: 1));
            possibleMoves.AddRange(GetLineMove(horInc: -1));
            possibleMoves.AddRange(GetLineMove(horInc: 1));

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

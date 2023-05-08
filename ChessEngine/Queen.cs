using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    class Queen : Piece
    {
        public Queen(string initPos, bool initColour, Board initBoard) : base(initPos, initColour, initBoard) 
        {
            value = 9;
        }

        public override List<string> GetPossibleMove()
        {
            possibleMoves = new List<string>();

            Bishop fakeBishop = new Bishop(pos, colour, board);
            Rook fakeRook = new Rook(pos,colour,board);
            
            possibleMoves.AddRange(fakeBishop.GetPossibleMove());
            possibleMoves.AddRange(fakeRook.GetPossibleMove());

            return possibleMoves;
        }
    }
}

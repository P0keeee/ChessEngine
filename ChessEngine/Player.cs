using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Player
    {
        protected bool colour;
        protected Board board;

        public Player(bool initColour, Board initBoard)
        {
            colour = initColour;
            board = initBoard;
        }

        public virtual void GetNextMove()
        {

        }

        public bool GetColour()
        {
            return colour;
        }
    }
}

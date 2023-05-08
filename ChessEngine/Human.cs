using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    class Human : Player
    {
        private int moveCount;
        public Human(bool initColour, Board initBoard) : base(initColour, initBoard)
        {

        }

        public override void GetNextMove()
        {
            int oldMoveCount = moveCount;
            
            while(oldMoveCount == moveCount)
            {
                continue;
            }
        }

        public void IncrementMove()
        {
            moveCount++;
        }
    }
}

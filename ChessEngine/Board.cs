using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessEngine
{
    public class Board
    {
        private char[,] boardTab;
        private string[,] coordTab;
        protected Board board;
        private string baseFen;
        private string fen;
        private Player whitePlayer;
        private Player blackPlayer;

        // true=white, false=black
        private bool colourTurn;

        private bool isUpdated;
        public int heuristicValue;
        public int QueenX = 0;
        public int QueenY = 0;
        private List<Piece> piecesList;
        public int Won;
        public Board()
        {
            boardTab = new char[8, 8];

            InitCoord();
            InitPiece();

            baseFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            fen = baseFen;

            LoadBoardWithFen(fen);

            colourTurn = true;

            whitePlayer = new Human(true, this);
            blackPlayer = new AI(false, this);

            Thread interfaceThread = new Thread(new ThreadStart(StartInterface));
            interfaceThread.Start();

            isUpdated = false;

            StartGame();
        }

        private void InitCoord()
        { 
            int len = (int) Math.Sqrt(boardTab.Length);

            coordTab = new string[len,len];

            for (int i = 0; i < len; i++)
            {
                for(int j = 0; j < len; j++)
                {
                    char[] coord = { (char)(j + 'a'), (char)((len-i) + '0') };
                    coordTab[i, j] = new string(coord);
                }
            }
        }

        private void InitPiece()
        {
            piecesList = new List<Piece>();
            
            int len = (int)Math.Sqrt(boardTab.Length);

            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    AddPiece(boardTab[i, j], coordTab[i, j]);
                }
            }
        }

        private void StartInterface()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BoardForm(this));
        }

        private void StartGame()
        {
            while(GetAllMove(true).Count>0 && GetAllMove(false).Count>0)
            {
                //Console.WriteLine("Before :");
                //PrintBoard();
                
                if(colourTurn)
                {
                    if (whitePlayer.GetType() == typeof(Human))
                        isUpdated = true;


                    whitePlayer.GetNextMove();
                    colourTurn = false;
                }
                else
                {
                    if (blackPlayer.GetType() == typeof(Human))
                        isUpdated = true;

                    blackPlayer.GetNextMove();
                    colourTurn = true;
                }


                UpdateFen();
                Console.WriteLine(fen);
                //Console.WriteLine("After :");
                // PrintBoard();
            }

            isUpdated = true;

            if(IsChecked(true))
            {
                Console.WriteLine("Black win by checkmate");
                Won = 0;
            }
            else if (IsChecked(false))
            {
                Console.WriteLine("White win by checkmate");
                Won = 1;
            }
            else
            {
                Console.WriteLine("Draw");
                Won = 2;
            }
        }
        
        public char[,] GetBoardTab()
        {
            return boardTab;
        }

        public List<string> GetMove(string coord)
        {
            List<string> possibleMove = new List<string>();

            foreach(Piece piece in ClonePiecesList(piecesList))
            {
                if(piece.GetPos() == coord)
                {
                    possibleMove = piece.GetPossibleMove();
                    break;
                }
            }

            return possibleMove;
        }

        public Dictionary<string,List<string>> GetAllMove(bool colour)
        {
            Dictionary<string, List<string>> possibleMove = new Dictionary<string, List<string>>();

            foreach (Piece piece in ClonePiecesList(piecesList))
            {
                if(piece.GetColour() == colour && piece.GetPossibleMove().Count > 0)
                {
                    possibleMove.Add(piece.GetPos(), piece.GetPossibleMove());
                }
            }

            return possibleMove;
        }

        public List<Player> GetHumanPlayer()
        {
            List<Player> humanPlayerList = new List<Player>();

            if(whitePlayer.GetType() == typeof(Human))
            {
                humanPlayerList.Add(whitePlayer);
            }

            if(blackPlayer.GetType() == typeof(Human))
            {
                humanPlayerList.Add(blackPlayer);
            }

            return humanPlayerList;
        }

        public bool GetColourTurn()
        {
            return colourTurn;
        }

        public void SetPieceCoord(string pieceCoord, string newCoord)
        {
            foreach (Piece piece in piecesList)
            {
                if (piece.GetPos() == pieceCoord)
                {
                    List<int> oldPos = CoordToIj(pieceCoord);
                    List<int> newPos = CoordToIj(newCoord);

                    char pieceLetter = boardTab[oldPos[0], oldPos[1]];
                    boardTab[oldPos[0], oldPos[1]] = ' ';

                    if (boardTab[newPos[0], newPos[1]] != ' ')
                    {
                        foreach (Piece pieceToKill in piecesList)
                        {
                            if (pieceToKill.GetPos() == newCoord)
                            {
                                piecesList.Remove(pieceToKill);
                                break;
                            }
                        }
                    }
                    
                    // promotion
                    if (piece.GetType() == typeof(Pawn) && (newPos[0] == 0 || newPos[0] == GetBoardEdgeLen() - 1))
                    {
                        piecesList.Add(new Queen(newCoord, piece.GetColour(), this));

                        boardTab[newPos[0], newPos[1]] = 'Q';

                        piecesList.Remove(piece);
                    }
                    else
                    {
                        piece.SetPos(newCoord);
                        boardTab[newPos[0], newPos[1]] = pieceLetter;
                    }

                    break;
                }
            }
        }

        public List<int> CoordToIj(string coord)
        {
            List<int> ij = new List<int>
            {
                ((int)Math.Sqrt(coordTab.Length) - (coord[1] - '1')) - 1,
                coord[0] - 'a'
            };

            return ij;
        }
        public string IjToCoord(int i, int j)
        {
            return coordTab[i,j];
        }

        public bool InBoard(int i, int j)
        {
            int lenBoard = GetBoardEdgeLen();

            if(i<0 || j<0 || i>=lenBoard || j>=lenBoard)
            {
                return false;
            }

            return true;
        }

        public int GetBoardEdgeLen()
        {
            return (int)Math.Sqrt(boardTab.Length);
        }

        public bool IsKillable(string pieceToKillCoord, bool pieceToMoveColour)
        {
            List<int> ijCoord = CoordToIj(pieceToKillCoord);

            char toKill = boardTab[ijCoord[0], ijCoord[1]];

            if((Char.IsUpper(toKill) && pieceToMoveColour == false) || (Char.IsLower(toKill) && pieceToMoveColour == true))
            {
                return true;
            }

            return false;
        }

        public bool IsVoid(int i, int j)
        {
            if(boardTab[i,j] == ' ')
            {
                return true;
            }

            return false;
        }

        public bool IsChecked(bool kingColour)
        {
            string kingPos = "";

            foreach(Piece piece in piecesList)
            {
                if(piece.GetType() == typeof(King) && piece.GetColour() == kingColour)
                {
                    kingPos = piece.GetPos();
                }
            }

            foreach (KeyValuePair<string, List<string>> pieceMoves in GetAllMove(!kingColour))
            {
                foreach(string move in pieceMoves.Value)
                {
                    if(move==kingPos)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public List<string> FilterMove(List<string> currentMoves, string pieceCoord, bool colour)
        {
            List<string> filterMoves = new List<string>(currentMoves);

            char[,] oldBoardTab = (char[,]) boardTab.Clone();
            List<Piece> oldPiecesList = ClonePiecesList(piecesList);
                
            foreach(string move in currentMoves)
            {
                SetPieceCoord(pieceCoord, move);

                if(IsChecked(colour))
                {
                    filterMoves.Remove(move);
                }

                piecesList = ClonePiecesList(oldPiecesList);
                boardTab = (char[,])oldBoardTab.Clone();
            }

            return filterMoves;
        }

        public List<Piece> ClonePiecesList(List<Piece> piecesListToClone)
        {
            List<Piece> newPiecesList = new List<Piece>();

            foreach(Piece piece in piecesListToClone)
            {
                newPiecesList.Add((Piece) piece.Clone());
            }

            return newPiecesList;
        }

        public bool IsUpdated()
        {
            return isUpdated;
        }

        public void IsRefreshed()
        {
            isUpdated = false;
        }

        public int GetDistance(string coord1, string coord2)
        {
            List<int> src = CoordToIj(coord1);
            List<int> dst = CoordToIj(coord2);

            return Math.Abs(src[0] - dst[0]) + Math.Abs(src[1] - dst[1]);
        }
        public int GetHeuristicValue()
        {
            int heuristicValue = 0;
            int materialValue = 0;
            int positionValue = 0;
            foreach (Piece piece in this.piecesList.ToList())
            {
                //Console.WriteLine(piece);
                int pieceValue = (int)piece.GetValue();
                if (piece.GetColour() == true)
                {
                    materialValue += piece.GetValue();
                    switch (piece.GetType().Name)
                    {
                        case "Queen":
                            Queen WhiteQueen = (Queen)piece;
                            positionValue = EvaluateQueenPosition(WhiteQueen);
                            break;
                        case "Bishop":
                            Bishop WhiteBishop = (Bishop)piece;
                            positionValue = EvaluateBishopPosition(WhiteBishop);
                            break;
                        case "Knight":
                            Knight WhiteKnight = (Knight)piece;
                            positionValue = EvaluateKnightPosition(WhiteKnight);
                            break;
                        case "Rook":
                            Rook WhiteRook = (Rook)piece;
                            positionValue = EvaluateRookPosition(WhiteRook);
                            break;
                    }
                }
                else
                {
                    materialValue -= piece.GetValue();
                    switch (piece.GetType().Name)
                    {
                        case "Queen":
                            Queen BlackQueen = (Queen)piece;
                            positionValue = -(EvaluateQueenPosition(BlackQueen));
                            break;
                        case "Bishop":
                            Bishop BlackBishop = (Bishop)piece;
                            positionValue = -(EvaluateBishopPosition(BlackBishop));
                            break;
                        case "Knight":
                            Knight BlackKnight = (Knight)piece;
                            positionValue = -(EvaluateKnightPosition(BlackKnight));
                            break;
                        case "Rook":
                            Rook BlackRook = (Rook)piece;
                            positionValue = -(EvaluateRookPosition(BlackRook));
                            break;
                    }
                }
            }
            //Console.WriteLine(heuristicValue);
            heuristicValue = positionValue + materialValue;
            return heuristicValue;
        }

        public int EvaluateQueenPosition(Piece queen)
        {
            int score = 0;
            // Evaluate the position of the queen on the board
            // Evaluate number of squares attacked by the queen
            int numAttackedSquares = queen.GetPossibleMove().Count * 5;
            score += numAttackedSquares;

            return score;
        }
        public int EvaluateRookPosition(Piece rook)
        {
            int score = 0;
            int numAttackedSquares = rook.GetPossibleMove().Count * 5;
            score += numAttackedSquares;

            return score;
        }
        public int EvaluateBishopPosition(Piece bishop)
        {
            int score = 0;
            int numAttackedSquares = bishop.GetPossibleMove().Count * 5;
            score += numAttackedSquares;

            return score;
        }
        public int EvaluateKnightPosition(Piece knight)
        {
            int score = 0;
            int numAttackedSquares = knight.GetPossibleMove().Count * 5;
            score += numAttackedSquares;

            return score;
        }

        public string GetFen()
        {
            return fen;
        }

        public void UpdateFen()
        {
            string newFen = "";
            int cptVoid = 0;

            for(int i=0; i<GetBoardEdgeLen(); i++)
            {
                for(int j=0; j<GetBoardEdgeLen(); j++)
                {
                    if(boardTab[i,j] != ' ')
                    {
                        if(cptVoid > 0)
                        {
                            newFen += cptVoid.ToString();
                        }

                        cptVoid = 0;
                        
                        newFen += boardTab[i, j];
                    }
                    else
                    {
                        cptVoid++;
                    }
                }

                if (cptVoid > 0)
                {
                    newFen += cptVoid.ToString();
                }

                cptVoid = 0;

                newFen += '/';
            }

            newFen = newFen.Remove(newFen.Length - 1);

            if (colourTurn)
            {
                newFen += " w";
            }
            else
            {
                newFen += " b";
            }

            King whiteKing = null;
            King blackKing = null;
            List<Rook> whiteRooks = new List<Rook>();
            List<Rook> blackRooks = new List<Rook>();

            foreach (Piece piece in piecesList)
            {
                if (piece.GetType() == typeof(King))
                {
                    if (piece.GetColour() == true)
                    {
                        whiteKing = (King) piece;
                    }
                    else
                    {
                        blackKing = (King) piece;
                    }
                }
                else if (piece.GetType() == typeof(Rook))
                {
                    if (piece.GetColour() == true)
                    {
                        whiteRooks.Add((Rook) piece);
                    }
                    else
                    {
                        blackRooks.Add((Rook) piece);
                    }
                }
            }
            fen = newFen;
        }


        public void LoadBoardWithFen(string fen)
        {
            piecesList.Clear();

            string[] splitFen = fen.Split(' ');

            int i=0;
            int j=0;

            foreach(string row in splitFen[0].Split('/'))
            {
                foreach(char letter in row.ToCharArray())
                {
                    if (Char.IsDigit(letter))
                    {
                        int nbVoid = (int)(letter - '0');

                        for(int k=j; k<nbVoid+j; k++)
                        {
                            boardTab[i, k] = ' ';
                        }

                        j += nbVoid;
                    }
                    else
                    {
                        boardTab[i, j] = letter;
                        AddPiece(letter, coordTab[i, j]);
                        j++;
                    }
                }

                j = 0;
                
                i++;
            }

            if(splitFen[1] == "w")
            {
                colourTurn = true;
            }
            else
            {
                colourTurn = false;
            }
            CheckPawnMove();
        }

        private void CheckPawnMove()
        {
            foreach(Piece piece in piecesList)
            {
                if(piece.GetType() == typeof(Pawn))
                {
                    Pawn pawn = (Pawn)piece;
                    
                    List<int> pawnPos = CoordToIj(piece.GetPos());
                    
                    if(piece.GetColour() && pawnPos[0] < GetBoardEdgeLen() - 2)
                    {
                        pawn.IsMoved();
                    }
                    if (!piece.GetColour() && pawnPos[0] > 1)
                    {
                        pawn.IsMoved();
                    }
                }
            }
        }

        private void AddPiece(char letter, string coord)
        {
            switch ((PieceEnum)letter)
            {
                case PieceEnum.WhiteKing:
                    piecesList.Add(new King(coord, true, this));
                    break;
                case PieceEnum.WhiteQueen:
                    piecesList.Add(new Queen(coord, true, this));
                    break;
                case PieceEnum.WhiteRook:
                    piecesList.Add(new Rook(coord, true, this));
                    break;
                case PieceEnum.WhiteKnight:
                    piecesList.Add(new Knight(coord, true, this));
                    break;
                case PieceEnum.WhiteBishop:
                    piecesList.Add(new Bishop(coord, true, this));
                    break;
                case PieceEnum.WhitePawn:
                    piecesList.Add(new Pawn(coord, true, this));
                    break;
                case PieceEnum.BlackKing:
                    piecesList.Add(new King(coord, false, this));
                    break;
                case PieceEnum.BlackQueen:
                    piecesList.Add(new Queen(coord, false, this));
                    break;
                case PieceEnum.BlackRook:
                    piecesList.Add(new Rook(coord, false, this));
                    break;
                case PieceEnum.BlackKnight:
                    piecesList.Add(new Knight(coord, false, this));
                    break;
                case PieceEnum.BlackBishop:
                    piecesList.Add(new Bishop(coord, false, this));
                    break;
                case PieceEnum.BlackPawn:
                    piecesList.Add(new Pawn(coord, false, this));
                    break;

            }
        }
    }
}

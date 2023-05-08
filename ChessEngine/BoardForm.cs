using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessEngine
{
    public partial class BoardForm : Form
    {
        private Board board;

        private char[,] lastBoardArr;

        private Size boardSize;
        private int nbBoxEdge;
        private int boxSize;

        private Color lightColour;
        private Color darkColour;

        private List<string> movesToDisp;

        private Dictionary<PieceEnum, Bitmap> piecesImage;

        private string pieceCoord;
        private string moveCoord;

        private List<Player> player;
        public BoardForm(Board initBoard)
        {
            InitializeComponent();

            board = initBoard;
            lastBoardArr = (char [,]) board.GetBoardTab().Clone();

            char[,] boardTab = board.GetBoardTab();

            player = board.GetHumanPlayer();

            movesToDisp = new List<string>();

            boardSize = boardDisp.Size;
            nbBoxEdge = (int)Math.Sqrt(boardTab.Length);
            boxSize = boardSize.Width / nbBoxEdge;

            piecesImage = new Dictionary<PieceEnum, Bitmap>(); 

            lightColour = Color.FromArgb(255, 245, 204);
            darkColour = Color.FromArgb(196, 164, 132);

            Bitmap sprite = new Bitmap("../../pieces_sprite.png");
            int pieceSize = sprite.Height / 2;
            int cropx = 0;
            int cropy = 0;
            

            foreach(PieceEnum piece in Enum.GetValues(typeof(PieceEnum)))
            {
                switch(piece)
                {
                    case PieceEnum.WhiteKing:
                        cropx = 0 * pieceSize;
                        cropy = 0 * pieceSize;
                        break;
                    case PieceEnum.WhiteQueen:
                        cropx = 1 * pieceSize;
                        cropy = 0 * pieceSize;
                        break;
                    case PieceEnum.WhiteRook:
                        cropx = 4 * pieceSize;
                        cropy = 0 * pieceSize;
                        break;
                    case PieceEnum.WhiteKnight:
                        cropx = 3 * pieceSize;
                        cropy = 0 * pieceSize;
                        break;
                    case PieceEnum.WhiteBishop:
                        cropx = 2 * pieceSize;
                        cropy = 0 * pieceSize;
                        break;
                    case PieceEnum.WhitePawn:
                        cropx = 5 * pieceSize;
                        cropy = 0 * pieceSize;
                        break;
                    case PieceEnum.BlackKing:
                        cropx = 0 * pieceSize;
                        cropy = 1 * pieceSize;
                        break;
                    case PieceEnum.BlackQueen:
                        cropx = 1 * pieceSize;
                        cropy = 1 * pieceSize;
                        break;
                    case PieceEnum.BlackRook:
                        cropx = 4 * pieceSize;
                        cropy = 1 * pieceSize;
                        break;
                    case PieceEnum.BlackKnight:
                        cropx = 3 * pieceSize;
                        cropy = 1 * pieceSize;
                        break;
                    case PieceEnum.BlackBishop:
                        cropx = 2 * pieceSize;
                        cropy = 1 * pieceSize;
                        break;
                    case PieceEnum.BlackPawn:
                        cropx = 5 * pieceSize;
                        cropy = 1 * pieceSize;
                        break;

                }

                piecesImage.Add(piece, ResizeImage(CropImage(sprite, cropx, cropy, pieceSize, pieceSize),boxSize,boxSize));
            }     
        }

        private Bitmap CropImage(Bitmap src, int newx, int newy, int width, int height)
        {
            Rectangle cropRect = new Rectangle(newx, newy, width, height);

            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
            }

            return target;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void BoardClick(object sender, EventArgs e)
        {
            MouseEventArgs mouseEvent = (MouseEventArgs)e;

            char[,] boardTab = board.GetBoardTab();

            int posi = (int)Math.Floor((double)mouseEvent.Y / boxSize);
            int posj = (int)Math.Floor((double)mouseEvent.X / boxSize);

            bool currentColour = board.GetColourTurn();

            foreach(Human human in player)
            {
                if(human.GetColour() == currentColour)
                {
                    if (movesToDisp.Count == 0)
                    {
                        if ((Char.IsLower(boardTab[posi, posj]) && currentColour == false) || (Char.IsUpper(boardTab[posi,posj]) && currentColour == true))
                        {
                            pieceCoord = board.IjToCoord(posi, posj);

                            movesToDisp = board.GetMove(pieceCoord);

                            Refresh();
                        }
                    }
                    else
                    {
                        moveCoord = board.IjToCoord(posi, posj);

                        if (movesToDisp.FindIndex(board.IjToCoord(posi, posj).Contains) >= 0)
                        {
                            board.SetPieceCoord(pieceCoord, moveCoord);
                            lastBoardArr = (char[,])board.GetBoardTab().Clone();
                            human.IncrementMove();
                            movesToDisp.Clear();
                        }
                        else
                        {
                            movesToDisp.Clear();
                        }

                        Refresh();
                    }
                }
            }
        }

        private void BoardPaint(object sender, PaintEventArgs e)
        {
            char[,] boardTab = lastBoardArr;

            Graphics g = e.Graphics;
           
            SolidBrush brush = new SolidBrush(lightColour);

            for (int i=0; i<nbBoxEdge; i++)
            {
                for(int j=0; j<nbBoxEdge; j++)
                {
                    if((i%2 == 0 && j%2 == 0) || (i%2 != 0 && j%2 != 0))
                    {
                        brush.Color = lightColour;
                    }
                    else
                    {
                        brush.Color = darkColour;
                    }

                    Rectangle rect = new Rectangle(i*boxSize, j*boxSize, boxSize, boxSize);

                    g.FillRectangle(brush, rect);
                }
            }
            

            

            RectangleF boxRect = new RectangleF(0, 0, boxSize, boxSize);
            GraphicsUnit units = GraphicsUnit.Pixel;

            for (int i = 0; i < nbBoxEdge; i++)
            {
                for (int j = 0; j < nbBoxEdge; j++)
                {
                    if(boardTab[i,j] != ' ')
                    {
                        g.DrawImage(piecesImage[(PieceEnum) boardTab[i,j]], j * boxSize, i * boxSize, boxRect, units);
                    }
                }
            }
        }
        private void BoardRefresh(object sender, EventArgs e)
        {
            if(board.IsUpdated())
            {
                lastBoardArr = (char[,])board.GetBoardTab().Clone();
                Refresh();
                board.IsRefreshed();
            }
        }

        private void dispWon_TextChanged(object sender, EventArgs e)
        {

        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            board.LoadBoardWithFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            //refresh bitmap
            Tree<int> rootNode = new Tree<int>(-99, board.GetFen());
            foreach(Tree<int> childNode in rootNode.Children)
            {
                rootNode.DeleteSubtree();
                break;
            }
            lastBoardArr = (char[,])board.GetBoardTab().Clone();
            Close();
            new Board();
            Refresh();
        }

        private void BoardForm_Load(object sender, EventArgs e)
        {

        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Domain
{
    public class Board
    {
        public SquaresCollection Squares;

        public List<Piece> Pieces;

        public Size Size = new Size(8, 8);

        public int SquareWidth = 75;
        public int PieceSize = 55;


        public Board()
        {
            this.Squares = new SquaresCollection();
            this.Pieces = new List<Piece>();

            SquareType squareType = SquareType.dark;            

            for(int col = 0; col < this.Size.Height; col++)
            {
                squareType = (squareType == SquareType.dark) ? SquareType.light : SquareType.dark;

                for (int row = 0; row < this.Size.Width; row++)
                {
                    squareType = (squareType == SquareType.dark) ? SquareType.light : SquareType.dark;                    

                    Square square = new Square(squareType, col, row);
                    this.Squares.Add(square);                    
                }
            }            
        }

        public void AddPiece(Team team, int colIndexPos, int rowIndexPos)
        {
            Square square = Squares.findBySquareColAndIndex(colIndexPos, rowIndexPos);

            Piece piece = new Piece(team, square);
            Pieces.Add(piece);
        }

        public void AddPiece(Team team, Square square)
        {
            Piece piece = new Piece(team, square);
            Pieces.Add(piece);
        }


        public Point getSquarePosition(Square square)
        {
            return new Point(square.ColumnIndex * this.SquareWidth, square.RowIndex * this.SquareWidth);
        }

        public Size getSquareSize()
        {
            return new Size(this.SquareWidth, this.SquareWidth);
        }

        public Point getPiecePosition(Piece piece)
        {
            int addToSquare = (this.SquareWidth - this.PieceSize) / 2;

            Point point = this.getSquarePosition(piece.CurrentSquare);
            point.Offset(addToSquare, addToSquare);

            return point;
        }

        public Point getPiecePositionOnSquare(Square square)
        {
            int addToSquare = (this.SquareWidth - this.PieceSize) / 2;

            Point point = this.getSquarePosition(square);
            point.Offset(addToSquare, addToSquare);

            return point;
        }

        public void MovePiece(CheckersMove move, CheckersMoveLogger logger)
        {
            logger.addLog(new CheckersLog(move));
            move.Piece.CurrentSquare = move.DestinationSquare;

            if(move.KilledPiece != null)
                deletePiece(move.KilledPiece);

            if (move.DestinationSquare.RowIndex == 0 && move.Piece.Team == Team.Team2)
                move.Piece.IsKing = true;
            else if (move.DestinationSquare.RowIndex == this.Size.Height - 1 && move.Piece.Team == Team.Team1)
                move.Piece.IsKing = true;            
        }

        private void deletePiece(Piece piece)
        {
            piece.CurrentSquare = null;
            Pieces.Remove(piece);
        }

        public Size getPieceSize()
        {
            return new Size(this.PieceSize, this.PieceSize);
        }

        public Piece getPieceAtPoint(Point point)
        {
            foreach (Piece piece in this.Pieces)
            {
                Point piecePosition = this.getPiecePosition(piece);

                if ((point.X >= piecePosition.X && point.X < piecePosition.X + PieceSize) &&
                    (point.Y >= piecePosition.Y && point.Y < piecePosition.Y + PieceSize))
                {
                    return piece;
                }
            }

            return null;
        }
        
        public CheckersMove getAllowedMoveAtPoint(Piece piece, Point point, bool KillMoveOnly = false)
        {
            List<CheckersMove> moves = this.getAllowedMovesForPiece(piece);
            if (KillMoveOnly)
                moves = moves.Where(x => x.KilledPiece != null).ToList<CheckersMove>();

            foreach (CheckersMove move in moves)
            {
                Point squarePosition = this.getSquarePosition(move.DestinationSquare);

                if((point.X >= squarePosition.X && point.X < squarePosition.X + SquareWidth) &&
                    (point.Y >= squarePosition.Y && point.Y < squarePosition.Y + SquareWidth))
                {
                    return move;
                }
            }

            return null;
        }

        public List<CheckersMove> getAllowedMovesForPiece(Piece piece)
        {
            List<CheckersMove> moves = new List<CheckersMove>();

            List<Square> standardMovesSquares = new List<Square>();
            List<Square> killMovesSquares = new List<Square>();

            int colIndex = piece.CurrentSquare.ColumnIndex;
            int rowIndex = piece.CurrentSquare.RowIndex;

            if (piece.IsKing == true || (piece.Team == Team.Team1 && piece.IsKing == false))
            {
                standardMovesSquares.Add(Squares.findBySquareColAndIndex(colIndex - 1, rowIndex + 1));
                standardMovesSquares.Add(Squares.findBySquareColAndIndex(colIndex + 1, rowIndex + 1));
                killMovesSquares.Add(Squares.findBySquareColAndIndex(colIndex - 2, rowIndex + 2));
                killMovesSquares.Add(Squares.findBySquareColAndIndex(colIndex + 2, rowIndex + 2));
            }

            if (piece.IsKing == true || (piece.Team == Team.Team2 && piece.IsKing == false))
            {
                standardMovesSquares.Add(Squares.findBySquareColAndIndex(colIndex + 1, rowIndex - 1));
                standardMovesSquares.Add(Squares.findBySquareColAndIndex(colIndex - 1, rowIndex - 1));
                killMovesSquares.Add(Squares.findBySquareColAndIndex(colIndex + 2, rowIndex - 2));
                killMovesSquares.Add(Squares.findBySquareColAndIndex(colIndex - 2, rowIndex - 2));
            }
           
            for(int x = 0; x < standardMovesSquares.Count; x++)
            {
                Square standardMoveSquare;
                Square killMoveSquare;
                CheckersMove move = null;

                if(standardMovesSquares[x] != null)
                {
                    standardMoveSquare = standardMovesSquares[x];
                    killMoveSquare = killMovesSquares[x];

                    //S'il y a un pion de l'autre équipe où notre déplacement serait possible, mais qu'il n'y en à pas de l'autre côté, on peut le manger.
                    if (standardMoveSquare.Piece != null && standardMoveSquare.Piece.Team != piece.Team && killMoveSquare != null && killMoveSquare.Piece == null)
                        move = new CheckersMove(piece, killMoveSquare, standardMoveSquare.Piece);
                    else if (standardMoveSquare.Piece == null)
                        move = new CheckersMove(piece, standardMoveSquare);
                }

                if (move != null)
                    moves.Add(move);
            }

            if (moves.Any(x => x.KilledPiece != null))
                moves = moves.Where(x => x.KilledPiece != null).ToList<CheckersMove>();

            return moves;
        }

        public List<CheckersMove> getKillMovesForPiece(Piece piece)
        {
            return this.getAllowedMovesForPiece(piece).Where(x => x.KilledPiece != null).ToList<CheckersMove>();
        }
        
        public List<Piece> GetPiecesAllowedToMove(Team team)
        {
            List<Piece> piecesAllowedToMove = new List<Piece>();
            List<Piece> pieces = Pieces.Where(x => x.Team == team).ToList<Piece>();

            foreach(Piece piece in pieces){
                if (this.getAllowedMovesForPiece(piece).Count > 0)
                    piecesAllowedToMove.Add(piece);
            }


            //Juste les pièces qui peuvent manger une autre ?
            bool killMoveOnly = piecesAllowedToMove.Any(x => getAllowedMovesForPiece(x).Any(y => y.KilledPiece != null));

            if (killMoveOnly)
                piecesAllowedToMove = piecesAllowedToMove.Where(x => getAllowedMovesForPiece(x).Any(y => y.KilledPiece != null)).ToList<Piece>();

            return piecesAllowedToMove;
        }

        public int PiecesCountByTeam(Team team)
        {
            return Pieces.Count(x => x.Team == team);
        }

        public Rectangle GetKingLogoPositionRectangle(Piece piece)
        {
            int rectWidth = 30;
            int rectHeight = 30;


            Point position = this.getPiecePosition(piece);

            position.Offset(this.PieceSize / 2 - rectWidth / 2, this.PieceSize / 2 - rectHeight / 2);

            return new Rectangle(position, new Size(rectWidth, rectHeight));
        }
    }
}

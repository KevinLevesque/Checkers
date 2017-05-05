using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Drawing
{
    public class GamePanelDrawer
    {

        private CheckersGame controller;

        public GamePanelDrawer()
        {
            this.controller = CheckersGame.getInstance();
        }


        public void draw(Graphics g)
        {
            drawBoard(g);
            drawPieces(g);
            drawAllowedMoves(g);
        }

 

        private void drawBoard(Graphics g)
        {
            Brush brush = null;

            Brush penDark = new SolidBrush(Color.DarkGray);
            Brush penLight = new SolidBrush(Color.LightGray);


            for (int row = 0; row < controller.Board.Size.Width; row++)
            {
                foreach (Square square in controller.Board.Squares.getRow(row))
                {                    
                    if (square.SquareType == SquareType.dark)
                        brush = penDark;
                    else
                        brush = penLight;

                    g.FillRectangle(brush, new Rectangle(controller.Board.getSquarePosition(square), controller.Board.getSquareSize()));
                }
            }
        }

        private void drawPieces(Graphics g)
        {
            Brush brush = null;

            Brush team1Brush = new SolidBrush(Color.Red);
            Brush team2Brush = new SolidBrush(Color.Black);


            foreach(Piece piece in controller.Board.Pieces)
            {
                if (piece.Team == PieceTeam.Team1)
                {
                    brush = team1Brush;
                }
                else
                {
                    brush = team2Brush;
                }


                if (piece == controller.HilightedPiece)
                    brush = new SolidBrush(Color.Yellow);


                g.FillEllipse(brush, new Rectangle(controller.Board.getPiecePosition(piece), controller.Board.getPieceSize()));

                if (piece.IsKing)
                {
                    StringFormat format = new StringFormat();
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Center;

                    g.DrawString("K", new Font("Arial", 16), Brushes.White, controller.Board.GetKingLogoPositionRectangle(piece), format);
                }
                   
            }

            foreach(Piece piece in controller.PiecesAllowedToMove){
                g.DrawEllipse(Pens.Aqua, new Rectangle(controller.Board.getPiecePosition(piece), controller.Board.getPieceSize()));
            }

        }


        private void drawAllowedMoves(Graphics g)
        {
            foreach(CheckersMove move in controller.Moves)
            {
                g.DrawEllipse(Pens.Yellow, new Rectangle(controller.Board.getPiecePositionOnSquare(move.DestinationSquare), controller.Board.getPieceSize()));
            }
        }




    }
}

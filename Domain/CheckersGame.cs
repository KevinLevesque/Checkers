using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Domain
{
    public enum Player { None, Player1, Player2 }


    public class CheckersGame
    {

        


        private static CheckersGame instance;

        public Board Board { get; private set; }

        public Piece HilightedPiece { get; private set; }
        public Piece SelectedPiece { get; private set; }
        public List<Piece> PiecesAllowedToMove { get; set; }
        public List<CheckersMove> Moves { get; private set; }
        public Player CurrentPlayer;
        public Player Winner;

        public bool ForceKill = false;

        private Point mousePosition;
        public Point MousePosition
        {
            get { return this.mousePosition; }
            set {
                this.mousePosition = value;
                setHighlightPiece();
                }
        }

        public static CheckersGame getInstance()
        {
            if (instance == null)
                instance = new CheckersGame();

            return instance;
        }


        public CheckersGame()
        {
            Board = new Board();
            MousePosition = new Point(0, 0);
            Moves = new List<CheckersMove>();
            

            CreatePieces();

            CurrentPlayer = Player.Player2;
            Winner = Player.None;
            NextTurn();
        }




        private void setHighlightPiece()
        {
            if (SelectedPiece != null)
            {
                HilightedPiece = SelectedPiece;
                return;
            }

            HilightedPiece = Board.getPieceAtPoint(this.MousePosition);
        }

        



        private void CreatePieces()
        {
            for(int row = 0; row < 8; row++)
            {
                if ((row < 3 || row > 4))
                {
                    foreach (Square square in Board.Squares.getRow(row).Where(x => x.SquareType == SquareType.dark))
                    {
                        Board.AddPiece((row < 3) ? PieceTeam.Team1 : PieceTeam.Team2, square);
                    }
                }                    
            }
        }


        public void ClickOnBoard()
        {
            if (ForceKill)
            {
                CheckersMove move = Board.getAllowedMoveAtPoint(SelectedPiece, this.MousePosition, true);
                if (move != null)
                {
                    Board.MovePiece(move);
                    if (Board.getKillMovesForPiece(move.Piece).Count == 0)
                    {
                        NextTurn();
                    }
                    else
                    {
                        Moves = Board.getKillMovesForPiece(SelectedPiece);
                    }
                }
                return;
            }


            if (SelectedPiece != null)
            {
                CheckersMove move = Board.getAllowedMoveAtPoint(SelectedPiece, this.MousePosition);
                if (move != null)
                {
                    Board.MovePiece(move);
                    //Si on a tué aucune pièce ou qu'on ne peut pas en tuer d'autre, on passe au prochain tour
                    if(move.KilledPiece == null || (move.KilledPiece != null && Board.getKillMovesForPiece(move.Piece).Count == 0))
                    {
                        NextTurn();
                    }
                    else
                    {
                        ForceKill = true;
                        Moves = Board.getKillMovesForPiece(SelectedPiece);
                        
                    }
                    return;
                }                
            }
            
            
            SelectedPiece = Board.getPieceAtPoint(this.MousePosition);

            if (!PiecesAllowedToMove.Contains(SelectedPiece))
                SelectedPiece = null;

            if (SelectedPiece != null)
            {
                if ((SelectedPiece.Team == PieceTeam.Team1 && CurrentPlayer == Player.Player1) || (SelectedPiece.Team == PieceTeam.Team2 && CurrentPlayer == Player.Player2))
                    Moves = Board.getAllowedMovesForPiece(SelectedPiece);
                else
                    SelectedPiece = null;

            }
            else
            {
                Moves.Clear();
                SelectedPiece = null;
            }

        }


        public void NextTurn()
        {
            CurrentPlayer = (CurrentPlayer == Player.Player1) ? Player.Player2 : Player.Player1;
            ForceKill = false;
            SelectedPiece = null;
            Moves.Clear();


            PiecesAllowedToMove = Board.GetPiecesAllowedToMove(CurrentPlayer == Player.Player1 ?PieceTeam.Team1 :PieceTeam.Team2);
            if(PiecesAllowedToMove.Count == 0)
            {
                Winner = CurrentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
            }
            

            //ToDo : Le joueur n'a peut-être aucune posibilité de mouvement, l'autre joueur gagne automatiquement
        }
    }
}

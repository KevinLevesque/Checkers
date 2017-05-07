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

    public class CheckersGame
    {

        


        private static CheckersGame instance;

        public List<UIListener> listeners;

        public Board Board { get; private set; }

        public CheckersMoveLogger logger;

        public Piece HilightedPiece { get; private set; }
        public Piece SelectedPiece { get; private set; }
        public List<Piece> PiecesAllowedToMove { get; set; }
        public List<CheckersMove> Moves { get; private set; }
        public Team CurrentPlayer;
        

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
            listeners = new List<UIListener>();

            Reset();
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
                        Board.AddPiece((row < 3) ? Team.Team1 : Team.Team2, square);
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
                    Board.MovePiece(move, logger);
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
                    Board.MovePiece(move, logger);
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
                if ((SelectedPiece.Team == Team.Team1 && CurrentPlayer == Team.Team1) || (SelectedPiece.Team == Team.Team2 && CurrentPlayer == Team.Team2))
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
            CurrentPlayer = (CurrentPlayer == Team.Team1) ? Team.Team2: Team.Team1;
            ForceKill = false;
            SelectedPiece = null;
            Moves.Clear();

            notifyUpdateCurrentPlayer(CurrentPlayer == Team.Team1 ? "Joueur 1" : "Joueur 2");

            PiecesAllowedToMove = Board.GetPiecesAllowedToMove(CurrentPlayer == Team.Team1 ? Team.Team1 : Team.Team2);
            if(PiecesAllowedToMove.Count == 0)
            {
                Team Winner = (CurrentPlayer == Team.Team1 ? Team.Team2 : Team.Team1);
                notifyUpdateWinner(Winner == Team.Team1 ? "Joueur 1" : "Joueur 2");
            }
            

            //ToDo : Le joueur n'a peut-être aucune posibilité de mouvement, l'autre joueur gagne automatiquement
        }


        public void addToUIListeners(UIListener listener)
        {
            listeners.Add(listener);
        }

 
        public void notifyUpdateWinner(string winner)
        {
            foreach (UIListener listener in listeners)
            {
                listener.UpdateWinner(winner);
            }
        }

        public void notifyUpdateCurrentPlayer(string currentPlayer)
        {
            foreach (UIListener listener in listeners)
            {
                listener.UpdateCurrentPlayer(currentPlayer);
            }
        }

        public void Reset()
        {
            Board = new Board();
            MousePosition = new Point(0, 0);
            Moves = new List<CheckersMove>();
            logger = new CheckersMoveLogger(listeners);



            CreatePieces();

            CurrentPlayer = Team.Team2;
            NextTurn();
        }
    }
}

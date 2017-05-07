using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class CheckersLog
    {

        
        private Piece pieceMoved;
        private Square moveFrom;
        private Square moveTo;
        private Piece killedPiece;
        private Square killedPieceSquare;


        public CheckersLog(CheckersMove move)
        {
           

            pieceMoved = move.Piece;
            moveFrom = move.Piece.CurrentSquare;
            moveTo = move.DestinationSquare;

            if (move.KilledPiece != null)
            {
                killedPiece = move.KilledPiece;
                killedPieceSquare = move.KilledPiece.CurrentSquare;
            }
        }


        public override string ToString()
        {
            string team = pieceMoved.Team == Team.Team1 ? "Joueur 1" : "Joueur 2";
            string pieceName = (pieceMoved.IsKing ? "de la dame" : "du pion");
            string killedPieceString = (killedPiece != null ? $", en éliminant la pièce en case {killedPieceSquare.Id}" : "");

            return $"{team} : Déplacement {pieceName} de la case {moveFrom.Id} jusqu'à la case {moveTo.Id}{killedPieceString}";
        }

    }
}

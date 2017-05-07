namespace Domain
{
    public class CheckersMove
    {

        public Piece Piece;
        public Square DestinationSquare;
        public Piece KilledPiece;


        public CheckersMove(Piece piece, Square destSquare, Piece killedPiece = null)
        {
            this.Piece = piece;
            this.DestinationSquare = destSquare;
            this.KilledPiece = killedPiece;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{


    public class Piece
    {


        public Team Team;
        public bool IsKing;
        private Square currentSquare;


        public Piece(Team team, Square currentSquare)
        {
            this.Team = team;
            this.IsKing = false;

            this.CurrentSquare = currentSquare;
        }

        public Square CurrentSquare
        {
            get { return currentSquare; }
            set
            {
                if(value == null)
                {
                    this.currentSquare.Piece = null;
                    this.currentSquare = null;
                }
                else
                {
                    if(this.currentSquare != null && this.currentSquare.Piece != null) this.currentSquare.Piece = null;

                    this.currentSquare = value;
                    this.currentSquare.Piece = this;
                }
            }
        }


        public void Move(Board board, Square destSquare)
        {
            
            //Déplacement double, on mange un autre pièce
            if (this.CurrentSquare.RowIndex + 2 == destSquare.RowIndex || this.CurrentSquare.RowIndex - 2 == destSquare.RowIndex)
            {

            }
        }




    }
}

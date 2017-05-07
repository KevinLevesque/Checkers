using System.Collections.Generic;

namespace Domain
{
    public class SquaresCollection : System.Collections.Generic.List<Square>
    {


        public Square findBySquareColAndIndex(int colIndex, int rowIndex)
        {
            foreach(Square s in this)
            {
                if (s.ColumnIndex == colIndex && s.RowIndex == rowIndex)
                    return s;                
            }

            return null;
        }

        public List<Square> getRow(int rowIndex)
        {
            List<Square> squares = new List<Square>();

            foreach (Square s in this)
            {
                if (s.RowIndex == rowIndex)
                    squares.Add(s);
            }

            return squares;
        }
      
    }
}

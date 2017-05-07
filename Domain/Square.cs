using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{

    public enum SquareType
    {
        light, dark
    }

    public class Square
    {

        public SquareType SquareType { get; set; }
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
        public Piece Piece { get; set; }

        public string Id
        {
            get
            {
                String columnLetter = (((Char)(65 + this.ColumnIndex )).ToString());
                return $"{columnLetter}{RowIndex+1}";
            }
        }

       
        public Square(SquareType type, int columnIndex, int rowIndex)
        {
            this.SquareType = type;
            this.ColumnIndex = columnIndex;
            this.RowIndex = rowIndex;
        }


        


    }
}

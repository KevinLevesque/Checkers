using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface UIListener
    {

        void UpdateLog(string log);
        void UpdateWinner(string winner);
        void UpdateCurrentPlayer(string currentPlayer);

    }
}

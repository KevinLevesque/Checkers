namespace Domain
{
    public interface UIListener
    {

        void UpdateLog(string log);
        void UpdateWinner(string winner);
        void UpdateCurrentPlayer(string currentPlayer);

    }
}

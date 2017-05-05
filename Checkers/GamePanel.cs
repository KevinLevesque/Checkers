using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Drawing;

namespace Checkers
{
    class GamePanel : System.Windows.Forms.Panel
    {

        private GamePanelDrawer gamePanelDrawer;
        CheckersGame CheckersGame;


        public GamePanel()
        {
            this.gamePanelDrawer = new GamePanelDrawer();
            base.DoubleBuffered = true;

            CheckersGame = CheckersGame.getInstance();
            
        }
        

        public void repaint(System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gamePanelDrawer.draw(e.Graphics);

            if ((CheckersGame.Board.getPieceAtPoint(CheckersGame.MousePosition) != null))
                Cursor = System.Windows.Forms.Cursors.Hand;
            else
                Cursor = System.Windows.Forms.Cursors.Default;

            
        }

    }
}

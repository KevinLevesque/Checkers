using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dames
{
    public partial class Form1 : Form
    {

        CheckersGame controller;

        public Form1()
        {
            InitializeComponent();

            controller = CheckersGame.getInstance();
            gamePanel2.Paint += GamePanel2_Paint;
        }

        private void GamePanel2_Paint(object sender, PaintEventArgs e)
        {
            gamePanel2.repaint(e);
        }

        private void gamePanel2_Click(object sender, EventArgs e)
        {
            controller.ClickOnBoard();
            gamePanel2.Refresh();
        }

        private void gamePanel2_MouseMove(object sender, MouseEventArgs e)
        {
            controller.MousePosition = e.Location;
            gamePanel2.Refresh();
        }

        private void gamePanel2_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}

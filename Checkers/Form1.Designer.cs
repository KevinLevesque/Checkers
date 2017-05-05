namespace Dames
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gamePanel2 = new Checkers.GamePanel();
            this.SuspendLayout();
            // 
            // gamePanel2
            // 
            this.gamePanel2.Location = new System.Drawing.Point(78, 12);
            this.gamePanel2.Name = "gamePanel2";
            this.gamePanel2.Size = new System.Drawing.Size(786, 628);
            this.gamePanel2.TabIndex = 0;
            this.gamePanel2.Click += new System.EventHandler(this.gamePanel2_Click);
            this.gamePanel2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gamePanel2_MouseClick);
            this.gamePanel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gamePanel2_MouseMove);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(901, 674);
            this.Controls.Add(this.gamePanel2);
            this.Name = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        
        private Checkers.GamePanel gamePanel2;
    }
}


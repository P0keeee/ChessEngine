namespace ChessEngine
{
    partial class BoardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoardForm));
            this.boardDisp = new System.Windows.Forms.PictureBox();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.dispWon = new System.Windows.Forms.TextBox();
            this.restartButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.boardDisp)).BeginInit();
            this.SuspendLayout();
            // 
            // boardDisp
            // 
            this.boardDisp.Location = new System.Drawing.Point(16, 15);
            this.boardDisp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.boardDisp.Name = "boardDisp";
            this.boardDisp.Size = new System.Drawing.Size(1067, 985);
            this.boardDisp.TabIndex = 0;
            this.boardDisp.TabStop = false;
            this.boardDisp.Click += new System.EventHandler(this.BoardClick);
            this.boardDisp.Paint += new System.Windows.Forms.PaintEventHandler(this.BoardPaint);
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Tick += new System.EventHandler(this.BoardRefresh);
            // 
            // dispWon
            // 
            this.dispWon.Location = new System.Drawing.Point(1105, 15);
            this.dispWon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dispWon.Multiline = true;
            this.dispWon.Name = "dispWon";
            this.dispWon.ReadOnly = true;
            this.dispWon.Size = new System.Drawing.Size(213, 94);
            this.dispWon.TabIndex = 1;
            this.dispWon.TextChanged += new System.EventHandler(this.dispWon_TextChanged);
            // 
            // restartButton
            // 
            this.restartButton.Location = new System.Drawing.Point(1105, 129);
            this.restartButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(100, 28);
            this.restartButton.TabIndex = 2;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // BoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1551, 1009);
            this.Controls.Add(this.restartButton);
            this.Controls.Add(this.dispWon);
            this.Controls.Add(this.boardDisp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "BoardForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.BoardForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.boardDisp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox boardDisp;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.TextBox dispWon;
        private System.Windows.Forms.Button restartButton;
    }
}


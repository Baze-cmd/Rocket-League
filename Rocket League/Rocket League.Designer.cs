namespace Rocket_League
{
    partial class RocketLeague
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // RocketLeague
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 853);
            Name = "RocketLeague";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Rocket League";
            Paint += RocketLeague_Paint;
            KeyDown += RocketLeague_KeyDown;
            KeyUp += RocketLeague_KeyUp;
            MouseDown += RocketLeague_MouseDown;
            MouseMove += RocketLeague_MouseMove;
            ResumeLayout(false);
        }

        #endregion
    }
}
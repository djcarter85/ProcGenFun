namespace ProcGenFun.WinForms
{
    partial class MainForm
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
            this.visualiseDistributionsButton = new Button();
            this.mazesButton = new Button();
            this.birthdaysButton = new Button();
            this.diceButton = new Button();
            this.randomWalkButton = new Button();
            this.flagsButton = new Button();
            this.perlinButton = new Button();
            this.SuspendLayout();
            // 
            // visualiseDistributionsButton
            // 
            this.visualiseDistributionsButton.Location = new Point(12, 12);
            this.visualiseDistributionsButton.Name = "visualiseDistributionsButton";
            this.visualiseDistributionsButton.Size = new Size(107, 50);
            this.visualiseDistributionsButton.TabIndex = 0;
            this.visualiseDistributionsButton.Text = "Visualise Distributions";
            this.visualiseDistributionsButton.UseVisualStyleBackColor = true;
            this.visualiseDistributionsButton.Click += this.VisualiseDistributionsButton_Click;
            // 
            // mazesButton
            // 
            this.mazesButton.Location = new Point(125, 12);
            this.mazesButton.Name = "mazesButton";
            this.mazesButton.Size = new Size(107, 50);
            this.mazesButton.TabIndex = 1;
            this.mazesButton.Text = "Mazes";
            this.mazesButton.UseVisualStyleBackColor = true;
            this.mazesButton.Click += this.MazesButton_Click;
            // 
            // birthdaysButton
            // 
            this.birthdaysButton.Location = new Point(237, 12);
            this.birthdaysButton.Name = "birthdaysButton";
            this.birthdaysButton.Size = new Size(107, 50);
            this.birthdaysButton.TabIndex = 2;
            this.birthdaysButton.Text = "Birthdays";
            this.birthdaysButton.UseVisualStyleBackColor = true;
            this.birthdaysButton.Click += this.BirthdaysButton_Click;
            // 
            // diceButton
            // 
            this.diceButton.Location = new Point(350, 12);
            this.diceButton.Name = "diceButton";
            this.diceButton.Size = new Size(107, 50);
            this.diceButton.TabIndex = 3;
            this.diceButton.Text = "Dice";
            this.diceButton.UseVisualStyleBackColor = true;
            this.diceButton.Click += this.DiceButton_Click;
            // 
            // randomWalkButton
            // 
            this.randomWalkButton.Location = new Point(463, 12);
            this.randomWalkButton.Name = "randomWalkButton";
            this.randomWalkButton.Size = new Size(107, 50);
            this.randomWalkButton.TabIndex = 4;
            this.randomWalkButton.Text = "Random Walk";
            this.randomWalkButton.UseVisualStyleBackColor = true;
            this.randomWalkButton.Click += this.RandomWalkButton_Click;
            // 
            // flagsButton
            // 
            this.flagsButton.Location = new Point(575, 12);
            this.flagsButton.Name = "flagsButton";
            this.flagsButton.Size = new Size(107, 50);
            this.flagsButton.TabIndex = 5;
            this.flagsButton.Text = "Flags";
            this.flagsButton.UseVisualStyleBackColor = true;
            this.flagsButton.Click += this.FlagsButton_Click;
            // 
            // mapsButton
            // 
            this.perlinButton.Location = new Point(688, 12);
            this.perlinButton.Name = "perlinButton";
            this.perlinButton.Size = new Size(107, 50);
            this.perlinButton.TabIndex = 6;
            this.perlinButton.Text = "Perlin";
            this.perlinButton.UseVisualStyleBackColor = true;
            this.perlinButton.Click += this.PerlinButton_Click;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(806, 74);
            this.Controls.Add(this.perlinButton);
            this.Controls.Add(this.flagsButton);
            this.Controls.Add(this.randomWalkButton);
            this.Controls.Add(this.diceButton);
            this.Controls.Add(this.birthdaysButton);
            this.Controls.Add(this.mazesButton);
            this.Controls.Add(this.visualiseDistributionsButton);
            this.Name = "MainForm";
            this.Text = "ProcGen Fun";
            this.ResumeLayout(false);
        }

        #endregion

        private Button visualiseDistributionsButton;
        private Button mazesButton;
        private Button birthdaysButton;
        private Button diceButton;
        private Button randomWalkButton;
        private Button flagsButton;
        private Button perlinButton;
    }
}
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
            visualiseDistributionsButton = new Button();
            mazesButton = new Button();
            birthdaysButton = new Button();
            diceButton = new Button();
            SuspendLayout();
            // 
            // visualiseDistributionsButton
            // 
            visualiseDistributionsButton.Location = new Point(12, 12);
            visualiseDistributionsButton.Name = "visualiseDistributionsButton";
            visualiseDistributionsButton.Size = new Size(107, 50);
            visualiseDistributionsButton.TabIndex = 0;
            visualiseDistributionsButton.Text = "Visualise Distributions";
            visualiseDistributionsButton.UseVisualStyleBackColor = true;
            visualiseDistributionsButton.Click += VisualiseDistributionsButton_Click;
            // 
            // mazesButton
            // 
            mazesButton.Location = new Point(125, 12);
            mazesButton.Name = "mazesButton";
            mazesButton.Size = new Size(107, 50);
            mazesButton.TabIndex = 1;
            mazesButton.Text = "Mazes";
            mazesButton.UseVisualStyleBackColor = true;
            mazesButton.Click += MazesButton_Click;
            // 
            // birthdaysButton
            // 
            birthdaysButton.Location = new Point(237, 12);
            birthdaysButton.Name = "birthdaysButton";
            birthdaysButton.Size = new Size(107, 50);
            birthdaysButton.TabIndex = 2;
            birthdaysButton.Text = "Birthdays";
            birthdaysButton.UseVisualStyleBackColor = true;
            birthdaysButton.Click += BirthdaysButton_Click;
            // 
            // diceButton
            // 
            diceButton.Location = new Point(350, 12);
            diceButton.Name = "diceButton";
            diceButton.Size = new Size(107, 50);
            diceButton.TabIndex = 3;
            diceButton.Text = "Dice";
            diceButton.UseVisualStyleBackColor = true;
            diceButton.Click += DiceButton_Click;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(466, 74);
            this.Controls.Add(diceButton);
            this.Controls.Add(birthdaysButton);
            this.Controls.Add(mazesButton);
            this.Controls.Add(visualiseDistributionsButton);
            this.Name = "MainForm";
            this.Text = "ProcGen Fun";
            ResumeLayout(false);
        }

        #endregion

        private Button visualiseDistributionsButton;
        private Button mazesButton;
        private Button birthdaysButton;
        private Button diceButton;
    }
}
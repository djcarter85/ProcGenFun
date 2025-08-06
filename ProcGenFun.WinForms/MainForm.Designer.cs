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
            SuspendLayout();
            // 
            // visualiseDistributionsButton
            // 
            this.visualiseDistributionsButton.Location = new Point(14, 16);
            this.visualiseDistributionsButton.Margin = new Padding(3, 4, 3, 4);
            this.visualiseDistributionsButton.Name = "visualiseDistributionsButton";
            this.visualiseDistributionsButton.Size = new Size(122, 67);
            this.visualiseDistributionsButton.TabIndex = 0;
            this.visualiseDistributionsButton.Text = "Visualise Distributions";
            this.visualiseDistributionsButton.UseVisualStyleBackColor = true;
            this.visualiseDistributionsButton.Click += VisualiseDistributionsButton_Click;
            // 
            // mazesButton
            // 
            this.mazesButton.Location = new Point(143, 16);
            this.mazesButton.Margin = new Padding(3, 4, 3, 4);
            this.mazesButton.Name = "mazesButton";
            this.mazesButton.Size = new Size(122, 67);
            this.mazesButton.TabIndex = 1;
            this.mazesButton.Text = "Mazes";
            this.mazesButton.UseVisualStyleBackColor = true;
            this.mazesButton.Click += MazesButton_Click;
            // 
            // birthdaysButton
            // 
            this.birthdaysButton.Location = new Point(271, 16);
            this.birthdaysButton.Margin = new Padding(3, 4, 3, 4);
            this.birthdaysButton.Name = "birthdaysButton";
            this.birthdaysButton.Size = new Size(122, 67);
            this.birthdaysButton.TabIndex = 2;
            this.birthdaysButton.Text = "Birthdays";
            this.birthdaysButton.UseVisualStyleBackColor = true;
            this.birthdaysButton.Click += BirthdaysButton_Click;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(408, 99);
            this.Controls.Add(this.birthdaysButton);
            this.Controls.Add(this.mazesButton);
            this.Controls.Add(this.visualiseDistributionsButton);
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "ProcGen Fun";
            ResumeLayout(false);
        }

        #endregion

        private Button visualiseDistributionsButton;
        private Button mazesButton;
        private Button birthdaysButton;
    }
}
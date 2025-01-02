namespace ProcGenFun.WinForms
{
    partial class MazeForm
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
            this.generateButton = new Button();
            this.saveImagesButton = new Button();
            this.pictureBox = new PictureBox();
            this.mazeAlgorithmCombo = new ComboBox();
            this.firstButton = new Button();
            this.previousButton = new Button();
            this.nextButton = new Button();
            this.lastButton = new Button();
            this.label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)this.pictureBox).BeginInit();
            this.SuspendLayout();
            // 
            // generateButton
            // 
            this.generateButton.Location = new Point(238, 16);
            this.generateButton.Margin = new Padding(3, 4, 3, 4);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new Size(170, 40);
            this.generateButton.TabIndex = 0;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += this.GenerateButton_Click;
            // 
            // saveImagesButton
            // 
            this.saveImagesButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.saveImagesButton.Location = new Point(689, 16);
            this.saveImagesButton.Margin = new Padding(3, 4, 3, 4);
            this.saveImagesButton.Name = "saveImagesButton";
            this.saveImagesButton.Size = new Size(170, 40);
            this.saveImagesButton.TabIndex = 1;
            this.saveImagesButton.Text = "Save images";
            this.saveImagesButton.UseVisualStyleBackColor = true;
            this.saveImagesButton.Click += this.SaveImagesButton_Click;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new Point(12, 109);
            this.pictureBox.Margin = new Padding(3, 4, 3, 4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new Size(114, 67);
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            // 
            // mazeAlgorithmCombo
            // 
            this.mazeAlgorithmCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.mazeAlgorithmCombo.FormattingEnabled = true;
            this.mazeAlgorithmCombo.Location = new Point(12, 23);
            this.mazeAlgorithmCombo.Name = "mazeAlgorithmCombo";
            this.mazeAlgorithmCombo.Size = new Size(220, 28);
            this.mazeAlgorithmCombo.TabIndex = 3;
            // 
            // firstButton
            // 
            this.firstButton.Enabled = false;
            this.firstButton.Location = new Point(12, 73);
            this.firstButton.Name = "firstButton";
            this.firstButton.Size = new Size(35, 29);
            this.firstButton.TabIndex = 4;
            this.firstButton.Text = "«";
            this.firstButton.UseVisualStyleBackColor = true;
            this.firstButton.Click += this.firstButton_Click;
            // 
            // previousButton
            // 
            this.previousButton.Enabled = false;
            this.previousButton.Location = new Point(53, 73);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new Size(35, 29);
            this.previousButton.TabIndex = 5;
            this.previousButton.Text = "<";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += this.previousButton_Click;
            // 
            // nextButton
            // 
            this.nextButton.Enabled = false;
            this.nextButton.Location = new Point(94, 73);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new Size(35, 29);
            this.nextButton.TabIndex = 6;
            this.nextButton.Text = ">";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += this.nextButton_Click;
            // 
            // lastButton
            // 
            this.lastButton.Enabled = false;
            this.lastButton.Location = new Point(135, 73);
            this.lastButton.Name = "lastButton";
            this.lastButton.Size = new Size(35, 29);
            this.lastButton.TabIndex = 7;
            this.lastButton.Text = "»";
            this.lastButton.UseVisualStyleBackColor = true;
            this.lastButton.Click += this.lastButton_Click;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(182, 77);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0, 20);
            this.label1.TabIndex = 8;
            // 
            // MazeForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(873, 717);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lastButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.previousButton);
            this.Controls.Add(this.firstButton);
            this.Controls.Add(this.mazeAlgorithmCombo);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.saveImagesButton);
            this.Controls.Add(this.generateButton);
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "MazeForm";
            this.Text = "ProcGen Fun: Mazes";
            ((System.ComponentModel.ISupportInitialize)this.pictureBox).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button generateButton;
        private Button saveImagesButton;
        private PictureBox pictureBox;
        private ComboBox mazeAlgorithmCombo;
        private Button firstButton;
        private Button previousButton;
        private Button nextButton;
        private Button lastButton;
        private Label label1;
    }
}
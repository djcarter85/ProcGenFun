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
            this.pictureBox.Location = new Point(14, 64);
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
            // MazeForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(873, 717);
            this.Controls.Add(this.mazeAlgorithmCombo);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.saveImagesButton);
            this.Controls.Add(this.generateButton);
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "MazeForm";
            this.Text = "ProcGen Fun: Mazes";
            ((System.ComponentModel.ISupportInitialize)this.pictureBox).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Button generateButton;
        private Button saveImagesButton;
        private PictureBox pictureBox;
        private ComboBox mazeAlgorithmCombo;
    }
}
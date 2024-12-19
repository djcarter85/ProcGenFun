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
            ((System.ComponentModel.ISupportInitialize)this.pictureBox).BeginInit();
            this.SuspendLayout();
            // 
            // generateButton
            // 
            this.generateButton.Location = new Point(12, 12);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new Size(149, 30);
            this.generateButton.TabIndex = 0;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += this.GenerateButton_Click;
            // 
            // saveImagesButton
            // 
            this.saveImagesButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.saveImagesButton.Location = new Point(603, 12);
            this.saveImagesButton.Name = "saveImagesButton";
            this.saveImagesButton.Size = new Size(149, 30);
            this.saveImagesButton.TabIndex = 1;
            this.saveImagesButton.Text = "Save images";
            this.saveImagesButton.UseVisualStyleBackColor = true;
            this.saveImagesButton.Click += this.SaveImagesButton_Click;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new Point(12, 48);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new Size(100, 50);
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            // 
            // MazeForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(764, 538);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.saveImagesButton);
            this.Controls.Add(this.generateButton);
            this.Name = "MazeForm";
            this.Text = "ProcGen Fun: Mazes";
            ((System.ComponentModel.ISupportInitialize)this.pictureBox).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Button generateButton;
        private Button saveImagesButton;
        private PictureBox pictureBox;
    }
}
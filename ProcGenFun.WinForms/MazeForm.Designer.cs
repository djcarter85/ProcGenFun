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
            generateButton = new Button();
            saveImagesButton = new Button();
            pictureBox = new PictureBox();
            algorithmCombo = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // generateButton
            // 
            generateButton.Location = new Point(207, 12);
            generateButton.Name = "generateButton";
            generateButton.Size = new Size(149, 30);
            generateButton.TabIndex = 0;
            generateButton.Text = "Generate";
            generateButton.UseVisualStyleBackColor = true;
            generateButton.Click += GenerateButton_Click;
            // 
            // saveImagesButton
            // 
            saveImagesButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            saveImagesButton.Location = new Point(603, 12);
            saveImagesButton.Name = "saveImagesButton";
            saveImagesButton.Size = new Size(149, 30);
            saveImagesButton.TabIndex = 1;
            saveImagesButton.Text = "Save images";
            saveImagesButton.UseVisualStyleBackColor = true;
            saveImagesButton.Click += SaveImagesButton_Click;
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(12, 48);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(100, 50);
            pictureBox.TabIndex = 2;
            pictureBox.TabStop = false;
            // 
            // algorithmCombo
            // 
            algorithmCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            algorithmCombo.FormattingEnabled = true;
            algorithmCombo.Items.AddRange(new object[] { "Binary Tree", "Sidewinder" });
            algorithmCombo.Location = new Point(12, 15);
            algorithmCombo.Name = "algorithmCombo";
            algorithmCombo.Size = new Size(189, 23);
            algorithmCombo.TabIndex = 3;
            // 
            // MazeForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(764, 538);
            this.Controls.Add(algorithmCombo);
            this.Controls.Add(pictureBox);
            this.Controls.Add(saveImagesButton);
            this.Controls.Add(generateButton);
            this.Name = "MazeForm";
            this.Text = "ProcGen Fun: Mazes";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button generateButton;
        private Button saveImagesButton;
        private PictureBox pictureBox;
        private ComboBox algorithmCombo;
    }
}
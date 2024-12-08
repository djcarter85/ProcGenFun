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
            generateAndSaveButton = new Button();
            SuspendLayout();
            // 
            // generateAndSaveButton
            // 
            generateAndSaveButton.Location = new Point(12, 12);
            generateAndSaveButton.Name = "generateAndSaveButton";
            generateAndSaveButton.Size = new Size(198, 58);
            generateAndSaveButton.TabIndex = 0;
            generateAndSaveButton.Text = "Generate and save";
            generateAndSaveButton.UseVisualStyleBackColor = true;
            generateAndSaveButton.Click += GenerateAndSaveButton_Click;
            // 
            // MazeForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(432, 82);
            this.Controls.Add(generateAndSaveButton);
            this.Name = "MazeForm";
            this.Text = "ProcGen Fun: Mazes";
            ResumeLayout(false);
        }

        #endregion

        private Button generateAndSaveButton;
    }
}
namespace ProcGenFun.WinForms
{
    partial class FlagsForm
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
            createFlagButton = new Button();
            pictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // createFlagButton
            // 
            createFlagButton.Location = new Point(12, 12);
            createFlagButton.Name = "createFlagButton";
            createFlagButton.Size = new Size(124, 54);
            createFlagButton.TabIndex = 0;
            createFlagButton.Text = "Create Flag";
            createFlagButton.UseVisualStyleBackColor = true;
            createFlagButton.Click += CreateFlagButton_Click;
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(12, 87);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(100, 50);
            pictureBox.TabIndex = 1;
            pictureBox.TabStop = false;
            // 
            // FlagsForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(pictureBox);
            this.Controls.Add(createFlagButton);
            this.Name = "FlagsForm";
            this.Text = "Flags";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button createFlagButton;
        private PictureBox pictureBox;
    }
}
namespace ProcGenFun.WinForms
{
    partial class MapsForm
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
            this.createMapButton = new Button();
            this.pictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)this.pictureBox).BeginInit();
            this.SuspendLayout();
            // 
            // createMapButton
            // 
            this.createMapButton.Location = new Point(12, 12);
            this.createMapButton.Name = "createMapButton";
            this.createMapButton.Size = new Size(124, 54);
            this.createMapButton.TabIndex = 0;
            this.createMapButton.Text = "Create Map";
            this.createMapButton.UseVisualStyleBackColor = true;
            this.createMapButton.Click += this.CreateMapButton_Click;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new Point(12, 72);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new Size(100, 50);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // MapsForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 645);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.createMapButton);
            this.Name = "MapsForm";
            this.Text = "Maps";
            ((System.ComponentModel.ISupportInitialize)this.pictureBox).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Button createMapButton;
        private PictureBox pictureBox;
    }
}
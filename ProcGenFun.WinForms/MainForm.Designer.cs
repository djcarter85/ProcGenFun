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
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(261, 74);
            this.Controls.Add(visualiseDistributionsButton);
            this.Name = "MainForm";
            this.Text = "Fun ProcGen";
            ResumeLayout(false);
        }

        #endregion

        private Button visualiseDistributionsButton;
    }
}
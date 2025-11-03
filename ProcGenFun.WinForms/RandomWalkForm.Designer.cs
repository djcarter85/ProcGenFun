namespace ProcGenFun.WinForms
{
    partial class RandomWalkForm
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
            outputTextBox = new TextBox();
            SuspendLayout();
            // 
            // outputTextBox
            // 
            outputTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            outputTextBox.Location = new Point(12, 12);
            outputTextBox.Multiline = true;
            outputTextBox.Name = "outputTextBox";
            outputTextBox.Size = new Size(776, 426);
            outputTextBox.TabIndex = 0;
            // 
            // RandomWalkForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(outputTextBox);
            this.Name = "RandomWalkForm";
            this.Text = "RandomWalkForm";
            Load += RandomWalkForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox outputTextBox;
    }
}
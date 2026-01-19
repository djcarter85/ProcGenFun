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
            this.createFlagsButton = new Button();
            this.SuspendLayout();
            // 
            // createFlagsButton
            // 
            this.createFlagsButton.Location = new Point(12, 12);
            this.createFlagsButton.Name = "createFlagsButton";
            this.createFlagsButton.Size = new Size(124, 54);
            this.createFlagsButton.TabIndex = 0;
            this.createFlagsButton.Text = "Create Flags";
            this.createFlagsButton.UseVisualStyleBackColor = true;
            this.createFlagsButton.Click += this.CreateFlagsButton_Click;
            // 
            // FlagsForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 645);
            this.Controls.Add(this.createFlagsButton);
            this.Name = "FlagsForm";
            this.Text = "Flags";
            this.ResumeLayout(false);
        }

        #endregion

        private Button createFlagsButton;
    }
}
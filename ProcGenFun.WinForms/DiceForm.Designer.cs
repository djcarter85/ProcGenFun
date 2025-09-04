namespace ProcGenFun.WinForms
{
    partial class DiceForm
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
            rollButton = new Button();
            rollLabel = new Label();
            SuspendLayout();
            // 
            // rollButton
            // 
            rollButton.Location = new Point(12, 12);
            rollButton.Name = "rollButton";
            rollButton.Size = new Size(75, 23);
            rollButton.TabIndex = 0;
            rollButton.Text = "Roll";
            rollButton.UseVisualStyleBackColor = true;
            rollButton.Click += RollButton_Click;
            // 
            // rollLabel
            // 
            rollLabel.AutoSize = true;
            rollLabel.Location = new Point(12, 50);
            rollLabel.Name = "rollLabel";
            rollLabel.Size = new Size(0, 15);
            rollLabel.TabIndex = 1;
            // 
            // DiceForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(373, 139);
            this.Controls.Add(rollLabel);
            this.Controls.Add(rollButton);
            this.Name = "DiceForm";
            this.Text = "DiceForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button rollButton;
        private Label rollLabel;
    }
}
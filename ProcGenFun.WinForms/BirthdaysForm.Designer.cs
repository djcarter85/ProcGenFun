namespace ProcGenFun.WinForms
{
    partial class BirthdaysForm
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
            birthdaysButton = new Button();
            probabilityLabel = new Label();
            SuspendLayout();
            // 
            // birthdaysButton
            // 
            birthdaysButton.Location = new Point(10, 10);
            birthdaysButton.Name = "birthdaysButton";
            birthdaysButton.Size = new Size(107, 50);
            birthdaysButton.TabIndex = 3;
            birthdaysButton.Text = "Estimate probability";
            birthdaysButton.UseVisualStyleBackColor = true;
            birthdaysButton.Click += BirthdaysButton_Click;
            // 
            // probabilityLabel
            // 
            probabilityLabel.AutoSize = true;
            probabilityLabel.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            probabilityLabel.Location = new Point(10, 63);
            probabilityLabel.Name = "probabilityLabel";
            probabilityLabel.Size = new Size(0, 65);
            probabilityLabel.TabIndex = 4;
            // 
            // BirthdaysForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(246, 140);
            this.Controls.Add(probabilityLabel);
            this.Controls.Add(birthdaysButton);
            this.Margin = new Padding(3, 2, 3, 2);
            this.Name = "BirthdaysForm";
            this.Text = "Birthdays";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button birthdaysButton;
        private Label probabilityLabel;
    }
}
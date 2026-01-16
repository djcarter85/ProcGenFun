namespace ProcGenFun.WinForms
{
    partial class ClockForm
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
            estimateProbabilitiesButton = new Button();
            outputLabel = new Label();
            SuspendLayout();
            // 
            // estimateProbabilitiesButton
            // 
            estimateProbabilitiesButton.Location = new Point(12, 12);
            estimateProbabilitiesButton.Name = "estimateProbabilitiesButton";
            estimateProbabilitiesButton.Size = new Size(124, 54);
            estimateProbabilitiesButton.TabIndex = 0;
            estimateProbabilitiesButton.Text = "Estimate Probabilities";
            estimateProbabilitiesButton.UseVisualStyleBackColor = true;
            estimateProbabilitiesButton.Click += EstimateProbabilitiesButton_Click;
            // 
            // outputLabel
            // 
            outputLabel.AutoSize = true;
            outputLabel.Location = new Point(12, 80);
            outputLabel.Name = "outputLabel";
            outputLabel.Size = new Size(0, 15);
            outputLabel.TabIndex = 1;
            // 
            // ClockForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(outputLabel);
            this.Controls.Add(estimateProbabilitiesButton);
            this.Name = "ClockForm";
            this.Text = "Clock";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button estimateProbabilitiesButton;
        private Label outputLabel;
    }
}
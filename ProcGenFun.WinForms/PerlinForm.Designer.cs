namespace ProcGenFun.WinForms
{
    partial class PerlinForm
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
            reRollButton = new Button();
            formsPlot = new ScottPlot.WinForms.FormsPlot();
            backButton = new Button();
            forwardButton = new Button();
            SuspendLayout();
            // 
            // reRollButton
            // 
            reRollButton.Location = new Point(12, 12);
            reRollButton.Name = "reRollButton";
            reRollButton.Size = new Size(124, 54);
            reRollButton.TabIndex = 0;
            reRollButton.Text = "Re-roll";
            reRollButton.UseVisualStyleBackColor = true;
            reRollButton.Click += ReRollButton_Click;
            // 
            // formsPlot
            // 
            formsPlot.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            formsPlot.DisplayScale = 1F;
            formsPlot.Location = new Point(12, 72);
            formsPlot.Name = "formsPlot";
            formsPlot.Size = new Size(776, 561);
            formsPlot.TabIndex = 1;
            // 
            // backButton
            // 
            backButton.Location = new Point(181, 23);
            backButton.Name = "backButton";
            backButton.Size = new Size(63, 32);
            backButton.TabIndex = 2;
            backButton.Text = "<";
            backButton.UseVisualStyleBackColor = true;
            backButton.Click += BackButton_Click;
            // 
            // forwardButton
            // 
            forwardButton.Location = new Point(250, 23);
            forwardButton.Name = "forwardButton";
            forwardButton.Size = new Size(63, 32);
            forwardButton.TabIndex = 3;
            forwardButton.Text = ">";
            forwardButton.UseVisualStyleBackColor = true;
            forwardButton.Click += ForwardButton_Click;
            // 
            // PerlinForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 645);
            this.Controls.Add(forwardButton);
            this.Controls.Add(backButton);
            this.Controls.Add(formsPlot);
            this.Controls.Add(reRollButton);
            this.Name = "PerlinForm";
            this.Text = "Perlin";
            ResumeLayout(false);
        }

        #endregion

        private Button reRollButton;
        private ScottPlot.WinForms.FormsPlot formsPlot;
        private Button backButton;
        private Button forwardButton;
    }
}
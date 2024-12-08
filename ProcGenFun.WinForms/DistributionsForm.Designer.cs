namespace ProcGenFun.WinForms
{
    partial class DistributionsForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            formsPlot = new ScottPlot.WinForms.FormsPlot();
            label1 = new Label();
            distributionTypeCombo = new ComboBox();
            regenerateButton = new Button();
            SuspendLayout();
            // 
            // formsPlot
            // 
            formsPlot.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            formsPlot.DisplayScale = 1F;
            formsPlot.Location = new Point(12, 35);
            formsPlot.Name = "formsPlot";
            formsPlot.Size = new Size(776, 403);
            formsPlot.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(72, 15);
            label1.TabIndex = 1;
            label1.Text = "Distribution:";
            // 
            // distributionTypeCombo
            // 
            distributionTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            distributionTypeCombo.FormattingEnabled = true;
            distributionTypeCombo.Location = new Point(90, 6);
            distributionTypeCombo.Name = "distributionTypeCombo";
            distributionTypeCombo.Size = new Size(167, 23);
            distributionTypeCombo.TabIndex = 2;
            // 
            // regenerateButton
            // 
            regenerateButton.Location = new Point(263, 6);
            regenerateButton.Name = "regenerateButton";
            regenerateButton.Size = new Size(108, 23);
            regenerateButton.TabIndex = 3;
            regenerateButton.Text = "Regenerate";
            regenerateButton.UseVisualStyleBackColor = true;
            regenerateButton.Click += RegenerateButton_Click;
            // 
            // DistributionsForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(regenerateButton);
            this.Controls.Add(distributionTypeCombo);
            this.Controls.Add(label1);
            this.Controls.Add(formsPlot);
            this.Name = "DistributionsForm";
            this.Text = "Fun ProcGen";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot formsPlot;
        private Label label1;
        private ComboBox distributionTypeCombo;
        private Button regenerateButton;
    }
}

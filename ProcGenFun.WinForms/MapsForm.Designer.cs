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
            createMapButton = new Button();
            formsPlot = new ScottPlot.WinForms.FormsPlot();
            SuspendLayout();
            // 
            // createMapButton
            // 
            createMapButton.Location = new Point(12, 12);
            createMapButton.Name = "createMapButton";
            createMapButton.Size = new Size(124, 54);
            createMapButton.TabIndex = 0;
            createMapButton.Text = "Create Map";
            createMapButton.UseVisualStyleBackColor = true;
            createMapButton.Click += CreateMapButton_Click;
            // 
            // formsPlot1
            // 
            formsPlot.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            formsPlot.DisplayScale = 1F;
            formsPlot.Location = new Point(12, 72);
            formsPlot.Name = "formsPlot1";
            formsPlot.Size = new Size(776, 561);
            formsPlot.TabIndex = 1;
            // 
            // MapsForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 645);
            this.Controls.Add(formsPlot);
            this.Controls.Add(createMapButton);
            this.Name = "MapsForm";
            this.Text = "Maps";
            ResumeLayout(false);
        }

        #endregion

        private Button createMapButton;
        private ScottPlot.WinForms.FormsPlot formsPlot;
    }
}
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
            groupSizeLabel = new Label();
            numberOfSharedBirthdaysLabel = new Label();
            groupSizeUpDown = new NumericUpDown();
            sharedBirthdaysUpDown = new NumericUpDown();
            occurOnCampCheckBox = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)groupSizeUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sharedBirthdaysUpDown).BeginInit();
            SuspendLayout();
            // 
            // birthdaysButton
            // 
            birthdaysButton.Location = new Point(240, 7);
            birthdaysButton.Name = "birthdaysButton";
            birthdaysButton.Size = new Size(101, 87);
            birthdaysButton.TabIndex = 3;
            birthdaysButton.Text = "Estimate probability";
            birthdaysButton.UseVisualStyleBackColor = true;
            birthdaysButton.Click += BirthdaysButton_Click;
            // 
            // probabilityLabel
            // 
            probabilityLabel.AutoSize = true;
            probabilityLabel.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            probabilityLabel.Location = new Point(10, 107);
            probabilityLabel.Name = "probabilityLabel";
            probabilityLabel.Size = new Size(0, 65);
            probabilityLabel.TabIndex = 4;
            // 
            // groupSizeLabel
            // 
            groupSizeLabel.AutoSize = true;
            groupSizeLabel.Location = new Point(12, 12);
            groupSizeLabel.Name = "groupSizeLabel";
            groupSizeLabel.Size = new Size(66, 15);
            groupSizeLabel.TabIndex = 5;
            groupSizeLabel.Text = "Group Size:";
            // 
            // numberOfSharedBirthdaysLabel
            // 
            numberOfSharedBirthdaysLabel.AutoSize = true;
            numberOfSharedBirthdaysLabel.Location = new Point(12, 41);
            numberOfSharedBirthdaysLabel.Name = "numberOfSharedBirthdaysLabel";
            numberOfSharedBirthdaysLabel.Size = new Size(158, 15);
            numberOfSharedBirthdaysLabel.TabIndex = 6;
            numberOfSharedBirthdaysLabel.Text = "Number of shared birthdays:";
            // 
            // groupSizeUpDown
            // 
            groupSizeUpDown.Location = new Point(180, 7);
            groupSizeUpDown.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            groupSizeUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            groupSizeUpDown.Name = "groupSizeUpDown";
            groupSizeUpDown.Size = new Size(47, 23);
            groupSizeUpDown.TabIndex = 7;
            groupSizeUpDown.Value = new decimal(new int[] { 71, 0, 0, 0 });
            // 
            // sharedBirthdaysUpDown
            // 
            sharedBirthdaysUpDown.Location = new Point(180, 39);
            sharedBirthdaysUpDown.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            sharedBirthdaysUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            sharedBirthdaysUpDown.Name = "sharedBirthdaysUpDown";
            sharedBirthdaysUpDown.Size = new Size(47, 23);
            sharedBirthdaysUpDown.TabIndex = 8;
            sharedBirthdaysUpDown.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // occurOnCampCheckBox
            // 
            occurOnCampCheckBox.AutoSize = true;
            occurOnCampCheckBox.Checked = true;
            occurOnCampCheckBox.CheckState = CheckState.Checked;
            occurOnCampCheckBox.Location = new Point(16, 71);
            occurOnCampCheckBox.Name = "occurOnCampCheckBox";
            occurOnCampCheckBox.Size = new Size(222, 19);
            occurOnCampCheckBox.TabIndex = 9;
            occurOnCampCheckBox.Text = "Shared birthday must occur on camp";
            occurOnCampCheckBox.UseVisualStyleBackColor = true;
            // 
            // BirthdaysForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(353, 194);
            this.Controls.Add(occurOnCampCheckBox);
            this.Controls.Add(sharedBirthdaysUpDown);
            this.Controls.Add(groupSizeUpDown);
            this.Controls.Add(numberOfSharedBirthdaysLabel);
            this.Controls.Add(groupSizeLabel);
            this.Controls.Add(probabilityLabel);
            this.Controls.Add(birthdaysButton);
            this.Margin = new Padding(3, 2, 3, 2);
            this.Name = "BirthdaysForm";
            this.Text = "Birthdays";
            ((System.ComponentModel.ISupportInitialize)groupSizeUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)sharedBirthdaysUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button birthdaysButton;
        private Label probabilityLabel;
        private Label groupSizeLabel;
        private Label numberOfSharedBirthdaysLabel;
        private NumericUpDown groupSizeUpDown;
        private NumericUpDown sharedBirthdaysUpDown;
        private CheckBox occurOnCampCheckBox;
    }
}
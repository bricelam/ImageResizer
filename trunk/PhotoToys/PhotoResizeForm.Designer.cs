namespace PhotoToys
{
	partial class PhotoResizeForm
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
			this.infoLabel = new System.Windows.Forms.Label();
			this.smallRadioButton = new System.Windows.Forms.RadioButton();
			this.mediumRadioButton = new System.Windows.Forms.RadioButton();
			this.largeRadioButton = new System.Windows.Forms.RadioButton();
			this.handheldRadioButton = new System.Windows.Forms.RadioButton();
			this.selectLabel = new System.Windows.Forms.Label();
			this.advancedButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.acceptButton = new System.Windows.Forms.Button();
			this.customRadioButton = new System.Windows.Forms.RadioButton();
			this.customLabel1 = new System.Windows.Forms.Label();
			this.customWidthTextBox = new System.Windows.Forms.TextBox();
			this.customLabel2 = new System.Windows.Forms.Label();
			this.customHeightTextBox = new System.Windows.Forms.TextBox();
			this.customLabel3 = new System.Windows.Forms.Label();
			this.smallerOnlyCheckBox = new System.Windows.Forms.CheckBox();
			this.resizeOriginalCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// infoLabel
			// 
			this.infoLabel.AutoSize = true;
			this.infoLabel.Location = new System.Drawing.Point(12, 9);
			this.infoLabel.Name = "infoLabel";
			this.infoLabel.Size = new System.Drawing.Size(388, 26);
			this.infoLabel.TabIndex = 0;
			this.infoLabel.Text = "You can create resized copies of one or more selected pictures and store them in\r" +
				"\nthe current folder.";
			// 
			// smallRadioButton
			// 
			this.smallRadioButton.AutoSize = true;
			this.smallRadioButton.Checked = true;
			this.smallRadioButton.Location = new System.Drawing.Point(24, 64);
			this.smallRadioButton.Name = "smallRadioButton";
			this.smallRadioButton.Size = new System.Drawing.Size(166, 17);
			this.smallRadioButton.TabIndex = 2;
			this.smallRadioButton.TabStop = true;
			this.smallRadioButton.Text = "&Small (fits a 640 x 480 screen)";
			this.smallRadioButton.UseVisualStyleBackColor = true;
			// 
			// mediumRadioButton
			// 
			this.mediumRadioButton.AutoSize = true;
			this.mediumRadioButton.Location = new System.Drawing.Point(24, 87);
			this.mediumRadioButton.Name = "mediumRadioButton";
			this.mediumRadioButton.Size = new System.Drawing.Size(178, 17);
			this.mediumRadioButton.TabIndex = 3;
			this.mediumRadioButton.Text = "&Medium (fits a 800 x 600 screen)";
			this.mediumRadioButton.UseVisualStyleBackColor = true;
			// 
			// largeRadioButton
			// 
			this.largeRadioButton.AutoSize = true;
			this.largeRadioButton.Location = new System.Drawing.Point(24, 110);
			this.largeRadioButton.Name = "largeRadioButton";
			this.largeRadioButton.Size = new System.Drawing.Size(174, 17);
			this.largeRadioButton.TabIndex = 4;
			this.largeRadioButton.Text = "&Large (fits a 1024 x 768 screen)";
			this.largeRadioButton.UseVisualStyleBackColor = true;
			// 
			// handheldRadioButton
			// 
			this.handheldRadioButton.AutoSize = true;
			this.handheldRadioButton.Location = new System.Drawing.Point(24, 133);
			this.handheldRadioButton.Name = "handheldRadioButton";
			this.handheldRadioButton.Size = new System.Drawing.Size(204, 17);
			this.handheldRadioButton.TabIndex = 5;
			this.handheldRadioButton.Text = "&Handheld PC (fits a 240 x 320 screen)";
			this.handheldRadioButton.UseVisualStyleBackColor = true;
			// 
			// selectLabel
			// 
			this.selectLabel.AutoSize = true;
			this.selectLabel.Location = new System.Drawing.Point(12, 48);
			this.selectLabel.Name = "selectLabel";
			this.selectLabel.Size = new System.Drawing.Size(70, 13);
			this.selectLabel.TabIndex = 1;
			this.selectLabel.Text = "Select a size:";
			// 
			// advancedButton
			// 
			this.advancedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.advancedButton.Location = new System.Drawing.Point(12, 156);
			this.advancedButton.Name = "advancedButton";
			this.advancedButton.Size = new System.Drawing.Size(79, 23);
			this.advancedButton.TabIndex = 6;
			this.advancedButton.Text = "&Advanced >>";
			this.advancedButton.UseVisualStyleBackColor = true;
			this.advancedButton.Click += new System.EventHandler(this.advancedButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(325, 156);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 8;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.acceptButton.Location = new System.Drawing.Point(244, 156);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(75, 23);
			this.acceptButton.TabIndex = 7;
			this.acceptButton.Text = "OK";
			this.acceptButton.UseVisualStyleBackColor = true;
			// 
			// customRadioButton
			// 
			this.customRadioButton.AutoSize = true;
			this.customRadioButton.Location = new System.Drawing.Point(24, 157);
			this.customRadioButton.Name = "customRadioButton";
			this.customRadioButton.Size = new System.Drawing.Size(60, 17);
			this.customRadioButton.TabIndex = 9;
			this.customRadioButton.TabStop = true;
			this.customRadioButton.Text = "&Custom";
			this.customRadioButton.UseVisualStyleBackColor = true;
			this.customRadioButton.Visible = false;
			this.customRadioButton.CheckedChanged += new System.EventHandler(this.customRadioButton_CheckedChanged);
			// 
			// customLabel1
			// 
			this.customLabel1.AutoSize = true;
			this.customLabel1.Enabled = false;
			this.customLabel1.Location = new System.Drawing.Point(80, 159);
			this.customLabel1.Name = "customLabel1";
			this.customLabel1.Size = new System.Drawing.Size(32, 13);
			this.customLabel1.TabIndex = 10;
			this.customLabel1.Text = "(fits a";
			this.customLabel1.Visible = false;
			// 
			// customWidthTextBox
			// 
			this.customWidthTextBox.Enabled = false;
			this.customWidthTextBox.Location = new System.Drawing.Point(115, 156);
			this.customWidthTextBox.Name = "customWidthTextBox";
			this.customWidthTextBox.Size = new System.Drawing.Size(50, 20);
			this.customWidthTextBox.TabIndex = 11;
			this.customWidthTextBox.Text = "1200";
			this.customWidthTextBox.Visible = false;
			// 
			// customLabel2
			// 
			this.customLabel2.AutoSize = true;
			this.customLabel2.Enabled = false;
			this.customLabel2.Location = new System.Drawing.Point(168, 159);
			this.customLabel2.Name = "customLabel2";
			this.customLabel2.Size = new System.Drawing.Size(18, 13);
			this.customLabel2.TabIndex = 12;
			this.customLabel2.Text = "by";
			this.customLabel2.Visible = false;
			// 
			// customHeightTextBox
			// 
			this.customHeightTextBox.Enabled = false;
			this.customHeightTextBox.Location = new System.Drawing.Point(189, 156);
			this.customHeightTextBox.Name = "customHeightTextBox";
			this.customHeightTextBox.Size = new System.Drawing.Size(50, 20);
			this.customHeightTextBox.TabIndex = 13;
			this.customHeightTextBox.Text = "1024";
			this.customHeightTextBox.Visible = false;
			// 
			// customLabel3
			// 
			this.customLabel3.AutoSize = true;
			this.customLabel3.Enabled = false;
			this.customLabel3.Location = new System.Drawing.Point(242, 159);
			this.customLabel3.Name = "customLabel3";
			this.customLabel3.Size = new System.Drawing.Size(42, 13);
			this.customLabel3.TabIndex = 14;
			this.customLabel3.Text = "screen)";
			this.customLabel3.Visible = false;
			// 
			// smallerOnlyCheckBox
			// 
			this.smallerOnlyCheckBox.AutoSize = true;
			this.smallerOnlyCheckBox.Location = new System.Drawing.Point(12, 182);
			this.smallerOnlyCheckBox.Name = "smallerOnlyCheckBox";
			this.smallerOnlyCheckBox.Size = new System.Drawing.Size(193, 17);
			this.smallerOnlyCheckBox.TabIndex = 15;
			this.smallerOnlyCheckBox.Text = "Ma&ke pictures smaller but not larger";
			this.smallerOnlyCheckBox.UseVisualStyleBackColor = true;
			this.smallerOnlyCheckBox.Visible = false;
			// 
			// resizeOriginalCheckBox
			// 
			this.resizeOriginalCheckBox.AutoSize = true;
			this.resizeOriginalCheckBox.Location = new System.Drawing.Point(12, 205);
			this.resizeOriginalCheckBox.Name = "resizeOriginalCheckBox";
			this.resizeOriginalCheckBox.Size = new System.Drawing.Size(251, 17);
			this.resizeOriginalCheckBox.TabIndex = 16;
			this.resizeOriginalCheckBox.Text = "&Resize the original pictures (don\'t create copies)";
			this.resizeOriginalCheckBox.UseVisualStyleBackColor = true;
			this.resizeOriginalCheckBox.Visible = false;
			// 
			// PhotoResizeForm
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(412, 191);
			this.Controls.Add(this.resizeOriginalCheckBox);
			this.Controls.Add(this.smallerOnlyCheckBox);
			this.Controls.Add(this.customLabel3);
			this.Controls.Add(this.customHeightTextBox);
			this.Controls.Add(this.customLabel2);
			this.Controls.Add(this.customWidthTextBox);
			this.Controls.Add(this.customLabel1);
			this.Controls.Add(this.customRadioButton);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.advancedButton);
			this.Controls.Add(this.selectLabel);
			this.Controls.Add(this.handheldRadioButton);
			this.Controls.Add(this.largeRadioButton);
			this.Controls.Add(this.mediumRadioButton);
			this.Controls.Add(this.smallRadioButton);
			this.Controls.Add(this.infoLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PhotoResizeForm";
			this.Text = "Resize Pictures";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PhotoResizeForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.RadioButton smallRadioButton;
		private System.Windows.Forms.RadioButton mediumRadioButton;
		private System.Windows.Forms.RadioButton largeRadioButton;
		private System.Windows.Forms.RadioButton handheldRadioButton;
		private System.Windows.Forms.Label selectLabel;
		private System.Windows.Forms.Button advancedButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.RadioButton customRadioButton;
		private System.Windows.Forms.Label customLabel1;
		private System.Windows.Forms.TextBox customWidthTextBox;
		private System.Windows.Forms.Label customLabel2;
		private System.Windows.Forms.TextBox customHeightTextBox;
		private System.Windows.Forms.Label customLabel3;
		private System.Windows.Forms.CheckBox smallerOnlyCheckBox;
		private System.Windows.Forms.CheckBox resizeOriginalCheckBox;
	}
}
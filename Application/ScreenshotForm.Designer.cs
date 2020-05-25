//INSTANT C# NOTE: Formerly VB project-level imports:
using System;
using System.Collections;
using System.Diagnostics;

using Inventor;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;

namespace InventorScreenshot
{
	public partial class ScreenshotForm : System.Windows.Forms.Form
	{
	  //Form overrides dispose to clean up the component list.
	  [System.Diagnostics.DebuggerNonUserCode()]
	  protected override void Dispose(bool disposing)
	  {
		try
		{
		  if (disposing && components != null)
		  {
			components.Dispose();
		  }
		}
		finally
		{
		  base.Dispose(disposing);
		}
	  }

	  //Required by the Windows Form Designer
	  private System.ComponentModel.IContainer components;

	  //NOTE: The following procedure is required by the Windows Form Designer
	  //It can be modified using the Windows Form Designer.  
	  //Do not modify it using the code editor.
	  [System.Diagnostics.DebuggerStepThrough()]
	  private void InitializeComponent()
	  {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenshotForm));
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.GroupBoxSettings = new System.Windows.Forms.GroupBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.ComboBoxGray = new System.Windows.Forms.ComboBox();
            this.ComboBoxFG = new System.Windows.Forms.ComboBox();
            this.ComboBoxBG = new System.Windows.Forms.ComboBox();
            this.ButtonWindow = new System.Windows.Forms.Button();
            this.RadioWindow = new System.Windows.Forms.RadioButton();
            this.RadioApplication = new System.Windows.Forms.RadioButton();
            this.RadioDocument = new System.Windows.Forms.RadioButton();
            this.GroupBoxSelectOp = new System.Windows.Forms.GroupBox();
            this.GroupBoxOutput = new System.Windows.Forms.GroupBox();
            this.CheckBoxPrinter = new System.Windows.Forms.CheckBox();
            this.CheckBoxFile = new System.Windows.Forms.CheckBox();
            this.CheckBoxClipboard = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.GroupBoxSettings.SuspendLayout();
            this.GroupBoxSelectOp.SuspendLayout();
            this.GroupBoxOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // PictureBox1
            // 
            this.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PictureBox1.Location = new System.Drawing.Point(293, 20);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(300, 250);
            this.PictureBox1.TabIndex = 10;
            this.PictureBox1.TabStop = false;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cancel.AutoSize = true;
            this.Cancel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.Location = new System.Drawing.Point(503, 308);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(90, 46);
            this.Cancel.TabIndex = 9;
            this.Cancel.Text = "Exit";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // OK
            // 
            this.OK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OK.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK.Location = new System.Drawing.Point(293, 308);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(98, 46);
            this.OK.TabIndex = 8;
            this.OK.Text = "Save Screenshot";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // GroupBoxSettings
            // 
            this.GroupBoxSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxSettings.Controls.Add(this.Label3);
            this.GroupBoxSettings.Controls.Add(this.Label2);
            this.GroupBoxSettings.Controls.Add(this.Label1);
            this.GroupBoxSettings.Controls.Add(this.ComboBoxGray);
            this.GroupBoxSettings.Controls.Add(this.ComboBoxFG);
            this.GroupBoxSettings.Controls.Add(this.ComboBoxBG);
            this.GroupBoxSettings.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBoxSettings.Location = new System.Drawing.Point(16, 147);
            this.GroupBoxSettings.Name = "GroupBoxSettings";
            this.GroupBoxSettings.Size = new System.Drawing.Size(253, 138);
            this.GroupBoxSettings.TabIndex = 7;
            this.GroupBoxSettings.TabStop = false;
            this.GroupBoxSettings.Text = "Settings";
            this.GroupBoxSettings.SizeChanged += new System.EventHandler(this.GroupBoxSettings_SizeChanged);
            // 
            // Label3
            // 
            this.Label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(24, 107);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(65, 16);
            this.Label3.TabIndex = 12;
            this.Label3.Text = "GrayScale";
            // 
            // Label2
            // 
            this.Label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(24, 72);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(74, 16);
            this.Label2.TabIndex = 12;
            this.Label2.Text = "Foreground";
            // 
            // Label1
            // 
            this.Label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(24, 33);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(74, 16);
            this.Label1.TabIndex = 12;
            this.Label1.Text = "Background";
            // 
            // ComboBoxGray
            // 
            this.ComboBoxGray.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ComboBoxGray.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxGray.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxGray.ForeColor = System.Drawing.Color.Blue;
            this.ComboBoxGray.FormatString = "WithGray;WithoutGray";
            this.ComboBoxGray.FormattingEnabled = true;
            this.ComboBoxGray.Items.AddRange(new object[] {
            "Off",
            "On"});
            this.ComboBoxGray.Location = new System.Drawing.Point(122, 104);
            this.ComboBoxGray.Name = "ComboBoxGray";
            this.ComboBoxGray.Size = new System.Drawing.Size(121, 24);
            this.ComboBoxGray.TabIndex = 5;
            this.ComboBoxGray.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxGray_SelectionChangeCommitted);
            // 
            // ComboBoxFG
            // 
            this.ComboBoxFG.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ComboBoxFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxFG.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxFG.ForeColor = System.Drawing.Color.Blue;
            this.ComboBoxFG.FormatString = "White;Black";
            this.ComboBoxFG.FormattingEnabled = true;
            this.ComboBoxFG.Items.AddRange(new object[] {
            "Normal",
            "Black"});
            this.ComboBoxFG.Location = new System.Drawing.Point(122, 70);
            this.ComboBoxFG.Name = "ComboBoxFG";
            this.ComboBoxFG.Size = new System.Drawing.Size(121, 24);
            this.ComboBoxFG.TabIndex = 4;
            this.ComboBoxFG.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxFG_SelectionChangeCommitted);
            // 
            // ComboBoxBG
            // 
            this.ComboBoxBG.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ComboBoxBG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxBG.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxBG.ForeColor = System.Drawing.Color.Blue;
            this.ComboBoxBG.FormattingEnabled = true;
            this.ComboBoxBG.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ComboBoxBG.Items.AddRange(new object[] {
            "Normal",
            "White"});
            this.ComboBoxBG.Location = new System.Drawing.Point(122, 31);
            this.ComboBoxBG.Name = "ComboBoxBG";
            this.ComboBoxBG.Size = new System.Drawing.Size(121, 24);
            this.ComboBoxBG.TabIndex = 3;
            this.ComboBoxBG.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxBG_SelectionChangeCommitted);
            // 
            // ButtonWindow
            // 
            this.ButtonWindow.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ButtonWindow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonWindow.Location = new System.Drawing.Point(138, 93);
            this.ButtonWindow.Name = "ButtonWindow";
            this.ButtonWindow.Size = new System.Drawing.Size(94, 26);
            this.ButtonWindow.TabIndex = 5;
            this.ButtonWindow.Text = "Window<";
            this.ButtonWindow.UseVisualStyleBackColor = true;
            this.ButtonWindow.Click += new System.EventHandler(this.ButtonWindow_Click);
            // 
            // RadioWindow
            // 
            this.RadioWindow.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RadioWindow.AutoSize = true;
            this.RadioWindow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadioWindow.Location = new System.Drawing.Point(29, 95);
            this.RadioWindow.Name = "RadioWindow";
            this.RadioWindow.Size = new System.Drawing.Size(72, 20);
            this.RadioWindow.TabIndex = 3;
            this.RadioWindow.Text = "Window";
            this.RadioWindow.UseVisualStyleBackColor = true;
            this.RadioWindow.Click += new System.EventHandler(this.RadioWindow_Click);
            // 
            // RadioApplication
            // 
            this.RadioApplication.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RadioApplication.AutoSize = true;
            this.RadioApplication.Checked = true;
            this.RadioApplication.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadioApplication.Location = new System.Drawing.Point(29, 25);
            this.RadioApplication.Name = "RadioApplication";
            this.RadioApplication.Size = new System.Drawing.Size(88, 20);
            this.RadioApplication.TabIndex = 1;
            this.RadioApplication.TabStop = true;
            this.RadioApplication.Text = "Application";
            this.RadioApplication.UseVisualStyleBackColor = true;
            this.RadioApplication.Click += new System.EventHandler(this.RadioApplication_Click);
            // 
            // RadioDocument
            // 
            this.RadioDocument.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RadioDocument.AutoSize = true;
            this.RadioDocument.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadioDocument.Location = new System.Drawing.Point(29, 60);
            this.RadioDocument.Name = "RadioDocument";
            this.RadioDocument.Size = new System.Drawing.Size(83, 20);
            this.RadioDocument.TabIndex = 0;
            this.RadioDocument.Text = "Document";
            this.RadioDocument.UseVisualStyleBackColor = true;
            this.RadioDocument.Click += new System.EventHandler(this.RadioDocument_Click);
            // 
            // GroupBoxSelectOp
            // 
            this.GroupBoxSelectOp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxSelectOp.Controls.Add(this.RadioWindow);
            this.GroupBoxSelectOp.Controls.Add(this.RadioApplication);
            this.GroupBoxSelectOp.Controls.Add(this.RadioDocument);
            this.GroupBoxSelectOp.Controls.Add(this.ButtonWindow);
            this.GroupBoxSelectOp.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBoxSelectOp.Location = new System.Drawing.Point(16, 4);
            this.GroupBoxSelectOp.Name = "GroupBoxSelectOp";
            this.GroupBoxSelectOp.Size = new System.Drawing.Size(253, 137);
            this.GroupBoxSelectOp.TabIndex = 12;
            this.GroupBoxSelectOp.TabStop = false;
            this.GroupBoxSelectOp.Text = "Select Options";
            this.GroupBoxSelectOp.SizeChanged += new System.EventHandler(this.GroupBoxSelectOp_SizeChanged);
            // 
            // GroupBoxOutput
            // 
            this.GroupBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxOutput.Controls.Add(this.CheckBoxPrinter);
            this.GroupBoxOutput.Controls.Add(this.CheckBoxFile);
            this.GroupBoxOutput.Controls.Add(this.CheckBoxClipboard);
            this.GroupBoxOutput.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBoxOutput.Location = new System.Drawing.Point(16, 300);
            this.GroupBoxOutput.Name = "GroupBoxOutput";
            this.GroupBoxOutput.Size = new System.Drawing.Size(253, 54);
            this.GroupBoxOutput.TabIndex = 14;
            this.GroupBoxOutput.TabStop = false;
            this.GroupBoxOutput.Text = "Output Location";
            // 
            // CheckBoxPrinter
            // 
            this.CheckBoxPrinter.AutoSize = true;
            this.CheckBoxPrinter.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBoxPrinter.Location = new System.Drawing.Point(174, 24);
            this.CheckBoxPrinter.Name = "CheckBoxPrinter";
            this.CheckBoxPrinter.Size = new System.Drawing.Size(65, 20);
            this.CheckBoxPrinter.TabIndex = 2;
            this.CheckBoxPrinter.Text = "Printer";
            this.CheckBoxPrinter.UseVisualStyleBackColor = true;
            // 
            // CheckBoxFile
            // 
            this.CheckBoxFile.AutoSize = true;
            this.CheckBoxFile.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBoxFile.Location = new System.Drawing.Point(103, 24);
            this.CheckBoxFile.Name = "CheckBoxFile";
            this.CheckBoxFile.Size = new System.Drawing.Size(47, 20);
            this.CheckBoxFile.TabIndex = 1;
            this.CheckBoxFile.Text = "File";
            this.CheckBoxFile.UseVisualStyleBackColor = true;
            // 
            // CheckBoxClipboard
            // 
            this.CheckBoxClipboard.AutoSize = true;
            this.CheckBoxClipboard.Checked = true;
            this.CheckBoxClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxClipboard.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBoxClipboard.Location = new System.Drawing.Point(14, 24);
            this.CheckBoxClipboard.Name = "CheckBoxClipboard";
            this.CheckBoxClipboard.Size = new System.Drawing.Size(81, 20);
            this.CheckBoxClipboard.TabIndex = 0;
            this.CheckBoxClipboard.Text = "Clipboard";
            this.CheckBoxClipboard.UseVisualStyleBackColor = true;
            // 
            // ScreenshotForm
            // 
            this.AcceptButton = this.Cancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 366);
            this.Controls.Add(this.GroupBoxOutput);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.GroupBoxSelectOp);
            this.Controls.Add(this.GroupBoxSettings);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScreenshotForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "screenGrab";
            this.Load += new System.EventHandler(this.ScreenshotForm_Load);
            this.SizeChanged += new System.EventHandler(this.ScreenshotForm_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScreenshotForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.GroupBoxSettings.ResumeLayout(false);
            this.GroupBoxSettings.PerformLayout();
            this.GroupBoxSelectOp.ResumeLayout(false);
            this.GroupBoxSelectOp.PerformLayout();
            this.GroupBoxOutput.ResumeLayout(false);
            this.GroupBoxOutput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

	  }
	  internal System.Windows.Forms.PictureBox PictureBox1;
	  internal System.Windows.Forms.Button Cancel;
	  internal System.Windows.Forms.Button OK;
	  internal System.Windows.Forms.GroupBox GroupBoxSettings;
	  internal System.Windows.Forms.ComboBox ComboBoxGray;
	  internal System.Windows.Forms.ComboBox ComboBoxFG;
	  internal System.Windows.Forms.ComboBox ComboBoxBG;
	  internal System.Windows.Forms.Button ButtonWindow;
	  internal System.Windows.Forms.RadioButton RadioWindow;
	  internal System.Windows.Forms.RadioButton RadioApplication;
	  internal System.Windows.Forms.RadioButton RadioDocument;
	  internal System.Windows.Forms.Label Label3;
	  internal System.Windows.Forms.Label Label2;
	  internal System.Windows.Forms.Label Label1;
	  internal System.Windows.Forms.GroupBox GroupBoxSelectOp;
	  internal System.Windows.Forms.GroupBox GroupBoxOutput;
	  internal System.Windows.Forms.CheckBox CheckBoxClipboard;
	  internal System.Windows.Forms.CheckBox CheckBoxPrinter;
	  internal System.Windows.Forms.CheckBox CheckBoxFile;
	}

}
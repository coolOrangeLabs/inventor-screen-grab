//INSTANT C# NOTE: Formerly VB project-level imports:
using System;
using System.Collections;
using System.Diagnostics;

namespace InventorScreenshot
{
	public partial class ProgressForm : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Location = new System.Drawing.Point(1, 3);
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(432, 42);
            this.ProgressBar1.TabIndex = 0;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 45);
            this.Controls.Add(this.ProgressBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Convert Progress";
            this.Load += new System.EventHandler(this.ProgressForm_Load);
            this.ResumeLayout(false);

	  }
	  internal System.Windows.Forms.ProgressBar ProgressBar1;
	}

}
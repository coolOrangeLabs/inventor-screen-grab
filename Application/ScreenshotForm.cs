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
using log4net.Core;
using Application = Inventor.Application;

namespace InventorScreenshot
{
	public partial class ScreenshotForm
	{

		internal ScreenshotForm()
		{
			InitializeComponent();
		}
#region variables //variables for this class

	  //enum for select options
	  public enum SelectOptionsEnum: int
	  {
		eApplication = 1,
		eDocument = 2,
		eWindow = 3,
		eObject = 4 //reserved for future use
	  }

	  //class for select parameters: size and corner point
	  public class clsSelectParam
	  {

		public System.Drawing.Point oCornerPt = new System.Drawing.Point(0, 0);
		public Size oSize = new Size(0, 0);

		public clsSelectParam()
		{
		}

		public clsSelectParam(System.Drawing.Point oP, Size oS)
		{
		  oCornerPt = oP;
		  oSize = oS;
		}

		public clsSelectParam(int X, int Y, int width, int height)
		{

		  oCornerPt.X = X;
		  oCornerPt.Y = Y;
		  oSize.Width = width;
		  oSize.Height = height;
		}

	  }

	  //class for select setting: background, forground, gray
	  public class clsSelectSetting
	  {

		public int oBG = 0;
		public int oFG = 0;
		public int oGray = 0;

		//compare two settings are same or not
		public static bool compare(clsSelectSetting oP1, clsSelectSetting oP2)
		{
		  bool tempcompare = false;

		  tempcompare = true;
		  if (oP1.oBG != oP2.oBG | oP1.oFG != oP2.oFG | oP1.oGray != oP2.oGray)
		  {
			tempcompare = false;
		  }

		  return tempcompare;
		}

		//set one setting to another one
		public void setEqualTo(clsSelectSetting oPNew)
		{

		  oBG = oPNew.oBG;
		  oFG = oPNew.oFG;
		  oGray = oPNew.oGray;

		}

	  }

	  // Inventor application object
	  private Inventor.Application m_inventorApplication;

	    //global variable for  corner point
	  private System.Drawing.Point oCornerPt = new System.Drawing.Point(0, 0);
	  //global variable for size 
	  private Size oSize = new Size(0, 0);

	  //select parameter for each select mode
	  private clsSelectParam oSelectParam_App;
	  private clsSelectParam oSelectParam_Doc;
	  private clsSelectParam oSelectParam_Win;

	  //select settings for each select mode
	  private clsSelectSetting oSelectSetting_App = new clsSelectSetting();
	  private clsSelectSetting oSelectSetting_Doc = new clsSelectSetting();
	  private clsSelectSetting oSelectSetting_Win = new clsSelectSetting();
	  private clsSelectSetting oSelectSetting_Current = new clsSelectSetting();

	  //final bitmap
	  private Bitmap oResultBitmap = null;

	  //bitmap for each select mode
	  private Bitmap oBitmap_App = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
	  private Bitmap oBitmap_Doc = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
	  private Bitmap oBitmap_Win = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

	  //select options
	  // 1: Application; 2: Document; 3: Window; 4: Object
	  public SelectOptionsEnum oSelectOption;

	  // flag if user has ever selected window
	  private bool oHasSelectWindow = false;

	  //interaction is running
	  public bool bTakeSnapShot = false;

	  //user is going to select  window
	  private bool selectingWin = false;

#endregion //variables

#region Form

	  public ScreenshotForm(Application oApp)
	  {

		InitializeComponent();

		if (oApp == null)
		{
		  MessageBox.Show("Error: Inventor Application is null");
		  return;
		}

		m_inventorApplication = oApp;

	      //initialize the option as eApplication
		oSelectOption = SelectOptionsEnum.eApplication;

		//disable window button
		ButtonWindow.Enabled = false;

		//Param for selecting application
		oSelectParam_App = new clsSelectParam(m_inventorApplication.Left, m_inventorApplication.Top, m_inventorApplication.Width, m_inventorApplication.Height);

		//params for selecting document
		if (m_inventorApplication.ActiveView != null)
		{
		  oSelectParam_Doc = new clsSelectParam(m_inventorApplication.ActiveView.Left, m_inventorApplication.ActiveView.Top, m_inventorApplication.ActiveView.Width, m_inventorApplication.ActiveView.Height);
		}

		//params for selecting window, wait the user to input
		oSelectParam_Win = new clsSelectParam();

		ComboBoxBG.SelectedIndex = 0;
		ComboBoxFG.SelectedIndex = 0;
		ComboBoxGray.SelectedIndex = 0;

		//picurebox's mode is StretchImage
		PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

		//original size values of the dialog
		//the third and forth are for newer values
		this.Tag = this.Width.ToString() + "," + this.Height.ToString() + "," + "0" + "," + "0";

		try
		{
		  foreach (Control oCtrl in this.Controls)
		  {

			//size values of the sub-control
			oCtrl.Tag = oCtrl.Left.ToString() + "," + oCtrl.Top.ToString() + "," + oCtrl.Width.ToString() + "," + oCtrl.Height.ToString();

			if (oCtrl.Name == "GroupBoxSelectOp" || oCtrl.Name == "GroupBoxSettings" || oCtrl.Name == "GroupBoxOutput")
			{

			  foreach (Control oSubCtrl in oCtrl.Controls)
			  {
				oSubCtrl.Tag = oSubCtrl.Left.ToString() + "," + oSubCtrl.Top.ToString() + "," + oSubCtrl.Width.ToString() + "," + oSubCtrl.Height.ToString();
			  }
			}
		  }

		}
		catch (Exception ex)
		{
		  MessageBox.Show(ex.ToString());
		}

	  }

	  private void ScreenshotForm_Load(object sender, System.EventArgs e)
	  {

		if (bTakeSnapShot)
		{
		  return;
		}

		bTakeSnapShot = false;

		try
		{
		  foreach (Control oCtrl in this.Controls)
		  {

			//setting font by the inventor font
			FontStyle oFontStyle = FontStyle.Regular;
			if (oCtrl.Font.Bold)
			{
			  oFontStyle = FontStyle.Bold;
			}
			oCtrl.Font = new Font(m_inventorApplication.GeneralOptions.TextAppearance, m_inventorApplication.GeneralOptions.TextSize, oFontStyle, GraphicsUnit.Point);

			//size values of the sub-control
			if (oCtrl.Name == "GroupBoxSelectOp" || oCtrl.Name == "GroupBoxSettings" || oCtrl.Name == "GroupBoxOutput")
			{

			  foreach (Control oSubCtrl in oCtrl.Controls)
			  {

				//change font of sub controls by the inventor font
				oFontStyle = FontStyle.Regular;
				oSubCtrl.Font = new Font(m_inventorApplication.GeneralOptions.TextAppearance, m_inventorApplication.GeneralOptions.TextSize, oFontStyle, GraphicsUnit.Point);
			  }
			}
		  }

		}
		catch (Exception ex)
		{
		  MessageBox.Show(ex.ToString());
		}
	  }

	  //ready to snapshot after dialog close
	  private void closeDialogForSnapshot()
	  {
		this.Close();
		bTakeSnapShot = true;
	  }

	  //when  the mode is Application
	  private void RadioApplication_Click(object sender, System.EventArgs e)
	  {

		if (oSelectOption != SelectOptionsEnum.eApplication)
		{


		  // record for next use
		  if (oSelectOption == SelectOptionsEnum.eDocument)
		  {
			oSelectSetting_Doc.setEqualTo(oSelectSetting_Current);
		  }
		  else if (oSelectOption == SelectOptionsEnum.eWindow)
		  {
			oSelectSetting_Win.setEqualTo(oSelectSetting_Current);
		  }

		  oSelectOption = SelectOptionsEnum.eApplication;
		  ButtonWindow.Enabled = false;

		  //final check if it  needs to be updated
		  if (!(clsSelectSetting.compare(oSelectSetting_App, oSelectSetting_Current)) | oBitmap_App == null) // when the dialog is re-opened
		  {

			closeDialogForSnapshot();
		  }
		  else
		  {
			//set existing image to picturebox
			if (oBitmap_App != null)
			{
			  PictureBox1.Image = oBitmap_App;
			  oResultBitmap = oBitmap_App;
			}
		  }

		}
	  }

	  //when the selection is document
	  private void RadioDocument_Click(object sender, System.EventArgs e)
	  {

		if (oSelectOption != SelectOptionsEnum.eDocument)
		{

		  // record it for next use
		  if (oSelectOption == SelectOptionsEnum.eApplication)
		  {
			oSelectSetting_App.setEqualTo(oSelectSetting_Current);
		  }
		  else if (oSelectOption == SelectOptionsEnum.eWindow)
		  {
			oSelectSetting_Win.setEqualTo(oSelectSetting_Current);
		  }

		  oSelectOption = SelectOptionsEnum.eDocument;
		  ButtonWindow.Enabled = false;

		  //final check if it  needs to be updated
		  if (!(clsSelectSetting.compare(oSelectSetting_Doc, oSelectSetting_Current)) | oBitmap_Doc == null) // ' when the dialog is re-opened
		  {

			closeDialogForSnapshot();
		  }
		  else
		  {
			//set existing image to picturebox
			if (oBitmap_Doc != null)
			{
			  PictureBox1.Image = oBitmap_Doc;
			  oResultBitmap = oBitmap_Doc;
			}
		  }
		}
	  }

	  //when the mode is window
	  private void RadioWindow_Click(object sender, System.EventArgs e)
	  {

		if (oSelectOption != SelectOptionsEnum.eWindow)
		{

		  // record it for next use
		  if (oSelectOption == SelectOptionsEnum.eApplication)
		  {
			oSelectSetting_App.setEqualTo(oSelectSetting_Current);
		  }
		  else if (oSelectOption == SelectOptionsEnum.eDocument)
		  {
			oSelectSetting_Doc.setEqualTo(oSelectSetting_Current);
		  }

		  oSelectOption = SelectOptionsEnum.eWindow;
		  ButtonWindow.Enabled = true;

		  //final check if it needs to be updated
		  if (!(clsSelectSetting.compare(oSelectSetting_Win, oSelectSetting_Current)) | oBitmap_Win == null)
		  {

			closeDialogForSnapshot();
		  }
		  else
		  {
			if (!(oHasSelectWindow))
			{
			  //need to select by user. so set preview to warning
			  PictureBox1.Image = GetWarningBitmap();
			  return;
			}
			else
			{
			  //set existing image to picturebox
			  if (oBitmap_Win != null)
			  {
				PictureBox1.Image = oBitmap_Win;
				oResultBitmap = oBitmap_Win;
			  }
			}
		  }
		}
	  }

	  private Bitmap GetWarningBitmap()
	  {

		int bmpWidth = PictureBox1.Width;
		int bmpHeight = PictureBox1.Height;

		Bitmap bmp = new Bitmap(bmpWidth, bmpHeight);
		Graphics gfx = Graphics.FromImage(bmp);

		Font font = new Font("Tahoma", 14F, FontStyle.Bold);

		StringFormat format = new StringFormat();
		format.Alignment = StringAlignment.Center;
		format.LineAlignment = StringAlignment.Center;

		Rectangle rect = new Rectangle(0, 0, bmpWidth, bmpHeight);

		SolidBrush foreBrush = new SolidBrush(System.Drawing.Color.White);
		SolidBrush backBrush = new SolidBrush(System.Drawing.Color.DarkGray);

		gfx.FillRectangle(backBrush, 0, 0, bmpWidth, bmpHeight);
		gfx.DrawString("Please Select Window", font, foreBrush, rect, format);

		return bmp;

	  }

	  //change background color
	  private void ComboBoxBG_SelectionChangeCommitted(object sender, System.EventArgs e)
	  {

		if (oSelectSetting_Current.oBG != ComboBoxBG.SelectedIndex)
		{
		  oSelectSetting_Current.oBG = ComboBoxBG.SelectedIndex;
		  closeDialogForSnapshot();
		}

	  }

	  //change foreground color
	  private void ComboBoxFG_SelectionChangeCommitted(object sender, System.EventArgs e)
	  {

		if (oSelectSetting_Current.oFG != ComboBoxFG.SelectedIndex)
		{
		  oSelectSetting_Current.oFG = ComboBoxFG.SelectedIndex;
		  closeDialogForSnapshot();
		}

	  }

	  //change gray setting
	  private void ComboBoxGray_SelectionChangeCommitted(object sender, System.EventArgs e)
	  {

		if (oSelectSetting_Current.oGray != ComboBoxGray.SelectedIndex)
		{
		  oSelectSetting_Current.oGray = ComboBoxGray.SelectedIndex;
		  if (oSelectSetting_Current.oFG == 0)
		  {
			closeDialogForSnapshot();
		  }
		  else
		  {
			// already force foreground, no need to do gray
		  }
		}

	  }

	  //to save the screenshot
	  private void OK_Click(object sender, System.EventArgs e)
	  {

		if (CheckBoxClipboard.Checked)
		{
		  System.Windows.Forms.Clipboard.SetImage(oResultBitmap);
		}

		if (CheckBoxFile.Checked)
		{
		  try
		  {
			//pop out the dialog to specify the file name.
			System.Windows.Forms.SaveFileDialog ofileDlg = new System.Windows.Forms.SaveFileDialog();
			ofileDlg.Filter = "JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|" + "GIF (*.gif)|*.gif|TIFF (*.tif)|*.tif|All files (*.*)|*.*";

			//save
			if (ofileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
			  oResultBitmap.Save(ofileDlg.FileName, GetFormatForFile(ofileDlg.FileName));
			}
		  }
		  catch (Exception ex)
		  {
			MessageBox.Show("Image creation failed:" + "\r" + ex.ToString());
		  }
		}

		if (CheckBoxPrinter.Checked)
		{
		  try
		  {
			PrintDocument pdoc = new PrintDocument();
			pdoc.PrintPage += pdoc_PrintPage;

			System.Windows.Forms.PrintDialog pdlg = new System.Windows.Forms.PrintDialog();
					pdlg.Document = pdoc;

					//known issue of MS: Print dialog does not show on 64bits OS.
					//http://social.msdn.microsoft.com/Forums/en-US/netfx64bit/thread/a707d202-1a8b-43b1-9fff-08aa7ceb200a
					//*******
					pdlg.UseEXDialog = true;
					//******

			if (pdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
			  pdoc.Print();
			}
		  }
		  catch (Exception ex)
		  {
			MessageBox.Show(ex.ToString());
		  }
		}


	  }

	  //print event
	  public void pdoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
	  {

		//copied from AutoCAD Screenshot plugin created by Kean

		Bitmap toPrint = oResultBitmap;
		int wid = toPrint.Width;
		int hgt = toPrint.Height;
		double ratio = Convert.ToDouble(wid / (double)hgt);

		if (wid != e.MarginBounds.Width)
		{
		  wid = e.MarginBounds.Width;
		  hgt = Convert.ToInt32(wid / ratio);
		}

		if (hgt > e.MarginBounds.Height)
		{
		  hgt = e.MarginBounds.Height;
		  wid = Convert.ToInt32(ratio * hgt);
		}

		e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
		e.Graphics.DrawImage(toPrint, e.MarginBounds.X, e.MarginBounds.Y, wid, hgt);

	  }

	  private void ScreenshotForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
	  {

		// shortcut ESC key for exit
		if (e.KeyCode == Keys.Escape)
		{
		  Cancel_Click(this, System.EventArgs.Empty);
		}

	  }

	  //exit the dialog
	  private void Cancel_Click(object sender, System.EventArgs e)
	  {

		this.Close();
		bTakeSnapShot = false;
	  }

	  //user is going to select window
	  private void ButtonWindow_Click(object sender, System.EventArgs e)
	  {

		selectingWin = true;
		closeDialogForSnapshot();
	  }

	  //resize the controls in the dialog
	  private void ScreenshotForm_SizeChanged(object sender, System.EventArgs e)
	  {

		if (this.Tag == null)
		{
			return;
		}

		string[] tmp = new string[4];
		tmp = this.Tag.ToString().Split(',');

		// forbid the dialog to be smaller than the original value
		if (this.Width < Convert.ToInt16(tmp[0]) | this.Height < Convert.ToInt16(tmp[1]))
		{

		  this.Size = new Size(Convert.ToInt16(tmp[0]), Convert.ToInt16(tmp[1]));
		  return;

		}

		//use the newer values
		int changeX = this.Width - Convert.ToInt16(tmp[0]);
		int changeY = this.Height - Convert.ToInt16(tmp[1]);

		foreach (Control oCtrl in this.Controls)
		{
		  tmp = oCtrl.Tag.ToString().Split(',');
		  oCtrl.Width = Convert.ToInt16(tmp[2]);

		  if (oCtrl.Name == "GroupBoxSelectOp")
		  {
			oCtrl.Height = Convert.ToInt16(tmp[3]) + Convert.ToInt32(changeY / 2.0);
		  }

		  if (oCtrl.Name == "GroupBoxSettings")
		  {
			oCtrl.Top = Convert.ToInt16(tmp[1]) + Convert.ToInt32(changeY / 2.0);
			oCtrl.Height = Convert.ToInt16(tmp[3]) + Convert.ToInt32(changeY / 2.0);
		  }

		  if (oCtrl.Name == "GroupBoxOutput")
		  {
			oCtrl.Top = Convert.ToInt16(tmp[1]) + Convert.ToInt32(changeY);
			oCtrl.Height = Convert.ToInt16(tmp[3]);
		  }

		  if (oCtrl.Name == "PictureBox1")
		  {
			oCtrl.Left = Convert.ToInt16(tmp[0]);
			oCtrl.Top = Convert.ToInt16(tmp[1]);
			oCtrl.Width = Convert.ToInt16(tmp[2]) + changeX;
			oCtrl.Height = Convert.ToInt16(tmp[3]) + changeY;
		  }

		  if (oCtrl.Name == "OK")
		  {
			oCtrl.Left = Convert.ToInt16(tmp[0]);
			oCtrl.Top = Convert.ToInt16(tmp[1]) + changeY;
		  }

		  if (oCtrl.Name == "Cancel")
		  {
			oCtrl.Left = Convert.ToInt16(tmp[0]) + changeX;
			oCtrl.Top = Convert.ToInt16(tmp[1]) + changeY;
		  }
		}
	  }

	  //resize the sub-control in the group box
	  private void GroupBoxSettings_SizeChanged(object sender, System.EventArgs e)
	  {

		if (GroupBoxSettings.Tag == null)
		{
			return;
		}

		string[] tmp = new string[4];
		tmp = GroupBoxSettings.Tag.ToString().Split(',');

		if (GroupBoxSettings.Width < Convert.ToInt16(tmp[2]) | GroupBoxSettings.Height < Convert.ToInt16(tmp[3]))
		{

		  GroupBoxSettings.Size = new Size(Convert.ToInt16(tmp[2]), Convert.ToInt16(tmp[3]));
		  return;
		}

		int changeX = GroupBoxSettings.Width - Convert.ToInt16(tmp[2]);
		int changeY = GroupBoxSettings.Height - Convert.ToInt16(tmp[3]);

		foreach (Control oCtrl in GroupBoxSettings.Controls)
		{

		  tmp = oCtrl.Tag.ToString().Split(',');
		  oCtrl.Left = Convert.ToInt16(tmp[0]);

		  if (oCtrl.Name == "Label1" || oCtrl.Name == "ComboBoxBG")
		  {
			oCtrl.Top = Convert.ToInt16(tmp[1]) + Convert.ToInt32(changeY / 4.0);
		  }

		  if (oCtrl.Name == "Label2" || oCtrl.Name == "ComboBoxFG")
		  {
			oCtrl.Top = Convert.ToInt16(tmp[1]) + Convert.ToInt32(changeY * 2 / 4.0);
		  }

		  if (oCtrl.Name == "Label3" || oCtrl.Name == "ComboBoxGray")
		  {
			oCtrl.Top = Convert.ToInt16(tmp[1]) + Convert.ToInt32(changeY * 3 / 4.0);
		  }
		}
	  }

	  //resize the sub-control in the group box
	  private void GroupBoxSelectOp_SizeChanged(object sender, System.EventArgs e)
	  {

		if (GroupBoxSelectOp.Tag == null)
		{
			return;
		}

		string[] tmp = new string[4];
		tmp = GroupBoxSelectOp.Tag.ToString().Split(',');

		if (GroupBoxSelectOp.Width < Convert.ToInt16(tmp[2]) | GroupBoxSelectOp.Height < Convert.ToInt16(tmp[3]))
		{

		  GroupBoxSelectOp.Size = new Size(Convert.ToInt16(tmp[2]), Convert.ToInt16(tmp[3]));
		  return;
		}

		var changeX = GroupBoxSelectOp.Width - Convert.ToInt16(tmp[2]);
		var changeY = GroupBoxSelectOp.Height - Convert.ToInt16(tmp[3]);

		foreach (Control oCtrl in GroupBoxSelectOp.Controls)
		{

		  tmp = oCtrl.Tag.ToString().Split(',');
		  oCtrl.Left = Convert.ToInt16(tmp[0]);

		  if (oCtrl.Name == "RadioApplication")
		  {
			oCtrl.Top = Convert.ToInt16(tmp[1]) + Convert.ToInt32(changeY / 4.0);
		  }

		  if (oCtrl.Name == "RadioDocument")
		  {
			oCtrl.Top = Convert.ToInt16(tmp[1]) + Convert.ToInt32(changeY * 2 / 4.0);
		  }

		  if (oCtrl.Name == "RadioWindow" || oCtrl.Name == "ButtonWindow")
		  {
			oCtrl.Top = Convert.ToInt16(tmp[1]) + Convert.ToInt32(changeY * 3 / 4.0);
		  }

		}
	  }

#endregion // Form

#region Bitmap

	  //update the bitmap according to select mode and settings
	  private void updateBitmap()
	  {

		if (m_inventorApplication == null)
		{
		  return;
		}

		//prepare the size and corner point
		switch (oSelectOption)
		{
		  case SelectOptionsEnum.eApplication:
			//point and size are always same for application
			oCornerPt = oSelectParam_App.oCornerPt;
			oSize = oSelectParam_App.oSize;
			break;
		  case SelectOptionsEnum.eDocument:
			//point and size are always same for document
			oCornerPt = oSelectParam_Doc.oCornerPt;
			oSize = oSelectParam_Doc.oSize;
			break;
		  case SelectOptionsEnum.eWindow:
			if (!(oHasSelectWindow))
			{
			  //need to select by user. so set preview to warning
			  PictureBox1.Image = GetWarningBitmap();
			  return;
			}
			else
			{
			  oCornerPt = oSelectParam_Win.oCornerPt;
			  oSize = oSelectParam_Win.oSize;
			}
			break;
		}

		System.Drawing.Color oBgColor = System.Drawing.Color.Empty;

		//first priority is background color
		switch (ComboBoxBG.SelectedIndex)
		{
		  case -1:
		  {
			oResultBitmap = createRawBitmap(oSize, oCornerPt);
			if (m_inventorApplication.ActiveDocument == null)
			{
			  return;
			}

			//normal background color 
			Inventor.Color oInvBGColor = null;
			if ((m_inventorApplication.ActiveDocument) is DrawingDocument)
			{
			  DrawingDocument oDrawDoc = (DrawingDocument) m_inventorApplication.ActiveDocument;
			  oInvBGColor = oDrawDoc.SheetSettings.SheetColor;
			}
			else
			{
			  oInvBGColor = m_inventorApplication.ActiveColorScheme.ScreenColor;
			}
			oBgColor = System.Drawing.Color.FromArgb(oInvBGColor.Red, oInvBGColor.Green, oInvBGColor.Blue);

			break;
		  }
		  case 0: // normal
		  {

			oResultBitmap = createRawBitmap(oSize, oCornerPt);
			if (m_inventorApplication.ActiveDocument == null)
			{
			  return;
			}

			//normal background color 
			Inventor.Color oInvBGColor = null;
			if ((m_inventorApplication.ActiveDocument) is DrawingDocument)
			{
                DrawingDocument oDrawDoc = (DrawingDocument) m_inventorApplication.ActiveDocument;
			  oInvBGColor = oDrawDoc.SheetSettings.SheetColor;
			}
			else
			{
			  oInvBGColor = m_inventorApplication.ActiveColorScheme.ScreenColor;
			}
			oBgColor = System.Drawing.Color.FromArgb(oInvBGColor.Red, oInvBGColor.Green, oInvBGColor.Blue);

			break;
		  }
		  case 1: //white Background
		  {
			oResultBitmap = getBmpByForceBGColor(System.Drawing.Color.White);
			oBgColor = System.Drawing.Color.White;
			break;
			}
		}

		//second priority is forecolor or grayscale

		switch (ComboBoxFG.SelectedIndex)
		{
		  case 0:
			switch (ComboBoxGray.SelectedIndex)
			{
			  case 0:
				//do nothing
			  break;
			  case 1: //gray
				oResultBitmap = ConvertToGrayscale(oResultBitmap, oBgColor, false, System.Drawing.Color.Empty);
				break;
			}
			break;
		  case 1: //black
            oResultBitmap = ConvertToGrayscale(oResultBitmap, oBgColor, true, System.Drawing.Color.Black);
			break;
		}


		//set image to picturebox
		if (oResultBitmap != null)
		{
		  PictureBox1.Image = oResultBitmap;

		  //store the bitmaps for the current mode
		  switch (oSelectOption)
		  {
			case SelectOptionsEnum.eApplication:
			  oBitmap_App = oResultBitmap;
			  break;
			case SelectOptionsEnum.eDocument:
			  oBitmap_Doc = oResultBitmap;
			  break;
			case SelectOptionsEnum.eWindow:
			  oBitmap_Win = oResultBitmap;
			  break;
		  }
		}

	  }

      //create raw bitmap (before conversion)
	  private Bitmap createRawBitmap(Size bmSize, System.Drawing.Point bmPt)
	  {
          System.Threading.Thread.Sleep(1000);
		Bitmap bmp = new Bitmap(bmSize.Width, bmSize.Height, PixelFormat.Format32bppArgb);
		Graphics gfx = Graphics.FromImage(bmp);
		gfx.CopyFromScreen(bmPt.X, bmPt.Y, 0, 0, bmSize, CopyPixelOperation.SourceCopy);

		return bmp;

	  }

	  //get bitmap with force background
	  private Bitmap getBmpByForceBGColor(System.Drawing.Color forceColor)
	  {
		Bitmap tempgetBmpByForceBGColor = null;

		if (m_inventorApplication.ActiveDocument == null)
		{
		  return null;
		}

		Document oDoc = m_inventorApplication.ActiveDocument;

		//in drawing document, change the sheet color and snapshot
		if ((oDoc) is DrawingDocument)
		{
		  //change back color and snapshot, and set the color to the original value

		  DrawingDocument oDrawDoc = (DrawingDocument) oDoc;
		  Inventor.Color oSheetOldColor = oDrawDoc.SheetSettings.SheetColor;

		  Inventor.Color oInvForceColor = m_inventorApplication.TransientObjects.CreateColor(forceColor.R, forceColor.G, forceColor.B);

		  //change color and snapshot the bitmap
		  oDrawDoc.SheetSettings.SheetColor = oInvForceColor;
		  m_inventorApplication.ActiveView.Update();
		  tempgetBmpByForceBGColor = createRawBitmap(oSize, oCornerPt);
		  //set color back
		  oDrawDoc.SheetSettings.SheetColor = oSheetOldColor;

		}
		else //other type of document, set color info of color skema
		{

            Inventor.BackgroundTypeEnum oOldBGType = m_inventorApplication.ColorSchemes.BackgroundType;
		  string oOldBGImage = "";
		  if (oOldBGType == BackgroundTypeEnum.kImageBackgroundType)
		  {
			oOldBGImage = m_inventorApplication.ActiveColorScheme.ImageFullFileName;
		  }

		  //change background color
		  m_inventorApplication.ColorSchemes.BackgroundType = BackgroundTypeEnum.kImageBackgroundType;

		  if (forceColor == System.Drawing.Color.White)
		  {
			m_inventorApplication.ActiveColorScheme.ImageFullFileName = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location) + "\\Resources\\white.bmp";
		  }

		  m_inventorApplication.ActiveView.Update();
		  tempgetBmpByForceBGColor = createRawBitmap(oSize, oCornerPt);

		  //set the color back
		  m_inventorApplication.ColorSchemes.BackgroundType = oOldBGType;
		  if (oOldBGType == BackgroundTypeEnum.kImageBackgroundType)
		  {
			m_inventorApplication.ActiveColorScheme.ImageFullFileName = oOldBGImage;
		  }

		}
		return tempgetBmpByForceBGColor;
	  }

	  //gray conversion
	  public Bitmap ConvertToGrayscale(Bitmap source, System.Drawing.Color bgColor, bool forceFgColor, System.Drawing.Color fgcolor)
	  {

		// From http://www.bobpowell.net/grayscale.htm 

		this.Enabled = false;

		Bitmap bm = new Bitmap(source.Width, source.Height);


	      long oProgress = 0;

		ProgressForm oProgressForm = new ProgressForm();
	    oProgressForm.SetMax(bm.Width*bm.Height);
        oProgressForm.Show();
        try
        {
            for (int y = 0; y < bm.Height; y++)
            {
                for (int x = 0; x < bm.Width; x++)
                {

                    System.Drawing.Color c = source.GetPixel(x, y);

                    if (forceFgColor)
                    {
                        if (!(SameColors(c, bgColor)))
                        {
                            bm.SetPixel(x, y, fgcolor);
                        }
                        else
                        {
                            //set the original color  
                            bm.SetPixel(x, y, c);
                        }
                    }
                    else
                    {
                        int lum = Convert.ToInt32(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                        bm.SetPixel(x, y, System.Drawing.Color.FromArgb(lum, lum, lum));
                    }

                    //update progress bar
                    oProgressForm.SetProgress();
                }
            }
        }
        catch (Exception ex)
        {
        }

		oProgressForm.Dispose();
		oProgressForm = null;

		this.Enabled = true;

		return bm;

	  }

	  //compare if the same color
	  private bool SameColors(System.Drawing.Color a, System.Drawing.Color b)
	  {
		return a.R == b.R && a.B == b.B && a.G == b.G;
	  }

#endregion //bitmap

#region other

	  //do select in screen
	  public void DoSelect()
	  {

		switch (oSelectOption)
		{
		  case SelectOptionsEnum.eApplication:

			if (m_inventorApplication.SoftwareVersion.Major > 13)
			{
			  m_inventorApplication.UserInterfaceManager.DoEvents();
			}
			else
			{
			  System.Windows.Forms.Application.DoEvents();
			}

			break;
		  case SelectOptionsEnum.eDocument:

			if (m_inventorApplication.SoftwareVersion.Major > 13)
			{
			  m_inventorApplication.UserInterfaceManager.DoEvents();
			}
			else
			{
			  System.Windows.Forms.Application.DoEvents();
			}

			break;
		  case SelectOptionsEnum.eWindow:

			if (selectingWin)
			{

			  selectingWin = false; // ready for next status

			  //get new corner and size of window
			  InteractionEventsManager oInterEventsM = new InteractionEventsManager(m_inventorApplication);

			  Size otempSize = new Size(0, 0);
			  Inventor.Point2d otempInvPoint2d = m_inventorApplication.TransientGeometry.CreatePoint2d(0, 0);

			  oInterEventsM.DoSelectRegion(ref otempSize, ref otempInvPoint2d);

			  //record the size and corner point.
			  if (otempSize.Height == 0 || otempSize.Width == 0)
			  {
				// the user may escape the selecting without selecting anything
				// so do nothing 
				return;
			  }
			    oHasSelectWindow = true;
			    oSelectParam_Win.oCornerPt.X = Convert.ToInt32(otempInvPoint2d.X);
			    oSelectParam_Win.oCornerPt.Y = Convert.ToInt32(otempInvPoint2d.Y);
			    oSelectParam_Win.oSize = otempSize;
			}
			else
			{
			  //just change selection mode. so update the bitmap directly
			  if (m_inventorApplication.SoftwareVersion.Major > 13)
			  {
				m_inventorApplication.UserInterfaceManager.DoEvents();
			  }
			  else
			  {
				System.Windows.Forms.Application.DoEvents();
			  }
			}

			break;
		}

		updateBitmap();

	  }

	  //get file format
	  private ImageFormat GetFormatForFile(string filename)
	  {

		// If all else fails, let's create a PNG 
		// (might also choose to throw an exception) 

		ImageFormat imf = ImageFormat.Png;
		if (filename.Contains("."))
		{
		  // Get the filename's extension (what follows the last ".") 

		  string ext = filename.Substring(filename.LastIndexOf(".") + 1);

		  // Get the first three characters of the extension 

		  if (ext.Length > 3)
		  {
			ext = ext.Substring(0, 3);
		  }

		  // Choose the format based on the extension (in lowercase) 

		  switch (ext.ToLower())
		  {
			case "bmp":
			  imf = ImageFormat.Bmp;
			  break;
			case "gif":
			  imf = ImageFormat.Gif;
			  break;
			case "jpg":
			  imf = ImageFormat.Jpeg;
			  break;
			case "tif":
			  imf = ImageFormat.Tiff;
			  break;
			case "wmf":
			  imf = ImageFormat.Wmf;
			  break;
			default:
			  imf = ImageFormat.Png;
			  break;
		  }
		}
		return imf;
	  }

	  //prepare the bitmap before loading the dialog.
	  //Since the document may have been modified,
	  //the bitmap should reflect this change
	  public void prepareBitmapsBeforeLoadDialog()
	  {

		// the dialog is re-opened, ready to re-generate when changing mode next time
		if (oSelectOption != SelectOptionsEnum.eApplication)
		{
		  oBitmap_App = null;
		}
		if (oSelectOption != SelectOptionsEnum.eDocument)
		{
		  oBitmap_Doc = null;
		}
		if (oSelectOption != SelectOptionsEnum.eWindow && oHasSelectWindow)
		{
		  oBitmap_Win = null;
		}

		//just re-generate the bitmap for current mode whendialog is re-opened
		updateBitmap();

	  }

#endregion

	}
}
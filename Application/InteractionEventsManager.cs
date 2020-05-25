//INSTANT C# NOTE: Formerly VB project-level imports:
using System;
using System.Collections;
using System.Diagnostics;

using Inventor;
using System.Drawing;

namespace InventorScreenshot
{
	public class InteractionEventsManager
	{

	  private Inventor.Application m_inventorApplication;

	  private Point2d oCornerPt;
	  private Size oSize;

	  //Interaction Events
	  private InteractionEvents m_InteractionEvents;

	  //Mouse event
	  private MouseEvents m_MouseEvents;

	  //Pick event for selecting object
	  private SelectEvents m_SelectEvents;

	  //flag to indicate the mouse is down
	  private bool m_flagMouseDown;

	  // start point,  for screenshot
		private Inventor.Point2d m_MouseStartViewPt;

		//start point, for temporary selecting rectangle
		private Inventor.Point m_StartModelPt;

		//flag to indicate screenshot is running 
		private bool isRunningScreenshot;

		//graphics node
		private GraphicsNode oGiNode;
		//coordinate set for graphics node
		private GraphicsCoordinateSet oCoordSet;
		//color set for graphics node
		private GraphicsColorSet oColorSet;
		//Line strip Graphics
		private LineStripGraphics oGiLineStripG;
		//Line  Graphics (for Inventor 2009 only)
		private LineGraphics oGiLineG;



		public InteractionEventsManager(Inventor.Application oApp)
		{
			m_inventorApplication = oApp;
			oCornerPt = m_inventorApplication.TransientGeometry.CreatePoint2d(0, 0);
			oSize = new Size();
//INSTANT C# NOTE: Converted event handler wireups:
        }

		public void DoSelectRegion(ref Size bmSize, ref Inventor.Point2d bmCornetPt)
		{

			StartEvent(true);
			bmSize = oSize;
			bmCornetPt = oCornerPt;
		}

		public void DoSelectObject(ref Size bmSize, ref Inventor.Point2d bmCornetPt)
		{

			StartEvent(false);
			bmSize = oSize;
			bmCornetPt = oCornerPt;
		}

		private void StartEvent(bool region_or_object)
		{

			//start interaction event
			if (m_InteractionEvents == null)
			{
				m_InteractionEvents = m_inventorApplication.CommandManager.CreateInteractionEvents();
			}
			else
			{
				m_InteractionEvents.Stop();
			}

			m_InteractionEvents.InteractionDisabled = false;

			if (region_or_object)
			{
				//get mouse event
				if (m_MouseEvents == null)
				{
					m_MouseEvents = m_InteractionEvents.MouseEvents;
					m_MouseEvents.MouseMoveEnabled = true;
					m_MouseStartViewPt = m_inventorApplication.TransientGeometry.CreatePoint2d(0, 0);
					m_flagMouseDown = false;
                    m_MouseEvents.OnMouseUp += m_MouseEvents_OnMouseUp;
                    m_MouseEvents.OnMouseMove += m_MouseEvents_OnMouseMove;
                    m_MouseEvents.OnMouseDown += m_MouseEvents_OnMouseDown;
				}
			}
			else
			{
				//get select event
				if (m_SelectEvents == null)
				{
					m_SelectEvents = m_InteractionEvents.SelectEvents;
					m_SelectEvents.SingleSelectEnabled = false;
					m_SelectEvents.WindowSelectEnabled = true;
                    m_SelectEvents.OnSelect += m_SelectEvents_OnSelect;
				}
			}



            m_InteractionEvents.OnTerminate += m_InteractionEvents_OnTerminate;

			m_InteractionEvents.Name = "MyScreenshot";

			//start
			m_InteractionEvents.Start();

			while (m_InteractionEvents != null)
			{
				if (m_inventorApplication.SoftwareVersion.Major > 13)
				{
					m_inventorApplication.UserInterfaceManager.DoEvents();
				}
				else
				{
					System.Windows.Forms.Application.DoEvents();
				}
			}
		}

		private void m_InteractionEvents_OnTerminate()
		{

            m_InteractionEvents.InteractionGraphics.PreviewClientGraphics.Delete();
            m_inventorApplication.ActiveView.Update();
            m_flagMouseDown = false;
            m_InteractionEvents.Stop();
            m_MouseEvents = null;
            m_SelectEvents = null;
            m_InteractionEvents = null;
		}

		private void m_MouseEvents_OnMouseDown(Inventor.MouseButtonEnum Button, Inventor.ShiftStateEnum ShiftKeys, Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View)
		{

			//if the interaction event is MyScreenshot,
			//then get the view position and model position

			if (m_InteractionEvents.Name == "MyScreenshot")
			{
				m_MouseStartViewPt = ViewPosition;
				m_StartModelPt = ModelPosition;
				m_flagMouseDown = true;

				//clean the last graphics
				m_InteractionEvents.InteractionGraphics.PreviewClientGraphics.Delete();
				m_inventorApplication.ActiveView.Update();

				//gi node
				oGiNode = m_InteractionEvents.InteractionGraphics.PreviewClientGraphics.AddNode(1);
				oCoordSet = m_InteractionEvents.InteractionGraphics.GraphicsDataSets.CreateCoordinateSet(1);

				//color set
				oColorSet = m_InteractionEvents.InteractionGraphics.GraphicsDataSets.CreateColorSet(1);
				oColorSet.Add(1, 255, 0, 0);

				TransientGeometry tg = m_inventorApplication.TransientGeometry;
				Inventor.Point tempP = tg.CreatePoint(ViewPosition.X, ViewPosition.Y, 0);

				oCoordSet.Add(1, tempP);
				oCoordSet.Add(2, tempP);
				oCoordSet.Add(3, tempP);
				oCoordSet.Add(4, tempP);
				oCoordSet.Add(5, tempP);

				try
				{
					if (oGiLineStripG != null)
					{
						oGiLineStripG.Delete();
						oGiLineStripG = null;
					}
					oGiLineStripG = oGiNode.AddLineStripGraphics();
					oGiLineStripG.CoordinateSet = oCoordSet;
					oGiLineStripG.ColorSet = oColorSet;
					oGiLineStripG.BurnThrough = true;
				}
				catch (Exception ex)
				{
					//a problem in Inventor 2009( R13 ) with 
					//LineStripGraphics.BurnThrough. Use LineGraphics as workaround

					if (oGiLineG != null)
					{
						oGiLineG.Delete();
						oGiLineG = null;
					}

					oGiLineG = oGiNode.AddLineGraphics();
					oGiLineG.CoordinateSet = oCoordSet;
					oGiLineG.ColorSet = oColorSet;
					oGiLineG.BurnThrough = true;
				}


			}
		}

		// version 2010-05-23: to solve the issue in Perspective View
		private void m_MouseEvents_OnMouseMove(Inventor.MouseButtonEnum Button, Inventor.ShiftStateEnum ShiftKeys, Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View)
		{

			//if the interaction event is MyScreenshot, draw selecting rectangle.
			if (m_InteractionEvents.Name == "MyScreenshot" && m_flagMouseDown)
			{

				TransientGeometry tg = m_inventorApplication.TransientGeometry;

				Inventor.Point P1 = tg.CreatePoint(m_MouseStartViewPt.X, -m_MouseStartViewPt.Y, 0);
				Inventor.Point P3 = tg.CreatePoint(ViewPosition.X, -ViewPosition.Y, 0);
				Inventor.Point P4 = tg.CreatePoint(P1.X, P3.Y, 0);
				Inventor.Point P2 = tg.CreatePoint(P3.X, P1.Y, 0);

				//update coordinates

				oCoordSet[1] = P1;
				oCoordSet[2] = P2;
				oCoordSet[3] = P3;
				oCoordSet[4] = P4;
				oCoordSet[5] = P1;

				//add line strip 
				if (oGiLineStripG != null)
				{
					//SetTransformBehavior, default value for PixelScale is 1
					oGiLineStripG.SetTransformBehavior(P1, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling, 1);
					oGiLineStripG.SetViewSpaceAnchor(P1, m_MouseStartViewPt, ViewLayoutEnum.kTopLeftViewCorner);
				}
				else if (oGiLineG != null)
				{
					//SetTransformBehavior, default value for PixelScale is 1
					oGiLineG.SetTransformBehavior(P1, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling, 1);
					oGiLineG.SetViewSpaceAnchor(P1, m_MouseStartViewPt, ViewLayoutEnum.kTopLeftViewCorner);
				}


				m_inventorApplication.ActiveView.Update();

			}
		}


		private void m_MouseEvents_OnMouseUp(Inventor.MouseButtonEnum Button, Inventor.ShiftStateEnum ShiftKeys, Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View)
		{

			//if the interaction event is MyScreenshot, create the image
			if (m_InteractionEvents.Name == "MyScreenshot")
			{

				m_InteractionEvents.InteractionGraphics.PreviewClientGraphics.Delete();
				m_inventorApplication.ActiveView.Update();

				if (oGiLineStripG != null)
				{
					oGiLineStripG.Delete();
					oGiLineStripG = null;
				}

				if (oGiLineG != null)
				{
					oGiLineG.Delete();
					oGiLineG = null;
				}

				//stop interaction event
				m_InteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow);
				m_flagMouseDown = false;
                if (m_MouseEvents != null)
                {
                    m_MouseEvents.OnMouseDown -= m_MouseEvents_OnMouseDown;
                    m_MouseEvents.OnMouseMove -= m_MouseEvents_OnMouseMove;
                    m_MouseEvents.OnMouseUp -= m_MouseEvents_OnMouseUp;
                    m_MouseEvents = null;
                }
				m_InteractionEvents.Stop();
				m_InteractionEvents = null;

				//prepare size of the image region, in pixel

                oSize = new Size(Convert.ToInt32(Math.Abs(ViewPosition.X - m_MouseStartViewPt.X)), Convert.ToInt32(Math.Abs(ViewPosition.Y - m_MouseStartViewPt.Y)));

				if (ViewPosition.X > m_MouseStartViewPt.X & ViewPosition.Y > m_MouseStartViewPt.Y)
				{

					oCornerPt = m_MouseStartViewPt;
				}
				else if (ViewPosition.X > m_MouseStartViewPt.X & ViewPosition.Y < m_MouseStartViewPt.Y)
				{

					oCornerPt.X = m_MouseStartViewPt.X;
					oCornerPt.Y = ViewPosition.Y;
				}
				else if (ViewPosition.X < m_MouseStartViewPt.X & ViewPosition.Y > m_MouseStartViewPt.Y)
				{

					oCornerPt.X = ViewPosition.X;
					oCornerPt.Y = m_MouseStartViewPt.Y;
				}
				else
				{
					oCornerPt = ViewPosition;
				}

				//take the view position in screen, calculate
				//the real pixel data of the corners

				oCornerPt.X = (int)View.Left + oCornerPt.X;
				oCornerPt.Y = (int)View.Top + oCornerPt.Y;
			}

		}

	  private void m_SelectEvents_OnSelect(Inventor.ObjectsEnumerator JustSelectedEntities, Inventor.SelectionDeviceEnum SelectionDevice, Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View)
	  {

		//reserved for future
	  }

	}
}
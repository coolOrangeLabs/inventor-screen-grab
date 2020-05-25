using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using InventorScreenshot.Commands;
using InventorScreenshot.Utilities;

namespace InventorScreenshot
{
      [GuidAttribute("b3aa6727-f2d0-4c6d-8150-16fa9c493dff")]
	  public class StandardAddInServer : Inventor.ApplicationAddInServer
	  {
          // Create a logger for use in this class
        private static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public StandardAddInServer()
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            FileInfo fi = new FileInfo(thisAssembly.Location + ".log4net");
            log4net.GlobalContext.Properties["LogFileName"] = fi.DirectoryName + "\\Log\\screenGrab";
            log4net.Config.XmlConfigurator.Configure(fi);
            log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
		// Inventor application object.
		private Inventor.Application m_inventorApplication;

		// GUID of the AddIn
		private string m_ClientId;


          #region ApplicationAddInServer Members

		public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
		{

		  // This method is called by Inventor when it loads the AddIn.
		  // The AddInSiteObject provides access to the Inventor Application object.
		  // The FirstTime flag indicates if the AddIn is loaded for the first time.
		  // Initialize AddIn members.
		    log.Debug("ScreenShot loaded!");
            m_inventorApplication = addInSiteObject.Application;

            m_ClientId = "{b3aa6727-f2d0-4c6d-8150-16fa9c493dff}";
            Type addinType = this.GetType();
            AdnCommand.AddCommand(
                new ScreenGrabCtrlCmd(
                    m_inventorApplication));

            AdnCommand.AddCommand(
                new ScreengrabAboutCtrlCmd(
                    m_inventorApplication));

            AdnCommand.AddCommand(
                new ScreenGrabHelpCtrlCmd(
                    m_inventorApplication));

            // Only after all commands have been added,
            // load Ribbon UI from customized xml file.
            // Make sure "InternalName" of above commands is matching 
            // "internalName" tag described in xml of corresponding command.
            AdnRibbonBuilder.CreateRibbon(
                m_inventorApplication,
               addinType,
               "InventorScreenshot.Resources.ribbons.xml");

        }

		public void Deactivate()
		{

		  // This method is called by Inventor when the AddIn is unloaded.
		  // The AddIn will be unloaded either manually by the user or
		  // when the Inventor session is terminated.
            
		  // Release objects.
		  Marshal.ReleaseComObject(m_inventorApplication);
		  m_inventorApplication = null;
            
		}

		public object Automation
		{

		  // This property is provided to allow the AddIn to expose an API 
		  // of its own to other programs. Typically, this  would be done by
		  // implementing the AddIn's API interface in a class and returning 
		  // that class object through this property.

		  get
		  {
			return null;
		  }

		}

		public void ExecuteCommand(int commandID)
		{

		  // Note:this method is now obsolete, you should use the 
		  // ControlDefinition functionality for implementing commands.

		}

#endregion

	}

}

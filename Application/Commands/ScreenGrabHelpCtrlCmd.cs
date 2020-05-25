using System;
using System.Reflection;
using System.Windows.Forms;
using Inventor;
using InventorScreenshot.Utilities;
using Application = Inventor.Application;

namespace InventorScreenshot.Commands
{
    class ScreenGrabHelpCtrlCmd : AdnButtonCommandBase
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ScreenGrabHelpCtrlCmd(Application application) : base(application)
        {
        }

        public override string DisplayName
        {
            get { return "Help"; }
        }

        public override string InternalName
        {
            get { return "coolOrange.Screenshot.ScreenGrabHelpCtrlCmd"; }
        }

        public override CommandTypesEnum Classification
        {
            get { return CommandTypesEnum.kEditMaskCmdType; }
        }

        public override string ClientId
        {
            get
            {
                Type t = typeof(StandardAddInServer);
                return t.GUID.ToString("B");
            }
        }

        public override string Description
        {
            get { return "coolOrange screenGrab help"; }
        }

        public override string ToolTipText
        {
            get { return "screenGrab help"; }
        }

        public override ButtonDisplayEnum ButtonDisplay
        {
            get { return ButtonDisplayEnum.kDisplayTextInLearningMode; }
        }

        public override string StandardIconName
        {
            get
            {
                return "InventorScreenshot.Resources.Help.ico";
            }
        }

        public override string LargeIconName
        {
            get
            {
                return "InventorScreenshot.Resources.Help.ico";
            }
        }

        protected override void OnExecute(NameValueMap context)
        {
            System.Diagnostics.Process.Start("http://wiki.coolorange.com/display/SCREEN");
            Terminate();
        }

        protected override void OnHelp(NameValueMap context)
        {
        }
    }
}

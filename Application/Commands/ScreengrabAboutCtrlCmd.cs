using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Inventor;
using InventorScreenshot.Properties;
using InventorScreenshot.Utilities;

namespace InventorScreenshot.Commands
{
    class ScreengrabAboutCtrlCmd : AdnButtonCommandBase
    {
        public ScreengrabAboutCtrlCmd(Application application) : base(application)
        {
        }

        public override string DisplayName
        {
            get { return "About"; }
        }

        public override string InternalName
        {
            get { return "coolOrange.Screenshot.ScreenGrabAboutCtrlCmd"; }
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
            get { return "coolOrange screenGrab version"; }
        }

        public override string ToolTipText
        {
            get { return "About screenGrab"; }
        }

        public override ButtonDisplayEnum ButtonDisplay
        {
            get { return ButtonDisplayEnum.kDisplayTextInLearningMode; }
        }

        public override string StandardIconName
        {
            get
            {
                return "InventorScreenshot.Resources.about.ico";
            }
        }

        public override string LargeIconName
        {
            get
            {
                return "InventorScreenshot.Resources.about.ico";
            }
        }

        protected override void OnExecute(NameValueMap context)
        {
            var frmSplashAbout = new FrmSplash()
            {
                lblInfo = { Text = "Free License" },
                versionlbl = { Text = "2019" },
                buildlbl = { Text = Assembly.GetExecutingAssembly().GetName().Version.ToString() },
                BackgroundImage = Resources.screenGrab1
            };
            RegisterCommandForm(frmSplashAbout, true);
        }

        protected override void OnHelp(NameValueMap context)
        {
        }
    }
}

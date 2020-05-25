using System;
using System.Reflection;
using Inventor;
using InventorScreenshot.Utilities;

namespace InventorScreenshot.Commands
{
    class ScreenGrabCtrlCmd : AdnButtonCommandBase
    {
        // Create a logger for use in this class
        private static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ScreenshotForm _screenShotForm;
        private const int MDelaytime = 500;

        public ScreenGrabCtrlCmd(Application application)
            : base(application)
        {
        }

        public override string DisplayName
        {
            get { return "screenGrab"; }
        }

        public override string InternalName
        {
            get { return "coolOrange.Screenshot.ScreenGrabCtrlCmd"; }
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
            get { return "Simple screen capture utility for Autodesk Inventor"; }
        }

        public override string ToolTipText
        {
            get { return "Takes a screen shot"; }
        }

        public override ButtonDisplayEnum ButtonDisplay
        {
            get { return ButtonDisplayEnum.kDisplayTextInLearningMode; }
        }

        public override string StandardIconName
        {
            get
            {
                return "InventorScreenshot.Resources.screenGrab.ico";
            }
        }

        public override string LargeIconName
        {
            get
            {
                return "InventorScreenshot.Resources.screenGrab.ico";
            }
        }

        protected override void OnExecute(NameValueMap context)
        {
            log.Info("ScreenGrab started...");
            if (_screenShotForm == null)
            {
                _screenShotForm = new ScreenshotForm(Application);
                _screenShotForm.Left = Convert.ToInt32(Application.Left + (Application.Width - _screenShotForm.Width) / 2.0);
                _screenShotForm.Top = Convert.ToInt32(Application.Top + (Application.Height - _screenShotForm.Height) / 2.0);
            }
            _screenShotForm.prepareBitmapsBeforeLoadDialog();
            do
            {
                _screenShotForm.bTakeSnapShot = false;
                _screenShotForm.ShowDialog();

                if (_screenShotForm.bTakeSnapShot)
                {

                    //delay for specific machine. adjust the value in app.config.
                    try
                    {
                        System.Threading.Thread.Sleep(Properties.Settings.Default.delayTime);
                    }
                    catch (Exception ex)
                    {
                        System.Threading.Thread.Sleep(MDelaytime);
                        log.Error("Problem getting delaytime from app.config. " + ex);
                    }

                    _screenShotForm.DoSelect();
                }
            } while ((_screenShotForm.bTakeSnapShot));
            
            Terminate();
            Application.CommandManager.StopActiveCommand();
            log.Info("ScreenGrab completed.");
        }

        protected override void OnHelp(NameValueMap context)
        {
        }


    }
}

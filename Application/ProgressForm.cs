//INSTANT C# NOTE: Formerly VB project-level imports:
using System;
using System.Collections;
using System.Diagnostics;

namespace InventorScreenshot
{
    public partial class ProgressForm
    {

        internal ProgressForm()
        {
            InitializeComponent();
        }
        private void ProgressForm_Load(object sender, System.EventArgs e)
        {
            ProgressBar1.Minimum = 0;
            ProgressBar1.Step = 1;
        }

        public void SetProgress()
        {
            ProgressBar1.PerformStep();
        }

        public void SetMax(int i)
        {
            ProgressBar1.Maximum = i;
        }
    }
}
using System;
using System.Windows.Forms;
#if CDBXP
using Microsoft.Win32.Windows7;
#endif
using System.ComponentModel;

namespace CDBurnerXP.Controls
{
    public class MarqueeProgressBar : ProgressBar
    {

        private bool m_ShowMarquee = false;
        private bool m_ShowInTaskbar = true;
        private Timer m_Timer = new Timer();

        #region "Properties"

        [DefaultValue(false)]
        public bool ShowMarquee
        {
            get { return m_ShowMarquee; }
            set
            {
                m_ShowMarquee = value;
                if (value == true)
                {
                    EnableMarque();
                }
                else
                {
                    DisableMarquee();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether or not the value of 
        /// the progress bar should be shown in the
        /// Windows 7 task bar.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowInTaskbar
        {
            get { return m_ShowInTaskbar; }
            set { m_ShowInTaskbar = value; }
        }

        public new int Value
        {
            set
            {
                base.Value = Math.Max(Minimum, Math.Min(Maximum, value));
                #if CDBXP
                if (m_ShowInTaskbar)
                {
                    if (base.Value == 0)
                    {
                        TaskBarExtensions.SetProgressState(ProgressState.NoProgress);
                    }
                    else
                    {
                        TaskBarExtensions.SetProgressState(m_ShowMarquee ? ProgressState.Indeterminate : ProgressState.Normal);
                        TaskBarExtensions.SetProgressValue((ulong)base.Value, (ulong)Maximum);
                    }
                }
                #endif
            }
            get
            {
                return base.Value;
            }
        }

        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            MarqueeAnimationSpeed = 40;
        }

        private bool IsOldOs()
        {
            // we have to emulate marquee for win2000 and older
            return Environment.OSVersion.Version.Major <= 5 && Environment.OSVersion.Version.Minor <= 0;
        }

        private void EnableMarque()
        {
            if (IsOldOs())
            {
                this.Step = 5;
                m_Timer.Interval = 100;
                m_Timer.Start();
            }
            else
            {
                #if CDBXP
                TaskBarExtensions.SetProgressState(ProgressState.Indeterminate);
                #endif
                Style = ProgressBarStyle.Marquee;
            }
        }

        private void DisableMarquee()
        {
            if (IsOldOs())
            {
                m_Timer.Stop();
            }
            else
            {
#if CDBXP
                TaskBarExtensions.SetProgressState(ProgressState.Normal);
#endif
                Style = ProgressBarStyle.Blocks;
            }
        }

        private void DoMarque(object sender, EventArgs e)
        {
            if (Value == 100)
            {
                Value = 0;
            }
            else
            {
                this.PerformStep();
            }
        }

        protected override void Dispose(bool disposing)
        {
#if CDBXP
            if (m_ShowInTaskbar)
            {
                TaskBarExtensions.SetProgressState(ProgressState.NoProgress);
            }
#endif
            if (disposing)
            {
                m_Timer.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WikiSpeak
{
    public partial class ArticleUserControl : UserControl
    {
        public ArticleUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// event signalling that the item got selected
        /// </summary>
        public event EventHandler PlayButtonClicked;

        /// <summary>
        /// Fires the Tapped event
        /// </summary>
        private void FirePlayButtonClickedEvent()
        {
            if (PlayButtonClicked != null)
            {
                PlayButtonClicked(this, new EventArgs());
            }
        }

        private void PlayButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            FirePlayButtonClickedEvent();
        }
    }
}

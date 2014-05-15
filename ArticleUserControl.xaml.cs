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
        /// Handles the control being tapped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            FireTappedEvent();
        }

        /// <summary>
        /// event signalling that the item got selected
        /// </summary>
        public event EventHandler Tapped;

        /// <summary>
        /// Fires the Tapped event
        /// </summary>
        private void FireTappedEvent()
        {
            if (Tapped != null)
            {
                Tapped(this, new EventArgs());
            }
        }
    }
}

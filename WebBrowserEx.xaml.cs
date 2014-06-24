using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Globalization;

namespace WikiSpeak
{
    public partial class WebBrowserEx : UserControl
    {
        public WebBrowserEx()
        {
            InitializeComponent();

            this.Browser.IsScriptEnabled = true;
            this.Browser.LoadCompleted += Browser_LoadCompleted;
        }

        /// <summary>
        /// Handler for when the browser has finished loading. When this happens it is safe to call scripts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            RefreshActiveFragmentIndex();
        }

        /// <summary>
        /// The filename for the webbrowser HTML
        /// </summary>
        private const string LocalHtmlFilename = "webbrowser{0}.html";

        /// <summary>
        /// Refreshes the contents of the page from background, text, etc.
        /// </summary>
        private void RefreshBrowserContents()
        {
            string filename = string.Format(LocalHtmlFilename, this.Name);

            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                streamWriter.Write(HTML);
                streamWriter.Close();
            }

            this.Browser.Navigate(new Uri(filename, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Refreshes the active fragment index on the browser
        /// </summary>
        private void RefreshActiveFragmentIndex()
        {
            try
            {
                Browser.InvokeScript("HighlightFragment", ActiveFragmentIndex.ToString(CultureInfo.InvariantCulture));
            }
            catch
            {
                // Do nothing on failure?
            }
        }

        #region HTML Property

        /// <summary>
        /// BackgroundImg property definition
        /// </summary>
        public static readonly DependencyProperty HTMLProperty =
            DependencyProperty.Register(
                "HTML",
                typeof(string),
                typeof(WebBrowserEx),
                new PropertyMetadata(string.Empty, OnHTMLPropertyChanged));

        /// <summary>
        /// Gets or sets the HTML contents to display
        /// </summary>
        public string HTML
        {
            get
            {
                return (string)GetValue(HTMLProperty);
            }
            set
            {
                SetValue(HTMLProperty, value);
            }
        }

        /// <summary>
        /// Handler for when the HTML contents have changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void OnHTMLPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            WebBrowserEx webBrowserEx = sender as WebBrowserEx;
            if (webBrowserEx != null)
            {
                webBrowserEx.RefreshBrowserContents();
            }
        }

        #endregion HTML Property

        #region ActiveFragmentIndex Property

        /// <summary>
        /// BackgroundImg property definition
        /// </summary>
        public static readonly DependencyProperty ActiveFragmentIndexProperty =
            DependencyProperty.Register(
                "ActiveFragmentIndex",
                typeof(int),
                typeof(WebBrowserEx),
                new PropertyMetadata(-1, OnActiveFragmentIndexPropertyChanged));

        /// <summary>
        /// Gets or sets the active fragment index
        /// </summary>
        public int ActiveFragmentIndex
        {
            get
            {
                return (int)GetValue(ActiveFragmentIndexProperty);
            }
            set
            {
                SetValue(ActiveFragmentIndexProperty, value);
            }
        }

        /// <summary>
        /// Handler for when the active fragment index has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void OnActiveFragmentIndexPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            WebBrowserEx webBrowserEx = sender as WebBrowserEx;
            if (webBrowserEx != null)
            {
                webBrowserEx.RefreshActiveFragmentIndex();
            }
        }

        #endregion ActiveFragmentIndex Property

    }
}

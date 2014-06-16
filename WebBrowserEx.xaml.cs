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

namespace WikiSpeak
{
    public partial class WebBrowserEx : UserControl
    {
        public WebBrowserEx()
        {
            InitializeComponent();

            _text.ActiveFragmentChanged += ActiveFragmentChangedHandler;
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
                streamWriter.Write(_text.HTML);
                streamWriter.Close();
            }

            this.Browser.Navigate(new Uri(filename, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// The text
        /// </summary>
        public StringEx _text = new StringEx();

        #region Text Property

        /// <summary>
        /// BackgroundImg property definition
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(WebBrowserEx),
                new PropertyMetadata(string.Empty, OnTextPropertyChanged));

        /// <summary>
        /// Gets or sets the Text
        /// </summary>
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// Handler for when the Text has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void OnTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            WebBrowserEx webBrowserEx = sender as WebBrowserEx;
            if (webBrowserEx != null)
            {
                webBrowserEx._text.Clear();
                webBrowserEx._text.Append(webBrowserEx.GetValue(TextProperty) as string);

                webBrowserEx.RefreshBrowserContents();
            }
        }

        #endregion Text Property

        /// <summary>
        /// Handler for when the active fragment changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ActiveFragmentChangedHandler(object sender, FragmentEventArgs args)
        {
            RefreshBrowserContents();
        }
    }
}

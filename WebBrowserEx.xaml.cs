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

        }

        private const string LocalBackgroundImgFilename = "webbrowserbackground.jpg";
        private const string LocalHtmlFilename = "webbrowser.html";

        /// <summary>
        /// Refreshes the contents of the page from background, text, etc.
        /// </summary>
        private void RefreshLocalStorage()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            using (IsolatedStorageFileStream targetStream = new IsolatedStorageFileStream(LocalBackgroundImgFilename, FileMode.OpenOrCreate, storage))
            {
                string backgroundImgUriAsString = this.GetValue(BackgroundImgProperty) as string;
                Uri backgroundImgUri = new Uri(backgroundImgUriAsString, UriKind.RelativeOrAbsolute);
                Stream sourceStream = App.GetResourceStream(backgroundImgUri).Stream;

                using (BinaryWriter writer = new BinaryWriter(targetStream))
                {
                    sourceStream.CopyTo(targetStream);
                    targetStream.Close();
                }

                string text;
                using (StreamReader streamReader = new StreamReader(App.GetResourceStream(new Uri("Assets/WebBrowser.html", UriKind.Relative)).Stream))
                {
                    text = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                text = text.Replace("!LocalBackgroundImgFilename!", LocalBackgroundImgFilename);

                StringBuilder content = new StringBuilder();
                foreach (StringEx.Fragment fragment in _text)
                {
                    string fragmentSnippet = fragment.Text;

                    fragmentSnippet = fragmentSnippet.Replace("\n", "<br/><br/>");
                    if (fragment == _text.ActiveFragment)
                    {
                        fragmentSnippet = "<div id=\"active_fragment\">" + fragmentSnippet + "</div>";
                    }

                    content.Append(fragmentSnippet);
                }
                text = text.Replace("!Content!", content.ToString());

                using (StreamWriter streamWriter = new StreamWriter(LocalHtmlFilename))
                {
                    streamWriter.Write(text);
                    streamWriter.Close();
                }
            }

            this.Browser.Navigate(new Uri(LocalHtmlFilename, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// The text
        /// </summary>
        private StringEx _text = new StringEx();

        #region BackgroundImg Property

        /// <summary>
        /// BackgroundImg property definition
        /// </summary>
        public static readonly DependencyProperty BackgroundImgProperty =
            DependencyProperty.Register(
                "BackgroundImg",
                typeof(string),
                typeof(WebBrowserEx),
                new PropertyMetadata(string.Empty, OnBackgroundImgPropertyChanged));

        /// <summary>
        /// Gets or sets the Uri of the BackgroundImg
        /// </summary>
        public string BackgroundImg
        {
            get
            {
                return (string)GetValue(BackgroundImgProperty);
            }
            set
            {
                SetValue(BackgroundImgProperty, value);
            }
        }

        /// <summary>
        /// Handler for when the BackgroundImg has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void OnBackgroundImgPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            WebBrowserEx webBrowserEx = sender as WebBrowserEx;
            if (webBrowserEx != null)
            {
                webBrowserEx.RefreshLocalStorage();
            }
        }

        #endregion BackgroundImg Property

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

                webBrowserEx.RefreshLocalStorage();
            }
        }

        #endregion Text Property
    }
}

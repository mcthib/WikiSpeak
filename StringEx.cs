using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiSpeak
{
    /// <summary>
    /// A string class that keeps track of sentences / fragments
    /// </summary>
    public class StringEx : List<StringEx.Fragment>
    {
        /// <summary>
        /// This is a sub-string
        /// </summary>
        public class Fragment
        {
            /// <summary>
            /// C-tor
            /// </summary>
            /// <param name="text"></param>
            /// <param name="startIndex"></param>
            public Fragment(string text, int startIndex)
            {
                Text = text;
                StartIndex = startIndex;
            }

            /// <summary>
            /// Gets or sets the text in the fragment
            /// </summary>
            public string Text
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets or sets the start index of the fragment
            /// </summary>
            public int StartIndex
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the end index of the fragment
            /// </summary>
            public int EndIndex
            {
                get
                {
                    return StartIndex + Text.Length - 1;
                }
            }

            /// <summary>
            /// String cast operator
            /// </summary>
            /// <param name="s">object</param>
            /// <returns>the text</returns>
            public static implicit operator string(Fragment fragment)
            {
                return (fragment == null ? null : fragment.Text);
            }
        }

        /// <summary>
        /// String cast operator
        /// </summary>
        /// <param name="s">object</param>
        /// <returns>a string that concatenates all the fragments</returns>
        public static implicit operator string(StringEx s)
        {
            return (s == null ? null : s.ToString());
        }

        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            foreach (Fragment fragment in this)
            {
                s.Append(fragment.Text);
            }

            return s.ToString();
        }

        /// <summary>
        /// Gets an HTML representation of the text
        /// </summary>
        public string HTML
        {
            get
            {
                string html;

                // Read resource template
                using (StreamReader streamReader = new StreamReader(App.GetResourceStream(new Uri("Assets/WebBrowser.html", UriKind.Relative)).Stream))
                {
                    html = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                // Build content string
                StringBuilder content = new StringBuilder();
                for (int fragment_index = 0; fragment_index < this.Count; fragment_index++)
                {
                    string fragmentSnippet = this[fragment_index].Text;

                    fragmentSnippet = fragmentSnippet.Replace("\n", "<br/><br/>");
                    fragmentSnippet = string.Format(
                        "<div id=\"{0}\" name=\"{0}\">{1}</div>",
                        fragment_index,
                        fragmentSnippet,
                        CultureInfo.InvariantCulture);

                    content.Append(fragmentSnippet);
                }

                // Replace content
                html = html.Replace("!Content!", content.ToString());

                return html;
            }
        }

        /// <summary>
        /// Gets the currently active fragment
        /// </summary>
        public Fragment ActiveFragment
        {
            get
            {
                return ((_activeFragmentIndex >= 0) && (_activeFragmentIndex < this.Count) ? this[_activeFragmentIndex] : null);
            }
        }

        /// <summary>
        /// Gets the currently active fragment index
        /// </summary>
        public int ActiveFragmentIndex
        {
            get
            {
                return _activeFragmentIndex;
            }
        }
        private int _activeFragmentIndex = 0;

        /// <summary>
        /// Advances the fragment cursor to the next one.
        /// </summary>
        /// <returns>true if this isn't the last fragment</returns>
        public bool FastForward()
        {
            if (CanFastForward)
            {
                int currentIndex = _activeFragmentIndex;

                do
                {
                    if (_activeFragmentIndex < this.Count)
                    {
                        _activeFragmentIndex++;
                    }
                } while ((ActiveFragment != null) && string.IsNullOrWhiteSpace(ActiveFragment));

                if (currentIndex != _activeFragmentIndex)
                {
                    FireActiveFragmentChangedEvent();
                }
            }

            return CanFastForward;
        }
        
        /// <summary>
        /// Backtracks the fragment cursor to the previous one.
        /// </summary>
        /// <returns>true if this isn't the first fragment</returns>
        public bool Rewind()
        {
            if (CanRewind)
            {
                int currentIndex = _activeFragmentIndex;

                do
                {
                    if (_activeFragmentIndex >= 0)
                    {
                        _activeFragmentIndex--;
                    }
                } while ((ActiveFragment != null) && string.IsNullOrWhiteSpace(ActiveFragment));

                if (currentIndex != _activeFragmentIndex)
                {
                    FireActiveFragmentChangedEvent();
                }
            }

            return CanRewind;
        }

        /// <summary>
        /// Gets whether there is anything to play currently
        /// </summary>
        public bool CanPlay
        {
            get
            {
                return ((this.Count > 0) && (_activeFragmentIndex < this.Count));
            }
        }

        /// <summary>
        /// Gets whether we can fast-forward
        /// </summary>
        public bool CanFastForward
        {
            get
            {
                return (_activeFragmentIndex < this.Count - 1);
            }
        }

        /// <summary>
        /// Gets whether we can rewind
        /// </summary>
        public bool CanRewind
        {
            get
            {
                return (_activeFragmentIndex > 0);
            }
        }

        /// <summary>
        /// Sets the active fragment to the first one
        /// </summary>
        public void ResetActiveFragment()
        {
            if (_activeFragmentIndex != 0)
            {
                _activeFragmentIndex = 0;
                FireActiveFragmentChangedEvent();
            }
        }

        /// <summary>
        /// Clears the string
        /// </summary>
        public new void Clear()
        {
            ResetActiveFragment();
            base.Clear();
        }

        /// <summary>
        /// Adds a string of text and automatically works out the fragments
        /// </summary>
        /// <param name="text">an arbitrary piece of text</param>
        public void Append(string text)
        {
            int startIndexFragment = this.ToString().Length;
            int startBlock = 0, cursor;
            for (cursor = 0; cursor < text.Length; cursor++)
            {
                switch (text[cursor])
                {
                    case '\n':
                    case '\r':
                    case '.':
                        // End of a fragment
                        this.Add(new Fragment(text.Substring(startBlock, cursor - startBlock + 1), startIndexFragment));
                        startIndexFragment += cursor - startBlock + 1
                            ;
                        startBlock = cursor + 1;
                        break;

                    default:
                        // no action
                        break;
                }
            }

            if (cursor > startBlock + 1)
            {
                this.Add(new Fragment(text.Substring(startBlock, cursor - startBlock), startIndexFragment));
            }
        }

        /// <summary>
        /// Event fired when the active event has changed
        /// </summary>
        public event EventHandler<FragmentEventArgs> ActiveFragmentChanged;

        /// <summary>
        /// Fires the ActiveFragmentChanged event
        /// </summary>
        public void FireActiveFragmentChangedEvent()
        {
            EventHandler<FragmentEventArgs> activeFragmentChanged = ActiveFragmentChanged;
            if (activeFragmentChanged != null)
            {
                activeFragmentChanged(this, new FragmentEventArgs(ActiveFragment));
            }
        }
    }
}

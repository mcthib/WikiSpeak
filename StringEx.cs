using System;
using System.Collections.Generic;
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
                    return StartIndex + Text.Length;
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
        /// Gets the currently active fragment
        /// </summary>
        private Fragment ActiveFragment
        {
            get
            {
                return ((_activeFragmentIndex >= 0) && (_activeFragmentIndex < this.Count) ? this[_activeFragmentIndex] : null);
            }
        }

        /// <summary>
        /// Gets the active fragment's index in the list
        /// </summary>
        private int _activeFragmentIndex = -1;

        /// <summary>
        /// Advances the fragment cursor to the next one.
        /// </summary>
        /// <returns>the currently active fragment, or null if no more</returns>
        public Fragment NextFragment()
        {
            do
            {
                if (_activeFragmentIndex < this.Count)
                {
                    _activeFragmentIndex++;
                }
            } while ((ActiveFragment != null) && string.IsNullOrWhiteSpace(ActiveFragment));

            return ActiveFragment;
        }
        
        /// <summary>
        /// Backtracks the fragment cursor to the previous one.
        /// </summary>
        /// <returns>the currently active fragment, or null if no more</returns>
        public Fragment PreviousFragment()
        {
            do
            {
                if (_activeFragmentIndex >= 0)
                {
                    _activeFragmentIndex--;
                }
            } while ((ActiveFragment != null) && string.IsNullOrWhiteSpace(ActiveFragment));

            return ActiveFragment;
        }

        /// <summary>
        /// Sets the active fragment to the first one
        /// </summary>
        public void ResetActiveFragment()
        {
            _activeFragmentIndex = 0;
        }
    }
}

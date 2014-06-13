﻿using System;
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
    }
}
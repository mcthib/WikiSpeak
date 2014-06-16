using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiSpeak
{
    /// <summary>
    /// Event arguments when the object is a fragment
    /// </summary>
    public class FragmentEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the fragment for the event
        /// </summary>
        public StringEx.Fragment Fragment
        {
            get;
            private set;
        }

        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="fragment">the fragment the event belongs to</param>
        public FragmentEventArgs(StringEx.Fragment fragment)
        {
            Fragment = fragment;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiSpeak.ViewModels
{
    /// <summary>
    /// This class abstracts the view model for the current article
    /// </summary>
    public class CurrentArticleViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The fragments associated with the article's contents
        /// </summary>
        private StringEx _fragments = new StringEx();

        public Article Article
        {
            get
            {
                return _article;
            }
            set
            {
                Article old = _article;
                if (old != value)
                {
                    _article = value;
                    NotifyPropertyChanged("Article");

                    RefreshArticleText();
                    NotifyPropertyChanged("HTML");
                    NotifyPropertyChanged("SML");
                    NotifyPropertyChanged("CanFastForward");
                    NotifyPropertyChanged("CanRewind");
                    NotifyPropertyChanged("CanPlay");
                }
            }
        }
        private Article _article = null;

        /// <summary>
        /// Fast forwards to the next article fragment
        /// </summary>
        public void FastForward()
        {
            bool canFastForward = _fragments.CanFastForward;
            if (_fragments.FastForward() != canFastForward)
            {
                NotifyPropertyChanged("CanFastForward");
            }
        }

        /// <summary>
        /// Rewinds to the previous article fragment
        /// </summary>
        public void Rewind()
        {
            bool canRewind = _fragments.CanRewind;
            if (_fragments.Rewind() != canRewind)
            {
                NotifyPropertyChanged("CanRewind");
            }
        }

        /// <summary>
        /// Gets whether there is anything to play currently
        /// </summary>
        public bool CanPlay
        {
            get
            {
                return _fragments.CanPlay;
            }
        }

        /// <summary>
        /// Gets whether we can fast-forward
        /// </summary>
        public bool CanFastForward
        {
            get
            {
                return _fragments.CanFastForward;
            }
        }

        /// <summary>
        /// Gets the active fragment index
        /// </summary>
        public int ActiveFragmentIndex
        {
            get
            {
                return _fragments.ActiveFragmentIndex;
            }
        }

        /// <summary>
        /// Gets whether we can rewind
        /// </summary>
        public bool CanRewind
        {
            get
            {
                return _fragments.CanRewind;
            }
        }


        /// <summary>
        /// Gets the HTML representation of the current article
        /// </summary>
        public string HTML
        {
            get
            {
                return _fragments.HTML;
            }
        }

        /// <summary>
        /// Refreshes the text of the article in the fragments list
        /// </summary>
        private void RefreshArticleText()
        {
            StringEx newText = new StringEx();

            if (Article != null)
            {
                newText.Append(Article.TextualContents);
            }

            _fragments = newText;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

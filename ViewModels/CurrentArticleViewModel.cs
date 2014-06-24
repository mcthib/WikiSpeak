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
        /// C-tor
        /// </summary>
        public CurrentArticleViewModel()
        {
            _fragments.ActiveFragmentChanged += _fragments_ActiveFragmentChanged;
        }

        /// <summary>
        /// Handler for when the active fragment index has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _fragments_ActiveFragmentChanged(object sender, FragmentEventArgs e)
        {
            NotifyPropertyChanged("ActiveFragmentIndex");
        }

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
                }
            }
        }
        private Article _article = null;

        /// <summary>
        /// Gets the title of the current article
        /// </summary>
        public string Title
        {
            get
            {
                return (_article == null ? string.Empty : _article.Title);
            }
        }

        /// <summary>
        /// Fast forwards to the next article fragment
        /// </summary>
        public void FastForward()
        {
            bool canFastForward = _fragments.CanFastForward;
            bool canRewind = _fragments.CanRewind;
            
            if (_fragments.FastForward() != canFastForward)
            {
                NotifyPropertyChanged("CanFastForward");
            }
            
            if (_fragments.CanRewind != canRewind)
            {
                NotifyPropertyChanged("CanRewind");
            }
        }

        /// <summary>
        /// Rewinds to the previous article fragment
        /// </summary>
        public void Rewind()
        {
            bool canRewind = _fragments.CanRewind;
            bool canFastForward = _fragments.CanFastForward;

            if (_fragments.Rewind() != canRewind)
            {
                NotifyPropertyChanged("CanRewind");
            }

            if (_fragments.CanFastForward != canFastForward)
            {
                NotifyPropertyChanged("CanFastForward");
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

            if (_fragments != null)
            {
                _fragments.ActiveFragmentChanged -= _fragments_ActiveFragmentChanged;
            }

            newText.ActiveFragmentChanged += _fragments_ActiveFragmentChanged;
            _fragments = newText;

            NotifyPropertyChanged("HTML");
            NotifyPropertyChanged("SML");
            NotifyPropertyChanged("CanFastForward");
            NotifyPropertyChanged("CanRewind");
            NotifyPropertyChanged("CanPlay");
            NotifyPropertyChanged("ActiveFragmentIndex");
            NotifyPropertyChanged("Title");
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

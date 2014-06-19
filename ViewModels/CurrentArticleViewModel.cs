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
                }
            }
        }
        private Article _article = null;

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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
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
    }
}

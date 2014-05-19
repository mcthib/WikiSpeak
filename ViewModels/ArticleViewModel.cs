using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WikiSpeak.ViewModels
{
	public class ArticleViewModel : INotifyPropertyChanged
	{


		/// <summary>
		/// Uhnderlying article object
		/// </summary>
		public Article Article
		{
			get
			{
				return _article;
			}
			set
			{
				Article previousArticle = _article;
				Article newArticle = value;

				if (previousArticle != newArticle)
				{

					if (previousArticle != null)
					{
						previousArticle.ContentChanged -= Article_ContentChanged;
					}

					_article = newArticle;

					if (newArticle != null)
					{
						newArticle.ContentChanged += Article_ContentChanged;
					}

					NotifyPropertyChanged("Title");
					NotifyPropertyChanged("Excerpt");
                    NotifyPropertyChanged("TextualContents");
				}
			}
		}
		private Article _article;

		/// <summary>
		/// Handler for when the content of the article has changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Article_ContentChanged(object sender, ArticleContentChangedEventArgs e)
		{
			NotifyPropertyChanged("Title");
			NotifyPropertyChanged("Excerpt");
            NotifyPropertyChanged("TextualContents");
		}

		/// <summary>
		/// Gets the title of the article
		/// </summary>
		public string Title
		{
			get
			{
				return (_article != null ? _article.Title : string.Empty);
			}
		}

        /// <summary>
        /// Gets the textual contents of the article
        /// </summary>
        public string TextualContents
        {
            get
            {
                return (_article != null ? _article.TextualContents : string.Empty);
            }
        }

		/// <summary>
		/// Gets the excerpt of the article
		/// </summary>
		public string Excerpt
		{
			get
			{
				return (_article != null ? _article.Excerpt : string.Empty);
			}
		}

        /// <summary>
        /// GEts the RTB representation of the article text
        /// </summary>
        public string RichTextBoxRepresentation
        {
            get
            {
                return "<paragraph>this is <bold>text</bold><paragraph>";
            }
        }

		/// <summary>
		/// Event for property changed notification
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Helper method for property change notification
		/// </summary>
		/// <param name="propertyName"></param>
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
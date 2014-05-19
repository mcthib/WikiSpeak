using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WikiSpeak.Resources;

namespace WikiSpeak.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{
		public MainViewModel()
		{
		}

		/// <summary>
		/// The collection of articles
		/// </summary>
		public ObservableCollection<ArticleViewModel> Articles
        {
            get
            {
                return _articles;
            }
            set
            {
                ObservableCollection<ArticleViewModel> currentValue = _articles;
                if (value != currentValue)
                {
                    _articles = value;
                    NotifyPropertyChanged("Articles");
                }
            }
        }
        private ObservableCollection<ArticleViewModel> _articles = new ObservableCollection<ArticleViewModel>();

        /// <summary>
		/// Currently playing article title
		/// </summary>
        public string CurrentArticleTitle
		{
			get
			{
                return (CurrentArticle == null ? string.Empty : CurrentArticle.Title);
			}
		}

        /// <summary>
        /// Currently playing article text for the reader view
        /// </summary>
        public string CurrentArticleRichTextBoxRepresentation
        {
            get
            {
                return (CurrentArticle == null ? string.Empty : CurrentArticle.RichTextBoxRepresentation);
            }
        }

        /// <summary>
        /// Currently playing article
        /// </summary>
        public ArticleViewModel CurrentArticle
        {
            get
            {
                return _currentArticle;
            }
            set
            {
                ArticleViewModel currentValue = _currentArticle;
                if (value != currentValue)
                {
                    _currentArticle = value;
                    NotifyPropertyChanged("CurrentArticle");
                    NotifyPropertyChanged("CurrentArticleTitle");
                    NotifyPropertyChanged("CurrentArticleRichTextBoxRepresentation");
                }
            }
        }
        private ArticleViewModel _currentArticle = null;

        /// <summary>
        /// Gets the visibility of the articles list
        /// </summary>
        public Visibility ArticleListVisibility
        {
            get
            {
                return (IsArticleViewVisible ? Visibility.Collapsed : Visibility.Visible);
            }
        }

        /// <summary>
        /// Gets the visibility of the article content view
        /// </summary>
        public Visibility ArticleViewVisibility
        {
            get
            {
                return (IsArticleViewVisible ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        /// <summary>
        /// Gets the icon URI for the App Bar article toggle button
        /// </summary>
        public Uri ArticleListViewToggleIconUri
        {
            get
            {
                return new Uri(IsArticleViewVisible ? "/Assets/Icons/Dark/manage.png" : "/Assets/Icons/Dark/edittext.png", UriKind.RelativeOrAbsolute);
            }
        }

        /// <summary>
        /// Gets or sets whether the article view is visible (toggle between list vs view)
        /// </summary>
        public bool IsArticleViewVisible
        {
            get
            {
                return _isArticleViewVisible;
            }
            set
            {
                bool oldValue = _isArticleViewVisible;
                if (oldValue != value)
                {
                    _isArticleViewVisible = value;
                    NotifyPropertyChanged("ArticleListVisibility");
                    NotifyPropertyChanged("ArticleViewVisibility");

                    // What is going on here?? Whelp, App Bars aren't real Silverlight controls. So, we need to actually do this BS here...
                    // Doesn't work: NotifyPropertyChanged("ArticleListViewToggleIconUri");
                }
            }
        }
        private bool _isArticleViewVisible = false;

        /// <summary>
		/// Sample property that returns a localized string
		/// </summary>
		public string LocalizedSampleProperty
		{
			get
			{
				return AppResources.SampleProperty;
			}
		}

		public bool IsDataLoaded
		{
			get;
			private set;
		}

		/// <summary>
		/// Creates and adds a few ItemViewModel objects into the Items collection.
		/// </summary>
		public void LoadData()
		{
			this.Articles.Add(new ArticleViewModel() { Article = new Article("Arti", "en") { Title = "first", Excerpt = "Excerpt of article 1" } });
            this.Articles.Add(new ArticleViewModel() { Article = new Article("Arti2", "en") { Title = "second", Excerpt = "Excerpt of article 2" } });
            this.Articles.Add(new ArticleViewModel() { Article = new Article("Arti", "en") { Title = "third", Excerpt = "Excerpt of article 1" } });
            this.Articles.Add(new ArticleViewModel() { Article = new Article("Arti2", "en") { Title = "fourth", Excerpt = "Excerpt of article 2" } });
            this.Articles.Add(new ArticleViewModel() { Article = new Article("Arti", "en") { Title = "fifth", Excerpt = "Excerpt of article 1" } });
            this.Articles.Add(new ArticleViewModel() { Article = new Article("Arti2", "en") { Title = "seventh", Excerpt = "Excerpt of article 2" } });
            this.Articles.Add(new ArticleViewModel() { Article = new Article("Arti", "en") { Title = "eight", Excerpt = "Excerpt of article 1" } });
            this.Articles.Add(new ArticleViewModel() { Article = new Article("Arti2", "en") { Title = "ninth", Excerpt = "Excerpt of article 2" } });

			this.IsDataLoaded = true;
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
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                return _currentArticleTitle;
			}
			set
			{
                string currentValue = _currentArticleTitle;
				if (value != currentValue)
				{
                    _currentArticleTitle = value;
                    NotifyPropertyChanged("CurrentArticleTitle");
				}
			}
		}
        private string _currentArticleTitle = string.Empty;

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

		/// <summary>
		/// Sample property that returns a localized string
		/// </summary>
		public Uri ArticleViewToggleButtonIconUri
		{
			get
			{
                return new Uri("/Assets/Icons/Dark/edittext.png", UriKind.RelativeOrAbsolute); // Text="article text"/>
                //<shell:ApplicationBarIconButton IconUri="/Assets/Icons/Dark/manage.png" Text="article list"/>
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
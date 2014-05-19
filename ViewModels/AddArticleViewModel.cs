using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WikiSpeak.Resources;

namespace WikiSpeak.ViewModels
{
	public class AddArticleViewModel : INotifyPropertyChanged
	{
        public AddArticleViewModel()
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
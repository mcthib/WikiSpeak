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
				if (_article != value)
				{
					_article = value;
					NotifyPropertyChanged("Title");
					NotifyPropertyChanged("Excerpt");
				}
			}
		}
		private Article _article;

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
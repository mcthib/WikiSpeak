﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WikiSpeak
{
	/// <summary>
	/// This class contains all the info relative to an article
	/// </summary>
	public class Article
	{
		/// <summary>
		/// The term that was used for the search
		/// </summary>
		public string SearchTerm;

		/// <summary>
		/// The locale is used for the search & for the voice
		/// </summary>
		public string Locale;

		/// <summary>
		/// The URL of the article we chose based on the search term
		/// </summary>
		public string Url;

		/// <summary>
		/// The title of the article in WikiPedia
		/// </summary>
		public string Title;

		/// <summary>
		/// An excerpt
		/// </summary>
		public string Excerpt;

		/// <summary>
		/// The contents, sanitized of Wikipedia formatting
		/// </summary>
		public string TextualContents;

		/// <summary>
		/// The status of the article
		/// </summary>
		public ArticleStatus Status;

		/// <summary>
		/// Possible statuses of an article
		/// </summary>
		public enum ArticleStatus
		{
			/// <summary>
			/// User entered terms
			/// </summary>
			Initial,

			/// <summary>
			/// Currently searching
			/// </summary>
			Searching,

			/// <summary>
			/// Couldn't find an article
			/// </summary>
			NotFound,

			/// <summary>
			/// Found article
			/// </summary>
			Found,

			/// <summary>
			/// Downloading article
			/// </summary>
			Downloading,

			/// <summary>
			/// Downloaded article
			/// </summary>
			Downloaded,
		}

		/// <summary>
		/// Event triggered when the contents of the article have changed
		/// </summary>
		public event EventHandler<ArticleContentChangedEventArgs> ContentChanged;

		/// <summary>
		/// Fires the event that signals that the contents have changed
		/// </summary>
		public void FireContentChangedEvent()
		{
			EventHandler<ArticleContentChangedEventArgs> contentChanged = ContentChanged;
			if (contentChanged != null)
			{
				contentChanged(this, new ArticleContentChangedEventArgs(this));
			}
		}

		/// <summary>
		/// C-tor
		/// </summary>
		/// <param name="searchTerm">search term(s) entered by user</param>
		/// <param name="locale">locale to use</param>
		public Article(string searchTerm, string locale)
		{
			SearchTerm = searchTerm;
			Locale = locale;
			Status = ArticleStatus.Initial;
		}

        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="articleTitleAndUrl">a previously found WikiPedia entry description</param>
        public Article(WikipediaHelper.ArticleTitleAndUrl articleTitleAndUrl)
        {
            Title = articleTitleAndUrl.Title;
            Url = articleTitleAndUrl.Url;
            Excerpt = articleTitleAndUrl.Description;
            Locale = articleTitleAndUrl.Locale;
            Status = ArticleStatus.Found;
        }

		/// <summary>
		/// Asynchronously searches for a matching article
		/// </summary>
		/// <returns></returns>
        public async Task SearchAsync()
        {
            if (this.Status == ArticleStatus.Initial)
            {
                this.Status = ArticleStatus.Searching;
                WikipediaHelper.ArticleTitleAndUrl[] articles = await WikipediaHelper.SearchForArticlesAsync(SearchTerm, Locale, 1);

                if (articles.Length > 0)
                {
                    this.Title = articles[0].Title;
                    this.Url = articles[0].Url;
                    this.Status = ArticleStatus.Found;

                    await DownloadAsync();
                }
                else
                {
                    this.Status = ArticleStatus.NotFound;
                }
            }
        }

        /// <summary>
        /// Actually downloads the article
        /// </summary>
        /// <returns></returns>
        public async Task DownloadAsync()
        {
            if (this.Status == ArticleStatus.Found)
            {
                this.Status = ArticleStatus.Downloading;
                this.TextualContents = await WikipediaHelper.DownloadWikipediaArticleAsync(new WikipediaHelper.ArticleTitleAndUrl() { Url = this.Url });
                this.Status = ArticleStatus.Downloaded;
                FireContentChangedEvent();
            }
        }
	}
}

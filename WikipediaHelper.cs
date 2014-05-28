using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace WikiSpeak
{
	/// <summary>
	/// This class is used to help with downloading and processing Wikipedia articles
	/// </summary>
	public abstract class WikipediaHelper
	{
		/// <summary>
		/// An article's title and its url
		/// </summary>
		public struct ArticleTitleAndUrl
		{
			/// <summary>
			/// The title of the article
			/// </summary>
			public string Title;

			/// <summary>
			/// The url where the article content is located
			/// </summary>
			public string Url;

            /// <summary>
            /// A short description of the article
            /// </summary>
            public string Description;

            /// <summary>
            /// The article locale
            /// </summary>
            public string Locale;
		}

		/// <summary>
		/// Asynchronously search for articles based on a search string
		/// </summary>
		/// <param name="searchString">a search string, e.g. "France" or "sac"</param>
		/// <param name="locale">the locale to search in</param>
		/// <param name="max_articles">the maximum number of articles to return</param>
		/// <returns>a list of terms and their URL</returns>
		public static async Task<ArticleTitleAndUrl[]> SearchForArticlesAsync(string searchString, string locale, int max_articles)
		{
			List<ArticleTitleAndUrl> articles = new List<ArticleTitleAndUrl>();
			HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument(); 

			HttpWebRequest request = HttpWebRequest.CreateHttp(
				string.Format(
					"http://{0}.wikipedia.org/w/api.php?action=opensearch&search={1}&limit={2}&format=xml",
					locale,
					Uri.EscapeUriString(searchString),
					max_articles));
			await request.GetResponseAsync().ContinueWith(
				t =>
				{
					try
					{
						using (Stream stream = t.Result.GetResponseStream())
						{
							doc.Load(stream);
						}
					}
					catch (Exception ex)
					{
						//
					}
				});

			// This XPath selection must be case insensitive...
			var items = doc.DocumentNode.SelectNodes("//searchsuggestion/section/item");
			if (items != null)
			{
				foreach (HtmlAgilityPack.HtmlNode node in items)
				{
					// These case-insensitive comparisons are the result of using an HTML parser to process XML...
					HtmlAgilityPack.HtmlNode titleNode = node.Descendants().Single(x => { return string.Equals(x.Name, "text", StringComparison.OrdinalIgnoreCase); });
					HtmlAgilityPack.HtmlNode urlNode = node.Descendants().Single(x => { return string.Equals(x.Name, "url", StringComparison.OrdinalIgnoreCase); });
                    HtmlAgilityPack.HtmlNode descriptionNode = node.Descendants().Single(x => { return string.Equals(x.Name, "description", StringComparison.OrdinalIgnoreCase); });

					string title = titleNode == null ? string.Empty : titleNode.InnerText;
					string url = urlNode == null ? string.Empty : urlNode.InnerText;
                    string description = descriptionNode == null ? string.Empty : descriptionNode.InnerText;

					if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(url))
					{
						articles.Add(
							new ArticleTitleAndUrl()
							{
								Title = title,
								Url = url,
                                Description = description,
                                Locale = locale
							});
					}
				}
			}

			return articles.ToArray();
		}

		/// <summary>
		/// Downloads and processes a Wikipedia article in such a way that it is human-readable
		/// </summary>
		/// <param name="article">the previously searched article</param>
		/// <returns>a human-readable string for the article</returns>
		public static async Task<string> DownloadWikipediaArticleAsync(ArticleTitleAndUrl article)
		{
			StringBuilder result = new StringBuilder();
			HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

			HttpWebRequest request = HttpWebRequest.CreateHttp(article.Url);
			await request.GetResponseAsync().ContinueWith(
				t =>
				{
					try
					{
						using (Stream stream = t.Result.GetResponseStream())
						{
							doc.Load(stream);
						}
					}
					catch (Exception ex)
					{
						//
					}
				});

			foreach (HtmlAgilityPack.HtmlNode node in doc.DocumentNode.SelectNodes("//*[@id=\"mw-content-text\"]/p | //*[@id=\"mw-content-text\"]/h2"))
			{
				string nodeText = HtmlAgilityPack.HtmlEntity.DeEntitize(
				    Regex.Replace(
					node.InnerText,
					"\\[[^\\]]+\\]",
					string.Empty));

                if (!string.IsNullOrWhiteSpace(nodeText))
                {
                    while (nodeText.IndexOf("\n\n") >= 0)
                    {
                        nodeText = nodeText.Replace("\n\n", "\n");
                    }
                    result.AppendLine(nodeText);
                }
			}

            result.Replace("\r", string.Empty);

			return result.ToString();
		}
	}
}

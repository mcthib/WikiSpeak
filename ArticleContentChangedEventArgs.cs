using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiSpeak
{
	/// <summary>
	/// Argument to an event fired when the contents of an article has changed
	/// </summary>
	public class ArticleContentChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Article in question
		/// </summary>
		public Article Article
		{
			get;
			private set;
		}

		/// <summary>
		/// C-tor
		/// </summary>
		/// <param name="article">article which contents have changed</param>
		public ArticleContentChangedEventArgs(Article article)
		{
			Article = article;
		}
	}
}

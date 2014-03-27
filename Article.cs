using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		/// The title of the article in WikiPedia
		/// </summary>
		public string Title;

		/// <summary>
		/// An excerpt
		/// </summary>
		public string Excerpt;

	}
}

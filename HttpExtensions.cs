using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WikiSpeak
{
	/// <summary>
	/// This class is used to abstract HttpWebRequest behind asnyc tasks.
	/// It was copied from Andy Wigley's blog: http://blogs.msdn.com/b/andy_wigley/archive/2013/02/07/async-and-await-for-http-networking-on-windows-phone.aspx
	/// </summary>
	public static class HttpExtensions
	{
		public static Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest request)
		{
			var taskComplete = new TaskCompletionSource<HttpWebResponse>();
			request.BeginGetResponse(asyncResponse =>
			{
				try
				{
					HttpWebRequest responseRequest = (HttpWebRequest)asyncResponse.AsyncState;
					HttpWebResponse someResponse =
					   (HttpWebResponse)responseRequest.EndGetResponse(asyncResponse);
					taskComplete.TrySetResult(someResponse);
				}
				catch (WebException webExc)
				{
					HttpWebResponse failedResponse = (HttpWebResponse)webExc.Response;
					taskComplete.TrySetResult(failedResponse);
				}
			}, request);
			return taskComplete.Task;
		}
	}
}

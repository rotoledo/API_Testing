using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RM.SourceData
{
	public class HttpClientSourceData
	{

		private readonly HttpClient _client;

		public HttpClientSourceData()
		{
			_client = new HttpClient();
		}

		public async Task<HttpResponseMessage> PostXFormAsync(string uri, Dictionary<string, string> form)
		{
			var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
			requestMessage.Content = new FormUrlEncodedContent(form);

			var response = await _client.SendAsync(requestMessage);

			return response;
		}

	}
}

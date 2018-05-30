using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Moq;
using Xunit;

namespace API_Testing.SOAP___Calculator_API
{
	public class CalculatorAPIScenarios : CalculatorAPIScenarioBase
	{
		HttpClient httpClient;
		XmlDocument xmlDocument;

		public CalculatorAPIScenarios()
		{
			httpClient = new HttpClient();
			xmlDocument = new XmlDocument();
		}

		[Theory]
		[InlineData(78, 94)]
		public async Task Add_Two_Numbers_successfully(int intA, int intB)
		{
			// Given
			string envelope = EnvelopeOperationBuilder("Add", intA, intB);
			var contetnt = new StringContent(envelope, Encoding.UTF8, "text/xml");

			// When
			var response = await httpClient.PostAsync(Post.PostEnvelope(), contetnt);
			var response_content = response.Content.ReadAsStringAsync().Result;

			// Then
			response.EnsureSuccessStatusCode();
			xmlDocument.LoadXml(response_content);
			string addResult = xmlDocument.GetElementsByTagName("AddResult").Item(0).FirstChild.Value;
			Assert.Equal((intA + intB).ToString(), addResult);

			var myXMLStringFromDB = 
				$@"<MedicalClearanceFormRoot><MedicalClearanceForm PassengerName='AAAAAAAAAAAAA' Age='11' PhoneNo='TTTTTTTTTTT' Email='ZZZZZZZZZZZZZZZZZZZ' BookingRefNo='11111111111111111111'/></MedicalClearanceFormRoot>";

			XmlSerializer serializer = new XmlSerializer(typeof(Wrapper));
			using (TextReader reader = new StringReader(myXMLStringFromDB))
			{
				Wrapper objModel = (Wrapper)serializer.Deserialize(reader);
			}
		}

		[Theory]
		[InlineData(134, 58)]
		public async void DivideTwoNumbers(int intA, int intB)
		{
			// Given
			var envelope = EnvelopeOperationBuilder("Divide", intA, intB);
			var content = new StringContent(envelope, Encoding.UTF8, "text/xml");

			// When
			var response = await httpClient.PostAsync(Post.PostEnvelope(), content);
			var response_content = response.Content.ReadAsStringAsync().Result;

			// Then
			response.EnsureSuccessStatusCode();
			xmlDocument.LoadXml(response_content);
			Assert.Equal("soap:Envelope", xmlDocument.DocumentElement.Name.ToString());
			Assert.Equal((intA / intB).ToString(), xmlDocument.InnerText);
		}


		[Theory]
		[InlineData(173, 74)]
		public async Task Request_Subtract_with_UNSUPPORTED_MEDIA_TYPE(int intA, int intB)
		{
			// Given
			string envelope = EnvelopeOperationBuilder("Subtract", intA, intB);
			var contetnt = new StringContent(envelope, Encoding.UTF8, "text/json");

			// When
			var response = await httpClient.PostAsync(Post.PostEnvelope(), contetnt);
			var response_content = response.Content.ReadAsStringAsync().Result;

			// Then
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
		}


		[Fact]
		public async Task Test_mock()
		{
			// Given
			Mock<IHttpHandler> _mock_HttpClient = new Mock<IHttpHandler>();

			// Wire Mock!!

			string envelope = EnvelopeOperationBuilder("Add", 5, 5);
			var contetnt = new StringContent(envelope, Encoding.UTF8, "text/json");

			var statusCode = HttpStatusCode.NotFound;

			_mock_HttpClient.Setup(m => m.PostAsync(Post.PostEnvelope(), contetnt))
				.ReturnsAsync(new HttpResponseMessage(statusCode));

			// When
			var response = await _mock_HttpClient.Object.PostAsync(Post.PostEnvelope(), contetnt);
			//var response_content = response.Content.ReadAsStringAsync().Result;

			// Then
			//Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(statusCode, response.StatusCode);
		}
	}

}




//public class IHttpClient : IHttpHandler
//{
//	private HttpClient _client = new HttpClient();

//	public HttpResponseMessage Get(string url)
//	{
//		return GetAsync(url).Result;
//	}

//	public HttpResponseMessage Post(string url, HttpContent content)
//	{
//		return PostAsync(url, content).Result;
//	}

//	public async Task<HttpResponseMessage> GetAsync(string url)
//	{
//		return await _client.GetAsync(url);
//	}

//	public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
//	{
//		return await _client.PostAsync(url, content);
//	}
//}

public interface IHttpHandler
{
	HttpResponseMessage Get(string url);
	HttpResponseMessage Post(string url, HttpContent content);
	Task<HttpResponseMessage> GetAsync(string url);
	Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
}

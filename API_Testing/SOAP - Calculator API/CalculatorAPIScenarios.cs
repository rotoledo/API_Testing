using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace API_Testing.SOAP___Calculator_API
{
	public class CalculatorAPIScenarios : CalculatorAPIScenarioBase
	{
		HttpClient httpClient = new HttpClient();
		XmlDocument xmlDocument = new XmlDocument();

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
			//response.Headers.
			xmlDocument.LoadXml(response_content);
			Assert.Equal("soap:Envelope", xmlDocument.DocumentElement.Name.ToString());
			Assert.Equal((intA / intB).ToString(), xmlDocument.InnerText);
		}


		[Theory]
		[InlineData(173, 74)]
		public async Task Request_Subtract_with_unsupported_media_type(int intA, int intB)
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
	}

}

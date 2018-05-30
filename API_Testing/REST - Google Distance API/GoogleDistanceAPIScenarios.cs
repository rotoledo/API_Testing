using Xunit;
using System;
using System.Text;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using API_Testing.Google_Distance_API;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Newtonsoft.Json.Schema;

namespace API_Testing
{
	public class GoogleDistanceAPIScenarios : GoogleDistanceAPIScenarioBase
	{
		HttpClient httpClient = new HttpClient();

		[Fact]
		public async Task Get_distance_between_two_cities()
		{
			// Given
			GoogleDistanceAPISearchFilter searchFilter = new GoogleDistanceAPISearchFilter("Porto Alegre", "São Paulo", "metric");

			// When
			var response = await httpClient.GetAsync(Get.GetDistance(searchFilter));

			// Then
			response.EnsureSuccessStatusCode();
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			string response_content = response.Content.ReadAsStringAsync().Result;
			Assert.Contains(searchFilter.Origins, response_content);
			Assert.Contains(searchFilter.Destinations, response_content);

			var json_object = JObject.Parse(response_content);
			Assert.Contains(searchFilter.Destinations, json_object.GetValue("destination_addresses").ToString());
			Assert.Equal("OK", json_object.GetValue("status").ToString());
			Assert.Contains(searchFilter.Origins, json_object.GetValue("origin_addresses").ToString());
			//Assert.Equal("1,129 km", json_object.SelectToken("rows[*].elements[*].distance.text").ToString());

			dynamic jsonResponse = JsonConvert.DeserializeObject(response_content);
			//jsonResponse
			JSchema schema = JSchema.Parse(schemaJson);
			json_object.IsValid(schema);
			
		}

		string schemaJson = @"{
			'description' : 'Distance between two cities',
			'type' : 'object',
			'destination_addresses' : {'type' : 'string'},
			'origin_addresses': {'type' : 'integer'},
			'status' : {'type' : 'string'},		
		}";


		[Fact]
		public async Task Get_distance_Not_Found()
		{
			// Given
			GoogleDistanceAPISearchFilter searchFilter = new GoogleDistanceAPISearchFilter("xxxxxxxxxx", "São Paulo", "metric");
			string json_string = JsonConvert.SerializeObject(searchFilter);
			var content = new StringContent(json_string, Encoding.UTF8, "application/json");

			// When
			var response = await httpClient.GetAsync(Get.GetDistance(searchFilter));

			// Then
			response.EnsureSuccessStatusCode();
			string response_content = response.Content.ReadAsStringAsync().Result;
			Assert.Contains("NOT_FOUND", response_content);
		}

		[Fact]
		public async Task Get_distance_INVALID_REQUEST()
		{
			// Given
			GoogleDistanceAPISearchFilter searchFilter = new GoogleDistanceAPISearchFilter("Porto Alegre", "São Paulo", "metric");
			string json_string = JsonConvert.SerializeObject(searchFilter);
			var content = new StringContent(json_string, Encoding.UTF8, "application/json");

			// When
			var response = await httpClient.PostAsync(Post.PostDistance(), content);

			// Then
			response.EnsureSuccessStatusCode();
			string response_content = response.Content.ReadAsStringAsync().Result;
			Assert.Contains("INVALID_REQUEST", response_content);
		}

		[Theory]
		[InlineData(300)]
		[InlineData(500)]
		[InlineData(800)]
		[InlineData(1000)]
		public async Task Get_distance_with_timeout_for_RESPONSE_TIME(int timeout)
		{
			// Given
			GoogleDistanceAPISearchFilter searchFilter = new GoogleDistanceAPISearchFilter("Porto Alegre", "São Paulo", "metric");
			HttpResponseMessage response = new HttpResponseMessage();
			httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);
			var stopwatch = new Stopwatch();

			// When
			try
			{
				stopwatch.Start();
				response = await httpClient.GetAsync(Get.GetDistance(searchFilter));
				stopwatch.Stop();
			}
			catch (TaskCanceledException)
			{
				throw new Exception($"Tempo de resposta da Request foi de {stopwatch.ElapsedMilliseconds} milisegundos, excedendo o Timeout de {timeout} milisegundos");
			}

			// Then
			response.EnsureSuccessStatusCode();
		}

	}

}

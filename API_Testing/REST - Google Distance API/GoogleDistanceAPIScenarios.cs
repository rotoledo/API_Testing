using Xunit;
using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using API_Testing.Google_Distance_API;
using System.Net;

namespace API_Testing
{
	public class GoogleDistanceAPIScenarios : GoogleDistanceAPIScenarioBase
	{
		HttpClient httpClient = new HttpClient();

		[Fact]
		public async Task Get_distance_between_two_cities()
		{
			// Given
			GoogleDistanceAPISearchFilter searchFilter = GoogleDistanceAPISearchFilterBuilder();

			// When
			var response = await httpClient.GetAsync(Get.GetDistance(searchFilter));

			// Then
			response.EnsureSuccessStatusCode();
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			string response_content = response.Content.ReadAsStringAsync().Result;
			Assert.Contains(searchFilter.Origins, response_content);
			Assert.Contains(searchFilter.Destinations, response_content);

			var _object = JsonConvert.DeserializeObject(response_content);
			
		}

		[Fact]
		public async Task Get_distance_Not_Found()
		{
			// Given
			GoogleDistanceAPISearchFilter searchFilter = GoogleDistanceAPISearchFilterBuilder();
			searchFilter.Origins = "xxxxxxxxxx";
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
			GoogleDistanceAPISearchFilter searchFilter = new GoogleDistanceAPISearchFilter();
			string json_string = JsonConvert.SerializeObject(searchFilter);
			var content = new StringContent(json_string, Encoding.UTF8, "application/json");

			// When
			var response = await httpClient.PostAsync(Post.PostDistance(), content);

			// Then
			response.EnsureSuccessStatusCode();
			string response_content = response.Content.ReadAsStringAsync().Result;
			Assert.Contains("INVALID_REQUEST", response_content);
		}

	}

}

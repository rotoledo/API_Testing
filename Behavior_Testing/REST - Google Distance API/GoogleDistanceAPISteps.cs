using System.Net;
using System.Net.Http;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using static API_Testing.Google_Distance_API.GoogleDistanceAPIScenarioBase;
using System;

namespace Behavior_Testing.REST___Google_Distance_API
{
	[Binding]
    public class GoogleDistanceAPISteps
    {
		HttpResponseMessage response;
		string response_content;
		JObject jObject;
		HttpClient httpClient;
		GoogleDistanceAPISearchFilter searchFilter;

		public GoogleDistanceAPISteps()
		{
			response = new HttpResponseMessage();
			response_content = "";
			jObject = new JObject();
			httpClient = new HttpClient();
			searchFilter = new GoogleDistanceAPISearchFilter();
		}


		[Given(@"I have inserted (.*) into the attribute Origins")]
		public void GivenIHaveInsertedIntoTheAttributeOrigins(string origins)
		{
			searchFilter.Origins = origins;
		}

		[Given(@"I have inserted (.*) into the attribute Destination")]
        public void GivenIHaveInsertedIntoTheAttributeDestination(string destinations)
        {
			searchFilter.Destinations = destinations;
        }

		[Given(@"I have inserted (.*) into the attribute Units")]
		public void GivenIHaveInsertedIntoTheAttributeUnits(string units)
		{
			searchFilter.Units = units;
		}

		[When(@"I send a HTTP Get Request")]
		public async System.Threading.Tasks.Task WhenISendAHTTPGetRequest()
		{
			response = await httpClient.GetAsync(Get.GetDistance(searchFilter));
			response_content = response.Content.ReadAsStringAsync().Result;
			jObject = JObject.Parse(response_content);
		}

		[Then(@"the response should be successful")]
        public void ThenTheResponseShouldBeSuccessful()
        {
			response.EnsureSuccessStatusCode();
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			Assert.AreEqual("OK", jObject.GetValue("status").ToString());
		}

		[Then(@"the response content should contain (.*) and (.*)")]
		public void ThenTheResponseContentShouldContainAnd(string origins, string destinations)
		{
			Assert.True(jObject.GetValue("origin_addresses").ToString().Contains(origins));
			Assert.True(jObject.GetValue("destination_addresses").ToString().Contains(destinations));
		}

		[Then(@"the response should inform the estimated distance")]
        public void ThenTheResponseShouldInformTheEstimatedDistance()
        {
			Assert.IsNotNull(jObject.SelectToken("rows[*].elements[*].distance.text"));
			Assert.IsNotEmpty(jObject.SelectToken("rows[*].elements[*].distance.text").ToString());
		}
        
        [Then(@"the response should inform the estimated time travel")]
        public void ThenTheResponseShouldInformTheEstimatedTimeTravel()
        {
			Assert.IsNotNull(jObject.SelectToken("rows[*].elements[*].duration.text"));
			Assert.IsNotEmpty(jObject.SelectToken("rows[*].elements[*].duration.text").ToString());
        }
    }
}

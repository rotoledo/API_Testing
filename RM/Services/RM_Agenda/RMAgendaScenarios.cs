using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Xunit;

namespace RM.RMAgenda
{
	public class RMAgendaScenarios : RMAgendaScenarioBase
	{
		HttpClient httpClient = new HttpClient();
		XmlDocument xmlDocument = new XmlDocument();

		//[Fact]
		public async Task Get_Agenda()
		{
			// When
			RMAgendaBody rmAgendaBody = new RMAgendaBody("AGENDA_VU", "o", "");
			var service = new RMAgendaService(rmAgendaBody);

			// When
			var response = await service.GetAsync();

			// Then
			response.EnsureSuccessStatusCode();
			var response_content = JsonConvert.DeserializeObject<Dictionary<string, object>>(await response.Content.ReadAsStringAsync());
			var response_content_pretty = JsonConvert.DeserializeObject<IEnumerable<Dictionary<string, object>>>(response_content["rows"].ToString());
			Assert.NotEmpty(response_content);
		}

	}
}

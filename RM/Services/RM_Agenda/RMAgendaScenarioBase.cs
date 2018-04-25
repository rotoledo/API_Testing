using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RM.Services;
using RM.SourceData;

namespace RM.RMAgenda
{

	public class RMAgendaScenarioBase : ScenarioBase
	{

		public class RMAgendaService : RMAgendaScenarioBase
		{

			private HttpClientSourceData _client;
			private string _apiReadViewAdressURI;
			private Dictionary<string, string> _form;
			private string token;
			ConfigurationBuilder ConfigurationBuilder;
			IConfigurationRoot ConfigurationRoot;


			public RMAgendaService(RMAgendaBody rmAgendaBody)
			{
				ConfigurationBuilder = new ConfigurationBuilder();
				ConfigurationBuilder.AddInMemoryCollection(RM_Access);
				ConfigurationRoot = ConfigurationBuilder.Build();

				token = "";
				_apiReadViewAdressURI = $"{ ConfigurationRoot["RM_Access"]}{""}";
				_client = new HttpClientSourceData();
				_form = new Dictionary<string, string>()
				{
					{ "codSentenca", rmAgendaBody.CodSentenca },
					{ "codAplicacao", rmAgendaBody.CodAplicacao },
					{ "parameters",  rmAgendaBody.Parameters }
				};
			}

			public Task<HttpResponseMessage> GetAsync()
			{
				return _client.PostXFormAsync(_apiReadViewAdressURI, _form);
			}
		}


		public class RMAgendaBody : RMAgendaScenarioBase
		{
			public RMAgendaBody(string codSentenca, string codAplicacao, string parameters)
			{
				CodSentenca = codSentenca;
				CodAplicacao = codAplicacao;
				Parameters = parameters;
			}

			public string CodSentenca { get; set; }
			public string CodAplicacao { get; set; }
			public string Parameters { get; set; }
		}

		public class Post
		{

			public static string PostEnvelope()
			{
				return @"http://poavudev01/TOTVSBusinessConnect/wsDataServer.asmx";
			}

			public static string EnvelopeBuilder()
			{
				return $@"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:br=""http://www.totvs.com.br/br/"">
							<soap:Header/>
								<soap:Body>
								<br:ReadRecordAuth>
									<br:DataServerName>SauPacienteData</br:DataServerName>
									<br:PrimaryKey>2;997852</br:PrimaryKey>
									<br:Contexto>CODCOLIGADA=2;CODSISTEMA=O;CODUSUARIO=mestre</br:Contexto>
									<br:Usuario>mestre</br:Usuario>
									<br:Senha>totvs</br:Senha>
								</br:ReadRecordAuth >
							</soap:Body>
						</soap:Envelope>";
			}
		}
	}


}

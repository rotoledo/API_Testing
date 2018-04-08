using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace API_Testing.SOAP___Calculator_API
{
	public class CalculatorAPIScenarioBase
	{
		private const string ApiUrlBase = "http://www.dneonline.com/calculator.asmx";

		public class Post
		{
			public static string PostEnvelope()
			{
				return $"{ApiUrlBase}";
			}
		}

		public string EnvelopeOperationBuilder(string function, int intA, int intB)
		{
			return
				$@"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:tem=""http://tempuri.org/"">
						<soap:Header/>
						<soap:Body>
							<tem:{function}>
								<tem:intA>{intA}</tem:intA>
								<tem:intB>{intB}</tem:intB>
							</tem:{function}>
						</soap:Body>
				</soap:Envelope>";
		}

	}

	[XmlRoot]
	public class Restult
	{
		[XmlElement("AddResult")]
		public string AddResult { get; set; }
	}
}

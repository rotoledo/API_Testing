using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Services
{
	public class ScenarioBase
	{

		public const string ApiUrlBase = "hostname";
		public Dictionary<string, string> RM_Access = new Dictionary<string, string>()
		{
			{ "RM_Access", $"http://{ApiUrlBase}/123456789BusinessConnect/wsFormDinamico.asmx/GetJSON" }
		};
		public const string User = "user";
		public const string Pword = "password";

	}
}

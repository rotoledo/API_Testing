using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Services
{
	public class ScenarioBase
	{

		public const string ApiUrlBase = "http://poavudev01";
		public Dictionary<string, string> RM_Access = new Dictionary<string, string>()
		{
			{ "RM_Access", $"{ApiUrlBase}/TOTVSBusinessConnect/wsFormDinamico.asmx/GetJSON" }
		};
		public const string User = "totvs";
		public const string Pword = "mestre";

	}
}

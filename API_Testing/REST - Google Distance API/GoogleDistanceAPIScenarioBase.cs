using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Testing.Google_Distance_API
{
	public class GoogleDistanceAPIScenarioBase
	{
		private const string ApiUrlBase = "https://maps.googleapis.com/maps/api/distancematrix/json";

		public class Get
		{
			public static string GetDistance(GoogleDistanceAPISearchFilter googleDistanceAPISearchFilter)
			{
				return $"{ApiUrlBase}?" + //key={googleDistanceAPISearchFilter.Key}&" +
					$"units={googleDistanceAPISearchFilter.Units}&" +
					$"origins={googleDistanceAPISearchFilter.Origins}&" +
					$"destinations={googleDistanceAPISearchFilter.Destinations}";
			}
		}

		public class Post
		{
			public static string PostDistance()
			{
				return $"{ApiUrlBase}";
			}
		}

		public GoogleDistanceAPISearchFilter GoogleDistanceAPISearchFilterBuilder()
		{
			return new GoogleDistanceAPISearchFilter()
			{
				//Key = "", // AIzaSyB41pvfD0OSloRW1fDoRQIgurftR4yQL30
				Origins = "Porto Alegre",
				Destinations = "São Paulo",
				Units = "metric"
			};
		}

	}


	public class GoogleDistanceAPISearchFilter
	{
		//public string Key { get; set; }
		public string Units { get; set; }  // imperial, metric
		public string Origins { get; set; }
		public string Destinations { get; set; }
	}
}
